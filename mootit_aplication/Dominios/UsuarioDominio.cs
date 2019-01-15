using mootit_aplication.Models;
using mootit_aplication.Persistencia;
using System.Data.Entity;
using System.Linq;

namespace mootit_aplication.Dominios
{
    public class UsuarioDominio
    {
        private mootitEntities1 db;

        public UsuarioDominio()
        {
            db = new mootitEntities1();
        }

        public USUARIO inserir(UsuarioModel item)
        {
            USUARIO _item = new USUARIO();

            _item.USU_ID = item.USU_ID;
            _item.USU_NM = item.USU_NM;
            _item.USU_LG = item.USU_LG;
            _item.USU_SN = item.USU_SN;
            _item.USU_ST = item.USU_ST;
            _item.USU_TP_AUTH = item.USU_TP_AUTH;
            
            db.USUARIO.Add(_item);
            db.SaveChanges();
            
            return _item;
        }


        public void alterar(UsuarioModel item)
        {
            USUARIO _item = new USUARIO();

            _item.USU_ID = item.USU_ID;
            _item.USU_NM = item.USU_NM;
            _item.USU_LG = item.USU_LG;
            _item.USU_SN = item.USU_SN;
            _item.USU_ST = item.USU_ST;
            _item.USU_TP_AUTH = item.USU_TP_AUTH;

            db.Entry(_item).State = EntityState.Modified;
            db.SaveChanges();
        }

        public IQueryable<UsuarioModel> buscaPorId(int? usu_id)
        {
            var _lista = db.USUARIO.Where(c => c.USU_ID == usu_id);

            return _lista.ToArray().Select(c =>
                    new UsuarioModel
                    {
                        USU_ID = c.USU_ID,
                        USU_NM = c.USU_NM,
                        USU_LG = c.USU_LG,
                        USU_SN = c.USU_SN,
                        USU_ST = c.USU_ST,
                        USU_TP_AUTH = c.USU_TP_AUTH,

                    }).OrderBy(c => c.USU_NM)
                    .AsQueryable()
            ;
        }

        public IQueryable<UsuarioModel> buscaPorLogin(string usu_lg)
        {
            var _lista =
               from USU in db.USUARIO
               where USU.USU_LG == usu_lg
               select new { USUARIO = USU };

            return _lista.ToArray().Select(c =>
                    new UsuarioModel
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