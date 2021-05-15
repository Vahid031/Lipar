using Lipar.Core.Application.Common;

namespace Market.Core.Application.Products.Commands.CreateProduct
{
    public class CreateProductCommand : IRequest
    {
        public string Name { get; set; }
        public string Barcode { get; set; }

    }
}
