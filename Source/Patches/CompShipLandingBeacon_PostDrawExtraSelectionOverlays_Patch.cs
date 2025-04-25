using System.Collections.Generic;
using System.Reflection.Emit;
using RimWorld;
using HarmonyLib;
// Ignore Spelling: Gauranlen, Plantable
#if RIMWORLD_15_OR_GREATER
namespace Drummeur.BetterLinkableColors.Patches;

/// <summary>
/// A <see cref="HarmonyPatch" /> to patch the <see cref="CompShipLandingBeacon.PostDrawExtraSelectionOverlays()" /> method.
/// </summary>
[HarmonyPatch(typeof(CompShipLandingBeacon), nameof(CompShipLandingBeacon.PostDrawExtraSelectionOverlays))]
public static class CompShipLandingBeacon_PostDrawExtraSelectionOverlays_Patch
{
    /// <summary>
    /// A <see cref="HarmonyTranspiler" />.
    /// </summary>
    /// <param name="instructions">A collection of <see cref="CodeInstruction" />s representing the IL code of the original method.</param>
    /// <returns>A collection of <see cref="CodeInstruction" />s representing the IL code of the new method.</returns>
    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
    {
        return BetterLinkableColorsPatches.Patch_PostDrawExtraSelectionOverlays_For_Vtc3_Vtc3_SC_float(instructions, generator, BetterLinkableColorsPatches.ActiveLineField);
    }
}
#endif
