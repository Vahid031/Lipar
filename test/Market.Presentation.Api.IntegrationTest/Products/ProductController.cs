using Market.Core.Application.Products.Commands;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Market.Presentation.Api.IntegrationTest.Products
{
    public class ProductController : IClassFixture<ApplicationFactory>
    {
        private readonly ApplicationFactory factory;

        public ProductController()
        {
            factory = new ApplicationFactory();
        }

        [Fact]
        public async Task Get_Product_List_Must_Be_Done()
        {
            //Arrange
            using var client = factory.CreateClient();

            //Act
            using var response = await client.GetAsync("api/Product/get");

            //Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Theory]
        [InlineData("00001", "Book00001")]
        [InlineData("00002", "Book00002")]
        [InlineData("00003", "Book00003")]
        [InlineData("00004", "Book00004")]
        [InlineData("00005", "Book00005")]
        [InlineData("00006", "Book00006")]
        [InlineData("00007", "Book00007")]
        public async Task Create_Product_Must_Be_Done(string barcode, string name)
        {
            //Arrange
            using var client = factory.CreateClient();

            //Act
            using var response = await client.PostAsync("api/Product/create", new CreateProductCommand
            {
                Barcode = barcode,
                Name = name
            }.ToStringContent());


            //Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

    }
}
