using System.Data;

namespace DoubleVPartnersBackend.Utilidades
{
    public class DbNullHelper
    {
        // Método para obtener el valor de una columna y manejar DBNull
        public static T GetValue<T>(DataRow row, string columnName, T defaultValue = default(T), bool allowNull = false)
        {
            if (row[columnName] == DBNull.Value)
            {
                if (allowNull)
                    return default; // Devolvemos null si es un tipo nullable
                return defaultValue; // Devolvemos el valor por defecto
            }
            return (T)row[columnName]; // Si no es DBNull, devuelve el valor convertido
        }
    }
}
