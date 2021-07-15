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

            // going to use the values as ints when we patch the methods
            public enum LineLayer
            {
                BOTTOM = 0,
                TOP = 1
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

            public static bool UseSolidShaderActive = true;
            public static bool UseSolidShaderInactive = true;
            public static bool UseSolidShaderPotential = true;
            public static bool UseSolidShaderSupplanted = true;

            public static ColorLabel ColorLabelActive = ColorLabel.GREEN;
            public static ColorLabel ColorLabelInactive = ColorLabel.RED;
            public static ColorLabel ColorLabelPotential = ColorLabel.BLUE;
            public static ColorLabel ColorLabelSupplanted = ColorLabel.YELLOW;

            public static LineLayer LineLayerActive = LineLayer.TOP;
            public static LineLayer LineLayerInactive = LineLayer.TOP;
            public static LineLayer LineLayerPotential = LineLayer.TOP;
            public static LineLayer LineLayerSupplanted = LineLayer.TOP;

            public static float Xoffset = -14;
            public static float Yoffset = 79.15f; //102.75f;

#if DEBUG
            public static int XoffsetMin = -100;
            public static int XoffsetMax = 100;

            public static int YoffsetMin = -150;
            public static int YoffsetMax = 150;
#endif

            private static Vector2 scrollVector2;

            public void DrawLineSettingsGui(ref Listing_Standard lst, string Name, ref ColorLabel ColorLabel, ref LineLayer LineLayer, ref bool UseSolidShader)
            {
                lst.CheckboxLabeled($"Use Solid Shader for {Name} Links", ref UseSolidShader);
                lst.AddLabeledRadioList($"{Name} Link Layer", Enum.GetValues(typeof(LineLayer)).Cast<LineLayer>(), ref LineLayer, el => el.ToString().ToLower());
                lst.AddLabeledRadioList($"{Name} Link Color", Enum.GetValues(typeof(ColorLabel)).Cast<ColorLabel>(), ref ColorLabel, el => el.ToString().ToLower());
                lst.GapLine();
            }

            public void DoWindowContents(Rect rect)
            {
                Listing_Standard header = new Listing_Standard();
                Color defaultColor = GUI.color;

                header.Begin(rect);

                Text.Font = GameFont.Medium;
                Text.Anchor = TextAnchor.MiddleCenter;
                GUI.color = Color.yellow;
                header.Label("Better Linkable Colors Settings");
                GUI.color = defaultColor;

                Text.Font = GameFont.Small;
                header.Label("All changes currently require a restart to take effect.");

                //header.Gap();
                header.GapLine();
                header.GapLine();
                header.End();

                Rect optionsRect = new Rect(rect.x, rect.y + 50, rect.width, rect.height-100);

                Listing_Standard options = new Listing_Standard();
                options.Begin(optionsRect);

                Text.Font = GameFont.Small;
                Text.Anchor = TextAnchor.UpperLeft;

                var defaultWidth = options.ColumnWidth;

                //options.Gap();
                //options.GapLine();

                Rect viewRect = new Rect(0, 0, rect.width - 18f, 1400f);
                viewRect.width -= 18f;

                //options.BeginScrollView(rect, ref scrollVector2, ref viewRect);

                options.ColumnWidth = rect.width / 2 + Xoffset;

                DrawLineSettingsGui(ref options, "Active", ref ColorLabelActive, ref LineLayerActive, ref UseSolidShaderActive);
                DrawLineSettingsGui(ref options, "Inactive", ref ColorLabelInactive, ref LineLayerInactive, ref UseSolidShaderInactive);

                //options.NewColumn();
                //options.Gap(Yoffset);

                DrawLineSettingsGui(ref options, "Potential", ref ColorLabelPotential, ref LineLayerPotential, ref UseSolidShaderPotential);
                DrawLineSettingsGui(ref options, "Overridden", ref ColorLabelSupplanted, ref LineLayerSupplanted, ref UseSolidShaderSupplanted);

                //options.ColumnWidth = rect.width / 4 + Xoffset;

                //options.NewColumn();
                //options.Gap(Yoffset);

                //options.AddLabeledRadioList("Inactive Link Color", Enum.GetValues(typeof(ColorLabel)).Cast<ColorLabel>(), ref ColorLabelInactive, el => el.ToString().ToLower());

                ////options.ColumnWidth = defaultWidth;
                ////options.GapLine();

                ////options.ColumnWidth = rect.width / 4 + Xoffset;

                //options.NewColumn();
                //options.Gap(Yoffset);

                //options.AddLabeledRadioList("Potential Link Color", Enum.GetValues(typeof(ColorLabel)).Cast<ColorLabel>(), ref ColorLabelPotential, el => el.ToString().ToLower());
                ////options.GapLine();

                //options.NewColumn();
                //options.Gap(Yoffset);

                //options.AddLabeledRadioList("Overridden Link Color", Enum.GetValues(typeof(ColorLabel)).Cast<ColorLabel>(), ref ColorLabelSupplanted, el => el.ToString().ToLower());
                ////options.GapLine();

                ////options.ColumnWidth = defaultWidth;
                ///
#if DEBUG
                options.ColumnWidth = defaultWidth; 

                options.Gap(100);

                options.SliderLabeled("OffsetX", ref Xoffset, el => Math.Round(el, 2).ToString(), XoffsetMin, XoffsetMax);
                options.SliderLabeled("OffsetY", ref Yoffset, el => Math.Round(el, 2).ToString(), YoffsetMin, YoffsetMax);
                //options.SliderLabeled("cbxOffset", ref checkboxOffset, el => Math.Round(el, 2).ToString(), -500, 10);
                options.ColumnWidth = rect.width / 2 + Xoffset;
#endif

                //options.EndScrollView(ref viewRect);

                options.End();

            }

            public override void ExposeData()
            {

                Scribe_Values.Look(ref UseSolidShaderActive, "linkablecolors_usesolidshaderactive", true);
                Scribe_Values.Look(ref UseSolidShaderInactive, "linkablecolors_usesolidshaderinactive", true);
                Scribe_Values.Look(ref UseSolidShaderPotential, "linkablecolors_usesolidshaderpotential", true);
                Scribe_Values.Look(ref UseSolidShaderSupplanted, "linkablecolors_usesolidshadersupplanted", true);

                Scribe_Values.Look(ref ColorLabelActive, "linkablecolors_activecolor", ColorLabel.GREEN);
                Scribe_Values.Look(ref ColorLabelInactive, "linkablecolors_inactivecolor", ColorLabel.RED);
                Scribe_Values.Look(ref ColorLabelPotential, "linkablecolors_potentialcolor", ColorLabel.BLUE);
                Scribe_Values.Look(ref ColorLabelSupplanted, "linkablecolors_supplantedcolor", ColorLabel.YELLOW);

                Scribe_Values.Look(ref LineLayerActive, "linkablecolors_activelinelayer", LineLayer.TOP);
                Scribe_Values.Look(ref LineLayerInactive, "linkablecolors_inactivelinelayer", LineLayer.TOP);
                Scribe_Values.Look(ref LineLayerPotential, "linkablecolors_potentiallinelayer", LineLayer.TOP);
                Scribe_Values.Look(ref LineLayerSupplanted, "linkablecolors_supplantedlinelayer", LineLayer.TOP);
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
