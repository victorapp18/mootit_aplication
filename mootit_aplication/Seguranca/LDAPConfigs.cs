using System;
using System.Diagnostics;
using System.Linq;

namespace mootit_aplication.Seguranca
{
    /// <summary>
    /// Interpreta as configurações necessárias para o AD
    /// </summary> 
    [DebuggerDisplay("{ldapUrl}")]
    public class LDAPConfigs
    {
        /// <summary>
        /// Endereço do AD 
        /// </summary>
        internal string ldapHost
        {
            get;
            private set;
        }
        /// <summary>
        /// Url montada através do ldapAddress
        /// </summary>
        internal string ldapUrl
        {
            get
            {
                string host = (ldapHost + "").Trim() != "" ? ldapHost.Trim() : "127.0.0.1";
                return String.Format("LDAP://{0}", host);
            }
        }
        /*
        /// <summary>
        /// Endereço do domínio
        /// </summary>
        internal string domainAddress
        {
            get;
            private set;
        }
         */
        /// <summary>
        /// Grupos permitidos pela url
        /// </summary>
        public string[] gruposPermitidos
        {
            get;
            private set;
        }
        /// <summary>
        /// Login para autenticar no AD
        /// </summary>
        public string login
        {
            get;
            private set;
        }
        /// <summary>
        /// Senha para autenticar no AD
        /// </summary>
        public string senha { get; set; }

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="ConnectionString">Strimg contendo os dados de configuração</param>
        private LDAPConfigs(string ConnectionString)
        {
            string[] _configs = String.Concat(ConnectionString, "").Trim().Split(';');
            //...
            ldapHost = _configs.FirstOrDefault(
                c => c.ToLower().Trim().StartsWith("ldap=")
            );
            ldapHost = buildPart(ldapHost);
            //...
            login = _configs.FirstOrDefault(
                c => c.ToLower().Trim().StartsWith("login=")
            );
            login = buildPart(login);
            //...
            senha = _configs.FirstOrDefault(
                c => c.ToLower().Trim().StartsWith("senha=")
            );
            senha = buildPart(senha);
            //...
            /*
            domainAddress = _configs.FirstOrDefault(
                c => c.ToLower().Trim().StartsWith("domain=")
            );
            domainAddress = buildPart(domainAddress);
             * */
            //...
            string _parteGrupos = _configs.FirstOrDefault(
                c => c.ToLower().Trim().StartsWith("grupos=")
            );
            _parteGrupos = buildPart(_parteGrupos);
            gruposPermitidos = _parteGrupos.Split(',');
        }
        /// <summary>
        /// Converte da Strimg contendo os dados de configuração para LDAPConfigs
        /// </summary>
        /// <param name="ConnectionString">Strimg contendo todos dos dados do AD</param>
        /// <returns>LDAPConfigs</returns>
        public static implicit operator LDAPConfigs(string ConnectionString)
        {
            return new LDAPConfigs(ConnectionString);
        }

        /// <summary>
        /// Formata usuário e dominio
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        internal string getUserIdentification(string userName)
        {
            /*
            return
                (domainAddress + "").Trim() != "" ?
                domainAddress + @"\" + userName :
                userName
            ;
             */
            return userName;
        }
        private string buildPart(string part)
        {
            if (part != null)
            {
                int _idxSeparador = part.IndexOf('=');
                return part.Substring(_idxSeparador + 1).Trim();
            }
            else
                return null;
        }
    }
}
