using mootit_aplication.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace mootit_aplication.Dominios
{
    public class UsuarioDominio
    {
        private mootitEntities1 db;

        public UsuarioDominio()
        {
            db = new mootitEntities1();
        }

        public USUARIO inserir(USUARIO item)
        {
            db.USUARIO.Add(item);
            db.SaveChanges();
            
            return item;
        }


        public void alterar(USUARIO item)
        {
            db.Entry(item).State = EntityState.Modified;
            db.SaveChanges();
        }

        public IQueryable<USUARIO> buscaPorId(int? usu_id)
        {
            var _lista = db.USUARIO.Where(c => c.USU_ID == usu_id);
            return _lista.OrderBy(c => c.USU_NM);
        }

        public IQueryable<USUARIO> buscaPorLogin(string usu_lg)
        {
            var _lista =
               from USU in db.USUARIO
               where USU.USU_LG == usu_lg
               select new { USUARIO = USU };

            return _lista.ToArray().Select(c =>
                    new USUARIO
                    {
                        USU_ID = c.USUARIO.USU_ID,
                        USU_NM = c.USUARIO.USU_NM,
                        USU_LG = c.USUARIO.USU_LG,
                        USU_SN = c.USUARIO.USU_SN,
                        USU_ST = c.USUARIO.USU_ST,
                        USU_TP_AUTH = c.USUARIO.USU_TP_AUTH,

                    }).
                    AsQueryable()
            ;

        }
        
        public void remover()
        {

        }
        
    }
}