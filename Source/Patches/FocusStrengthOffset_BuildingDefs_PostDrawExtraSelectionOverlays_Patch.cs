using System.Collections.Generic;
using System.Reflection.Emit;
using Verse;
using RimWorld;
using HarmonyLib;
// Ignore Spelling: Gauranlen, Plantable, Defs
#if RIMWORLD_13_OR_GREATER
namespace Drummeur.BetterLinkableColors.Patches;

/// <summary>
/// A <see cref="HarmonyPatch" /> to patch the <see cref="FocusStrengthOffset_BuildingDefs.DrawConnectionsAffectedByBuildingOverlay(ThingDef, Pawn)" /> method.
/// </summary>
[HarmonyPatch(typeof(FocusStrengthOffset_BuildingDefs), nameof(FocusStrengthOffset_BuildingDefs.PostDrawExtraSelectionOverlays))]
public static class FocusStrengthOffset_BuildingDefs_PostDrawExtraSelectionOverlays_Patch
{
    /// <summary>
    /// A <see cref="HarmonyTranspiler" />.
    /// </summary>
    /// <param name="instructions">A collection of <see cref="CodeInstruction" />s representing the IL code of the original method.</param>
    /// <returns>A collection of <see cref="CodeInstruction" />s representing the IL code of the new method.</returns>
    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
    {
        return BetterLinkableColorsPatches.Patch_PostDrawExtraSelectionOverlays_For_Vtc3_Vtc3_SC_float(instructions, generator, BetterLinkableColorsPatches.PotentialLineField);
    }
}
#endif
