using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;
using Verse;
using RimWorld;
using HarmonyLib;
// Ignore Spelling: Gauranlen, Plantable
namespace Drummeur.BetterLinkableColors;
/// <summary>
/// A collection of methods to use for creating the better linkable colors patches.
/// </summary>
[StaticConstructorOnStartup]
public static class LinkableColorsPatches
{
#region Method References

#pragma warning disable IDE1006 // Naming Styles
    /// <summary>
    /// A reference to the <see cref="GenDraw.DrawLineBetween(Vector3,Vector3)" /> method.
    /// </summary>
    internal static readonly MethodInfo DrawLineBetween2adic = AccessTools.Method(typeof(GenDraw), nameof(GenDraw.DrawLineBetween), [typeof(Vector3), typeof(Vector3)]);

#if RIMWORLD_13_OR_GREATER
    /// <summary>
    /// A reference to the <see cref="GenDraw.DrawLineBetween(Vector3,Vector3,Material,float)" /> method.
    /// </summary>
    /// <remarks>
    /// 1.3 changed the signature of GenDraw.DrawLineBetween to add an optional float argument for line thickness
    /// </remarks>
    internal static readonly MethodInfo DrawLineBetween3adic = AccessTools.Method(typeof(GenDraw), nameof(GenDraw.DrawLineBetween), [typeof(Vector3), typeof(Vector3), typeof(Material), typeof(float)]);
#else

    /// <summary>
    /// A reference to the <see cref="GenDraw.DrawLineBetween(Vector3,Vector3,Material)" /> method.
    /// </summary>
    internal static readonly MethodInfo DrawLineBetween3adic = AccessTools.Method(typeof(GenDraw), nameof(GenDraw.DrawLineBetween), [typeof(Vector3), typeof(Vector3), typeof(Material)]);
#endif

    /// <summary>
    /// A reference to the <see cref="ActiveLineMat" /> property getter.
    /// </summary>
    internal static readonly FieldInfo ActiveLineField = AccessTools.Field(typeof(Materials), nameof(Materials.ActiveLineMat));

    /// <summary>
    /// A reference to the <see cref="InactiveLineMat" /> property getter.
    /// </summary>
    internal static readonly FieldInfo InactiveLineField = AccessTools.Field(typeof(Materials), nameof(Materials.InactiveLineMat));

    /// <summary>
    /// A reference to the <see cref="PotentialLineMat" /> property getter.
    /// </summary>
    internal static readonly FieldInfo PotentialLineField = AccessTools.Field(typeof(Materials), nameof(Materials.PotentialLineMat));

    /// <summary>
    /// A reference to the <see cref="SupplantedLineMat" /> property getter.
    /// </summary>
    internal static readonly FieldInfo SupplantedLineField = AccessTools.Field(typeof(Materials), nameof(Materials.OverriddenLineMat));

    /// <summary>
    /// A reference to the <see cref="CompAffectedByFacilities.InactiveFacilityLineMat" /> field.
    /// </summary>
    internal static readonly FieldInfo InactiveFacilityLineMatField = AccessTools.Field(typeof(CompAffectedByFacilities), nameof(CompAffectedByFacilities.InactiveFacilityLineMat));

    /// <summary>
    /// A reference to the <see cref="Settings.LineThickness" /> field.
    /// </summary>
    internal static readonly FieldInfo LineThicknessField = AccessTools.Field(typeof(Settings), nameof(Settings.LineThickness));
#pragma warning restore IDE1006 // Naming Styles

#endregion Method References

    /// <summary>
    /// A <see langword="bool" /> that determines if finding methods or fields _failed, so that the patching aborts safely.
    /// </summary>
    internal static readonly bool failed;

