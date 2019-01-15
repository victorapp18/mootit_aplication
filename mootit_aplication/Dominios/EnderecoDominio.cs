using mootit_aplication.Models;
using mootit_aplication.Persistencia;
using System.Data.Entity;
using System.Linq;

namespace mootit_aplication.Dominios
{
    public class EnderecoDominio
    {
        private mootitEntities1 db;

        public EnderecoDominio()
        {
            db = new mootitEntities1();
        }

        public EnderecoModel inserir(EnderecoModel item)
        {
            ENDERECO _item = new ENDERECO();

            _item.END_ID = item.END_ID;
            _item.USU_ID = item.USU_ID;
            _item.END_LOGRADOURO = item.END_LOGRADOURO;
            _item.END_NR = item.END_NR;
            _item.END_CIDADE = item.END_CIDADE;
            _item.END_BAIRRO = item.END_BAIRRO;
            _item.END_CEP = item.END_CEP;
            _item.END_LOGRADOURO = item.END_LOGRADOURO;
            _item.END_LATITUDE = item.END_LATITUDE;
            _item.END_LONGITUDE = item.END_LONGITUDE;
            

            db.ENDERECO.Add(_item);
            db.SaveChanges();

            return item;
        }
        public void alterar(EnderecoModel item)
        {
            ENDERECO _item = new ENDERECO();

            _item.END_ID = item.END_ID;
            _item.USU_ID = item.USU_ID;
            _item.END_LOGRADOURO = item.END_LOGRADOURO;
            _item.END_NR = item.END_NR;
            _item.END_CIDADE = item.END_CIDADE;
            _item.END_BAIRRO = item.END_BAIRRO;
            _item.END_CEP = item.END_CEP;
            _item.END_LOGRADOURO = item.END_LOGRADOURO;
            _item.END_LATITUDE = item.END_LATITUDE;
            _item.END_LONGITUDE = item.END_LONGITUDE;

            db.Entry(_item).State = EntityState.Modified;
            db.SaveChanges();
        }

        public IQueryable<EnderecoModel> buscaPorUsu_Id(int? usu_id)
        {
            var _lista = db.ENDERECO.Where(c => c.USU_ID == usu_id);

            return _lista.ToArray().Select(c =>
                    new EnderecoModel
                    {
                        USU_ID = c.USU_ID,
                        END_ID = c.END_ID,
                        END_LOGRADOURO = c.END_LOGRADOURO,
                        END_NR = c.END_NR,
                        END_CIDADE = c.END_CIDADE,
                        END_BAIRRO = c.END_BAIRRO,
                        END_CEP = c.END_CEP,
                        END_LATITUDE = c.END_LATITUDE,
                        END_LONGITUDE = c.END_LONGITUDE,

                    }).
                    AsQueryable()
            ;
        }

        public IQueryable<EnderecoModel> buscarTodosMenosUsuLogodo(int? usu_id)
        {
            var _lista =
               from END in db.ENDERECO
               where END.USU_ID != usu_id
               select new { ENDERECO = END };

            return _lista.ToArray().Select(c =>
                    new EnderecoModel
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