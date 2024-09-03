using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using StockManagementAPI.API.Models;
using StockManagementAPI.Core.Entities;
using StockManagementAPI.Core.Interfaces;
using StockManagementAPI.Infrastructure.Data;

namespace StockManagementAPI.Core.Services {
    public class ProductService : IProductService {
        private readonly StockManagementDbContext _context;
        private readonly IEmailService _emailService;
        private readonly IMemoryCache _cache;
        private readonly int _cacheDuration;
        public ProductService(StockManagementDbContext context, IEmailService emailService, IMemoryCache cache, IOptions<CacheSettings> cacheSettings) {
            _cache = cache;
            _context = context;
            _emailService = emailService;
            _cacheDuration = cacheSettings.Value.DefaultCacheDuration;
        }

        public async Task<ProductResponse> CreateProductAsync(ProductRequest request) {
            var product = new Product
            {
                Name = request.Name,
                StockQuantity = request.StockQuantity,
                Price = request.Price,
                PriceHistories = new List<PriceHistory>()
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return new ProductResponse {
                Id = product.Id,
                Name = product.Name,
                StockQuantity = product.StockQuantity,
                Price = product.Price
            };
        }

        public async Task<UpdatePriceResponse?> UpdateProductPriceAsync(int id, UpdatePriceRequest request) {
            var product = await _context.Products.Include(p => p.PriceHistories).FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
                return null;

            var priceHistory = new PriceHistory
            {
                ProductId = id,
                OldPrice = product.Price,
                NewPrice = request.NewPrice,
                ChangeDate = DateTime.UtcNow.AddMinutes(5) // TO DO Bu süre configden okunabilir.
            };

            product.PriceHistories.Add(priceHistory);
            await Task.Delay(TimeSpan.FromMinutes(5));
            product.Price = request.NewPrice;

            await _context.SaveChangesAsync();
            await _emailService.SendEmailAsync("diazinfo@example.com", "Price Update", $"The price for {product.Name} has been updated to {product.Price}");

            return new UpdatePriceResponse {
                ProductId = product.Id,
                NewPrice = product.Price,
                ScheduledUpdate = priceHistory.ChangeDate
            };
        }

        public async Task<IEnumerable<ProductResponse>?> GetProductsAsync(decimal? minPrice, decimal? maxPrice, int? minStock) {
            var cacheKey = $"products-{minPrice}-{maxPrice}-{minStock}";

            if (!_cache.TryGetValue(cacheKey, out List<ProductResponse>? cachedProducts)) {
                var query = _context.Products.AsQueryable();

                if (minPrice.HasValue)
                    query = query.Where(p => p.Price >= minPrice.Value);

                if (maxPrice.HasValue)
                    query = query.Where(p => p.Price <= maxPrice.Value);

                if (minStock.HasValue)
                    query = query.Where(p => p.StockQuantity >= minStock.Value);

                var products = await query.Include(p => p.PriceHistories).ToListAsync();

                cachedProducts = products.Select(p => new ProductResponse {
                    Id = p.Id,
                    Name = p.Name,
                    StockQuantity = p.StockQuantity,
                    Price = p.Price,
                    PriceHistories = p.PriceHistories.Select(ph => new PriceHistoryResponse {
                        OldPrice = ph.OldPrice,
                        NewPrice = ph.NewPrice,
                        ChangeDate = ph.ChangeDate
                    }).ToList()
                }).ToList();

                _cache.Set(cacheKey, cachedProducts, TimeSpan.FromSeconds(_cacheDuration));
            }

            return cachedProducts;
        }

        public async Task<ProductResponse> GetProductByIdAsync(int id) {
            var product = await _context.Products.Include(p => p.PriceHistories).FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
                return null;

            return new ProductResponse {
                Id = product.Id,
                Name = product.Name,
                StockQuantity = product.StockQuantity,
                Price = product.Price,
                PriceHistories = product.PriceHistories.Select(ph => new PriceHistoryResponse {
                    OldPrice = ph.OldPrice,
                    NewPrice = ph.NewPrice,
                    ChangeDate = ph.ChangeDate
                }).ToList()
            };
        }

    }
}
