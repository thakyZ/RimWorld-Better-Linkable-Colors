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
public static class BetterLinkableColorsPatches
{
#region Method References

#pragma warning disable IDE1006 // Naming Styles
    /// <summary>
    /// A reference to the <see cref="GenDraw.DrawLineBetween(Vector3,Vector3)" /> method.
    /// </summary>
    internal static readonly MethodInfo DrawLineBetween_Vtc3_Vtc3 = AccessTools.Method(typeof(GenDraw), nameof(GenDraw.DrawLineBetween), [typeof(Vector3), typeof(Vector3)]);

#if RIMWORLD_13_OR_GREATER
    /// <summary>
    /// A reference to the <see cref="GenDraw.DrawLineBetween(Vector3,Vector3,Material,float)" /> method.
    /// </summary>
    /// <remarks>
    /// 1.3 changed the signature of GenDraw.DrawLineBetween to add an optional float argument for line thickness
    /// </remarks>
    internal static readonly MethodInfo DrawLineBetween_Vtc3_Vtc3_Mat_float = AccessTools.Method(typeof(GenDraw), nameof(GenDraw.DrawLineBetween), [typeof(Vector3), typeof(Vector3), typeof(Material), typeof(float)]);

    /// <summary>
    /// A reference to the <see cref="GenDraw.DrawLineBetween(Vector3,Vector3,SimpleColor,float)" /> method.
    /// </summary>
    internal static readonly MethodInfo DrawLineBetween_Vtc3_Vtc3_SC_float = AccessTools.Method(typeof(GenDraw), nameof(GenDraw.DrawLineBetween), [typeof(Vector3), typeof(Vector3), typeof(SimpleColor), typeof(float)]);
#else

