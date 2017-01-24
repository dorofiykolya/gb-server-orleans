using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Grains;
using Orleans.Runtime.Host;
using StorageProvider.UserSQLStorageProvider;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            var silo = new SiloHost("SILO");
            silo.InitializeOrleansSilo();
            silo.StartOrleansSilo();

            System.Console.ReadKey();
        }

        public class TestData
        {
            public string Name { get; set; }
            public string Data { get; set; }
            public int UserId { get; set; }
        }

        private static void Test()
        {
            var sqlconnBuilder = new SqlConnectionStringBuilder("Data Source=91.218.212.20;Initial Catalog=TheFunGame;Persist Security Info=True;User ID=sa;Password=\"d?yYev@,P=26P=RD\"");

            var sqlCon = new SqlConnection(sqlconnBuilder.ConnectionString);
            sqlCon.Open();
            sqlCon.Close();

            //READ

            using (var db = new UserDbContext(sqlconnBuilder.ConnectionString))
            {
                var obj = db.Set(typeof(UserInfoState)).Find(1);
            }

            //WRITE
            
            using (var db = new UserDbContext(sqlconnBuilder.ConnectionString))
            {
                db.Entry(new UserInfoState
                {
                    UserId = 1,
                    Name = "User_1"
                }).State = EntityState.Added;
                db.SaveChanges();
            }
        }
    }
}
