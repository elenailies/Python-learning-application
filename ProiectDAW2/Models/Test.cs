using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProiectDAW.Models
{
    public class Test
    {
        [Key]
        public int TestId { get; set; }

        [Required(ErrorMessage = "Numele testului este obligatoriu!")]
        [StringLength(100, ErrorMessage = "Numele testului nu poate avea mai mult de 100 de caractere")]
        public string TestName { get; set; }

        [Required(ErrorMessage = "Campul este obligatoriu!")]
        public string intrebare_1 { get; set; }
        [Required(ErrorMessage = "Campul este obligatoriu!")]
        public string raspuns_1_1 { get; set; }
        [Required(ErrorMessage = "Campul este obligatoriu!")]
        public string raspuns_1_2 { get; set; }
        [Required(ErrorMessage = "Campul este obligatoriu!")]
        public string raspuns_1_3 { get; set; }
        [Required(ErrorMessage = "Campul este obligatoriu!")]
        public string raspuns_1_4 { get; set; }
        [Required(ErrorMessage = "Campul este obligatoriu!")]
        public int raspuns_corect_1 { get; set; }
        [Required(ErrorMessage = "Campul este obligatoriu!")]
        public int raspuns_ales_1 { get; set; }


        [Required(ErrorMessage = "Campul este obligatoriu!")]
        public string intrebare_2 { get; set; }
        [Required(ErrorMessage = "Campul este obligatoriu!")]
        public string raspuns_2_1 { get; set; }
        [Required(ErrorMessage = "Campul este obligatoriu!")]
        public string raspuns_2_2 { get; set; }
        [Required(ErrorMessage = "Campul este obligatoriu!")]
        public string raspuns_2_3 { get; set; }
        [Required(ErrorMessage = "Campul este obligatoriu!")]
        public string raspuns_2_4 { get; set; }
        [Required(ErrorMessage = "Campul este obligatoriu!")]
        public int raspuns_corect_2 { get; set; }
        [Required(ErrorMessage = "Campul este obligatoriu!")]
        public int raspuns_ales_2 { get; set; }

        [Required(ErrorMessage = "Campul este obligatoriu!")]
        public string intrebare_3 { get; set; }
        [Required(ErrorMessage = "Campul este obligatoriu!")]
        public string raspuns_3_1 { get; set; }
        [Required(ErrorMessage = "Campul este obligatoriu!")]
        public string raspuns_3_2 { get; set; }
        [Required(ErrorMessage = "Campul este obligatoriu!")]
        public string raspuns_3_3 { get; set; }
        [Required(ErrorMessage = "Campul este obligatoriu!")]
        public string raspuns_3_4 { get; set; }
        [Required(ErrorMessage = "Campul este obligatoriu!")]
        public int raspuns_corect_3 { get; set; }
        [Required(ErrorMessage = "Campul este obligatoriu!")]
        public int raspuns_ales_3 { get; set; }

    }
}