    /// <summary>
    /// A reference to the <see cref="GenDraw.DrawLineBetween(Vector3,Vector3,Material)" /> method.
    /// </summary>
    internal static readonly MethodInfo DrawLineBetween_Vtc3_Vtc3_Mat_float = AccessTools.Method(typeof(GenDraw), nameof(GenDraw.DrawLineBetween), [typeof(Vector3), typeof(Vector3), typeof(Material)]);
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
    static BetterLinkableColorsPatches()
    {
        if (DrawLineBetween_Vtc3_Vtc3 is null)
        {
            failed = true;
            throw new MissingMethodException($"Unable to find method with name {nameof(GenDraw)}.{nameof(GenDraw.DrawLineBetween)}({typeof(Vector3).FullName}, {typeof(Vector3).FullName})");
        }

        if (DrawLineBetween_Vtc3_Vtc3_Mat_float is null)
        {
            failed = true;
#if RIMWORLD_13_OR_GREATER
            throw new MissingMethodException($"Unable to find method with name {nameof(GenDraw)}.{nameof(GenDraw.DrawLineBetween)}({typeof(Vector3).FullName}, {typeof(Vector3).FullName}, {typeof(Material).FullName}, {typeof(float).FullName})");
#else
            throw new MissingMethodException($"Unable to find method with name {nameof(GenDraw)}.{nameof(GenDraw.DrawLineBetween)}({typeof(Vector3).FullName}, {typeof(Vector3).FullName}, {nameof(UnityEngine)}.{typeof(Material).FullName})");
#endif
        }

#if RIMWORLD_13_OR_GREATER
        if (DrawLineBetween_Vtc3_Vtc3_SC_float is null)
        {
            failed = true;
            throw new MissingMethodException($"Unable to find method with name {nameof(GenDraw)}.{nameof(GenDraw.DrawLineBetween)}({typeof(Vector3).FullName}, {typeof(Vector3).FullName}, {typeof(SimpleColor).FullName}, {typeof(float).FullName})");
        }
#endif

        if (ActiveLineField is null)
        {
            failed = true;
            throw new MissingFieldException($"Unable to find field with name {nameof(BetterLinkableColorsPatches)}.{nameof(Materials.ActiveLineMat)}");
        }

        if (InactiveLineField is null)
        {
            failed = true;
            throw new MissingFieldException($"Unable to find field with name {nameof(BetterLinkableColorsPatches)}.{nameof(Materials.InactiveLineMat)}");
        }

        if (PotentialLineField is null)
        {
            failed = true;
            throw new MissingFieldException($"Unable to find field with name {nameof(BetterLinkableColorsPatches)}.{nameof(Materials.PotentialLineMat)}");
        }

        if (SupplantedLineField is null)
        {
            failed = true;
            throw new MissingFieldException($"Unable to find field with name {nameof(BetterLinkableColorsPatches)}.{nameof(Materials.OverriddenLineMat)}");
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

        if (!BetterLinkableColorsPatches.failed)
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
    internal static IEnumerable<CodeInstruction> Patch_PostDrawExtraSelectionOverlays_For_Vtc3_Vtc3(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
    {
        // Original code:
        //foreach (CodeInstruction op in instructions)
        //{
        //    // intercept the call to the 2adic DrawnLineBetween methods
        //    // load the appropriate _color and then call the 3adic method
        //    if (op.Calls(DrawLineBetween_Vtc3_Vtc3))
        //    {
        //        //yield return new CodeInstruction(OpCodes.Ldsfld, GreenLine);
        //        yield return new CodeInstruction(OpCodes.Callvirt, ActiveLinePropertyGetter);
        //
        //        if (VersionControl.CurrentMinor >= 3)
        //        {
        //            yield return new CodeInstruction(OpCodes.Ldsfld, BetterLinkableColorsPatches.LineThicknessField);
        //        }
        //
        //        yield return new CodeInstruction(OpCodes.Call, DrawLineBetween_Vtc3_Vtc3_Mat_float);
        //    }
        //    // the only Ldsfld is the one we want to intercept, so we don't need to check the operand
        //    // load our own field instead of the original one
        //    else if (op.Is(OpCodes.Ldsfld, null))
        //    {
        //        //yield return new CodeInstruction(OpCodes.Ldsfld, RedLine);
        //        yield return new CodeInstruction(OpCodes.Callvirt, BetterLinkableColorsPatches.InactiveLinePropertyGetter);
        //    }
        //    // make sure we use the correct line thickness
        //    else if (op.Is(OpCodes.Ldc_R4, null))
        //    {
        //        yield return new CodeInstruction(OpCodes.Ldc_R4, BetterLinkableColorsPatches.LineThicknessField);
        //    }
        //    // otherwise, we're good to go!
        //    else
        //    {
        //        yield return op;
        //    }
        //}

        if (BetterLinkableColorsPatches.failed)
        {
            return instructions;
        }

        CodeMatcher matcher = new CodeMatcher(instructions, generator)
            .MatchStartForward([new CodeMatch(OpCodes.Call, BetterLinkableColorsPatches.DrawLineBetween_Vtc3_Vtc3)])
            .ThrowIfInvalid("Failed to match for segment 1")
            .RemoveInstructions(1)
            .InsertAndAdvance(new CodeInstruction(OpCodes.Ldsfld, BetterLinkableColorsPatches.ActiveLineField));

        if (VersionControl.CurrentMinor >= 3)
        {
            _ = matcher.InsertAndAdvance(new CodeInstruction(OpCodes.Ldsfld, BetterLinkableColorsPatches.LineThicknessField));
        }

        _ = matcher.InsertAndAdvance(new CodeInstruction(OpCodes.Call, BetterLinkableColorsPatches.DrawLineBetween_Vtc3_Vtc3_Mat_float));

        if (VersionControl.CurrentMinor >= 3)
        {
            _ = matcher
                .MatchStartForward([
                    new CodeMatch(OpCodes.Ldsfld, BetterLinkableColorsPatches.InactiveFacilityLineMatField),
                    new CodeMatch(OpCodes.Ldc_R4, 0.2f),
                    new CodeMatch(OpCodes.Call, BetterLinkableColorsPatches.DrawLineBetween_Vtc3_Vtc3_Mat_float),
                ]).ThrowIfInvalid("Failed to match for segment 2")
                .RemoveInstructions(3);
        }
        else
        {
            _ = matcher
                .MatchStartForward([
                    new CodeMatch(OpCodes.Ldsfld, BetterLinkableColorsPatches.InactiveFacilityLineMatField),
                    new CodeMatch(OpCodes.Call, BetterLinkableColorsPatches.DrawLineBetween_Vtc3_Vtc3),
                ]).ThrowIfInvalid("Failed to match for segment 2")
                .RemoveInstructions(2);
        }

        _ = matcher.InsertAndAdvance(new CodeInstruction(OpCodes.Ldsfld, BetterLinkableColorsPatches.ActiveLineField));

        if (VersionControl.CurrentMinor >= 3)
        {
            _ = matcher.InsertAndAdvance(new CodeInstruction(OpCodes.Ldsfld, BetterLinkableColorsPatches.LineThicknessField));
        }

        return matcher.InsertAndAdvance(new CodeInstruction(OpCodes.Call, BetterLinkableColorsPatches.DrawLineBetween_Vtc3_Vtc3_Mat_float))
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
        //    if (op.Calls(DrawLineBetween_Vtc3_Vtc3))
        //    {
        //        // load the potential link Material
        //        //yield return new CodeInstruction(OpCodes.Ldsfld, BlueLine);
        //        yield return new CodeInstruction(OpCodes.Callvirt, BetterLinkableColorsPatches.PotentialLinePropertyGetter);
        //
        //        // call DrawLineBetween(UnityEngine.Vector3, UnityEngine.Vector3, UnityEngine.Material)
        //        if (VersionControl.CurrentMinor >= 3)
        //        {
        //            yield return new CodeInstruction(OpCodes.Ldsfld, BetterLinkableColorsPatches.LineThicknessField);
        //        }
        //
        //        yield return new CodeInstruction(OpCodes.Call, BetterLinkableColorsPatches.DrawLineBetween_Vtc3_Vtc3_Mat_float);
        //    }
        //    // fine to go
        //    else
        //    {
        //        yield return op;
        //    }
        //}

        // If finding the methods/fields _failed just return the original instructions.
        if (BetterLinkableColorsPatches.failed)
        {
            return instructions;
        }

        CodeMatcher matcher = new CodeMatcher(instructions, generator)
            .MatchStartForward([
                new CodeMatch(OpCodes.Call, BetterLinkableColorsPatches.DrawLineBetween_Vtc3_Vtc3),
            ]).ThrowIfInvalid("Failed to match for segment 1")
            .RemoveInstructions(1)
            .InsertAndAdvance(new CodeInstruction(OpCodes.Ldsfld, BetterLinkableColorsPatches.PotentialLineField));

        if (VersionControl.CurrentMinor >= 3)
        {
            _ = matcher.InsertAndAdvance(new CodeInstruction(OpCodes.Ldsfld, BetterLinkableColorsPatches.LineThicknessField));
        }

        return matcher
            .InsertAndAdvance(new CodeInstruction(OpCodes.Call, BetterLinkableColorsPatches.DrawLineBetween_Vtc3_Vtc3_Mat_float))
            .Instructions();
    }

#if RIMWORLD_13_OR_GREATER
    /// <summary>
    /// A Wrapper for <see cref="HarmonyTranspiler" /> for patching.
    /// This us used to draw potential links to other things from the given placed object.
    /// </summary>
    /// <param name="instructions">A collection of <see cref="CodeInstruction" />s representing the IL code of the original method.</param>
    /// <returns>A collection of <see cref="CodeInstruction" />s representing the IL code of the new method.</returns>
    internal static IEnumerable<CodeInstruction> Patch_PostDrawExtraSelectionOverlays_For_Vtc3_Vtc3_Mat_float(IEnumerable<CodeInstruction> instructions, ILGenerator generator, FieldInfo matField)
    {
        if (BetterLinkableColorsPatches.failed)
        {
            return instructions;
        }

        return new CodeMatcher(instructions, generator)
            .MatchStartForward([
                new CodeMatch(OpCodes.Call, BetterLinkableColorsPatches.DrawLineBetween_Vtc3_Vtc3_Mat_float),
            ]).ThrowIfInvalid("Failed to match for segment 1")
            .Advance(-2)
            .RemoveInstructions(3)
            .InsertAndAdvance([
                new CodeInstruction(OpCodes.Ldsfld, matField),
                new CodeInstruction(OpCodes.Ldsfld, BetterLinkableColorsPatches.LineThicknessField),
                new CodeInstruction(OpCodes.Call, BetterLinkableColorsPatches.DrawLineBetween_Vtc3_Vtc3_Mat_float),
            ])
            .Instructions();
    }

    /// <summary>
    /// A Wrapper for <see cref="HarmonyTranspiler" /> for patching.
    /// This us used to draw potential links to other things from the given placed object.
    /// </summary>
    /// <param name="instructions">A collection of <see cref="CodeInstruction" />s representing the IL code of the original method.</param>
    /// <returns>A collection of <see cref="CodeInstruction" />s representing the IL code of the new method.</returns>
    internal static IEnumerable<CodeInstruction> Patch_PostDrawExtraSelectionOverlays_For_Vtc3_Vtc3_SC_float(IEnumerable<CodeInstruction> instructions, ILGenerator generator, FieldInfo matField)
    {
        if (BetterLinkableColorsPatches.failed)
        {
            return instructions;
        }

        return new CodeMatcher(instructions, generator)
            .MatchStartForward([new CodeMatch(OpCodes.Call, BetterLinkableColorsPatches.DrawLineBetween_Vtc3_Vtc3_SC_float)])
            .ThrowIfInvalid("Failed to match for segment 1")
            .Advance(-2)
            .RemoveInstructions(3)
            .InsertAndAdvance([
                new CodeInstruction(OpCodes.Ldsfld, matField),
                new CodeInstruction(OpCodes.Ldsfld, BetterLinkableColorsPatches.LineThicknessField),
                new CodeInstruction(OpCodes.Call, BetterLinkableColorsPatches.DrawLineBetween_Vtc3_Vtc3_Mat_float),
            ])
            .Instructions();
    }
#endif
}
