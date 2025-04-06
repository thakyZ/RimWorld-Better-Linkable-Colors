using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;
using RimWorld;
using HarmonyLib;
namespace Drummeur.BetterLinkableColors;
/// <summary>
/// A collection of methods to use for creating the better linkable colors patches.
/// </summary>
[StaticConstructorOnStartup]
public class LinkableColorsPatches
{
#region Method References

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
    /// Gets which shader to use from the <see cref="ShaderDatabase" /> which is determined by the <see cref="LinkableColorsSettings.UseSolidLineShader" /> setting.
    /// </summary>
    internal static Shader Shader => UseSolidLineShader ? ShaderDatabase.SolidColor : ShaderDatabase.Transparent;

    ///// <summary>
    ///// Gets the value for the active line using the reference to the material from the <see cref="MaterialPool" /> using the value from the <see cref="LinkableColorsSettings.ActiveColorLabel" /> setting.
    ///// </summary>
    //internal static Material ActiveLineMat => MaterialPool.MatFrom(GenDraw.LineTexPath, Shader, Colors[ActiveColorLabel]);

    ///// <summary>
    ///// Gets the value for the inactive line using the reference to the material from the <see cref="MaterialPool" /> using the value from the <see cref="LinkableColorsSettings.InactiveColorLabel" /> setting.
    ///// </summary>
    //internal static Material InactiveLineMat => MaterialPool.MatFrom(GenDraw.LineTexPath, Shader, Colors[InactiveColorLabel]);

    ///// <summary>
    ///// Gets the value for the potential line using the reference to the material from the <see cref="MaterialPool" /> using the value from the <see cref="LinkableColorsSettings.PotentialColorLabel" /> setting.
    ///// </summary>
    //internal static Material PotentialLineMat => MaterialPool.MatFrom(GenDraw.LineTexPath, Shader, Colors[PotentialColorLabel]);

    ///// <summary>
    ///// Gets the value for the supplanted line using the reference to the material from the <see cref="MaterialPool" /> using the value from the <see cref="LinkableColorsSettings.SupplantedColorLabel" /> setting.
    ///// </summary>
    //internal static Material SupplantedLineMat => MaterialPool.MatFrom(GenDraw.LineTexPath, Shader, Colors[SupplantedColorLabel]);

    /// <summary>
    /// Gets the value for the active line using the reference to the material from the <see cref="MaterialPool" /> using the value from the <see cref="LinkableColorsSettings.ActiveColorLabel" /> setting.
    /// </summary>
    internal static Material ActiveLineMat => MaterialPool.MatFrom(GenDraw.LineTexPath, Shader, ColorHelper.ColorFromRgbString(ActiveColorString) ?? ColorHelper.Defaults.ActiveColor);

    /// <summary>
    /// Gets the value for the inactive line using the reference to the material from the <see cref="MaterialPool" /> using the value from the <see cref="LinkableColorsSettings.InactiveColorLabel" /> setting.
    /// </summary>
    internal static Material InactiveLineMat => MaterialPool.MatFrom(GenDraw.LineTexPath, Shader, ColorHelper.ColorFromRgbString(InactiveColorString) ?? ColorHelper.Defaults.InactiveColor);

    /// <summary>
    /// Gets the value for the potential line using the reference to the material from the <see cref="MaterialPool" /> using the value from the <see cref="LinkableColorsSettings.PotentialColorLabel" /> setting.
    /// </summary>
    internal static Material PotentialLineMat => MaterialPool.MatFrom(GenDraw.LineTexPath, Shader, ColorHelper.ColorFromRgbString(PotentialColorString) ?? ColorHelper.Defaults.PotentialColor);

    /// <summary>
    /// Gets the value for the supplanted line using the reference to the material from the <see cref="MaterialPool" /> using the value from the <see cref="LinkableColorsSettings.SupplantedColorLabel" /> setting.
    /// </summary>
    internal static Material SupplantedLineMat => MaterialPool.MatFrom(GenDraw.LineTexPath, Shader, ColorHelper.ColorFromRgbString(SupplantedColorString) ?? ColorHelper.Defaults.SupplantedColor);

