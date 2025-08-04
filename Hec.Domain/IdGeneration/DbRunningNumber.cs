using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hec.IdGeneration
{
    public class DbRunningNumber : IRunningNumber
    {
        private string connStr;

        public DbRunningNumber(string connectionStringName)
        {
            this.connStr = ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
        }

        protected SqlConnection GetConn()
        {
            return new SqlConnection(this.connStr);
        }

        public void InitializeStorage(bool deleteExisting = false)
        {
            using (var conn = GetConn())
            {
                var sql = @"
                    CREATE TABLE RunningNumber (
                        [Key] varchar(255) NOT NULL,
                        [Number] int NOT NULL,
                        PRIMARY KEY CLUSTERED 
                        (
                            [Key] ASC
                        ) ON [PRIMARY]
                    ) ON [PRIMARY];";

                if (deleteExisting)
                {
                    sql = @"
                        IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'RunningNumber') BEGIN
                            DROP TABLE RunningNumber  
                        END;" + sql;
                }
                else
                {
                    sql = @"
                        IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'RunningNumber') BEGIN
                            " + sql + @"
                        END";
                }

                var cmd = new SqlCommand(sql, conn);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        public string Next(INumberSpecification spec)
        {
            using (var conn = GetConn())
            {
                //NOTE:
                //  We use Isolation Level SERIALIZABLE (the strictest) to prevent multiple threads from 
                //  inserting row with the same key. (The default Isolation Level READ COMMITTED fails for this).
                //  It is also the slowest, but speed is low priority since there won't really be doing
                //  that much call to this method anyway in the application.
                //  Isolation Level reference: http://msdn.microsoft.com/en-us/library/ms173763(v=sql.105).aspx

                var cmd = new SqlCommand(String.Format(@"
                    SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;
                    BEGIN TRANSACTION;
                        UPDATE RunningNumber SET Number = Number+1 WHERE [Key] = '{0}'
                        IF @@ROWCOUNT=0
                            INSERT INTO RunningNumber VALUES ('{0}', 1);
                        SELECT Number FROM RunningNumber WHERE [Key]='{0}';
                    COMMIT TRANSACTION;", spec.GetKey()), conn);

                conn.Open();
                var number = (int)cmd.ExecuteScalar();
                conn.Close();

                return spec.Format(number);
            }
        }
    }
}
