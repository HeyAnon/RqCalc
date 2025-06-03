using System;
using System.Collections.Generic;


using Framework.Reactive;

using Anon.RQ_Calc.Domain;
using Anon.RQ_Calc.Logic;

namespace Anon.RQ_Calc.WPF
{
    public class TextTemplateModel : ContextModel
    {
        public TextTemplateModel(IApplicationContext context, IReadOnlyDictionary<TextTemplateVariableType, decimal> evaluateStats, ITextTemplate textTemplate)
            : base(context)
        {
            if (evaluateStats == null) throw new ArgumentNullException(nameof(evaluateStats));
            if (textTemplate == null) throw new ArgumentNullException(nameof(textTemplate));

            this.Header = textTemplate.Header;
            this.Message = textTemplate.EvaluateMessage(evaluateStats);
        }

        public string Header
        {
            get { return this.GetValue(v => v.Header); }
            private set { this.SetValue(v => v.Header, value); }
        }

        public string Message
        {
            get { return this.GetValue(v => v.Message); }
            private set { this.SetValue(v => v.Message, value); }
        }
    }
}