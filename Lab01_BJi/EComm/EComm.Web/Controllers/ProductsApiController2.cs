using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EComm.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace EComm.Web.Controllers
{
    [Route("api/v{version:apiVersion}")]
    [ApiVersion("2.0")]
    public class ProductsApiController2 : Controller
    {
        private ECommContext _context;

        public ProductsApiController2(ECommContext context)
        {
            _context = context;
        }

        // GET: api/values
        [HttpGet("products")]
        public IEnumerable<Product> Get()
        {
            return _context.Products;
        }

        // GET: api/values
        [HttpGet("products/{id}")]
        public IActionResult Get(int id)
        {
            var product = _context.Products.Include(p => p.Supplier).SingleOrDefault(p => p.Id == id); ;
            if (product == null)
            {
                return NotFound(null);
            }
            return new ObjectResult(product);
        }


        /* in postman, change http to Post, click the "Body" tab, paste in the json body, set text format to "JSON (application/json)",
         * 
         * {
  "productName": "Bin Ji",
  "unitPrice": 89,
  "package": "24 - 12 oz bottles",
  "isDiscontinued": false,
  "supplierId": 1,
  "formattedUnitPrice": "$89.00"
}.

    After clicking "Send" button, the bottom status should show "201 created", and the body should show the new product detail, and the headers should show: Location →http://localhost:5000/api/products/79 

    */
        // POST api/values
        [HttpPost("product")]
        public IActionResult Post([FromBody]Product product)
        {
            if (product == null)
            {
                return BadRequest();
            }
            _context.Products.Add(product);
            _context.SaveChanges();
            return CreatedAtAction("Get", new {id = product.Id}, product);
        }

        // PUT api/values/5
        [HttpPut("product")]
        public IActionResult Put(int id, [FromBody]Product product)
        {
            if (product == null || product.Id != id)
            {
                return BadRequest();
            }
            if (_context.Products.Count(p => p.Id == id) == 0)
            {
                return NotFound();
            }
            _context.Products.Update(product);
            _context.SaveChanges();
            return new NoContentResult();
        }


        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
