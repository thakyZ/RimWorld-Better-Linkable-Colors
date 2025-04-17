using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;
using RimWorld;
using System.Runtime.Remoting.Contexts;
#if RIMWORLD_13_OR_LESS
using Drummeur.BetterLinkableColors.Util;
#endif
// Ignore Spelling: unformatter
namespace Drummeur.BetterLinkableColors;
/// <summary>
/// A class to help provide UI components for the settings window.
/// </summary>
/// <remarks>
/// <para>Originally from <see href="https://github.com/RimWorld-CCL-Reborn/4M-Mehni-s-Misc-Modifications/blob/44ef9ee6e64c15d1d9acb0a7e2aa3ae307c2e354/Mehni's%20Misc%20Modifications/SettingsHelper.cs"/></para>
/// <para>Thanks to AlexTD for the below.</para>
/// </remarks>
internal static class SettingsHelper
{
    /// <summary>
    /// A constant <see langword="float" /> _value determining the gap size that should be used.
    /// </summary>
    private const float _GAP = 12f;

    /// <summary>
    /// Renders a solid _color box as a button with a label.
    /// </summary>
    /// <param name="listing_standard">The <see cref="Listing_Standard" /> to append the labeled slider to.</param>
    /// <param name="label">A <see langword="string" /> that represents the label to add to the solid _color button.</param>
    /// <param name="color">The <see cref="Color" /> to render the solid _color button with.</param>
    /// <returns><see langword="true" /> when the button has been pressed; otherwise <see langword="false" />.</returns>
    /// <returns><see langword="true" /> when the <paramref name="value" /> has been altered; otherwise <see langword="false" />.</returns>
    public static bool DrawButtonSolidColorLabeled(this Listing_Standard listing_standard, string label, Color color)
    {
        var output = false;
        Rect rect = listing_standard.GetRect();
        var labelWidth = listing_standard.ColumnWidth - Text.LineHeight;
        rect.SplitVertically(labelWidth, out Rect rect2, out Rect rect3);
        Rect rect4 = rect3.ContractedBy(2);
        TextAnchor anchor = Text.Anchor;
        Text.Anchor = TextAnchor.MiddleLeft;
        Widgets.Label(rect2, label);
        Text.Anchor = TextAnchor.MiddleRight;
        Widgets.DrawBoxSolid(rect3, Color.black);
        Widgets.DrawBoxSolid(rect4, color);
        Widgets.DrawHighlightIfMouseover(rect4);

        if (Widgets.ButtonInvisible(rect, true))
        {
            SoundDefOf.Tick_High.PlayOneShotOnCamera();
            output = true;
        }

        Text.Anchor = anchor;
        return output;
    }

    /// <summary>
    /// Renders a labeled slider with a reference <paramref name="value" />.
    /// </summary>
    /// <param name="listing_standard">The <see cref="Listing_Standard" /> to append the labeled slider to.</param>
    /// <param name="label">A <see langword="string" /> that represents the label to add to the slider.</param>
    /// <param name="value">An <see langword="int" /> reference _value that represents the slider's current _value.</param>
    /// <param name="formatter">A function that formats the <see langword="int" /> _value into a valid <see langword="string" />.</param>
    /// <param name="min">An optional <see langword="int" /> that determines the minimum _value for the slider.</param>
    /// <param name="max">An optional <see langword="int" /> that determines the maximum _value for the slider.</param>
    /// <param name="tooltip">An optional <see langword="string" /> that defines the tooltip to use for the slider.</param>
    /// <returns><see langword="true" /> when the <paramref name="value" /> has been altered; otherwise <see langword="false" />.</returns>
    public static bool SliderLabeled(this Listing_Standard listing_standard,
        string label, ref int value, Func<int, string>? formatter = null, int min = 0, int max = 100, string? tooltip = null, float? margin = null)
    {
        float floatValue = value;
        var output = listing_standard.SliderLabeled(label, ref floatValue, (float _val) => formatter?.Invoke((int)_val) ?? $"{(int)_val}", min, max, tooltip, margin);
        value = (int)floatValue;
        return output;
    }

