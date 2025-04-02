using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

using drummeur.linkablecolors.Util;

using static drummeur.linkablecolors.Settings.LinkableColorsSettings;

namespace drummeur.linkablecolors
{
    // todo: color picker for all these colors

    [StaticConstructorOnStartup]
    class LinkableColorsPatches
    {
        internal static MethodInfo DrawLineBetween2adic = AccessTools.Method(typeof(GenDraw), nameof(GenDraw.DrawLineBetween), new Type[] { typeof(Vector3), typeof(Vector3) });

        // 1.3 changed the signature of GenDraw.DrawLineBetween to add an optional float argument for line thickness
        internal static MethodInfo DrawLineBetween3adic = VersionControl.CurrentMinor >= 3 ?
            AccessTools.Method(typeof(GenDraw), nameof(GenDraw.DrawLineBetween), new Type[] { typeof(Vector3), typeof(Vector3), typeof(Material), typeof(float) }) :
            AccessTools.Method(typeof(GenDraw), nameof(GenDraw.DrawLineBetween), new Type[] { typeof(Vector3), typeof(Vector3), typeof(Material) });


        internal static Shader shader = UseSolidLineShader ? ShaderDatabase.SolidColor : ShaderDatabase.Transparent;

        //internal static Material ActiveLineMat = MaterialPool.MatFrom(GenDraw.LineTexPath, shader, Colors[ActiveColorLabel]);
        //internal static Material InactiveLineMat = MaterialPool.MatFrom(GenDraw.LineTexPath, shader, Colors[InactiveColorLabel]);
        //internal static Material PotentialLineMat = MaterialPool.MatFrom(GenDraw.LineTexPath, shader, Colors[PotentialColorLabel]);
        //internal static Material SupplantedLineMat = MaterialPool.MatFrom(GenDraw.LineTexPath, shader, Colors[SupplantedColorLabel]);

        internal static Material ActiveLineMat = MaterialPool.MatFrom(GenDraw.LineTexPath, shader, ColorHelper.ColorFromRgbString(ActiveColorString) ?? ColorHelper.Defaults.ActiveColor);
        internal static Material InactiveLineMat = MaterialPool.MatFrom(GenDraw.LineTexPath, shader, ColorHelper.ColorFromRgbString(InactiveColorString) ?? ColorHelper.Defaults.InactiveColor);
        internal static Material PotentialLineMat = MaterialPool.MatFrom(GenDraw.LineTexPath, shader, ColorHelper.ColorFromRgbString(PotentialColorString) ?? ColorHelper.Defaults.PotentialColor);
        internal static Material SupplantedLineMat = MaterialPool.MatFrom(GenDraw.LineTexPath, shader, ColorHelper.ColorFromRgbString(SupplantedColorString) ?? ColorHelper.Defaults.SupplantedColor);


        internal static FieldInfo ActiveLine = AccessTools.Field(typeof(LinkableColorsPatches), nameof(ActiveLineMat));
        internal static FieldInfo InactiveLine = AccessTools.Field(typeof(LinkableColorsPatches), nameof(InactiveLineMat));
        internal static FieldInfo PotentialLine = AccessTools.Field(typeof(LinkableColorsPatches), nameof(PotentialLineMat));
        internal static FieldInfo SupplantedLine = AccessTools.Field(typeof(LinkableColorsPatches), nameof(SupplantedLineMat));

        static LinkableColorsPatches()
        {
            var harmony = new Harmony("drummeur.linkablecolors");
            harmony.PatchAll();
        }

        internal static IEnumerable<CodeInstruction> PatchPostDrawExtraSectionOverlays(IEnumerable<CodeInstruction> source)
        {         
            foreach (var op in source)
            {
                // intercept the call to the 2adic DrawnLineBetween methods
                // load the appropriate color and then call the 3adic method
                if (op.opcode == OpCodes.Call && op.operand as MethodInfo == DrawLineBetween2adic)
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
                else if (op.opcode == OpCodes.Ldsfld)
                {
                    //yield return new CodeInstruction(OpCodes.Ldsfld, RedLine);
                    yield return new CodeInstruction(OpCodes.Ldsfld, InactiveLine);
                }
                // make sure we use the correct line thickness
                else if (op.opcode == OpCodes.Ldc_R4)
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

        internal static IEnumerable<CodeInstruction> PatchDrawLinesToPotentialThingsToLinkTo(IEnumerable<CodeInstruction> source)
        {
            // everything here is fine except that we want to intercept `call void Verse.GenDraw::DrawLineBetween(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3, valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)`
            // and load a color first, and then use the 3-arg call instead of the 2-arg call
            foreach (var op in source)
            {
                // intercept
                if (op.opcode == OpCodes.Call && op.operand as MethodInfo == DrawLineBetween2adic)
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

    [HarmonyPatch(typeof(CompAffectedByFacilities), nameof(CompAffectedByFacilities.PostDrawExtraSelectionOverlays))]
    public static class CompAffectedByFacilities_PostDrawExtraSelectionOverlays_Patch
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> source)
        {
            return LinkableColorsPatches.PatchPostDrawExtraSectionOverlays(source);
        }
    }

    [HarmonyPatch(typeof(CompFacility), nameof(CompFacility.PostDrawExtraSelectionOverlays))]
    public static class CompFacilities_PostDrawExtraSelectionOverlays_Patch
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> source)
        {
            return LinkableColorsPatches.PatchPostDrawExtraSectionOverlays(source);
        }
    }

    [HarmonyPatch(typeof(CompAffectedByFacilities), nameof(CompAffectedByFacilities.DrawLinesToPotentialThingsToLinkTo))]
    public static class CompAffectedByFacilities_DrawLinesToPotentialThingsToLinkTo_Patch
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> source)
        {
            return LinkableColorsPatches.PatchDrawLinesToPotentialThingsToLinkTo(source);
        }
    }

    [HarmonyPatch(typeof(CompFacility), nameof(CompFacility.DrawLinesToPotentialThingsToLinkTo))]
    public static class CompFacilities_DrawLinesToPotentialThingsToLinkTo_Patch
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> source)
        {
            return LinkableColorsPatches.PatchDrawLinesToPotentialThingsToLinkTo(source);
        }
    }

    // yeah, well, it's going to be a yellow line now...
    [StaticConstructorOnStartup]
    [HarmonyPatch(typeof(CompAffectedByFacilities), nameof(CompAffectedByFacilities.DrawRedLineToPotentiallySupplantedFacility))]
    public static class CompAffectedByFacilities_DrawRedLineToPotentiallSupplantedFacility_Patch
    {
        internal static Shader shader = UseSolidLineShader ? ShaderDatabase.SolidColor : ShaderDatabase.Transparent;

        internal static Material SupplantedLineMat = MaterialPool.MatFrom(GenDraw.LineTexPath, shader, ColorHelper.ColorFromRgbString(SupplantedColorString) ?? ColorHelper.Defaults.SupplantedColor);

        internal static FieldInfo SupplantedLine = AccessTools.Field(typeof(LinkableColorsPatches), nameof(SupplantedLineMat));

        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> source)
        {

            foreach (var op in source)
            {
                if (op.opcode == OpCodes.Ldsfld)
                {
                    //yield return new CodeInstruction(OpCodes.Ldsfld, LinkableColorsPatches.YellowLine);
                    yield return new CodeInstruction(OpCodes.Ldsfld, SupplantedLine);
                }
                else if (op.opcode == OpCodes.Ldc_R4)
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
}



