namespace RqCalc.Tests;

[TestClass]
public class FacadeTests
{
    [TestMethod]
    public void TestLoadImage()
    {
        var facade = new JsonFacade(ServiceFacadeEnvironment.Configuration);

        var genderImage = facade.GetImage("Gender", 1);

        var classImage = facade.GetImage("Class", 1);

        var costumeImage = facade.GetImage("Costume", 472);

        return;
    }

    [TestMethod]
    public void TestLoadStartupComplexData()
    {
        var facade = new JsonFacade(ServiceFacadeEnvironment.Configuration);

        var data = facade.GetCharacterStartupComplexData();

        return;
    }
}