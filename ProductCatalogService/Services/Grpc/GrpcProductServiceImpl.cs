
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using GrpcProductService;
using MongoDB.Driver;
using static GrpcProductService.GrpcProductService;

namespace ProductCatalogService.Services.Grpc;

public class GrpcProductServiceImpl(IProductService productService) : GrpcProductServiceBase
{
    public override async Task<ProductResponse> GetProduct(ProductRequest request, ServerCallContext context)
    {
        var productIds = request.ProductIds.ToList();
        var products = await productService.GetByIdsAsync(productIds);
        var productResponse = new ProductResponse();
        productResponse.Products.AddRange(products.Select(x => new ProductModel()
        {
            Id = x.Id,
            Name = x.Name,
            Price = x.Price.ToString()
        }));
        return productResponse;
    }
}
