using System.Collections.Generic;
using drummeur.linkablecolors.Util;
using RimWorld;
using UnityEngine;
using Verse;
using RimWorld;
using Drummeur.BetterLinkableColors.Util;
namespace Drummeur.BetterLinkableColors;
/// <summary>
/// A class to contain the settings for the Better Linkable Colors mod.
/// </summary>
public class LinkableColorsSettings : ModSettings
{
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
    public static ColorLabel SupplantedColorLabel = ColorLabel.YELLOW;

    /// <summary>
    /// A <see langword="string" /> representing a color to use for the link lines that are active.
    /// </summary>
    public static string ActiveColorString = ColorHelper.Defaults.ActiveColor.ToHtmlString();

    /// <summary>
    /// A <see langword="string" /> representing a color to use for the link lines that are inactive.
    /// </summary>
    public static string InactiveColorString = ColorHelper.Defaults.InactiveColor.ToHtmlString();

    /// <summary>
    /// A <see langword="string" /> representing a color to use for the link lines that can be connected when attempting to place a blueprint.
    /// </summary>
    public static string PotentialColorString = ColorHelper.Defaults.PotentialColor.ToHtmlString();

    /// <summary>
    /// A <see langword="string" /> representing a color to use for the link lines that are being replace with another linkage when attempting to place a blueprint.
    /// </summary>
    public static string SupplantedColorString = ColorHelper.Defaults.SupplantedColor.ToHtmlString();

    /// <summary>
    ///A <see langword="float" /> representing the thickness of the lines.
    /// </summary>
    public static float LineThickness = 0.2f;

    /// <summary>
    /// A <see langword="string" /> to use as the buffer when setting the thickness of the lines.
    /// </summary>
    private static string? _LineThicknessBuffer;

    /// <summary>
    /// A <see langword="float" /> to set the X offset of the line.
    /// </summary>
    public static float XOffset = -14;

    /// <summary>
    /// A <see langword="float" /> to set the Y offset of the line.
    /// </summary>
    public static float YOffset = 102.75f;

    /// <summary>
    /// A <see langword="float" /> to set the offset for the check box.
    /// </summary>
    public static float CheckBoxOffset = -190f;

    ///// <summary>
    ///// Renders the settings window contents.
    ///// </summary>
    ///// <param name="rect">A <see cref="Rect" /> that determines the bounds of the settings window.</param>
    //public void DoWindowContents(Rect rect)
    //{
    //    Listing_Standard options = new Listing_Standard();
    //    Color defaultColor = GUI.color;
    //
    //    options.Begin(rect);
    //
    //    Text.Font = GameFont.Medium;
    //    Text.Anchor = TextAnchor.MiddleCenter;
    //    GUI.color = Color.yellow;
    //    _ = options.Label("Better Linkable Colors Settings");
    //    GUI.color = defaultColor;
    //
    //    Text.Font = GameFont.Small;
    //    Text.Anchor = TextAnchor.UpperLeft;
    //
    //    var defaultWidth = options.ColumnWidth;
    //
    //    options.Gap();
    //
    //    options.ColumnWidth = (rect.width / 2) + CheckBoxOffset;
    //
    //    if (VersionControl.CurrentMinor >= 3)
    //    {
    //        options.TextFieldNumericLabeled("Line Thickness      ", ref LineThickness, ref _LineThicknessBuffer, 0);
    //    }
    //    else
    //    {
    //        options.ColumnWidth = defaultWidth;
    //        _ = options.Label("All changes require a restart to take effect.");
    //        options.ColumnWidth = (rect.width / 2) + CheckBoxOffset;
    //    }
    //
    //    options.CheckboxLabeled("Use Solid Colors for Line Shader", ref UseSolidLineShader);
    //
    //    //options.ColumnWidth = defaultWidth / 3;
    //
    //    //options.SliderLabeledSettable("Line Thickness", ref LineThickness, ref _LineThicknessBuffer, el => el.ToString("F3"), 0, 1);
    //
    //    options.ColumnWidth = defaultWidth;
    //
    //    options.GapLine();
    //
    //    options.ColumnWidth = (rect.width / 4) + XOffset;
    //
    //    options.AddLabeledRadioList("Active Link Color", Enum.GetValues(typeof(ColorLabel)).Cast<ColorLabel>(), ref ActiveColorLabel, el => el.ToString().ToLower());
    //
    //    options.ColumnWidth = defaultWidth;
    //
    //    options.GapLine();
    //
    //    if (VersionControl.CurrentMinor >= 3)
    //    {
    //        options.Label("All changes require a restart to take effect.");
    //    }
    //
    //    //options.Gap(100);
    //    //options.SliderLabeled("OffsetX", ref XOffset, el => Math.Round(el, 2).ToString(), -100, -14);
    //    //options.SliderLabeled("OffsetY", ref YOffset, el => Math.Round(el, 2).ToString(), 90, 120);
    //    //options.SliderLabeled("cbxOffset", ref CheckBoxOffset, el => Math.Round(el, 2).ToString(), -500, 10);
    //    options.ColumnWidth = (rect.width / 4) + XOffset;
    //
    //    options.NewColumn();
    //    options.Gap(YOffset);
    //
    //    options.AddLabeledRadioList("Inactive Link Color", Enum.GetValues(typeof(ColorLabel)).Cast<ColorLabel>(), ref InactiveColorLabel, el => el.ToString().ToLower());
    //
    //    //options.ColumnWidth = defaultWidth;
    //    //options.GapLine();
    //
    //    //options.ColumnWidth = (rect.width / 4) + XOffset;
    //
    //    options.NewColumn();
    //    options.Gap(YOffset);
    //
    //    options.AddLabeledRadioList("Potential Link Color", Enum.GetValues(typeof(ColorLabel)).Cast<ColorLabel>(), ref PotentialColorLabel, el => el.ToString().ToLower());
    //    //options.GapLine();
    //
    //    options.NewColumn();
    //    options.Gap(YOffset);
    //
    //    options.AddLabeledRadioList("Overridden Link Color", Enum.GetValues(typeof(ColorLabel)).Cast<ColorLabel>(), ref SupplantedColorLabel, el => el.ToString().ToLower());
    //    //options.GapLine();
    //
    //    options.ColumnWidth = defaultWidth;
    //
    //    options.End();
    //}

