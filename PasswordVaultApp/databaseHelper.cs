using System;
using Microsoft.Data.SqlClient; 

namespace PasswordVaultApp
{
      public static class DatabaseManager
    {
        private static string connectionString = "Server=DESKTOP-LS4I652;Database=PasswordVaultDB;Trusted_Connection=True;TrustServerCertificate=True;";

        private delegate void dataBaseConfigurationLogic (SqlConnection sqlConnection);

        private static void TalkToDB(dataBaseConfigurationLogic dbModificationLogic)
        {
            using(SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                dbModificationLogic(connection);
            }
        }
    }
}