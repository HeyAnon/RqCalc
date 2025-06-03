using System.Collections.Generic;

namespace Anon.RQ_Calc.WPF
{
    internal static class StatConst
    {
        public const string PrimaryStatName = "%primaryStat%";

        public const string EnergyStatName = "%energyStat%";

        public const string RestoreEnergyStatName = "%restoreEnergyStat%";

        public const string RestoreEnergyPerHitStatName = "%restoreEnergyPerHitStat%";

        public const string RestoreEnergyPerKillStatName = "%restoreEnergyPerKillStat%";


        public static readonly IReadOnlyList<string> SpecialStats = new[] {PrimaryStatName, EnergyStatName, RestoreEnergyStatName, RestoreEnergyPerHitStatName, RestoreEnergyPerKillStatName};
    }
}