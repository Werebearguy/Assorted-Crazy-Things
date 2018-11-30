using Terraria;
using Terraria.ModLoader;

namespace Harblesnargits_Mod_01.Buffs
{
	public class CursedSkull : ModBuff
		{
			public override void SetDefaults()
				{
					DisplayName.SetDefault("Cursed Skull");
					Description.SetDefault("It won't curse you, I promise.");
					Main.buffNoTimeDisplay[Type] = true;
					Main.vanityPet[Type] = true;
				}
			public override void Update(Player player, ref int buffIndex)
				{
					player.buffTime[buffIndex] = 18000;
					player.GetModPlayer<MyPlayer>(mod).CursedSkull = true;
					bool petProjectileNotSpawned = player.ownedProjectileCounts[mod.ProjectileType("CursedSkull")] <= 0;
					if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer)
						{
							Projectile.NewProjectile(player.position.X + (float)(player.width / 2), player.position.Y + (float)(player.height / 2), 0f, 0f, mod.ProjectileType("CursedSkull"), 0, 0f, player.whoAmI, 0f, 0f);
						}
				}
		}
}
