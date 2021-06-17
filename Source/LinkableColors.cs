using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace drummeur.linkablecolors
{
    // todo: color picker for all these colors

    [StaticConstructorOnStartup]
    class LinkableColors
    {
        internal static Material GreenLineMat = MaterialPool.MatFrom(GenDraw.LineTexPath, ShaderDatabase.SolidColor, Color.green);
        internal static Material RedLineMat = MaterialPool.MatFrom(GenDraw.LineTexPath, ShaderDatabase.SolidColor, Color.red);
        internal static Material BlueLineMat = MaterialPool.MatFrom(GenDraw.LineTexPath, ShaderDatabase.SolidColor, Color.blue);
        internal static Material YellowLineMat = MaterialPool.MatFrom(GenDraw.LineTexPath, ShaderDatabase.SolidColor, Color.yellow);

        internal static FieldInfo GreenLine = AccessTools.Field(typeof(LinkableColors), nameof(GreenLineMat));
        internal static FieldInfo RedLine = AccessTools.Field(typeof(LinkableColors), nameof(RedLineMat));
        internal static FieldInfo BlueLine = AccessTools.Field(typeof(LinkableColors), nameof(BlueLineMat));
        internal static FieldInfo YellowLine = AccessTools.Field(typeof(LinkableColors), nameof(YellowLineMat));

        internal static MethodInfo DrawLineBetween2adic = AccessTools.Method(typeof(GenDraw), nameof(GenDraw.DrawLineBetween), new Type[] { typeof(Vector3), typeof(Vector3) });
        internal static MethodInfo DrawLineBetween3adic = AccessTools.Method(typeof(GenDraw), nameof(GenDraw.DrawLineBetween), new Type[] { typeof(Vector3), typeof(Vector3), typeof(Material) });

        static LinkableColors()
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
                    yield return new CodeInstruction(OpCodes.Ldsfld, GreenLine);
                    yield return new CodeInstruction(OpCodes.Call, DrawLineBetween3adic);
                }
                // the only Ldsfld is the one we want to intercept, so we don't need to check the operand
                // load our own field instead of the original one
                else if (op.opcode == OpCodes.Ldsfld)
                {
                    yield return new CodeInstruction(OpCodes.Ldsfld, RedLine);
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
                    yield return new CodeInstruction(OpCodes.Ldsfld, BlueLine);

                    // call DrawLineBetween(UnityEngine.Vector3, UnityEngine.Vector3, UnityEngine.Material)
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
            return LinkableColors.PatchPostDrawExtraSectionOverlays(source);
        }
    }

    [HarmonyPatch(typeof(CompFacility), nameof(CompFacility.PostDrawExtraSelectionOverlays))]
    public static class CompFacilities_PostDrawExtraSelectionOverlays_Patch
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> source)
        {
            return LinkableColors.PatchPostDrawExtraSectionOverlays(source);
        }
    }

    [HarmonyPatch(typeof(CompAffectedByFacilities), nameof(CompAffectedByFacilities.DrawLinesToPotentialThingsToLinkTo))]
    public static class CompAffectedByFacilities_DrawLinesToPotentialThingsToLinkTo_Patch
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> source)
        {
            return LinkableColors.PatchDrawLinesToPotentialThingsToLinkTo(source);
        }
    }

    [HarmonyPatch(typeof(CompFacility), nameof(CompFacility.DrawLinesToPotentialThingsToLinkTo))]
    public static class CompFacilities_DrawLinesToPotentialThingsToLinkTo_Patch
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> source)
        {
            return LinkableColors.PatchDrawLinesToPotentialThingsToLinkTo(source);
        }
    }

    // yeah, well, it's going to be a yellow line now...
    [HarmonyPatch(typeof(CompAffectedByFacilities), nameof(CompAffectedByFacilities.DrawRedLineToPotentiallySupplantedFacility))]
    public static class CompAffectedByFacilities_DrawRedLineToPotentiallSupplantedFacility
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> source)
        {
            foreach (var op in source)
            {
                if (op.opcode == OpCodes.Ldsfld)
                {
                    yield return new CodeInstruction(OpCodes.Ldsfld, LinkableColors.YellowLine);
                }
                else
                {
                    yield return op;
                }
            }
        }
    }
}



