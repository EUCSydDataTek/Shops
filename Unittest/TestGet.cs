namespace Unittest;
using Moq;
using ServiceLayer;
using WebApp.Pages.Shops;

public class TestGet
{

    [Fact]
    public void TestName()
    {

        int ExpectedAmount = 10;

        // Arrange
        var mock = new Mock<IShopService>();

        mock.Setup(s => s.GetShopsByName(null,1,10))
            .Returns(new ShopViewModel() { Shops = Testhelpers.CreateTestData(ExpectedAmount), TotalCount = 1 } );
                        
        ListModel listModel = new ListModel(mock.Object); // Setup "dependency injection"


        // Act
        listModel.OnGet(); // run GET
    
        // Assert count
        Assert.Equal(ExpectedAmount,listModel.Shops.Count());
    }

}
