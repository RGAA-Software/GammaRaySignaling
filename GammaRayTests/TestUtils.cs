using GammaRaySignaling;

namespace GammaRayTests;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestGenClientId()
    {
        Console.WriteLine("ID: " + new ClientIdGenerator().Gen("xxxxxx"));
    }
    
    [Test]
    public void TestRespJsonMessage()
    {
        Console.WriteLine("" + Common.MakeJsonMessage(200, "OKOK", new Dictionary<string, string>
        {
            {"Name", "Jack Sparrow"},
            {"Age", "I don't known"}
        }));
    }
}