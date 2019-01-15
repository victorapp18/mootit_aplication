using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using mootit_aplication.Persistencia;

namespace mootit_aplication.Models
{
    public class UsuarioModel
    {

        [Display(Name = "USU_LG")]
        [Required(ErrorMessage = "Login é requerido")]
        public string USU_LG { get; set; }

        [Display(Name = "USU_SN")]
        [Required(ErrorMessage = "Senha é requerida")]
        public string USU_SN { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public UsuarioModel()
        {
            this.ENDERECO = new HashSet<ENDERECO>();
        }

        public int USU_ID { get; set; }
        public Nullable<bool> USU_ST { get; set; }
        public string USU_NM { get; set; }
        public Nullable<byte> USU_TP_AUTH { get; set; }

        public string USU_SN_NOVA { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ENDERECO> ENDERECO { get; set; }

    }
}