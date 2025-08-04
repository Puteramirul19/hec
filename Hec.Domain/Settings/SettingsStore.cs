using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Hec.Settings
{
    public interface ISettings
    {
    }

    public class SettingsStore
    {
        private HecContext db;

        public SettingsStore(HecContext db)
        {
            this.db = db;
        }

        public async Task<T> Get<T>() where T: ISettings, new()
        {
            var type = typeof(T);
            var serializer = new XmlSerializer(type);

            var key = type.Name;

            var kv = await db.KeyValues.FindAsync(key);
            if (kv == null) return new T();

            var reader = new StringReader(kv.Value);
            return (T)serializer.Deserialize(reader);
        }

        public async Task Save(object obj)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");

            var type = obj.GetType();
            var serializer = new XmlSerializer(type);

            using (var writer = new StringWriter())
            {
                serializer.Serialize(writer, obj);
                
                var value = writer.ToString();
                var key = type.Name;

                var kv = db.KeyValues.Find(key);
                if (kv == null)
                {
                    kv = new KeyValue { Key = key, Value = value };
                    db.KeyValues.Add(kv);
                }
                else
                {
                    kv.Value = value;
                    db.Entry(kv).State = System.Data.Entity.EntityState.Modified;
                }

                await db.SaveChangesAsync();
            }
        }
    }
}
