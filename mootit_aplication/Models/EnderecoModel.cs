using mootit_aplication.Persistencia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mootit_aplication.Models
{
    public class EnderecoModel
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

        public double END_DISTANCIA { get; set; }

        public virtual USUARIO USUARIO { get; set; }

    }
}