using System;
using Microsoft.Data.SqlClient; 

namespace PasswordVaultApp
{
    public static class DatabaseManager
    {
        private static SqlConnection connection = new SqlConnection("Server=DESKTOP-LS4I652;Database=PasswordVaultDB;Trusted_Connection=True;TrustServerCertificate=True;");

    }
}