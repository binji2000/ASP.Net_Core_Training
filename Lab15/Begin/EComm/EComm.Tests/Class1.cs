using EComm.Data;
using EComm.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace EComm.Tests
{
    public class Class1
    {
        [Fact]
        public void PassingTest()
        {
            Assert.Equal(4, (2 + 2));
        }

        [Fact]
        public void ProductDetails()
        {
            // Arrange
            var controller = new ProductController(CreateStubContext());

            // Act
            var result = controller.Detail(2);

            // Assert
            Assert.IsAssignableFrom<ViewResult>(result);
            var vr = result as ViewResult;
            Assert.IsAssignableFrom<Product>(vr.Model);
            var model = vr.Model as Product;
            Assert.Equal("Bread", model.ProductName);
        }

        private ECommContext CreateStubContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ECommContext>();
            optionsBuilder.UseInMemoryDatabase();
            var context = new ECommContext(optionsBuilder.Options);

            // Add sample data
            context.Products.Add(new Product { Id = 1, ProductName = "Milk", UnitPrice = 2.50M });
            context.Products.Add(new Product { Id = 2, ProductName = "Bread", UnitPrice = 3.25M });
            context.Products.Add(new Product { Id = 3, ProductName = "Juice", UnitPrice = 5.75M });
            context.SaveChanges();

            return context;
        }
    }
}
