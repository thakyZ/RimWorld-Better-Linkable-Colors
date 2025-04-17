using System;
using System.Reflection;

using HarmonyLib;

using UnityEngine;

using Verse;

namespace Drummeur.BetterLinkableColors.Util;
internal static class RectExtensions
{
#if RIMWORLD_13_OR_LESS
#region Field References

#pragma warning disable IDE1006 // Naming Styles
    /// <summary>
    /// A reference to the <see cref="Listing_Standard.curY" /> field.
    /// </summary>
    private static readonly FieldInfo Listing_Standard_curY_Field = AccessTools.Field(typeof(Listing_Standard), "curY");

    /// <summary>
    /// A reference to the <see cref="Listing_Standard.curX" /> field.
    /// </summary>
    private static readonly FieldInfo Listing_Standard_curX_Field = AccessTools.Field(typeof(Listing_Standard), "curX");

    /// <summary>
    /// A reference to the <see cref="Listing_Standard.listingRect" /> field.
    /// </summary>
    private static readonly FieldInfo Listing_Standard_listingRect_Field = AccessTools.Field(typeof(Listing_Standard), "listingRect");
#pragma warning restore IDE1006 // Naming Styles

#endregion Field References

    /// <summary>
    /// A static constructor to set up this extension class for accessing private/protected fields/methods.
    /// </summary>
    static RectExtensions()
    {
        if (Listing_Standard_curY_Field is null)
        {
            throw new MissingFieldException($"Unable to find field with name {nameof(Listing_Standard)}.curY");
        }

        if (Listing_Standard_curX_Field is null)
        {
            throw new MissingFieldException($"Unable to find field with name {nameof(Listing_Standard)}.curX");
        }

        if (Listing_Standard_listingRect_Field is null)
        {
            throw new MissingFieldException($"Unable to find field with name {nameof(Listing_Standard)}.listingRect");
        }
    }

#region Wrapper Methods

    /// <summary>
    /// A wrapper to get the value of the protected field <see cref="Listing_Standard.curY" />.
    /// </summary>
    /// <param name="listing_standard">The instance of <see cref="Listing_Standard" /> to get the field value of.</param>
    /// <returns>The value of the protected field <see cref="Listing_Standard.curY" /> if successful otherwise <c>default(float)</c>.</returns>
    private static float GetListing_Standard_curY_Value(this Listing_Standard listing_standard)
      => Listing_Standard_curY_Field?.GetValue(listing_standard) is float value ? value : default;

    /// <summary>
    /// A wrapper to set the value of the protected field <see cref="Listing_Standard.curY" />.
    /// </summary>
    /// <param name="listing_standard">The instance of <see cref="Listing_Standard" /> to set the field value of.</param>
    private static void SetListing_Standard_curY_Value(this Listing_Standard listing_standard, float newValue)
      => Listing_Standard_curY_Field?.SetValue(listing_standard, newValue);

    /// <summary>
    /// A wrapper to set the value of the protected field <see cref="Listing_Standard.curY" /> with the addition of the current value and a new value.
    /// </summary>
    /// <param name="listing_standard">The instance of <see cref="Listing_Standard" /> to set the field value of.</param>
    private static void AddToListing_Standard_curY_Value(this Listing_Standard listing_standard, float value)
      => listing_standard.SetListing_Standard_curY_Value(listing_standard.GetListing_Standard_curY_Value() + value);

    /// <summary>
    /// A wrapper to get the value of the protected field <see cref="Listing_Standard.curX" />.
    /// </summary>
    /// <param name="listing_standard">The instance of <see cref="Listing_Standard" /> to get the field value of.</param>
    /// <returns>The value of the protected field <see cref="Listing_Standard.curX" /> if successful otherwise <c>default(float)</c>.</returns>
    private static float GetListing_Standard_curX_Value(this Listing_Standard listing_standard)
      => Listing_Standard_curX_Field?.GetValue(listing_standard) is float value ? value : default;

    /// <summary>
    /// A wrapper to get the value of the protected field <see cref="Listing_Standard.listingRect" />.
    /// </summary>
    /// <param name="listing_standard">The instance of <see cref="Listing_Standard" /> to get the field value of.</param>
    /// <returns>The value of the protected field <see cref="Listing_Standard.listingRect" /> if successful otherwise <c>default(Rect)</c>.</returns>
    private static Rect GetListing_Standard_listingRect_Value(this Listing_Standard listing_standard)
      => Listing_Standard_listingRect_Field?.GetValue(listing_standard) is Rect value ? value : default;

#endregion Wrapper Methods

#if RIMWORLD_12_OR_LESS
    /// <summary>
    /// Derived from 1.5 RimWorld <see cref="Verse.GenUI" /> class.
    /// </summary>
    public static void SplitHorizontally(this Rect rect, float topHeight, out Rect top, out Rect bottom)
    {
        _ = rect.SplitHorizontallyWithMargin(out top, out bottom, out var _, 0f, new float?(topHeight), null);
    }

