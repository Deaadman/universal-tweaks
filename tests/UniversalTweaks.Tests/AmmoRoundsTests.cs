using UniversalTweaks.Core;

namespace UniversalTweaks.Tests;

public class AmmoRoundsTests
{
    [Theory]
    [InlineData("GEAR_RifleAmmoBox", "GEAR_RifleAmmoSingle")]
    [InlineData("GEAR_RevolverAmmoBox", "GEAR_RevolverAmmoSingle")]
    [InlineData("GEAR_RifleAmmoBox(Clone)", "GEAR_RifleAmmoSingle")]
    [InlineData("GEAR_RevolverAmmoBox (Clone)", "GEAR_RevolverAmmoSingle")]
    public void One_round_from_a_box_drops_as_a_single_round(string box, string single) =>
        Assert.Equal(single, AmmoRounds.GetSingleRound(box, 1));

    [Theory]
    [InlineData(0)]
    [InlineData(2)]
    [InlineData(10)]
    public void More_than_one_round_stays_a_box(int units) =>
        Assert.Null(AmmoRounds.GetSingleRound("GEAR_RifleAmmoBox", units));

    [Theory]
    [InlineData("GEAR_RifleAmmoSingle")]
    [InlineData("GEAR_FlareGunAmmoSingle")]
    [InlineData("GEAR_Stick")]
    public void Other_items_are_left_alone(string gearItemName) =>
        Assert.Null(AmmoRounds.GetSingleRound(gearItemName, 1));
}
