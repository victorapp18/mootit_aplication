using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace mootit_aplication.Models
{
    public class UsuarioViewModel
    {

        [Display(Name = "USU_LG")]
        [Required(ErrorMessage = "Login é requerido")]
        public string USU_LG { get; set; }

        [Display(Name = "USU_SN")]
        [Required(ErrorMessage = "Senha é requerida")]
        public string USU_SN { get; set; }

        public int USU_ID { get; set; }
        public Nullable<bool> USU_ST { get; set; }
        public string USU_NM { get; set; }
        public Nullable<byte> USU_TP_AUTH { get; set; }

    }
}