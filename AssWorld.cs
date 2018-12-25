using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using AssortedCrazyThings.NPCs;
using AssortedCrazyThings.NPCs.DungeonBird;

namespace AssortedCrazyThings
{
	public class AssWorld : ModWorld
	{
        //basically "if they were alive last update"
        public bool lilmegalodonAlive = false;
        public bool megalodonAlive = false;
        public bool miniocramAlive = false;
        //"are they alive this update"
        bool lilmegalodonSpawned;
        bool isMegalodonSpawned;
        bool isMiniocramSpawned;
        //static names, in case you want to change them later
        public static string lilmegalodonName = LittleMegalodon.name;
        public static string megalodonName = Megalodon.name;
        public static string miniocramName = SpawnOfOcram.name;
        public static string lilmegalodonMessage = Megalodon.message;
        public static string megalodonMessage = Megalodon.message;
        public static string miniocramMessage = SpawnOfOcram.message;
        //the megalodon messages are modified down below in the Disappear message

        //Soul stuff
        public static int[] harvesterTypes = new int[3];
        public static bool downedHarvester;
        public static bool spawnHarvester;

        //Mods loaded
        public static bool isPlayerHealthManaBarLoaded = false;

        public override void Initialize()
        {
            lilmegalodonAlive = false;
            megalodonAlive = false;
            miniocramAlive = false;

            harvesterTypes[0] = mod.NPCType(aaaHarvester1.typeName);
            harvesterTypes[1] = mod.NPCType(aaaHarvester2.typeName);
            harvesterTypes[2] = mod.NPCType(aaaHarvester3.typeName);
            downedHarvester = false; //cant spawn more than once in each start of a world
            spawnHarvester = false;

        isPlayerHealthManaBarLoaded = ModLoader.GetMod("PlayerHealthManaBar") != null;
        }

        private void UpdateHarvesterSpawn()
        {
            if (!Main.dayTime) //if night
            {
                if (!Main.fastForwardTime)
                {
                    if (spawnHarvester && Main.netMode != 1 && Main.time > 4860.0) //after 4860.0 ticks, 81 seconds, spawn
                    {
                        for (int k = 0; k < 255; k++)
                        {
                            if (Main.player[k].active && !Main.player[k].dead/* && (double)Main.player[k].position.Y < Main.worldSurface * 16.0*/)
                            {
                                NPC.SpawnOnPlayer(k, harvesterTypes[0]);
                                AwakeningMessage(BaseHarvester.message);
                                spawnHarvester = false;
                                break;
                            }
                        }
                    }
                    if (Main.time >= 32400.0) //32400 is the last tick of the night
                    {
                        spawnHarvester = true;
                    }
                }
            }
            else //if day
            {
                //32400
                if (Main.time >= 54000.0) //54000 is the last tick of the day
                {
                    if (!Main.fastForwardTime)
                    {
                        //skeletron defeated
                        if (!downedHarvester && NPC.downedBoss3 && Main.netMode != 1)
                        {
                            bool flag3 = false;
                            for (int n = 0; n < 255; n++)
                            {
                                if (Main.player[n].active && Main.player[n].statLifeMax >= 300)
                                {
                                    flag3 = true;
                                    break;
                                }
                            }
                            if (flag3/* && Main.rand.Next(3) == 0*/)
                            {
                                spawnHarvester = true;
                                AwakeningMessage("Bring a bug net to the dungeon, don't ask why...");
                            }
                        }
                    }
                }
            }
        }

        //small methods I made for myself to not make the code cluttered since I have to use these six times
        public static void AwakeningMessage(string message, Vector2 pos = default(Vector2), int soundStyle = -1)
        {
            if(soundStyle != -1) Main.PlaySound(SoundID.Roar, pos, soundStyle); //soundStyle 2 for screech, 0 for regular roar
            if (Main.netMode == NetmodeID.SinglePlayer)
            {
                Main.NewText(message, 175, 75, 255);
            }
            else if (Main.netMode == NetmodeID.Server)
            {
                NetMessage.BroadcastChatMessage(NetworkText.FromLiteral(message), new Color(175, 75, 255));
            }
        }

