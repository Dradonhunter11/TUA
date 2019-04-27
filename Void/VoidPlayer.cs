using System;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using TUA.Enum;
using TUA.Utilities;

namespace TUA.Void
{
    internal class VoidPlayer : ModPlayer
    {
        public int voidAffinity = 0;
        public int maxVoidAffinity = 100;

        /// <summary>
        /// Tier 2 allow the player to get a max of 200 void affinity, which is enough for a tier 2 artefact.
        /// </summary>
        public bool voidTier2Unlocked = false;
        /// <summary>
        /// Tier 3 allow the player to get a max of 500 void affinity, which is enough for a tier 3 artefact.
        /// </summary>
        public bool voidTier3Unlocked = false;
        /// <summary>
        /// Tier 4 allow the player to get a max of 1200 void affinity, which is enough for a tier 4 artefact,
        /// Also really dangerous to yield above 1000, so void generator might be the best thing to get it.
        /// </summary>
        public bool voidTier4Unlocked = false; //Secret tier

        public override TagCompound Save()
        {
            return new TagCompound()
            {
                ["tier2"] = voidTier2Unlocked,
                ["tier3"] = voidTier3Unlocked,
                ["tier4"] = voidTier4Unlocked,
                ["affinity"] = voidAffinity
            };

        }

        public override void Load(TagCompound tag)
        {
            voidTier2Unlocked = tag.GetBool("tier2");
            voidTier3Unlocked = tag.GetBool("tier3");
            voidTier4Unlocked = tag.GetBool("tier4");
            voidAffinity = tag.GetInt("affinity");

            UIManager.OpenVoidUI();
        }

        public override void PreUpdate()
        {
            if (maxVoidAffinity < 200 && voidTier2Unlocked)
            {
                maxVoidAffinity = 200;
            }
            else if (maxVoidAffinity < 500 && voidTier3Unlocked)
            {
                maxVoidAffinity = 500;
            }
            else if (maxVoidAffinity < 1200 && voidTier4Unlocked)
            {
                maxVoidAffinity = 1200;
            }
        }

        public override void PostUpdate()
        {
            if (voidAffinity >= 100 && voidTier2Unlocked)
            {

            }
        }

        public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
        {
            ModPacket packet = mod.GetPacket();
            packet.Write((byte)TUANetMessage.VoidPlayerSync);
            packet.Write((byte)player.whoAmI);
            packet.Write(voidAffinity);
            packet.Write(maxVoidAffinity);
            packet.Write(voidTier2Unlocked);
            packet.Write(voidTier3Unlocked);
            packet.Write(voidTier4Unlocked);
            packet.Send(toWho, fromWho);
        }

        public void AddVoidAffinity(int voidAffinity)
        {
            this.voidAffinity = Math.Min(this.voidAffinity + voidAffinity, maxVoidAffinity);
        }
    }
}
