using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace AssortedCrazyThings.NPCs
{
    public class DemonEyeSeasonal : DemonEyeRecolorBase
    {
        public override int TotalNumberOfThese => 4;

        /*OG = 0
        * OP = 1
        * UG = 2
        * UP = 3
        */
        public override string Texture
        {
            get
            {
                return "AssortedCrazyThings/NPCs/DemonEyeSeasonal_0"; //use fixed texture
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (Main.halloween)
            {
                return SpawnCondition.OverworldNightMonster.Chance * 0.025f;
            }
            return 0f;
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (NPC.life <= 0)
            {
                switch ((int)AiTexture)
                {
                    case 0:
                        Gore.NewGore(NPC.position, NPC.velocity, Mod.Find<ModGore>("DemonEyeGreenGore_1").Type, 1f);
                        Gore.NewGore(NPC.position, NPC.velocity, Mod.Find<ModGore>("DemonEyeGreenGore_0").Type, 1f);
                        break;
                    case 1:
                        Gore.NewGore(NPC.position, NPC.velocity, Mod.Find<ModGore>("DemonEyePurpleGore_1").Type, 1f);
                        Gore.NewGore(NPC.position, NPC.velocity, Mod.Find<ModGore>("DemonEyePurpleGore_0").Type, 1f);
                        break;
                    case 2:
                        Gore.NewGore(NPC.position, NPC.velocity, Mod.Find<ModGore>("DemonEyeGreenGore_1").Type, 1f);
                        Gore.NewGore(NPC.position, NPC.velocity, Mod.Find<ModGore>("DemonEyeGreenGore_0").Type, 1f);
                        break;
                    case 3:
                        Gore.NewGore(NPC.position, NPC.velocity, Mod.Find<ModGore>("DemonEyePurpleGore_1").Type, 1f);
                        Gore.NewGore(NPC.position, NPC.velocity, Mod.Find<ModGore>("DemonEyePurpleGore_0").Type, 1f);
                        break;
                    default:
                        break;
                }
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = Mod.GetTexture("NPCs/DemonEyeSeasonal_" + AiTexture).Value;
            Vector2 stupidOffset = new Vector2(0f, NPC.height / 3); //gfxoffY is for when the npc is on a slope or half brick
            SpriteEffects effect = NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Vector2 drawOrigin = new Vector2(NPC.width * 0.5f, NPC.height * 0.5f);
            Vector2 drawPos = NPC.position - screenPos + drawOrigin + stupidOffset;
            spriteBatch.Draw(texture, drawPos, NPC.frame, drawColor, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, effect, 0f);
            return false;
        }
    }
}