    /// <summary>
    /// Renders the settings window contents.
    /// </summary>
    /// <param name="rect">A <see cref="Rect" /> that determines the bounds of the settings window.</param>
    public void DoWindowContents(Rect rect)
    {
        var options = new Listing_Standard();
        Color defaultColor = GUI.color;

        options.Begin(rect);

        Text.Font = GameFont.Medium;
        Text.Anchor = TextAnchor.MiddleCenter;
        GUI.color = Color.yellow;

        _ = options.Label("Better Linkable Colors Settings");
#if RIMWORLD_14_OR_GREATER
        _ = options.SubLabel("All changes require a restart to take effect", 1f);
#else
        _ = options.Label("All changes require a restart to take effect", 1f);
#endif
        GUI.color = defaultColor;

        Text.Font = GameFont.Small;
        Text.Anchor = TextAnchor.UpperLeft;

        //var defaultWidth = options.ColumnWidth;

        options.Gap();

        options.ColumnWidth = (rect.width / 2) + CheckBoxOffset;

        if (VersionControl.CurrentMinor >= 3)
        {
            options.TextFieldNumericLabeled("Line Thickness      ", ref LineThickness, ref _LineThicknessBuffer, 0);
        }

        options.CheckboxLabeled("Use Solid Colors for Line Shader", ref UseSolidLineShader);

        //options.ColumnWidth = defaultWidth;

        options.GapLine();

        _ = options.Label("Color Customization");

        //_ = options.Label("You can use any color name recognized by Unity (such as red, cyan, blue, ...)");
        //_ = options.Label("or a hexadecimal literal starting with '#' (such as #FF0000, #00FFFF, #0000FF, ...),");
        //_ = options.Label("but the text entry field will always show the hex literal.");

        //options.ColumnWidth = rect.width / 4 + XOffset;

        //options.AddLabeledRadioList("Active Link Color", Enum.GetValues(typeof(ColorLabel)).Cast<ColorLabel>(), ref ActiveColorLabel, el => el.ToString().ToLower());

        //Text.Anchor = TextAnchor.MiddleLeft;

        //_ = options.SubLabel("Active Link Color", 1f);

        ActiveColorString = options.TextEntryLabeled("Active ", ActiveColorString);

        if (!ColorUtility.TryParseHtmlString(ActiveColorString, out Color _))
        {
#if RIMWORLD_14_OR_GREATER
            _ = options.SubLabel("  Invalid Color", 1f);
#else
            _ = options.Label("  Invalid Color", 1f);
#endif
        }

        //options.ColumnWidth = defaultWidth; 

        //options.GapLine();

        //if (VersionControl.CurrentMinor >= 3)
        //{
        //    _ = options.Label("All changes require a restart to take effect.");
        //}

        //options.Gap(100);
        //options.SliderLabeled("OffsetX", ref XOffset, el => Math.Round(el, 2).ToString(), -100, -14);
        //options.SliderLabeled("OffsetY", ref YOffset, el => Math.Round(el, 2).ToString(), 90, 120);
        //options.SliderLabeled("cbxOffset", ref CheckBoxOffset, el => Math.Round(el, 2).ToString(), -500, 10);
        //options.ColumnWidth = rect.width / 4 + XOffset;

        //options.NewColumn();
        //options.Gap(YOffset);

        //options.AddLabeledRadioList("Inactive Link Color", Enum.GetValues(typeof(ColorLabel)).Cast<ColorLabel>(), ref InactiveColorLabel, el => el.ToString().ToLower());

        //_ = options.SubLabel("Inactive Link Color", 1f);
        InactiveColorString = options.TextEntryLabeled("Inactive ", InactiveColorString);

        if (!ColorUtility.TryParseHtmlString(InactiveColorString, out Color _))
        {
#if RIMWORLD_14_OR_GREATER
            _ = options.SubLabel("  Invalid Color", 1f);
#else
            _ = options.Label("  Invalid Color", 1f);
#endif
        }

        //options.ColumnWidth = defaultWidth;
        //options.GapLine();

        //options.ColumnWidth = rect.width / 4 + XOffset;

        //options.NewColumn();
        //options.Gap(YOffset);

        //options.AddLabeledRadioList("Potential Link Color", Enum.GetValues(typeof(ColorLabel)).Cast<ColorLabel>(), ref PotentialColorLabel, el => el.ToString().ToLower());
        //options.GapLine();

        //_ = options.SubLabel("Potential Link Color", 1f);
        PotentialColorString = options.TextEntryLabeled("Potential ", PotentialColorString);

        if (!ColorUtility.TryParseHtmlString(PotentialColorString, out Color _))
        {
#if RIMWORLD_14_OR_GREATER
            _ = options.SubLabel("  Invalid Color", 1f);
#else
            _ = options.Label("  Invalid Color", 1f, );
#endif
        }

        //options.NewColumn();
        //options.Gap(YOffset);

        //options.AddLabeledRadioList("Overridden Link Color", Enum.GetValues(typeof(ColorLabel)).Cast<ColorLabel>(), ref SupplantedColorLabel, el => el.ToString().ToLower());
        //options.GapLine();

        //_ = options.SubLabel("Overridden Link Color", 1f);
        SupplantedColorString = options.TextEntryLabeled("Overridden ", SupplantedColorString);

        if (!ColorUtility.TryParseHtmlString(SupplantedColorString, out Color _))
        {
#if RIMWORLD_14_OR_GREATER
            _ = options.SubLabel("  Invalid Color", 1f);
#else
            _ = options.Label("  Invalid Color", 1f);
#endif
        }

        //options.ColumnWidth = defaultWidth;

        options.End();
    }

