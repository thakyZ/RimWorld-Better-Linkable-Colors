using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;
using RimWorld;
namespace Drummeur.BetterLinkableColors;
/// <summary>
/// A class to help provide UI components for the settings window.
/// </summary>
/// <remarks>
/// <para>Thanks to AlexTD for the below.</para>
/// <para>
/// Originally from <see href="https://github.com/RimWorld-CCL-Reborn/4M-Mehni-s-Misc-Modifications/blob/44ef9ee6e64c15d1d9acb0a7e2aa3ae307c2e354/Mehni's%20Misc%20Modifications/SettingsHelper.cs"/>
/// </para>
/// </remarks>
internal static class SettingsHelper
{
    ///// <summary>
    ///// A constant <see langword="float" /> value determining the gap size that should be used.
    ///// </summary>
    //private const float Gap = 12f;

    ///// <summary>
    ///// Renders a labeled slider with a reference <paramref name="value" />.
    ///// </summary>
    ///// <param name="listing_Standard">The <see cref="Listing_Standard" /> to append the labeled slider to.</param>
    ///// <param name="label">A <see langword="string" /> that represents the label to add to the slider.</param>
    ///// <param name="value">An <see langword="int" /> reference value that represents the slider's current value.</param>
    ///// <param name="formatter">A function that formats the <see langword="int" /> value into a valid <see langword="string" />.</param>
    ///// <param name="min">An <see langword="int" /> that determines the minimum value for the slider.</param>
    ///// <param name="max">An <see langword="int" /> that determines the maximum value for the slider.</param>
    ///// <param name="tooltip">An optional <see langword="string" /> that defines the tooltip to use for the slider.</param>
    //public static void SliderLabeled(this Listing_Standard listing_Standard, string label, ref int value, Func<int, string> formatter, int min = 0, int max = 100, string? tooltip = null)
    //{
    //    float fVal = value;
    //    listing_Standard.SliderLabeled(label, ref fVal, (float _val) => formatter((int)_val)), (float)min, (float)max);
    //    value = (int)fVal;
    //}

    /// <summary>
    /// Renders a labeled slider with a reference <paramref name="val" />.
    /// </summary>
    /// <param name="listing_Standard">The <see cref="Listing_Standard" /> to append the labeled slider to.</param>
    /// <param name="label">A <see langword="string" /> that represents the label to add to the slider.</param>
    /// <param name="val">A <see langword="float" /> reference value that represents the slider's current value.</param>
    /// <param name="formatter">A function that formats the <see langword="float" /> value into a valid <see langword="string" />.</param>
    /// <param name="min">An <see langword="int" /> that determines the minimum value for the slider.</param>
    /// <param name="max">An <see langword="int" /> that determines the maximum value for the slider.</param>
    /// <param name="tooltip">An optional <see langword="string" /> that defines the tooltip to use for the slider.</param>
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

    /// <summary>
    /// Renders a labeled slider with a reference <paramref name="val" /> that can be manually set via a reference <paramref name="buffer" />.
    /// </summary>
    /// <param name="listing_Standard">The <see cref="Listing_Standard" /> to append the labeled slider to.</param>
    /// <param name="label">A <see langword="string" /> that represents the label to add to the slider.</param>
    /// <param name="val">A <see langword="float" /> reference value that represents the slider's current value.</param>
    /// <param name="buffer">A <see langword="string" /> reference value that represents the buffer to set the slider's current value.</param>
    /// <param name="min">A <see langword="float" /> that determines the minimum value for the slider.</param>
    /// <param name="max">A <see langword="float" /> that determines the maximum value for the slider.</param>
    /// <param name="tooltip">An optional <see langword="string" /> that defines the tooltip to use for the slider.</param>
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