    /// <summary>
    /// Renders a labeled slider with a reference <paramref name="value" />.
    /// </summary>
    /// <param name="listing_standard">The <see cref="Listing_Standard" /> to append the labeled slider to.</param>
    /// <param name="label">A <see langword="string" /> that represents the label to add to the slider.</param>
    /// <param name="value">A <see langword="float" /> reference _value that represents the slider's current _value.</param>
    /// <param name="formatter">A function that formats the <see langword="float" /> _value into a valid <see langword="string" />.</param>
    /// <param name="min">An optional <see langword="int" /> that determines the minimum _value for the slider.</param>
    /// <param name="max">An optional <see langword="int" /> that determines the maximum _value for the slider.</param>
    /// <param name="tooltip">An optional <see langword="string" /> that defines the tooltip to use for the slider.</param>
    /// <returns><see langword="true" /> when the <paramref name="value" /> has been altered; otherwise <see langword="false" />.</returns>
    public static bool SliderLabeled(this Listing_Standard listing_standard,
        string label, ref float value, Func<float, string>? formatter = null, float min = 0f, float max = 1f, string? tooltip = null, float? margin = null)
    {
        var oldValue = value;
        Rect rect = listing_standard.GetRect();
        margin ??= 0.7f;
        Rect rect2 = rect.LeftPart(margin.Value).Rounded();
        Rect rect3 = rect.RightPart(1.0f - margin.Value).Rounded().LeftPart(0.8f).Rounded();
        Rect rect4 = rect.RightPart(0.2f).Rounded();

        TextAnchor anchor = Text.Anchor;
        Text.Anchor = TextAnchor.MiddleLeft;
        Widgets.Label(rect2, label);

#if RIMWORLD_14
        Widgets.HorizontalSlider(rect3, ref value, new FloatRange(min, max));
#else
        value = Widgets.HorizontalSlider(rect3, value, min, max, true);
        Text.Anchor = TextAnchor.MiddleRight;
#endif

        Widgets.Label(rect4, formatter?.Invoke(value) ?? $"{value}");

        if (!tooltip.NullOrEmpty())
        {
            Widgets.DrawHighlightIfMouseover(rect);
            TooltipHandler.TipRegion(rect, tooltip);
        }

        Text.Anchor = anchor;
        listing_standard.Gap(listing_standard.verticalSpacing);
        return oldValue != value;
    }

    /// <summary>
    /// Renders a labeled slider with a reference <paramref name="value" />.
    /// </summary>
    /// <param name="listing_standard">The <see cref="Listing_Standard" /> to append the labeled slider to.</param>
    /// <param name="label">A <see langword="string" /> that represents the label to add to the slider.</param>
    /// <param name="value">A <see langword="float" /> reference _value that represents the slider's current _value.</param>
    /// <param name="min">An optional <see langword="int" /> that determines the minimum _value for the slider.</param>
    /// <param name="max">An optional <see langword="int" /> that determines the maximum _value for the slider.</param>
    /// <param name="tooltip">An optional <see langword="string" /> that defines the tooltip to use for the slider.</param>
    /// <returns><see langword="true" /> when the <paramref name="value" /> has been altered; otherwise <see langword="false" />.</returns>
    public static bool SliderLabeled(this Listing_Standard listing_standard,
        string label, ref float value, float min = 0f, float max = 1f, string? tooltip = null, float? margin = null)
    {
        var oldValue = value;
        Rect rect = listing_standard.GetRect();
        margin ??= 0.7f;
        Rect rect2 = rect.LeftPart(margin.Value).Rounded();
        Rect rect3 = rect.RightPart(1.0f - margin.Value).Rounded();

        TextAnchor anchor = Text.Anchor;
        Text.Anchor = TextAnchor.MiddleLeft;
        Widgets.Label(rect2, label);

#if RIMWORLD_14
        Widgets.HorizontalSlider(rect3, ref value, new FloatRange(min, max));
#else
        value = Widgets.HorizontalSlider(rect3, value, min, max, true);
        Text.Anchor = TextAnchor.MiddleRight;
#endif

        if (!tooltip.NullOrEmpty())
        {
            Widgets.DrawHighlightIfMouseover(rect);
            TooltipHandler.TipRegion(rect, tooltip);
        }

        Text.Anchor = anchor;
        listing_standard.Gap(listing_standard.verticalSpacing);
        return oldValue != value;
    }

