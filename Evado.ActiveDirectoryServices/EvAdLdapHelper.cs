using System;
using System.DirectoryServices;

namespace Evado.ActiveDirectoryServices
{
    public class EvAdLdapHelper
    {
        private const string LdapRootPath = "LDAP://192.168.10.53/dc=evado,dc=local";
		private const string Username = "evado";
        private const string Password = "Invision!1";
        public EvAdsUserProfile FindUserObjectById(string evUserId)
        {
            EvAdsUserProfile evUser = null;
            var deSearch = GetDirectorySearcher();
            deSearch.Filter = SearchFilter("user", evUserId);
            deSearch.PropertiesToLoad.Add("department");
            var result = deSearch.FindOne();

            DisplayProperties(result);

            SetProperty(result, "pager", "2222-2222-222");
            Console.ReadLine();
            DisplayProperties(result);
		
            Console.ReadLine();
			var deUser = GetDirectoryEntry(result);

            evUser = new EvAdsUserProfile(deUser.Properties["cn"].Value.ToString())
            {
                DisplayName = deUser.Properties["cn"].Value.ToString(),
                EmailAddress = deUser.Properties["mail"].Value.ToString(),
                VoiceTelephoneNumber = deUser.Properties["mobile"].Value.ToString()
            };

            return evUser;
        }

        private void DisplayProperties(SearchResult result)
        {
            if (result != null)
            {
                var deUser = GetDirectoryEntry(result);
                using (deUser)
                {
                    var properties = deUser.Properties.PropertyNames;
                    foreach (var property in properties)
                    {
                        Console.WriteLine(@"{0} : {1}", property, deUser.Properties[(string)property].Value.ToString());

                    }
                }
            }
        }

        private static DirectoryEntry GetDirectoryEntry(SearchResult result)
        {
            var deUser = new DirectoryEntry(result.Path, Username, Password, AuthenticationTypes.Secure);
            return deUser;
        }

        public string GetProperty(SearchResult result, string propertyName) 
        {
            string propertyValue = null;
            if (result != null)
            {
                var deUser = GetDirectoryEntry(result);
                using (deUser)
                {
                    propertyValue = deUser.Properties[propertyName].Value.ToString();
                }
            }
            return propertyValue;
        }

        public void SetProperty(SearchResult result, string propertyName, string newValue)
        {
            if (result != null)
            {
                var deUser = GetDirectoryEntry(result);
                using (deUser)
                {
                    deUser.Properties[propertyName].Value = newValue;
                    deUser.CommitChanges();
                }
            }
			
        }

        public EvAdsGroupProfile FindGroupObjectByName(string evGroupName)
        {
            EvAdsGroupProfile evGroup = null;

            var deSearch = GetDirectorySearcher();
            deSearch.Filter = SearchFilter("group", evGroupName);
            var result = deSearch.FindOne();

            if (result != null)
            {
                var deGroup = new DirectoryEntry(result.Path);
                using (deGroup)
                {
                    var name = deGroup.Properties["Name"].Value as string;
                    evGroup = new EvAdsGroupProfile()
                        {
                            Name = name
                        };
                }
            }

            return evGroup;
        }

        private DirectorySearcher GetDirectorySearcher()
        {
            var de = new DirectoryEntry();
            de.Path = LdapRootPath;
            de.Username = "evado";
            de.Password = "Invision!1";
            de.AuthenticationType = AuthenticationTypes.Secure;

            var deSearch = new DirectorySearcher();

            deSearch.SearchRoot = de;

            return deSearch;
        }

        private string SearchFilter(string objectType, string cn)
        {
            return string.Format("(&(objectClass={0}) (cn={1}))", objectType, cn);
        }
    }
}