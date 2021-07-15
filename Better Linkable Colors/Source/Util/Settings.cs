using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using UnityEngine;
using Verse;

namespace drummeur.linkablecolors
{
    class Settings
    {
        public enum ColorLabel
        {
            BLACK,
            BLUE,
            CYAN,
            GREEN,
            GREY,
            MAGENTA,
            RED,
            WHITE,
            YELLOW,
            //CUSTOM
        };

        public static readonly Dictionary<ColorLabel, Color> Colors = new Dictionary<ColorLabel, Color>()
        {
            [ColorLabel.CYAN] = Color.cyan,
            [ColorLabel.GREY] = Color.grey,
            [ColorLabel.MAGENTA] = Color.magenta,
            [ColorLabel.RED] = Color.red,
            [ColorLabel.YELLOW] = Color.yellow,
            [ColorLabel.BLACK] = Color.black,
            [ColorLabel.WHITE] = Color.white,
            [ColorLabel.GREEN] = Color.green,
            [ColorLabel.BLUE] = Color.blue
        };


        public struct LineSettings
        {
            public string Name;
            public ColorLabel LineColor;
            public bool UseSolidLineShader;

            public LineSettings(string Name, ColorLabel LineColor, bool UseSolidLineShader)
            {
                this.Name = Name;
                this.LineColor = LineColor;
                this.UseSolidLineShader = UseSolidLineShader;
            }
        }

        public class LinkableColorsSettings : ModSettings
        {
            public void DrawLineSettingsGui(LineSettings lineSettings, Listing_Standard listingStandard)
            {
                listingStandard.CheckboxLabeled($"Use Solid Colors for {lineSettings.Name} Line Shader", ref lineSettings.UseSolidLineShader);

                listingStandard.AddLabeledRadioList($"{lineSettings.Name} Color", Enum.GetValues(typeof(ColorLabel)).Cast<ColorLabel>(), ref lineSettings.LineColor, el => el.ToString().ToLower());

                listingStandard.GapLine();
            }

            public static void Init()
            {
                ActiveLink = new LineSettings("Active Link", ColorLabel.GREEN, true);
                //InactiveLink = new LineSettings("Inactive Link", ColorLabel.RED, true);
                //PotentialLink = new LineSettings("Potential Link", ColorLabel.BLUE, true);
                //SupplantedLink = new LineSettings("Overridden Link", ColorLabel.YELLOW, true);
            }

            public static LineSettings ActiveLink; //= new LineSettings("Active Link", ColorLabel.GREEN, true);
            //public static LineSettings InactiveLink; //= new LineSettings("Inactive Link", ColorLabel.RED, true);
            //public static LineSettings PotentialLink; //= new LineSettings("Potential Link", ColorLabel.BLUE, true);
            //public static LineSettings SupplantedLink; //= new LineSettings("Overridden Link", ColorLabel.YELLOW, true);

            public static float Xoffset = -14;
            public static float Yoffset = 102.75f;
            public static float checkboxOffset = -190f;

            public void DoWindowContents(Rect rect)
            {

                
                Listing_Standard options = new Listing_Standard();
                Color defaultColor = GUI.color;

                options.Begin(rect);

                Text.Font = GameFont.Medium;
                Text.Anchor = TextAnchor.MiddleCenter;
                GUI.color = Color.yellow;
                options.Label("Better Linkable Colors Settings");
                GUI.color = defaultColor;

                Text.Font = GameFont.Small;
                Text.Anchor = TextAnchor.UpperLeft;

                var defaultWidth = options.ColumnWidth;

                options.Gap();

                options.Label("All changes currently require a restart to take effect.");

                DrawLineSettingsGui(ActiveLink, options);
                //DrawLineSettingsGui(InactiveLink, options);
                //DrawLineSettingsGui(PotentialLink, options);
                //DrawLineSettingsGui(SupplantedLink, options);

                options.End();

            }

            public override void ExposeData()
            {
                Scribe_Values.Look(ref ActiveLink, "linkablecolors_activelink");
                //Scribe_Values.Look(ref InactiveLink, "linkablecolors_inactivelink");
                //Scribe_Values.Look(ref PotentialLink, "linkablecolors_potentiallink");
                //Scribe_Values.Look(ref SupplantedLink, "linkablecolors_supplantedlink");
            }
        }

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
}
