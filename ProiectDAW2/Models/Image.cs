using ProiectDAW2.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProiectDAW.Models
{
    public class Image
    {
        [Key]
        public int ImageId { get; set; }
        [Required(ErrorMessage = "Calea pentru poza este obligatorie!")]
        public string ImagePath { get; set; }
        [Required(ErrorMessage = "Numele este obligatoriu!")]
        public string ImageName { get; set; }
        [Required(ErrorMessage = "Extensia este obligatorie!")]
        public string Extension { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}