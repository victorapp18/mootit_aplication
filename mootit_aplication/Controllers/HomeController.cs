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
using mootit_aplication.Dominios;
using System.Device.Location;

namespace mootit_aplication.Controllers
{
    public class HomeController : Controle.Controlador
    {
        #region Auxiliar
        private UsuarioDominio _usuarioDominio;
        public UsuarioDominio usuarioDominio
        {
            get
            {
                if (_usuarioDominio == null)
                    _usuarioDominio = new UsuarioDominio();
                return _usuarioDominio;
            }
        }

        private EnderecoDominio _enderecoDominio;
        public EnderecoDominio enderecoDominio
        {
            get
            {
                if (_enderecoDominio == null)
                    _enderecoDominio = new EnderecoDominio();
                return _enderecoDominio;
            }
        }

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

        #region View
        public ActionResult Index()
        {
            ViewBag.idUsuario = null; 

            return View();
        }

        [HttpPost]
        public ActionResult Index(FormCollection campos)
        {
            ViewBag.text = "";
            try
            {
                var usu_lg = campos["USU_LG"];
                var buscalogin = usuarioDominio.buscaPorLogin(usu_lg);
                    
                if (buscalogin.Count() > 0)
                {
                    string usu_sn = null;
                    int usu_id = 0;

                    foreach (var item in buscalogin)
                    {
                        usu_sn = item.USU_SN;
                        usu_lg = item.USU_LG;
                        usu_id = item.USU_ID;

                    }

                    if (usu_sn == campos["USU_SN"].ToMD5()) {

                        this.USU_ID = usu_id;

                        this.USU_LG = usu_lg;

                        return RedirectToAction("Principal", new { usu_id = usu_id });
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
        public ActionResult Principal(int? usu_id)
        {
            @ViewBag.idUsuario = usu_id;

            var eNDERECO = enderecoDominio.buscaPorUsu_Id(usu_id);

            EnderecoModel item = new EnderecoModel();
            foreach (var _item in eNDERECO)
            {
                item.END_ID = _item.END_ID;
                item.USU_ID = _item.USU_ID;
                item.END_LOGRADOURO = _item.END_LOGRADOURO;
                item.END_NR = _item.END_NR;
                item.END_CIDADE = _item.END_CIDADE;
                item.END_BAIRRO = _item.END_BAIRRO;
                item.END_CEP = _item.END_CEP;
                item.END_LOGRADOURO = _item.END_LOGRADOURO;
                item.END_LATITUDE = _item.END_LATITUDE;
                item.END_LONGITUDE = _item.END_LONGITUDE;
            }


            if (eNDERECO.Count() > 0)
            {
                
                if (item.END_LOGRADOURO == null)
                {
                    Response.Redirect("~/Endereco/Edit?USU_ID=" + usu_id);
                }
                else
                {
                    
                    return View(item);
                }

            }
            else
            {

                EnderecoModel _item = new EnderecoModel();
                _item.USU_ID = usu_id;

                enderecoDominio.inserir(_item);
                
                Response.Redirect("~/Endereco/Edit?USU_ID=" + usu_id);

            }
            return View(eNDERECO);
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

        #endregion

        #region Util
        public JsonResult BuscaEndProximo()
        {
            var eNDERECO = enderecoDominio.buscaPorUsu_Id(this.USU_ID);

            EnderecoModel itemUsuario = new EnderecoModel();
            foreach (var _item in eNDERECO)
            {
                itemUsuario.END_ID = _item.END_ID;
                itemUsuario.USU_ID = _item.USU_ID;
                itemUsuario.END_LOGRADOURO = _item.END_LOGRADOURO;
                itemUsuario.END_NR = _item.END_NR;
                itemUsuario.END_CIDADE = _item.END_CIDADE;
                itemUsuario.END_BAIRRO = _item.END_BAIRRO;
                itemUsuario.END_CEP = _item.END_CEP;
                itemUsuario.END_LOGRADOURO = _item.END_LOGRADOURO;
                itemUsuario.END_LATITUDE = _item.END_LATITUDE;
                itemUsuario.END_LONGITUDE = _item.END_LONGITUDE;
            }

            //postcode australia 2600 -> 3000
            //float latA = float.Parse(itemUsuario.END_LATITUDE, CultureInfo.InvariantCulture.NumberFormat);
            //float longA = float.Parse(itemUsuario.END_LONGITUDE, CultureInfo.InvariantCulture.NumberFormat);

            //double Lat1 = Convert.ToDouble(itemUsuario.END_LATITUDE);
            //double Long1 = Convert.ToDouble(itemUsuario.END_LONGITUDE);

            List<EnderecoModel> listaEndereco = new List<EnderecoModel>();
            var enderecos = enderecoDominio.buscarTodosMenosUsuLogodo(itemUsuario.USU_ID); //db.ENDERECO.Where(x => x.USU_ID != itemUsuario.USU_ID).AsQueryable();

            foreach (var item in enderecos)
            {
                double latA = Convert.ToDouble(itemUsuario.END_LATITUDE);
                double longA = Convert.ToDouble(itemUsuario.END_LONGITUDE);

                double latB = Convert.ToDouble(item.END_LATITUDE);
                double longB = Convert.ToDouble(item.END_LONGITUDE);
                
                double distance = (DistanceAddress.DistanceBetween(latA, latB, longA, longB));
                
                listaEndereco.Add(new EnderecoModel() { USU_ID = item.USU_ID, END_LATITUDE = item.END_LATITUDE, END_LONGITUDE = item.END_LONGITUDE, END_DISTANCIA = distance });
            }

            List<EnderecoModel> listaEnderecosProximos =
                                                            (from end in listaEndereco
                                                             orderby end.END_DISTANCIA descending
                                                             select end).Take(3).ToList();

            //listaEndereco = listaEndereco.OrderByDescending(x => x.END_DISTANCIA).Skip(3).Take(3).ToList();

            return Json(listaEnderecosProximos, JsonRequestBehavior.AllowGet);
        }
        #endregion

    }
}