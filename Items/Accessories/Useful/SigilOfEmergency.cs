﻿using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Accessories.Useful
{
    class SigilOfEmergency : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sigil of Emergency");
            Tooltip.SetDefault("Summons a temporary minion to help you upon reaching critical health" +
                "\nIncreases your max number of minions");
        }

        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 22;
            item.value = Item.sellPrice(0, 1, 0, 0);
            item.rare = -11;
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            //4
            if (4 * player.statLife < player.statLifeMax2)
            {
                player.GetModPlayer<AssPlayer>().tempSoulMinion = true;
            }
            player.maxMinions++;
        }
    }
}
