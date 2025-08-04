using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Hec.Migrations
{
    public partial class Configuration
    {
        private void RunSql(HecContext db)
        {
            var asm = Assembly.GetExecutingAssembly();
            
            foreach (var resourceName in new[] { "files.sql" })
            {
                using (var stream = asm.GetManifestResourceStream("Hec.Migrations." + resourceName))
                using (var reader = new StreamReader(stream))
                {
                    string content = reader.ReadToEnd();
                    foreach (var sql in content.Split(new[] { "GO" }, StringSplitOptions.RemoveEmptyEntries)) /* watch out for the split placeholders */
                    {
                        if (!String.IsNullOrEmpty(sql))
                            db.Database.ExecuteSqlCommand(sql);
                    }
                }
            } 
        }
    }
}
