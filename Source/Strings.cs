using System;
using Verse;
namespace Drummeur.BetterLinkableColors;
/// <summary>
/// Strings for localizations
/// </summary>
internal static class Strings
{
#pragma warning disable IDE1006 // Naming Styles
    /// <summary>
    /// Gets the <see langword="string" /> for the settings title.
    /// <para description="default">Better Linkable Colors</para>
    /// </summary>
    public static readonly string SettingsCategoryTitle = "BetterLinkableColors.Settings.Title".TranslateSimple();

    /// <summary>
    /// Gets the <see langword="string" /> for the settings colors category title.
    /// <para description="default">Colors</para>
    /// </summary>
    public static readonly string SettingsCategoryColorsTitle = "BetterLinkableColors.Settings.Colors.Title".TranslateSimple();
    /// <summary>
    /// Gets the <see langword="string" /> for the label of the text box to set the line thickness.
    /// <para description="default">Line Thickness</para>
    /// </summary>
    public static readonly string SettingsLineThicknessLabel = $"{"BetterLinkableColors.Settings.LineThickness.Label".TranslateSimple(),-6}";
    /// <summary>
    /// Gets the <see langword="string" /> for the label of the check box to set whether or not to use solid colors..
    /// <para description="default">Use Solid Colors for Line Shader</para>
    /// </summary>
    public static readonly string SettingsUseSolidColorsLabel = "BetterLinkableColors.Settings.UseSolidColors.Label".TranslateSimple();

    /// <summary>
    /// Gets the <see langword="string" /> for the settings _color customization category title.
    /// <para description="default">Color Customization</para>
    /// </summary>
    public static readonly string SettingsCategoryColorCustomizationTitle = "BetterLinkableColors.Settings.ColorCustomization.Title".TranslateSimple();

    /// <summary>
    /// Gets the <see langword="string" /> for the settings _color customization category description.
    /// <para description="default">You can use any _color name recognized by Unity (such as _red, _cyan, _blue, ...)</para>
    /// </summary>
    public static readonly string SettingsCategoryColorCustomizationDescription1 = "BetterLinkableColors.Settings.ColorCustomization.Description1".TranslateSimple();

    /// <summary>
    /// Gets the <see langword="string" /> for the settings _color customization category description.
    /// <para description="default">or a hexadecimal literal starting with '#' (such as #FF0000, #00FFFF, #0000FF, ...),</para>
    /// </summary>
    public static readonly string SettingsCategoryColorCustomizationDescription2 = "BetterLinkableColors.Settings.ColorCustomization.Description2".TranslateSimple();

    /// <summary>
    /// Gets the <see langword="string" /> for the settings _color customization category description.
    /// <para description="default">but the text entry field will always show the _hex literal.</para>
    /// </summary>
    public static readonly string SettingsCategoryColorCustomizationDescription3 = "BetterLinkableColors.Settings.ColorCustomization.Description3".TranslateSimple();

    /// <summary>
    /// Gets the <see langword="string" /> for the label of the active link _color selection.
    /// <para description="default">Active Link Color</para>
    /// </summary>
    public static readonly string SettingsActiveLinkColorLabel = "BetterLinkableColors.Settings.ActiveLinkColor.Label".TranslateSimple();
    /// <summary>
    /// Gets the <see langword="string" /> for the sub-label of the active link _color selection.
    /// <para description="default">Active Link Color</para>
    /// </summary>
    public static readonly string SettingsActiveLinkColorSubLabel = "BetterLinkableColors.Settings.ActiveLinkColor.SubLabel".TranslateSimple();
    /// <summary>
    /// Gets the <see langword="string" /> for the label of the active link textbox.
    /// <para description="default">Active</para>
    /// </summary>
    public static readonly string SettingsActiveTextEntryLabel = $"{"BetterLinkableColors.Settings.ActiveTextEntry.Label".TranslateSimple(),-1}";

    /// <summary>
    /// Gets the <see langword="string" /> for the label of the inactive link _color selection.
    /// <para description="default">Inactive Link Color</para>
    /// </summary>
    public static readonly string SettingsInactiveLinkColorLabel = "BetterLinkableColors.Settings.InactiveLinkColor.Label".TranslateSimple();
    /// <summary>
    /// Gets the <see langword="string" /> for the sub-label of the inactive link _color selection.
    /// <para description="default">Inactive Link Color</para>
    /// </summary>
    public static readonly string SettingsInactiveLinkColorSubLabel = "BetterLinkableColors.Settings.InactiveLinkColor.SubLabel".TranslateSimple();
    /// <summary>
    /// Gets the <see langword="string" /> for the label of the inactive link textbox.
    /// <para description="default">Inactive</para>
    /// </summary>
    public static readonly string SettingsInactiveTextEntryLabel = $"{"BetterLinkableColors.Settings.InactiveTextEntry.Label".TranslateSimple(),-1}";

    /// <summary>
    /// Gets the <see langword="string" /> for the label of the potential link _color selection.
    /// <para description="default">Potential Link Color</para>
    /// </summary>
    public static readonly string SettingsPotentialLinkColorLabel = "BetterLinkableColors.Settings.PotentialLinkColor.Label".TranslateSimple();
    /// <summary>
    /// Gets the <see langword="string" /> for the sub-label of the potential link _color selection.
    /// <para description="default">Potential Link Color</para>
    /// </summary>
    public static readonly string SettingsPotentialLinkColorSubLabel = "BetterLinkableColors.Settings.PotentialLinkColor.SubLabel".TranslateSimple();
    /// <summary>
    /// Gets the <see langword="string" /> for the label of the potential link textbox.
    /// <para description="default">Potential</para>
    /// </summary>
    public static readonly string SettingsPotentialTextEntryLabel = $"{"BetterLinkableColors.Settings.PotentialTextEntry.Label".TranslateSimple(),-1}";