    /// <summary>
    /// Exposes data for the game's settings.
    /// </summary>
    public override void ExposeData()
    {
        Scribe_Values.Look(ref UseSolidLineShader, $"{nameof(linkablecolors)}_{nameof(UseSolidLineShader).ToLower()}", true);

        //Scribe_Values.Look(ref ActiveColorLabel, $"{nameof(linkablecolors)}_{nameof(ActiveColorLabel).Replace("Label", "").ToLower()}", ColorLabel.GREEN);
        //Scribe_Values.Look(ref InactiveColorLabel, $"{nameof(linkablecolors)}_{nameof(InactiveColorLabel).Replace("Label", "").ToLower()}", ColorLabel.RED);
        //Scribe_Values.Look(ref PotentialColorLabel, $"{nameof(linkablecolors)}_{nameof(PotentialColorLabel).Replace("Label", "").ToLower()}", ColorLabel.BLUE);
        //Scribe_Values.Look(ref SupplantedColorLabel, $"{nameof(linkablecolors)}_{nameof(SupplantedColorLabel).Replace("Label", "").ToLower()}", ColorLabel.YELLOW);

        Scribe_Values.Look(ref LineThickness, $"{nameof(linkablecolors)}_{nameof(LineThickness).ToLower()}", 0.2f);

        Scribe_Values.Look(ref ActiveColorString, $"{nameof(linkablecolors)}_{nameof(ActiveColorString).ToLower()}", ColorHelper.Defaults.ActiveColor.ToHtmlString());
        Scribe_Values.Look(ref InactiveColorString, $"{nameof(linkablecolors)}_{nameof(InactiveColorString).ToLower()}", ColorHelper.Defaults.InactiveColor.ToHtmlString());
        Scribe_Values.Look(ref PotentialColorString, $"{nameof(linkablecolors)}_{nameof(PotentialColorString).ToLower()}", ColorHelper.Defaults.PotentialColor.ToHtmlString());
        Scribe_Values.Look(ref SupplantedColorString, $"{nameof(linkablecolors)}_{nameof(SupplantedColorString).ToLower()}", ColorHelper.Defaults.SupplantedColor.ToHtmlString());
    }
}