    /// <summary>
    /// Renders a labeled slider with a reference <paramref name="value" /> that can be manually set via a reference <paramref name="buffer" />.
    /// </summary>
    /// <param name="listing_standard">The <see cref="Listing_Standard" /> to append the labeled slider to.</param>
    /// <param name="label">A <see langword="string" /> that represents the label to add to the slider.</param>
    /// <param name="value">A <see langword="float" /> reference _value that represents the slider's current _value.</param>
    /// <param name="formatter">A function that formats the <see langword="float" /> _value into a valid <see langword="string" />.</param>
    /// <param name="min">An optional <see langword="float" /> that determines the minimum _value for the slider.</param>
    /// <param name="max">An optional <see langword="float" /> that determines the maximum _value for the slider.</param>
    /// <param name="tooltip">An optional <see langword="string" /> that defines the tooltip to use for the slider.</param>
    /// <returns><see langword="true" /> when the <paramref name="value" /> has been altered; otherwise <see langword="false" />.</returns>
    public static bool SliderLabeledSettable(this Listing_Standard listing_standard,
        string label, ref float value, Func<float, string>? bufferFormatter = null,
        Func<float, float>? formatter = null, Func<float, float>? unformatter = null,
        float min = 0f, float max = 1f,
        float? bufferMin = null, float? bufferMax = null,
        string? tooltip = null, float? margin = null)
    {
        if (formatter is null || unformatter is null)
        {
            throw new ArgumentException($"Both {nameof(formatter)} and {nameof(unformatter)} must be specified or neither.");
        }

        var oldValue = value;
        Rect rect = listing_standard.LineRectSplitter(out Rect leftHalf, out Rect rightHalf, 0.9f, Text.LineHeight);
        var left_listing_standard = new Listing_Standard();
        left_listing_standard.Begin(leftHalf);
        _ = left_listing_standard.SliderLabeled(label, ref value, min, max, null, margin);
        left_listing_standard.End();
        var right_listing_standard = new Listing_Standard();
        right_listing_standard.Begin(rightHalf);
        var _value = formatter?.Invoke(value) ?? value;
        var buffer = bufferFormatter?.Invoke(_value) ?? $"{_value}";
        right_listing_standard.TextFieldNumeric(ref _value, ref buffer, bufferMin ?? min, bufferMax ?? max);
        value = unformatter?.Invoke(_value) ?? _value;
        left_listing_standard.End();
        if (!tooltip.NullOrEmpty())
        {
            Widgets.DrawHighlightIfMouseover(rect);
            TooltipHandler.TipRegion(rect, tooltip);
        }

        return oldValue != value;
    }

    /// <summary>
    /// Renders a labeled slider with a reference <paramref name="value" /> that can be manually set via a reference <paramref name="buffer" />.
    /// </summary>
    /// <param name="listing_standard">The <see cref="Listing_Standard" /> to append the labeled slider to.</param>
    /// <param name="label">A <see langword="string" /> that represents the label to add to the slider.</param>
    /// <param name="value">A <see langword="float" /> reference _value that represents the slider's current _value.</param>
    /// <param name="buffer">A <see langword="string" /> reference _value that represents the buffer to set the slider's current _value.</param>
    /// <param name="min">An optional <see langword="float" /> that determines the minimum _value for the slider.</param>
    /// <param name="max">An optional <see langword="float" /> that determines the maximum _value for the slider.</param>
    /// <param name="tooltip">An optional <see langword="string" /> that defines the tooltip to use for the slider.</param>
    /// <returns><see langword="true" /> when the <paramref name="value" /> has been altered; otherwise <see langword="false" />.</returns>
    public static bool SliderLabeledSettable(this Listing_Standard listing_standard,
        string label, ref float value, ref string buffer,
        float min = 0f, float max = 1f,
        float? bufferMin = null, float? bufferMax = null,
        string? tooltip = null, float? margin = null)
    {
        var oldValue = value;
        Rect rect = listing_standard.LineRectSplitter(out Rect leftHalf, out Rect rightHalf, 0.9f, Text.LineHeight);
        var left_listing_standard = new Listing_Standard();
        left_listing_standard.Begin(leftHalf);
        _ = left_listing_standard.SliderLabeled(label, ref value, min, max, null, margin);
        left_listing_standard.End();
        var right_listing_standard = new Listing_Standard();
        right_listing_standard.Begin(rightHalf);
        right_listing_standard.TextFieldNumeric(ref value, ref buffer, bufferMin ?? min, bufferMax ?? max);
        left_listing_standard.End();
        if (!tooltip.NullOrEmpty())
        {
            Widgets.DrawHighlightIfMouseover(rect);
            TooltipHandler.TipRegion(rect, tooltip);
        }

        return oldValue != value;
    }

