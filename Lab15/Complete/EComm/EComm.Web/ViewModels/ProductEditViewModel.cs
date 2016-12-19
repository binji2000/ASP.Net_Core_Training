using EComm.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EComm.Web.ViewModels
{
    public class ProductEditViewModel
    {
        public Product Product { get; set; }
        public List<Supplier> Suppliers { get; set; }

        public IEnumerable<SelectListItem> SupplierChoices
        {
            get
            {
                if (Suppliers == null) return new List<SelectListItem>();
                return from s in Suppliers
                       select new SelectListItem { Value = s.Id.ToString(), Text = s.CompanyName };
            }
        }
    }
}
