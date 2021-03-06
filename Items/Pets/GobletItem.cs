using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
    public class GobletItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Goblet Battle Standard");
            Tooltip.SetDefault("Summons a tiny goblin to follow you");
        }

        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.ZephyrFish);
            item.shoot = mod.ProjectileType("GobletProj");
            item.buffType = mod.BuffType("GobletBuff");
            item.rare = -11;
            item.value = Item.sellPrice(silver: 10);
        }

        public override void UseStyle(Player player)
        {
            if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
            {
                player.AddBuff(item.buffType, 3600, true);
            }
        }
    }
}
