using System;
using UnityEngine;
using Verse;
using Verse.Noise;
namespace Drummeur.BetterLinkableColors;
[StaticConstructorOnStartup]
public class Dialog_ColorWheel : Window
{
    private const int _BUTTON_WIDTH = 90;
    private const float _BUTTON_HEIGHT = 30f;
    public Color previousColor = Color.white;

    public Color color = Color.white;
    public Action<Color> OnComplete;
    public Action<Color> OnConfirm;
    public Action<Color> OnSave;
    public int currentState = 0;

    public static int red;
    public static int green;
    public static int blue;

    public static int cyan;
    public static int yellow;
    public static int magenta;
    public static int key;

    public static int hue;
    public static int saturation;
    public static int value;

    public static string hex;

    public static int @decimal;

    public static bool draggingCP;
    public static bool draggingHue;
    public static bool draggingDisplacement;

    public static Texture2D colorChart = new(255, 255);
    public static Texture2D hueChart = new(1, 255);

    public static Color blackist = new(0.06f, 0.06f, 0.06f);
    public static Color greyist = new(0.2f, 0.2f, 0.2f);

    static Dialog_ColorWheel()
    {
        for (var i = 0; i < 255; i++)
        {
            hueChart.SetPixel(0, i, Color.HSVToRGB(Mathf.InverseLerp(0f, 255f, i), 1f, 1f));
        }

        hueChart.Apply(false);

        for (var j = 0; j < 255; j++)
        {
            for (var k = 0; k < 255; k++)
            {
                Color color = Color.clear;
                var c = Color.Lerp(color, Color.white, Mathf.InverseLerp(0f, 255f, j));
                color = Color32.Lerp(Color.black, c, Mathf.InverseLerp(0f, 255f, k));
                colorChart.SetPixel(j, k, color);
            }
        }

        colorChart.Apply(false);
    }

    public Dialog_ColorWheel(Color color, Action<Color> onComplete)
    {
        this.previousColor = color;
        this.color = color;
        this.OnComplete = onComplete;
        doCloseX = true;
        closeOnClickedOutside = true;
        OnConfirm = (_color) => this.previousColor = _color;
        OnSave = Settings.SavedPalette.Add;
    }

    public override Vector2 InitialSize => new(375, 350 + _BUTTON_HEIGHT);

    public override void DoWindowContents(Rect inRect)
    {
        var colorContainerRect = new Rect(inRect)
        {
            height = inRect.width - 25
        };

        DrawColorPicker(colorContainerRect);

        var buttonRect = new Rect(0f, inRect.height - _BUTTON_HEIGHT, _BUTTON_WIDTH, _BUTTON_HEIGHT);
        DoBottomButtons(buttonRect);
    }

    private void DoBottomButtons(Rect rect)
    {
        if (Widgets.ButtonText(rect, Strings.SettingsApplyButtonLabel))
        {
            OnComplete(color);
        }

        rect.x += _BUTTON_WIDTH;

        if (Widgets.ButtonText(rect, Strings.SettingsSaveButtonLabel))
        {
            OnSave(color);
        }

        rect.x += _BUTTON_WIDTH;

        if (Widgets.ButtonText(rect, Strings.SettingsOkButtonLabel))
        {
            OnConfirm(color);
        }

        rect.x += _BUTTON_WIDTH;

        if (Widgets.ButtonText(rect, "CancelButton".Translate()))
        {
            Close(true);
        }
    }