    /// <summary>
    /// A reference to the <see cref="ActiveLineMat" /> property.
    /// </summary>
    internal static readonly PropertyInfo ActiveLine = AccessTools.Property(typeof(LinkableColorsPatches), nameof(ActiveLineMat));

    /// <summary>
    /// A reference to the <see cref="InactiveLineMat" /> property.
    /// </summary>
    internal static readonly PropertyInfo InactiveLine = AccessTools.Property(typeof(LinkableColorsPatches), nameof(InactiveLineMat));

    /// <summary>
    /// A reference to the <see cref="PotentialLineMat" /> property.
    /// </summary>
    internal static readonly PropertyInfo PotentialLine = AccessTools.Property(typeof(LinkableColorsPatches), nameof(PotentialLineMat));

    /// <summary>
    /// A reference to the <see cref="SupplantedLineMat" /> property.
    /// </summary>
    internal static readonly PropertyInfo SupplantedLine = AccessTools.Property(typeof(LinkableColorsPatches), nameof(SupplantedLineMat));

#endregion Method References

    /// <summary>
    /// A static constructor to set up HarmonyLib for patching.
    /// </summary>
    static LinkableColorsPatches()
    {
        var harmony = new Harmony($"{nameof(drummeur)}.{nameof(linkablecolors)}");
        harmony.PatchAll();
    }

    /// <summary>
    /// A Wrapper for <see cref="HarmonyTranspiler" /> for patching.
    /// This us used to draw extra section overlays.
    /// </summary>
    /// <param name="instructions">A collection of <see cref="CodeInstruction" />s representing the IL code of the original method.</param>
    /// <returns>A collection of <see cref="CodeInstruction" />s representing the IL code of the new method.</returns>
    internal static IEnumerable<CodeInstruction> PatchPostDrawExtraSectionOverlays(IEnumerable<CodeInstruction> instructions)
    {
        foreach (CodeInstruction op in instructions)
        {
            // intercept the call to the 2adic DrawnLineBetween methods
            // load the appropriate color and then call the 3adic method
            if (op.Calls(DrawLineBetween2adic))
            {
                //yield return new CodeInstruction(OpCodes.Ldsfld, GreenLine);
                yield return new CodeInstruction(OpCodes.Ldsfld, ActiveLine);

                if (VersionControl.CurrentMinor >= 3)
                {
                    yield return new CodeInstruction(OpCodes.Ldc_R4, LineThickness);
                }

                yield return new CodeInstruction(OpCodes.Call, DrawLineBetween3adic);
            }
            // the only Ldsfld is the one we want to intercept, so we don't need to check the operand
            // load our own field instead of the original one
            else if (op.Is(OpCodes.Ldsfld, null))
            {
                //yield return new CodeInstruction(OpCodes.Ldsfld, RedLine);
                yield return new CodeInstruction(OpCodes.Ldsfld, InactiveLine);
            }
            // make sure we use the correct line thickness
            else if (op.Is(OpCodes.Ldc_R4, null))
            {
                yield return new CodeInstruction(OpCodes.Ldc_R4, LineThickness);
            }
            // otherwise, we're good to go!
            else
            {
                yield return op;
            }
        }
    }

    /// <summary>
    /// A Wrapper for <see cref="HarmonyTranspiler" /> for patching.
    /// This us used to draw potential links to other things from the given placed object.
    /// </summary>
    /// <param name="instructions">A collection of <see cref="CodeInstruction" />s representing the IL code of the original method.</param>
    /// <returns>A collection of <see cref="CodeInstruction" />s representing the IL code of the new method.</returns>
    internal static IEnumerable<CodeInstruction> PatchDrawLinesToPotentialThingsToLinkTo(IEnumerable<CodeInstruction> instructions)
    {
        // everything here is fine except that we want to intercept `call void Verse.GenDraw::DrawLineBetween(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3, valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)`
        // and load a color first, and then use the 3-arg call instead of the 2-arg call
        foreach (CodeInstruction op in instructions)
        {
            // intercept
            if (op.Calls(DrawLineBetween2adic))
            {
                // load the potential link Material
                //yield return new CodeInstruction(OpCodes.Ldsfld, BlueLine);
                yield return new CodeInstruction(OpCodes.Ldsfld, PotentialLine);

                // call DrawLineBetween(UnityEngine.Vector3, UnityEngine.Vector3, UnityEngine.Material)
                if (VersionControl.CurrentMinor >= 3)
                {
                    yield return new CodeInstruction(OpCodes.Ldc_R4, LineThickness);
                }

                yield return new CodeInstruction(OpCodes.Call, DrawLineBetween3adic);
            }
            // fine to go
            else
            {
                yield return op;
            }
        }
    }
}

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
    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        return LinkableColorsPatches.PatchPostDrawExtraSectionOverlays(instructions);
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
    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        return LinkableColorsPatches.PatchPostDrawExtraSectionOverlays(instructions);
    }
}

