using Microsoft.AspNetCore.Mvc;
using StockManagement.API.Models;
using StockManagement.API.Core.Interfaces;

namespace StockManagement.API.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : Controller {
        private readonly IProductService _productService;

        public ProductController(IProductService productService) {
            _productService = productService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] ProductRequest request) {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _productService.CreateProductAsync(request);
            return CreatedAtAction(nameof(GetProductById), new { id = response.Id }, response);
        }

        [HttpPut("{id}/price")]
        public async Task<IActionResult> UpdateProductPrice(int id, [FromBody] UpdatePriceRequest request) {
            var response = await _productService.UpdateProductPriceAsync(id, request);
            if (response == null)
                return NotFound();

            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts([FromQuery] decimal? minPrice, [FromQuery] decimal? maxPrice, [FromQuery] int? minStock) {
            var response = await _productService.GetProductsAsync(minPrice, maxPrice, minStock);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id) {
            var response = await _productService.GetProductByIdAsync(id);
            if (response == null)
                return NotFound();

            return Ok(response);
        }
    }
}
