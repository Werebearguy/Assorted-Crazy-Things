using AssortedCrazyThings.Items.Weapons;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items
{
    //TODO Recipes for drone unlockables
    public abstract class DroneUnlockables : ModItem
    {
        public override void SetDefaults()
        {
            item.maxStack = 999;
            item.rare = -11;
            item.width = 26;
            item.height = 24;
            item.consumable = true;
            item.maxStack = 1;
            item.UseSound = SoundID.Item4;
            item.useTime = 40;
            item.useAnimation = 40;
            item.useStyle = ItemUseStyleID.HoldingUp;
            item.value = Item.sellPrice(silver: 50);
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            AssPlayer mPlayer = Main.LocalPlayer.GetModPlayer<AssPlayer>();
            string name = DroneController.GetDroneData(UnlockedType).Name;
            if (!mPlayer.droneControllerUnlocked.HasFlag(UnlockedType))
            {
                tooltips.Add(new TooltipLine(mod, "Unlocks", "Unlocks the " + name + " for the Drone Controller"));
            }
            else
            {
                tooltips.Add(new TooltipLine(mod, "Unlocks", "Already unlocked " + name + " for the Drone Controller"));
            }
        }

        public abstract DroneType UnlockedType { get; }

        public override bool CanUseItem(Player player)
        {
            if (Main.netMode != NetmodeID.Server && Main.myPlayer == player.whoAmI)
            {
                AssPlayer mPlayer = player.GetModPlayer<AssPlayer>();
                if (!mPlayer.droneControllerUnlocked.HasFlag(UnlockedType))
                {
                    return true;
                }
            }
            return false;
        }

        public override bool UseItem(Player player)
        {
            if (Main.netMode != NetmodeID.Server && Main.myPlayer == player.whoAmI)
            {
                player.GetModPlayer<AssPlayer>().droneControllerUnlocked |= UnlockedType;
                AssortedCrazyThings.UIText("Unlocked: " + DroneController.GetDroneData(UnlockedType).Name, CombatText.HealLife);
            }
            return true;
        }
    }

    public class DroneUnlockableBasic : DroneUnlockables
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Basic Laser Drone");
        }

        public override DroneType UnlockedType
        {
            get
            {
                return DroneType.BasicLaser;
            }
        }
    }

    public class DroneUnlockableHeavy : DroneUnlockables
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Heavy Laser Drone");
        }

        public override DroneType UnlockedType
        {
            get
            {
                return DroneType.HeavyLaser;
            }
        }
    }

    public class DroneUnlockableMissile : DroneUnlockables
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Missile Drone");
        }

        public override DroneType UnlockedType
        {
            get
            {
                return DroneType.Missile;
            }
        }
    }

    public class DroneUnlockableHealing : DroneUnlockables
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Healing Drone");
        }

        public override DroneType UnlockedType
        {
            get
            {
                return DroneType.Healing;
            }
        }
    }

    public class DroneUnlockableShield : DroneUnlockables
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shield Drone");
        }

        public override DroneType UnlockedType
        {
            get
            {
                return DroneType.Shield;
            }
        }
    }
}
