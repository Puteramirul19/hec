using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hec.Auth
{
    public interface IDirectoryService
    {
        DirectoryUser GetUserByStaffNo(string staffNo);
        DirectoryUser Authenticate(string staffNo, string password);
    }
}