    /// <summary>
    /// Renders a ranged slider with a reference <paramref name="range" />.
    /// </summary>
    /// <param name="listing_Standard">The <see cref="Listing_Standard" /> to append the range slider to.</param>
    /// <param name="label">A <see langword="string" /> that represents the label to add to the range slider.</param>
    /// <param name="range">A <see cref="Verse.FloatRange" /> reference that can be used to determine the value that the range slider is set to.</param>
    /// <param name="min">A <see langword="float" /> that determines the minimum value for the range slider.</param>
    /// <param name="max">A <see langword="float" /> that determines the maximum value for the range slider.</param>
    /// <param name="tooltip">An optional <see langword="string" /> that defines the tooltip to use for the range slider.</param>
    /// <param name="valueStyle">An optional <see cref="ToStringStyle" /> to use for the rendering the text of the float</param>
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

    /// <summary>
    /// Renders a labeled text field with a reference <paramref name="settingsValue" />.
    /// </summary>
    /// <param name="listing_Standard">The <see cref="Listing_Standard" /> to append the range slider to.</param>
    /// <param name="label">A <see langword="string" /> that represents the label to add to the range slider.</param>
    /// <param name="settingsValue">A <see langword="string" /> reference value that represents the buffer to set the text field's current value.</param>
    /// <param name="tooltip">An optional <see langword="string" /> that defines the tooltip to use for the range slider.</param>
    /// <param name="leftPartPct">A <see langword="float" /> determining the percentage to use from the left of the given <see cref="Listing_Standard" />'s bounds.</param>
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

    /// <summary>
    /// Renders a labeled numerical text field with a reference <paramref name="settingsValue" />.
    /// </summary>
    /// <typeparam name="T">A type param that should match a built in numeric type.</typeparam>
    /// <param name="listing_Standard">The <see cref="Listing_Standard" /> to append the labeled numeric text field to.</param>
    /// <param name="label">A <see langword="string" /> that represents the label to add to the numeric text field.</param>
    /// <param name="settingsValue">A <see langword="string" /> reference value that represents the buffer to set the numeric text field's current value.</param>
    /// <param name="tooltip">An optional <see langword="string" /> that defines the tooltip to use for the range slider.</param>
    /// <param name="leftPartPct">A <see langword="float" /> determining the percentage to use from the left of the given <see cref="Listing_Standard" />'s bounds.</param>
    /// <param name="min">A <see langword="float" /> that determines the minimum value for the slider.</param>
    /// <param name="max">A <see langword="float" /> that determines the maximum value for the slider.</param>
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

    /// <summary>
    /// Renders a split in the given <see cref="Listing_Standard" /> that splits the area vertically.
    /// </summary>
    /// <param name="listing_Standard">The <see cref="Listing_Standard" /> to split the area of.</param>
    /// <param name="leftHalf">An output <see cref="Rect" /> that determines the bounds for the left half of the split area.</param>
    /// <param name="leftPartPct">An optional <see langword="float" /> that determines at what percentage should the <paramref name="leftHalf"/> be split at.</param>
    /// <param name="height">An optional <see langword="float" /> that determines the height of the split area.</param>
    /// <returns>A <see cref="Rect" /> of the split area in the <see cref="Listing_Standard" />.</returns>
    public static Rect LineRectSplitter(this Listing_Standard listing_Standard, out Rect leftHalf, float leftPartPct = 0.5f, float? height = null)
    {
        Rect lineRect = listing_Standard.GetRect(height);
        leftHalf = lineRect.LeftPart(leftPartPct).Rounded();
        return lineRect;
    }

