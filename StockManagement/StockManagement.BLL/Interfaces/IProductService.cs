using StockManagement.API.Models;

namespace StockManagement.API.Core.Interfaces {
    public interface IProductService {
        Task<ProductResponse> CreateProductAsync(ProductRequest request);
        Task<UpdatePriceResponse?> UpdateProductPriceAsync(int id, UpdatePriceRequest request);
        Task<IEnumerable<ProductResponse>?> GetProductsAsync(decimal? minPrice, decimal? maxPrice, int? minStock);
        Task<ProductResponse> GetProductByIdAsync(int id);
    }
}
