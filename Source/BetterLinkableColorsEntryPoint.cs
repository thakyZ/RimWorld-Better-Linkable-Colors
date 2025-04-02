using UnityEngine;
using Verse;

namespace drummeur.linkablecolors;

public class BetterLinkableColorsEntryPoint : Mod
{
    internal LinkableColorsSettings Settings { get; }

    public LinkableColors(ModContentPack content) : base(content)
    {
        this.Settings = GetSettings<LinkableColorsSettings>();
    }

    public override string SettingsCategory() => "BetterLinkableColors.Settings.Title".TranslateSimple();

    public override void DoSettingsWindowContents(Rect inRect)
    {
        this.Settings.DoWindowContents(inRect);
    }
}
