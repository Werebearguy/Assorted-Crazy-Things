using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs
{
    public class Cloudfish : ModNPC
    {
        public float scareRange = 200f;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cloudfish");
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.Goldfish];
        }

        public override void SetDefaults()
        {
            npc.width = 38;
            npc.height = 36;
            npc.damage = 0;
            npc.defense = 0;
            npc.lifeMax = 5;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.value = 0f;
            npc.knockBackResist = 0.25f;
            npc.aiStyle = -1; //custom
            aiType = NPCID.Goldfish;
            animationType = NPCID.Goldfish;
            npc.noGravity = true;
            Main.npcCatchable[mod.NPCType("Cloudfish")] = true;
            npc.catchItem = ItemID.Cloudfish;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.player.ZoneSkyHeight)
            {
                if (Main.tile[spawnInfo.spawnTileX, spawnInfo.spawnTileY].liquid == 0)
                {
                    return 0f;
                }
                else if (
                   !WorldGen.SolidTile(spawnInfo.spawnTileX, spawnInfo.spawnTileY) &&
                   !WorldGen.SolidTile(spawnInfo.spawnTileX, spawnInfo.spawnTileY + 1) &&
                   !WorldGen.SolidTile(spawnInfo.spawnTileX, spawnInfo.spawnTileY + 2))
                {
                    return SpawnCondition.Sky.Chance * 4f; //0.05f before, 100f now because water check
                }
            }
            return 0f;
        }

        //public override int SpawnNPC(int tileX, int tileY)
        //{
        //    if (Main.tile[tileX, tileY].liquid == 0)
        //    {
        //        return 0;
        //    }
        //    else if (
        //       !WorldGen.SolidTile(tileX, tileY) &&
        //       !WorldGen.SolidTile(tileX, tileY + 1) &&
        //       !WorldGen.SolidTile(tileX, tileY + 2))
        //    {
        //        //actually spawn
        //        return base.SpawnNPC(tileX, tileY);
        //    }
        //    return 0;
        //}

        public override void NPCLoot()
        {
            {
                Item.NewItem(npc.getRect(), ItemID.Cloud, 10 + Main.rand.Next(20));
                if (Main.rand.NextBool(10))
                    Item.NewItem(npc.getRect(), ItemID.RainCloud, 10 + Main.rand.Next(20));
                if (Main.rand.NextBool(15))
                    Item.NewItem(npc.getRect(), ItemID.SnowCloudBlock, 10 + Main.rand.Next(20));
            }
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life <= 0)
            {
            }
        }
		
		public override void AI()
		{
            //modified foldfish AI
			if (npc.direction == 0)
			{
				npc.TargetClosest();
			}
			if (npc.wet)
			{
				bool flag12 = false;
                npc.TargetClosest(faceTarget: false);
                Vector2 centerpos = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
                Vector2 playerpos = new Vector2(Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2),
                                                Main.player[npc.target].position.Y + (float)(Main.player[npc.target].height / 2));
                float distancex = playerpos.X - centerpos.X; //positive if player on the right
                float distancey = playerpos.Y - centerpos.Y; //positive if player below
                float distancen = (float)Math.Sqrt((double)(distancex * distancex + distancey * distancey)); //distance between player and fish
                if (Main.player[npc.target].wet && distancen < scareRange)
                {
                    if(!Main.player[npc.target].dead)
                    {
                        flag12 = true;
                    }
                }
                if (!flag12)
				{
                    if (npc.collideX)
                    {
                        npc.velocity.X = npc.velocity.X * -1f;
						npc.direction *= -1;
                        npc.netUpdate = true;
					}
					if (npc.collideY)
					{
                        npc.netUpdate = true;
						if (npc.velocity.Y > 0f)
						{
							npc.velocity.Y = Math.Abs(npc.velocity.Y) * -1f;
							npc.directionY = -1;
							npc.ai[0] = -1f;
						}
						else if (npc.velocity.Y < 0f)
						{
							npc.velocity.Y = Math.Abs(npc.velocity.Y);
							npc.directionY = 1;
							npc.ai[0] = 1f;
						}
					}
				}
				if (flag12) //if target is in water
				{
                    npc.TargetClosest(faceTarget: false);
                    //face away from the player
                    npc.direction = (distancex >= 0f) ? -1 : 1;
                    npc.directionY = (distancey >= 0f) ? -1 : 1;


                    npc.velocity.X = npc.velocity.X + (float)npc.direction * 0.1f;
					npc.velocity.Y = npc.velocity.Y + (float)npc.directionY * 0.1f;


					if (npc.velocity.X > 3f)
					{
						npc.velocity.X = 3f;
					}
					if (npc.velocity.X < -3f)
					{
						npc.velocity.X = -3f;
					}
					if (npc.velocity.Y > 2f)
					{
						npc.velocity.Y = 2f;
					}
					if (npc.velocity.Y < -2f)
					{
						npc.velocity.Y = -2f;
					}
				}
				else
				{
					npc.velocity.X = npc.velocity.X + (float)npc.direction * 0.1f;
					if (npc.velocity.X < -1f || npc.velocity.X > 1f)
					{
						npc.velocity.X = npc.velocity.X * 0.95f;
					}
					if (npc.ai[0] == -1f)
					{
						npc.velocity.Y = npc.velocity.Y - 0.01f;
						if ((double)npc.velocity.Y < -0.3)
						{
							npc.ai[0] = 1f;
						}
					}
					else
					{
						npc.velocity.Y = npc.velocity.Y + 0.01f;
						if ((double)npc.velocity.Y > 0.3)
						{
							npc.ai[0] = -1f;
						}
					}
					int num261 = (int)(npc.position.X + (float)(npc.width / 2)) / 16;
					int num262 = (int)(npc.position.Y + (float)(npc.height / 2)) / 16;
					if (Main.tile[num261, num262 - 1] == null)
					{
						Tile[,] tile3 = Main.tile;
						int num263 = num261;
						int num264 = num262 - 1;
						Tile tile4 = new Tile();
						tile3[num263, num264] = tile4;
					}
					if (Main.tile[num261, num262 + 1] == null)
					{
						Tile[,] tile5 = Main.tile;
						int num265 = num261;
						int num266 = num262 + 1;
						Tile tile6 = new Tile();
						tile5[num265, num266] = tile6;
					}
					if (Main.tile[num261, num262 + 2] == null)
					{
						Tile[,] tile7 = Main.tile;
						int num267 = num261;
						int num268 = num262 + 2;
						Tile tile8 = new Tile();
						tile7[num267, num268] = tile8;
					}
					if (Main.tile[num261, num262 - 1].liquid > 128)
					{
						if (Main.tile[num261, num262 + 1].active())
						{
							npc.ai[0] = -1f;
						}
						else if (Main.tile[num261, num262 + 2].active())
						{
							npc.ai[0] = -1f;
						}
					}
					if ((double)npc.velocity.Y > 0.4 || (double)npc.velocity.Y < -0.4)
					{
						npc.velocity.Y = npc.velocity.Y * 0.95f;
					}
				}
			}
			else //not wet, frantically jump around
			{
				if (npc.velocity.Y == 0f)
				{
					if (Main.netMode != 1)
					{
						npc.velocity.Y = (float)Main.rand.Next(-50, -20) * 0.1f;
						npc.velocity.X = (float)Main.rand.Next(-20, 20) * 0.1f;
						npc.netUpdate = true;
					}
				}
				npc.velocity.Y = npc.velocity.Y + 0.3f;
				if (npc.velocity.Y > 10f)
				{
					npc.velocity.Y = 10f;
				}
				npc.ai[0] = 1f;
			}
			npc.rotation = npc.velocity.Y * (float)npc.direction * 0.1f;
			if ((double)npc.rotation < -0.2)
			{
				npc.rotation = -0.2f;
			}
			if ((double)npc.rotation > 0.2)
			{
				npc.rotation = 0.2f;
			}
		}
    }
}