    /// <summary>
    /// Draw ColorPicker and HuePicker
    /// </summary>
    /// <param name="fullRect"></param>
    public void DrawColorPicker(Rect fullRect)
    {
        fullRect.SplitHorizontally((_BUTTON_HEIGHT * 2) + 6, out Rect colorRow, out Rect lowerRow);

        Rect colorBackground = colorRow.ContractedBy(4);
        Widgets.DrawBoxSolid(colorBackground, Color.black);
        colorRow.ContractedBy(4).SplitHorizontally((_BUTTON_HEIGHT * 2) + 6, out Rect previousColorRect, out Rect currentColorRect);
        Widgets.DrawBoxSolid(previousColorRect, this.previousColor);
        Widgets.DrawBoxSolid(currentColorRect, this.color);
        lowerRow.SplitHorizontally(_BUTTON_HEIGHT + 16, out Rect buttonRow, out Rect bottomRowRect);
        var buttonRect = new Rect(0f, buttonRow.height - _BUTTON_HEIGHT, _BUTTON_WIDTH, _BUTTON_HEIGHT);
        if (Widgets.ButtonText(buttonRect, "RGB"))
        {
            this.currentState = 0;
        }

        buttonRect.x += _BUTTON_WIDTH;
        if (Widgets.ButtonText(buttonRect, "HSV"))
        {
            this.currentState = 1;
        }

        buttonRect.x += _BUTTON_WIDTH;
        if (Widgets.ButtonText(buttonRect, "CMYK"))
        {
            this.currentState = 2;
        }
        
        var bottomRow = new Listing_Standard();
        bottomRow.Begin(bottomRowRect);
        bottomRowRect.SplitVertically(_BUTTON_HEIGHT, out Rect firstRowRect, out Rect _RowRect);
        _RowRect.SplitVertically(_BUTTON_HEIGHT, out Rect secondRowRect, out Rect __RowRect);
        __RowRect.SplitVertically(_BUTTON_HEIGHT, out Rect thirdRowRect, out Rect ___RowRect);
        ___RowRect.SplitVertically(_BUTTON_HEIGHT, out Rect fourthRowRect, out Rect bottomHalf);
        if (currentState == 0)
        {
            var redRow = new Listing_Standard();
            redRow.Begin(firstRowRect);
            if (redRow.SliderLabeled("Red", ref red, (int el) => $"{el}", 0, 255))
            {

            }
            redRow.End();
            var greenRow = new Listing_Standard();
            greenRow.Begin(secondRowRect);
            if (greenRow.SliderLabeled("Green", ref green, (int el) => $"{el}", 0, 255)) 
            {
            }
            greenRow.End();
            var blueRow = new Listing_Standard();
            blueRow.Begin(__RowRect);
            if (blueRow.SliderLabeled("Blue", ref blue, (int el) => $"{el}", 0, 255))
            {

            }
            blueRow.End();
        }
        else if (currentState == 1)
        {
            var hueRow = new Listing_Standard();
            hueRow.Begin(firstRowRect);
            if (hueRow.SliderLabeled("Hue", ref hue, (int el) => $"{el}", 0, 100))
            {

            }
            hueRow.End();
            var saturationRow = new Listing_Standard();
            saturationRow.Begin(secondRowRect);
            if (saturationRow.SliderLabeled("Saturation", ref saturation, (int el) => $"{el}", 0, 100))
            {

            }
            saturationRow.End();
            var valueRow = new Listing_Standard();
            valueRow.Begin(thirdRowRect);
            if (valueRow.SliderLabeled("Value", ref value, (int el) => $"{el}", 0, 100))
            {

            }
            valueRow.End();
        }
        else if (currentState == 2)
        {
            var cyanRow = new Listing_Standard();
            cyanRow.Begin(firstRowRect);
            if (cyanRow.SliderLabeled("Cyan", ref cyan, (int el) => $"{el}", 0, 255))
            {

            }
            cyanRow.End();
            var yellowRow = new Listing_Standard();
            yellowRow.Begin(secondRowRect);
            if (yellowRow.SliderLabeled("Yellow", ref yellow, (int el) => $"{el}", 0, 255))
            {

            }
            yellowRow.End();
            var magentaRow = new Listing_Standard();
            magentaRow.Begin(thirdRowRect);
            if (magentaRow.SliderLabeled("Magenta", ref magenta, (int el) => $"{el}", 0, 255))
            {

            }
            magentaRow.End();
            var keyRow = new Listing_Standard();
            keyRow.Begin(fourthRowRect);
            if (keyRow.SliderLabeled("Key", ref key, (int el) => $"{el}", 0, 255))
            {

            }
            keyRow.End();
        }

        var bottomHalfRow = new Listing_Standard();
        bottomHalfRow.Begin(bottomHalf);
        bottomHalfRow.AddLabeledTextField("Hex:", ref hex, "hex value here");
        bottomHalfRow.AddLabeledNumericalTextField("Decimal:", ref @decimal, "decimal value here");
        bottomHalfRow.End();

        bottomRow.End();

        //Rect rect = fullRect.ContractedBy(10f);
        //rect.width = 15f;

        //if (Input.GetMouseButtonDown(0) && Mouse.IsOver(rect) && !draggingHue)
        //{
        //    draggingHue = true;
        //}

        //if (draggingHue && Event.current.isMouse)
        //{
        //    var num = hue;
        //    hue = Mathf.InverseLerp(rect.height, 0f, Event.current.mousePosition.y - rect.y);
        //    if (hue != num)
        //    {
        //        colorSetter(hue, saturation, value);
        //    }
        //}

        //if (Input.GetMouseButtonUp(0))
        //{
        //    draggingHue = false;
        //}

        //Widgets.DrawBoxSolid(rect.ExpandedBy(1f), Color.grey);
        //Widgets.DrawTexturePart(rect, new Rect(0f, 0f, 1f, 1f), hueChart);
        //var rect2 = new Rect(0f, 0f, 16f, 16f)
        //{
        //    center = new Vector2(rect.center.x, (rect.height * (1f - hue)) + rect.y).Rounded()
        //};

        //Widgets.DrawTextureRotated(rect2, Textures.ColorHue, 0f);
        //rect = fullRect.ContractedBy(10f);
        //rect.x = rect.xMax - rect.height;
        //rect.width = rect.height;

        //if (Input.GetMouseButtonDown(0) && Mouse.IsOver(rect) && !draggingCP)
        //{
        //    draggingCP = true;
        //}

        //if (draggingCP)
        //{
        //    saturation = Mathf.InverseLerp(0f, rect.width, Event.current.mousePosition.x - rect.x);
        //    value = Mathf.InverseLerp(rect.width, 0f, Event.current.mousePosition.y - rect.y);
        //    colorSetter(hue, saturation, value);
        //}

        //if (Input.GetMouseButtonUp(0))
        //{
        //    draggingCP = false;
        //}

        //Widgets.DrawBoxSolid(rect.ExpandedBy(1f), Color.grey);
        //Widgets.DrawBoxSolid(rect, Color.white);
        //GUI.color = Color.HSVToRGB(hue, 1f, 1f);
        //Widgets.DrawTextureFitted(rect, colorChart, 1f);
        //GUI.color = Color.white;
        //GUI.BeginClip(rect);
        //rect2.center = new Vector2(rect.width * saturation, rect.width * (1f - value));

        //if (value >= 0.4f && (hue <= 0.5f || saturation <= 0.5f))
        //{
        //    GUI.color = blackist;
        //}

        //Widgets.DrawTextureFitted(rect2, Textures.ColorPicker, 1f);
        //GUI.color = Color.white;
        //GUI.EndClip();

        //return rect;
    }
}
