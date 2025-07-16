using Framework.Core;
using RqCalc.Domain;
using RqCalc.Domain._Base;
using RqCalc.Domain.GuildTalent;
using RqCalc.Domain.Talent;
using RqCalc.Model.Impl;

namespace RqCalc.Model._Extensions;

public static class TextTemplateExtensions
{
    public static string EvaluateMessage(this ITextTemplate textTemplate, IReadOnlyDictionary<TextTemplateVariableType, decimal> evaluateStats)
    {
        if (evaluateStats == null) throw new ArgumentNullException(nameof(evaluateStats));
        if (textTemplate == null) throw new ArgumentNullException(nameof(textTemplate));
            

        var args = textTemplate.Variables.ToArray(var => 

            (object)Math.Round(var.Value * evaluateStats[var.Type]));

        return string.Format(textTemplate.Message, args);
    }


    public static ITalentDescription GetMainDescription(this ITalent talent)
    {
        if (talent == null) throw new ArgumentNullException(nameof(talent));

        return new TalentDescription
        {
            Body = talent.GetMainTextTemplate(),

            Buffs = talent.BuffDescriptions.OrderBy(buff => buff.OrderIndex).Where(buff => !buff.IsPassive).ToArray(buff => buff.ToTextTemplate())
        };
    }

    public static ITalentDescription GetDescription(this IGuildTalent talent, int points)
    {
        if (talent == null) throw new ArgumentNullException(nameof(talent));

        return new TalentDescription
        {
            Body = talent.GetTextTemplate(points),

            Buffs = []
        };
    }

    public static ITextTemplate GetTextTemplate(this IGuildTalent talent, int points)
    {
        if (talent == null) throw new ArgumentNullException(nameof(talent));

        return new TextTemplate
        {
            Message = talent.GetDescriptionTemplate(),
            Variables = talent
                .Variables
                .Where(var => var.Points == points)
                .Select(var => new TextTemplateVariable { Index = var.Index, Type = TextTemplateVariableType.Const, Value = var.Value })
                .ToList<ITextTemplateVariableBase>()
        };
    }

    public static string GetDescriptionTemplate(this IGuildTalent talent)
    {
        if (talent == null) throw new ArgumentNullException(nameof(talent));

        return talent.Description ?? talent.Bonuses.Single().Type.Template;
    }

    public static ITalentDescription? GetPassiveDescription(this ITalent talent)
    {
        if (talent == null) throw new ArgumentNullException(nameof(talent));

        return talent.GetPassiveTextTemplate().Maybe(body => new TalentDescription
        {
            Body = body,

            Buffs = talent.BuffDescriptions.OrderBy(buff => buff.OrderIndex).Where(buff => buff.IsPassive).ToArray(buff => buff.ToTextTemplate())
        });
    }


    public static ITextTemplate GetMainTextTemplate(this ITalent talent)
    {
        if (talent == null) throw new ArgumentNullException(nameof(talent));

        return new TextTemplate
        {
            Message = talent.Description,
            Variables = talent.Variables.ToList<ITextTemplateVariableBase>()
        };
    }

    public static ITextTemplate? GetPassiveTextTemplate(this ITalent talent)
    {
        if (talent == null) throw new ArgumentNullException(nameof(talent));

        if (string.IsNullOrWhiteSpace(talent.PassiveDescription))
        {
            return null;
        }
        else
        {
            return new TextTemplate
            {
                Message = talent.PassiveDescription,
                Variables = talent.Variables.ToList<ITextTemplateVariableBase>()
            };
        }
    }


    public static ITextTemplate ToTextTemplate(this IBuffDescriptionElement buffElement)
    {
        if (buffElement == null) throw new ArgumentNullException(nameof(buffElement));

        var multiplyCoeff = buffElement.Value.GetValueOrDefault(1);
        var multiplyCoeffStr = multiplyCoeff == 1 ? "" : $" (x{multiplyCoeff})";

        var template = buffElement.Description.ToTextTemplate().Multiply(multiplyCoeff);

        return new TextTemplate
        {
            Header = buffElement.Description.IsStack ? $"{template.Header}{multiplyCoeffStr}" : template.Header,
            Message = template.Message,
            Variables = template.Variables.ToList()
        };
    }


    public static ITextTemplate ToTextTemplate(this IBuffDescription buffDescription)
    {
        if (buffDescription == null) throw new ArgumentNullException(nameof(buffDescription));
            
        return new TextTemplate
        {
            Header = buffDescription.Name,

            Message = buffDescription.Template,

            Variables = buffDescription.Variables.ToDictionary(var => var.Index).Values.ToList<ITextTemplateVariableBase>()
        };
    }

    public static ITextTemplate Multiply(this ITextTemplate template, decimal value)
    {
        if (template == null) throw new ArgumentNullException(nameof(template));

        return new TextTemplate
        {
            Header = template.Header,

            Message = template.Message,

            Variables = template
                .Variables
                .Select(v => new VirtualTextTemplateVariableBase { Type = v.Type, Value = v.Value * value })
                .ToList<ITextTemplateVariableBase>()
        };
    }
}