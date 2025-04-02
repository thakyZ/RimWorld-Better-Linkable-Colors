// originally from https://github.com/RimWorld-CCL-Reborn/4M-Mehni-s-Misc-Modifications/blob/master/Mehnis%20Misc%20Modifications/SettingsHelper.cs
using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace drummeur.linkablecolors
{

    //thanks to AlexTD for the below
    internal static class SettingsHelper
    {
        //private static float gap = 12f;

        //public static void SliderLabeled(this Listing_Standard ls, string label, ref int val, Func<Num, string> formatter, float min = 0f, float max = 100f, string tooltip = null)
        //{
        //    float fVal = val;
        //    ls.SliderLabeled(label, ref fVal, formatter, min, max);
        //    val = (int)fVal;
        //}

        public static void SliderLabeled(this Listing_Standard ls, string label, ref float val, Func<float, string> formatter, float min = 0f, float max = 1f, string tooltip = null)
        {
            Rect rect = ls.GetRect(Text.LineHeight);
            Rect rect2 = rect.LeftPart(.70f).Rounded();
            Rect rect3 = rect.RightPart(.30f).Rounded().LeftPart(.67f).Rounded();
            Rect rect4 = rect.RightPart(.10f).Rounded();

            TextAnchor anchor = Text.Anchor;
            Text.Anchor = TextAnchor.MiddleLeft;
            Widgets.Label(rect2, label);

            float result = Widgets.HorizontalSlider(rect3, val, min, max, true);
            val = result;
            Text.Anchor = TextAnchor.MiddleRight;
            Widgets.Label(rect4, formatter(val));
            if (!tooltip.NullOrEmpty())
            {
                TooltipHandler.TipRegion(rect, tooltip);
            }

            Text.Anchor = anchor;
            ls.Gap(ls.verticalSpacing);
        }

        public static void SliderLabeledSettable(this Listing_Standard ls, string label, ref float val, ref string buffer, Func<float, string> formatter, float min = 0f, float max = 1f, string tooltip = null)
        {
            Rect rect = ls.GetRect(Text.LineHeight);
            Rect rect2 = rect.LeftPart(.70f).Rounded();
            Rect rect3 = rect.RightPart(.30f).Rounded().LeftPart(.67f).Rounded();
            Rect rect4 = rect.RightPart(.10f).Rounded();

            TextAnchor anchor = Text.Anchor;
            Text.Anchor = TextAnchor.MiddleLeft;
            Widgets.Label(rect2, label);

            float result = Widgets.HorizontalSlider(rect3, val, min, max, true);
            val = result;
            Text.Anchor = TextAnchor.MiddleRight;

            ls.TextFieldNumeric(ref val, ref buffer, min, max);
            //Widgets.Label(rect4, formatter(val));
            if (!tooltip.NullOrEmpty())
            {
                TooltipHandler.TipRegion(rect, tooltip);
            }

            Text.Anchor = anchor;
            ls.Gap(ls.verticalSpacing);
        }

        public static void FloatRange(this Listing_Standard ls, string label, ref FloatRange range, float min = 0f, float max = 1f, string tooltip = null, ToStringStyle valueStyle = ToStringStyle.FloatTwo)
        {
            Rect rect = ls.GetRect(Text.LineHeight);
            Rect rect2 = rect.LeftPart(.70f).Rounded();
            Rect rect3 = rect.RightPart(.30f).Rounded().LeftPart(.9f).Rounded();
            rect3.yMin -= 5f;
            //Rect rect4 = rect.RightPart(.10f).Rounded();

            TextAnchor anchor = Text.Anchor;
            Text.Anchor = TextAnchor.MiddleLeft;
            Widgets.Label(rect2, label);

            Text.Anchor = TextAnchor.MiddleRight;
            int id = ls.CurHeight.GetHashCode();
            Widgets.FloatRange(rect3, id, ref range, min, max, null, valueStyle);
            if (!tooltip.NullOrEmpty())
            {
                TooltipHandler.TipRegion(rect, tooltip);
            }
            Text.Anchor = anchor;
            ls.Gap(ls.verticalSpacing);
        }

        public static void AddLabeledTextField(this Listing_Standard listing_Standard, string label, ref string settingsValue, float leftPartPct = 0.5f)
        {
            //listing_Standard.Gap(Gap);
            listing_Standard.LineRectSpilter(out Rect leftHalf, out Rect rightHalf, leftPartPct);

            // TODO: tooltips
            //Widgets.DrawHighlightIfMouseover(lineRect);
            //TooltipHandler.TipRegion(lineRect, "TODO: TIP GOES HERE");

            Widgets.Label(leftHalf, label);

            string buffer = settingsValue.ToString();
            settingsValue = Widgets.TextField(rightHalf, buffer);
        }

        public static void AddLabeledNumericalTextField<T>(this Listing_Standard listing_Standard, string label, ref T settingsValue, float leftPartPct = 0.5f, float minValue = 1f, float maxValue = 100000f) where T : struct
        {
            //listing_Standard.Gap(Gap);
            listing_Standard.LineRectSpilter(out Rect leftHalf, out Rect rightHalf, leftPartPct);

            // TODO: tooltips
            //Widgets.DrawHighlightIfMouseover(lineRect);
            //TooltipHandler.TipRegion(lineRect, "TODO: TIP GOES HERE");

            Widgets.Label(leftHalf, label);

            string buffer = settingsValue.ToString();
            Widgets.TextFieldNumeric(rightHalf, ref settingsValue, ref buffer, minValue, maxValue);
        }

        public static Rect LineRectSpilter(this Listing_Standard listing_Standard, out Rect leftHalf, float leftPartPct = 0.5f, float? height = null)
        {
            Rect lineRect = listing_Standard.GetRect(height);
            leftHalf = lineRect.LeftPart(leftPartPct).Rounded();
            return lineRect;
        }

        public static Rect LineRectSpilter(this Listing_Standard listing_Standard, out Rect leftHalf, out Rect rightHalf, float leftPartPct = 0.5f, float? height = null)
        {
            Rect lineRect = listing_Standard.LineRectSpilter(out leftHalf, leftPartPct, height);
            rightHalf = lineRect.RightPart(1f - leftPartPct).Rounded();
            return lineRect;
        }

        public static Rect GetRect(this Listing_Standard listing_Standard, float? height = null)
        {
            return listing_Standard.GetRect(height ?? Text.LineHeight);
        }

        #region labeledradiobuttons

        public class LabeledRadioValue<TLabel, TValue>
        {
            public TLabel Label { get; set; }
            public TValue Value { get; set; }

            public LabeledRadioValue(TLabel label, TValue val)
            {
                Label = label;
                Value = val;
            }
        }

        public class LabeledRadioValue<T>: LabeledRadioValue<string, T>
        {           
            public LabeledRadioValue(string label, T value) : base(label, value)
            {
                Label = label;
                Value = value;
            }
        }

        //thanks to Why_is_that for the below, who in turn got his stuff from
        // REFERENCE: https://github.com/erdelf/GodsOfRimworld/blob/master/Source/Ankh/ModControl.cs
        // REFERENCE: https://github.com/erdelf/PrisonerRansom/
        // modified by drummeur to make it use generics and Enumerables
        public static void AddLabeledRadioList<T>(this Listing_Standard listing_Standard, string header, IEnumerable<T> labels, ref T val, float? headerHeight = null)
        { 
            if (header != string.Empty) 
            { 
                Widgets.Label(listing_Standard.GetRect(headerHeight), header); 
            }

            listing_Standard.AddRadioList(GenerateLabeledRadioValues(labels), ref val);
        }

        public static void AddLabeledRadioList<T>(this Listing_Standard listing_Standard, string header, IEnumerable<T> labels, ref T val, Func<T, string> formatter, float? headerHeight = null)
        {
            if (header != string.Empty)
            {
                Widgets.Label(listing_Standard.GetRect(headerHeight), header);
            }

            listing_Standard.AddRadioList(GenerateLabeledRadioValues(labels, formatter), ref val);
        }


        private static IEnumerable<LabeledRadioValue<T>> GenerateLabeledRadioValues<T>(IEnumerable<T> labels)
        {
            foreach (var label in labels)
            {
                yield return new LabeledRadioValue<T>(label.ToString(), label);
            }
        }

        private static IEnumerable<LabeledRadioValue<T>> GenerateLabeledRadioValues<T>(IEnumerable<T> labels, Func<T, string> formatter)
        {
            foreach (var label in labels)
            {
                yield return new LabeledRadioValue<T>(formatter(label), label);
            }
        }

        private static void AddRadioList<T>(this Listing_Standard listing_Standard, IEnumerable<LabeledRadioValue<T>> items, ref T val, float? height = null)
        {
            foreach (LabeledRadioValue<T> item in items)
            {
                Rect lineRect = listing_Standard.GetRect(height);
                if (Widgets.RadioButtonLabeled(lineRect, item.Label, EqualityComparer<T>.Default.Equals(item.Value, val)))
                {
                    val = item.Value;
                }
            }
        }

        // not really sure that this is worth it because we just end up converting TLabels to strings at the end of the line anyway
        public static void AddLabeledRadioList<TLabel, TValue>(this Listing_Standard listing_standard, string header, IEnumerable<KeyValuePair<TLabel, TValue>> kvp, ref TValue val, float? headerHeight = null)
        {
            if (header != string.Empty)
            {
                Widgets.Label(listing_standard.GetRect(headerHeight), header);
            }

            listing_standard.AddRadioList(GenerateLabeledRadioValues(kvp), ref val);
        }

        private static IEnumerable<LabeledRadioValue<TLabel, TValue>> GenerateLabeledRadioValues<TLabel, TValue>(IEnumerable<KeyValuePair<TLabel, TValue>> kvps)
        { 
            foreach (var kvp in kvps)
            {
                yield return new LabeledRadioValue<TLabel, TValue>(kvp.Key, kvp.Value);
            }
        }

        private static void AddRadioList<TLabel, TValue>(this Listing_Standard listing_Standard, IEnumerable<LabeledRadioValue<TLabel, TValue>> items, ref TValue val, float? height = null)
        {
            foreach (var item in items)
            {
                Rect lineRect = listing_Standard.GetRect(height);
                if (Widgets.RadioButtonLabeled(lineRect, item.Label.ToString(), EqualityComparer<TValue>.Default.Equals(item.Value, val)))
                {
                    val = item.Value;
                }
            }
        }

        #endregion
    }
}