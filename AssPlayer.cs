using AssortedCrazyThings.Items;
using AssortedCrazyThings.Items.Fun;
using AssortedCrazyThings.NPCs.DungeonBird;
using AssortedCrazyThings.Projectiles.Minions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace AssortedCrazyThings
{
    public class AssPlayer : ModPlayer
    {
        public bool everburningCandleBuff = false;
        public bool everburningCursedCandleBuff = false;
        public bool everfrozenCandleBuff = false;
        //public bool variable_debuff_04;
        //public bool variable_debuff_05;
        public bool everburningShadowflameCandleBuff = false;
        //public bool variable_debuff_07;

        public bool teleportHome = false;
        public bool canTeleportHome = false;
        public const short TeleportHomeTimerMax = 600; //in seconds //10 ingame minutes
        public short teleportHomeTimer = 0; //gets saved when you relog so you can't cheese it

        //TECHNICALLY NOT DEFENCE; YOU JUST GET 1 DAMAGE FROM EVERYTHING FOR A CERTAIN DURATION
        public bool getDefense = false;
        public bool canGetDefense = false;
        public const short GetDefenseTimerMax = 600; //in seconds //10 ingame minutes
        private const short GetDefenseDurationMax = 600; //in ticks //10 ingame seconds
        public short getDefenseDuration = 0;
        public short getDefenseTimer = 0; //gets saved when you relog so you can't cheese it

        //slime accessory stuff
        public uint slotsPlayer = 0;
        //public int counter = 30;
        //public int clientcounter = 30;

        //soul minion stuff
        public bool soulMinion = false;
        public bool tempSoulMinion = false;
        public int selectedSoulMinionType = (int)CompanionDungeonSoulMinionBase.SoulType.Dungeon;

        public bool slimePackMinion = false;
        public byte selectedSlimePackMinionType = 0;

        //empowering buff stuff
        public bool empoweringBuff = false;
        private const short empoweringTimerMax = 60; //in seconds //one minute until it caps out (independent of buff duration)
        private short empoweringTimer = 0;
        public static float empoweringTotal = 0.5f; //this gets modified in AssWorld.PreUpdate()
        public float step;

        //enhanced hunter potion stuff
        public bool enhancedHunterBuff = false;

        public bool soulSaviorArmor = false;

        private bool rightClickPrev = false;
        private bool rightClickPrev2 = false;

        private bool leftClickPrev = false;
        private bool leftClickPrev2 = false;

        //legacy, but don't delete
        public int planteraGitGudCounter = 0;

        public override void ResetEffects()
        {
            everburningCandleBuff = false;
            everburningCursedCandleBuff = false;
            everfrozenCandleBuff = false;
            //variable_debuff_04 = false;
            //variable_debuff_05 = false;
            everburningShadowflameCandleBuff = false;
            //variable_debuff_07 = false;
            teleportHome = false;
            getDefense = false;
            soulMinion = false;
            tempSoulMinion = false;
            slimePackMinion = false;
            empoweringBuff = false;
            enhancedHunterBuff = false;
            soulSaviorArmor = false;
        }

        public bool RightClickPressed
        {
            get
            {
                return rightClickPrev && !rightClickPrev2;
            }
        }

        public bool RightClickReleased
        {
            get
            {
                return !rightClickPrev && rightClickPrev2;
            }
        }

        public bool LeftClickPressed
        {
            get
            {
                return leftClickPrev && !leftClickPrev2;
            }
        }

        public bool LeftClickReleased
        {
            get
            {
                return !leftClickPrev && leftClickPrev2;
            }
        }

        public override TagCompound Save()
        {
            return new TagCompound
            {
                {"slotsPlayer", (int)slotsPlayer}, //keep as legacy
                {"teleportHomeWhenLowTimer", (int)teleportHomeTimer},
                {"getDefenseTimer", (int)getDefenseTimer},
                {"planteraGitGudCounter", (int)planteraGitGudCounter}, //keep as legacy
            };
        }

        public override void Load(TagCompound tag)
        {
            slotsPlayer = (uint)tag.GetInt("slotsPlayer"); //keep as legacy
            teleportHomeTimer = (short)tag.GetInt("teleportHomeWhenLowTimer");
            getDefenseTimer = (short)tag.GetInt("getDefenseTimer");
            planteraGitGudCounter = tag.GetInt("planteraGitGudCounter"); //keep as legacy
        }

        public override void OnEnterWorld(Player player)
        {
            if (slotsPlayer != 0) //transfer legacy variable over to new one
            {
                PetPlayer mPlayer = player.GetModPlayer<PetPlayer>(mod);
                mPlayer.slots = slotsPlayer;
                slotsPlayer = 0;
            }

            if (planteraGitGudCounter != 0) //transfer legacy variable over to new one
            {
                GitGudPlayer gPlayer = player.GetModPlayer<GitGudPlayer>(mod);
                gPlayer.planteraGitGudCounter = (byte)planteraGitGudCounter;
                planteraGitGudCounter = 0;
            }
        }

        private void ResetEmpoweringTimer()
        {
            if (empoweringBuff && !player.HasBuff(BuffID.ShadowDodge))
            {
                for (int i = 0; i < empoweringTimer; i++)
                {
                    Dust dust = Dust.NewDustPerfect(player.Center, 135, new Vector2(Main.rand.NextFloat(-3f, 3f), Main.rand.NextFloat(-3f, 3f)) + (new Vector2(Main.rand.Next(-1, 1), Main.rand.Next(-1, 1)) * ((6 * empoweringTimer) / empoweringTimerMax)), 26, new Color(255, 255, 255), Main.rand.NextFloat(1.5f, 2.4f));
                    dust.noLight = true;
                    dust.noGravity = true;
                    dust.fadeIn = Main.rand.NextFloat(1f, 2.3f);
                }
                empoweringTimer = 0;
            }
        }

        public int SpawnSoul(CompanionDungeonSoulMinionBase.SoulStats stats, bool temp = false)
        {
            return SpawnSoul(stats.Type, stats.Damage, stats.Knockback, temp);
        }

        public int SpawnSoul(int type, int damage, float knockback, bool temp = false)
        {
            int index = 0;
            Vector2 spawnPos = new Vector2(player.position.X + (player.width / 2) + player.direction * 8f, player.Bottom.Y - 12f);
            Vector2 spawnVelo = new Vector2(player.velocity.X + player.direction * 1.5f, player.velocity.Y - 1f);

            index = Projectile.NewProjectile(spawnPos, spawnVelo, type, damage, knockback, player.whoAmI, 0f, 0f);
            if (temp) return index; //spawn only one 

            if (player.HeldItem.type != mod.ItemType<Items.Weapons.EverhallowedLantern>()) //spawn only one when holding Everhallowed Lantern
            {
                spawnPos.Y += 2f;
                spawnVelo.X -= 0.5f * player.direction;
                spawnVelo.Y += 0.5f;
                Projectile.NewProjectile(spawnPos, spawnVelo, type, damage, knockback, player.whoAmI, 0f, 0f);
            }

            return index;
        }

        private void SpawnSoulTemp()
        {
            if (tempSoulMinion && player.whoAmI == Main.myPlayer)
            {
                bool checkIfAlive = false;
                for (int i = 0; i < 1000; i++)
                {
                    if (Main.projectile[i].active && Main.projectile[i].owner == player.whoAmI && Main.projectile[i].type == CompanionDungeonSoulMinionBase.GetAssociatedStats((int)CompanionDungeonSoulMinionBase.SoulType.Temp).Type)
                    {
                        if (Main.projectile[i].minionSlots == 0f)
                        {
                            checkIfAlive = true;
                            break;
                        }
                    }
                }
                
                if (!checkIfAlive)
                {
                    //twice the damage
                    var stats = CompanionDungeonSoulMinionBase.GetAssociatedStats((int)CompanionDungeonSoulMinionBase.SoulType.Temp);
                    int i = SpawnSoul(stats, true);
                    Main.projectile[i].minionSlots = 0f;
                    Main.projectile[i].timeLeft = 600; //10 seconds
                }
            }
        }

        private bool EligibleToRecieveSoulBuff(NPC npc)
        {
            //returns true if isn't in soulbuffblacklist or is a worm body or tail
            
            return Array.IndexOf(AssortedCrazyThings.soulBuffBlacklist, npc.type) == -1 || AssUtils.IsWormBodyOrTail(npc);
        }

        private void SpawnSoulsWhenHarvesterIsAlive()
        {
            //ALWAYS GENERATE SOULS WHEN ONE IS ALIVE (otherwise he will never eat stuff when you aren't infront of dungeon walls)
            if (Main.time % 30 == 4)
            {
                bool shouldDropSouls = false;
                int index = 200;
                for (short j = 0; j < 200; j++)
                {
                    if (Main.npc[j].active && Array.IndexOf(AssWorld.harvesterTypes, Main.npc[j].type) != -1)
                    {
                        shouldDropSouls = true;
                        index = j;
                        break;
                    }
                }

                if (shouldDropSouls)
                {
                    int distance = (int)(Main.npc[index].Center - player.Center).Length();
                    if (distance < 2880 || player.ZoneDungeon) //one and a half screens or in dungeon
                    {
                        for (short j = 0; j < 200; j++)
                        {
                            if (Main.npc[j].active && Main.npc[j].lifeMax > 5 && !Main.npc[j].friendly && !Main.npc[j].dontTakeDamage && !Main.npc[j].immortal)
                            {
                                if (Array.IndexOf(AssWorld.harvesterTypes, Main.npc[j].type) == -1 && EligibleToRecieveSoulBuff(Main.npc[j]))
                                {
                                    Main.npc[j].AddBuff(mod.BuffType("SoulBuff"), 60, true);
                                }
                            }
                        }
                    }
                }
            }
        }

        public void ConvertInertSoulsInventory()
        {
            //this gets called once on server side for all players, and then each player calls it on itself client side
            int tempStackCount = 0;
            int itemTypeOld = mod.ItemType<CaughtDungeonSoul>();
            int itemTypeNew = mod.ItemType<CaughtDungeonSoulFreed>(); //version that is used in crafting

            Item[][] inventoryArray = { player.inventory, player.bank.item, player.bank2.item, player.bank3.item }; //go though player inv
            for (int y = 0; y < inventoryArray.Length; y++)
            {
                for (int e = 0; e < inventoryArray[y].Length; e++)
                {
                    if (inventoryArray[y][e].type == itemTypeOld) //find inert soul
                    {
                        tempStackCount = inventoryArray[y][e].stack;
                        inventoryArray[y][e].SetDefaults(itemTypeNew); //override with awakened
                        inventoryArray[y][e].stack = tempStackCount;
                    }
                }
            }

            //trash slot
            if (player.trashItem.type == itemTypeOld)
            {
                tempStackCount = player.trashItem.stack;
                player.trashItem.SetDefaults(itemTypeNew); //override with awakened
                player.trashItem.stack = tempStackCount;
            }

            //mouse item
            if (Main.netMode != NetmodeID.Server && Main.mouseItem.type == itemTypeOld)
            {
                tempStackCount = Main.mouseItem.stack;
                Main.mouseItem.SetDefaults(itemTypeNew); //override with awakened
                Main.mouseItem.stack = tempStackCount;
            }
        }

        private void UpdateTeleportHomeWhenLow()
        {
            //this code runs even when the accessory is not equipped
            canTeleportHome = teleportHomeTimer <= 0;

            if (!canTeleportHome && Main.time % 60 == 59)
            {
                teleportHomeTimer--;
            }
        }

        private void UpdateGetDefenseWhenLow()
        {
            //this code runs even when the accessory is not equipped
            canGetDefense = getDefenseTimer <= 0;

            if (!canGetDefense && Main.time % 60 == 59)
            {
                getDefenseTimer--;
            }

            if (getDefenseDuration != 0)
            {
                getDefenseDuration--;
            }
        }

        private void Empower()
        {
            if (empoweringBuff)
            {
                if (Main.time % 60 == 0)
                {
                    if (empoweringTimer < empoweringTimerMax)
                    {
                        empoweringTimer++;
                        step = (empoweringTimer * empoweringTotal) / empoweringTimerMax;
                    }
                }
            }
            else empoweringTimer = 0;
        }

        private bool GetDefense(double damage)
        {
            if (getDefense)
            {
                if (canGetDefense)
                {
                    player.statLife += (int)damage;
                    player.AddBuff(BuffID.RapidHealing, 600);
                    CombatText.NewText(player.getRect(), CombatText.HealLife, "Defense increased");

                    getDefenseTimer = GetDefenseTimerMax;
                    getDefenseDuration = GetDefenseDurationMax;
                    return false;
                }
            }
            return true;
        }

        private bool TeleportHome(double damage)
        {
            if (teleportHome)
            {
                if (canTeleportHome && player.whoAmI == Main.myPlayer)
                {
                    //this part here is from vanilla magic mirror code
                    player.grappling[0] = -1;
                    player.grapCount = 0;
                    for (int i = 0; i < 1000; i++)
                    {
                        //Kill all grappling hooks
                        if (Main.projectile[i].active && Main.projectile[i].owner == player.whoAmI && Main.projectile[i].aiStyle == 7)
                        {
                            Main.projectile[i].Kill();
                        }
                    }

                    //inserted before player.Spawn()
                    player.statLife += (int)damage;

                    player.Spawn();
                    for (int i = 0; i < 70; i++)
                    {
                        Dust.NewDust(player.position, player.width, player.height, 15, 0f, 0f, 150, default(Color), 1.5f);
                    }
                    //end

                    player.AddBuff(BuffID.RapidHealing, 300, false);

                    if (Main.netMode == NetmodeID.Server)
                    {
                        NetMessage.SendData(MessageID.PlayerControls, -1, -1, null, player.whoAmI);
                    }

                    teleportHomeTimer = TeleportHomeTimerMax;
                    return false;
                }
            }
            return true;
        }

        private void RightClickStatus()
        {
            if (Main.mouseRight && !rightClickPrev)
            {
                rightClickPrev = true;
                return;
            }
            if (!Main.mouseRight && rightClickPrev)
            {
                rightClickPrev = false;
                return;
            }

            if (rightClickPrev && !rightClickPrev2)
            {
                rightClickPrev2 = true;
            }
            if (!rightClickPrev && rightClickPrev2)
            {
                rightClickPrev2 = false;
            }
        }

        private void LeftClickStatus()
        {
            if (Main.mouseLeft && !leftClickPrev)
            {
                leftClickPrev = true;
                return;
            }
            if (!Main.mouseLeft && leftClickPrev)
            {
                leftClickPrev = false;
                return;
            }

            if (leftClickPrev && !leftClickPrev2)
            {
                leftClickPrev2 = true;
            }
            if (!leftClickPrev && leftClickPrev2)
            {
                leftClickPrev2 = false;
            }
        }

        private void ApplyCandleDebuffs(Entity victim)
        {
            if (victim is NPC)
            {
                if (everburningCandleBuff) ((NPC)victim).AddBuff(BuffID.OnFire, 120);
                if (everburningCursedCandleBuff) ((NPC)victim).AddBuff(BuffID.CursedInferno, 120);
                if (everfrozenCandleBuff) ((NPC)victim).AddBuff(BuffID.Frostburn, 120);
                //if (variable_debuff_04) ((NPC)victim).AddBuff(BuffID.Ichor, 120);
                //if (variable_debuff_05) ((NPC)victim).AddBuff(BuffID.Venom, 120);
                if (everburningShadowflameCandleBuff) ((NPC)victim).AddBuff(BuffID.ShadowFlame, 60);
                //if (variable_debuff_07) ((NPC)victim).AddBuff(BuffID.Bleeding, 120);
            }
            else if (victim is Player)
            {
                //return;
                //if (everburningCandleBuff) ((Player)victim).AddBuff(BuffID.OnFire, 120);
                //if (everburningCursedCandleBuff) ((Player)victim).AddBuff(BuffID.CursedInferno, 120);
                //if (everfrozenCandleBuff) ((Player)victim).AddBuff(BuffID.Frostburn, 120);
                ////if (variable_debuff_04) ((NPC)victim).AddBuff(BuffID.Ichor, 120);
                ////if (variable_debuff_05) ((NPC)victim).AddBuff(BuffID.Venom, 120);
                //if (everburningShadowflameCandleBuff) ((Player)victim).AddBuff(BuffID.ShadowFlame, 60);
                ////if (variable_debuff_07) ((NPC)victim).AddBuff(BuffID.Bleeding, 120);
            }
        }

        public static readonly PlayerLayer SlimeHandlerKnapsack = new PlayerLayer("AssortedCrazyThings", "SlimeHandlerKnapsack", PlayerLayer.MiscEffectsBack, delegate (PlayerDrawInfo drawInfo)
        {
            if (drawInfo.shadow != 0f)
            {
                return;
            }
            Player drawPlayer = drawInfo.drawPlayer;
            Mod mod = AssUtils.Instance;

            if ((drawPlayer.wings == 0 || drawPlayer.velocity.Y == 0f)/* && (drawPlayer.inventory[drawPlayer.selectedItem].type == mod.ItemType<Items.Weapons.SlimeHandlerKnapsack>())*/)
            {
                Texture2D texture = mod.GetTexture("Items/Weapons/SlimeHandlerKnapsack_Back");
                float drawX = (int)drawInfo.position.X + drawPlayer.width / 2f - Main.screenPosition.X;
                float drawY = (int)drawInfo.position.Y + drawPlayer.height - Main.screenPosition.Y;
                
                Vector2 stupidOffset = new Vector2(0f, - drawPlayer.bodyFrame.Height / 2);

                SpriteEffects spriteEffects = SpriteEffects.None;
                if (drawPlayer.gravDir == 1f)
                {
                    if (drawPlayer.direction == 1)
                    {
                        spriteEffects = SpriteEffects.None;
                    }
                    else
                    {
                        spriteEffects = SpriteEffects.FlipHorizontally;
                    }
                }
                else
                {
                    if (drawPlayer.direction == 1)
                    {
                        spriteEffects = SpriteEffects.FlipVertically;
                    }
                    else
                    {
                        spriteEffects = (SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically);
                    }
                }
                Color color = Lighting.GetColor((int)drawPlayer.Center.X / 16, (int)drawPlayer.Center.Y / 16);

                DrawData drawData = new DrawData(texture, new Vector2(drawX, drawY) + drawPlayer.bodyPosition + stupidOffset, drawPlayer.bodyFrame, color, drawPlayer.bodyRotation, drawInfo.bodyOrigin, 1f, spriteEffects, 0);
                Main.playerDrawData.Add(drawData);
            }
        });

        public static readonly PlayerLayer HarvesterWings = new PlayerLayer("AssortedCrazyThings", "HarvesterWings", PlayerLayer.Wings, delegate (PlayerDrawInfo drawInfo)
        {
            if (drawInfo.shadow != 0f)
            {
                return;
            }
            Player drawPlayer = drawInfo.drawPlayer;
            Mod mod = AssUtils.Instance;

            if (drawPlayer.wings == mod.GetEquipSlot("HarvesterWings", EquipType.Wings))
            {
                Texture2D texture = mod.GetTexture("Items/Accessories/Useful/HarvesterWings_Wings_Glowmask");
                float drawX = (int)drawInfo.position.X + drawPlayer.width / 2f - Main.screenPosition.X;
                float drawY = (int)drawInfo.position.Y + drawPlayer.height / 2f - Main.screenPosition.Y;

                Vector2 stupidOffset = new Vector2(-9 * drawPlayer.direction + 0 * drawPlayer.direction, 2f * drawPlayer.gravDir + 0 * drawPlayer.gravDir);

                SpriteEffects spriteEffects = SpriteEffects.None;
                if (drawPlayer.gravDir == 1f)
                {
	                if (drawPlayer.direction == 1)
	                {
		                spriteEffects = SpriteEffects.None;
	                }
	                else
	                {
		                spriteEffects = SpriteEffects.FlipHorizontally;
	                }
                }
                else
                {
	                if (drawPlayer.direction == 1)
	                {
		                spriteEffects = SpriteEffects.FlipVertically;
	                }
	                else
	                {
		                spriteEffects = (SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically);
	                }
                }
                DrawData drawData = new DrawData(texture, new Vector2(drawX, drawY) + stupidOffset, new Rectangle(0, texture.Height / 4 * drawPlayer.wingFrame, texture.Width, texture.Height / 4), new Color(255, 255, 255, 0)/* * num51 * (1f - shadow) * 0.5f*/, drawPlayer.bodyRotation, new Vector2(texture.Width / 2, texture.Height / 8), 1f, spriteEffects, 0);
                drawData.shader = drawInfo.wingShader;
                Main.playerDrawData.Add(drawData);

                if (drawPlayer.velocity.Y != 0)
                {
                    if (Main.rand.NextBool(3))
                    {
                        int dustOffset = 4;
                        if (drawPlayer.direction == 1)
                        {
                            dustOffset = -40;
                        }
                        int dustIndex = Dust.NewDust(new Vector2(drawPlayer.position.X + (drawPlayer.width / 2) + dustOffset, drawPlayer.position.Y + (drawPlayer.height / 2) - 8f), 30, 30, 135, 0f, 0f, 0, default(Color), 1.5f);
                        Main.dust[dustIndex].noGravity = true;
                        Main.dust[dustIndex].noLight = true;
                        Main.dust[dustIndex].velocity *= 0.3f;
                        if (Main.rand.NextBool(5))
                        {
                            Main.dust[dustIndex].fadeIn = 1f;
                        }
                        Main.dust[dustIndex].shader = GameShaders.Armor.GetSecondaryShader(drawPlayer.cWings, drawPlayer);
                    }
                }
            }
        });
        
        public static readonly PlayerLayer SoulSaviorGlowmask = new PlayerLayer("AssortedCrazyThings", "SoulSaviorGlowmask", PlayerLayer.Body, delegate (PlayerDrawInfo drawInfo)
        {
            if (drawInfo.shadow != 0f)
            {
                return;
            }
            Player drawPlayer = drawInfo.drawPlayer;
            Mod mod = AssUtils.Instance;
            
            if (drawPlayer.body == mod.GetEquipSlot("SoulSaviorPlate", EquipType.Body))
            {
                Texture2D texture = mod.GetTexture("Items/Armor/SoulSaviorPlate_Glowmask");
                float drawX = (int)drawInfo.position.X - drawPlayer.bodyFrame.Width / 2 + drawPlayer.width / 2 - Main.screenPosition.X;
                float drawY = (int)drawInfo.position.Y + drawPlayer.height - drawPlayer.bodyFrame.Height + 4f - Main.screenPosition.Y;

                Vector2 stupidOffset = new Vector2(drawPlayer.bodyFrame.Width / 2, drawPlayer.bodyFrame.Height / 2);

                SpriteEffects spriteEffects = SpriteEffects.None;
                if (drawPlayer.gravDir == 1f)
                {
                    if (drawPlayer.direction == 1)
                    {
                        spriteEffects = SpriteEffects.None;
                    }
                    else
                    {
                        spriteEffects = SpriteEffects.FlipHorizontally;
                    }
                }
                else
                {
                    if (drawPlayer.direction == 1)
                    {
                        spriteEffects = SpriteEffects.FlipVertically;
                    }
                    else
                    {
                        spriteEffects = (SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically);
                    }
                }

                DrawData drawData = new DrawData(texture, new Vector2(drawX, drawY) + drawPlayer.bodyPosition + stupidOffset, drawPlayer.bodyFrame, Color.White, drawPlayer.bodyRotation, drawInfo.bodyOrigin, 1f, spriteEffects, 0);
                drawData.shader = drawInfo.bodyArmorShader;
                Main.playerDrawData.Add(drawData);

                //Generate visual dust
                if (Main.rand.NextFloat() < 0.1f)
                {
                    Vector2 position = drawPlayer.Center - new Vector2(8f, 0f) + new Vector2(Main.rand.Next(8), Main.rand.Next(8));
                    if (drawPlayer.direction == 1)
                    {
                        position.X += 8f;
                    }
                    Dust dust = Dust.NewDustPerfect(position, 135, new Vector2(Main.rand.NextFloat(-0.3f, 0.3f), Main.rand.NextFloat(-0.3f, -0.1f)), 100, Color.White, 0.6f);
                    dust.noGravity = true;
                    dust.noLight = true;
                    dust.fadeIn = Main.rand.NextFloat(0.5f, 0.8f);

                    dust.shader = GameShaders.Armor.GetSecondaryShader(drawInfo.bodyArmorShader, drawPlayer);
                }
            }
        });

        public override void ModifyDrawLayers(List<PlayerLayer> layers)
        {
            int wingLayer = layers.FindIndex(PlayerLayer => PlayerLayer.Name.Equals("Wings"));
            if (player.inventory[player.selectedItem].type == mod.ItemType<Items.Weapons.SlimeHandlerKnapsack>())
            {
                if (player.velocity.Y == 0f)
                {
                    layers.RemoveAt(wingLayer);
                }
                layers.Insert(wingLayer + 1, SlimeHandlerKnapsack);
            }
            else
            {
                if (wingLayer != -1)
                {
                    HarvesterWings.visible = true;
                    layers.Insert(wingLayer + 1, HarvesterWings);
                }
            }

            int bodyLayer = layers.FindIndex(PlayerLayer => PlayerLayer.Name.Equals("Body"));
            layers.Insert(bodyLayer + 1, SoulSaviorGlowmask);
        }

        public override void ModifyHitByProjectile(Projectile proj, ref int damage, ref bool crit)
        {
            ResetEmpoweringTimer();

            SpawnSoulTemp();
        }

        public override void ModifyHitByNPC(NPC npc, ref int damage, ref bool crit)
        {
            ResetEmpoweringTimer();

            SpawnSoulTemp();
        }

        public override void ModifyHitNPC(Item item, NPC target, ref int damage, ref float knockback, ref bool crit)
        {
            ApplyCandleDebuffs(target);
        }

        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            ApplyCandleDebuffs(target);
        }

        public override void ModifyHitPvp(Item item, Player target, ref int damage, ref bool crit)
        {
            ApplyCandleDebuffs(target);

            ResetEmpoweringTimer();

            SpawnSoulTemp();
        }

        public override void ModifyHitPvpWithProj(Projectile proj, Player target, ref int damage, ref bool crit)
        {
            ApplyCandleDebuffs(target);

            ResetEmpoweringTimer();

            SpawnSoulTemp();
        }

        public override void GetWeaponDamage(Item item, ref int damage)
        {
            if (empoweringBuff && !item.summon && damage > 0) damage += (int)(damage * step); //summon damage gets handled in AssGlobalProj
        }

        public override void GetWeaponCrit(Item item, ref int crit)
        {
            if (empoweringBuff) crit += (int)(10 * step);
        }

        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {
            //getDefense before teleportHome (so you don't teleport BEFORE you gain the defense)

            if (!GetDefense(damage)) return false;

            if (!TeleportHome(damage)) return false;

            if (NPC.AnyNPCs(NPCID.Plantera)) planteraGitGudCounter++;

            return base.PreKill(damage, hitDirection, pvp, ref playSound, ref genGore, ref damageSource);
        }

        public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {
            if (getDefenseDuration != 0) damage = 1;

            return base.PreHurt(pvp, quiet, ref damage, ref hitDirection, ref crit, ref customDamage, ref playSound, ref genGore, ref damageSource);
        }

        public override void PostUpdateBuffs()
        {
            UpdateTeleportHomeWhenLow();

            UpdateGetDefenseWhenLow();

            Empower();
        }

        public override void PreUpdate()
        {
            SpawnSoulsWhenHarvesterIsAlive();

            RightClickStatus();

            LeftClickStatus();
        }
    }
}
