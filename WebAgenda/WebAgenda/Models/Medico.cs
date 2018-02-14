using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace WebAgenda.Models
{
    public class Medico
    {
        #region Atributos
        int med_Id = 0;
        string med_Nome;
        string med_Email;
        string med_Telefones;
        string med_Obs;

        public int Med_Id { get => med_Id; set => med_Id = value; }
        public string Med_Nome { get => med_Nome; set => med_Nome = value; }
        public string Med_Email { get => med_Email; set => med_Email = value; }
        public string Med_Telefones { get => med_Telefones; set => med_Telefones = value; }
        public string Med_Obs { get => med_Obs; set => med_Obs = value; }

        #endregion

        #region CRUD
        public DataTable GetDtMedicos(string pWhere)
        {
            DataTable dt = new DataTable();

            string sSql = $"Select {Environment.NewLine} " +
                $"  Med_Id {Environment.NewLine} " +
                $"  , Med_Nome {Environment.NewLine} " +
                $"  , Med_Email {Environment.NewLine} " +
                $"  , Med_Telefones {Environment.NewLine} " +
                $"  , Med_Obs {Environment.NewLine} " +
                $"from medicos {Environment.NewLine} " +
                (string.IsNullOrEmpty(pWhere) ? "" : $"where {pWhere} {Environment.NewLine} ");

            dt = BdMySql.getDataTable(sSql);

            return dt;
        }

        public List<Medico> GetListaMedicos(string pWhere)
        {
            List<Medico> lista = new List<Medico>();

            DataTable dt = GetDtMedicos(pWhere);

            Medico usu = new Medico
            {
                Med_Id = 0,
                Med_Nome = "Não é Médico",
                Med_Email = "",
                med_Telefones = "",
                Med_Obs = "",
            };


            foreach (DataRow dR in dt.Rows)
            {
                usu = new Medico
                {
                    Med_Id = Convert.ToInt32(dR["Med_Id"]),
                    Med_Nome = dR["Med_Nome"] == DBNull.Value ? "" : dR["Med_Nome"].ToString(),
                    Med_Email = dR["Med_Email"].ToString(),
                    med_Telefones = dR["med_Telefones"].ToString(),
                    Med_Obs = dR["Usu_Login"].ToString()
                };
                lista.Add(usu);
            }

            return lista;
        }

        public List<SelectListItem> GetListaMedicosCombo(int pMed_Id)
        {
            List<SelectListItem> lista = new List<SelectListItem>();

            DataTable dt = GetDtMedicos("");

            SelectListItem lstItem = new SelectListItem()
            {
                Value = 0.ToString(),
                Text = "Não é Médico",
                Selected = 0 == pMed_Id
            };

            lista.Add(lstItem);

            foreach (DataRow dR in dt.Rows)
            {
                lstItem = new SelectListItem()
                {
                    Value = dR["Med_Id"].ToString(),
                    Text = dR["Med_Nome"].ToString(), 
                    Selected = dR["Med_Id"].ToString() == pMed_Id.ToString()
                };
                lista.Add(lstItem);
            }

            return lista;
        }

        public void Gravar()
        {
            string sSql = "";
            StringBuilder sLog = new StringBuilder();

            if (
                string.IsNullOrEmpty(Med_Nome)
                && string.IsNullOrEmpty(Med_Email)
                && string.IsNullOrEmpty(Med_Telefones)
                && string.IsNullOrEmpty(Med_Obs)
               )
            {
                throw new System.ArgumentException("Não fora informado dados a serem salvos!", "Salvar informações do Médido");
            }

            Med_Obs = Med_Obs.Replace("'", "`");

            if (Med_Id > 0)
            {
                sLog.AppendLine($"{Sistema.Usuario.Usu_Nome} Alterou informações do Médico com ID {Med_Id} ");

                DataTable dtAntes = GetDtMedicos($"Where Usu_Id = {Med_Id}");

                if (dtAntes.Rows.Count == 0)
                {
                    throw new System.ArgumentException($"Não foi possivel localizar o Médido com ID {Med_Id}!", "Salvar informações de Médico");
                }

                StringBuilder sCampos = new StringBuilder();

                if (!string.IsNullOrEmpty(Med_Nome) && dtAntes.Rows[0]["Usu_Nome"].ToString() != Med_Nome.Trim())
                {
                    sCampos.AppendLine($", Usu_Nome = '{Med_Nome}' {Environment.NewLine} ");
                    sLog.AppendLine($"Nome do Médico de {dtAntes.Rows[0]["Usu_Nome"].ToString()} para {Med_Nome}");
                }

                if (!string.IsNullOrEmpty(Med_Email) && dtAntes.Rows[0]["Usu_Login"].ToString() != Med_Email.Trim())
                {
                    sCampos.AppendLine($", Med_Email = '{Med_Email}' {Environment.NewLine} ");
                    sLog.AppendLine($"e-Mails do Médico de {dtAntes.Rows[0]["Med_Email"].ToString()} para {Med_Email}");
                }

                if (!string.IsNullOrEmpty(Med_Telefones) && dtAntes.Rows[0]["Usu_Senha"].ToString() != Med_Telefones.Trim())
                {
                    sCampos.AppendLine($", Med_Telefones = '{Med_Telefones}' {Environment.NewLine} ");
                    sLog.AppendLine($"Teledones do Médico de {dtAntes.Rows[0]["Med_Telefones"].ToString()} para {Med_Telefones}");
                }

                if (!string.IsNullOrEmpty(Med_Obs) && dtAntes.Rows[0]["Med_Obs"].ToString() != Med_Obs.Trim())
                {
                    sCampos.AppendLine($", Med_Obs = '{Med_Obs}' {Environment.NewLine} ");
                    sLog.AppendLine($"Observações do Médico de [{dtAntes.Rows[0]["Med_Obs"].ToString()}] para [{Med_Obs}] ");
                }

                if (string.IsNullOrEmpty(sCampos.ToString()))
                {
                    throw new System.ArgumentException($"Não foi possivel localizada alterações a serem salvas!", "Salvar informações do Médico");
                }

                sSql = $"Update medicos set {Environment.NewLine} " +
                    sCampos.ToString().Substring(2) +
                    $"Where Med_Id = {Med_Id} { Environment.NewLine} ";

                BdMySql.executaComando(sSql);
                Logs.Incluir(sLog.ToString());
            }
            else
            {
                if (
                    string.IsNullOrEmpty(Med_Nome)
                    || string.IsNullOrEmpty(Med_Email)
                    || string.IsNullOrEmpty(Med_Telefones)
                   )
                {
                    throw new System.ArgumentException("É obrigatório informar Nome, Email e Telefones!", "Incluir informações do Médico");
                }

                sLog.AppendLine($"{Sistema.Usuario.Usu_Nome} Incluiu informações do Usuário");
                sLog.AppendLine($"Nome do Médico => {Med_Nome}");
                sLog.AppendLine($"e-Mail do Médico => {Med_Email}");
                sLog.AppendLine($"Teledones do Médico => {Med_Telefones}");
                sLog.AppendLine($"Observações para o Médico => { Med_Obs }");

                sSql = $"Insert into Usuarios ( {Environment.NewLine} " +
                    $"    Med_Id {Environment.NewLine} " +
                    $"    , Med_Nome {Environment.NewLine} " +
                    $"    , Med_Email {Environment.NewLine} " +
                    $"    , Med_Telefones {Environment.NewLine} " +
                    $"    , Med_Obs {Environment.NewLine} " +
                    $") values( {Environment.NewLine} " +
                    $"    {Med_Id} {Environment.NewLine} " +
                    $"    , '{Med_Nome}' {Environment.NewLine} " +
                    $"    , '{Med_Email}' {Environment.NewLine} " +
                    $"    , '{Med_Telefones}' {Environment.NewLine} " +
                    $"    , '{Med_Obs}' {Environment.NewLine} " +
                    $") {Environment.NewLine} ";

                Med_Id = BdMySql.executaInsertRecuperaId(sSql);

            }
        }

        public void GetUsuario()
        {
            if (Med_Id <= 0)
            {
                throw new System.ArgumentException("Obrigatório informar o ID Médido", "Carregar Médido");
            }
            else
            {
                string sWhere = $" Where Med_Id = {Med_Id} {Environment.NewLine} ";

                DataTable dt = GetDtMedicos(sWhere);

                if (dt.Rows.Count > 0)
                {
                    Med_Id = Convert.ToInt32(dt.Rows[0]["Med_Id"]);
                    Med_Nome = dt.Rows[0]["Med_Nome"].ToString();
                    Med_Email = dt.Rows[0]["Med_Email"].ToString();
                    Med_Obs = dt.Rows[0]["Med_Obs"].ToString();
                }
                else
                {
                    throw new System.ArgumentException("Não foi possivel localizar o Médico com as informações fornecidas!", "Carregar Médico");
                }
            }
        }

        public void Excluir()
        {
            if (Med_Id <= 0)
            {
                throw new System.ArgumentException("Obrigatório informar o ID ", "Excluir Médico");
            }
            else
            {
                GetUsuario();
                StringBuilder sLog = new StringBuilder();

                sLog.AppendLine($"{Sistema.Usuario.Usu_Nome} Incluiu informações do Médico");
                sLog.AppendLine($"Nome do Médico => {Med_Nome}");
                sLog.AppendLine($"e-Mail do Médico => {Med_Email}");
                sLog.AppendLine($"Telefones do Médico => {Med_Telefones}");
                sLog.AppendLine($"Obs. do Médico => {Med_Obs}");


                string sSql = $"Delete from medico {Environment.NewLine} " +
                    $"where Med_Id = {Med_Id} ";

                int QtdeReg = BdMySql.executaComando(sSql);

                if (QtdeReg == 0)
                { throw new System.ArgumentException($"Não foi possível localizar o Médico com ID {Med_Id}", "Excluir Médico"); }
                else
                { Logs.Incluir(sLog.ToString()); }
            }
        }

        #endregion
    }
}