    /// <summary>
    /// Gets the <see langword="string" /> for the label of the overridden link _color selection.
    /// <para description="default">Overridden Link Color</para>
    /// </summary>
    public static readonly string SettingsOverriddenLinkColorLabel = "BetterLinkableColors.Settings.OverriddenLinkColor.Label".TranslateSimple();
    /// <summary>
    /// Gets the <see langword="string" /> for the sub-label of the overridden link _color selection.
    /// <para description="default">Overridden Link Color</para>
    /// </summary>
    public static readonly string SettingsOverriddenLinkColorSubLabel = "BetterLinkableColors.Settings.OverriddenLinkColor.SubLabel".TranslateSimple();
    /// <summary>
    /// Gets the <see langword="string" /> for the label of the overridden link textbox.
    /// <para description="default">Overridden</para>
    /// </summary>
    public static readonly string SettingsOverriddenTextEntryLabel = $"{"BetterLinkableColors.Settings.OverriddenTextEntry.Label".TranslateSimple(),-1}";

    /// <summary>
    /// Gets the <see langword="string" /> for the label that shows when a text box has an invalid _color.
    /// <para description="default">Invalid Color</para>
    /// </summary>
    public static readonly string SettingsInvalidColorSubLabel = $"{"BetterLinkableColors.Settings.InvalidColor.SubLabel".TranslateSimple(),2}";
    /// <summary>
    /// Gets the <see langword="string" /> for the label that shows when a change requires a restart.
    /// <para description="default">All changes require a restart to take effect</para>
    /// </summary>
    [Obsolete("The requiring restart should not be an issue anymore.")]
    public static readonly string SettingsRequireRestartLabel = "BetterLinkableColors.Settings.RequireRestart.Label".TranslateSimple();

    /// <summary>
    /// Gets the <see langword="string" /> for the label the button to apply a setting.
    /// <para description="default">Apply</para>
    /// </summary>
    public static readonly string SettingsApplyButtonLabel = "BetterLinkableColors.Settings.ApplyButton.Label".TranslateSimple();
    /// <summary>
    /// Gets the <see langword="string" /> for the label the button to apply a setting preset.
    /// <para description="default">Save</para>
    /// </summary>
    public static readonly string SettingsSaveButtonLabel = "BetterLinkableColors.Settings.SaveButton.Label".TranslateSimple();
    /// <summary>
    /// Gets the <see langword="string" /> for the label the button to accept and confirm a setting.
    /// <para description="default">Ok</para>
    /// </summary>
    public static readonly string SettingsOkButtonLabel = "BetterLinkableColors.Settings.OkButton.Label".TranslateSimple();
    /// <summary>
    /// Gets the <see langword="string" /> for the label the button to cancel a setting change.
    /// <para description="default">Cancel</para>
    /// </summary>
    public static readonly string SettingsCancelButtonLabel = "BetterLinkableColors.Settings.OkayButton.Label".TranslateSimple();

#if DEBUG
    /// <summary>
    /// Gets the <see langword="string" /> for the label the button to open the developer panel.
    /// <para description="default">Dev</para>
    /// </summary>
    public static readonly string SettingsDevButtonLabel = "BetterLinkableColors.Settings.DevButton.Label".TranslateSimple();
    /// <summary>
    /// Gets the <see langword="string" /> for the label the slider for modifying the X-axis offset for the settings window.
    /// <para description="default">Offset X</para>
    /// </summary>
    public static readonly string SettingsOffsetXSliderLabel = "BetterLinkableColors.Settings.OffsetXSlider.Label".TranslateSimple();
    /// <summary>
    /// Gets the <see langword="string" /> for the label the slider for modifying the Y-axis offset for the settings window.
    /// <para description="default">Offset Y</para>
    /// </summary>
    public static readonly string SettingsOffsetYSliderLabel = "BetterLinkableColors.Settings.OffsetYSlider.Label".TranslateSimple();
    /// <summary>
    /// Gets the <see langword="string" /> for the label the slider for modifying the offset for the check box for the settings window.
    /// <para description="default">CheckBox Offset</para>
    /// </summary>
    public static readonly string SettingsCheckBoxOffsetSliderLabel = "BetterLinkableColors.Settings.CheckBoxOffsetSlider.Label".TranslateSimple();
    /// <summary>
    /// Gets the <see langword="string" /> for the label the slider for modifying the height of the top row for the settings window.
    /// <para description="default">Top Row Height</para>
    /// </summary>
    public static readonly string SettingsTopRowHeightSliderLabel = "BetterLinkableColors.Settings.TopRowHeightSlider.Label".TranslateSimple();
    /// <summary>
    /// Gets the <see langword="string" /> for the label the slider for modifying the solid _color button margin.
    /// <para description="default">Color Button Margin</para>
    /// </summary>
    public static readonly string SettingsColorMarginSliderLabel = "BetterLinkableColors.Settings.ColorMarginSlider.Label".TranslateSimple();
#endif
#pragma warning restore IDE1006 // Naming Styles
}
