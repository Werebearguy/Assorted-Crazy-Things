using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Harblesnargits_Mod_01.Items.Weapons
{
    public class weap_flamethrower_01 : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Breath of Spazmatism");
            Tooltip.SetDefault("Fires a stream of cursed flames.");
        }

        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.Flamethrower);
            item.width = 58;
            item.height = 30;
            item.damage = 20;
            item.UseSound = SoundID.Item34;
            item.shoot = mod.ProjectileType("proj_fire_01");
            item.shootSpeed = 7f;
            item.noMelee = true;
            item.ranged = true;
            item.useAmmo = AmmoID.Gel;
            item.useTime = 3; //adjusted from 10 to 3 to match spazmatism speed
            item.useAnimation = 3; //^
            item.useStyle = 5;
            item.value = 10000;
            item.rare = -11;
            item.autoReuse = true;
        }

        public override bool ConsumeAmmo(Player player)
        {
            return Main.rand.NextFloat() >= .66f; //66% chance not to consume ammo (since its so fast)
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Vector2 muzzleOffset = Vector2.Normalize(new Vector2(speedX, speedY)) * 0f;
            if (Collision.CanHit(position, 5, 0, position + muzzleOffset, 5, 0))
            {
                position += muzzleOffset;
            }
            return true;
        }
    }
}
