using Framework.Reactive;
using RqCalc.Domain;
using RqCalc.Model;
using RqCalc.Model._Extensions;

namespace RqCalc.Wpf.Models;

public class TextTemplateModel : NotifyModelBase
{
    public TextTemplateModel(IReadOnlyDictionary<TextTemplateVariableType, decimal> evaluateStats, ITextTemplate textTemplate)

    {
        if (evaluateStats == null) throw new ArgumentNullException(nameof(evaluateStats));
        if (textTemplate == null) throw new ArgumentNullException(nameof(textTemplate));

        this.Header = textTemplate.Header;
        this.Message = textTemplate.EvaluateMessage(evaluateStats);
    }

    public string Header
    {
        get => this.GetValue(v => v.Header);
        private set => this.SetValue(v => v.Header, value);
    }

    public string Message
    {
        get => this.GetValue(v => v.Message);
        private set => this.SetValue(v => v.Message, value);
    }
}