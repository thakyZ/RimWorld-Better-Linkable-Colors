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

    public override string SettingsCategory() => "Better Linkable Colors";

    public override void DoSettingsWindowContents(Rect inRect)
    {
        this.Settings.DoWindowContents(inRect);
    }
}