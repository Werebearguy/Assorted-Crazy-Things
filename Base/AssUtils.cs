﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Base
{
    static class AssUtils
    {
        /// <summary>
        /// The instance of the mod
        /// </summary>
        public static AssortedCrazyThings Instance => ModContent.GetInstance<AssortedCrazyThings>(); //just shorter writing AssUtils.Instance than AssortedCrazyThings.Instance

        /// <summary>
        /// The config of the mod
        /// </summary>
        public static Config AssConfig => ModContent.GetInstance<Config>();

        /// <summary>
        /// Types of modded NPCs which names are ending with Body or Tail
        /// </summary>
        public static int[] isModdedWormBodyOrTail;

        public static void Print(object o)
        {
            if (Main.netMode == NetmodeID.Server)
            {
                Console.WriteLine(o.ToString());
            }

            if (Main.netMode == NetmodeID.MultiplayerClient || Main.netMode == NetmodeID.SinglePlayer)
            {
                Main.NewText(o.ToString());
            }
        }

        public static void UIText(string str, Color color)
        {
            CombatText.NewText(Main.LocalPlayer.getRect(), color, str);
        }

        public static Dust QuickDust(int dustType, Vector2 pos, Color color, Vector2 dustVelo = default(Vector2), int alpha = 0, float scale = 1f)
        {
            Dust dust = Dust.NewDustPerfect(pos, dustType, dustVelo, alpha, color, scale);
            dust.position = pos;
            dust.velocity = dustVelo;
            dust.fadeIn = 1f;
            dust.noLight = false;
            dust.noGravity = true;
            return dust;
        }

        public static void QuickDustLine(int dustType, Vector2 start, Vector2 end, float splits, Color color = default(Color), Vector2 dustVelo = default(Vector2), int alpha = 0, float scale = 1f)
        {
            QuickDust(dustType, start, color, dustVelo);
            float num = 1f / splits;
            for (float num2 = 0f; num2 < 1f; num2 += num)
            {
                QuickDust(dustType, Vector2.Lerp(start, end, num2), color, dustVelo, alpha, scale);
            }
        }

        /// <summary>
        /// Something similar to Dust.QuickDust
        /// </summary>
        public static Dust DrawDustAtPos(Vector2 pos, int dustType = 169)
        {
            //used for showing a position as a dust for debugging
            Dust dust = QuickDust(dustType, pos, Color.White);
            dust.noGravity = true;
            dust.noLight = true;
            return dust;
        }

        public static void DrawSkeletronLikeArms(SpriteBatch spriteBatch, string texString, Vector2 selfPos, Vector2 centerPos, float selfPad = 0f, float centerPad = 0f, float direction = 0f)
        {
            DrawSkeletronLikeArms(spriteBatch, ModContent.GetTexture(texString), selfPos, centerPos, selfPad, centerPad, direction);
        }

        /// <summary>
        /// Draws two "arms" originating from selfPos, "attached" at centerPos
        /// </summary>
        public static void DrawSkeletronLikeArms(SpriteBatch spriteBatch, Texture2D tex, Vector2 selfPos, Vector2 centerPos, float selfPad = 0f, float centerPad = 0f, float direction = 0f)
        {
            //with all float params = 0f, the arm will originate below the selfPos
            //Pos parameters should be Entity.Center
            //Pad parameters are actually just y offsets
            //direction determines in what direction the elbow bends and by how much (-1 to 1 are preferred)
            //if (tex == null) tex = Main.boneArmTexture;
            Vector2 drawPos = selfPos;
            drawPos += new Vector2(-5f * direction, selfPad);
            centerPos.Y += -tex.Height / 2 + centerPad;
            for (int i = 0; i < 2; i++)
            {
                float x = centerPos.X - drawPos.X;
                float y = centerPos.Y - drawPos.Y;
                float magnitude;
                if (i == 0) //first arm piece starting at selfPos
                {
                    x += -(100 + tex.Height) * direction;
                    y += 100 + tex.Width;
                    magnitude = (float)Math.Sqrt(x * x + y * y);
                    magnitude = (tex.Height / 2) / magnitude;
                    drawPos.X += x * magnitude;
                    drawPos.Y += y * magnitude;
                }
                else //second arm piece
                {
                    x += -(30 + tex.Width / 2) * direction;
                    y += 30 + tex.Height / 2;
                    magnitude = (float)Math.Sqrt(x * x + y * y);
                    magnitude = (tex.Height / 2) / magnitude;
                    drawPos.X += x * magnitude;
                    drawPos.Y += y * magnitude;
                }
                float rotation = (float)Math.Atan2(y, x) - 1.57f;
                Color color = Lighting.GetColor((int)drawPos.X / 16, (int)(drawPos.Y / 16f));
                spriteBatch.Draw(tex, new Vector2(drawPos.X - Main.screenPosition.X, drawPos.Y - Main.screenPosition.Y), tex.Bounds, color, rotation, tex.Bounds.Size() / 2, 1f, SpriteEffects.None, 0f);
                if (i == 0)
                {
                    //padding for the second arm piece
                    drawPos.X += x * magnitude * 1.1f;
                    drawPos.Y += y * magnitude * 1.1f;
                }
                else if (Main.instance.IsActive) //not sure what this part does
                {
                    drawPos.X += x * magnitude - 16f;
                    drawPos.Y += y * magnitude - 6f;
                }
            }
        }

        public static void DrawTether(SpriteBatch spriteBatch, string texString, Vector2 start, Vector2 end)
        {
            DrawTether(spriteBatch, ModContent.GetTexture(texString), start, end);
        }

        //Credit to IDGCaptainRussia
        /// <summary>
        /// Draws a "connection" between two points
        /// </summary>
        public static void DrawTether(SpriteBatch spriteBatch, Texture2D tex, Vector2 start, Vector2 end)
        {
            Vector2 position = start;
            Vector2 mountedCenter = end;
            float num1 = tex.Height;
            Vector2 vector2_4 = mountedCenter - position;
            Vector2 vector2_4tt = mountedCenter - position;
            float keepgoing = vector2_4tt.Length();
            Vector2 vector2t = vector2_4;
            vector2t.Normalize();
            position -= vector2t * (num1 * 0.5f);

            float rotation = (float)Math.Atan2(vector2_4.Y, vector2_4.X) - 1.57f;
            bool flag = true;
            if (float.IsNaN(position.X) && float.IsNaN(position.Y))
                flag = false;
            if (float.IsNaN(vector2_4.X) && float.IsNaN(vector2_4.Y))
                flag = false;
            while (flag)
            {
                if (keepgoing <= -1)
                {
                    flag = false;
                }
                else
                {
                    Vector2 vector2_1 = vector2_4;
                    vector2_1.Normalize();
                    position += vector2_1 * num1;
                    keepgoing -= num1;
                    vector2_4 = mountedCenter - position;
                    Color color2 = Lighting.GetColor((int)position.X / 16, (int)position.Y / 16);
                    color2 = new Color(color2.R, color2.G, color2.B, 255);
                    spriteBatch.Draw(tex, position - Main.screenPosition, new Rectangle(0, 0, tex.Width, (int)Math.Min(num1, num1 + keepgoing)), color2, rotation, tex.Bounds.Size() / 2, 1f, SpriteEffects.None, 0.0f);
                }
            }
        }

        /// <summary>
        /// Combines two arrays (first + second in order)
        /// </summary>
        public static T[] ConcatArray<T>(T[] first, T[] second)
        {
            T[] combined = new T[first.Length + second.Length];
            Array.Copy(first, combined, first.Length);
            Array.Copy(second, 0, combined, first.Length, second.Length);
            return combined;
        }

        /// <summary>
        /// Fills an array with a default value.
        /// If array is null, creates one with the length specified.
        /// Else, overrides each element with default value
        /// </summary>
        public static void FillWithDefault<T>(ref T[] array, T def, int length = -1)
        {
            if (array == null)
            {
                if (length == -1)
                    throw new ArgumentOutOfRangeException("Array is null but length isn't specified");
                array = new T[length];
            }
            else
            {
                length = array.Length;
            }

            for (int i = 0; i < length; i++)
            {
                array[i] = def;
            }
        }

        /// <summary>
        /// Fills a list with a default value.
        /// If array is null, creates one with the length specified.
        /// Else, overrides each element with default value
        /// </summary>
        public static void FillWithDefault<T>(ref List<T> list, T def, int length = -1)
        {
            if (list == null)
            {
                if (length == -1)
                    throw new ArgumentOutOfRangeException("List is null but length isn't specified");
                list = new List<T>(length);
            }
            else
            {
                length = list.Count;
            }

            for (int i = 0; i < length; i++)
            {
                list.Add(def);
            }
        }

        /// <summary>
        /// Like NPC.AnyNPC, but checks for each type in the passed array.
        /// If one exists, returns true
        /// </summary>
        public static bool AnyNPCs(int[] types)
        {
            //Like AnyNPCs but checks for an array
            for (int i = 0; i < types.Length; i++)
            {
                if (NPC.AnyNPCs(types[i])) return true;
            }
            return false;
        }

        /// <summary>
        /// Like NPC.AnyNPC, but checks for each type in the passed list.
        /// If one exists, returns true
        /// </summary>
        public static bool AnyNPCs(List<int> types)
        {
            return AnyNPCs(types.ToArray());
        }

        /// <summary>
        /// Like NPC.AnyNPC, but checks for custom condition (active already true).
        /// If one exists, returns true
        /// </summary>
        public static bool AnyNPCs(Func<NPC, bool> condition)
        {
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                if (Main.npc[i].active && condition(Main.npc[i])) return true;
            }
            return false;
        }

        /// <summary>
        /// Counts all NPCs in the passed array
        /// </summary>
        public static int CountAllNPCs(int[] types)
        {
            int count = 0;
            for (int i = 0; i < types.Length; i++)
            {
                count += NPC.CountNPCS(types[i]);
            }
            return count;
        }

        /// <summary>
        /// Counts all active projectiles of the given type, and of a given owner if specified
        /// </summary>
        public static int CountProjs(int type, int owner = -1)
        {
            int count = 0;
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile proj = Main.projectile[i];
                if (proj.active && proj.type == type &&
                    (owner < 0 || proj.owner == owner))
                {
                    count++;
                }
            }
            return count;
        }

        /// <summary>
        /// Checks if given NPC is a worm body or tail
        /// </summary>
        public static bool IsWormBodyOrTail(NPC npc)
        {
            return npc.dontCountMe || Array.BinarySearch(isModdedWormBodyOrTail, npc.type) >= 0 || npc.type == NPCID.EaterofWorldsTail || npc.type == NPCID.EaterofWorldsBody/* || npc.realLife != -1*/;
        }

        /// <summary>
        /// Checks if player is in an evil biome (any of three)
        /// </summary>
        public static bool EvilBiome(Player player)
        {
            return player.ZoneCorrupt || player.ZoneCrimson || player.ZoneHoly;
        }

        /// <summary>
        /// Formats Main.time into a string representation with AM/PM
        /// </summary>
        public static string GetTimeAsString(bool accurate = true)
        {
            string suffix = "AM";
            double doubletime = Main.time;
            if (!Main.dayTime)
            {
                doubletime += 54000.0;
            }
            doubletime = doubletime / 86400.0 * 24.0;
            double wtf = 7.5;
            doubletime = doubletime - wtf - 12.0;
            if (doubletime < 0.0)
            {
                doubletime += 24.0;
            }
            if (doubletime >= 12.0)
            {
                suffix = "PM";
            }
            int hours = (int)doubletime;
            double doubleminutes = doubletime - hours;
            doubleminutes = (int)(doubleminutes * 60.0);
            string minutes = string.Concat(doubleminutes);
            if (doubleminutes < 10.0)
            {
                minutes = "0" + minutes;
            }
            if (hours > 12)
            {
                hours -= 12;
            }
            if (hours == 0)
            {
                hours = 12;
            }
            if (!accurate) minutes = (!(doubleminutes < 30.0)) ? "30" : "00";
            return Language.GetTextValue("Game.Time", hours + ":" + minutes + " " + suffix);
        }

        public static string GetMoonPhaseAsString(bool showNumber = false)
        {
            string suffix = "";
            if (showNumber) suffix = " (" + (Main.moonPhase + 1) + ")";
            string prefix = Lang.inter[102].Value + ": "; //can't seem to find "Moon Phase" in the lang files for GameUI
            string value = "";
            string check = "";
            switch (Main.moonPhase)
            {
                case 0:
                    check = "FullMoon";
                    break;
                case 1:
                    check = "WaningGibbous";
                    break;
                case 2:
                    check = "ThirdQuarter";
                    break;
                case 3:
                    check = "WaningCrescent";
                    break;
                case 4:
                    check = "NewMoon";
                    break;
                case 5:
                    check = "WaxingCrescent";
                    break;
                case 6:
                    check = "FirstQuarter";
                    break;
                case 7:
                    check = "WaxingGibbous";
                    break;
                default:
                    break;
            }
            value = Language.GetTextValue("GameUI." + check);
            if (value != "") return prefix + value + suffix;
            return "";
        }

        /// <summary>
        /// Alternative NewProjectile, automatically sets owner to Main.myPlayer.
        /// Also doesn't take into account vanilla projectiles that set things like ai or timeLeft, so only use this for ModProjectiles.
        /// Use preCreate if you want to spawn or not spawn the projectile based on the projectile itself.
        /// Use preSync to set ai[0], ai[1] and other values
        /// </summary>
        public static int NewProjectile(Vector2 position, Vector2 velocity, int Type, int Damage, float KnockBack, Func<Projectile, bool> preCreate = null, Action<Projectile> preSync = null)
        {
            return NewProjectile(position.X, position.Y, velocity.X, velocity.Y, Type, Damage, KnockBack, preCreate, preSync);
        }

        /// <summary>
        /// Alternative NewProjectile, automatically sets owner to Main.myPlayer.
        /// Also doesn't take into account vanilla projectiles that set things like ai or timeLeft, so only use this for ModProjectiles.
        /// Use preCreate if you want to spawn or not spawn the projectile based on the projectile itself.
        /// Use preSync to set ai[0], ai[1] and other values
        /// </summary>
        public static int NewProjectile(float X, float Y, float SpeedX, float SpeedY, int Type, int Damage, float KnockBack, Func<Projectile, bool> preCreate = null, Action<Projectile> preSync = null)
        {
            if (preCreate != null)
            {
                Projectile test = new Projectile();
                test.SetDefaults(Type);
                if (!preCreate(test)) return 1000;
            }

            int index = 1000;
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                if (!Main.projectile[i].active)
                {
                    index = i;
                    break;
                }
            }
            if (index == 1000)
            {
                return index;
            }
            int Owner = Main.myPlayer;
            //float ai0 = 0f;
            //float ai1 = 0f;

            Projectile projectile = Main.projectile[index];
            projectile.SetDefaults(Type);
            projectile.position.X = X - projectile.width * 0.5f;
            projectile.position.Y = Y - projectile.height * 0.5f;
            projectile.owner = Owner;
            projectile.velocity.X = SpeedX;
            projectile.velocity.Y = SpeedY;
            projectile.damage = Damage;
            projectile.knockBack = KnockBack;
            projectile.identity = index;
            projectile.gfxOffY = 0f;
            projectile.stepSpeed = 1f;
            projectile.wet = Collision.WetCollision(projectile.position, projectile.width, projectile.height);
            if (projectile.ignoreWater)
            {
                projectile.wet = false;
            }
            projectile.honeyWet = Collision.honey;
            Main.projectileIdentity[Owner, index] = index;
            //projectile.ai[0] = ai0;
            //projectile.ai[1] = ai1;
            if (Type > 0)
            {
                if (ProjectileID.Sets.NeedsUUID[Type])
                {
                    projectile.projUUID = projectile.identity;
                }
            }

            preSync?.Invoke(projectile);

            if (Main.netMode != NetmodeID.SinglePlayer)
            {
                NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, index);
            }
            return index;
        }

        /// <summary>
        /// Alternative, static version of npc.DropItemInstanced. Checks the playerCondition delegate before syncing/spawning the item
        /// </summary>
        public static void DropItemInstanced(NPC npc, Vector2 Position, Vector2 HitboxSize, int itemType, int itemStack = 1, Func<NPC, Player, bool> condition = null, bool interactionRequired = true)
        {
            if (itemType > 0)
            {
                if (Main.netMode == NetmodeID.Server)
                {
                    int item = Item.NewItem((int)Position.X, (int)Position.Y, (int)HitboxSize.X, (int)HitboxSize.Y, itemType, itemStack, true);
                    Main.itemLockoutTime[item] = 54000;
                    for (int p = 0; p < Main.maxPlayers; p++)
                    {
                        if (Main.player[p].active && (npc.playerInteraction[p] || !interactionRequired))
                        {
                            if (condition != null && condition(npc, Main.player[p]) ||
                                condition == null)
                                NetMessage.SendData(MessageID.InstancedItem, p, -1, null, item);
                        }
                    }
                    Main.item[item].active = false;
                }
                else if (Main.netMode == NetmodeID.SinglePlayer)
                {
                    if (condition != null && condition(npc, Main.LocalPlayer) ||
                        condition == null)
                        Item.NewItem((int)Position.X, (int)Position.Y, (int)HitboxSize.X, (int)HitboxSize.Y, itemType, itemStack);
                }
                //npc.value = 0f;
            }
        }

        /// <summary>
        /// Checks if given item is present in the players inventory or equip slots
        /// </summary>
        public static bool ItemInInventoryOrEquipped(Player player, Item item, bool ignoreVanity = false)
        {
            if (player.HasItem(item.type)) return true;
            if (item.accessory || item.headSlot > 0 || item.bodySlot > 0 || item.legSlot > 0)
            {
                int maxLength = ignoreVanity ? 10 : player.armor.Length;
                for (int i = 0; i < maxLength; i++)
                {
                    if (player.armor[i].type == item.type) return true;
                }
            }
            return false;
        }
    }
}
