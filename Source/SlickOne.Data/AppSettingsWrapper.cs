using System;
using System.Configuration;
using System.Collections.Specialized;
using System.Dynamic;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlickOne.Data
{
    /// <summary>
    /// READ Application Setting
    /// http://www.nullskull.com/faq/624/c-40-dynamic-programming-to-create-a-wrapper-for-reading-appsettings-section.aspx
    /// </summary>
    public class AppSettingsWrapper : DynamicObject
    {
        private NameValueCollection _items;

        public AppSettingsWrapper()
        {
            _items = ConfigurationManager.AppSettings;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = _items[binder.Name];
            return result != null;
        }
    }
}
