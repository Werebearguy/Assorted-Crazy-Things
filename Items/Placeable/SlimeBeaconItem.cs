using AssortedCrazyThings.Tiles;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Placeable
{
    public class SlimeBeaconItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Slime Beacon");
            Tooltip.SetDefault("'Do The Slime With Me!'");
        }

        public override void SetDefaults()
        {
            item.width = 28;
            item.height = 28;
            item.maxStack = 99;
            item.useTurn = true;
            item.autoReuse = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.consumable = true;
            item.rare = -11;
            item.value = Item.buyPrice(0, 10, 0, 0);
            item.createTile = ModContent.TileType<SlimeBeaconTile>();
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient) tooltips.Add(new TooltipLine(mod, "Multi", "[c/FFA01D:DOES NOT WORK IN MULTIPLAYER]"));
        }
    }
}
