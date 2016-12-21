using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using EComm.Web.Models;

namespace EComm.Web.ViewModels
{
    public class CartViewModel
    {
        public ShoppingCart Cart { get; set; }

        [Required]
        [Display(Name = "Customer Name")]
        public string CustomerName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [CreditCard]
        public string CreditCard { get; set; }
    }
}
