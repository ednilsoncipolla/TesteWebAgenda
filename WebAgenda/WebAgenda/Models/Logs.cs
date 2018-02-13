using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAgenda.Models
{
    public static class Logs
    {
        public static void Incluir(string sDescricao)
        {
            if (!string.IsNullOrEmpty(sDescricao))
            {
                string sSql = $"Insert into Logs ( {Environment.NewLine} " +
                    $"    Log_Data {Environment.NewLine} " +
                    $"    , Log_Descricao {Environment.NewLine} " +
                    $") values( {Environment.NewLine} " +
                    $"    current_timestamp() {Environment.NewLine} " +
                    $"    , '{sDescricao}' {Environment.NewLine} " +
                    $") { Environment.NewLine} ";

                BdMySql.executaComando(sSql);
            }
        }
    }
}