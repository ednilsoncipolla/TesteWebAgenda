using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

namespace WebAgenda.Models
{
    public class Usuario
    {
        #region Atributos
        int usu_Id = 0;
        int med_Id = 0;
        string usu_Nome;
        string usu_Login;
        string usu_Senha;

        public int Usu_Id { get => usu_Id; set => usu_Id = value; }
        public int Med_Id { get => med_Id; set => med_Id = value; }
        public string Usu_Nome { get => usu_Nome; set => usu_Nome = value; }
        public string Usu_Login { get => usu_Login; set => usu_Login = value; }
        public string Usu_Senha { get => usu_Senha; set => usu_Senha = value; }

        #endregion

        #region CRUD
        public DataTable GetDtUsuarios(string pWhere)
        {
            DataTable dt = new DataTable();

            string sSql = $"Select {Environment.NewLine} " +
                $"  Usu_Id {Environment.NewLine} " +
                $"  , Med_Id {Environment.NewLine} " +
                $"  , Usu_Nome {Environment.NewLine} " +
                $"  , Usu_Login {Environment.NewLine} " +
                $"  ,Usu_Senha {Environment.NewLine} " +
                $"from usuarios {Environment.NewLine} " +
                (string.IsNullOrEmpty(pWhere) ? "" : $"where {pWhere} {Environment.NewLine} ");

            dt = BdMySql.getDataTable(sSql);

            return dt;
        }

        public void Gravar()
        {
            string sSql = "";
            StringBuilder sLog = new StringBuilder();

            if (
                string.IsNullOrEmpty(Usu_Nome)
                && string.IsNullOrEmpty(Usu_Login)
                && string.IsNullOrEmpty(Usu_Senha)
                && Med_Id <= 0
               )
            {
                throw new System.ArgumentException("Não fora informado dados a serem salvos!", "Salvar informações de Usuário");
            }

            if (Usu_Id > 0)
            {
                if (!string.IsNullOrEmpty(Usu_Login))
                {
                    DataTable dt = GetDtUsuarios($"Where Usu_Login = {Usu_Login} and Usu_Id <> {Usu_Id} ");

                    if (dt.Rows.Count > 0)
                    {
                        throw new System.ArgumentException("Login inválido, já está em uso!", "Incluir Usuário");
                    }
                }

                sLog.AppendLine($"{Sistema.Usuario.Usu_Nome} Alterou informações do Usuário com ID {Usu_Id} ");

                DataTable dtAntes = GetDtUsuarios($"Where Usu_Id = {Usu_Id}");

                if (dtAntes.Rows.Count == 0)
                {
                    throw new System.ArgumentException($"Não foi possivel localizar o Usuário com ID {Usu_Id}!", "Salvar informações de Usuário");
                }

                StringBuilder sCampos = new StringBuilder();

                if (Med_Id > 0 && dtAntes.Rows[0]["Med_Id"].ToString() != Med_Id.ToString())
                {
                    sCampos.AppendLine($", Med_Id = {Med_Id} {Environment.NewLine} ");
                    sLog.AppendLine($"Id do Médico de {dtAntes.Rows[0]["Med_Id"].ToString()} para {Med_Id}");
                }

                if (!string.IsNullOrEmpty(Usu_Nome) && dtAntes.Rows[0]["Usu_Nome"].ToString() != Usu_Nome.Trim())
                {
                    sCampos.AppendLine($", Usu_Nome = '{Usu_Nome}' {Environment.NewLine} ");
                    sLog.AppendLine($"Nome do Usuario de {dtAntes.Rows[0]["Usu_Nome"].ToString()} para {Usu_Nome}");
                }

                if (!string.IsNullOrEmpty(Usu_Login) && dtAntes.Rows[0]["Usu_Login"].ToString() != Usu_Login.Trim())
                {
                    sCampos.AppendLine($", Usu_Login = '{Usu_Login}' {Environment.NewLine} ");
                    sLog.AppendLine($"Login do Usuario de {dtAntes.Rows[0]["Usu_Login"].ToString()} para {Usu_Login}");
                }

                if (!string.IsNullOrEmpty(Usu_Senha) && dtAntes.Rows[0]["Usu_Senha"].ToString() != Usu_Senha.Trim())
                {
                    sCampos.AppendLine($", Usu_Senha = '{Usu_Senha}' {Environment.NewLine} ");
                    sLog.AppendLine($"Senha do Usuario!");
                }

                if (string.IsNullOrEmpty(sCampos.ToString()))
                {
                    throw new System.ArgumentException($"Não foi possivel localizada alterações a serem salvas!", "Salvar informações de Usuário");
                }

                sSql = $"Update usuarios set {Environment.NewLine} " +
                    sCampos.ToString().Substring(2) +
                    $"Where Usu_Id = {Usu_Id} { Environment.NewLine} ";

                BdMySql.executaComando(sSql);
                Logs.Incluir(sLog.ToString());
            }
            else
            {
                if (
                    string.IsNullOrEmpty(Usu_Nome)
                    || string.IsNullOrEmpty(Usu_Login)
                    || string.IsNullOrEmpty(Usu_Senha)
                   )
                {
                    throw new System.ArgumentException("É obrigatório informar Nome, Login e Senha!", "Incluir informações de Usuário");
                }

                DataTable dt = GetDtUsuarios($"Where Usu_Login = {Usu_Login}");

                if (dt.Rows.Count > 0)
                {
                    throw new System.ArgumentException("Login inválido, já está em uso!", "Incluir Usuário");
                }

                sLog.AppendLine($"{Sistema.Usuario.Usu_Nome} Incluiu informações do Usuário");
                sLog.AppendLine($"Nome do Usuario => {Usu_Nome}");
                sLog.AppendLine($"Login do Usuario => {Usu_Login}");
                sLog.AppendLine($"Senha do Usuario!");
                if (Med_Id > 0)
                {
                    sLog.AppendLine($"Id do Médico => {Med_Id}");
                }

                sSql = $"Insert into Usuarios ( {Environment.NewLine} " +
                    $"    Med_Id {Environment.NewLine} " +
                    $"    , Usu_Nome {Environment.NewLine} " +
                    $"    , Usu_Login {Environment.NewLine} " +
                    $"    ,Usu_Senha {Environment.NewLine} " +
                    $") values( {Environment.NewLine} " +
                    $"    {Med_Id} {Environment.NewLine} " +
                    $"    , '{Usu_Nome}' {Environment.NewLine} " +
                    $"    , '{Usu_Login}' {Environment.NewLine} " +
                    $"    , '{Usu_Senha}' {Environment.NewLine} " +
                    $") {Environment.NewLine} ";

                Usu_Id = BdMySql.executaInsertRecuperaId(sSql);

            }
        }

