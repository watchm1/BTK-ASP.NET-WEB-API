using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using ProductApp.Models;

namespace ProductApp.Controllers;

[Route("api/products")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly ILogger<ProductController> _logger;
    public ProductController(ILogger<ProductController> logger)
    {
        this._logger = logger;   
    }
    [HttpGet]
    public IActionResult GetAllProducts()
    {
        var products = new List<Product>()
        {
            new Product
            {
                Id = 1,
                ProductName = "Product1"
            },
            new Product
            {
                Id = 2,
                ProductName = "Product2"
            },
            new Product
            {
                Id = 3,
                ProductName = "Product3"
            }
        };
        this._logger.LogInformation("Get all products has been called");
        return Ok(products);
    }
}
