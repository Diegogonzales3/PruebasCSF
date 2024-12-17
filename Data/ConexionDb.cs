using System.Data;
using Microsoft.Data.SqlClient;

namespace PruebasCSF.Data
{
    public static class ConexionDb
    {

        public static string CadenaConexion = "Server=localhost;Database=BibliotecaDb;User Id=sa;Password=u202420050;TrustServerCertificate=True;";

        public static DataTable EjecutarConsulta(string query, List<SqlParameter> parametros = null)
        {
            using (SqlConnection conexion = new SqlConnection(CadenaConexion))
            {
                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    if (parametros != null)
                    {
                        foreach (var parametro in parametros)
                        {
                            comando.Parameters.Add(parametro);
                        }
                    }

                    SqlDataAdapter adaptador = new SqlDataAdapter(comando);
                    DataTable tabla = new DataTable();
                    adaptador.Fill(tabla);
                    return tabla;
                }
            }
        }

        // Método genérico para ejecutar comandos (INSERT, UPDATE, DELETE)
        public static int EjecutarComando(string query, List<SqlParameter> parametros = null)
        {
            using (SqlConnection conexion = new SqlConnection(CadenaConexion))
            {
                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    if (parametros != null)
                    {
                        foreach (var parametro in parametros)
                        {
                            comando.Parameters.Add(parametro);
                        }
                    }

                    conexion.Open();
                    int filasAfectadas = comando.ExecuteNonQuery();
                    return filasAfectadas;
                }
            }
        }
    }
}
