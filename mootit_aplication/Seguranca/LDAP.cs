using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Runtime.InteropServices;

namespace mootit_aplication.Seguranca
{
    /// <summary> 
    /// Gerenciador de autenticação com o AD
    /// </summary>
    /// <remarks>
    /// <pre>
    ///    ======== URL =====
    ///    
    ///    ldap=192.160.0.1;domain=192.160.0.1;grupos=grupo1,grupo2
    /// 
    ///    ========= C# ===== 
    /// 
    ///    LDAPConfigs dadosSeguranca = ConfigurationManager.AppSettings["dadosSeguranca"];
    ///    LDAP _autenticacao = new LDAP(dadosSeguranca);
    ///    AuthenticationStatus _status = _autenticacao.validate(txtMatricula.Text, txtSenha.Text);
    /// </pre>
    /// </remarks>
    public class LDAP
    {
        /// <summary>
        /// Dados da conexão com LDAP
        /// </summary>
        public LDAPConfigs config
        {
            get;
            private set;
        }
        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="config">
        /// Configurações para a autenticação<br/>
        /// <b>Exe: "ldap=0.0.0.0;domain=0.0.0.0;grupos=grupo1,grupo2"</b>
        /// </param>
        public LDAP(LDAPConfigs config)
        {
            this.config = config;
        }

        /// <summary>
        /// Valida o usuário no AD
        /// </summary>
        /// <param name="userName">Login</param>
        /// <param name="password">Senha</param>
        /// <returns>Status da autenticação empacotado</returns>
        public StatusAutenticacao autentica(string userName, string password)
        {
            string userIdentification = config.getUserIdentification(userName);
            var entry = new DirectoryEntry(
                config.ldapUrl, userIdentification, password
            );
            return getAutenticacao(entry, userName);
        }
        /// <summary>
        /// Faz autenticação desconsiderando senha
        /// </summary>
        /// <param name="userName">Nome de usuário</param>
        /// <returns>Status da autenticação empacotado</returns>
        public StatusAutenticacao buscaUsuario(string userName)
        {
            if (string.Concat(config.login, "").Trim() == "")
                throw new Exception("Login do LDAP não definido");
            string userIdentification = config.getUserIdentification(config.login);
            var entry = new DirectoryEntry(
                config.ldapUrl, userIdentification, config.senha
            );
            return getAutenticacao(entry, userName);
        }

        private StatusAutenticacao getAutenticacao(DirectoryEntry entry, string userName)
        {
            try
            {
                DirectorySearcher search = new DirectorySearcher(entry);
                search.Filter = "(SAMAccountName=" + userName + ")";
                search.PropertiesToLoad.Add("cn");
                search.PropertiesToLoad.Add("mail");
                SearchResult result = search.FindOne();
                //...
                string _filtro = (string)result.Properties["cn"][0];

                string _email = null;
                try { _email = (string)result.Properties["mail"][0]; }
                catch { }

                //string[] _grupos = GetGroups(_filtro, result.Path);
                //...
                return new StatusAutenticacao()
                {
                    success = true,
                    message = null,
                    user = new StatusAutenticacao.User() { userName = userName, Email = _email }, //groups = _grupos, Email = _email },
                };
            }
            catch (COMException ex)
            {
                AuthenticationIndex _indice;
                switch (ex.ErrorCode)
                {
                    case -2147023570: _indice = AuthenticationIndex.PasswordFail; break;
                    case -2147016646: _indice = AuthenticationIndex.ServerError; break;
                    default: _indice = AuthenticationIndex.LDAPException; break;
                }
                return GetError(_indice, userName, ex.Message);
            }
            catch (Exception ex)
            {
                return GetError(AuthenticationIndex.SimpleException, userName, ex.Message);
            }
        }
        private string[] GetGroups(string filterAttribute, string _pathServer)
        {
            DirectorySearcher search = new DirectorySearcher(_pathServer);
            search.Filter = "(cn=" + filterAttribute + ")";
            search.PropertiesToLoad.Add("memberOf");
            List<String> groupNames = new List<string>();

            try
            {
                SearchResult result = search.FindOne();
                int propertyCount = result.Properties["memberOf"].Count;
                string dn;
                int equalsIndex, commaIndex;

                for (int propertyCounter = 0; propertyCounter < propertyCount; propertyCounter++)
                {
                    dn = (string)result.Properties["memberOf"][propertyCounter];
                    equalsIndex = dn.IndexOf("=", 1);
                    commaIndex = dn.IndexOf(",", 1);
                    if (-1 == equalsIndex)
                        return new string[0];
                    groupNames.Add(dn.Substring((equalsIndex + 1), (commaIndex - equalsIndex) - 1));
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error obtaining group names. " + ex.Message);
            }
            return groupNames.ToArray();
        }
        private StatusAutenticacao GetError(AuthenticationIndex index, string userName, string message)
        {
            return new StatusAutenticacao()
            {
                success = false,
                index = index,
                message = message,
                user = new StatusAutenticacao.User() { userName = userName, groups = new string[0] },
            };
        }
    }

}