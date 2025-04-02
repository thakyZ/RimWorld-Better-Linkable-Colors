using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using drummeur.linkablecolors.Util;
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

            //public static ColorLabel ActiveColorLabel = ColorLabel.GREEN;
            //public static ColorLabel InactiveColorLabel = ColorLabel.RED;
            //public static ColorLabel PotentialColorLabel = ColorLabel.BLUE;
            //public static ColorLabel SupplantedColorLabel = ColorLabel.YELLOW;

            public static string ActiveColorString = ColorHelper.Defaults.ActiveColor.ToHtmlString();
            public static string InactiveColorString = ColorHelper.Defaults.InactiveColor.ToHtmlString();
            public static string PotentialColorString = ColorHelper.Defaults.PotentialColor.ToHtmlString();
            public static string SupplantedColorString = ColorHelper.Defaults.SupplantedColor.ToHtmlString();

            public static float LineThickness = 0.2f;

            private static string LineThicknessBuffer;

            public static float Xoffset = -14;
            public static float Yoffset = 102.75f;
            public static float checkboxOffset = -190f;

            //public void DoWindowContents(Rect rect)
            //{
            //    Listing_Standard options = new Listing_Standard();
            //    Color defaultColor = GUI.color;

            //    options.Begin(rect);

            //    Text.Font = GameFont.Medium;
            //    Text.Anchor = TextAnchor.MiddleCenter;
            //    GUI.color = Color.yellow;
            //    options.Label("Better Linkable Colors Settings");
            //    GUI.color = defaultColor;

            //    Text.Font = GameFont.Small;
            //    Text.Anchor = TextAnchor.UpperLeft;

            //    var defaultWidth = options.ColumnWidth;

            //    options.Gap();

            //    options.ColumnWidth = rect.width / 2 + checkboxOffset;

            //    if (VersionControl.CurrentMinor >= 3)
            //    {
            //        options.TextFieldNumericLabeled("Line Thickness      ", ref LineThickness, ref LineThicknessBuffer, 0);
            //    }
            //    else
            //    {
            //        options.ColumnWidth = defaultWidth;
            //        options.Label("All changes require a restart to take effect.");
            //        options.ColumnWidth = rect.width / 2 + checkboxOffset;
            //    }

            //    options.CheckboxLabeled("Use Solid Colors for Line Shader", ref UseSolidLineShader);

            //    //options.ColumnWidth = defaultWidth / 3;

            //    //options.SliderLabeledSettable("Line Thickness", ref LineThickness, ref LineThicknessBuffer, el => el.ToString("F3"), 0, 1);


            //    options.ColumnWidth = defaultWidth;

            //    options.GapLine();

            //    options.ColumnWidth = rect.width / 4 + Xoffset;

            //    options.AddLabeledRadioList("Active Link Color", Enum.GetValues(typeof(ColorLabel)).Cast<ColorLabel>(), ref ActiveColorLabel, el => el.ToString().ToLower());


            //    options.ColumnWidth = defaultWidth;

            //    options.GapLine();

            //    if (VersionControl.CurrentMinor >= 3)
            //    {
            //        options.Label("All changes require a restart to take effect.");
            //    }

            //    //options.Gap(100);
            //    //options.SliderLabeled("OffsetX", ref Xoffset, el => Math.Round(el, 2).ToString(), -100, -14);
            //    //options.SliderLabeled("OffsetY", ref Yoffset, el => Math.Round(el, 2).ToString(), 90, 120);
            //    //options.SliderLabeled("cbxOffset", ref checkboxOffset, el => Math.Round(el, 2).ToString(), -500, 10);
            //    options.ColumnWidth = rect.width / 4 + Xoffset;

            //    options.NewColumn();
            //    options.Gap(Yoffset);

            //    options.AddLabeledRadioList("Inactive Link Color", Enum.GetValues(typeof(ColorLabel)).Cast<ColorLabel>(), ref InactiveColorLabel, el => el.ToString().ToLower());

            //    //options.ColumnWidth = defaultWidth;
            //    //options.GapLine();

            //    //options.ColumnWidth = rect.width / 4 + Xoffset;

            //    options.NewColumn();
            //    options.Gap(Yoffset);

            //    options.AddLabeledRadioList("Potential Link Color", Enum.GetValues(typeof(ColorLabel)).Cast<ColorLabel>(), ref PotentialColorLabel, el => el.ToString().ToLower());
            //    //options.GapLine();

            //    options.NewColumn();
            //    options.Gap(Yoffset);

            //    options.AddLabeledRadioList("Overridden Link Color", Enum.GetValues(typeof(ColorLabel)).Cast<ColorLabel>(), ref SupplantedColorLabel, el => el.ToString().ToLower());
            //    //options.GapLine();

            //    options.ColumnWidth = defaultWidth;

            //    options.End();
            //}


            public void DoWindowContents(Rect rect)
            {
                Listing_Standard options = new Listing_Standard();
                Color defaultColor = GUI.color;

                options.Begin(rect);

                Text.Font = GameFont.Medium;
                Text.Anchor = TextAnchor.MiddleCenter;
                GUI.color = Color.yellow;

                options.Label("Better Linkable Colors Settings");
                options.SubLabel("All changes require a restart to take effect", 1f);

                GUI.color = defaultColor;

                Text.Font = GameFont.Small;
                Text.Anchor = TextAnchor.UpperLeft;

                //var defaultWidth = options.ColumnWidth;

                options.Gap();

                options.ColumnWidth = rect.width / 2 + checkboxOffset;

                if (VersionControl.CurrentMinor >= 3)
                {
                    options.TextFieldNumericLabeled("Line Thickness      ", ref LineThickness, ref LineThicknessBuffer, 0);
                }

                options.CheckboxLabeled("Use Solid Colors for Line Shader", ref UseSolidLineShader);

                //options.ColumnWidth = defaultWidth;

                options.GapLine();

                options.Label("Color Customization");


                //options.Label("You can use any color name recognized by Unity (such as red, cyan, blue, ...)");
                //options.Label("or a hexadecimal literal starting with '#' (such as #FF0000, #00FFFF, #0000FF, ...),");
                //options.Label("but the text entry field will always show the hex literal.");

                //options.ColumnWidth = rect.width / 4 + Xoffset;

                //options.AddLabeledRadioList("Active Link Color", Enum.GetValues(typeof(ColorLabel)).Cast<ColorLabel>(), ref ActiveColorLabel, el => el.ToString().ToLower());

                //Text.Anchor = TextAnchor.MiddleLeft;

                //options.SubLabel("Active Link Color", 1f);

                ActiveColorString = options.TextEntryLabeled("Active ", ActiveColorString);

                if (!ColorUtility.TryParseHtmlString(ActiveColorString, out Color _))
                {
                    options.SubLabel("  Invalid Color", 1f);
                }

                //options.ColumnWidth = defaultWidth; 
                
                //options.GapLine();

                //if (VersionControl.CurrentMinor >= 3)
                //{
                //    options.Label("All changes require a restart to take effect.");
                //}

                //options.Gap(100);
                //options.SliderLabeled("OffsetX", ref Xoffset, el => Math.Round(el, 2).ToString(), -100, -14);
                //options.SliderLabeled("OffsetY", ref Yoffset, el => Math.Round(el, 2).ToString(), 90, 120);
                //options.SliderLabeled("cbxOffset", ref checkboxOffset, el => Math.Round(el, 2).ToString(), -500, 10);
                //options.ColumnWidth = rect.width / 4 + Xoffset;

                //options.NewColumn();
                //options.Gap(Yoffset);

                //options.AddLabeledRadioList("Inactive Link Color", Enum.GetValues(typeof(ColorLabel)).Cast<ColorLabel>(), ref InactiveColorLabel, el => el.ToString().ToLower());

                //options.SubLabel("Inactive Link Color", 1f);
                InactiveColorString = options.TextEntryLabeled("Inactive ", InactiveColorString);

                if (!ColorUtility.TryParseHtmlString(InactiveColorString, out Color _))
                {
                    options.SubLabel("  Invalid Color", 1f);
                }

                //options.ColumnWidth = defaultWidth;
                //options.GapLine();

                //options.ColumnWidth = rect.width / 4 + Xoffset;

                //options.NewColumn();
                //options.Gap(Yoffset);

                //options.AddLabeledRadioList("Potential Link Color", Enum.GetValues(typeof(ColorLabel)).Cast<ColorLabel>(), ref PotentialColorLabel, el => el.ToString().ToLower());
                //options.GapLine();

                //options.SubLabel("Potential Link Color", 1f);
                PotentialColorString = options.TextEntryLabeled("Potential ", PotentialColorString);

                if (!ColorUtility.TryParseHtmlString(PotentialColorString, out Color _))
                {
                    options.SubLabel("  Invalid Color", 1f);
                }

                //options.NewColumn();
                //options.Gap(Yoffset);

                //options.AddLabeledRadioList("Overridden Link Color", Enum.GetValues(typeof(ColorLabel)).Cast<ColorLabel>(), ref SupplantedColorLabel, el => el.ToString().ToLower());
                //options.GapLine();

                //options.SubLabel("Overridden Link Color", 1f);
                SupplantedColorString = options.TextEntryLabeled("Overridden ", SupplantedColorString);

                if (!ColorUtility.TryParseHtmlString(SupplantedColorString, out Color _))
                {
                    options.SubLabel("  Invalid Color", 1f);
                }

                //options.ColumnWidth = defaultWidth;

                options.End();
            }

            public override void ExposeData()
            {
                Scribe_Values.Look(ref UseSolidLineShader, "linkablecolors_usesolidlineshader", true);

                //Scribe_Values.Look(ref ActiveColorLabel, "linkablecolors_activecolor", ColorLabel.GREEN);
                //Scribe_Values.Look(ref InactiveColorLabel, "linkablecolors_inactivecolor", ColorLabel.RED);
                //Scribe_Values.Look(ref PotentialColorLabel, "linkablecolors_potentialcolor", ColorLabel.BLUE);
                //Scribe_Values.Look(ref SupplantedColorLabel, "linkablecolors_supplantedcolor", ColorLabel.YELLOW);

                Scribe_Values.Look(ref LineThickness, "linkablecolors_linethickness", 0.2f);

                Scribe_Values.Look(ref ActiveColorString, "linkablecolors_activecolorstring", ColorHelper.Defaults.ActiveColor.ToHtmlString());
                Scribe_Values.Look(ref InactiveColorString, "linkablecolors_inactivecolorstring", ColorHelper.Defaults.InactiveColor.ToHtmlString());
                Scribe_Values.Look(ref PotentialColorString, "linkablecolors_potentialcolorstring", ColorHelper.Defaults.PotentialColor.ToHtmlString());
                Scribe_Values.Look(ref SupplantedColorString, "linkablecolors_supplantedcolorstring", ColorHelper.Defaults.SupplantedColor.ToHtmlString());
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
