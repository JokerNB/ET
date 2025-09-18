/** This is an automatically generated class by FUICodeSpawner. Please do not modify it. **/

using FairyGUI;

namespace ET.Client
{
    [EnableClass]
    public class LoadingBinder
    {
        public static void BindAll()
        {
            UIObjectFactory.SetPackageItemExtension(ET.Client.Loading.FUI_LoadingUI.URL, typeof(ET.Client.Loading.FUI_LoadingUI));
            UIObjectFactory.SetPackageItemExtension(ET.Client.Loading.FUI_LoadingProgress.URL, typeof(ET.Client.Loading.FUI_LoadingProgress));
        }
    }
}
