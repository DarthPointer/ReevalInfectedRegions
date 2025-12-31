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
public class ReevalInfectedRegionsMod : BaseUnityPlugin
{
    public void OnEnable()
    {
        On.PlayerProgression.SaveToDisk += PlayerProgression_SaveToDisk;
    }

    public void OnDisabe()
    {
        On.PlayerProgression.SaveToDisk -= PlayerProgression_SaveToDisk;
    }

    private bool PlayerProgression_SaveToDisk(On.PlayerProgression.orig_SaveToDisk orig, PlayerProgression self, bool saveCurrentState, bool saveMaps, bool saveMiscProg)
    {
        self.currentSaveState.miscWorldSaveData.regionsInfectedBySentientRot = [..
            self.currentSaveState.miscWorldSaveData.regionsInfectedBySentientRot.Where(
            (region) =>
            (self.currentSaveState.regionStates.First((regionState) => regionState.regionName == region)
            ?.sentientRotProgression?.Where((kvp) => (kvp.Value?.rotIntensity ?? 0) > 0)
            .Count() ?? 0) > 0)];

        self.currentSaveState.miscWorldSaveData.regionsInfectedBySentientRotSpread = [..
            self.currentSaveState.miscWorldSaveData.regionsInfectedBySentientRotSpread.Where(
            (region) =>
            (self.currentSaveState.regionStates.First((regionState) => regionState.regionName == region)
            ?.sentientRotProgression?.Where((kvp) => (kvp.Value?.rotIntensity ?? 0)> 0)
            .Count() ?? 0) > 0)];

        return orig(self, saveCurrentState, saveMaps, saveMiscProg);
    }
}
