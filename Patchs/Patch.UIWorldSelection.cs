using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.IO;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Utilities;

namespace TUA.Patchs
{
    internal static partial class Patch
    {
        private static Dictionary<Guid, TagCompound> TUAWorldData = new Dictionary<Guid, TagCompound>();
        private static void UIWorldListItemOnctor(On.Terraria.GameContent.UI.Elements.UIWorldListItem.orig_ctor orig, Terraria.GameContent.UI.Elements.UIWorldListItem self, WorldFileData data, int snappointindex)
        {
            orig(self, data, snappointindex);
            try
            {
                string path = Path.ChangeExtension(data.Path, ".twld");
                if (File.Exists(path))
                {
                    
                    var buf = FileUtilities.ReadAllBytes(path, data.IsCloudSave);
                    var tag = TagIO.FromStream(new MemoryStream(buf));
                    var list = tag.GetList<TagCompound>("modData").FirstOrDefault((TagCompound m) =>
                        m.Get<string>("mod") == "TUA" && m.Get<string>("name") == "TUAWorld");
                    if (list != null)
                    {
                        TUAWorldData.Add(data.UniqueId, tag.GetList<TagCompound>("modData").FirstOrDefault((TagCompound m) =>
                            m.Get<string>("mod") == "TUA" && m.Get<string>("name") == "TUAWorld"));
                    }
                }
            }
            catch (Exception e)
            {
                ErrorLogger.Log(e.Message);
            }
        }

        private static void UIWorldListItemOnDrawSelf(ILContext il)
        {
            ILCursor c = new ILCursor(il);

            int textStackID = 0;

            if (c.TryGotoNext(i => i.MatchStloc(out textStackID),
                i => i.MatchLdsfld(out _),
                i => i.MatchLdloc(out _),
                i => i.MatchCallvirt(out _)))
            {
                c.Index += 1;
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<Func<UIWorldListItem, string>>((uiItem) =>
                {
                    var data = (WorldFileData)typeof(UIWorldListItem).GetField("_data", BindingFlags.Instance | BindingFlags.NonPublic)
                        .GetValue(uiItem);
                    return TUAWorldData.ContainsKey(data.UniqueId) && TUAWorldData[data.UniqueId].Get<TagCompound>("data").GetByte("UltraMode") == 1
                        ?
                        Language.GetTextValue("Ultra")
                        :
                        (data.IsExpertMode)
                            ? Language.GetTextValue("UI.Expert")
                            : Language.GetTextValue("UI.Normal");
                    
                });
                c.Emit(OpCodes.Stloc, textStackID);
            }
        }

        //public override TagCompound Save()
        //{
        //    BitsByte bits = new BitsByte();

        //    bits[0] = UltraMode;
        //    bits[1] = EoADowned;
        //    bits[2] = Wasteland;
        //    bits[3] = EoCCutsceneFirstTimePlayed;
        //    bits[4] = RealisticTimeMode;
        //    return new TagCompound
        //    {
        //        ["Flags"] = (byte)bits,
        //        ["EoCCutscene"] = EoCDeathCount,
        //        ["_nextSnowflakeIncrement"] = _nextSnowflakeIncrement
        //    };
        //    //tc.Add("apocalypseMoon", apocalypseMoon);
        //}
    }
}
