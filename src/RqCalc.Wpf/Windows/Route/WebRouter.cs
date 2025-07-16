using RqCalc.Wpf.Models;

namespace RqCalc.Wpf.Windows.Route;

public class WebRouter(WpfApplicationWebRouteSettings settings) : ICodeRouter
{
    public void RouteMain(string code) => System.Diagnostics.Process.Start($"{settings.WebSite}{settings.MainPage}?code={code}");

    public void RouteTalent(string code) => System.Diagnostics.Process.Start($"{settings.WebSite}{settings.TalentPage}?code={code}");

    public void RouteGuildTalent(string code) => System.Diagnostics.Process.Start($"{settings.WebSite}{settings.GuildTalentPage}?code={code}");
}
