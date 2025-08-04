using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hec.Auth
{
    public class ActiveDirectoryService : IDirectoryService
    {
        private string path;
        private string userDomain;
        private string adminUsername;
        private string adminPassword;

        public ActiveDirectoryService(string path, string userDomain, string adminUsername, string adminPassword)
        {
            this.path = path;
            this.userDomain = userDomain;
            this.adminUsername = adminUsername;
            this.adminPassword = adminPassword;
        }

        private DirectoryEntry GetDirectoryEntry(string username, string password)
        {
            try
            {
                DirectoryEntry de = new DirectoryEntry();
                de.Path = this.path;
                de.AuthenticationType = AuthenticationTypes.Secure;
                de.Username = this.userDomain + @"\" + username;
                de.Password = password;

                return de;
            }
            catch (Exception)
            {
                DirectoryEntry de = new DirectoryEntry();
                return de;
            }
        }

        private SearchResultCollection SearchAccount(string authUsername, string authPassword, string searchUsername)
        {
            DirectoryEntry de = GetDirectoryEntry(authUsername, authPassword);

            DirectorySearcher deSearch = new DirectorySearcher();
            deSearch.SearchRoot = de;
            deSearch.Filter = "(SAMAccountName=" + searchUsername + ")";

            return deSearch.FindAll();
        }

        public DirectoryUser Authenticate(string username, string password)
        {
            try
            {
                var results = SearchAccount(username, password, username);

                if (results.Count > 0)
                {
                    return BuildDirectoryUser(results[0]);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }

            //try
            //{
            //    using (var ctx = new PrincipalContext(ContextType.Domain, "localhost:389", "DC=SMILS,DC=TNB,DC=NET", "SMILS\\Administrator", "&kZE7t8H6--"))
            //    {

            //        var canlogin = ctx.ValidateCredentials(username, password);

            //        return canlogin;
            //    }
            //}
            //catch(Exception err)
            //{
            //    Console.WriteLine(err.Message);
            //    return false;
            //}
        }

        private DirectoryUser BuildDirectoryUser(SearchResult result)
        {
            // NOTE:
            //  -SAMAccountName untuk staff id
            //  - Property name->description      
            //  1. displayName/name ->full name
            //  2. mail->email
            //  3. mobile->telephone number
            //  4. title->designation(optional not mandatory)
            //  5. thumbnailPhoto->gambar(type OctetString)

            var u = new DirectoryUser
            {
                StaffNo = ReadStringProperty(result, "SAMAccountName"),
                FullName = ReadStringProperty(result, "displayName"),
                Email = ReadStringProperty(result, "mail"),
                PhoneNumber = ReadStringProperty(result, "mobile"),
                OfficeNumber = ReadStringProperty(result, "office")
            };

            var bytes = ReadBytesProperty(result, "thumbnailPhoto");
            if (bytes != null && bytes.Length > 0)
            {
                u.Photo = "data:image/jpg;base64," + Convert.ToBase64String(bytes);
            }
            else
            {
                u.Photo = "";
            }

            return u;
        }

        public string ReadStringProperty(SearchResult result, string propertyName)
        {
            try
            {
                return (string)result.Properties[propertyName][0];
            }
            catch
            {
                return "";
            }
        }

        public byte[] ReadBytesProperty(SearchResult result, string propertyName)
        {
            try
            {
                return (byte[])result.Properties[propertyName][0];
            }
            catch
            {
                return new byte[] { };
            }
        }

        public DirectoryUser GetUserByStaffNo(string staffNo)
        {
            var results = SearchAccount(this.adminUsername, this.adminPassword, staffNo);

            if (results.Count > 0)
            {
                return BuildDirectoryUser(results[0]);
            }
            else
            {
                return null;
            }
        }
    }
}
