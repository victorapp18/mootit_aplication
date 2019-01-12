using mootit_aplication.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace mootit_aplication.Dominios
{
    public class EnderecoDominio
    {
        private mootitEntities1 db;

        public EnderecoDominio()
        {
            db = new mootitEntities1();
        }

        public ENDERECO inserir(ENDERECO item)
        {
            db.ENDERECO.Add(item);
            db.SaveChanges();

            return item;
        }
        public void alterar(ENDERECO item)
        {
            db.Entry(item).State = EntityState.Modified;
            db.SaveChanges();
        }

        public IQueryable<ENDERECO> buscaPorUsu_Id(int? usu_id)
        {
            var _lista = db.ENDERECO.Where(c => c.USU_ID == usu_id);
            return _lista;
        }

        public IQueryable<ENDERECO> buscarTodosMenosUsuLogodo(int? usu_id)
        {
            var _lista =
               from END in db.ENDERECO
               where END.USU_ID != usu_id
               select new { ENDERECO = END };

            return _lista.ToArray().Select(c =>
                    new ENDERECO
                    {
                        USU_ID = c.ENDERECO.USU_ID,
                        END_ID = c.ENDERECO.END_ID,
                        END_LOGRADOURO = c.ENDERECO.END_LOGRADOURO,
                        END_NR = c.ENDERECO.END_NR,
                        END_CIDADE = c.ENDERECO.END_CIDADE,
                        END_BAIRRO = c.ENDERECO.END_BAIRRO,
                        END_CEP = c.ENDERECO.END_CEP,
                        END_LATITUDE = c.ENDERECO.END_LATITUDE,
                        END_LONGITUDE = c.ENDERECO.END_LONGITUDE,

                    }).
                    AsQueryable()
            ;

        }
        
    }
}