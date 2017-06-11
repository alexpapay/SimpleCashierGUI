using CashiersTerminal.Models;
using MySql.Data.Entity;
using MySql.Data.MySqlClient;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace CashiersTerminal.Database
{
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(string connectionString) : base(connectionString) {}

        public DbSet<Users> Users { get; set; }
        public DbSet<SalesTable> SalesTable { get; set; }
        public DbSet<StoreTable> StoreTable { get; set; }
        public DbSet<Docs> Docs { get; set; }
    }

    public class ContextFactory : IDbContextFactory<DatabaseContext>
    {
        private static string _connectionString;

        public static void SetConnectionParameters(string serverAddress, string username, string password,
            string database, uint port = 3306)
        {
            var connectionStringBuilder = new MySqlConnectionStringBuilder
            {
                Server = serverAddress,
                UserID = username,
                Password = password,
                Database = database,
                Port = port
            };

            _connectionString = connectionStringBuilder.ToString();
        }

        private static DatabaseContext _instance;

        public static DatabaseContext Instance
        {
            get
            {
                if (_instance != null) return _instance;
                return _instance = new ContextFactory().Create();
            }
            private set { }
        }

        public DatabaseContext Create()
        {
            if (string.IsNullOrEmpty(_connectionString))
                throw new InvalidOperationException(
                    "[ОШИБКА] Установите параметры соединения перед подключением к БД.");

            return new DatabaseContext (_connectionString);
        }
    }
}