        public bool Autenticar()
        {
            bool Autenticado = false;

            if (string.IsNullOrEmpty(Usu_Login) && string.IsNullOrEmpty(Usu_Senha))
            {
                throw new System.ArgumentException("Obrigatório informar o ID ou login do usuário", "Carregar Usuário");
            }
            else
            {
                string sWhere = $"Where Usu_Login = '{Usu_Login} and Usu_Senha = {Usu_Senha} {Environment.NewLine} ";

                DataTable dt = GetDtUsuarios(sWhere);

                if (dt.Rows.Count > 0)
                {
                    Usu_Id = Convert.ToInt32(dt.Rows[0]["Usu_Id"]);
                    Med_Id = Convert.ToInt32(dt.Rows[0]["Med_Id"]);
                    Usu_Nome = dt.Rows[0]["Usu_Nome"].ToString();
                    Usu_Login = dt.Rows[0]["Usu_Nome"].ToString();
                    Autenticado = true;
                }
            }

            return Autenticado;
        }

        public void GetUsuario()
        {
            if (Usu_Id <= 0 && string.IsNullOrEmpty(Usu_Login))
            {
                throw new System.ArgumentException("Obrigatório informar o ID ou login do usuário", "Carregar Usuário");
            }
            else
            {
                string sWhere = $" Where {(Usu_Id > 0 ? $"Usu_Id = {Usu_Id}" : $"Usu_Login = '{Usu_Login}")}  {Environment.NewLine} ";

                DataTable dt = GetDtUsuarios(sWhere);

                if (dt.Rows.Count > 0)
                {
                    Usu_Id = Convert.ToInt32(dt.Rows[0]["Usu_Id"]);
                    Med_Id = Convert.ToInt32(dt.Rows[0]["Med_Id"]);
                    Usu_Nome = dt.Rows[0]["Usu_Nome"].ToString();
                    Usu_Login = dt.Rows[0]["Usu_Nome"].ToString();
                }
                else
                {
                    throw new System.ArgumentException("Não foi possivel localizar o usuário com as informações fornecidas!", "Carregar Usuário");
                }
            }
        }

        public void Excluir()
        {
            if (Usu_Id <= 0)
            {
                throw new System.ArgumentException("Obrigatório informar o ID ", "Excluir Usuário");
            }
            else
            {
                GetUsuario();
                StringBuilder sLog = new StringBuilder();

                sLog.AppendLine($"{Sistema.Usuario.Usu_Nome} Incluiu informações do Usuário");
                sLog.AppendLine($"Nome do Usuario => {Usu_Nome}");
                sLog.AppendLine($"Login do Usuario => {Usu_Login}");
                sLog.AppendLine($"Senha do Usuario!");
                if (Med_Id > 0)
                {
                    sLog.AppendLine($"Id do Médico => {Med_Id}");
                }

                string sSql = $"Delete from usuarios {Environment.NewLine} " +
                    $"where Usu_Id = {Usu_Id} ";

                int QtdeReg = BdMySql.executaComando(sSql);

                if (QtdeReg == 0)
                { throw new System.ArgumentException($"Não foi possível localizar o Usuário com ID {Usu_Id}", "Excluir Usuário"); }
                else
                { Logs.Incluir(sLog.ToString()); }
            }
        }

        #endregion
    }
}