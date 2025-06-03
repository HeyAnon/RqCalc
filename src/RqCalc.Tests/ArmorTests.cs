namespace RqCalc.Tests;

[TestClass]
public class ArmorTests
{
    [TestMethod]
    public void TestArmorByLevel()
    {
        using (var connection = new SQLiteConnection("Data Source=..\\..\\..\\..\\_db\\rqdata.sqlite;foreign keys=true;Version=3;Read Only=True"))
        using (var dataSource = new RQDBContext(connection, false, ImplementTypeResolver.Default.WithCache()))
        {
            foreach (var gr in dataSource.ClassLevelHpBonuses.GroupBy(bonus => bonus.Class))
            {
                IClass @class = gr.Key;

                var offset1 = 0;

                var offset2 = 100;// @class.GetRoot().HpPerVitality;

                var coeffs = gr.OrderBy(bonus => bonus.Level)
                    .Select(bonus => offset1 + (bonus.Value + offset2) / MathHelper.GetBaseLevelCoeff(bonus.Level))
                    .ToList();

                var zipRes = coeffs.Windowed2((v1, v2) => v1 - v2).ToList();

                return;
            }
        }
    }

    [TestMethod]
    public void CoeffTest()
    {
        var coeff = MathHelper.GetBaseLevelCoeff(60);
    }
}