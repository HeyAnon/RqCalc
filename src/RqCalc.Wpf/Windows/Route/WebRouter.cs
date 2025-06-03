using System;

namespace Anon.RQ_Calc.WPF
{
    public class WebRouter : ICodeRouter
    {
        private readonly IApplicationSettings _settings;

        public WebRouter(IApplicationSettings settings)
        {
            this._settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }



        public void RouteMain(string code)
        {
            System.Diagnostics.Process.Start($"{this._settings.WebSite}{this._settings.MainPage}?code={code}");
        }

        public void RouteTalent(string code)
        {
            System.Diagnostics.Process.Start($"{this._settings.WebSite}{this._settings.TalentPage}?code={code}");
        }

        public void RouteGuildTalent(string code)
        {
            System.Diagnostics.Process.Start($"{this._settings.WebSite}{this._settings.GuildTalentPage}?code={code}");
        }
    }
}