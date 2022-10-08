using ProiectDAW.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProiectDAW2.Models
{   // tabel asociativ intre cosul de cumparaturi si produs
    // a.i. un produs poate fi adaugat in mai multe cosuri de cumparaturi
    // si un cos de cumparaturi contine mai multe produse adaugate
    public class Adding
    {
        [Key]
        public int AddingId { get; set; }

        [Required(ErrorMessage = "Produsul este obligatoriu!")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Cosul de cumparaturi este obligatoriu!")]
        public int ShoppingCartId { get; set; }

        [Required(ErrorMessage = "Cantitatea este obligatorie!")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Cantitatea este obligatorie!")]
        public float TotalPricePerProduct{ get; set; }

        public virtual Product Product { get; set; }
        public virtual ShoppingCart ShoppingCart { get; set; }
    }
}