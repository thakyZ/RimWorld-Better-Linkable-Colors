using System.Collections.Generic;
using System.Reflection.Emit;
using RimWorld;
using HarmonyLib;
namespace Drummeur.BetterLinkableColors;

/// <summary>
/// A <see cref="HarmonyPatch" /> to patch the <see cref="ThisIsMine.CompCanBelongToRoomOwners.PostDrawExtraSelectionOverlays()" /> method.
/// </summary>
[HarmonyPatchCategory("ThisIsMine")]
[HarmonyPatch(typeof(ThisIsMine.CompCanBelongToRoomOwners), nameof(ThisIsMine.CompCanBelongToRoomOwners.PostDrawExtraSelectionOverlays))]
public static class CompCanBelongToRoomOwners_PostDrawExtraSelectionOverlays_Patch
{
    /// <summary>
    /// A <see cref="HarmonyTranspiler" />.
    /// </summary>
    /// <param name="instructions">A collection of <see cref="CodeInstruction" />s representing the IL code of the original method.</param>
    /// <returns>A collection of <see cref="CodeInstruction" />s representing the IL code of the new method.</returns>
    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
    {
        if (LinkableColorsPatches.failed)
        {
            return instructions;
        }

        CodeMatcher matcher = new CodeMatcher(instructions, generator)
            .MatchStartForward([new CodeMatch(OpCodes.Call, LinkableColorsPatches.DrawLineBetween2adic)])
            .ThrowIfInvalid("Failed to match for segment 1")
            .RemoveInstructions(1)
            .InsertAndAdvance(new CodeInstruction(OpCodes.Ldsfld, LinkableColorsPatches.ActiveLineField));

        if (VersionControl.CurrentMinor >= 3)
        {
            _ = matcher.InsertAndAdvance(new CodeInstruction(OpCodes.Ldsfld, LinkableColorsPatches.LineThicknessField));
        }

        return matcher.InsertAndAdvance(new CodeInstruction(OpCodes.Call, LinkableColorsPatches.DrawLineBetween3adic))
            .Instructions();
    }
}
