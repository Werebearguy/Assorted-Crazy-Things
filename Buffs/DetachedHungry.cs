using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Buffs
{
	public class DetachedHungry : ModBuff
		{
			public override void SetDefaults()
				{
					DisplayName.SetDefault("Detached Hungry");
					Description.SetDefault("You might be the next meal...");
					Main.buffNoTimeDisplay[Type] = true;
					Main.vanityPet[Type] = true;
				}
			public override void Update(Player player, ref int buffIndex)
				{
					player.buffTime[buffIndex] = 18000;
					player.GetModPlayer<MyPlayer>(mod).DetachedHungry = true;
					bool petProjectileNotSpawned = player.ownedProjectileCounts[mod.ProjectileType("DetachedHungry")] <= 0;
					if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer)
						{
							Projectile.NewProjectile(player.position.X + (float)(player.width / 2), player.position.Y + (float)(player.height / 2), 0f, 0f, mod.ProjectileType("DetachedHungry"), 0, 0f, player.whoAmI, 0f, 0f);
						}
				}
		}
}
