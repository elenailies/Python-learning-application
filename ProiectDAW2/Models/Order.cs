using ProiectDAW2.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProiectDAW2.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        //[Required(ErrorMessage = "User-ul este obligatoriu!")]
        //public string UserId { get; set; }

        [Required(ErrorMessage = "Cosul este obligatoriu!")]
        public int ShoppingCartId { get; set; }

        [Required(ErrorMessage = "Numele este obligatoriu!")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Numarul de telefon este obligatoriu!")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Introduceti o adresa email!")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Judetul este obligatoriu!")]
        public string Region { get; set; }
        [Required(ErrorMessage = "Localitatea este obligatorie!")]
        public string City { get; set; }
        [Required(ErrorMessage = "Codul postal este obligatoriu!")]
        public string ZipCode { get; set; }
        [Required(ErrorMessage = "Introduceti o adresa!")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Alegeti o metoda de plata!")]
        public string PaymentM { get; set; }


        public virtual ShoppingCart ShoppingCart { get; set; }
        //public virtual ApplicationUser User { get; set; }
        //public virtual ICollection<Adding> Addings { get; set; }
    }
    /*
    public enum Payment
    {
        Ramburs_la_curier,
        Card_online
    }
    */
}