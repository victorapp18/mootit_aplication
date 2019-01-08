using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mootit_aplication.Models
{
    public class EnderecoViewModel
    {
        public int END_ID { get; set; }
        public Nullable<int> USU_ID { get; set; }
        public string END_LATITUDE { get; set; }
        public string END_LONGITUDE { get; set; }
        
        public double END_DISTANCIA { get; set; }

        public virtual USUARIO USUARIO { get; set; }


    }
}