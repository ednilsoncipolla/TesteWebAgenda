using System;
using MySql.Data.MySqlClient;
using System.Data;

namespace WebAgenda.Models
{
    public static class BdMySql
    {
        public static string Servidor = "opmy0018.servidorwebfacil.com";
        public static string Usuario = "ednil_WebAgenda";
        public static string Senha = "PwdWebAgenda@10";
        public static string BancoDeDados = "ednilson1_webagenda";

        public static string sConexaoMySql
        {
            get
            {
                return "Data Source = " + Servidor + ";" +
                    "UserName = " + Usuario + ";" +
                    "Password = " + Senha + ";" +
                    "DataBase = " + BancoDeDados + ";";
            }
            internal set { }
        }


        public static int doExecutaComando(String sql)
        {
            MySqlConnection myConexao = null;
            MySqlCommand myComando = new MySqlCommand();
            try
            {

                myConexao = new MySqlConnection(sConexaoMySql);
                myConexao.Open();

                int retorno = 0;

                myComando.Connection = myConexao;
                myComando.CommandType = CommandType.Text;
                myComando.CommandText = sql;


                retorno = myComando.ExecuteNonQuery();

                return retorno;
            }
            catch (Exception exc)
            {
                string exc1 = exc.Message;
                return 0;
            }
            finally
            {
                if (myConexao != null)
                {
                    myConexao.Close();
                    myConexao = null;
                    myComando = null;
                }
            }
        }

        public static int executaComando(string sql)
        {
            using (MySqlConnection myConexao = new MySqlConnection(sConexaoMySql))
            {
                MySqlCommand myComando = new MySqlCommand();

                myConexao.Open();

                myComando.Connection = myConexao;
                myComando.CommandType = CommandType.Text;
                myComando.CommandText = sql;


                int retorno = 0;
                retorno = myComando.ExecuteNonQuery();

                return retorno;
            }
        }

        public static int executaInsertRecuperaId(String sql)
        {
            MySqlConnection myConexao = null;
            MySqlCommand myComando = new MySqlCommand();

            myConexao = new MySqlConnection(sConexaoMySql);
            myConexao.Open();

            myComando.Connection = myConexao;
            myComando.CommandType = CommandType.Text;
            myComando.CommandText = sql;

            myComando.CommandText = sql + "; Select LAST_INSERT_ID();";
            myComando.CommandTimeout = 10800;

            int retorno = Convert.ToInt32(myComando.ExecuteScalar());

            myConexao.Close();
            myConexao = null;
            myComando = null;

            return retorno;
        }

        public static DataTable getDataTable(String comandoSql)
        {
            MySqlConnection myConexao = null;
            try
            {
                MySqlDataAdapter daAdapter;
                myConexao = new MySqlConnection(sConexaoMySql);
                myConexao.Open();
                daAdapter = new MySqlDataAdapter(comandoSql, myConexao);

                DataTable dtResultado = new DataTable();
                daAdapter.Fill(dtResultado);

                return dtResultado;
            }
            finally
            {
                if (myConexao != null)
                {
                    myConexao.Close();
                    myConexao = null;
                }
            }

        }

        public static DataTable getTable(String sql)
        {
            MySqlConnection myConexao = null;
            try
            {
                MySqlDataAdapter daAdapter;
                myConexao = new MySqlConnection(sConexaoMySql);
                myConexao.Open();
                daAdapter = new MySqlDataAdapter(sql, myConexao);

                DataTable dtResultado = new DataTable();
                daAdapter.Fill(dtResultado);

                return dtResultado;
            }
            finally
            {
                if (myConexao != null)
                {
                    myConexao.Close();
                    myConexao = null;
                }
            }

        }
    }

}