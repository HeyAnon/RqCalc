namespace RqCalc.Wpf.Models;

public record WpfApplicationWebRouteSettings
{
    public string WebSite { get; set; } = null!;

    public string MainPage { get; set; } = null!;

    public string TalentPage { get; set; } = null!;

    public string GuildTalentPage { get; set; } = null!;
}
