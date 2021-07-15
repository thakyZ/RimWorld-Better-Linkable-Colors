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
        public class LinkableColorsSettings : ModSettings
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

            public static bool UseSolidLineShader = true;

            public static ColorLabel ActiveColorLabel = ColorLabel.GREEN;
            public static ColorLabel InactiveColorLabel = ColorLabel.RED;
            public static ColorLabel PotentialColorLabel = ColorLabel.BLUE;
            public static ColorLabel SupplantedColorLabel = ColorLabel.YELLOW;

            public static float LineThickness = 0.2f;

            private static string LineThicknessBuffer;

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

                options.ColumnWidth = rect.width / 2 + checkboxOffset;

                if (VersionControl.CurrentMinor >= 3)
                {
                    options.TextFieldNumericLabeled("Line Thickness      ", ref LineThickness, ref LineThicknessBuffer, 0);
                }
                else
                {
                    options.ColumnWidth = defaultWidth;
                    options.Label("All changes require a restart to take effect.");
                    options.ColumnWidth = rect.width / 2 + checkboxOffset;
                }

                options.CheckboxLabeled("Use Solid Colors for Line Shader", ref UseSolidLineShader);

                //options.ColumnWidth = defaultWidth / 3;

                //options.SliderLabeledSettable("Line Thickness", ref LineThickness, ref LineThicknessBuffer, el => el.ToString("F3"), 0, 1);


                options.ColumnWidth = defaultWidth;

                options.GapLine();

                options.ColumnWidth = rect.width / 4 + Xoffset;

                options.AddLabeledRadioList("Active Link Color", Enum.GetValues(typeof(ColorLabel)).Cast<ColorLabel>(), ref ActiveColorLabel, el => el.ToString().ToLower());


                options.ColumnWidth = defaultWidth; 
                
                options.GapLine();

                if (VersionControl.CurrentMinor >= 3)
                {
                    options.Label("All changes require a restart to take effect.");
                }

                //options.Gap(100);
                //options.SliderLabeled("OffsetX", ref Xoffset, el => Math.Round(el, 2).ToString(), -100, -14);
                //options.SliderLabeled("OffsetY", ref Yoffset, el => Math.Round(el, 2).ToString(), 90, 120);
                //options.SliderLabeled("cbxOffset", ref checkboxOffset, el => Math.Round(el, 2).ToString(), -500, 10);
                options.ColumnWidth = rect.width / 4 + Xoffset;

                options.NewColumn();
                options.Gap(Yoffset);

                options.AddLabeledRadioList("Inactive Link Color", Enum.GetValues(typeof(ColorLabel)).Cast<ColorLabel>(), ref InactiveColorLabel, el => el.ToString().ToLower());

                //options.ColumnWidth = defaultWidth;
                //options.GapLine();

                //options.ColumnWidth = rect.width / 4 + Xoffset;

                options.NewColumn();
                options.Gap(Yoffset);

                options.AddLabeledRadioList("Potential Link Color", Enum.GetValues(typeof(ColorLabel)).Cast<ColorLabel>(), ref PotentialColorLabel, el => el.ToString().ToLower());
                //options.GapLine();

                options.NewColumn();
                options.Gap(Yoffset);

                options.AddLabeledRadioList("Overridden Link Color", Enum.GetValues(typeof(ColorLabel)).Cast<ColorLabel>(), ref SupplantedColorLabel, el => el.ToString().ToLower());
                //options.GapLine();

                options.ColumnWidth = defaultWidth;

                options.End();

            }

            public override void ExposeData()
            {
                Scribe_Values.Look(ref UseSolidLineShader, "linkablecolors_usesolidlineshader", true);

                Scribe_Values.Look(ref ActiveColorLabel, "linkablecolors_activecolor", ColorLabel.GREEN);
                Scribe_Values.Look(ref InactiveColorLabel, "linkablecolors_inactivecolor", ColorLabel.RED);
                Scribe_Values.Look(ref PotentialColorLabel, "linkablecolors_potentialcolor", ColorLabel.BLUE);
                Scribe_Values.Look(ref SupplantedColorLabel, "linkablecolors_supplantedcolor", ColorLabel.YELLOW);

                Scribe_Values.Look(ref LineThickness, "linkablecolors_linethickness", 0.2f);
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
