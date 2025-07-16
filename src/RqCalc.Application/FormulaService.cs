using System.Linq.Expressions;

using Framework.Core;
using Framework.DataBase;
using Framework.ExpressionParsers;

using RqCalc.Application.Calculation;
using RqCalc.Domain;
using RqCalc.Domain._Base;
using RqCalc.Domain.Formula;
using RqCalc.Model;

namespace RqCalc.Application;

public class FormulaService : IFormulaService
{
    private readonly IDataSource<IPersistentDomainObjectBase> dataSource;
    private readonly IVersion lastVersion;
    private readonly INativeExpressionParser nativeParser;
    private readonly ApplicationSettings settings;

    private readonly IReadOnlyDictionary<IFormula, Func<ICharacterCalculationChangedState, decimal>> formulas;

    public FormulaService(
        IDataSource<IPersistentDomainObjectBase> dataSource,
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

    public Func<ICharacterCalculationChangedState, decimal> GetFunc(IFormula formula) => this.formulas[formula];

    private Dictionary<IFormula, Func<ICharacterCalculationChangedState, decimal>> GetParsedFormulas() =>
        this.dataSource.GetFullList<IFormula>().Where(formula => formula.Enabled).ToDictionary(
            formula => formula,
            formula =>
            {
                var expr = this.nativeParser.Parse(new NativeExpressionParsingData(new MethodTypeInfo(formula.Variables.Select(ParseVariableType), typeof(decimal)),
                                                                                   formula.Value));

                var stateParam = Expression.Parameter(typeof(ICharacterCalculationChangedState));

                var lambda = Expression.Lambda<Func<ICharacterCalculationChangedState, decimal>>(

                    Expression.Invoke(expr,

                                      formula.Variables.Select(var =>
                                                               {
                                                                   var varExpr = this.GetCompileSourceFormulaArgExpression(var);

                                                                   return (varExpr as LambdaExpression).Maybe(varLambda => varLambda.GetBodyWithOverrideParameters(stateParam)) ?? varExpr;
                                                               })), stateParam);

                return lambda.Compile();
            });

    private static Type ParseVariableType(IFormulaVariable variable)
    {
        switch (variable.Type)
        {
            case FormulaVariableType.Decimal:

            case FormulaVariableType.Stat:
            case FormulaVariableType.StatDescription:
            case FormulaVariableType.HpPerVitality:

            case FormulaVariableType.LevelDef:
            case FormulaVariableType.LevelAttack:
                return typeof(decimal);

            case FormulaVariableType.Int32:
            case FormulaVariableType.Level:
            case FormulaVariableType.MaxLevel:
            case FormulaVariableType.LevelMultiplicity:
                return typeof(int);

            case FormulaVariableType.CurrentWeaponInfo:
                return typeof(WeaponInfo);

            default:
                throw new ArgumentOutOfRangeException(nameof(variable));

        }
    }

    private Expression GetCompileSourceFormulaArgExpression(IFormulaVariable variable)
    {
        if (variable == null) throw new ArgumentNullException(nameof(variable));

        switch (variable.Type)
        {
            case FormulaVariableType.Decimal:
                return ExpressionHelper.Create((ICharacterCalculationChangedState state) => state.CustomVariables[variable.Index]);

            case FormulaVariableType.Int32:
                return ExpressionHelper.Create((ICharacterCalculationChangedState state) => (int)state.CustomVariables[variable.Index]);

            case FormulaVariableType.Level:
                return ExpressionHelper.Create((ICharacterCalculationChangedState state) => state.Level);

            case FormulaVariableType.MaxLevel:
                return Expression.Constant(this.lastVersion.MaxLevel);

            case FormulaVariableType.Stat:
            {
                var stat = variable.TypeStat.FromMaybe(() => "Null stat");

                return ExpressionHelper.Create((ICharacterCalculationChangedState state) => state.Stats[stat]);
            }

            case FormulaVariableType.StatDescription:
            {
                var stat = variable.TypeStat.FromMaybe(() => "Null stat");

                var descFormula = stat.DescriptionFormula.FromMaybe(() => "Null Desc Formula");

                var descDel = LazyHelper.Create(() => this.formulas[descFormula]);

                return ExpressionHelper.Create((ICharacterCalculationChangedState state) => descDel.Value(state.ChangeVariable(state.Stats[stat])));
            }

            case FormulaVariableType.CurrentWeaponInfo:
                return ExpressionHelper.Create((ICharacterCalculationChangedState state) => state.CurrentWeaponInfo);

            case FormulaVariableType.LevelDef:
                return ExpressionHelper.Create((ICharacterCalculationChangedState state) => MathHelper.ArmorByLevel(state.Level, this.settings.QualityMaxLevel));

            case FormulaVariableType.LevelAttack:
                return ExpressionHelper.Create((ICharacterCalculationChangedState state) => MathHelper.AttackByLevel(state.Level));

            case FormulaVariableType.LevelMultiplicity:
                return Expression.Constant(this.settings.LevelMultiplicity);

            case FormulaVariableType.HpPerVitality:
                return ExpressionHelper.Create((ICharacterCalculationChangedState state) => state.Class.HpPerVitality);

            default:
                throw new ArgumentOutOfRangeException(nameof(variable.Type));
        }
    }
}