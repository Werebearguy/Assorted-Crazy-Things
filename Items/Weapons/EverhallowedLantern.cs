﻿using AssortedCrazyThings.Buffs;
using AssortedCrazyThings.Projectiles.Minions;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Weapons
{
    public class EverhallowedLantern : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Everhallowed Lantern");
            Tooltip.SetDefault("Summons a freed Dungeon Soul to fight for you.");
        }

        public override void SetDefaults()
        {
            //Defaults for damage, shoot and knockback dont matter too much here
            //default to PostWol
            item.damage = CompanionDungeonSoulMinionBase.DefDamage;
            item.summon = true;
            item.mana = 10;
            item.width = 26;
            item.height = 40;
            item.useTime = 30;
            item.useAnimation = 30;
            item.useStyle = 4; //4 for life crystal
            item.noMelee = true;
            item.value = Item.sellPrice(0, 0, 95, 0);
            item.rare = -11;
            item.UseSound = SoundID.Item44;
            item.shoot = mod.ProjectileType<CompanionDungeonSoulFrightMinion>();
            item.shootSpeed = 10f;
            item.knockBack = CompanionDungeonSoulMinionBase.DefKnockback;
            item.buffType = mod.BuffType<CompanionDungeonSoulMinionBuff>();
            item.buffTime = 3600;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true; //true
        }

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2 && player.itemTime == 0 && player.whoAmI == Main.myPlayer)
            {
                AssPlayer mPlayer = player.GetModPlayer<AssPlayer>(mod);
                CompanionDungeonSoulMinionBase.SoulStats stats = CompanionDungeonSoulMinionBase.GetAssociatedStats(mod, mPlayer.CycleSoulType()); //<- switch here
                item.damage = stats.Damage;
                item.shoot = stats.Type;
                item.knockBack = stats.Knockback;

                if (player.whoAmI == Main.myPlayer)
                {
                    CompanionDungeonSoulMinionBase.SoulType soulType = (CompanionDungeonSoulMinionBase.SoulType)stats.SoulType;
                    if(soulType == CompanionDungeonSoulMinionBase.SoulType.Dungeon)
                    {
                        CombatText.NewText(new Rectangle((int)player.position.X, (int)player.position.Y, player.width, player.height),
                         CombatText.HealLife, "Selected: " + soulType.ToString() + " Soul");
                    }
                    else
                    {
                        CombatText.NewText(new Rectangle((int)player.position.X, (int)player.position.Y, player.width, player.height),
                         CombatText.HealLife, "Selected: Soul of " + soulType.ToString());
                    }
                }
                return true;
            }
            return true;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if (player.altFunctionUse != 2)
            {
                AssPlayer mPlayer = player.GetModPlayer<AssPlayer>(mod);
                mPlayer.SpawnSoul(item.shoot, item.damage, item.knockBack);
            }
            return false;
        }

        public override void HoldItem(Player player)
        {
            player.itemLocation.X = player.Center.X;
            player.itemLocation.Y = player.Bottom.Y + 2f;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            //need a dummy because you can't remove elements from a list while you are iterating
            TooltipLine line = new TooltipLine(mod, "dummy", "dummy");

            foreach (TooltipLine line2 in tooltips)
            {
                if (line2.mod == "Terraria" && line2.Name == "BuffTime")
                {
                    line = line2;
                }
            }
            if(line.Name != "dummy") tooltips.Remove(line);


            int insertIndex = tooltips.Count; //it can insert on the "last" index (special case)

            for (int i = 0; i < tooltips.Count; i++)
            {
                if (tooltips[i].Name == "Tooltip0")
                {
                    insertIndex = i + 1; //it inserts "left" of where it found the index (without +1), so everything else get pushed one up
                    break;
                }
            }

            AssPlayer mPlayer = Main.LocalPlayer.GetModPlayer<AssPlayer>(mod);
            CompanionDungeonSoulMinionBase.SoulType soulType = (CompanionDungeonSoulMinionBase.SoulType)mPlayer.selectedSoulMinionType;
            if (soulType == CompanionDungeonSoulMinionBase.SoulType.Dungeon)
            {
                tooltips.Insert(insertIndex, new TooltipLine(mod, "Type", "Selected: " + soulType.ToString() + " Soul."));
            }
            else
            {
                tooltips.Insert(insertIndex, new TooltipLine(mod, "Type", "Selected: Soul of " + soulType.ToString() + "."));
            }
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.SoulofNight, 5);
            recipe.AddIngredient(ItemID.SoulofLight, 5);
            recipe.AddIngredient(mod.ItemType<EverglowLantern>(), 1);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
