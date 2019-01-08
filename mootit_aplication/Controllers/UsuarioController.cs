using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using mootit_aplication.Models;

namespace mootit_aplication.Controllers
{
    public class UsuarioController : Controller
    {
        private mootitEntities1 db = new mootitEntities1();

        #region View
        // GET: Usuario
        public ActionResult Index()
        {
            return View(db.USUARIO.ToList());
        }

        // GET: Usuario/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            USUARIO uSUARIO = db.USUARIO.Find(id);
            if (uSUARIO == null)
            {
                return HttpNotFound();
            }
            return View(uSUARIO);
        }

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
                db.USUARIO.Add(_item);
                db.SaveChanges();

                var buscalogin = db.USUARIO.SingleOrDefault(x=>x.USU_LG == uSUARIO.USU_LG);
                
                return RedirectToAction("Create","Endereco", new { USU_ID = buscalogin.USU_ID});
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
            USUARIO uSUARIO = db.USUARIO.Find(USU_ID);
            if (uSUARIO == null)
            {
                return HttpNotFound();
            }

            ViewBag.usuario = uSUARIO.USU_LG;
            uSUARIO.USU_SN = uSUARIO.USU_SN.ToMD5();

            return View(uSUARIO);
        }

        // POST: Usuario/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "USU_ID,USU_LG,USU_SN,USU_ST,USU_NM,USU_TP_AUTH")] USUARIO uSUARIO)
        {
            if (ModelState.IsValid)
            {
                db.Entry(uSUARIO).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(uSUARIO);
        }

        // GET: Usuario/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            USUARIO uSUARIO = db.USUARIO.Find(id);
            if (uSUARIO == null)
            {
                return HttpNotFound();
            }
            return View(uSUARIO);
        }

        // POST: Usuario/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {

            USUARIO uSUARIO = db.USUARIO.Find(id);
            db.USUARIO.Remove(uSUARIO);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        #endregion

        #region Util
        protected USUARIO viewParaEntidade(System.IConvertible id, FormCollection campos)
        {
            string _senha = "";
            USUARIO _item = new USUARIO();
            _item.USU_LG = campos["USU_LG"];
            _senha = campos["USU_SN"].ToMD5();
            _item.USU_SN = _senha;//.ToMD5();
            _item.USU_ST = Convert.ToBoolean(campos["USU_ST"] == "true,false");

            return _item;
        }

        [HttpGet]
        public JsonResult AjaxValidaUsuario(string _login)
        {
            
            var _usuario = db.USUARIO.SingleOrDefault(x=>x.USU_LG == _login);
            if (_usuario != null)
            {
                return Json(new { OK = true, Nome = _usuario.USU_LG }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return null;
            }
        }
        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion
    }
}
