using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;

namespace TUA.API.Injection
{
    class ModBrowserInjection
    {
        public static void PopulateModBrowser(MonoModExtraHook.orig_populatebrowser orig, object instance)
        {
            Object modBrowserInstance = StaticManager<Type>.GetItem("TMain").Assembly.GetType("Terraria.ModLoader.Interface")
                .GetField("modBrowser", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null);
            ErrorLogger.Log(modBrowserInstance);
            StaticManager<Type>.GetItem("TMain").Assembly.GetType("Terraria.ModLoader.UI.UIModBrowser").GetField("loading").SetValue(modBrowserInstance, true);
            StaticManager<Type>.GetItem("TMain").Assembly.GetType("Terraria.ModLoader.UI.UIModBrowser").GetField("_specialModPackFilterTitle", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(modBrowserInstance, "");
            //StaticManager<Type>.GetItem("TMain").Assembly.GetType("Terraria.ModLoader.UI.UIModBrowser").GetField("_specialModPackFilterTitle", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(modBrowserInstance, null );
            UITextPanel<String> reloadButton = (UITextPanel<String>) StaticManager<Type>.GetItem("TMain").Assembly.GetType("Terraria.ModLoader.UI.UIModBrowser")
                .GetField("reloadButton", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(modBrowserInstance);
            reloadButton.SetText(Language.GetTextValue("tModLoader.MBGettingData"));

            MethodInfo SetHeading = StaticManager<Type>.GetItem("TMain").Assembly.GetType("Terraria.ModLoader.UI.UIModBrowser")
                .GetMethod("SetHeading", BindingFlags.Instance | BindingFlags.NonPublic);

            SetHeading.Invoke(modBrowserInstance, new object[] {Language.GetTextValue("tModLoader.MenuModBrowser")});
            UIPanel uiPanel = (UIPanel) StaticManager<Type>.GetItem("TMain").Assembly.GetType("Terraria.ModLoader.UI.UIModBrowser")
                .GetField("uIPanel", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(modBrowserInstance);
            uiPanel.Append((UIElement)StaticManager<Type>.GetItem("TMain").Assembly.GetType("Terraria.ModLoader.UI.UIModBrowser")
                .GetField("uILoader", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(modBrowserInstance));
            /*uIPanel.Append(uILoader);*/

            UIList modList = (UIList) StaticManager<Type>.GetItem("TMain").Assembly.GetType("Terraria.ModLoader.UI.UIModBrowser")
                .GetField("modList").GetValue(modBrowserInstance);
            IList items = (IList) StaticManager<Type>.GetItem("TMain").Assembly.GetType("Terraria.ModLoader.UI.UIModBrowser")
                .GetField("items", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.DeclaredOnly).GetValue(modBrowserInstance);
            items.Clear();
            modList.Deactivate();
            try
            {
                ServicePointManager.Expect100Continue = false;
                string url = "http://javid.ddns.net/tModLoader/listmods.php";
                var values = new NameValueCollection
                {
                    { "modloaderversion", TUA.tModLoaderVersion2 },
                    { "platform", ModLoader.compressedPlatformRepresentation },
                };
                using (WebClient client = new WebClient())
                {
                    ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback((sender, certificate, chain, policyErrors) => { return true; });
                    Type eventType = typeof(WebClient);
                    EventInfo UploadCompleteValue = eventType.GetEvent("UploadValuesCompleted");
                    Type eventHandler = UploadCompleteValue.EventHandlerType;

                    MethodInfo methodToMakeDelegate = StaticManager<Type>.GetItem("TMain").Assembly
                        .GetType("Terraria.ModLoader.UI.UIModBrowser").GetMethod("UploadComplete",
                            BindingFlags.Public | BindingFlags.Instance);
                    ErrorLogger.Log("EventHandler : " + UploadCompleteValue);
                    ErrorLogger.Log("Method to add : " + methodToMakeDelegate);
                    Delegate d = Delegate.CreateDelegate(eventHandler, modBrowserInstance, methodToMakeDelegate);

                    MethodInfo addHandler = UploadCompleteValue.GetAddMethod();
                    Object[] addHandlerArgs = { d };
                    addHandler.Invoke(client, addHandlerArgs);
                    //client.UploadValuesCompleted += new UploadValuesCompletedEventHandler(d);
                    client.UploadValuesAsync(new Uri(url), "POST", values);
                }
            }
            catch (WebException e)
            {

                if (e.Status == WebExceptionStatus.Timeout)
                {
                    SetHeading.Invoke(modBrowserInstance, new object[] {Language.GetTextValue("tModLoader.MenuModBrowser") + " " + Language.GetTextValue("tModLoader.MBOfflineWithReason", Language.GetTextValue("tModLoader.MBBusy")) });
                    return;
                }
                if (e.Status == WebExceptionStatus.ProtocolError)
                {
                    var resp = (HttpWebResponse)e.Response;
                    if (resp.StatusCode == HttpStatusCode.NotFound)
                    {
                        SetHeading.Invoke(modBrowserInstance, new object[] {Language.GetTextValue("tModLoader.MenuModBrowser") + " " + Language.GetTextValue("tModLoader.MBOfflineWithReason", resp.StatusCode)});
                        return;
                    }
                    SetHeading.Invoke(modBrowserInstance, new object[] {Language.GetTextValue("tModLoader.MenuModBrowser") + " " + Language.GetTextValue("tModLoader.MBOfflineWithReason", resp.StatusCode)});
                    return;
                }
            }
            catch (Exception e)
            {
                Object[] obj = new object[] {e};
                
                typeof(ErrorLogger)
                    .GetMethod("LogModBrowserException", BindingFlags.NonPublic | BindingFlags.Static)
                    .Invoke(null, obj);
                return;
            }
        }
    }
}
