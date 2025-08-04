using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hec.IdGeneration
{
    public class InMemoryRunningNumber : IRunningNumber
    {
        private IDictionary<string, int> storage = new Dictionary<string, int>();

        public InMemoryRunningNumber()
        {
        }
        
        public void InitializeStorage(bool deleteExisting = false)
        {
            storage = new Dictionary<string, int>();
        }
                
        public string Next(INumberSpecification spec)
        {
            var key = spec.GetKey();
            if (!storage.ContainsKey(key))
            {
                storage.Add(key, 0);
            }

            storage[key]++;
            return spec.Format(storage[key]);
        }
    }
}
