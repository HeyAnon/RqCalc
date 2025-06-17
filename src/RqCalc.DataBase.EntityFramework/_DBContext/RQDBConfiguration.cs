namespace RqCalc.DataBase.EntityFramework._DBContext
{
    public class RqdbConfiguration : DbConfiguration
    {
        public RqdbConfiguration()
        {
            this.SetProviderFactory("System.Data.SQLite", SQLiteFactory.Instance);
            this.SetProviderFactory("System.Data.SQLite.EF6", SQLiteProviderFactory.Instance);
            this.SetProviderServices("System.Data.SQLite", (DbProviderServices)SQLiteProviderFactory.Instance.GetService(typeof(DbProviderServices)));
        }
    }
}