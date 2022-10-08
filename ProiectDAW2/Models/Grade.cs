using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace ProiectDAW2.Models
{
    public class Grade
    {
        [Key]
        public int GradeId { get; set; }
        public string TestName { get; set; }
        public int Score { get; set; }
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}