    /// <summary>
    /// A static constructor to set up HarmonyLib for patching.
    /// </summary>
    static LinkableColorsPatches()
    {
        if (DrawLineBetween2adic is null)
        {
            failed = true;
            throw new MissingMethodException($"Unable to find method with name {nameof(GenDraw)}.{nameof(GenDraw.DrawLineBetween)}({nameof(UnityEngine)}.{nameof(Vector3)}, {nameof(UnityEngine)}.{nameof(Vector3)})");
        }

        if (DrawLineBetween3adic is null)
        {
            failed = true;
#if RIMWORLD_13_OR_GREATER
            throw new MissingMethodException($"Unable to find method with name {nameof(GenDraw)}.{nameof(GenDraw.DrawLineBetween)}({nameof(UnityEngine)}.{nameof(Vector3)}, {nameof(UnityEngine)}.{nameof(Vector3)}, {nameof(UnityEngine)}.{nameof(Material)}, float)");
#else
            throw new MissingMethodException($"Unable to find method with name {nameof(GenDraw)}.{nameof(GenDraw.DrawLineBetween)}({nameof(UnityEngine)}.{nameof(Vector3)}, {nameof(UnityEngine)}.{nameof(Vector3)}, {nameof(UnityEngine)}.{nameof(Material)})");
#endif
        }

        if (ActiveLineField is null)
        {
            failed = true;
            throw new MissingFieldException($"Unable to find field with name {nameof(LinkableColorsPatches)}.{nameof(Materials.ActiveLineMat)}()");
        }

        if (InactiveLineField is null)
        {
            failed = true;
            throw new MissingFieldException($"Unable to find field with name {nameof(LinkableColorsPatches)}.{nameof(Materials.InactiveLineMat)}()");
        }

        if (PotentialLineField is null)
        {
            failed = true;
            throw new MissingFieldException($"Unable to find field with name {nameof(LinkableColorsPatches)}.{nameof(Materials.PotentialLineMat)}()");
        }

        if (SupplantedLineField is null)
        {
            failed = true;
            throw new MissingFieldException($"Unable to find field with name {nameof(LinkableColorsPatches)}.{nameof(Materials.OverriddenLineMat)}()");
        }

        if (InactiveFacilityLineMatField is null)
        {
            failed = true;
            throw new MissingFieldException($"Unable to find field with name {nameof(CompAffectedByFacilities)}.{nameof(CompAffectedByFacilities.InactiveFacilityLineMat)}");
        }

        if (LineThicknessField is null)
        {
            failed = true;
            throw new MissingFieldException($"Unable to find field with name {nameof(Settings)}.{nameof(Settings.LineThickness)}");
        }

        if (!LinkableColorsPatches.failed)
        {
            var harmony = new Harmony($"{nameof(Drummeur)}.{nameof(BetterLinkableColors)}");
            harmony.PatchAllUncategorized();
            if (ModsConfig.IsActive("Mlie.ThisIsMine"))
            {
                harmony.PatchCategory("ThisIsMine");
            }
        }
    }