/// <summary>
/// A <see cref="HarmonyPatch" /> to patch the <see cref="CompAffectedByFacilities.DrawLinesToPotentialThingsToLinkTo()" /> method.
/// </summary>
[HarmonyPatch(typeof(CompAffectedByFacilities), nameof(CompAffectedByFacilities.DrawLinesToPotentialThingsToLinkTo))]
public static class CompAffectedByFacilities_DrawLinesToPotentialThingsToLinkTo_Patch
{
    /// <summary>
    /// A <see cref="HarmonyTranspiler" />.
    /// </summary>
    /// <param name="instructions">A collection of <see cref="CodeInstruction" />s representing the IL code of the original method.</param>
    /// <returns>A collection of <see cref="CodeInstruction" />s representing the IL code of the new method.</returns>
    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        return LinkableColorsPatches.PatchDrawLinesToPotentialThingsToLinkTo(instructions);
    }
}

/// <summary>
/// A <see cref="HarmonyPatch" /> to patch the <see cref="CompFacility.DrawLinesToPotentialThingsToLinkTo()" /> method.
/// </summary>
[HarmonyPatch(typeof(CompFacility), nameof(CompFacility.DrawLinesToPotentialThingsToLinkTo))]
public static class CompFacilities_DrawLinesToPotentialThingsToLinkTo_Patch
{
    /// <summary>
    /// A <see cref="HarmonyTranspiler" />.
    /// </summary>
    /// <param name="instructions">A collection of <see cref="CodeInstruction" />s representing the IL code of the original method.</param>
    /// <returns>A collection of <see cref="CodeInstruction" />s representing the IL code of the new method.</returns>
    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        return LinkableColorsPatches.PatchDrawLinesToPotentialThingsToLinkTo(instructions);
    }
}

/// <summary>
/// A <see cref="HarmonyPatch" /> to patch the <see cref="CompAffectedByFacilities.DrawRedLineToPotentiallySupplantedFacility()" /> method.
/// </summary>
/// <remarks>Yeah, well, it's going to be a yellow line now...</remarks>
[StaticConstructorOnStartup]
[HarmonyPatch(typeof(CompAffectedByFacilities), nameof(CompAffectedByFacilities.DrawRedLineToPotentiallySupplantedFacility))]
public static class CompAffectedByFacilities_DrawRedLineToPotentiallySupplantedFacility_Patch
{
    /// <summary>
    /// A <see cref="HarmonyTranspiler" />.
    /// </summary>
    /// <param name="instructions">A collection of <see cref="CodeInstruction" />s representing the IL code of the original method.</param>
    /// <returns>A collection of <see cref="CodeInstruction" />s representing the IL code of the new method.</returns>
    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        foreach (CodeInstruction op in instructions)
        {
            if (op.Is(OpCodes.Ldsfld, null))
            {
                //yield return new CodeInstruction(OpCodes.Ldsfld, LinkableColorsPatches.YellowLine);
                yield return new CodeInstruction(OpCodes.Ldsfld, LinkableColorsPatches.SupplantedLine);
            }
            else if (op.Is(OpCodes.Ldc_R4, null))
            {
                yield return new CodeInstruction(OpCodes.Ldc_R4, LineThickness);
            }
            else
            {
                yield return op;
            }
        }
    }
}
