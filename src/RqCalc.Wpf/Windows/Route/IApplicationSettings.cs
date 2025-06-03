namespace Anon.RQ_Calc.WPF
{
    public interface IApplicationSettings
    {
        string WebSite { get; }
        
        string MainPage { get; }
        
        string TalentPage { get; }

        string GuildTalentPage { get; }
    }
}