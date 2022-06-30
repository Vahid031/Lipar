using System.Threading.Tasks;
using Xunit;

namespace Lipar.Presentation.Api.IntegrationTest.Products
{
    public class ProductControllerTest : IClassFixture<ApiTestFactory<TestStarup>>
    {
        private readonly ApiTestFactory<TestStarup> factory;

        public ProductControllerTest(ApiTestFactory<TestStarup> factory)
        {
            this.factory = factory;
        }

        [Fact]
        public void Tesssssst()
        {
            //Arrange
            //using var client = factory.CreateClient();


            //Act
            //using var response = await client.GetAsync(ProductApiRoutes.Get);


            //Assert
            Assert.True(true);
        }
    }
}
