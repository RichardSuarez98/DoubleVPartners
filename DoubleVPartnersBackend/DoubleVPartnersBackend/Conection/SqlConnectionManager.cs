using System.Data;
using Microsoft.Data.SqlClient;
using Serilog;

namespace DoubleVPartnersBackend.Conection
{
    public class SqlConnectionManager
    {
        private readonly string? connectionString;

        public SqlConnectionManager(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrEmpty(connectionString))
            {
                Log.Warning("La cadena de conexión no está configurada.");
            }
        }


         public SqlConnection OpenConnection()
         {
             SqlConnection connection = new SqlConnection(connectionString);
             try
             {
                 connection.Open();
                 Log.Information("Conexión exitosa a la Base de Datos");

                 return connection;
             }
             catch (Exception ex)
             {
                 Log.Error($"Error al conectar: {ex.Message}");
                 return null;
             }
         }
         
        // Método para ejecutar un Stored Procedure con parámetros
        public DataTable ExecuteStoredProcedure(string storedProcedure, SqlParameter[] parameters)
        {
            try
            {
                using (SqlConnection connection = OpenConnection())
                {
                    if (connection == null)
                    {
                        return null;
                    }

                    SqlDataAdapter dataAdapter = new SqlDataAdapter(storedProcedure, connection);
                    dataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                    dataAdapter.SelectCommand.Parameters.AddRange(parameters);

                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);
                    return dataTable;
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Error al ejecutar el procedimiento almacenado: {ex.Message}");
                return null;
            }
        }








    }
}