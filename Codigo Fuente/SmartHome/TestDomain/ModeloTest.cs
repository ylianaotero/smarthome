using Domain.Concrete;

namespace TestDomain;

[TestClass]

public class ModeloTest
{
    private Modelo _modelo;

    [TestInitialize]
    public void TestInitialize()
    {
        _modelo = new Modelo();
    }
    
    [TestMethod]
    public void TestSetModelo()
    {
        _modelo.set_Value("abcdef");
        
        Assert.AreEqual(_modelo.get_Value(), "abcdef");
    }

    [TestMethod]
    public void TestGetModelo()
    {
        _modelo.set_Value("abcdef");
        
        Assert.AreEqual(_modelo.get_Value(), "abcdef");
    }
}