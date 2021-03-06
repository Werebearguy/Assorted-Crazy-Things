using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.ID;

namespace AssortedCrazyThings.NPCs.DungeonBird
{
    public class Harvester1 : HarvesterBase
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault(name);
            Main.npcFrameCount[npc.type] = 9;
        }

        public override void SetDefaults()
        {
            maxVeloScale = 1.3f; //1.3f default
            maxAccScale = 0.04f; //0.04f default
            stuckTime = 6; //*30 for ticks, *0.5 for seconds
            afterEatTime = 60;
            eatTime = EatTimeConst - 30; // +60
            idleTime = IdleTimeConst;
            hungerTime = 2040; //AI_Timer //1000
            maxSoulsEaten = 5; //3
            jumpRange = 160;//also noclip detect range //100 for restricted v
            restrictedSoulSearch = true;
            noDamage = true;

            transformTime = 120;
            soulsEaten = 0;
            stopTime = idleTime;
            aiTargetType = Target_Soul;
            target = 0;
            stuckTimer = 0;
            rndJump = 0;
            transformServer = false;
            transformTo = AssWorld.harvesterTypes[1];

            defLifeMax = maxSoulsEaten + 1;


            npc.dontTakeDamage = true;  //if true, it wont show hp count while mouse over
            npc.chaseable = false;
            npc.npcSlots = 0.5f;
            npc.width = DungeonSoulBase.wid;
            npc.height = DungeonSoulBase.hei;
            npc.damage = 0;
            npc.defense = 1;
            npc.scale = defScale;
            npc.lifeMax = defLifeMax;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.aiStyle = -1; //91;
            aiType = -1; //91
            npc.alpha = 255;
            animationType = -1;
            npc.lavaImmune = true;
            npc.buffImmune[BuffID.Confused] = false;
            npc.timeLeft = NPC.activeTime * 30; //doesnt do jackshit
        }

        public override void FindFrame(int frameHeight)
        {
            //npc.spriteDirection = npc.velocity.X <= 0f ? 1 : -1; //flipped in the sprite
            npc.spriteDirection = -npc.direction;
            npc.gfxOffY = 0f;
            if (AI_State == STATE_APPROACH)
            {
                npc.frameCounter++;
                if (npc.velocity.X != 0)
                {
                    if (npc.velocity.Y == 0)
                    {
                        if (npc.frameCounter <= 8.0)
                        {
                            npc.frame.Y = frameHeight * 3;
                        }
                        else if (npc.frameCounter <= 16.0)
                        {
                            npc.frame.Y = frameHeight * 4;
                        }
                        else if (npc.frameCounter <= 24.0)
                        {
                            npc.frame.Y = frameHeight * 3;
                        }
                        else if (npc.frameCounter <= 32.0)
                        {
                            npc.frame.Y = frameHeight * 5;
                        }
                        else
                        {
                            npc.frameCounter = 0;
                        }
                    }
                    else
                    {
                        npc.frame.Y = frameHeight * 6;
                    }
                }
                else
                {
                    npc.frame.Y = frameHeight * 3;
                }
            }
            else if (AI_State == STATE_NOCLIP)
            {
                npc.frameCounter++;
                if (npc.frameCounter <= 3.0)
                {
                    npc.frame.Y = frameHeight * 7; //"fly"
                }
                else if (npc.frameCounter <= 6.0)
                {
                    npc.frame.Y = frameHeight * 8;
                }
                else
                {
                    npc.frameCounter = 0;
                }
            }
            else if (AI_State == STATE_TRANSFORM)
            {
                npc.gfxOffY += 1f;
                npc.frame.Y = 0;
            }
            else if (AI_State == STATE_STOP)
            {
                npc.gfxOffY += 1f;
                if (stopTime == eatTime)
                {
                    npc.frameCounter++;
                    if (npc.velocity.Y == 0 || npc.velocity.Y < 3f && npc.velocity.Y > 0f)
                    {
                        if (npc.frameCounter <= 8.0)
                        {
                            npc.frame.Y = 0;
                        }
                        else if (npc.frameCounter <= 16.0)
                        {
                            npc.frame.Y = frameHeight * 1;
                        }
                        else if (npc.frameCounter <= 24.0)
                        {
                            npc.frame.Y = frameHeight * 2;
                        }
                        else if (npc.frameCounter <= 32.0)
                        {
                            npc.frame.Y = frameHeight * 1;
                        }
                        else
                        {
                            npc.frameCounter = 0;
                        }
                    }
                    else if (!SolidCollisionNew(npc.position + new Vector2(-1f, -1f), npc.width + 2, npc.height + 10))
                    {
                        npc.frame.Y = frameHeight * 6;
                    }
                }
                else
                {
                    npc.frame.Y = 0;
                }
            }
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            if (AI_State == STATE_STOP && stopTime == eatTime)
            {
                Texture2D texture = mod.GetTexture("NPCs/DungeonBird/Harvester1Souleat");
                Vector2 stupidOffset = new Vector2(0f, 3f + npc.gfxOffY); //gfxoffY is for when the npc is on a slope or half brick
                SpriteEffects effect = npc.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
                Vector2 drawOrigin = new Vector2(npc.width * 0.5f, npc.height * 0.5f);
                Vector2 drawPos = npc.position - Main.screenPosition + drawOrigin + stupidOffset;

                drawColor.R = Math.Max(drawColor.R, (byte)200);
                drawColor.G = Math.Max(drawColor.G, (byte)200);
                drawColor.B = Math.Max(drawColor.B, (byte)200);
                spriteBatch.Draw(texture, drawPos, npc.frame, drawColor, npc.rotation, npc.frame.Size() / 2, npc.scale, effect, 0f);
            }
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            //from server to client
            writer.Write((byte)soulsEaten);
            writer.Write((byte)stuckTimer);
            writer.Write((byte)rndJump);
            writer.Write((short)target);
            BitsByte flags = new BitsByte();
            flags[0] = aiInit;
            flags[1] = aiTargetType;
            flags[2] = transformServer;
            writer.Write(flags);
            //Print("send: " + AI_State + " " + stuckTimer.ToString() + " " + target.ToString() + " " + transformServer.ToString());
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            //from client to server
            soulsEaten = reader.ReadByte();
            stuckTimer = reader.ReadByte();
            rndJump = reader.ReadByte();
            target = reader.ReadInt16();
            BitsByte flags = reader.ReadByte();
            aiInit = flags[0];
            aiTargetType = flags[1];
            transformServer = flags[2];
            //Print("recv: " + AI_State + " " + stuckTimer.ToString() + " " + target.ToString() + " " + transformServer.ToString());
        }

        public override void AI()
        {
            HarvesterAI(allowNoclip: !restrictedSoulSearch);
        }
    }
}
