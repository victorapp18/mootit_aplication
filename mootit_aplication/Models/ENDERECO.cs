//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace mootit_aplication.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ENDERECO
    {
        public int END_ID { get; set; }
        public Nullable<int> USU_ID { get; set; }
        public string END_LOGRADOURO { get; set; }
        public string END_NR { get; set; }
        public string END_BAIRRO { get; set; }
        public string END_CEP { get; set; }
        public string END_LATITUDE { get; set; }
        public string END_LONGITUDE { get; set; }
        public string END_CIDADE { get; set; }

        public decimal END_DISTANCIA { get; set; }

        public virtual USUARIO USUARIO { get; set; }
    }
}