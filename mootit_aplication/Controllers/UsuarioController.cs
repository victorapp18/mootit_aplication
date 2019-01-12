using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using mootit_aplication.Models;
using mootit_aplication.Dominios;

namespace mootit_aplication.Controllers
{
    public class UsuarioController : Controller
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

        #endregion

        #region View
        // GET: Usuario
        //public ActionResult Index()
        //{
        //    return View();
        //}

        // GET: Usuario/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    USUARIO uSUARIO = db.USUARIO.Find(id);
        //    if (uSUARIO == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(uSUARIO);
        //}

        // GET: Usuario/Create
        public ActionResult Create()
        {

            return View();
        }

        // POST: Usuario/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create([Bind(Include = "USU_NM,USU_LG,USU_SN,USU_ST")] USUARIO uSUARIO, FormCollection campos)
        {
            USUARIO _item = new USUARIO();
            _item = viewParaEntidade(null, campos);
            
            if (ModelState.IsValid)
            {
                try
                {
                    usuarioDominio.inserir(_item);

                    var buscalogin = usuarioDominio.buscaPorLogin(_item.USU_LG);

                    foreach (var item in buscalogin)
                    {
                        _item.USU_ID = item.USU_ID;
                    }

                    return RedirectToAction("Create", "Endereco", new { USU_ID = _item.USU_ID });
                }
                catch
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                
            }

            return View(uSUARIO);
        }

        // GET: Usuario/Edit/5
        public ActionResult Edit(int? USU_ID)
        {
            if (USU_ID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            try
            {
                var uSUARIO = usuarioDominio.buscaPorId(USU_ID);
                if (uSUARIO == null)
                {
                    return HttpNotFound();
                }

                USUARIO _item = new USUARIO();

                foreach (var item in uSUARIO)
                {
                    ViewBag.usuario = item.USU_LG;
                    _item.USU_ID = item.USU_ID;
                    _item.USU_NM = item.USU_NM;
                    _item.USU_LG = item.USU_LG;
                    _item.USU_SN = item.USU_SN;
                    _item.USU_ST = item.USU_ST;
                    _item.USU_TP_AUTH = item.USU_TP_AUTH;
                }

                return View(_item);
            }
            catch
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        // POST: Usuario/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit([Bind(Include = "USU_ID,USU_LG,USU_ST,USU_NM,USU_SN,USU_SN_NOVA")] USUARIO uSUARIO)
        {
            try
            {
                if (uSUARIO.USU_SN_NOVA != null)
                {
                    string _senha = "";
                    _senha = uSUARIO.USU_SN_NOVA.ToMD5();
                    uSUARIO.USU_SN = _senha;//.ToMD5();
                    
                    usuarioDominio.alterar(uSUARIO);

                    return RedirectToAction("Edit", "Endereco", new { USU_ID = uSUARIO.USU_ID });
                }
                else
                {

                    usuarioDominio.alterar(uSUARIO);

                    return RedirectToAction("Edit", "Endereco", new { USU_ID = uSUARIO.USU_ID });
                }
            }
            catch
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            
        }

        // GET: Usuario/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    USUARIO uSUARIO = db.USUARIO.Find(id);
        //    if (uSUARIO == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(uSUARIO);
        //}

        // POST: Usuario/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{

        //    USUARIO uSUARIO = db.USUARIO.Find(id);
        //    db.USUARIO.Remove(uSUARIO);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        #endregion

        #region Util
        protected USUARIO viewParaEntidade(System.IConvertible id, FormCollection campos)
        {
            string _senha = "";
            USUARIO _item = new USUARIO();
            _item.USU_NM = campos["USU_NM"];
            _item.USU_LG = campos["USU_LG"];
            _senha = campos["USU_SN"].ToMD5();
            _item.USU_SN = _senha;//.ToMD5();
            _item.USU_ST = Convert.ToBoolean(campos["USU_ST"] == "true,false");

            return _item;
        }

        [HttpGet]
        public JsonResult AjaxValidaUsuario(string _login)
        {
            var _usuario = usuarioDominio.buscaPorLogin(_login);

            if (_usuario.Count() > 0)
            {
                return Json(new { OK = false}, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { OK = true}, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult AjaxValidaSenhaAntiga(string _senhaAntiga, int _USU_ID)
        {
            var _usuario = usuarioDominio.buscaPorId(_USU_ID);

            if (_usuario != null)
            {
                string senhaUsuario = null;

                foreach (var item in _usuario)
                {
                    senhaUsuario = item.USU_SN;   
                }

                if (senhaUsuario == _senhaAntiga.ToMD5() ) {
                    return Json(new { OK = true }, JsonRequestBehavior.AllowGet);
                }else
                {
                    return Json(new { OK = false }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { OK = true }, JsonRequestBehavior.AllowGet);
            }
        }
        
        #endregion
    }
}
