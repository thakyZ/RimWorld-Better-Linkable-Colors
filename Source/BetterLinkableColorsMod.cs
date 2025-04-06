using UnityEngine;
using Verse;
namespace Drummeur.BetterLinkableColors;
/// <summary>
/// The entry point of the mod when loading the game.
/// </summary>
public class BetterLinkableColorsMod : Mod
{
    /// <summary>
    /// An instance of the settings to reference.
    /// </summary>
    internal static Settings Settings { get; private set; } = null!;

    /// <summary>
    /// Initializes a new instance of the <see cref="BetterLinkableColorsMod" /> class.
    /// </summary>
    public BetterLinkableColorsMod(ModContentPack content) : base(content)
    {
        Log.Message("Loading Settings");
        Settings = GetSettings<Settings>();
        Log.Message("Loaded Settings");
        //Materials.Reset();
    }

    /// <summary>
    /// Gets the title of the settings window for the mod.
    /// </summary>
    /// <returns>A <see langword="string" /> representing the title for the mod's settings window.</returns>
    public override string SettingsCategory() => Strings.SettingsCategoryTitle;

    /// <summary>
    /// An method that is executed when saving the settings, or when the mod's settings window is closed.
    /// </summary>
    public override void WriteSettings()
    {
        base.WriteSettings();
        Materials.Reset();
    }

    /// <summary>
    /// Renders the contents of the mod's settings <see cref="Window" />.
    /// </summary>
    /// <param name="inRect">A <see cref="Rect" /> defining the bounds of the mod's settings <see cref="Window" />.</param>
    public override void DoSettingsWindowContents(Rect inRect)
    {
        Settings.DoWindowContents(inRect);
    }
}
