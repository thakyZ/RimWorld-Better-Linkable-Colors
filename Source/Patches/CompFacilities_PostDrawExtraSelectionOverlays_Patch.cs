using System.Collections.Generic;
using System.Reflection.Emit;
using RimWorld;
using HarmonyLib;
// Ignore Spelling: Gauranlen, Plantable
namespace Drummeur.BetterLinkableColors.Patches;

/// <summary>
/// A <see cref="HarmonyPatch" /> to patch the <see cref="CompFacility.PostDrawExtraSelectionOverlays()" /> method.
/// </summary>
[HarmonyPatch(typeof(CompFacility), nameof(CompFacility.PostDrawExtraSelectionOverlays))]
public static class CompFacilities_PostDrawExtraSelectionOverlays_Patch
{
    /// <summary>
    /// A <see cref="HarmonyTranspiler" />.
    /// </summary>
    /// <param name="instructions">A collection of <see cref="CodeInstruction" />s representing the IL code of the original method.</param>
    /// <returns>A collection of <see cref="CodeInstruction" />s representing the IL code of the new method.</returns>
    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
    {
        return BetterLinkableColorsPatches.Patch_PostDrawExtraSelectionOverlays_For_Vtc3_Vtc3(instructions, generator);
    }
}
