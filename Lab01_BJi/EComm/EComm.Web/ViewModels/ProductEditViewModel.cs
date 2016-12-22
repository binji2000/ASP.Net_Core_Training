using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EComm.Data;
using Microsoft.EntityFrameworkCore;

namespace EComm.Web.ViewModels
{
    public class ProductEditViewModel
    {
        public Product Product { get; set; }

        public DbSet<Supplier> Suppliers { get; set; }
    }
}
