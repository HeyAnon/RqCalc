namespace RqCalc.Tests;

[TestClass]
public class DbImagesTests
{
    [TestMethod]
    public void ReSaveImages()
    {
        using (var connection = new SQLiteConnection("Data Source=..\\..\\..\\..\\_db\\rqdata.sqlite;foreign keys=true;Version=3"))
        using (var dataSource = new RQDBContext(connection, true, ImplementTypeResolver.Default.WithCache()))
        {
            foreach (var dbImage in dataSource.Images)
            {
                using (var inputStream = new MemoryStream(dbImage.Data))
                using (var inputBitmap = new Bitmap(inputStream))
                using (var outputStream = new MemoryStream())
                using (var outputBitmap = new Bitmap(inputBitmap.Width, inputBitmap.Height))
                using (var graphics = Graphics.FromImage(outputBitmap))
                {
                    graphics.DrawImage(inputBitmap, 0, 0, inputBitmap.Width, inputBitmap.Height);

                    outputBitmap.Save(outputStream, inputBitmap.RawFormat);

                    dbImage.Data = outputStream.ToArray();

                    dataSource.Images.AddOrUpdate(dbImage);
                }
            }

            dataSource.SaveChanges();
        }
    }

    [TestMethod]
    public void ReSaveTalentGrayImages()
    {
        using (var connection = new SQLiteConnection("Data Source=..\\..\\..\\..\\_db\\rqdata.sqlite;foreign keys=true;Version=3"))
        using (var dataSource = new RQDBContext(connection, true, ImplementTypeResolver.Default.WithCache()))
        {
            foreach (var talent in dataSource.Talents)
            {
                using (var inputStream = new MemoryStream(talent.Image.Data))
                using (var inputBitmap = new Bitmap(inputStream))
                using (var outputStream = new MemoryStream())
                using (var outputBitmap = inputBitmap.MakeGrayscale3())                    
                {
                    outputBitmap.Save(outputStream, inputBitmap.RawFormat);

                    talent.GrayImage.Data = outputStream.ToArray();

                    dataSource.Images.AddOrUpdate(talent.GrayImage);
                }
            }

            dataSource.SaveChanges();
        }
    }

    [TestMethod]
    public void ReSaveGuildTalentGrayImages()
    {
        using (var connection = new SQLiteConnection("Data Source=..\\..\\..\\..\\_db\\rqdata.sqlite;foreign keys=true;Version=3"))
        using (var dataSource = new RQDBContext(connection, true, ImplementTypeResolver.Default.WithCache()))
        {
            foreach (var talent in dataSource.GuildTalents)
            {
                using (var inputStream = new MemoryStream(talent.Image.Data))
                using (var inputBitmap = new Bitmap(inputStream))
                using (var outputStream = new MemoryStream())
                using (var outputBitmap = inputBitmap.MakeGrayscale3())
                {
                    outputBitmap.Save(outputStream, inputBitmap.RawFormat);

                    talent.GrayImage.Data = outputStream.ToArray();

                    dataSource.Images.AddOrUpdate(talent.GrayImage);
                }
            }

            dataSource.SaveChanges();
        }
    }
}