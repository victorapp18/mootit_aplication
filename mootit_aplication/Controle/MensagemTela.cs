using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mootit_aplication.Controle
{
    public class MensagemTela
    {
        public string mensagem { get; set; }
        public string cssClass
        {
            get;
            private set;
        }

        private MensagemTela(string mensagem, string cssClass)
        {
            this.mensagem = mensagem;
            this.cssClass = cssClass;
        }

        public static MensagemTela sucesso(string mensagem)
        {
            return new MensagemTela(mensagem, "success");
        }
        public static MensagemTela erro(string mensagem)
        {
            return new MensagemTela(mensagem, "error");
        }
        public static MensagemTela erro(Exception ex, string titulo)
        {
            var _erro = getOrigem(ex);
            if (_erro != null)
            {
                var _mensagem = _erro.Message;
                if (string.IsNullOrWhiteSpace(titulo))
                    _mensagem = string.Format("<strong>{0}</strong><br/>{1}", titulo, _mensagem);
                return MensagemTela.erro(_mensagem);
            }
            else
                return null;
        }

        private static Exception getOrigem(Exception ex)
        {
            if (ex != null)
            {
                return
                    ex.InnerException != null ?
                   getOrigem(ex.InnerException) :
                    ex
                ;
            }
            else
                return null;
        }
    }
}
