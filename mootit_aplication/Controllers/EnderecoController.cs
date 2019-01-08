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

namespace mootit_aplication.Controllers
{
    public class EnderecoController : Controlador

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

        #region View

        // GET: Endereco
        public ActionResult Index()
        {
            var eNDERECO = db.ENDERECO.Include(e => e.USUARIO);
            return View(eNDERECO.ToList());
        }

        // GET: Endereco/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ENDERECO eNDERECO = db.ENDERECO.Find(id);
            if (eNDERECO == null)
            {
                return HttpNotFound();
            }
            return View(eNDERECO);
        }

        // GET: Endereco/Create
        public ActionResult Create(int usu_id)
        {
            this.USU_ID = usu_id;
            
            ViewBag.USU_ID = new SelectList(db.USUARIO, "USU_ID", "USU_LG");
            return View();
        }

        // POST: Endereco/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create([Bind(Include = "USU_ID,END_NR,END_BAIRRO,END_CEP,END_LOGRADOURO,END_CIDADE")] ENDERECO eNDERECO)
        {
            eNDERECO.USU_ID = this.USU_ID;

            string addrres = eNDERECO.END_LOGRADOURO + ", " + eNDERECO.END_NR + ", "+eNDERECO.END_CIDADE+", " + eNDERECO.END_BAIRRO;

            var destination_latLong = GoogleGeoCorder.GetLatLongByAddress(addrres);

            string lat = Convert.ToString(destination_latLong.results[0].geometry.location.lat, CultureInfo.InvariantCulture);

            string lng = Convert.ToString(destination_latLong.results[0].geometry.location.lng, CultureInfo.InvariantCulture);

            eNDERECO.END_LATITUDE = lat;
            eNDERECO.END_LONGITUDE = lng;

            if (ModelState.IsValid)
            {
                db.ENDERECO.Add(eNDERECO);
                db.SaveChanges();

                var buscalogin = db.USUARIO.SingleOrDefault(x => x.USU_ID == this.USU_ID);
                this.USU_LG = buscalogin.USU_LG;

                return RedirectToAction("Principal","Home");
            }

            ViewBag.USU_ID = new SelectList(db.USUARIO, "USU_ID", "USU_LG", eNDERECO.USU_ID);
            return View(eNDERECO);
        }

        

        // GET: Endereco/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ENDERECO eNDERECO = db.ENDERECO.Find(id);
            if (eNDERECO == null)
            {
                return HttpNotFound();
            }
            ViewBag.USU_ID = new SelectList(db.USUARIO, "USU_ID", "USU_LG", eNDERECO.USU_ID);
            return View(eNDERECO);
        }

        // POST: Endereco/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "END_ID,USU_ID,END_LOGRADOURO,END_NR,END_BAIRRO,END_CEP,END_LATITUDE,END_LONGITUDE")] ENDERECO eNDERECO)
        {
            if (ModelState.IsValid)
            {
                db.Entry(eNDERECO).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.USU_ID = new SelectList(db.USUARIO, "USU_ID", "USU_LG", eNDERECO.USU_ID);
            return View(eNDERECO);
        }

        // GET: Endereco/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ENDERECO eNDERECO = db.ENDERECO.Find(id);
            if (eNDERECO == null)
            {
                return HttpNotFound();
            }
            return View(eNDERECO);
        }

        // POST: Endereco/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ENDERECO eNDERECO = db.ENDERECO.Find(id);
            db.ENDERECO.Remove(eNDERECO);
            db.SaveChanges();
            return RedirectToAction("Index");
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
