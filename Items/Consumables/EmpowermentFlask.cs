﻿using AssortedCrazyThings.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Consumables
{
    class EmpowermentFlask : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Empowerment Flask");
            Tooltip.SetDefault("Incrementally increases damage dealt over time"
                + "\nBonus resets upon taking damage"
                + "\n(Summon damage only increases marginally)");
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 30;
            item.useStyle = ItemUseStyleID.EatingUsing;
            item.useAnimation = 17;
            item.useTime = 17;
            item.useTurn = true;
            item.UseSound = SoundID.Item3;
            item.maxStack = 30;
            item.consumable = true;
            item.buffTime = 7200; //two minutes
            item.buffType = ModContent.BuffType<EmpoweringBuff>();
            item.rare = -11;
            item.value = Item.sellPrice(silver: 2);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.BottledWater, 1);
            recipe.AddIngredient(ModContent.ItemType<CaughtDungeonSoulFreed>(), 3);
            recipe.AddIngredient(ItemID.Bone, 10);
            recipe.AddTile(TileID.Bottles);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
