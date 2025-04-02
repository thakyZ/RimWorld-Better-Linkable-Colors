using UnityEngine;
using Verse;

namespace drummeur.linkablecolors;

/// <summary>
/// The entry point of the mod when loading the game.
/// </summary>
public class BetterLinkableColorsEntryPoint : Mod
{
    /// <summary>
    /// An instance of the settings to reference.
    /// </summary>
    internal LinkableColorsSettings Settings { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="BetterLinkableColorsEntryPoint" /> class.
    /// </summary>
    public BetterLinkableColorsEntryPoint(ModContentPack content) : base(content)
    {
        this.Settings = GetSettings<LinkableColorsSettings>();
    }

    public override string SettingsCategory() => "BetterLinkableColors.Settings.Title".TranslateSimple();

    public override void DoSettingsWindowContents(Rect inRect)
    {
        this.Settings.DoWindowContents(inRect);
    }
}
