using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Harblesnargits_Mod_01.NPCs
{
	public class CuteSlimeRainbow : ModNPC
		{
			public override void SetStaticDefaults()
				{
					DisplayName.SetDefault("Cute Slime");
					Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.ToxicSludge];
				}
			public override void SetDefaults()
				{
					npc.width = 42;
					npc.height = 52;
					npc.scale = 1.2f;
					npc.friendly = true;
					npc.damage = 0;
					npc.defense = 2;
					npc.lifeMax = 5;
					npc.HitSound = SoundID.NPCHit1;
					npc.DeathSound = SoundID.NPCDeath1;
					npc.value = 25f;
					npc.knockBackResist = 0.3f;
					npc.aiStyle = 1;
					aiType = NPCID.ToxicSludge;
					animationType = NPCID.ToxicSludge;
					Main.npcCatchable[mod.NPCType("CuteSlimeRainbow")] = true;
					npc.catchItem = (short)mod.ItemType("CuteSlimeRainbow");
				}
			public override float SpawnChance(NPCSpawnInfo spawnInfo)
				{
					return SpawnCondition.OverworldDaySlime.Chance * 0.025f;
				}
			public override void NPCLoot()
				{
					{
						Item.NewItem(npc.getRect(), ItemID.Gel);
					}
				}
			public override Color? GetAlpha(Color lightColor)
				{
					return Main.DiscoColor;
				}
			public override void HitEffect(int hitDirection, double damage)
				{
					{
						if (npc.life <= 0)
							{
							}
					}
				}
		}
}
