﻿using AssortedCrazyThings.Buffs;
using AssortedCrazyThings.Projectiles.Minions.CompanionDungeonSouls;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Weapons
{
    public class EverglowLantern : MinionItemBase
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Everglow Lantern");
            Tooltip.SetDefault("Summons two freed Dungeon Souls at a time to fight for you\nEach Dungeon Soul occupies only half a minion slot");
        }

        public override void SetDefaults()
        {
            //Defaults for damage, shoot and knockback dont matter too much here
            //default to PreWol
            item.damage = EverhallowedLantern.BaseDmg / 2 - 1;
            item.summon = true;
            item.mana = 10;
            item.width = 18;
            item.height = 38;
            item.useTime = 36;
            item.useAnimation = 36;
            item.useStyle = ItemUseStyleID.HoldingUp; //4 for life crystal
            item.noMelee = true;
            item.value = Item.sellPrice(0, 0, 75, 0);
            item.rare = -11;
            item.UseSound = SoundID.Item44;
            item.shoot = ModContent.ProjectileType<CompanionDungeonSoulPreWOFMinion>();
            item.shootSpeed = 10f;
            item.knockBack = EverhallowedLantern.BaseKB;
            item.buffType = ModContent.BuffType<CompanionDungeonSoulMinionBuff>();
        }

        public override bool AltFunctionUse(Player player)
        {
            return false; //true
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            //one that shoots out far 
            Projectile.NewProjectile(player.Center.X + player.direction * 8f, player.Bottom.Y - 12f, player.velocity.X + player.direction * 1.5f, player.velocity.Y - 1f, type, damage, knockBack, Main.myPlayer, 0f, 0f);
            //one that shoots out less
            Projectile.NewProjectile(player.Center.X + player.direction * 8f, player.Bottom.Y - 10f, player.velocity.X + player.direction * 1, player.velocity.Y - 1 / 2f, type, damage, knockBack, Main.myPlayer, 0f, 0f);

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
            string[] newDamage;
            string tempString;

            foreach (TooltipLine line2 in tooltips)
            {
                if (line2.mod == "Terraria" && line2.Name == "Damage")
                {
                    try //try catch in case some other mods modify it
                    {
                        //split string up into words
                        newDamage = line2.text.Split(new string[] { " " }, 10, StringSplitOptions.RemoveEmptyEntries);

                        //rebuild text string and add "x 2" after the damage number
                        tempString = newDamage[0] + " x 2";

                        //add remaining words back
                        for (int i = 1; i < newDamage.Length; i++)
                        {
                            tempString += " " + newDamage[i];
                        }

                        line2.text = tempString;
                    }
                    catch (Exception)
                    {

                    }
                }
            }
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.MeteoriteBar, 5);
            recipe.AddIngredient(ModContent.ItemType<CaughtDungeonSoulFreed>(), 2);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
