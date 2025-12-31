using System.Security.Permissions;
using System.Security;
using BepInEx;
using System.Linq;

#pragma warning disable CS0618 // Type or member is obsolete
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]
[module: UnverifiableCode]
#pragma warning restore CS0618 // Type or member is obsolete

namespace ReevalInfectedRegions;

[BepInPlugin("DarthPointer.ReevalInfectedRegions", "Reeval Infected Regions", "0.0")]
public class ReevalInfectedRegionsMod
{
    public void OnEnable()
    {
        On.SaveState.SaveToString += DaHook;
    }

    public void OnDisabe()
    {
        On.SaveState.SaveToString -= DaHook;
    }

    private string DaHook(On.SaveState.orig_SaveToString orig, SaveState self)
    {
        self.miscWorldSaveData.regionsInfectedBySentientRot = [.. self.miscWorldSaveData.regionsInfectedBySentientRot.Where(
            (region) =>
            self.regionStates.First((regionState) => regionState.regionName == region)
            .sentientRotProgression.Where((kvp) => kvp.Value.rotIntensity > 0)
            .Count() > 0)];

        self.miscWorldSaveData.regionsInfectedBySentientRotSpread = [.. self.miscWorldSaveData.regionsInfectedBySentientRotSpread.Where(
            (region) =>
            self.regionStates.First((regionState) => regionState.regionName == region)
            .sentientRotProgression.Where((kvp) => kvp.Value.rotIntensity > 0)
            .Count() > 0)];

        return orig(self);
    }
}