    /// <summary>
    /// Renders a split in the given <see cref="Listing_Standard" /> that splits the area vertically.
    /// </summary>
    /// <param name="listing_Standard">The <see cref="Listing_Standard" /> to split the area of.</param>
    /// <param name="leftHalf">An output <see cref="Rect" /> that determines the bounds for the left half of the split area.</param>
    /// <param name="rightHalf">An output <see cref="Rect" /> that determines the bounds for the right half of the split area.</param>
    /// <param name="leftPartPct">An optional <see langword="float" /> that determines at what percentage should the <paramref name="leftHalf"/> be split at.</param>
    /// <param name="height">An optional <see langword="float" /> that determines the height of the split area.</param>
    /// <returns>A <see cref="Rect" /> of the split area in the <see cref="Listing_Standard" />.</returns>
    public static Rect LineRectSplitter(this Listing_Standard listing_Standard, out Rect leftHalf, out Rect rightHalf, float leftPartPct = 0.5f, float? height = null)
    {
        Rect lineRect = listing_Standard.LineRectSplitter(out leftHalf, leftPartPct, height);
        rightHalf = lineRect.RightPart(1f - leftPartPct).Rounded();
        return lineRect;
    }

    /// <summary>
    /// Gets a <see cref="Rect" /> of the given <see cref="Listing_Standard" />.
    /// </summary>
    /// <param name="listing_Standard">The <see cref="Listing_Standard" /> to split the area of.</param>
    /// <param name="height">An optional <see langword="float" /> that determines the height of the returned <see cref="Rect" />.</param>
    /// <returns>A <see cref="Rect" /> of the <see cref="Listing_Standard" /> bounds.</returns>
    public static Rect GetRect(this Listing_Standard listing_Standard, float? height = null)
    {
        return listing_Standard.GetRect(height ?? Text.LineHeight);
    }

#region Labeled Radio Buttons

    /// <summary>
    /// A class to encapsulate the label and value of a labeled radio option.
    /// </summary>
    /// <typeparam name="TLabel">A type constraint for the label of the radio option.</typeparam>
    /// <typeparam name="TValue">A type constraint for the value of the radio option.</typeparam>
    public class LabeledRadioValue<TLabel, TValue>
    {
        /// <summary>
        /// Gets or sets the label of the radio option.
        /// </summary>
        public TLabel Label { get; set; }

        /// <summary>
        /// Gets or sets the value of the radio option.
        /// </summary>
        public TValue Value { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LabeledRadioValue{TLabel,TValue}" /> class.
        /// </summary>
        /// <param name="label">The label of the new radio option.</param>
        /// <param name="value">The value of the new radio option.</param>
        public LabeledRadioValue(TLabel label, TValue value)
        {
            this.Label = label;
            this.Value = value;
        }
    }

    /// <summary>
    /// A class to encapsulate the label and value of a labeled radio option.
    /// </summary>
    /// <typeparam name="T">A type constraint for the value of the radio option.</typeparam>
    public class LabeledRadioValue<T>: LabeledRadioValue<string, T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LabeledRadioValue{T}" /> class.
        /// </summary>
        /// <param name="label">The label of the new radio option.</param>
        /// <param name="value">The value of the new radio option.</param>
        public LabeledRadioValue(string label, T value) : base(label, value) { }
    }

    /// <summary>
    /// Renders a labeled radio list to the given <see cref="Listing_Standard" />.
    /// </summary>
    /// <typeparam name="T">A type constraint for the type that the labels use.</typeparam>
    /// <param name="listing_Standard">The <see cref="Listing_Standard" /> to append the labeled radio list to.</param>
    /// <param name="header">A <see langword="string" /> that represents the header to use for the labeled radio list.</param>
    /// <param name="labels">A collection of <typeparamref name="T" />s to use for the labeled radio list.</param>
    /// <param name="val">A <typeparamref name="T" /> reference to set the value of the labeled radio list.</param>
    /// <param name="headerHeight">An optional <see langword="float" /> to set the height of the header.</param>
    /// <remarks>
    /// Thanks to Why_is_that for the below, who in turn got his stuff from<br />
    /// REFERENCE: <see href="https://github.com/erdelf/GodsOfRimworld/blob/18ebc693ef17ab1d28b58a2c9d4d3f7d48d95517/Source/Ankh/ModControl.cs" /><br />
    /// REFERENCE: <see href="https://github.com/erdelf/PrisonerRansom/" /><br />
    /// modified by drummeur to make it use generics and <see cref="IEnumerable{T}" />s.
    /// </remarks>
    public static void AddLabeledRadioList<T>(this Listing_Standard listing_Standard, string? header, IEnumerable<T> labels, ref T val, float? headerHeight = null)
    {
        if (!string.IsNullOrEmpty(header))
        {
            Widgets.Label(listing_Standard.GetRect(headerHeight), header);
        }

        listing_Standard.AddRadioList(GenerateLabeledRadioValues(labels), ref val);
    }

