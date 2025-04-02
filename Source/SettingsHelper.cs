using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace drummeur.linkablecolors;

//thanks to AlexTD for the below
internal static class SettingsHelper
{
    //private static float Gap = 12f;

    //public static void SliderLabeled(this Listing_Standard listing_Standard, string label, ref int value, Func<int, string> formatter, int min = 0, int max = 100, string? tooltip = null)
    //{
    //    float fVal = value;
    //    listing_Standard.SliderLabeled(label, ref fVal, (float _val) => formatter((int)_val)), (float)min, (float)max);
    //    value = (int)fVal;
    //}

    public static void SliderLabeled(this Listing_Standard listing_Standard, string label, ref float val, Func<float, string> formatter, float min = 0f, float max = 1f, string? tooltip = null)
    {
        Rect rect = listing_Standard.GetRect(Text.LineHeight);
        Rect rect2 = rect.LeftPart(.70f).Rounded();
        Rect rect3 = rect.RightPart(.30f).Rounded().LeftPart(.67f).Rounded();
        Rect rect4 = rect.RightPart(.10f).Rounded();

        TextAnchor anchor = Text.Anchor;
        Text.Anchor = TextAnchor.MiddleLeft;
        Widgets.Label(rect2, label);

#if RIMWORLD_14
        Widgets.HorizontalSlider(rect3, ref val, new FloatRange(min, max), label);
#else
        val = Widgets.HorizontalSlider(rect3, val, min, max, true);
        Text.Anchor = TextAnchor.MiddleRight;
        Widgets.Label(rect4, formatter(val));
#endif
        if (!tooltip.NullOrEmpty())
        {
            TooltipHandler.TipRegion(rect, tooltip);
        }

        Text.Anchor = anchor;
        listing_Standard.Gap(listing_Standard.verticalSpacing);
    }

    public static void SliderLabeledSettable(this Listing_Standard listing_Standard, string label, ref float val, ref string buffer, /* Func<float, string> formatter, */ float min = 0f, float max = 1f, string? tooltip = null)
    {
        Rect rect  = listing_Standard.GetRect(Text.LineHeight);
        Rect rect2 = rect.LeftPart(.70f).Rounded();
        Rect rect3 = rect.RightPart(.30f).Rounded().LeftPart(.67f).Rounded();
        /* Rect rect4 */ _ = rect.RightPart(.10f).Rounded();

        TextAnchor anchor = Text.Anchor;
        Text.Anchor = TextAnchor.MiddleLeft;
        Widgets.Label(rect2, label);

#if RIMWORLD_14
        Widgets.HorizontalSlider(rect3, ref val, new FloatRange(min, max), label);
#else
        val = Widgets.HorizontalSlider(rect3, val, min, max, true);
        //Widgets.Label(rect4, formatter(value));
#endif

        Text.Anchor = TextAnchor.MiddleRight;
        listing_Standard.TextFieldNumeric(ref val, ref buffer, min, max);

        if (!tooltip.NullOrEmpty())
        {
            TooltipHandler.TipRegion(rect, tooltip);
        }

        Text.Anchor = anchor;
        listing_Standard.Gap(listing_Standard.verticalSpacing);
    }

    public static void FloatRange(this Listing_Standard listing_Standard, string label, ref FloatRange range, float min = 0f, float max = 1f, string? tooltip = null, ToStringStyle valueStyle = ToStringStyle.FloatTwo)
    {
        Rect rect   = listing_Standard.GetRect(Text.LineHeight);
        Rect rect2  = rect.LeftPart(.70f).Rounded();
        Rect rect3  = rect.RightPart(.30f).Rounded().LeftPart(.9f).Rounded();
        rect3.yMin -= 5f;
        //_           = rect.RightPart(.10f).Rounded();

        TextAnchor anchor = Text.Anchor;
        Text.Anchor = TextAnchor.MiddleLeft;
        Widgets.Label(rect2, label);

        Text.Anchor = TextAnchor.MiddleRight;
        var id = listing_Standard.CurHeight.GetHashCode();
        Widgets.FloatRange(rect3, id, ref range, min, max, null, valueStyle);
        if (!tooltip.NullOrEmpty())
        {
            TooltipHandler.TipRegion(rect, tooltip);
        }

        Text.Anchor = anchor;
        listing_Standard.Gap(listing_Standard.verticalSpacing);
    }

