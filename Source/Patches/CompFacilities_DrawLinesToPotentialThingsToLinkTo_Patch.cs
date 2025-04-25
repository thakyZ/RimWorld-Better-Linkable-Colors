using System.Collections.Generic;
using System.Reflection.Emit;
using Verse;
using RimWorld;
using HarmonyLib;
// Ignore Spelling: Gauranlen, Plantable
namespace Drummeur.BetterLinkableColors.Patches;

/// <summary>
/// A <see cref="HarmonyPatch" /> to patch the <see cref="CompFacility.DrawLinesToPotentialThingsToLinkTo(ThingDef,IntVec3,Rot4,Map)" /> method.
/// </summary>
[HarmonyPatch(typeof(CompFacility), nameof(CompFacility.DrawLinesToPotentialThingsToLinkTo))]
public static class CompFacilities_DrawLinesToPotentialThingsToLinkTo_Patch
{
    /// <summary>
    /// A <see cref="HarmonyTranspiler" />.
    /// </summary>
    /// <param name="instructions">A collection of <see cref="CodeInstruction" />s representing the IL code of the original method.</param>
    /// <returns>A collection of <see cref="CodeInstruction" />s representing the IL code of the new method.</returns>
    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
    {
        return BetterLinkableColorsPatches.PatchDrawLinesToPotentialThingsToLinkTo(instructions, generator);
    }
}
