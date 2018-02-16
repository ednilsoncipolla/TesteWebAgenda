using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace WebAgenda.Models
{
    public class Paciente
    {
        #region Atributos
        int pac_Id = 0;
        string pac_Nome;
        string pac_Email;
        string pac_Telefones;
        string pac_Obs;

        [Display(Name = "ID")]
        public int Pac_Id { get => pac_Id; set => pac_Id = value; }

        [Display(Name = "Nome do Paciente", Prompt = "Nome do Paciente")]
        [Required(ErrorMessage = "Informe o nome do paciente")]
        public string Pac_Nome { get => pac_Nome; set => pac_Nome = value; }

        [Display(Name = "e-Mail do Paciente")]
        [DataType(DataType.EmailAddress)]
        public string Pac_Email { get => pac_Email; set => pac_Email = value; }

        [Display(Name = "Telefones do Paciente")]
        [Required(ErrorMessage = "Informe o(s) telefone(s) do paciente")]
        public string Pac_Telefones { get => pac_Telefones; set => pac_Telefones = value; }

        [Display(Name = "Observações sobre o Paciente")]
        public string Pac_Obs { get => pac_Obs; set => pac_Obs = value; }

        #endregion

        #region CRUD
        public DataTable GetDtPacientes(string pWhere)
        {
            DataTable dt = new DataTable();

            string sSql = $"Select {Environment.NewLine} " +
                $"  Pac_Id {Environment.NewLine} " +
                $"  , Pac_Nome {Environment.NewLine} " +
                $"  , Pac_Email {Environment.NewLine} " +
                $"  , Pac_Telefones {Environment.NewLine} " +
                $"  , Pac_Obs {Environment.NewLine} " +
                $"from Pacientes {Environment.NewLine} " +
                (string.IsNullOrEmpty(pWhere) ? "" : $"where {pWhere} {Environment.NewLine} ");

            dt = BdMySql.getDataTable(sSql);

            return dt;
        }

        public List<Paciente> GetListaPacientes(string pWhere)
        {
            List<Paciente> lista = new List<Paciente>();

            DataTable dt = GetDtPacientes(pWhere);

            foreach (DataRow dR in dt.Rows)
            {
                Paciente pac = new Paciente
                {
                    Pac_Id = Convert.ToInt32(dR["Pac_Id"]),
                    Pac_Nome = dR["Pac_Nome"].ToString(),
                    Pac_Email = dR["Pac_Email"].ToString(),
                    Pac_Telefones = dR["Pac_Telefones"].ToString(),
                    Pac_Obs = dR["Pac_Obs"].ToString()
                };
                lista.Add(pac);
            }

            return lista;
        }

        public List<SelectListItem> GetListaMedicosCombo(int pMed_Id)
        {
            List<SelectListItem> lista = new List<SelectListItem>();

            DataTable dt = GetDtPacientes("");

            foreach (DataRow dR in dt.Rows)
            {
                SelectListItem lstItem = new SelectListItem()
                {
                    Value = dR["Pac_Id"].ToString(),
                    Text = dR["Pac_Nome"].ToString(),
                    Selected = dR["Pac_Id"].ToString() == pMed_Id.ToString()
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
                string.IsNullOrEmpty(Pac_Nome)
                && string.IsNullOrEmpty(Pac_Email)
                && string.IsNullOrEmpty(Pac_Telefones)
                && string.IsNullOrEmpty(Pac_Obs)
               )
            {
                throw new System.ArgumentException("Não fora informado dados a serem salvos!", "Salvar informações do Passiente");
            }

            Pac_Obs = Pac_Obs.Replace("'", "`");

            if (Pac_Id > 0)
            {
                sLog.AppendLine($"{Sistema.Usuario.Usu_Nome} Alterou informações do Passiente com ID {Pac_Id} ");

                DataTable dtAntes = GetDtPacientes($"Where Pass_Id = {Pac_Id}");

                if (dtAntes.Rows.Count == 0)
                {
                    throw new System.ArgumentException($"Não foi possivel localizar o Paciente com ID {Pac_Id}!", "Salvar informações de Paciente");
                }

                StringBuilder sCampos = new StringBuilder();

                if (!string.IsNullOrEmpty(Pac_Nome) && dtAntes.Rows[0]["Pac_Nome"].ToString() != Pac_Nome.Trim())
                {
                    sCampos.AppendLine($", Pac_Nome = '{Pac_Nome}' {Environment.NewLine} ");
                    sLog.AppendLine($"Nome do Paciente de {dtAntes.Rows[0]["Pac_Nome"].ToString()} para {Pac_Nome}");
                }

                if (!string.IsNullOrEmpty(Pac_Email) && dtAntes.Rows[0]["Pac_Email"].ToString() != Pac_Email.Trim())
                {
                    sCampos.AppendLine($", Pac_Email = '{Pac_Email}' {Environment.NewLine} ");
                    sLog.AppendLine($"e-Mails do Paciente de {dtAntes.Rows[0]["Pac_Email"].ToString()} para {Pac_Email}");
                }

                if (!string.IsNullOrEmpty(Pac_Telefones) && dtAntes.Rows[0]["Pac_Telefones"].ToString() != Pac_Telefones.Trim())
                {
                    sCampos.AppendLine($", Pac_Telefones = '{Pac_Telefones}' {Environment.NewLine} ");
                    sLog.AppendLine($"Teledones do Paciente de {dtAntes.Rows[0]["Pac_Telefones"].ToString()} para {Pac_Telefones}");
                }

                if (!string.IsNullOrEmpty(Pac_Obs) && dtAntes.Rows[0]["Pac_Obs"].ToString() != Pac_Obs.Trim())
                {
                    sCampos.AppendLine($", Pac_Obs = '{Pac_Obs}' {Environment.NewLine} ");
                    sLog.AppendLine($"Observações do Paciente de [{dtAntes.Rows[0]["Pac_Obs"].ToString()}] para [{Pac_Obs}] ");
                }

                if (string.IsNullOrEmpty(sCampos.ToString()))
                {
                    throw new System.ArgumentException($"Não foi possivel localizada alterações a serem salvas!", "Salvar informações do Paciente");
                }

                sSql = $"Update Pacientes set {Environment.NewLine} " +
                    sCampos.ToString().Substring(2) +
                    $"Where Pac_Id = {Pac_Id} { Environment.NewLine} ";

                BdMySql.executaComando(sSql);
                Logs.Incluir(sLog.ToString());
            }
            else
            {
                if (
                    string.IsNullOrEmpty(Pac_Nome)
                    || string.IsNullOrEmpty(Pac_Email)
                    || string.IsNullOrEmpty(Pac_Telefones)
                   )
                {
                    throw new System.ArgumentException("É obrigatório informar Nome, Email e Telefones do paciente!", "Incluir informações do Paciente");
                }

                sLog.AppendLine($"{Sistema.Usuario.Usu_Nome} Incluiu informações do Paciente");
                sLog.AppendLine($"Nome do Paciente => {Pac_Nome}");
                sLog.AppendLine($"e-Mail do Paciente => {Pac_Email}");
                sLog.AppendLine($"Teledones do Paciente => {Pac_Telefones}");
                sLog.AppendLine($"Observações para o Paciente => { Pac_Obs }");

                sSql = $"Insert into Pacientes ( {Environment.NewLine} " +
                    $"    Pac_Id {Environment.NewLine} " +
                    $"    , Pac_Nome {Environment.NewLine} " +
                    $"    , Pac_Email {Environment.NewLine} " +
                    $"    , Pac_Telefones {Environment.NewLine} " +
                    $"    , Pac_Obs {Environment.NewLine} " +
                    $") values( {Environment.NewLine} " +
                    $"    {Pac_Id} {Environment.NewLine} " +
                    $"    , '{Pac_Nome}' {Environment.NewLine} " +
                    $"    , '{Pac_Email}' {Environment.NewLine} " +
                    $"    , '{Pac_Telefones}' {Environment.NewLine} " +
                    $"    , '{Pac_Obs}' {Environment.NewLine} " +
                    $") {Environment.NewLine} ";

                Pac_Id = BdMySql.executaInsertRecuperaId(sSql);

            }
        }

        public void GetPaciente()
        {
            if (Pac_Id <= 0)
            {
                throw new System.ArgumentException("Obrigatório informar o ID Paciente");
            }
            else
            {
                string sWhere = $" Where Pac_Id = {Pac_Id} {Environment.NewLine} ";

                DataTable dt = GetDtPacientes(sWhere);

                if (dt.Rows.Count > 0)
                {
                    Pac_Id = Convert.ToInt32(dt.Rows[0]["Pac_Id"]);
                    Pac_Nome = dt.Rows[0]["Pac_Nome"].ToString();
                    Pac_Email = dt.Rows[0]["Pac_Email"].ToString();
                    Pac_Obs = dt.Rows[0]["Pac_Obs"].ToString();
                }
                else
                {
                    throw new System.ArgumentException("Não foi possivel localizar o Médico com as informações fornecidas!");
                }
            }
        }

        public void Excluir()
        {
            if (Pac_Id <= 0)
            {
                throw new System.ArgumentException("Obrigatório informar o ID ");
            }
            else
            {
                GetPaciente();
                StringBuilder sLog = new StringBuilder();

                sLog.AppendLine($"{Sistema.Usuario.Usu_Nome} Incluiu informações do Médico");
                sLog.AppendLine($"Nome do Paciente => {Pac_Nome}");
                sLog.AppendLine($"e-Mail do Paciente => {Pac_Email}");
                sLog.AppendLine($"Telefones do Paciente => {Pac_Telefones}");
                sLog.AppendLine($"Obs. do Paciente => {Pac_Obs}");


                string sSql = $"Delete from pacientes {Environment.NewLine} " +
                    $"where Pac_Id = {Pac_Id} ";

                int QtdeReg = BdMySql.executaComando(sSql);

                if (QtdeReg == 0)
                { throw new System.ArgumentException($"Não foi possível localizar o Paciente com ID {Pac_Id}"); }
                else
                { Logs.Incluir(sLog.ToString()); }
            }
        }

        #endregion
    }
}