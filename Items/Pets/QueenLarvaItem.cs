using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Pets
{
    public class QueenLarvaItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Queen Larva");
            Tooltip.SetDefault("Summons a Queen Bee Larva to follow you"
                + "\nAppearance can be changed with Costume Suitcase");
        }

        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.ZephyrFish);
            item.shoot = mod.ProjectileType("QueenLarvaProj");
            item.buffType = mod.BuffType("QueenLarvaBuff");
            item.width = 28;
            item.height = 32;
            item.rare = -11;
            item.value = Item.sellPrice(copper: 10);
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
