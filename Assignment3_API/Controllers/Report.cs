using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Assignment3_API.ViewModels;
using Assignment3_API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using static Assignment3_API.Controllers.Report;

namespace Assignment3_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Report : ControllerBase
    {

        private readonly AppDbContext _context;
        public Report(AppDbContext appDbContext)
        { 
            _context = appDbContext;
        }


        //Get products
        [HttpGet("ProductListing")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var products = await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.ProductType)
                .Select(p => new
                {
                    p.ProductId,
                    p.Name,
                    p.Image,
                    p.Description,
                    p.Price,
                    BrandName = p.Brand.Name,
                    ProductTypeName = p.ProductType.Name,
                })
                .ToListAsync();

            //Check if there are products in the database
            if(products == null || products.Count == 0) 
            { 
                return NotFound("No products found"); 
            }
            return Ok(products);
        }


        public class ProductDto
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public string Image { get; set; }  // Base64 string or another image format
            public int BrandId { get; set; }
            public int ProductTypeId { get; set; }
            public decimal Price { get; set; }
        }


        //Adding products
        [HttpPost("AddProduct")]
        public async Task<ActionResult<Product>> AddProduct( ProductDto productDto)
        {
            if (productDto == null)
            {
                return BadRequest("Product data is null.");
            }

            //Ensure the product type and brand exist
            var brand = await _context.Brands.FindAsync(productDto.BrandId);
            var productType = await _context.ProductTypes.FindAsync(productDto.ProductTypeId); ;

            if (brand == null || productType == null)
            {
                return BadRequest("Invalid Brand or ProductType.");

            }

        

            // Map DTO to Product entity
            var product = new Product
            {
                Name = productDto.Name,
                Description = productDto.Description,
                BrandId = productDto.BrandId,
                ProductTypeId = productDto.ProductTypeId,
                Price = productDto.Price,
                Image = productDto.Image,  // Handle image conversion if it's in base64 format
            };


            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Product created", product = new { id = product.ProductId, name = product.Name } });


        }




        //Get brands
        [HttpGet("GetBrands")]
        public async Task<ActionResult<IEnumerable<Brand>>> GetBrand()
        {
            var brands = await _context.Brands.ToListAsync();
            if(brands ==  null || brands.Count == 0)
            {
                return NotFound("No brands found.");
            }

            return Ok(brands);
        }

        //Get Product types
        [HttpGet("GetProdutType")]
        public async Task<ActionResult<IEnumerable<ProductType>>> GetProductType()
        {
            var productType = await _context.ProductTypes.ToListAsync();
            if(productType == null || productType.Count == 0)
            {
                return NotFound("No product type found.");
            }

            return Ok(productType);
        }


        //Get Top 10 most expensive products
        [HttpGet("TenMostExpensiveProducts")]
        public async Task<ActionResult<IEnumerable<Product>>> GetTop10ExpesniveProducts()
        {
            var products = await _context.Products
                                         .Include(p => p.Brand)
                                         .Include(p => p.ProductType)
                                         .OrderByDescending(p => p.Price)
                                         .Take(10)
                                         .ToListAsync();

            // Check if there are no products
            if (products == null || products.Count == 0)
            {
                return NotFound("No products found.");
            }


            // Select the relevant fields to return
            var result = products.Select(p => new
            {
                p.Name,
                p.Price,
                Brand = p.Brand.Name,
                ProductType = p.ProductType.Name, 
                p.Description
            }).ToList();

            return Ok(result);


        }



        //Grouped by brand
        [HttpGet("Reports")]
        public async Task<IActionResult> GetProductCountForDashboard()
        {
           //Brand count grouped by name
           var brandcount = await _context.Products.
                                                   Include(p => p.Brand)
                                                   .GroupBy(p => p.Brand.Name)
                                                   .Select(g => new
                                                   {
                                                       Label = g.Key,
                                                       Count = g.Count()
                                                   }).ToListAsync();

            //Product Type count
            var typeCount = await _context.Products.
                                                    Include(p => p.ProductType)
                                                    .GroupBy(p => p.ProductType.Name)
                                                    .Select(g => new
                                                    {
                                                        Label = g.Key,
                                                        Count = g.Count()
                                                    }).ToListAsync();

            return Ok(new
            {
                brandChart = brandcount,
                typeChart = typeCount
            });


        }




    }
}
