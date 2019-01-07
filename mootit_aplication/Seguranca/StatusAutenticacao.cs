using System;
using System.Diagnostics;

namespace mootit_aplication.Seguranca
{
    /// <summary>
    /// Retorno da autenticação empacotado
    /// </summary>
    [Serializable]
    [DebuggerDisplay("{success} - {index}")]
    public class StatusAutenticacao
    {
        /// <summary>
        /// True se autenticado 
        /// </summary>
        public bool success
        {
            get;
            internal set;
        }
        /// <summary>
        /// Mensagem em caso de erro
        /// </summary>
        public string message
        {
            get;
            internal set;
        }
        /// <summary>
        /// Tipo do erro
        /// </summary>
        public AuthenticationIndex index
        {
            get;
            internal set;
        }
        /// <summary>
        /// Usuário a ser autenticado
        /// </summary>
        public User user
        {
            get;
            internal set;
        }

        /// <summary>
        /// Converte de AuthenticationStatus para booleano (True se autenticado)
        /// </summary>
        /// <param name="status">AuthenticationStatus a ser convertido</param>
        /// <returns>True se autenticado</returns>
        public static implicit operator bool(StatusAutenticacao status)
        {
            return status.success;
        }
        /// <summary>
        /// Converte de AuthenticationStatus para AuthenticationIndex(Tipo do erro)
        /// </summary>
        /// <param name="status">AuthenticationStatus a ser convertido</param>
        /// <returns>Tipo do erro</returns>
        public static implicit operator AuthenticationIndex(StatusAutenticacao status)
        {
            return status.index;
        }

        /// <summary>
        /// Usuário a ser autenticado
        /// </summary>
        [Serializable]
        [DebuggerDisplay("{userName}")]
        public class User
        {
            /// <summary>
            /// Login
            /// </summary>
            public string userName
            {
                get;
                internal set;
            }
            /// <summary>
            /// E-mail do usuário
            /// </summary>
            public string Email
            {
                get;
                internal set;
            }

            /// <summary>
            /// Grupos que o usuário pertence
            /// </summary>
            public string[] groups
            {
                get;
                internal set;
            }
        }
    }

    /// <summary>
    /// Tipo de erro
    /// </summary>
    [Serializable]
    public enum AuthenticationIndex
    {
        /// <summary>
        /// Login ou senha incorreto(s)
        /// </summary>
        PasswordFail = 1,
        /// <summary>
        /// Erro ao conectar ao servidor
        /// </summary>
        ServerError = 2,
        /// <summary>
        /// Erro do servidor
        /// </summary>
        LDAPException = 3,
        /// <summary>
        /// Erro qualquer
        /// </summary>
        SimpleException = 4,
    }
}