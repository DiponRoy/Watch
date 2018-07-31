using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlServerTableWatch
{
    public class OutboxDbContext : DbContext
    {
        public IDbSet<SmsOutbox> Sms { get; set; }

        public OutboxDbContext() : base("DbOutbox")
        {
            
        }

        static OutboxDbContext()
        {
            Database.SetInitializer<OutboxDbContext>(null);                             /*only mapp to db*/
            Database.SetInitializer(new CreateDatabaseIfNotExists<OutboxDbContext>());  /*create if not exists */
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();  /*table name Students to Student*/
        }
    }
}