    /// <summary>
    /// Renders a ranged slider with a reference <paramref name="range" />.
    /// </summary>
    /// <param name="listing_standard">The <see cref="Listing_Standard" /> to append the range slider to.</param>
    /// <param name="label">A <see langword="string" /> that represents the label to add to the range slider.</param>
    /// <param name="range">A <see cref="Verse.FloatRange" /> reference that can be used to determine the _value that the range slider is set to.</param>
    /// <param name="min">An optional <see langword="float" /> that determines the minimum _value for the range slider.</param>
    /// <param name="max">An optional <see langword="float" /> that determines the maximum _value for the range slider.</param>
    /// <param name="tooltip">An optional <see langword="string" /> that defines the tooltip to use for the range slider.</param>
    /// <param name="valueStyle">An optional <see cref="ToStringStyle" /> to use for the rendering the text of the float</param>
    /// <returns><see langword="true" /> when the <paramref name="value" /> has been altered; otherwise <see langword="false" />.</returns>
    public static bool FloatRange(this Listing_Standard listing_standard,
        string label, ref FloatRange range, float min = 0f, float max = 1f, string? tooltip = null, ToStringStyle valueStyle = ToStringStyle.FloatTwo)
    {
        FloatRange originalRange = range;
        Rect rect   = listing_standard.GetRect();
        Rect rect2  = rect.LeftPart(.70f).Rounded();
        Rect rect3  = rect.RightPart(.30f).Rounded().LeftPart(.9f).Rounded();
        rect3.yMin -= 5f;

        TextAnchor anchor = Text.Anchor;
        Text.Anchor = TextAnchor.MiddleLeft;
        Widgets.Label(rect2, label);

        Text.Anchor = TextAnchor.MiddleRight;
        var id = listing_standard.CurHeight.GetHashCode();
        Widgets.FloatRange(rect3, id, ref range, min, max, null, valueStyle);

        if (!tooltip.NullOrEmpty())
        {
            Widgets.DrawHighlightIfMouseover(rect);
            TooltipHandler.TipRegion(rect, tooltip);
        }

        Text.Anchor = anchor;
        listing_standard.Gap(listing_standard.verticalSpacing);
        return range != originalRange;
    }

