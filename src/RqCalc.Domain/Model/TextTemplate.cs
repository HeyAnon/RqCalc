using RqCalc.Domain.Persistent._Base._Blocks;

namespace RqCalc.Domain.Model;

public class TextTemplate : ITextTemplate
{
    public ITextTemplateVariableBase[] Variables { get; set; }


    public string Header { get; set; }

    public string Message { get; set; }



    IReadOnlyList<ITextTemplateVariableBase> ITextTemplate.Variables => this.Variables;
}