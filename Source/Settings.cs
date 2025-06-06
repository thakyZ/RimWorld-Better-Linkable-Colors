﻿using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using RimWorld;
using Drummeur.BetterLinkableColors.Util;
namespace Drummeur.BetterLinkableColors;
/// <summary>
/// A class to contain the settings for the Better Linkable Colors mod.
/// </summary>
public class Settings : ModSettings
{
#pragma warning disable IDE1006 // Naming Styles
    /// <summary>
    /// A <see cref="IReadOnlyDictionary{ColorLabel,Color}" /> to convert the enum <see cref="ColorLabel" /> into a <see cref="Color" />.
    /// </summary>
    /// <remarks>
    /// Please check if the <see cref="ColorLabel" /> is not <see cref="ColorLabel.CUSTOM" /> before indexing the dictionary.
    /// </remarks>
    public static readonly IReadOnlyDictionary<ColorLabel, Color> Colors = new Dictionary<ColorLabel, Color>()
    {
        [ColorLabel.CYAN] = Color.cyan,
        [ColorLabel.GREY] = Color.grey,
        [ColorLabel.MAGENTA] = Color.magenta,
        [ColorLabel.RED] = Color.red,
        [ColorLabel.YELLOW] = Color.yellow,
        [ColorLabel.BLACK] = Color.black,
        [ColorLabel.WHITE] = Color.white,
        [ColorLabel.GREEN] = Color.green,
        [ColorLabel.BLUE] = Color.blue,
    };

    /// <summary>
    /// A <see cref="bool" /> determining if the solid line shader should be used.
    /// </summary>
    public static bool UseSolidLineShader = true;

    /// <summary>
    /// A <see cref="ColorLabel" /> to use for the link lines that are active.
    /// </summary>
    public static ColorLabel ActiveColorLabel = ColorLabel.GREEN;

    /// <summary>
    /// A <see cref="ColorLabel" /> to use for the link lines that are inactive.
    /// </summary>
    public static ColorLabel InactiveColorLabel = ColorLabel.RED;

    /// <summary>
    /// A <see cref="ColorLabel" /> to use for the link lines that can be connected when attempting to place a blueprint.
    /// </summary>
    public static ColorLabel PotentialColorLabel = ColorLabel.BLUE;

    /// <summary>
    /// A <see cref="ColorLabel" /> to use for the link lines that are being replace with another linkage when attempting to place a blueprint.
    /// </summary>
    public static ColorLabel OverriddenColorLabel = ColorLabel.YELLOW;

    /// <summary>
    /// A <see langword="string" /> representing a _color to use for the link lines that are active.
    /// </summary>
    public static Color ActiveColor = ColorHelper.Defaults.ActiveColor;

    /// <summary>
    /// A <see langword="string" /> representing a _color to use for the link lines that are inactive.
    /// </summary>
    public static Color InactiveColor = ColorHelper.Defaults.InactiveColor;

    /// <summary>
    /// A <see langword="string" /> representing a _color to use for the link lines that can be connected when attempting to place a blueprint.
    /// </summary>
    public static Color PotentialColor = ColorHelper.Defaults.PotentialColor;

    /// <summary>
    /// A <see langword="string" /> representing a _color to use for the link lines that are being replace with another linkage when attempting to place a blueprint.
    /// </summary>
    public static Color OverriddenColor = ColorHelper.Defaults.OverriddenColor;

    /// <summary>
    /// A constant <see langword="int" /> that represents the maximum number of saved palettes.
    /// </summary>
    private const int MAX_PALETTES = 64;

    /// <summary>
    /// A <see cref="List{Color}" /> of saved palette colors.
    /// </summary>
    public static List<Color> SavedPalette = new(MAX_PALETTES);

    /// <summary>
    ///A <see langword="float" /> representing the thickness of the lines.
    /// </summary>
    public static float LineThickness = 0.2f;
#pragma warning restore IDE1006 // Naming Styles

