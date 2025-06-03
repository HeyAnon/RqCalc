namespace RqCalc.Tests;

[TestClass]
public class ImageSourceTests
{
    [TestMethod]
    public void TestPreLoadedImageSourceService()
    {
        using (var connection = new SQLiteConnection(Properties.Settings.Default.ConnectionString))
        using (var dataSource = new RQDBContext(connection, true, ImplementTypeResolver.Default.WithCache()))
        {
            var imageSourceCache = new PreLoadedImageSourceService(dataSource);

            var imageSource = imageSourceCache.GetImageSource("Class");

            var image = imageSource.GetById(1);

            return;
        }
    }

    [TestMethod]
    public void TestUniCards()
    {
        using (var connection = new SQLiteConnection(Properties.Settings.Default.ConnectionString))
        using (var dataSource = new RQDBContext(connection, true, ImplementTypeResolver.Default.WithCache()))
        {
            var r = dataSource.Cards.Where(c => !c.EquipmentSlots.Any()).ToList();

            return;
        }
    }
}