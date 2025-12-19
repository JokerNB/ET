/** This is an automatically generated class by FUICodeSpawner. Please do not modify it. **/

using System.Collections.Generic;
using FairyGUI.Dynamic;

namespace ET.Client
{
    [EnableClass]
    public sealed class UIPackageMapping : IUIPackageHelper
    {
        private readonly Dictionary<string, string> m_PackageIdToNameMap = new()
        {
            {"0mie2hot", "UICoinShop"},
            {"0oxdmt1z", "UICommon"},
            {"046ui8np", "UIFont"},
            {"kjwej5kn", "UIMain"},
            {"0rrl8mre", "UISettings"},
            // <last line>
        };

        public string GetPackageNameById(string id)
        {
            return m_PackageIdToNameMap.TryGetValue(id, out var packageName) ? packageName : null;
        }
    }
}
