using UniversalTweaks.Tweaks;

using Xunit;

namespace UniversalTweaks.Tests;

public class TextureSwapTests
{
    [Fact]
    public void ResolveTextureName_MreWithVariantDisabled_ReturnsEmpty()
    {
        var result = TextureSwap.ResolveTextureName("GEAR_MRE", mreTextureVariantEnabled: false);

        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void ResolveTextureName_MreWithVariantEnabled_ReturnsBrownMreTexture()
    {
        var result = TextureSwap.ResolveTextureName("GEAR_MRE", mreTextureVariantEnabled: true);

        Assert.Equal("ico_GearItem__BrownMRE", result);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void ResolveTextureName_UnmappedGearItem_ReturnsEmpty(bool mreTextureVariantEnabled)
    {
        var result = TextureSwap.ResolveTextureName("GEAR_SomeOtherItem", mreTextureVariantEnabled);

        Assert.Equal(string.Empty, result);
    }
}