    /// <summary>
    /// Renders a labeled radio list to the given <see cref="Listing_Standard" /> with a function to format the value to a string.
    /// </summary>
    /// <typeparam name="T">A type constraint for the type that the labels use.</typeparam>
    /// <param name="listing_Standard">The <see cref="Listing_Standard" /> to append the labeled radio list to.</param>
    /// <param name="header">A <see langword="string" /> that represents the header to use for the labeled radio list.</param>
    /// <param name="labels">A collection of <typeparamref name="T" />s to use for the labeled radio list.</param>
    /// <param name="val">A <typeparamref name="T" /> reference to set the value of the labeled radio list.</param>
    /// <param name="formatter">A function that formats the <typeparamref name="T" /> value into a valid <see langword="string" />.</param>
    /// <param name="headerHeight">An optional <see langword="float" /> to set the height of the header.</param>
    public static void AddLabeledRadioList<T>(this Listing_Standard listing_Standard, string? header, IEnumerable<T> labels, ref T val, Func<T, string> formatter, float? headerHeight = null)
    {
        if (!string.IsNullOrEmpty(header))
        {
            Widgets.Label(listing_Standard.GetRect(headerHeight), header);
        }

        listing_Standard.AddRadioList(GenerateLabeledRadioValues(labels, formatter), ref val);
    }

    /// <summary>
    /// A method to create new <see cref="LabeledRadioValue{T}" /> entries from a collection of <typeparamref name="T" /> labels.
    /// Using dotNET's built in <see cref="object.ToString()" /> method.
    /// </summary>
    /// <typeparam name="T">A type constraint for the type that the labels use.</typeparam>
    /// <param name="labels">A collection of <typeparamref name="T" />s to use for the labeled radio list.</param>
    /// <returns>A collection of <see cref="LabeledRadioValue{T}" /> that match the labels.</returns>
    private static IEnumerable<LabeledRadioValue<T>> GenerateLabeledRadioValues<T>(IEnumerable<T> labels)
    {
        foreach (T? label in labels)
        {
            yield return new LabeledRadioValue<T>(label?.ToString() ?? "NULL", label);
        }
    }

    /// <summary>
    /// A method to create new <see cref="LabeledRadioValue{T}" /> entries from a collection of <typeparamref name="T" /> labels.
    /// Using a function to format the labels into a <see langword="string" />.
    /// </summary>
    /// <typeparam name="T">A type constraint for the type that the labels use.</typeparam>
    /// <param name="labels">A collection of <typeparamref name="T" />s to use for the labeled radio list.</param>
    /// <param name="formatter">A function that formats the <typeparamref name="T" /> value into a valid <see langword="string" />.</param>
    /// <returns>A collection of <see cref="LabeledRadioValue{T}" /> that match the labels.</returns>
    private static IEnumerable<LabeledRadioValue<T>> GenerateLabeledRadioValues<T>(IEnumerable<T> labels, Func<T, string> formatter)
    {
        foreach (T? label in labels)
        {
            yield return new LabeledRadioValue<T>(formatter(label) ?? "NULL", label);
        }
    }