    /// <summary>
    /// Renders a labeled text field with a reference <paramref name="value" />.
    /// </summary>
    /// <param name="listing_standard">The <see cref="Listing_Standard" /> to append the range slider to.</param>
    /// <param name="label">A <see langword="string" /> that represents the label to add to the range slider.</param>
    /// <param name="value">A <see langword="string" /> reference _value that represents the buffer to set the text field's current _value.</param>
    /// <param name="tooltip">An optional <see langword="string" /> that defines the tooltip to use for the range slider.</param>
    /// <param name="leftPartPct">An optional <see langword="float" /> determining the percentage to use from the left of the given <see cref="Listing_Standard" />'s bounds.</param>
    /// <returns><see langword="true" /> when the <paramref name="value" /> has been altered; otherwise <see langword="false" />.</returns>
    public static bool AddLabeledTextField(this Listing_Standard listing_standard, string label, ref string value, Func<string, string>? formatter = null, string? tooltip = null, float leftPartPct = 0.5f, StringComparison comparison = StringComparison.Ordinal)
    {
        var oldValue = value;
        listing_standard.Gap(_GAP);
        _ = listing_standard.LineRectSplitter(out Rect leftHalf, out Rect rightHalf, leftPartPct);

        if (!tooltip.NullOrEmpty())
        {
            Widgets.DrawHighlightIfMouseover(leftHalf);
            TooltipHandler.TipRegion(leftHalf, tooltip);
        }

        Widgets.Label(leftHalf, label);

        var buffer = formatter?.Invoke(value) ?? $"{value}";
        value = Widgets.TextField(rightHalf, buffer);
        return !value.Equals(oldValue, comparison);
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
    /// Renders a labeled numerical text field with a reference <paramref name="value" />.
    /// </summary>
    /// <typeparam name="T">A type param that should match a built in numeric type.</typeparam>
    /// <param name="listing_standard">The <see cref="Listing_Standard" /> to append the labeled numeric text field to.</param>
    /// <param name="label">A <see langword="string" /> that represents the label to add to the numeric text field.</param>
    /// <param name="value">A <see langword="string" /> reference _value that represents the buffer to set the numeric text field's current value.</param>
    /// <param name="tooltip">An optional <see langword="string" /> that defines the tooltip to use for the range slider.</param>
    /// <param name="leftPartPct">An optional <see langword="float" /> determining the percentage to use from the left of the given <see cref="Listing_Standard" />'s bounds.</param>
    /// <param name="min">An optional <see langword="float" /> that determines the minimum _value for the slider.</param>
    /// <param name="max">An optional <see langword="float" /> that determines the maximum _value for the slider.</param>
    /// <returns><see langword="true" /> when the <paramref name="value" /> has been altered; otherwise <see langword="false" />.</returns>
    public static bool AddLabeledNumericalTextField<T>(this Listing_Standard listing_standard, string label, ref T value, Func<T, string>? formatter = null, string? tooltip = null, float leftPartPct = 0.5f, float min = 1f, float max = 100000f) where T : struct
    {
        if (!IsNumericType(typeof(T)))
        {
            _ = listing_standard.SubLabel($"Type of generic argument {nameof(T)} is not a numerical value, got \"{typeof(T).GetType().Name}\".", 1.0f);
            return false;
        }

        T oldValue = value;
        listing_standard.Gap(_GAP);
        _ = listing_standard.LineRectSplitter(out Rect leftHalf, out Rect rightHalf, leftPartPct);

        if (!tooltip.NullOrEmpty())
        {
            Widgets.DrawHighlightIfMouseover(leftHalf);
            TooltipHandler.TipRegion(leftHalf, tooltip);
        }

        Widgets.Label(leftHalf, label);

        var buffer = formatter?.Invoke(value) ?? $"{value}";
        Widgets.TextFieldNumeric(rightHalf, ref value, ref buffer, min, max);
        return !EqualityComparer<T>.Default.Equals(value, oldValue);
    }

    /// <summary>
    /// Renders a labeled text field with a prefix using a reference <paramref name="value" />.
    /// </summary>
    /// <param name="listing_standard">The <see cref="Listing_Standard" /> to append the labeled numeric text field to.</param>
    /// <param name="label">A <see langword="string" /> that represents the label to add to the numeric text fields.</param>
    /// <param name="value">A <see langword="string" /> reference _value that represents the buffer to set the numeric text field's current value.</param>
    /// <param name="formatter">A function that formats the <see langword="string" /> value into a valid <see langword="string" /> with prefix.</param>
    /// <param name="tooltip">An optional <see langword="string" /> that defines the tooltip to use for the range slider.</param>
    /// <param name="leftPartPct">An optional <see langword="float" /> determining the percentage to use from the left of the given <see cref="Listing_Standard" />'s bounds.</param>
    /// <returns><see langword="true" /> when the <paramref name="value" /> has been altered; otherwise <see langword="false" />.</returns>
    public static bool AddLabeledTextFieldWithPrefix(this Listing_Standard listing_standard, string label, ref string value, Func<string, string>? formatter = null, Func<string, string>? unformatter = null, string? tooltip = null, float leftPartPct = 0.5f, StringComparison comparisonType = StringComparison.Ordinal)
    {
        if (formatter is null || unformatter is null)
        {
            throw new ArgumentException($"Both {nameof(formatter)} and {nameof(unformatter)} must be specified or neither.");
        }

        var oldValue = value;
        listing_standard.Gap(_GAP);
        _ = listing_standard.LineRectSplitter(out Rect leftHalf, out Rect rightHalf, leftPartPct);

        if (!tooltip.NullOrEmpty())
        {
            Widgets.DrawHighlightIfMouseover(leftHalf);
            TooltipHandler.TipRegion(leftHalf, tooltip);
        }

        Widgets.Label(leftHalf, label);

        var buffer = formatter?.Invoke(value) ?? value;
        var temp = Widgets.TextField(rightHalf, buffer);
        value = unformatter?.Invoke(temp) ?? temp;
        return !oldValue.Equals(value, comparisonType);
    }

    /// <summary>
    /// Renders a split in the given <see cref="Listing_Standard" /> that splits the area vertically.
    /// </summary>
    /// <param name="listing_standard">The <see cref="Listing_Standard" /> to split the area of.</param>
    /// <param name="leftHalf">An output <see cref="Rect" /> that determines the bounds for the left half of the split area.</param>
    /// <param name="leftPartPct">An optional <see langword="float" /> that determines at what percentage should the <paramref name="leftHalf"/> be split at.</param>
    /// <param name="height">An optional <see langword="float" /> that determines the height of the split area.</param>
    /// <returns>A <see cref="Rect" /> of the split area in the <see cref="Listing_Standard" />.</returns>
    public static Rect LineRectSplitter(this Listing_Standard listing_standard, out Rect leftHalf, float leftPartPct = 0.5f, float? height = null)
    {
        Rect lineRect = listing_standard.GetRect(height);
        leftHalf = lineRect.LeftPart(leftPartPct).Rounded();
        return lineRect;
    }

    /// <summary>
    /// Renders a split in the given <see cref="Listing_Standard" /> that splits the area vertically.
    /// </summary>
    /// <param name="listing_standard">The <see cref="Listing_Standard" /> to split the area of.</param>
    /// <param name="leftHalf">An output <see cref="Rect" /> that determines the bounds for the left half of the split area.</param>
    /// <param name="rightHalf">An output <see cref="Rect" /> that determines the bounds for the right half of the split area.</param>
    /// <param name="leftPartPct">An optional <see langword="float" /> that determines at what percentage should the <paramref name="leftHalf"/> be split at.</param>
    /// <param name="height">An optional <see langword="float" /> that determines the height of the split area.</param>
    /// <returns>A <see cref="Rect" /> of the split area in the <see cref="Listing_Standard" />.</returns>
    public static Rect LineRectSplitter(this Listing_Standard listing_standard, out Rect leftHalf, out Rect rightHalf, float leftPartPct = 0.5f, float? height = null)
    {
        Rect lineRect = listing_standard.LineRectSplitter(out leftHalf, leftPartPct, height);
        rightHalf = lineRect.RightPart(1f - leftPartPct).Rounded();
        return lineRect;
    }

    /// <summary>
    /// Gets a <see cref="Rect" /> of the given <see cref="Listing_Standard" />.
    /// </summary>
    /// <param name="listing_standard">The <see cref="Listing_Standard" /> to split the area of.</param>
    /// <param name="height">An optional <see langword="float" /> that determines the height of the returned <see cref="Rect" />.</param>
    /// <returns>A <see cref="Rect" /> of the <see cref="Listing_Standard" /> bounds.</returns>
    public static Rect GetRect(this Listing_Standard listing_standard, float? height = null)
    {
        return listing_standard.GetRect(height ?? Text.LineHeight);
    }

    #region Labeled Radio Buttons

    /// <summary>
    /// A class to encapsulate the label and _value of a labeled radio option.
    /// </summary>
    /// <typeparam name="TLabel">A type constraint for the label of the radio option.</typeparam>
    /// <typeparam name="TValue">A type constraint for the _value of the radio option.</typeparam>
    public class LabeledRadioValue<TLabel, TValue>
    {
        /// <summary>
        /// Gets or sets the label of the radio option.
        /// </summary>
        public TLabel Label { get; set; }

        /// <summary>
        /// Gets or sets the _value of the radio option.
        /// </summary>
        public TValue Value { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LabeledRadioValue{TLabel,TValue}" /> class.
        /// </summary>
        /// <param name="label">The label of the new radio option.</param>
        /// <param name="value">The _value of the new radio option.</param>
        public LabeledRadioValue(TLabel label, TValue value)
        {
            this.Label = label;
            this.Value = value;
        }
    }

    /// <summary>
    /// A class to encapsulate the label and _value of a labeled radio option.
    /// </summary>
    /// <typeparam name="T">A type constraint for the _value of the radio option.</typeparam>
    public class LabeledRadioValue<T> : LabeledRadioValue<string, T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LabeledRadioValue{T}" /> class.
        /// </summary>
        /// <param name="label">The label of the new radio option.</param>
        /// <param name="value">The _value of the new radio option.</param>
        public LabeledRadioValue(string label, T value) : base(label, value) { }
    }

    /// <summary>
    /// Renders a labeled radio list to the given <see cref="Listing_Standard" />.
    /// </summary>
    /// <typeparam name="T">A type constraint for the type that the labels use.</typeparam>
    /// <param name="listing_standard">The <see cref="Listing_Standard" /> to append the labeled radio list to.</param>
    /// <param name="header">A <see langword="string" /> that represents the header to use for the labeled radio list.</param>
    /// <param name="labels">A collection of <typeparamref name="T" />s to use for the labeled radio list.</param>
    /// <param name="val">A <typeparamref name="T" /> reference to set the _value of the labeled radio list.</param>
    /// <param name="headerHeight">An optional <see langword="float" /> to set the height of the header.</param>
    /// <remarks>
    /// Thanks to Why_is_that for the below, who in turn got his stuff from<br />
    /// REFERENCE: <see href="https://github.com/erdelf/GodsOfRimworld/blob/18ebc693ef17ab1d28b58a2c9d4d3f7d48d95517/Source/Ankh/ModControl.cs" /><br />
    /// REFERENCE: <see href="https://github.com/erdelf/PrisonerRansom/" /><br />
    /// modified by Drummeur to make it use generics and <see cref="IEnumerable{T}" />s.
    /// </remarks>
    public static void AddLabeledRadioList<T>(this Listing_Standard listing_standard, string? header, IEnumerable<T> labels, ref T val, float? headerHeight = null)
    {
        if (!string.IsNullOrEmpty(header))
        {
            Widgets.Label(listing_standard.GetRect(headerHeight), header);
        }

        listing_standard.AddRadioList(GenerateLabeledRadioValues(labels), ref val);
    }

    /// <summary>
    /// Renders a labeled radio list to the given <see cref="Listing_Standard" /> with a function to format the _value to a string.
    /// </summary>
    /// <typeparam name="T">A type constraint for the type that the labels use.</typeparam>
    /// <param name="listing_standard">The <see cref="Listing_Standard" /> to append the labeled radio list to.</param>
    /// <param name="header">A <see langword="string" /> that represents the header to use for the labeled radio list.</param>
    /// <param name="labels">A collection of <typeparamref name="T" />s to use for the labeled radio list.</param>
    /// <param name="val">A <typeparamref name="T" /> reference to set the _value of the labeled radio list.</param>
    /// <param name="formatter">A function that formats the <typeparamref name="T" /> _value into a valid <see langword="string" />.</param>
    /// <param name="headerHeight">An optional <see langword="float" /> to set the height of the header.</param>
    public static void AddLabeledRadioList<T>(this Listing_Standard listing_standard, string? header, IEnumerable<T> labels, ref T val, Func<T, string> formatter, float? headerHeight = null)
    {
        if (!string.IsNullOrEmpty(header))
        {
            Widgets.Label(listing_standard.GetRect(headerHeight), header);
        }

        listing_standard.AddRadioList(GenerateLabeledRadioValues(labels, formatter), ref val);
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
    /// <param name="formatter">A function that formats the <typeparamref name="T" /> _value into a valid <see langword="string" />.</param>
    /// <returns>A collection of <see cref="LabeledRadioValue{T}" /> that match the labels.</returns>
    private static IEnumerable<LabeledRadioValue<T>> GenerateLabeledRadioValues<T>(IEnumerable<T> labels, Func<T, string> formatter)
    {
        foreach (T? label in labels)
        {
            yield return new LabeledRadioValue<T>(formatter(label) ?? "NULL", label);
        }
    }

    /// <summary>
    /// Renders a radio list to the given <see cref="Listing_Standard" /> with a function to format the _value to a string
    /// with a collection of <see cref="LabeledRadioValue{T}" />.
    /// </summary>
    /// <typeparam name="T">A type constraint for the type that the labels use.</typeparam>
    /// <param name="listing_standard">The <see cref="Listing_Standard" /> to append the labeled radio list to.</param>
    /// <param name="items">A collection of <see cref="LabeledRadioValue{T}" />s to use for the labeled radio list.</param>
    /// <param name="value">A <typeparamref name="T" /> reference to set the _value of the labeled radio list.</param>
    /// <param name="height">An optional <see langword="float" /> to set the height of the header.</param>
    private static void AddRadioList<T>(this Listing_Standard listing_standard, IEnumerable<LabeledRadioValue<T>> items, ref T value, float? height = null)
    {
        foreach (LabeledRadioValue<T> item in items)
        {
#if RIMWORLD_15_OR_GREATER
            if (Widgets.RadioButtonLabeled(listing_standard.GetRect(height), item.Label, EqualityComparer<T>.Default.Equals(item.Value, value), false))
#else
            if (Widgets.RadioButtonLabeled(listing_standard.GetRect(height), item.Label, false) && EqualityComparer<T>.Default.Equals(item.Value, value))
#endif
            {
                value = item.Value;
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
    /// <param name="val">A <typeparamref name="TValue" /> reference to set the _value of the labeled radio list.</param>
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
    /// <param name="listing_standard">The <see cref="Listing_Standard" /> to append the labeled radio list to.</param>
    /// <param name="items">A collection of <see cref="LabeledRadioValue{TLabel,TValue}" />s to use for the labeled radio list.</param>
    /// <param name="value">An <typeparamref name="TValue" /> reference to set the _value of the labeled radio list.</param>
    /// <param name="height">An optional <see langword="float" /> to set the height of the header.</param>
    private static void AddRadioList<TLabel, TValue>(this Listing_Standard listing_standard, IEnumerable<LabeledRadioValue<TLabel, TValue>> items, ref TValue value, float? height = null)
    {
        foreach (LabeledRadioValue<TLabel, TValue> item in items)
        {
#if RIMWORLD_15_OR_GREATER
            if (Widgets.RadioButtonLabeled(listing_standard.GetRect(height), item.Label?.ToString() ?? "NULL", EqualityComparer<TValue>.Default.Equals(item.Value, value), false))
#else
            if (Widgets.RadioButtonLabeled(listing_standard.GetRect(height), item.Label?.ToString() ?? "NULL", false) && EqualityComparer<TValue>.Default.Equals(item.Value, value))
#endif
            {
                value = item.Value;
            }
        }
    }

    #endregion Labeled Radio Buttons

    /// <summary>
    /// Draws a checkered color background.
    /// </summary>
    /// <param name="texture">The bounds of the checkered color background.</param>
    public static void DoTransparentBackground(ref Texture2D texture)
    {
        var count = 0;
        var gridSize = Math.Min(texture.width, texture.height) / 3d;
        var gridHeight = (int)Math.Ceiling(texture.height / gridSize);
        var gridWidth = (int)Math.Ceiling(texture.width / gridSize);
        var lighter = new Color(0.5f, 0.5f, 0.5f);
        var drarker = new Color(0.75f, 0.75f, 0.75f);
        for (var y = 0; y < gridHeight; y++) {
            for (var x = 0; x < gridWidth; x++) {
                texture.SetPixels(
                    x: 0 + (int)(x * gridSize),
                    y: 0 + (int)(y * gridSize),
                    blockWidth: (int)Math.Min(gridSize, texture.width - (x * gridSize)),
                    blockHeight: (int)Math.Min(gridSize, texture.height - (y * gridSize)),
                    colors: [count % 2 == 0 ? drarker : lighter]);
                    count++;
            }

            if (gridHeight < gridWidth) {
                count++;
            }
        }
    }
}