using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SqlServerTableWatch
{
    class Program
    {
        private static OutboxDbContext _db;
        private static SmsOutboxWatch _watch;

        static void Main(string[] args)
        {
            try
            {
                /*db create*/
                _db = new OutboxDbContext();
                _db.Configuration.AutoDetectChangesEnabled = false;
                _db.Sms.ToList();

                /*watch*/
                _watch = new SmsOutboxWatch();
                _watch.Inserted += Watch_Inserted;
                _watch.Updated += Watch_Updated;
                _watch.Deleted += Watch_Deleted;

                /*cmd*/
                Console.Write("Write cmd (start/stop): ");
                while (true)
                {
                    string cmd = Console.ReadLine();

                    bool close = false;
                    switch (cmd)
                    {
                        case "start":
                            _watch.Start();
                            Console.Write("watch started.");
                            break;
                        case "stop":
                            _watch.Stop();
                            Console.Write("watch stoped.");
                            close = true;
                            break;
                        default:
                            Console.WriteLine("unknone cmd");
                            break;
                    }
                    if (close)
                    {
                        break;
                    }
                }
            }
            catch (Exception exception)
            {
                Dispose();
                throw;
            }
            finally
            {
                Dispose();
            }
        }

        private static void PrintEnity(string title, SmsOutbox entity)
        {
            Console.WriteLine(title + " \t: " + JsonConvert.SerializeObject(entity));
        }

        private static void Watch_Inserted(SmsOutbox entity)
        {
            PrintEnity("Inserted", entity);

            /*send sms*/
            _db.Sms.Attach(entity);
            entity.SentDateTimeUtc = DateTime.UtcNow;
            _db.Entry(entity).State = EntityState.Modified;
            _db.SaveChanges();
            PrintEnity("Sms Process", entity);
        }

        private static void Watch_Updated(SmsOutbox entity)
        {
            PrintEnity("Updated", entity);
        }

        private static void Watch_Deleted(SmsOutbox entity)
        {
            PrintEnity("Deleted", entity);
        }

        private static void Dispose()
        {
            _db?.Dispose();
            _watch?.Dispose();
        }
    }
}
