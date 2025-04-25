using System.Collections.Generic;
using System.Reflection.Emit;
using Verse;
using RimWorld;
using HarmonyLib;
// Ignore Spelling: Gauranlen, Plantable
namespace Drummeur.BetterLinkableColors.Patches;

/// <summary>
/// A <see cref="HarmonyPatch" /> to patch the <see cref="CompAffectedByFacilities.DrawRedLineToPotentiallySupplantedFacility()" /> method.
/// </summary>
/// <remarks>Yeah, well, it's going to be a _yellow line now...</remarks>
[StaticConstructorOnStartup]
[HarmonyPatch(typeof(CompAffectedByFacilities), nameof(CompAffectedByFacilities.DrawRedLineToPotentiallySupplantedFacility))]
public static class CompAffectedByFacilities_DrawRedLineToPotentiallySupplantedFacility_Patch
{
    /// <summary>
    /// A <see cref="HarmonyTranspiler" />.
    /// </summary>
    /// <param name="instructions">A collection of <see cref="CodeInstruction" />s representing the IL code of the original method.</param>
    /// <returns>A collection of <see cref="CodeInstruction" />s representing the IL code of the new method.</returns>
    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
    {
        // Original code:
        //var found = false;
        // everything here is fine except that we want to intercept
        // `call void Verse.GenDraw::DrawLineBetween(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3, valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)`
        // and load a _color first, and then use the 3-arg call instead of the 2-arg call
        //foreach (CodeInstruction op in instructions)
        //{
        //    if (!found && op.Is(OpCodes.Ldsfld, BetterLinkableColorsPatches.InactiveFacilityLineMatField))
        //    {
        //        //yield return new CodeInstruction(OpCodes.Ldsfld, BetterLinkableColorsPatches.YellowLine);
        //        yield return new CodeInstruction(OpCodes.Callvirt, BetterLinkableColorsPatches.SupplantedLinePropertyGetter);
        //        found = true;
        //    }
        //    else if (found && op.Is(OpCodes.Ldc_R4, 0.2f))
        //    {
        //        yield return new CodeInstruction(OpCodes.Ldsfld, BetterLinkableColorsPatches.LineThicknessField);
        //        found = false;
        //    }
        //    else
        //    {
        //        yield return op;
        //    }
        //}

        // If finding the methods/fields _failed just return the original instructions.
        if (BetterLinkableColorsPatches.failed)
            return instructions;

        var matcher = new CodeMatcher(instructions, generator);

        if (VersionControl.CurrentMinor >= 3)
        {
            _ = matcher.MatchStartForward([
                    new CodeMatch(OpCodes.Ldsfld, BetterLinkableColorsPatches.InactiveFacilityLineMatField),
                    new CodeMatch(OpCodes.Ldc_R4, 0.2f),
                    new CodeMatch(OpCodes.Call, BetterLinkableColorsPatches.DrawLineBetween_Vtc3_Vtc3_Mat_float),
                ])
                .ThrowIfInvalid("Failed to match for segment 1")
                .RemoveInstructions(3);
        }
        else
        {
            _ = matcher.MatchStartForward([
                    new CodeMatch(OpCodes.Ldsfld, BetterLinkableColorsPatches.InactiveFacilityLineMatField),
                    new CodeMatch(OpCodes.Call, BetterLinkableColorsPatches.DrawLineBetween_Vtc3_Vtc3_Mat_float),
                ])
                .ThrowIfInvalid("Failed to match for segment 1")
                .RemoveInstructions(2);
        }

        _ = matcher.InsertAndAdvance(new CodeInstruction(OpCodes.Ldsfld, BetterLinkableColorsPatches.SupplantedLineField));

        if (VersionControl.CurrentMinor >= 3)
            _ = matcher.InsertAndAdvance(new CodeInstruction(OpCodes.Ldsfld, BetterLinkableColorsPatches.LineThicknessField));

        return matcher.InsertAndAdvance(new CodeInstruction(OpCodes.Call, BetterLinkableColorsPatches.DrawLineBetween_Vtc3_Vtc3_Mat_float))
                .Instructions();
    }
}
