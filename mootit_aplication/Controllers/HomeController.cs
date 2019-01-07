using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using mootit_aplication.Models;
using System.Web.Security;
using mootit_aplication.Controle;
using System.Globalization;
using mootit_aplication.Util;

namespace mootit_aplication.Controllers
{
    public class HomeController : Controle.Controlador
    {
        private mootitEntities1 db = new mootitEntities1();

        #region Auxiliar
        public int USU_ID
        {
            get
            {
                try { return (int)Session["USU_ID"]; }
                catch (Exception ex) { throw new Exception("Erro ao recuperar Requisição " + ex.Message); }
            }
            set { Session["USU_ID"] = value; }
        }
        
        #endregion

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(FormCollection campos)
        {
            ViewBag.text = "";
            try
            {
                var login = campos["USU_LG"];
                var buscalogin = db.USUARIO.SingleOrDefault(x=>x.USU_LG == login);
                
                if (buscalogin != null)
                {

                    if (buscalogin.USU_SN == campos["USU_SN"].ToMD5()) {

                        this.USU_ID = buscalogin.USU_ID;

                        this.USU_LG = buscalogin.USU_LG;

                        return RedirectToAction("Principal");
                    }else
                    {
                        mensagem = MensagemTela.erro("senha incorreta");
                        return View();
                    }
                }
                else
                {
                    mensagem = MensagemTela.erro("Login incorreto");
                    return View();
                }
            }
            catch (Exception ex)
            {
                mensagem = MensagemTela.erro(ex.Message);
                return View();
            }
        }
        public ActionResult Principal()
        {
            ENDERECO eNDERECO = db.ENDERECO.SingleOrDefault(x => x.USU_ID == this.USU_ID);
            
            if (eNDERECO == null)
            {
                return HttpNotFound();
            }
            return View(eNDERECO);
        }

        public JsonResult BuscaEndProximo()
        {
            ENDERECO eNDERECO = db.ENDERECO.SingleOrDefault(x => x.USU_ID == this.USU_ID);

            //postcode australia 2600 -> 3000
            float latA = float.Parse(eNDERECO.END_LATITUDE, CultureInfo.InvariantCulture.NumberFormat);
            float longA = float.Parse(eNDERECO.END_LONGITUDE, CultureInfo.InvariantCulture.NumberFormat);

            List<EnderecoViewModel> listaEndereco = new List<EnderecoViewModel>();
            var enderecos = db.ENDERECO.Where(x => x.USU_ID != eNDERECO.USU_ID).AsQueryable();

            foreach (var item in enderecos)
            {
                float latB = float.Parse(item.END_LATITUDE, CultureInfo.InvariantCulture.NumberFormat);
                float longB = float.Parse(item.END_LONGITUDE, CultureInfo.InvariantCulture.NumberFormat);

                decimal distance = (DistanceAddress.DistanceBetween(latA, latB, longA, longB));

                listaEndereco.Add(new EnderecoViewModel() { USU_ID = item.USU_ID, END_LATITUDE = item.END_LATITUDE, END_LONGITUDE = item.END_LONGITUDE, END_DISTANCIA = distance });
            }

            listaEndereco = listaEndereco.OrderByDescending(x => x.END_DISTANCIA).Take(3).ToList();

            return Json(listaEndereco, JsonRequestBehavior.AllowGet);
        }

        
        [Authorize]
        [HttpGet]
        public ActionResult Sair()
        {
            Session.Abandon();
            FormsAuthentication.SignOut();
            //...
            var _resposta = new { url = FormsAuthentication.LoginUrl };
            return Json(_resposta, JsonRequestBehavior.AllowGet);
        }

    }
}