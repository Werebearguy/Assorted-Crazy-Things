using AssortedCrazyThings.Projectiles.Pets;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs
{
    public class DocileDemonEyeBuff : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Docile Demon Eye");
            Description.SetDefault("A Demon Eye is following you."
                + "\nChange its appearance with a Demon Eye Contact Case.");
            Main.buffNoTimeDisplay[Type] = true;
            Main.vanityPet[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.buffTime[buffIndex] = 18000;
            AssPlayer mPlayer = player.GetModPlayer<AssPlayer>(mod);
            player.GetModPlayer<PetPlayer>(mod).DocileDemonEye = true;
            bool petProjectileNotSpawned = player.ownedProjectileCounts[mod.ProjectileType("DocileDemonEyeProj")] <= 0;
            if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer)
            {
                int i = Projectile.NewProjectile(player.position.X + (player.width / 2), player.position.Y + (player.height / 2), 0f, 0f, mod.ProjectileType("DocileDemonEyeProj"), 0, 0f, player.whoAmI, 0f, 0f);
                Main.projectile[i].GetGlobalProjectile<DocileEyeProj>(mod).SetEyeType(mPlayer.eyePetType);
                mPlayer.eyePetIndex = i;
            }
        }
    }
}