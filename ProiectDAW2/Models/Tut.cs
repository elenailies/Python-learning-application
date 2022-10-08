using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProiectDAW2.Models
{
    public class Tut
    {

        [Key]
        public int TutId { get; set; }
        public string TutName { get; set; }
        public int Rezolvat { get; set; }
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}