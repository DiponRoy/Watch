using System;
using TableDependency.Enums;
using TableDependency.EventArgs;
using TableDependency.SqlClient;

namespace SqlServerTableWatch
{
    public class TableWatch<Table> : IDisposable where Table : class, new()
    {
        public delegate void EntityChange(Table entity);

        public event EntityChange Inserted;
        public event EntityChange Updated;
        public event EntityChange Deleted;

        protected SqlTableDependency<Table> SqlTableDependency { get; set; }

        protected TableWatch(string connectionString)
        {
            SqlTableDependency = new SqlTableDependency<Table>(connectionString);
            SqlTableDependency.OnChanged += TableEntityChange;
        }

        private void TableEntityChange(object sender, RecordChangedEventArgs<Table> e)
        {
            var changedEntity = e.Entity;
            switch (e.ChangeType)
            {
                case ChangeType.None:
                    break;
                case ChangeType.Delete:
                    Deleted?.Invoke(changedEntity);
                    break;
                case ChangeType.Insert:
                    Inserted?.Invoke(changedEntity);
                    break;
                case ChangeType.Update:
                    Updated?.Invoke(changedEntity);
                    break;
                default:
                    break;
            }
        }

        public void Start()
        {
            SqlTableDependency.Start();

        }

        public void Stop()
        {
            SqlTableDependency.Stop();            
        }

        public void Dispose()
        {
            SqlTableDependency?.Dispose();
        }
    }
}