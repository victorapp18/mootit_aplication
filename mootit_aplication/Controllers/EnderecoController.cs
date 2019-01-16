using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using mootit_aplication.Models;
using System.Xml;
using System.IO;
using System.Web.Security;
using mootit_aplication.Controle;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using mootit_aplication.Util;
using System.Globalization;
using mootit_aplication.Dominios;

namespace mootit_aplication.Controllers
{
    public class EnderecoController : Controlador

    {

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

        #endregion

        #region View

        // GET: Endereco
        //public ActionResult Index()
        //{
        //    var eNDERECO = db.ENDERECO.Include(e => e.USUARIO);
        //    return View(eNDERECO.ToList());
        //}

        // GET: Endereco/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    ENDERECO eNDERECO = db.ENDERECO.Find(id);
        //    if (eNDERECO == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(eNDERECO);
        //}

        // GET: Endereco/Create
        public ActionResult Create(int usu_id)
        {
            return View();
        }

        // POST: Endereco/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create([Bind(Include = "USU_ID,END_NR,END_BAIRRO,END_CEP,END_LOGRADOURO,END_CIDADE")] EnderecoModel eNDERECO)
        {
            eNDERECO.USU_ID = usuarioLogado.USU_ID;

            string addrres = eNDERECO.END_LOGRADOURO + ", " + eNDERECO.END_NR + ", "+eNDERECO.END_CIDADE+", " + eNDERECO.END_BAIRRO;

            var destination_latLong = GoogleGeoCorder.GetLatLongByAddress(addrres);

            //string lat = Convert.ToString(destination_latLong.results[0].geometry.location.lat, CultureInfo.InvariantCulture);

            //string lng = Convert.ToString(destination_latLong.results[0].geometry.location.lng, CultureInfo.InvariantCulture);

            eNDERECO.END_LATITUDE = "0";
            eNDERECO.END_LONGITUDE = "0";

            if (ModelState.IsValid)
            {
                try
                {
                    enderecoDominio.inserir(eNDERECO);

                    var buscalogin = usuarioDominio.buscaPorId(usuarioLogado.USU_ID);

                    foreach (var item in buscalogin)
                    {
                        eNDERECO.USU_ID = item.USU_ID;
                    }
                    
                    return RedirectToAction("Principal", "Home", new {usu_id = eNDERECO.USU_ID });
                }
                catch(Exception e)
                {
                    mensagem = MensagemTela.erro("erro no sistema: "+e);
                    return View();
                }
                
            }
            return View(eNDERECO);
        }

        

        // GET: Endereco/Edit/5
        public ActionResult Edit(int? USU_ID)
        {
            if (USU_ID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var eNDERECO = enderecoDominio.buscaPorUsu_Id(USU_ID);
            if (eNDERECO.Count() == 0)
            {
                return View();
            }
            else
            {
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

                return View(item);
            }
        }

        // POST: Endereco/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit([Bind(Include = "END_ID,USU_ID,END_LOGRADOURO,END_NR,END_CIDADE,END_BAIRRO,END_CEP")] EnderecoModel eNDERECO)
        {

            string addrres = eNDERECO.END_LOGRADOURO + ", " + eNDERECO.END_NR + ", " + eNDERECO.END_CIDADE + ", " + eNDERECO.END_BAIRRO;

            var destination_latLong = GoogleGeoCorder.GetLatLongByAddress(addrres);

            //string lat = Convert.ToString(destination_latLong.results[0].geometry.location.lat, CultureInfo.InvariantCulture);

            //string lng = Convert.ToString(destination_latLong.results[0].geometry.location.lng, CultureInfo.InvariantCulture);

            eNDERECO.END_LATITUDE = "-1.29267215";
            eNDERECO.END_LONGITUDE = "-48.47828597";

            if (ModelState.IsValid)
            {
                try
                {
                    enderecoDominio.alterar(eNDERECO);
                    
                    return RedirectToAction("Principal", "Home", new { usu_id = eNDERECO.USU_ID });
                }
                catch (Exception e)
                {
                    mensagem = MensagemTela.erro("erro no sistema: " + e);
                    return View();
                }

            }
            else
            {
                return View(eNDERECO);
            }
        }

        // GET: Endereco/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    ENDERECO eNDERECO = db.ENDERECO.Find(id);
        //    if (eNDERECO == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(eNDERECO);
        //}

        // POST: Endereco/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    ENDERECO eNDERECO = db.ENDERECO.Find(id);
        //    db.ENDERECO.Remove(eNDERECO);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
        #endregion

    }
}
