using UniversalTweaks.Core;

namespace UniversalTweaks.Tests;

public class InventoryIconsTests
{
    [Fact]
    public void The_mre_keeps_the_games_icon_with_the_brown_mre_off() =>
        Assert.Equal(string.Empty, InventoryIcons.ResolveTextureName(InventoryIcons.MreGearName, mreTextureVariantEnabled: false));

    [Fact]
    public void The_mre_gets_the_brown_icon_with_the_brown_mre_on() =>
        Assert.Equal("ico_GearItem__BrownMRE",
            InventoryIcons.ResolveTextureName(InventoryIcons.MreGearName, mreTextureVariantEnabled: true));

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void Other_items_keep_the_games_icon(bool mreTextureVariantEnabled) =>
        Assert.Equal(string.Empty, InventoryIcons.ResolveTextureName("GEAR_SomeOtherItem", mreTextureVariantEnabled));
}
