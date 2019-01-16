using mootit_aplication.Models;
using mootit_aplication.Persistencia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mootit_aplication.Controle
{
    public class Controlador : Controller
    {
        private mootitEntities1 db = new mootitEntities1();

        protected MensagemTela mensagem
        {
            set
            {
                ViewBag.mensagemTela = value != null ? value.mensagem : null;
                ViewBag.mensagemTelaCss = value != null ? value.cssClass : null;
            }
        }

        #region Dados logado

        private USUARIO _usuarioLogado;
        public USUARIO usuarioLogado
        {
            get
            {
                if ((_usuarioLogado == null) && this.User.Identity.IsAuthenticated)
                    _usuarioLogado = db.USUARIO.SingleOrDefault(x => x.USU_LG == this.User.Identity.Name);
                return _usuarioLogado;
            }
        }

        private string _nomeController;
        public string nomeController
        {
            get
            {
                if (_nomeController == null)
                {
                    _nomeController = this.GetType().Name;
                    _nomeController = _nomeController.Substring(0, _nomeController.Length - 10);
                }
                return _nomeController;
            }
        }

        #endregion
        
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);

            if (usuarioLogado != null)
            {
                //var _funcionario = funcionarioLogado;

                var _usuario = usuarioLogado;

                ViewBag.nomeUsuarioLogado = usuarioLogado.USU_NM;
                ViewBag.idUsuario = _usuarioLogado.USU_ID;
                ViewBag.usuario = _usuarioLogado.USU_LG;
                //...
            }
        }
    }
}