    public static void AddLabeledTextField(this Listing_Standard listing_Standard, string label, ref string settingsValue, string? tooltip = null, float leftPartPct = 0.5f)
    {
        //listing_Standard.Gap(Gap);
        _ = listing_Standard.LineRectSplitter(out Rect leftHalf, out Rect rightHalf, leftPartPct);

        if (!tooltip.NullOrEmpty())
        {
            Widgets.DrawHighlightIfMouseover(leftHalf);
            TooltipHandler.TipRegion(leftHalf, tooltip);
        }

        Widgets.Label(leftHalf, label);

        var buffer = $"{settingsValue}";
        settingsValue = Widgets.TextField(rightHalf, buffer);
    }

    /// <summary>
    /// Tests if the given <see cref="Type" /> is a valid numeric type.
    /// </summary>
    /// <param name="t">The <see cref="Type" /> to test.</param>
    /// <returns><see langword="true" /> when the provided <see cref="Type" /> is a numeric type; otherwise, <see langword="false" />.</returns>
    private static bool IsNumericType(Type t)
        => t.IsEquivalentTo(typeof(int))
        || t.IsEquivalentTo(typeof(uint))
        || t.IsEquivalentTo(typeof(short))
        || t.IsEquivalentTo(typeof(ushort))
        || t.IsEquivalentTo(typeof(long))
        || t.IsEquivalentTo(typeof(ulong))
        || t.IsEquivalentTo(typeof(byte))
        || t.IsEquivalentTo(typeof(sbyte))
        || t.IsEquivalentTo(typeof(int))
        || t.IsEquivalentTo(typeof(float))
        || t.IsEquivalentTo(typeof(decimal))
        || t.IsEquivalentTo(typeof(double))
        || t.IsEquivalentTo(typeof(nint))
        || t.IsEquivalentTo(typeof(nuint));

    public static void AddLabeledNumericalTextField<T>(this Listing_Standard listing_Standard, string label, ref T settingsValue, string? tooltip = null, float leftPartPct = 0.5f, float min = 1f, float max = 100000f) where T : struct
    {
        if (!IsNumericType(typeof(T))) return;

        //listing_Standard.Gap(Gap);
        _ = listing_Standard.LineRectSplitter(out Rect leftHalf, out Rect rightHalf, leftPartPct);

        if (!tooltip.NullOrEmpty())
        {
            Widgets.DrawHighlightIfMouseover(leftHalf);
            TooltipHandler.TipRegion(leftHalf, tooltip);
        }

        Widgets.Label(leftHalf, label);

        var buffer = $"{settingsValue}";
        Widgets.TextFieldNumeric(rightHalf, ref settingsValue, ref buffer, min, max);
    }

    public static Rect LineRectSplitter(this Listing_Standard listing_Standard, out Rect leftHalf, float leftPartPct = 0.5f, float? height = null)
    {
        Rect lineRect = listing_Standard.GetRect(height);
        leftHalf = lineRect.LeftPart(leftPartPct).Rounded();
        return lineRect;
    }

    public static Rect LineRectSplitter(this Listing_Standard listing_Standard, out Rect leftHalf, out Rect rightHalf, float leftPartPct = 0.5f, float? height = null)
    {
        Rect lineRect = listing_Standard.LineRectSplitter(out leftHalf, leftPartPct, height);
        rightHalf = lineRect.RightPart(1f - leftPartPct).Rounded();
        return lineRect;
    }

    public static Rect GetRect(this Listing_Standard listing_Standard, float? height = null)
    {
        return listing_Standard.GetRect(height ?? Text.LineHeight);
    }

#region Labeled Radio Buttons

    public class LabeledRadioValue<TLabel, TValue>
    {
        public TLabel Label { get; set; }
        public TValue Value { get; set; }

