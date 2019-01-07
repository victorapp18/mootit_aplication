using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

public static class StringExtencoes
{
    public static bool temValor(this string valor, bool trim = true)
    {
        return
            trim ?
            (valor + "").Trim() != "" :
            (valor + "") != ""
        ;
    }

    public static bool vazio(this string valor, bool trim = true)
    {
        return !valor.temValor(trim);
    }

    public static string ToMD5(this string valor)
    {
        MD5 md5Hasher = MD5.Create();
        byte[] valorCriptografado = md5Hasher.ComputeHash(Encoding.Default.GetBytes(valor));
        StringBuilder strBuilder = new StringBuilder();
        for (int i = 0; i < valorCriptografado.Length; i++)
        {
            strBuilder.Append(valorCriptografado[i].ToString("x2"));
        }
        return strBuilder.ToString();
    }

    public static string AplicaMascaraProtocolo(this string protocolo)
    {
        string _saida = "";
        if (protocolo != null)
        {
            string[] _values =
                protocolo.Select(c => c.ToString()).ToArray();
            switch (_values.Length)
            {
                case 12:
                    {
                        string _arg0 = protocolo.Substring(0, 4);
                        string _arg1 = protocolo.Substring(4, 2);
                        string _arg2 = protocolo.Substring(6, 6);
                        _saida = string.Format("{0}.{1}.{2}", _arg0, _arg1, _arg2);
                    }
                    break;
                default:
                    _saida = protocolo;
                    break;
            }
        }
        else
        {
            _saida = protocolo;
        }
        return _saida;
    }

    public static string AplicaMascaraBop(this string bop)
    {
        string _saida = "";
        if (bop != null)
        {

            bop = bop.PadLeft(16, '0');
            string[] _values =
                bop.Select(c => c.ToString()).ToArray();
            switch (_values.Length)
            {
                case 16:
                    {
                        string _arg0 = bop.Substring(0, 5);
                        string _arg1 = bop.Substring(5, 4);
                        string _arg2 = bop.Substring(9, 6);
                        string _arg3 = bop.Substring(15, 1);
                        _saida = string.Format("{0}/{1}.{2}-{3}",
                            _arg0, _arg1, _arg2, _arg3);
                    }
                    break;
                default:
                    _saida = bop;
                    break;
            }
        }
        else { _saida = bop; }

        return _saida;
    }

    public static string AplicaMascaraProcessoTJ(this string processo)
    {
        string _saida = "";
        if (processo != null)
        {

            processo = processo.PadLeft(20, '0');
            string[] _values =
                processo.Select(c => c.ToString()).ToArray();
            switch (_values.Length)
            {
                case 20:
                    {
                        string _arg0 = processo.Substring(0, 7);
                        string _arg1 = processo.Substring(7, 2);
                        string _arg2 = processo.Substring(9, 4);
                        string _arg3 = processo.Substring(13, 1);
                        string _arg4 = processo.Substring(14, 2);
                        string _arg5 = processo.Substring(16, 4);
                        _saida = string.Format("{0}-{1}.{2}.{3}.{4}.{5}",
                            _arg0, _arg1, _arg2, _arg3, _arg4, _arg5);
                    }
                    break;
                default:
                    _saida = processo;
                    break;
            }
        }
        else { _saida = processo; }

        return _saida;
    }

    public static string RetiraMascaraBop(this string bop)
    {
        if (!string.IsNullOrWhiteSpace(bop))
        {
            string _retorno = null;
            foreach (char letra in bop)
            {
                if (char.IsNumber(letra))
                    _retorno += letra;
            }
            return _retorno;
        }
        else
            return bop;
    }

    public static string TipoDocumentoDescricao(this int tipoDocumento)
    {
        string _saida = string.Empty;
        switch (tipoDocumento)
        {
            case 1:
                _saida = "BOP";
                break;
            case 2:
                _saida = "TOMBO";
                break;
            case 3:
                _saida = "Ofício";
                break;
            case 4:
                _saida = "Processo SIMPE";
                break;
            case 5:
                _saida = "Processo Judicial";
                break;
            default:
                _saida = "-";
                break;
        }
        return _saida;
    }

    public static bool equivalente(this string valor, string teste)
    {
        return string.Concat(valor, "").Trim().ToLower() == string.Concat(teste, "").ToLower();
    }

    public static string formataData(DateTime data)
    {
        string mes, dataCompleta = "";
        int month = data.Month;

        #region Formatação do Mês
        switch (month)
        {
            case 1: mes = "Janeiro"; break;
            case 2: mes = "Fevereiro"; break;
            case 3: mes = "Março"; break;
            case 4: mes = "Abril"; break;
            case 5: mes = "Maio"; break;
            case 6: mes = "Junho"; break;
            case 7: mes = "Julho"; break;
            case 8: mes = "Agosto"; break;
            case 9: mes = "Setembro"; break;
            case 10: mes = "Outubro"; break;
            case 11: mes = "Novembro"; break;
            case 12: mes = "Dezembro"; break;
            default: mes = "" + month; break;
        }
        #endregion

        dataCompleta = data.Day + " de " + mes + " de " + data.Year + "";

        return dataCompleta;
    }
}
