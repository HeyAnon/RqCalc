using Framework.Core;
using Framework.DataBase;
using Framework.ExpressionParsers;

using RqCalc.Application.Calc;
using RqCalc.Application.Settings;
using RqCalc.Domain;
using RqCalc.Domain._Base;
using RqCalc.Domain.Formula;
using RqCalc.Model;

using System.Linq.Expressions;

namespace RqCalc.Application;

public class FormulaService : IFormulaService
{
    private readonly IDataSource<IPersistentDomainObjectBase> dataSource;
    private readonly IVersion lastVersion;
    private readonly INativeExpressionParser nativeParser;
    private readonly ApplicationSettings settings;

    private readonly IReadOnlyDictionary<IFormula, Func<ICalcState, decimal>> formulas;

    public FormulaService(IDataSource<IPersistentDomainObjectBase> dataSource,
        IVersion lastVersion,
        INativeExpressionParser nativeParser,
        ApplicationSettings settings)
    {
        this.dataSource = dataSource;
        this.lastVersion = lastVersion;
        this.nativeParser = nativeParser;
        this.settings = settings;

        this.formulas = this.GetParsedFormulas();
    }

    public Func<ICalcState, decimal> GetFunc(IFormula formula)
    {
        return this.formulas[formula];
    }

    private Dictionary<IFormula, Func<ICalcState, decimal>> GetParsedFormulas()
    {
        return this.dataSource.GetFullList<IFormula>().Where(formula => formula.Enabled).ToDictionary(
            formula => formula,
            formula =>
            {
                var expr = nativeParser.Parse(new NativeExpressionParsingData(new MethodTypeInfo(formula.Variables.Select(ParseVariableType), typeof(decimal)), formula.Value));

                var stateParam = Expression.Parameter(typeof(ICalcState));

                var lambda = Expression.Lambda<Func<ICalcState, decimal>>(

                    Expression.Invoke(expr,

                        formula.Variables.Select(var =>
                        {
                            var varExpr = this.GetCompileSourceFormulaArgExpression(var);

                            return (varExpr as LambdaExpression).Maybe(varLambda => varLambda.GetBodyWithOverrideParameters(stateParam)) ?? varExpr;
                        })), stateParam);

                return lambda.Compile();
            });
    }

    private Expression GetCompileSourceFormulaArgExpression(IFormulaVariable variable)
    {
        if (variable == null) throw new ArgumentNullException(nameof(variable));

        switch (variable.Type)
        {
            case FormulaVariableType.Decimal:
                return ExpressionHelper.Create((ICalcState state) => state.CustomVariables[variable.Index]);

            case FormulaVariableType.Int32:
                return ExpressionHelper.Create((ICalcState state) => (int)state.CustomVariables[variable.Index]);

            case FormulaVariableType.Level:
                return ExpressionHelper.Create((ICalcState state) => state.Level);

            case FormulaVariableType.MaxLevel:
                return Expression.Constant(lastVersion.MaxLevel);

            case FormulaVariableType.Stat:
                {
                    var stat = variable.TypeStat.FromMaybe(() => "Null stat");

                    return ExpressionHelper.Create((ICalcState state) => state.Stats[stat]);
                }

            case FormulaVariableType.StatDescription:
                {
                    var stat = variable.TypeStat.FromMaybe(() => "Null stat");

                    var descFormula = stat.DescriptionFormula.FromMaybe(() => "Null Desc Formula");

                    var descDel = LazyHelper.Create(() => this.formulas[descFormula]);

                    return ExpressionHelper.Create((ICalcState state) => descDel.Value(state.ChangeVariable(state.Stats[stat])));
                }

            case FormulaVariableType.CurrentWeaponInfo:
                return ExpressionHelper.Create((ICalcState state) => state.CurrentWeaponInfo);

            case FormulaVariableType.LevelDef:
                return ExpressionHelper.Create((ICalcState state) => MathHelper.ArmorByLevel(state.Level, this.Settings.QualityMaxLevel));

            case FormulaVariableType.LevelAttack:
                return ExpressionHelper.Create((ICalcState state) => MathHelper.AttackByLevel(state.Level));

            case FormulaVariableType.LevelMultiplicity:
                return Expression.Constant(settings.LevelMultiplicity);

            case FormulaVariableType.HpPerVitality:
                return ExpressionHelper.Create((ICalcState state) => state.Class.HpPerVitality);

            default:
                throw new ArgumentOutOfRangeException(nameof(variable.Type));
        }
    }
}