    /// <summary>
    /// Renders a radio list to the given <see cref="Listing_Standard" /> with a function to format the value to a string
    /// with a collection of <see cref="LabeledRadioValue{T}" />.
    /// </summary>
    /// <typeparam name="T">A type constraint for the type that the labels use.</typeparam>
    /// <param name="listing_Standard">The <see cref="Listing_Standard" /> to append the labeled radio list to.</param>
    /// <param name="items">A collection of <see cref="LabeledRadioValue{T}" />s to use for the labeled radio list.</param>
    /// <param name="val">A <typeparamref name="T" /> reference to set the value of the labeled radio list.</param>
    /// <param name="height">An optional <see langword="float" /> to set the height of the header.</param>
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

    /// <summary>
    /// Renders a radio list to the given <see cref="Listing_Standard" /> <see cref="KeyValuePair{TLabel,TValue}" /> list as it's values and labels.
    /// </summary>
    /// <typeparam name="TLabel">A type constraint for the type that the labels use.</typeparam>
    /// <typeparam name="TValue">A type constraint for the type that the values use.</typeparam>
    /// <param name="listing_standard">The <see cref="Listing_Standard" /> to append the labeled radio list to.</param>
    /// <param name="header">A <see langword="string" /> that represents the header to use for the labeled radio list.</param>
    /// <param name="keyValuePairs">A collection of <see cref="KeyValuePair{TLabel,TValue}" />s to use for the labeled radio list.</param>
    /// <param name="val">A <typeparamref name="T" /> reference to set the value of the labeled radio list.</param>
    /// <param name="headerHeight">An optional <see langword="float" /> to set the height of the header.</param>
    /// <remarks>
    /// Not really sure that this is worth it because we just end up converting <typeparamref name="TLabel" />s to strings at the end of the line anyway.
    /// </remarks>
    public static void AddLabeledRadioList<TLabel, TValue>(this Listing_Standard listing_standard, string header, IEnumerable<KeyValuePair<TLabel, TValue>> keyValuePairs, ref TValue val, float? headerHeight = null)
    {
        if (!string.IsNullOrEmpty(header))
        {
            Widgets.Label(listing_standard.GetRect(headerHeight), header);
        }

        listing_standard.AddRadioList(GenerateLabeledRadioValues(keyValuePairs), ref val);
    }

    /// <summary>
    /// A method to create new <see cref="LabeledRadioValue{TLabel,TValue}" /> entries from a collection of <see cref="KeyValuePair{TLabel,TValue}" /> labels.
    /// </summary>
    /// <typeparam name="TLabel">A type constraint for the type that the labels use.</typeparam>
    /// <typeparam name="TValue">A type constraint for the type that the values use.</typeparam>
    /// <param name="kvps">A collection of <see cref="KeyValuePair{TLabel,TValue}" />s to use for the labeled radio list.</param>
    /// <returns>A collection of <see cref="LabeledRadioValue{TLabel,TValue}" /> that match the labels.</returns>
    private static IEnumerable<LabeledRadioValue<TLabel, TValue>> GenerateLabeledRadioValues<TLabel, TValue>(IEnumerable<KeyValuePair<TLabel, TValue>> kvps)
    {
        foreach (KeyValuePair<TLabel, TValue> kvp in kvps)
        {
            yield return new LabeledRadioValue<TLabel, TValue>(kvp.Key, kvp.Value);
        }
    }

    /// <summary>
    /// Renders a radio list to the given <see cref="Listing_Standard" /> <see cref="LabeledRadioValue{TLabel,TValue}" /> list as it's values and labels.
    /// </summary>
    /// <typeparam name="TLabel">A type constraint for the type that the labels use.</typeparam>
    /// <typeparam name="TValue">A type constraint for the type that the values use.</typeparam>
    /// <param name="listing_Standard">The <see cref="Listing_Standard" /> to append the labeled radio list to.</param>
    /// <param name="items">A collection of <see cref="LabeledRadioValue{TLabel,TValue}" />s to use for the labeled radio list.</param>
    /// <param name="val">An optional <see langword="float" /> to set the height of the header.</param>
    /// <param name="height">An optional <see langword="float" /> to set the height of the header.</param>
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