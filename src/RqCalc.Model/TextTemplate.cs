using RqCalc.Domain._Base;

namespace RqCalc.Model;

public class TextTemplate : ITextTemplate
{
    public List<ITextTemplateVariableBase> Variables { get; set; } = [];


    public string Header { get; set; }

    public string Message { get; set; }



    IReadOnlyList<ITextTemplateVariableBase> ITextTemplate.Variables => this.Variables;
}