namespace Anon.RQ_Calc.WPF
{
    public interface ICodeRouter
    {
        void RouteMain(string code);

        void RouteTalent(string code);

        void RouteGuildTalent(string code);
    }
}