    /// <summary>
    /// A <see langword="string" /> to use as the buffer when setting the thickness of the lines.
    /// </summary>
    private static string? _lineThicknessBuffer;

    /// <summary>
    /// A <see langword="float" /> to set the X offset of the columns.
    /// </summary>
    public static float xOffset = -14;

    /// <summary>
    /// A <see langword="float" /> to set the Y offset of the columns.
    /// </summary>
    public static float yOffset = 0; // 102.75f;

    /// <summary>
    /// A <see langword="float" /> to set the height of the top row.
    /// </summary>
    public static float topRowHeight = 245.0f;

    /// <summary>
    /// A <see langword="float" /> to set the offset for the check box.
    /// </summary>
    public static float checkBoxOffset = -190f;

    /// <summary>
    /// A <see langword="float" /> to set the offset for the solid _color margin.
    /// </summary>
    public static float colorMargin = 0.0f;

#if DEBUG
    /// <summary>
    /// A <see langword="float" /> to set the height of the dev row.
    /// </summary>
    public static float devRowHeight = 30.0f;
#endif

    /// <summary>
    /// Renders the settings window contents.
    /// </summary>
    /// <param name="rect">A <see cref="Rect" /> that determines the bounds of the settings window.</param>
    public void DoWindowContents(Rect rect)
    {
        var options = new Listing_Standard();
        var defaultWidth = rect.width;
#if DEBUG
        var defaultHeight = rect.height;
        rect.SplitHorizontally(defaultHeight - devRowHeight, out Rect optionsRowRect, out Rect devRowRect);
#else
        Rect optionsRowRect = rect;
#endif
        options.Begin(optionsRowRect);
        Color defaultColor = GUI.color;
        var bottomHeight = optionsRowRect.height - topRowHeight;

        var topRow = new Listing_Standard();
        topRow.Begin(options.GetRect(topRowHeight));

        Text.Font = GameFont.Medium;
        Text.Anchor = TextAnchor.MiddleCenter;
        GUI.color = Color.yellow;

        _ = topRow.Label(Strings.SettingsCategoryTitle);
        //_ = topRow.SubLabel(Strings.SettingsRequireRestartLabel, 1f);

        GUI.color = defaultColor;

        Text.Font = GameFont.Small;
        Text.Anchor = TextAnchor.UpperLeft;

        topRow.Gap();

        topRow.ColumnWidth = (defaultWidth / 2) + checkBoxOffset;

        if (VersionControl.CurrentMinor >= 3)
        {
            topRow.TextFieldNumericLabeled(Strings.SettingsLineThicknessLabel, ref LineThickness, ref _lineThicknessBuffer, 0);
        }

        topRow.CheckboxLabeled(Strings.SettingsUseSolidColorsLabel, ref UseSolidLineShader);

        topRow.ColumnWidth = defaultWidth;
        topRow.GapLine();

        Text.Anchor = TextAnchor.MiddleCenter;
        Text.Font = GameFont.Medium;

        _ = topRow.Label(Strings.SettingsCategoryColorCustomizationTitle);
        _ = topRow.SubLabel(Strings.SettingsCategoryColorCustomizationDescription1, 1f);
        _ = topRow.SubLabel(Strings.SettingsCategoryColorCustomizationDescription2, 1f);
        _ = topRow.SubLabel(Strings.SettingsCategoryColorCustomizationDescription3, 1f);

        Text.Font = GameFont.Small;
        Text.Anchor = TextAnchor.UpperLeft;

        topRow.End();

        var bottomRow = new Listing_Standard();
        bottomRow.Begin(options.GetRect(bottomHeight));

        bottomRow.ColumnWidth = (defaultWidth / 4) + xOffset;

        //_ = bottomRow.SubLabel(Strings.SettingsActiveLinkColorLabel, 1f);

        if (bottomRow.DrawButtonSolidColorLabeled(Strings.SettingsOverriddenLinkColorLabel, ActiveColor))
        {
            Find.WindowStack.Add(new Dialog_ColorPicker(ActiveColor, (Color color) => ActiveColor = color));
        }

        //bottomRow.AddLabeledRadioList(Strings.SettingsActiveLinkColorLabel, Enum.GetValues(typeof(ColorLabel)).Cast<ColorLabel>(), ref ActiveColorLabel, el => el.ToString().ToLower());

        //if (ActiveColorLabel is ColorLabel.CUSTOM)
        //{
        //    _ = bottomRow.SubLabel(Strings.SettingsActiveLinkColorSubLabel, 1f);

        //    ActiveColorString = bottomRow.TextEntryLabeled(Strings.SettingsActiveTextEntryLabel, ActiveColorString);

        //    if (!ColorUtility.TryParseHtmlString(ActiveColorString, out Color _))
        //    {
        //        _ = bottomRow.SubLabel(Strings.SettingsInvalidColorSubLabel, 1f);
        //    }
        //}

        //bottomRow.NewColumn();

        //_ = bottomRow.SubLabel(Strings.SettingsInactiveLinkColorLabel, 1f);

        if (bottomRow.DrawButtonSolidColorLabeled(Strings.SettingsInactiveLinkColorLabel, InactiveColor))
        {
            Find.WindowStack.Add(new Dialog_ColorPicker(InactiveColor, (Color color) => InactiveColor = color));
        }

        //bottomRow.AddLabeledRadioList(Strings.SettingsInactiveLinkColorLabel, Enum.GetValues(typeof(ColorLabel)).Cast<ColorLabel>(), ref InactiveColorLabel, el => el.ToString().ToLower());

        //if (InactiveColorLabel is ColorLabel.CUSTOM)
        //{
        //    _ = bottomRow.SubLabel(Strings.SettingsInactiveLinkColorSubLabel, 1f);

        //    InactiveColorString = bottomRow.TextEntryLabeled(Strings.SettingsInactiveTextEntryLabel, InactiveColorString);

        //    if (!ColorUtility.TryParseHtmlString(InactiveColorString, out Color _))
        //    {
        //        _ = bottomRow.SubLabel(Strings.SettingsInvalidColorSubLabel, 1f);
        //    }
        //}

        //bottomRow.NewColumn();

        //_ = bottomRow.SubLabel(Strings.SettingsPotentialLinkColorLabel, 1f);

        if (bottomRow.DrawButtonSolidColorLabeled(Strings.SettingsOverriddenLinkColorLabel, PotentialColor))
        {
            Find.WindowStack.Add(new Dialog_ColorPicker(PotentialColor, (Color color) => PotentialColor = color));
        }

        //bottomRow.AddLabeledRadioList(Strings.SettingsPotentialLinkColorLabel, Enum.GetValues(typeof(ColorLabel)).Cast<ColorLabel>(), ref PotentialColorLabel, el => el.ToString().ToLower());

        //if (PotentialColorLabel is ColorLabel.CUSTOM)
        //{
        //    _ = bottomRow.SubLabel(Strings.SettingsPotentialLinkColorSubLabel, 1f);

        //    PotentialColorString = bottomRow.TextEntryLabeled(Strings.SettingsPotentialTextEntryLabel, PotentialColorString);

        //    if (!ColorUtility.TryParseHtmlString(PotentialColorString, out Color _))
        //    {
        //        _ = bottomRow.SubLabel(Strings.SettingsInvalidColorSubLabel, 1f);
        //    }
        //}

        //bottomRow.NewColumn();

        if (bottomRow.DrawButtonSolidColorLabeled(Strings.SettingsOverriddenLinkColorLabel, OverriddenColor))
        {
            Find.WindowStack.Add(new Dialog_ColorPicker(OverriddenColor, (Color color) => OverriddenColor = color));
        }

        //bottomRow.AddLabeledRadioList(Strings.SettingsOverriddenLinkColorLabel, Enum.GetValues(typeof(ColorLabel)).Cast<ColorLabel>(), ref OverriddenColorLabel, el => el.ToString().ToLower());

        //if (OverriddenColorLabel is ColorLabel.CUSTOM)
        //{
        //    _ = bottomRow.SubLabel(Strings.SettingsOverriddenLinkColorSubLabel, 1f);

        //    SupplantedColorString = bottomRow.TextEntryLabeled(Strings.SettingsOverriddenTextEntryLabel, SupplantedColorString);

        //    if (!ColorUtility.TryParseHtmlString(SupplantedColorString, out Color _))
        //    {
        //        _ = bottomRow.SubLabel(Strings.SettingsInvalidColorSubLabel, 1f);
        //    }
        //}
#if DEBUG
        _ = bottomRow.SubLabel(Strings.SettingsDevButtonLabel, 1f);
#endif
        bottomRow.End();

        options.End();

#if DEBUG
        var devRow = new Listing_Standard();
        devRow.Begin(devRowRect);
        if (Widgets.ButtonText(devRow.GetRect(30.0f).RightPartPixels(60.0f), Strings.SettingsDevButtonLabel, true, true, true, TextAnchor.MiddleCenter))
        {
            Find.WindowStack.Add(new GenericWindow((Window window, Rect _rect) =>
            {
                window.doCloseX = true;
                window.closeOnClickedOutside = true;
                window.draggable = true;
                var devOptions = new Listing_Standard();
                devOptions.Begin(_rect);
                devOptions.SliderLabeled(Strings.SettingsOffsetXSliderLabel, ref xOffset, (float el) => $"{el:F2}", -100.0f, -14.0f, margin: 0.3f);
                devOptions.SliderLabeled(Strings.SettingsOffsetYSliderLabel, ref yOffset, (float el) => $"{el:F2}", 90.0f, 120.0f, margin: 0.3f);
                devOptions.SliderLabeled(Strings.SettingsCheckBoxOffsetSliderLabel, ref checkBoxOffset, (float el) => $"{el:F2}", -500.0f, 10.0f, margin: 0.3f);
                devOptions.SliderLabeled(Strings.SettingsTopRowHeightSliderLabel, ref topRowHeight, (float el) => $"{el:F2}", 0.0f, defaultHeight - devRowHeight, margin: 0.3f);
                devOptions.SliderLabeled(Strings.SettingsColorMarginSliderLabel, ref colorMargin, (float el) => $"{el:F2}", 0.0f, 1.0f, margin: 0.3f);
                devOptions.SliderLabeled(Strings.SettingsDevButtonLabel, ref devRowHeight, (float el) => $"{el:F2}", 0.0f, defaultHeight - optionsRowRect.height, margin: 0.3f);
                devOptions.End();
            }));
        }

        devRow.End();
#endif
    }

