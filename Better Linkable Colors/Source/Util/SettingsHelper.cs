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

        public static void AddLabeledRadioList<T>(this Listing_Standard listing_Standard, string header, IEnumerable<T> labels, ref T val, Func<T, string> formatter, float? headerHeight = null)
        {
            if (header != string.Empty)
            {
                Widgets.Label(listing_Standard.GetRect(headerHeight), header);
            }

            listing_Standard.AddRadioList(GenerateLabeledRadioValues(labels, formatter), ref val);
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