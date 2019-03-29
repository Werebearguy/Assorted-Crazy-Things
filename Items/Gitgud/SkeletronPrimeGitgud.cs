using Terraria;

namespace AssortedCrazyThings.Items.Gitgud
{
    public class SkeletronPrimeGitgud : GitgudItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Clock Set Ten Years Ahead");
        }

        public override void MoreSetDefaults()
        {
            item.width = 32;
            item.height = 32;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<GitGudPlayer>(mod).skeletronPrimeGitgud = true;
        }
    }
}