        public LabeledRadioValue(TLabel label, TValue value)
        {
            this.Label = label;
            this.Value = value;
        }
    }

    public class LabeledRadioValue<T>: LabeledRadioValue<string, T>
    {
        public LabeledRadioValue(string label, T value) : base(label, value) { }
    }

    //thanks to Why_is_that for the below, who in turn got his stuff from
    // REFERENCE: https://github.com/erdelf/GodsOfRimworld/blob/master/Source/Ankh/ModControl.cs
    // REFERENCE: https://github.com/erdelf/PrisonerRansom/
    // modified by drummeur to make it use generics and Enumerables
    public static void AddLabeledRadioList<T>(this Listing_Standard listing_Standard, string? header, IEnumerable<T> labels, ref T val, float? headerHeight = null)
    {
        if (!string.IsNullOrEmpty(header))
        {
            Widgets.Label(listing_Standard.GetRect(headerHeight), header);
        }

        listing_Standard.AddRadioList(GenerateLabeledRadioValues(labels), ref val);
    }

    public static void AddLabeledRadioList<T>(this Listing_Standard listing_Standard, string? header, IEnumerable<T> labels, ref T val, Func<T, string> formatter, float? headerHeight = null)
    {
        if (!string.IsNullOrEmpty(header))
        {
            Widgets.Label(listing_Standard.GetRect(headerHeight), header);
        }

        listing_Standard.AddRadioList(GenerateLabeledRadioValues(labels, formatter), ref val);
    }

    private static IEnumerable<LabeledRadioValue<T>> GenerateLabeledRadioValues<T>(IEnumerable<T> labels)
    {
        foreach (T? label in labels)
        {
            yield return new LabeledRadioValue<T>(label?.ToString() ?? "NULL", label);
        }
    }

    private static IEnumerable<LabeledRadioValue<T>> GenerateLabeledRadioValues<T>(IEnumerable<T> labels, Func<T, string> formatter)
    {
        foreach (T? label in labels)
        {
            yield return new LabeledRadioValue<T>(formatter(label) ?? "NULL", label);
        }
    }

    private static void AddRadioList<T>(this Listing_Standard listing_Standard, IEnumerable<LabeledRadioValue<T>> items, ref T val, float? height = null)
    {
        foreach (LabeledRadioValue<T> item in items)
        {
            if (Widgets.RadioButtonLabeled(listing_Standard.GetRect(height), item.Label, EqualityComparer<T>.Default.Equals(item.Value, val)))
            {
                val = item.Value;
            }
        }
    }

    public static void AddLabeledRadioList<TLabel, TValue>(this Listing_Standard listing_standard, string header, IEnumerable<KeyValuePair<TLabel, TValue>> keyValuePairs, ref TValue val, float? headerHeight = null)
    {
        if (!string.IsNullOrEmpty(header))
        {
            Widgets.Label(listing_standard.GetRect(headerHeight), header);
        }

        listing_standard.AddRadioList(GenerateLabeledRadioValues(keyValuePairs), ref val);
    }

    private static IEnumerable<LabeledRadioValue<TLabel, TValue>> GenerateLabeledRadioValues<TLabel, TValue>(IEnumerable<KeyValuePair<TLabel, TValue>> kvps)
    {
        foreach (KeyValuePair<TLabel, TValue> kvp in kvps)
        {
            yield return new LabeledRadioValue<TLabel, TValue>(kvp.Key, kvp.Value);
        }
    }

    private static void AddRadioList<TLabel, TValue>(this Listing_Standard listing_Standard, IEnumerable<LabeledRadioValue<TLabel, TValue>> items, ref TValue val, float? height = null)
    {
        foreach (LabeledRadioValue<TLabel, TValue> item in items)
        {
            if (Widgets.RadioButtonLabeled(listing_Standard.GetRect(height), item.Label?.ToString() ?? "NULL", EqualityComparer<TValue>.Default.Equals(item.Value, val)))
            {
                val = item.Value;
            }
        }
    }

#endregion Labeled Radio Buttons
}