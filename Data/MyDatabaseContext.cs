using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
namespace DotNetCoreSqlDb.Models
{
    public class MyDatabaseContext : DbContext
    {
        public MyDatabaseContext (DbContextOptions<MyDatabaseContext> options)
            : base(options)
        {
            //the following is used to connected to azure sql server when authentication via active directory is in use
            //var conn = (System.Data.SqlClient.SqlConnection)Database.GetDbConnection();
            //conn.AccessToken = (new Microsoft.Azure.Services.AppAuthentication.AzureServiceTokenProvider()).GetAccessTokenAsync("https://database.windows.net/").Result;
        }

        public DbSet<DotNetCoreSqlDb.Models.Todo> Todo { get; set; }
    }
}
