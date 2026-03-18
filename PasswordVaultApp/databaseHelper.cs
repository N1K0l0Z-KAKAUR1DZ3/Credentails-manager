using System;
using Microsoft.Data.SqlClient;
using Microsoft.Identity.Client;

namespace PasswordVaultApp
{
    public static class DatabaseManager
    {
        private static string connectionString = "Server=DESKTOP-LS4I652;Database=PasswordVault;Trusted_Connection=True;TrustServerCertificate=True;";

        private delegate void dataBaseConfigurationLogic(SqlConnection sqlConnection);

        private static void TalkToDB(dataBaseConfigurationLogic dbModificationLogic)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                dbModificationLogic(connection);
            }
        }


        //getting commands
        public static Credentials? GetRecord(Credentials creds, SqlConnection sqlCon)
        {
            string sqlCommandSyntax = "SELECT CredentialID, GroupID, CredNmae, Username, Password FROM Credentials WHERE CredName = @name";

            using (SqlCommand cmd = new SqlCommand(sqlCommandSyntax, sqlCon))
            {
                cmd.Parameters.AddWithValue("@name", creds.Name);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Credentials
                        {
                            Id = reader.GetInt32(0),
                            GroupID = reader.GetInt32(1),
                            Name = reader.GetString(2),
                            Username = reader.GetString(3),
                            Password = reader.GetString(4)
                        };
                    }
                }
            }
            return null;
        }

        public static Group? GetRecord(Group group, SqlConnection sqlCon)
        {
            string sqlCommandSyntax = "SELECT GroupID, GroupName FROM Groups WHERE GroupName = @name";

            using (SqlCommand cmd = new SqlCommand(sqlCommandSyntax, sqlCon))
            {
                cmd.Parameters.AddWithValue("@name", group.Name);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Group(reader.GetInt32(0), reader.GetString(1));
                    }
                }
            }
            return null;
        }
        public static MainPassword? GetRecord(MainPassword creds, SqlConnection sqlCon)
        {
           string sqlCommandSyntax = "SELECT TOP 1 MainPasswordHash, Salt FROM MainPassword";

            using (SqlCommand cmd = new SqlCommand(sqlCommandSyntax, sqlCon))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new MainPassword(reader.GetInt32(0), reader.GetString(1), reader.GetString(2));
                    }
                }
            }
            return null;
        }


        //edditing commands
        public static void EditRecord(Credentials creds, SqlConnection sqlCon)
        {

        }
        public static void EditRecord(Group group, SqlConnection sqlCon)
        {

        }
        public static void EditRecord(MainPassword mainPass, SqlConnection sqlCon)
        {

        }


        //incrementing commands
        public static void IncrementRecord(Credentials creds, SqlConnection sqlCon)
        {

        }
        public static void IncrementRecord(Group group, SqlConnection sqlCon)
        {

        }

        public static void SetMainPassword(MainPassword mainPass, SqlConnection sqlCon)
        {

        }


        //delete commands
        public static void DeleteRecord(Group group, SqlConnection sqlCon)
        {

        }
        public static void DeleteRecord(Credentials creds, SqlConnection sqlCon)
        {

        }
    }
}