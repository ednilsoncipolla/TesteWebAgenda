using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

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

        public int Pac_Id { get => pac_Id; set => pac_Id = value; }
        public string Pac_Nome { get => pac_Nome; set => pac_Nome = value; }
        public string Pac_Email { get => pac_Email; set => pac_Email = value; }
        public string Pac_Telefones { get => pac_Telefones; set => pac_Telefones = value; }
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