        public static void DisappearMessage(string message)
        {
            if (Main.netMode == NetmodeID.SinglePlayer)
            {
                Main.NewText(message, 175, 255, 175);
            }
            else if (Main.netMode == NetmodeID.Server)
            {
                NetMessage.BroadcastChatMessage(NetworkText.FromLiteral(message), new Color(175, 255, 175));
            }
        }

        //not used anywhere, but might be helpful
        private void KillInstantly(NPC npc)
        {
            // These 3 lines instantly kill the npc without showing damage numbers, dropping loot, or playing DeathSound. Use this for instant deaths
            npc.life = 0;
            npc.HitEffect();
            npc.active = false;
            Main.PlaySound(SoundID.NPCDeath16, npc.position); // plays a fizzle sound
        }

        public override void PostUpdate()
		{
            UpdateHarvesterSpawn();
            //those flags are checked for trueness each update
            lilmegalodonSpawned = false;
            isMiniocramSpawned = false;
            isMegalodonSpawned = false;
            for (short j = 0; j < 200; j++)
            {
                if (Main.npc[j].active)
                {
                    if(Main.npc[j].TypeName == lilmegalodonName && !lilmegalodonSpawned)
                    {
                        lilmegalodonSpawned = true;
                        //check if it wasnt alive in previous update
                        if(!lilmegalodonAlive)
                        {
                            AwakeningMessage(lilmegalodonMessage, Main.npc[j].position, 0);
                            lilmegalodonAlive = true;
                        }
                    }
                    if (Main.npc[j].TypeName == megalodonName && !isMegalodonSpawned)
                    {
                        isMegalodonSpawned = true;
                        if (!megalodonAlive)
                        {
                            AwakeningMessage(megalodonMessage, Main.npc[j].position, 0);
                            megalodonAlive = true;
                        }
                    }
                    if (Main.npc[j].TypeName == miniocramName && !isMiniocramSpawned)
                    {
                        isMiniocramSpawned = true;
                        if (!miniocramAlive)
                        {
                            AwakeningMessage(miniocramMessage, Main.npc[j].position, 0);
                            miniocramAlive = true;
                        }
                    }
                }
            }
            //after this we know that either atleast one miniboss is active or not
            //if alive, but not active, print disappear message
            if (!lilmegalodonSpawned && lilmegalodonAlive)
            {
                lilmegalodonAlive = false;
                DisappearMessage("The " + megalodonName + " disappeared... for now.");
            }
            if (!isMegalodonSpawned && megalodonAlive)
            {
                megalodonAlive = false;
                DisappearMessage("The " + megalodonName + " disappeared... for now.");
            }
            if (!isMiniocramSpawned && miniocramAlive)
            {
                miniocramAlive = false;
                DisappearMessage("The " + miniocramName + " disappeared... for now.");
            }

            //
        }

        //public override void NetSend(BinaryWriter writer)
        //{
        //    BitsByte flags = new BitsByte();
        //    flags[0] = megasnailAlive;
        //    flags[1] = miniocramAlive;
        //    flags[2] = megalodonAlive;
        //    writer.Write(flags);
        //}

        //public override void NetReceive(BinaryReader reader)
        //{
        //    BitsByte flags = reader.ReadByte();
        //    megasnailAlive = flags[0];
        //    miniocramAlive = flags[1];
        //    megalodonAlive = flags[2];
        //}

        public override void PreUpdate()
        {
            //track fallen NPCs every 6 ticks and replace them with souls

            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                if(Main.time % 60 == 15 && NPC.CountNPCS(mod.NPCType(aaaSoul.name)) > 10) //limit soul count in the world to 10
                {
                    short oldest = 200;
                    int timeleftmin = int.MaxValue;
                    for (short j = 0; j < 200; j++)
                    {
                        if (Main.npc[j].active && Main.npc[j].type == mod.NPCType(aaaSoul.name))
                        {
                            if(Main.npc[j].timeLeft < timeleftmin)
                            {
                                timeleftmin = Main.npc[j].timeLeft;
                                oldest = j;
                            }
                        }
                    }
                    if (oldest != 200)
                    {
                        Main.npc[oldest].life = 0;
                        Main.npc[oldest].active = false;
                    }
                }
            } //end Main.NetMode
        } //end PreUpdate 
    }
}