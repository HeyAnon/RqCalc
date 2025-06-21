namespace RqCalc.Wpf.Windows.Route;

public class WebRouter(IApplicationSettings settings) : ICodeRouter
{
    private readonly IApplicationSettings settings = settings ?? throw new ArgumentNullException(nameof(settings));


    public void RouteMain(string code)
    {
        System.Diagnostics.Process.Start($"{this.settings.WebSite}{this.settings.MainPage}?code={code}");
    }

    public void RouteTalent(string code)
    {
        System.Diagnostics.Process.Start($"{this.settings.WebSite}{this.settings.TalentPage}?code={code}");
    }

    public void RouteGuildTalent(string code)
    {
        System.Diagnostics.Process.Start($"{this.settings.WebSite}{this.settings.GuildTalentPage}?code={code}");
    }
}