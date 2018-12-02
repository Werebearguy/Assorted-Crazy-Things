using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedCrazyThings.Items.Accessories.Useful
{
	public class EverburningCandle : ModItem
		{
			public override void SetStaticDefaults()
				{
					DisplayName.SetDefault("Everburning Candle");
					Tooltip.SetDefault("Inflicts fire damage on all attacks");
				}
			public override void SetDefaults()
				{
					item.width = 24;
					item.height = 22;
					item.value = 0;
					item.rare = -11;
					item.accessory = true;
				}
			public override void UpdateAccessory(Player player, bool hideVisual)
				{
					player.GetModPlayer<SimpleModPlayer>().everburningCandleBuff = true;
				}
			public override void AddRecipes()
				{
					ModRecipe recipe = new ModRecipe(mod);
					recipe.AddIngredient(ItemID.MagmaStone, 1);
					recipe.AddIngredient(ItemID.SoulofLight, 50);
					recipe.AddTile(TileID.TinkerersWorkbench);
					recipe.SetResult(this);
					recipe.AddRecipe();
				}
		}
}