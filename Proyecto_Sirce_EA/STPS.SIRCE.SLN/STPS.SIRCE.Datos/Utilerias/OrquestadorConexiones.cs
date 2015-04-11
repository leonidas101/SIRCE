using STPS.SIRCE.Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STPS.SIRCE.Datos
{
    public class OrquestadorConexiones
    {
        public DataTable ObtenerConsulta(Enumeradores.ConexionesSatelitales conexion, string Query)
        {
            SqlConnection con = new SqlConnection();
            switch (conexion)
            {
                case Enumeradores.ConexionesSatelitales.DNE:
                    con.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DNEDB"].ConnectionString;
                    break;
                case Enumeradores.ConexionesSatelitales.CI:
                    con.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["CIDB"].ConnectionString;
                    break;
            }
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = Query;
            cmd.CommandType = CommandType.Text;
            DataSet ds = new DataSet();
            using (SqlDataAdapter sda = new SqlDataAdapter())
            {
                cmd.Connection = con;
                sda.SelectCommand = cmd;
                sda.Fill(ds);
                con.Close();
                con.Dispose();
            }
            return ds.Tables[0];
        }
    }
}