    /// <summary>
    /// A Wrapper for <see cref="HarmonyTranspiler" /> for patching.
    /// This us used to draw extra section overlays.
    /// </summary>
    /// <param name="instructions">A collection of <see cref="CodeInstruction" />s representing the IL code of the original method.</param>
    /// <returns>A collection of <see cref="CodeInstruction" />s representing the IL code of the new method.</returns>
    internal static IEnumerable<CodeInstruction> PatchPostDrawExtraSectionOverlays(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
    {
        // Original code:
        //foreach (CodeInstruction op in instructions)
        //{
        //    // intercept the call to the 2adic DrawnLineBetween methods
        //    // load the appropriate _color and then call the 3adic method
        //    if (op.Calls(DrawLineBetween2adic))
        //    {
        //        //yield return new CodeInstruction(OpCodes.Ldsfld, GreenLine);
        //        yield return new CodeInstruction(OpCodes.Callvirt, ActiveLinePropertyGetter);
        //
        //        if (VersionControl.CurrentMinor >= 3)
        //        {
        //            yield return new CodeInstruction(OpCodes.Ldsfld, LinkableColorsPatches.LineThicknessField);
        //        }
        //
        //        yield return new CodeInstruction(OpCodes.Call, DrawLineBetween3adic);
        //    }
        //    // the only Ldsfld is the one we want to intercept, so we don't need to check the operand
        //    // load our own field instead of the original one
        //    else if (op.Is(OpCodes.Ldsfld, null))
        //    {
        //        //yield return new CodeInstruction(OpCodes.Ldsfld, RedLine);
        //        yield return new CodeInstruction(OpCodes.Callvirt, LinkableColorsPatches.InactiveLinePropertyGetter);
        //    }
        //    // make sure we use the correct line thickness
        //    else if (op.Is(OpCodes.Ldc_R4, null))
        //    {
        //        yield return new CodeInstruction(OpCodes.Ldc_R4, LinkableColorsPatches.LineThicknessField);
        //    }
        //    // otherwise, we're good to go!
        //    else
        //    {
        //        yield return op;
        //    }
        //}

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

        _ = matcher.InsertAndAdvance(new CodeInstruction(OpCodes.Call, LinkableColorsPatches.DrawLineBetween3adic));

        if (VersionControl.CurrentMinor >= 3)
        {
            _ = matcher
                .MatchStartForward([
                    new CodeMatch(OpCodes.Ldsfld, LinkableColorsPatches.InactiveFacilityLineMatField),
                    new CodeMatch(OpCodes.Ldc_R4, 0.2f),
                    new CodeMatch(OpCodes.Call, LinkableColorsPatches.DrawLineBetween3adic),
                ]).ThrowIfInvalid("Failed to match for segment 2")
                .RemoveInstructions(3);
        }
        else
        {
            _ = matcher
                .MatchStartForward([
                    new CodeMatch(OpCodes.Ldsfld, LinkableColorsPatches.InactiveFacilityLineMatField),
                    new CodeMatch(OpCodes.Call, LinkableColorsPatches.DrawLineBetween2adic),
                ]).ThrowIfInvalid("Failed to match for segment 2")
                .RemoveInstructions(2);
        }

        _ = matcher.InsertAndAdvance(new CodeInstruction(OpCodes.Ldsfld, LinkableColorsPatches.ActiveLineField));

        if (VersionControl.CurrentMinor >= 3)
        {
            _ = matcher.InsertAndAdvance(new CodeInstruction(OpCodes.Ldsfld, LinkableColorsPatches.LineThicknessField));
        }

        return matcher.InsertAndAdvance(new CodeInstruction(OpCodes.Call, LinkableColorsPatches.DrawLineBetween3adic))
            .Instructions();
    }

    /// <summary>
    /// A Wrapper for <see cref="HarmonyTranspiler" /> for patching.
    /// This us used to draw potential links to other things from the given placed object.
    /// </summary>
    /// <param name="instructions">A collection of <see cref="CodeInstruction" />s representing the IL code of the original method.</param>
    /// <returns>A collection of <see cref="CodeInstruction" />s representing the IL code of the new method.</returns>
    internal static IEnumerable<CodeInstruction> PatchDrawLinesToPotentialThingsToLinkTo(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
    {
        // Original code:
        //// everything here is fine except that we want to intercept `call void Verse.GenDraw::DrawLineBetween(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3, valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)`
        //// and load a _color first, and then use the 3-arg call instead of the 2-arg call
        //foreach (CodeInstruction op in instructions)
        //{
        //    // intercept
        //    if (op.Calls(DrawLineBetween2adic))
        //    {
        //        // load the potential link Material
        //        //yield return new CodeInstruction(OpCodes.Ldsfld, BlueLine);
        //        yield return new CodeInstruction(OpCodes.Callvirt, LinkableColorsPatches.PotentialLinePropertyGetter);
        //
        //        // call DrawLineBetween(UnityEngine.Vector3, UnityEngine.Vector3, UnityEngine.Material)
        //        if (VersionControl.CurrentMinor >= 3)
        //        {
        //            yield return new CodeInstruction(OpCodes.Ldsfld, LinkableColorsPatches.LineThicknessField);
        //        }
        //
        //        yield return new CodeInstruction(OpCodes.Call, LinkableColorsPatches.DrawLineBetween3adic);
        //    }
        //    // fine to go
        //    else
        //    {
        //        yield return op;
        //    }
        //}

        // If finding the methods/fields _failed just return the original instructions.
        if (LinkableColorsPatches.failed)
        {
            return instructions;
        }

        CodeMatcher matcher = new CodeMatcher(instructions, generator)
            .MatchStartForward([
                new CodeMatch(OpCodes.Call, LinkableColorsPatches.DrawLineBetween2adic),
            ]).ThrowIfInvalid("Failed to match for segment 1")
            .RemoveInstructions(1)
            .InsertAndAdvance(new CodeInstruction(OpCodes.Ldsfld, LinkableColorsPatches.PotentialLineField));

        if (VersionControl.CurrentMinor >= 3)
        {
            _ = matcher.InsertAndAdvance(new CodeInstruction(OpCodes.Ldsfld, LinkableColorsPatches.LineThicknessField));
        }

        return matcher
            .InsertAndAdvance(new CodeInstruction(OpCodes.Call, LinkableColorsPatches.DrawLineBetween3adic))
            .Instructions();
    }

    /// <summary>
    /// A Wrapper for <see cref="HarmonyTranspiler" /> for patching.
    /// This us used to draw potential links to other things from the given placed object.
    /// </summary>
    /// <param name="instructions">A collection of <see cref="CodeInstruction" />s representing the IL code of the original method.</param>
    /// <returns>A collection of <see cref="CodeInstruction" />s representing the IL code of the new method.</returns>
    internal static IEnumerable<CodeInstruction> PatchDrawConnections(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
    {
        if (LinkableColorsPatches.failed)
        {
            return instructions;
        }

        CodeMatcher matcher = new CodeMatcher(instructions, generator)
            .MatchStartForward([
                new CodeMatch(OpCodes.Call, LinkableColorsPatches.DrawLineBetween3adic),
            ]).ThrowIfInvalid("Failed to match for segment 1")
            .RemoveInstructions(1)
            .InsertAndAdvance(new CodeInstruction(OpCodes.Ldsfld, LinkableColorsPatches.PotentialLineField));

        if (VersionControl.CurrentMinor >= 3)
        {
            _ = matcher.InsertAndAdvance(new CodeInstruction(OpCodes.Ldsfld, LinkableColorsPatches.LineThicknessField));
        }

        return matcher
            .InsertAndAdvance(new CodeInstruction(OpCodes.Call, LinkableColorsPatches.DrawLineBetween3adic))
            .Instructions();
    }
}

/// <summary>
/// A <see cref="HarmonyPatch" /> to patch the <see cref="CompShipLandingBeacon.PostDrawExtraSelectionOverlays()" /> method.
/// </summary>
[HarmonyPatch(typeof(CompShipLandingBeacon), nameof(CompShipLandingBeacon.PostDrawExtraSelectionOverlays))]
public static class CompShipLandingBeacon_PostDrawExtraSelectionOverlays
{
    /// <summary>
    /// A <see cref="HarmonyTranspiler" />.
    /// </summary>
    /// <param name="instructions">A collection of <see cref="CodeInstruction" />s representing the IL code of the original method.</param>
    /// <returns>A collection of <see cref="CodeInstruction" />s representing the IL code of the new method.</returns>
    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
    {
        return LinkableColorsPatches.PatchDrawConnections(instructions, generator);
    }
}

#if RIMWORLD_13_OR_GREATER
/// <summary>
/// A <see cref="HarmonyPatch" /> to patch the <see cref="GauranlenUtility.DrawConnectionsAffectedByBuildingOverlay(Map, ThingDef, Faction, IntVec3, Rot4)" /> method.
/// </summary>
[HarmonyPatch(typeof(FocusStrengthOffset_BuildingDefs), nameof(GauranlenUtility.DrawConnectionsAffectedByBuildingOverlay))]
public static class GauranlenUtility_DrawConnectionsAffectedByBuildingOverlay
{
    /// <summary>
    /// A <see cref="HarmonyTranspiler" />.
    /// </summary>
    /// <param name="instructions">A collection of <see cref="CodeInstruction" />s representing the IL code of the original method.</param>
    /// <returns>A collection of <see cref="CodeInstruction" />s representing the IL code of the new method.</returns>
    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
    {
        return LinkableColorsPatches.PatchDrawConnections(instructions, generator);
    }
}

/// <summary>
/// A <see cref="HarmonyPatch" /> to patch the <see cref="PlaceWorker_ShowSpeakerConnections.DrawSurroundingsInfo(IntVec3)" /> method.
/// </summary>
[HarmonyPatch(typeof(CompPlantable), "DrawSurroundingsInfo")]
public static class CompPlantable_DrawSurroundingsInfo_Patch
{
    /// <summary>
    /// A <see cref="HarmonyTranspiler" />.
    /// </summary>
    /// <param name="instructions">A collection of <see cref="CodeInstruction" />s representing the IL code of the original method.</param>
    /// <returns>A collection of <see cref="CodeInstruction" />s representing the IL code of the new method.</returns>
    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
    {
        return LinkableColorsPatches.PatchDrawConnections(instructions, generator);
    }
}
#endif

#if RIMWORLD_15_OR_GREATER
/// <summary>
/// A <see cref="HarmonyPatch" /> to patch the <see cref="CompPsychicRitualSpot.PostDrawExtraSelectionOverlays()" /> method.
/// </summary>
[HarmonyPatch(typeof(CompPsychicRitualSpot), nameof(CompPsychicRitualSpot.PostDrawExtraSelectionOverlays))]
public static class CompPsychicRitualSpot_PostDrawExtraSelectionOverlays
{
    /// <summary>
    /// A <see cref="HarmonyTranspiler" />.
    /// </summary>
    /// <param name="instructions">A collection of <see cref="CodeInstruction" />s representing the IL code of the original method.</param>
    /// <returns>A collection of <see cref="CodeInstruction" />s representing the IL code of the new method.</returns>
    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
    {
        return LinkableColorsPatches.PatchDrawConnections(instructions, generator);
    }
}

/// <summary>
/// A <see cref="HarmonyPatch" /> to patch the <see cref="PlaceWorker_ShowSpeakerConnections.DrawConnections()" /> method.
/// </summary>
[HarmonyPatch(typeof(PlaceWorker_ShowSpeakerConnections), nameof(PlaceWorker_ShowSpeakerConnections.DrawConnections))]
public static class PlaceWorker_ShowSpeakerConnections_DrawConnections_Patch
{
    /// <summary>
    /// A <see cref="HarmonyTranspiler" />.
    /// </summary>
    /// <param name="instructions">A collection of <see cref="CodeInstruction" />s representing the IL code of the original method.</param>
    /// <returns>A collection of <see cref="CodeInstruction" />s representing the IL code of the new method.</returns>
    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
    {
        return LinkableColorsPatches.PatchDrawConnections(instructions, generator);
    }
}
#endif

/// <summary>
/// A <see cref="HarmonyPatch" /> to patch the <see cref="CompAffectedByFacilities.PostDrawExtraSelectionOverlays()" /> method.
/// </summary>
[HarmonyPatch(typeof(CompAffectedByFacilities), nameof(CompAffectedByFacilities.PostDrawExtraSelectionOverlays))]
public static class CompAffectedByFacilities_PostDrawExtraSelectionOverlays_Patch
{
    /// <summary>
    /// A <see cref="HarmonyTranspiler" />.
    /// </summary>
    /// <param name="instructions">A collection of <see cref="CodeInstruction" />s representing the IL code of the original method.</param>
    /// <returns>A collection of <see cref="CodeInstruction" />s representing the IL code of the new method.</returns>
    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
    {
        return LinkableColorsPatches.PatchPostDrawExtraSectionOverlays(instructions, generator);
    }
}

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
        return LinkableColorsPatches.PatchPostDrawExtraSectionOverlays(instructions, generator);
    }
}

/// <summary>
/// A <see cref="HarmonyPatch" /> to patch the <see cref="CompAffectedByFacilities.DrawLinesToPotentialThingsToLinkTo(ThingDef,IntVec3,Rot4,Map)" /> method.
/// </summary>
[HarmonyPatch(typeof(CompAffectedByFacilities), nameof(CompAffectedByFacilities.DrawLinesToPotentialThingsToLinkTo))]
public static class CompAffectedByFacilities_DrawLinesToPotentialThingsToLinkTo_Patch
{
    /// <summary>
    /// A <see cref="HarmonyTranspiler" />.
    /// </summary>
    /// <param name="instructions">A collection of <see cref="CodeInstruction" />s representing the IL code of the original method.</param>
    /// <returns>A collection of <see cref="CodeInstruction" />s representing the IL code of the new method.</returns>
    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
    {
        return LinkableColorsPatches.PatchDrawLinesToPotentialThingsToLinkTo(instructions, generator);
    }
}

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
        return LinkableColorsPatches.PatchDrawLinesToPotentialThingsToLinkTo(instructions, generator);
    }
}

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
        //    if (!found && op.Is(OpCodes.Ldsfld, LinkableColorsPatches.InactiveFacilityLineMatField))
        //    {
        //        //yield return new CodeInstruction(OpCodes.Ldsfld, LinkableColorsPatches.YellowLine);
        //        yield return new CodeInstruction(OpCodes.Callvirt, LinkableColorsPatches.SupplantedLinePropertyGetter);
        //        found = true;
        //    }
        //    else if (found && op.Is(OpCodes.Ldc_R4, 0.2f))
        //    {
        //        yield return new CodeInstruction(OpCodes.Ldsfld, LinkableColorsPatches.LineThicknessField);
        //        found = false;
        //    }
        //    else
        //    {
        //        yield return op;
        //    }
        //}

        // If finding the methods/fields _failed just return the original instructions.
        if (LinkableColorsPatches.failed)
        {
            return instructions;
        }

        var matcher = new CodeMatcher(instructions, generator);

        if (VersionControl.CurrentMinor >= 3)
        {
            _ = matcher.MatchStartForward([
                    new CodeMatch(OpCodes.Ldsfld, LinkableColorsPatches.InactiveFacilityLineMatField),
                    new CodeMatch(OpCodes.Ldc_R4, 0.2f),
                    new CodeMatch(OpCodes.Call, LinkableColorsPatches.DrawLineBetween3adic),
                ])
                .ThrowIfInvalid("Failed to match for segment 1")
                .RemoveInstructions(3);
        }
        else
        {
            _ = matcher.MatchStartForward([
                    new CodeMatch(OpCodes.Ldsfld, LinkableColorsPatches.InactiveFacilityLineMatField),
                    new CodeMatch(OpCodes.Call, LinkableColorsPatches.DrawLineBetween3adic),
                ])
                .ThrowIfInvalid("Failed to match for segment 1")
                .RemoveInstructions(2);
        }

        _ = matcher.InsertAndAdvance(new CodeInstruction(OpCodes.Ldsfld, LinkableColorsPatches.SupplantedLineField));

        if (VersionControl.CurrentMinor >= 3)
        {
            _ = matcher.InsertAndAdvance(new CodeInstruction(OpCodes.Ldsfld, LinkableColorsPatches.LineThicknessField));
        }

        return matcher.InsertAndAdvance(new CodeInstruction(OpCodes.Call, LinkableColorsPatches.DrawLineBetween3adic))
                .Instructions();
    }
}