    /// <summary>
    /// Derived from 1.5 RimWorld <see cref="Verse.GenUI" /> class.
    /// </summary>
    public static void SplitVertically(this Rect rect, float leftWidth, out Rect left, out Rect right)
    {
        _ = rect.SplitVerticallyWithMargin(out left, out right, out var _, 0f, new float?(leftWidth), null);
    }

    /// <summary>
    /// Derived from 1.5 RimWorld <see cref="Verse.GenUI" /> class.
    /// </summary>
    public static bool SplitHorizontallyWithMargin(this Rect rect, out Rect top, out Rect bottom, out float overflow, float compressibleMargin = 0f, float? topHeight = null, float? bottomHeight = null)
    {
        if ((topHeight.HasValue && bottomHeight.HasValue) || (!topHeight.HasValue && !bottomHeight.HasValue))
        {
            throw new ArgumentException("Exactly one null height and one non-null height must be provided.");
        }

        overflow = Mathf.Max(0f, (topHeight ?? bottomHeight ?? default) - rect.height);
        var topMargin = Mathf.Clamp(topHeight ?? (rect.height - (bottomHeight ?? default) - compressibleMargin), 0f, rect.height);
        var bottomMargin = Mathf.Clamp(bottomHeight ?? (rect.height - (topHeight ?? default) - compressibleMargin), 0f, rect.height);
        top = new Rect(rect.x, rect.y, rect.width, topMargin);
        bottom = new Rect(rect.x, rect.yMax - bottomMargin, rect.width, bottomMargin);
        return overflow == default;
    }

    /// <summary>
    /// Derived from 1.5 RimWorld <see cref="Verse.GenUI" /> class.
    /// </summary>
    public static void SplitVerticallyWithMargin(this Rect rect, out Rect left, out Rect right, float margin)
    {
        var num = rect.width / 2f;
        left = new Rect(rect.x, rect.y, num - (margin / 2f), rect.height);
        right = new Rect(left.xMax + margin, rect.y, num - (margin / 2f), rect.height);
    }

    /// <summary>
    /// Derived from 1.5 RimWorld <see cref="Verse.GenUI" /> class.
    /// </summary>
    public static bool SplitVerticallyWithMargin(this Rect rect, out Rect left, out Rect right, out float overflow, float compressibleMargin = 0f, float? leftWidth = null, float? rightWidth = null)
    {
        if ((leftWidth.HasValue && rightWidth.HasValue) || (!leftWidth.HasValue && !rightWidth.HasValue))
        {
            throw new ArgumentException("Exactly one null width and one non-null width must be provided.");
        }

        overflow = Mathf.Max(0f, (leftWidth ?? rightWidth ?? default) - rect.width);
        var leftMarin = Mathf.Clamp(leftWidth ?? (rect.width - (rightWidth ?? default) - compressibleMargin), 0f, rect.width);
        var rightMargin = Mathf.Clamp(rightWidth ?? (rect.width - (leftWidth ?? default) - compressibleMargin), 0f, rect.width);
        left = new Rect(rect.x, rect.y, leftMarin, rect.height);
        right = new Rect(rect.xMax - rightMargin, rect.y, rightMargin, rect.height);
        return overflow == default;
    }
#endif

    /// <summary>
    /// Derived from 1.5 RimWorld <see cref="Verse.Listing_Standard" /> class.
    /// </summary>
    private static void NewColumnIfNeeded(this Listing_Standard listing_standard, float neededHeight)
    {
        if (listing_standard.maxOneColumn)
        {
            return;
        }

        if (listing_standard.GetListing_Standard_curY_Value() + neededHeight > listing_standard.GetListing_Standard_listingRect_Value().height)
        {
            listing_standard.NewColumn();
        }
    }

    /// <summary>
    /// Derived from 1.5 RimWorld <see cref="Verse.Listing_Standard" /> class.
    /// </summary>
    public static Rect GetRect(this Listing_Standard listing_standard, float height, float widthPct = 1f)
    {
        listing_standard.NewColumnIfNeeded(height);
        var rect = new Rect(listing_standard.GetListing_Standard_curX_Value(), listing_standard.GetListing_Standard_curY_Value(), listing_standard.ColumnWidth * widthPct, height);
        listing_standard.AddToListing_Standard_curY_Value(height);
        return rect;
    }

    /// <summary>
    /// Derived from 1.5 RimWorld <see cref="Verse.Listing_Standard" /> class.
    /// </summary>
    public static Rect SubLabel(this Listing_Standard listing_standard, string label, float widthPct)
    {
        Rect rect = listing_standard.GetRect(Text.CalcHeight(label, listing_standard.ColumnWidth * widthPct), widthPct);
        const float offset = 20f;
        rect.x += offset;
        rect.width -= offset;
        Text.Font = GameFont.Tiny;
        GUI.color = Color.gray;
        Widgets.Label(rect, label);
        GUI.color = Color.white;
        Text.Font = GameFont.Small;
        listing_standard.Gap(listing_standard.verticalSpacing);
        return rect;
    }
#endif
}