    /// <summary>
    /// Exposes data for the game's settings.
    /// </summary>
    public override void ExposeData()
    {
        Scribe_Values.Look(ref UseSolidLineShader, $"{nameof(BetterLinkableColors)}_{nameof(UseSolidLineShader)}", true);

        Scribe_Values.Look(ref LineThickness, $"{nameof(BetterLinkableColors)}_{nameof(LineThickness)}", 0.2f);

        Scribe_Values.Look(ref ActiveColor, $"{nameof(BetterLinkableColors)}_{nameof(ActiveColor)}", ColorHelper.Defaults.ActiveColor);
        Scribe_Values.Look(ref InactiveColor, $"{nameof(BetterLinkableColors)}_{nameof(InactiveColor)}", ColorHelper.Defaults.InactiveColor);
        Scribe_Values.Look(ref PotentialColor, $"{nameof(BetterLinkableColors)}_{nameof(PotentialColor)}", ColorHelper.Defaults.PotentialColor);
        Scribe_Values.Look(ref OverriddenColor, $"{nameof(BetterLinkableColors)}_{nameof(OverriddenColor)}", ColorHelper.Defaults.OverriddenColor);
        Scribe_Collections.Look(ref SavedPalette, $"{nameof(BetterLinkableColors)}_{nameof(SavedPalette)}", ctorArgs: MAX_PALETTES);
    }
}
