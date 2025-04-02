using UnityEngine;

using Verse;

namespace drummeur.linkablecolors;

partial class Settings
{
    public class LinkableColors : Mod
    {
        public LinkableColors(ModContentPack content) : base(content)
        {
            GetSettings<LinkableColorsSettings>();
        }

        public override string SettingsCategory() => "Better Linkable Colors";

        public override void DoSettingsWindowContents(Rect inRect)
        {
            GetSettings<LinkableColorsSettings>().DoWindowContents(inRect);
        }
    }
}
