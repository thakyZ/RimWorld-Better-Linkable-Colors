using System;

using Drummeur.BetterLinkableColors.Util;

using UnityEngine;

using Verse;
namespace Drummeur.BetterLinkableColors;
/// <summary>
/// Creates a <see cref="Window" /> for the color picker dialog.
/// </summary>
[StaticConstructorOnStartup]
public class Dialog_ColorPicker : Window
{
    /// <summary>
    /// An enum to determine the current state of the sliders in the <see cref="Dialog_ColorPicker" /> <see cref="Window" />.
    /// </summary>
    private enum ColorPickerState
    {
        /// <summary>
        /// The current state of the sliders is set to RGB.
        /// </summary>
        RGB = 0,
        /// <summary>
        /// The current state of the sliders is set to HSV.
        /// </summary>
        HSV = 1,
        /// <summary>
        /// The current state of the sliders is set to CMYK.
        /// </summary>
        CMYK = 2,
    }

    /// <summary>
    /// A constant <see langword="int" /> value that represents the default button width.
    /// </summary>
    private const int _BUTTON_WIDTH = 90;

    /// <summary>
    /// A constant <see langword="int" /> value that represents the default button height.
    /// </summary>
    private const float _BUTTON_HEIGHT = 30f;

    /// <summary>
    /// A constant <see langword="float" /> value that represents the default slider margin.
    /// </summary>
    private const float _SLIDER_MARGIN = 0.2f;

    /// <summary>
    /// An instance of a <see cref="Color" /> that represents the previously chosen color.
    /// </summary>
    private Color _previousColor = Color.white;

    /// <summary>
    /// An instance of a <see cref="Color" /> that represents the currently chosen color.
    /// </summary>
    private Color _color = Color.white;

    /// <summary>
    /// An <see cref="Action{Color}" /> that represents a method to execute when the user completes their color selection.
    /// </summary>
    private readonly Action<Color> _onComplete;

    /// <summary>
    /// An <see cref="Action{Color}" /> that represents a method to execute when the user confirms their color selection.
    /// </summary>
    private readonly Action<Color> _onConfirm;

    /// <summary>
    /// An <see cref="Action{Color}" /> that represents a method to execute when the user saves their color selection to a palette.
    /// </summary>
    private readonly Action<Color> _onSave;

    /// <summary>
    /// A <see cref="ColorPickerState" /> that represents the current state that the Dialog is at.
    /// </summary>
    private ColorPickerState _currentState = ColorPickerState.RGB;

    /// <summary>
    /// A <see langword="float" /> value representing a slider value for a segment of color.s
    /// </summary>
    private float _red, _green, _blue,
        _hue, _saturation, _value,
        _cyan, _magenta, _yellow, _key,
        _alpha;

    /// <summary>
    /// A <see langword="string" /> value representing the hex value of the currently selected color.
    /// </summary>
    private string _hex = "FFFFFF";

    /// <summary>
    /// A <see langword="bool" /> value representing if a slider is currently being dragged.
    /// </summary>
    //private bool _draggingRed, _draggingGreen, _draggingBlue,
    //    _draggingHue, _draggingSaturation, _draggingValue,
    //    _draggingCyan, _draggingMagenta, _draggingYellow, _draggingKey,
    //    _draggingAlpha;

    /// <summary>
    /// A <see cref="Texture2D" /> value representing a background for the color sliders.
    /// </summary>
    //private static readonly Texture2D _sliderRed, _sliderGreen, _sliderBlue,
    //    _sliderHue, _sliderSaturation, _sliderValue,
    //    _sliderCyan, _sliderMagenta, _sliderYellow, _sliderKey,
    //    _sliderAlpha;

    /// <summary>
    /// A <see cref="Texture2D" /> value representing a background for the color display.
    /// </summary>
    private static readonly Texture2D _colorDisplayBackground = new((int)(_BUTTON_HEIGHT * 4), (int)(_BUTTON_HEIGHT * 4));

    /// <summary>
    /// A <see cref="Color" /> to represent the darkest color choice.
    /// </summary>
    //private Color _blackist = new(0.06f, 0.06f, 0.06f);

    /// <summary>
    /// A <see cref="Color" /> to represent the _greyist color choice.
    /// </summary>
    //private Color _greyist = new(0.2f, 0.2f, 0.2f);

    /// <summary>
    /// Initializes a new static instance of the <see cref="Dialog_ColorPicker" /> class.
    /// </summary>
    static Dialog_ColorPicker()
    {
        SettingsHelper.DoTransparentBackground(ref _colorDisplayBackground);
        //for (var i = 0; i < 255; i++)
        //{
        //    hueChart.SetPixel(0, i, Color.HSVToRGB(Mathf.InverseLerp(0f, 255f, i), 1f, 1f));
        //}

        //hueChart.Apply(false);

        //for (var j = 0; j < 255; j++)
        //{
        //    for (var k = 0; k < 255; k++)
        //    {
        //        Color color = Color.clear;
        //        var c = Color.Lerp(color, Color.white, Mathf.InverseLerp(0f, 255f, j));
        //        color = Color32.Lerp(Color.black, c, Mathf.InverseLerp(0f, 255f, k));
        //        colorChart.SetPixel(j, k, color);
        //    }
        //}

        //colorChart.Apply(false);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Dialog_ColorPicker" /> class.
    /// </summary>
    /// <param name="color">An instance of a <see cref="Color" /> representing the <see cref="Color" /> to start the dialog with.</param>
    /// <param name="onComplete">A method to execute when the user has completed their choice.</param>
    public Dialog_ColorPicker(Color color, Action<Color> onComplete)
    {
        this.closeOnClickedOutside = this.doCloseX = true;
        this._color = this._previousColor = color;
        this._onComplete = onComplete;
        this._onConfirm = (_color) => this._previousColor = _color;
        this._onSave = Settings.SavedPalette.Add;

        // Initialize the current color slider values.
        this._red = this._color.r;
        this._green = this._color.g;
        this._blue = this._color.b;
        this._alpha = this._color.a;
        Color.RGBToHSV(this._color, out _hue, out _saturation, out _value);
        ColorHelper.RGBToCMYK(this._color, out _cyan, out _magenta, out _yellow, out _key);
        this._hex = ColorUtility.ToHtmlStringRGB(this._color);
    }

    /// <summary>
    /// Gets the value of the initial size of the <see cref="Window" />.
    /// </summary>
    public override Vector2 InitialSize => new(400, 350 + (_BUTTON_HEIGHT * 2));

    /// <summary>
    /// A centralized method to set update the colors of every other slider.
    /// </summary>
    /// <param name="red">An optional <see langword="float" /> to set the color red to.</param>
    /// <param name="green">An optional <see langword="float" /> to set the color green to.</param>
    /// <param name="blue">An optional <see langword="float" /> to set the color blue to.</param>
    /// <param name="hue">An optional <see langword="float" /> to set the color's hue to.</param>
    /// <param name="saturation">An optional <see langword="float" /> to set the color's saturation to.</param>
    /// <param name="value">An optional <see langword="float" /> to set the color's value to.</param>
    /// <param name="cyan">An optional <see langword="float" /> to set the color's CMYK cyan value to.</param>
    /// <param name="yellow">An optional <see langword="float" /> to set the color's CMYK yellow value to.</param>
    /// <param name="magenta">An optional <see langword="float" /> to set the color's CMYK magenta value to.</param>
    /// <param name="key">An optional <see langword="float" /> to set the color's CMYK cyan key to.</param>
    /// <param name="alpha">An optional <see langword="float" /> to set the color's alpha to.</param>
    /// <param name="hex">>An optional <see langword="float" /> to set the color's hex value to.</param>
    /// <returns>
    /// <see langword="true" /> when the value of <paramref name="hex"/> is parsed successfully, or if the color's hex is not set; otherwise <see langword="false" />.
    /// </returns>
    public bool SetColor(float red = -1, float green = -1, float blue = -1,
        float hue = -1, float saturation = -1, float value = -1,
        float cyan = -1, float yellow = -1, float magenta = -1, float key = -1,
        float alpha = -1, string? hex = null)
    {
        if (red > -1)
        {
            this._color.r = red;
            Color.RGBToHSV(this._color, out _hue, out _saturation, out _value);
            ColorHelper.RGBToCMYK(this._color, out _cyan, out _magenta, out _yellow, out _key);
            this._hex = ColorUtility.ToHtmlStringRGB(this._color);
        }
        else if (green > -1)
        {
            this._color.g = green;
            Color.RGBToHSV(this._color, out _hue, out _saturation, out _value);
            ColorHelper.RGBToCMYK(this._color, out _cyan, out _magenta, out _yellow, out _key);
            this._hex = ColorUtility.ToHtmlStringRGB(this._color);
        }
        else if (blue > -1)
        {
            this._color.b = blue;
            Color.RGBToHSV(this._color, out _hue, out _saturation, out _value);
            ColorHelper.RGBToCMYK(this._color, out _cyan, out _magenta, out _yellow, out _key);
            this._hex = ColorUtility.ToHtmlStringRGB(this._color);
        }
        else if (hue > -1)
        {
            this._color = this._color.SetHue(hue);
            this._red = this._color.r;
            this._green = this._color.g;
            this._blue = this._color.b;
            ColorHelper.RGBToCMYK(this._color, out _cyan, out _magenta, out _yellow, out _key);
            this._hex = ColorUtility.ToHtmlStringRGB(this._color);
        }
        else if (saturation > -1)
        {
            this._color = this._color.SetSaturation(saturation);
            this._red = this._color.r;
            this._green = this._color.g;
            this._blue = this._color.b;
            ColorHelper.RGBToCMYK(this._color, out _cyan, out _magenta, out _yellow, out _key);
            this._hex = ColorUtility.ToHtmlStringRGB(this._color);
        }
        else if (value > -1)
        {
            this._color = this._color.SetValue(value);
            this._red = this._color.r;
            this._green = this._color.g;
            this._blue = this._color.b;
            ColorHelper.RGBToCMYK(this._color, out _cyan, out _magenta, out _yellow, out _key);
            this._hex = ColorUtility.ToHtmlStringRGB(this._color);
        }
        else if (cyan > -1)
        {
            this._color = this._color.SetCyan(cyan);
            this._red = this._color.r;
            this._green = this._color.g;
            this._blue = this._color.b;
            Color.RGBToHSV(this._color, out _hue, out _saturation, out _value);
            this._hex = ColorUtility.ToHtmlStringRGB(this._color);
        }
        else if (magenta > -1)
        {
            this._color = this._color.SetMagenta(magenta);
            this._red = this._color.r;
            this._green = this._color.g;
            this._blue = this._color.b;
            Color.RGBToHSV(this._color, out _hue, out _saturation, out _value);
            this._hex = ColorUtility.ToHtmlStringRGB(this._color);
        }
        else if (yellow > -1)
        {
            this._color = this._color.SetYellow(yellow);
            this._red = this._color.r;
            this._green = this._color.g;
            this._blue = this._color.b;
            Color.RGBToHSV(this._color, out _hue, out _saturation, out _value);
            this._hex = ColorUtility.ToHtmlStringRGB(this._color);
        }
        else if (key > -1)
        {
            this._color = this._color.SetKey(key);
            this._red = this._color.r;
            this._green = this._color.g;
            this._blue = this._color.b;
            Color.RGBToHSV(this._color, out _hue, out _saturation, out _value);
            this._hex = ColorUtility.ToHtmlStringRGB(this._color);
        }
        else if (alpha > -1)
        {
            this._color.a = alpha;
        }
        else if (hex is not null)
        {
            if (ColorUtility.TryParseHtmlString(hex, out Color color))
            {
                this._color = color;
                this._red = this._color.r;
                this._green = this._color.g;
                this._blue = this._color.b;
                Color.RGBToHSV(this._color, out _hue, out _saturation, out _value);
                ColorHelper.RGBToCMYK(this._color, out _cyan, out _magenta, out _yellow, out _key);
            }
            else
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Renders the window contents.
    /// </summary>
    /// <param name="inRect">An instance of a <see cref="Rect" /> that represents the bounds of the <see cref="Window" />.</param>
    public override void DoWindowContents(Rect inRect)
    {
        var colorContainerRect = new Rect(inRect)
        {
            height = inRect.width - 25
        };

        colorContainerRect.SplitHorizontally((_BUTTON_HEIGHT * 4) + 4, out Rect colorRowPre, out Rect lowerRow);

        this.DoColorBlocks(colorRowPre);

        lowerRow.y += 2;
        lowerRow.SplitHorizontally(_BUTTON_HEIGHT, out Rect buttonRow, out Rect bottomRowRect);

        this.DoColorSliderButtons(buttonRow);

        bottomRowRect.y += 2;

        this.DoColorSliders(colorContainerRect);

        var buttonRect = new Rect(0f, inRect.height - _BUTTON_HEIGHT, _BUTTON_WIDTH, _BUTTON_HEIGHT);
        this.DoBottomButtons(buttonRect);
    }

    /// <summary>
    /// Renders the buttons at the bottom of the <see cref="Window" />.
    /// </summary>
    /// <param name="rect">An instance of a <see cref="Rect" /> that represents the bounds of the buttons.</param>
    private void DoBottomButtons(Rect rect)
    {
        if (Widgets.ButtonText(rect, Strings.SettingsApplyButtonLabel))
        {
            this._onComplete(this._color);
        }

        rect.x += _BUTTON_WIDTH;

        if (Widgets.ButtonText(rect, Strings.SettingsSaveButtonLabel))
        {
            this._onSave(this._color);
        }

        rect.x += _BUTTON_WIDTH;

        if (Widgets.ButtonText(rect, Strings.SettingsOkButtonLabel))
        {
            this._onConfirm(this._color);
        }

        rect.x += _BUTTON_WIDTH;

        if (Widgets.ButtonText(rect, "CancelButton".Translate()))
        {
            this.Close(true);
        }
    }

    private void DoColorBlocks(Rect rect)
    {
        Rect colorRow = rect.LeftPartPixels(_BUTTON_HEIGHT * 4).TopPartPixels(_BUTTON_HEIGHT * 4);
        Widgets.DrawBoxSolid(colorRow, Color.black);

        Rect colorRowBg = colorRow.ContractedBy(2);
        Widgets.DrawTextureFitted(colorRowBg, _colorDisplayBackground, 1f);

        colorRowBg.SplitHorizontally(_BUTTON_HEIGHT * 2, out Rect previousColorRect, out Rect currentColorRect);
        Widgets.DrawBoxSolid(previousColorRect, this._previousColor);
        Widgets.DrawBoxSolid(currentColorRect, this._color);
    }

    private void DoColorSliderButtons(Rect rect)
    {
        var buttonRect = new Rect(0f, rect.y, _BUTTON_WIDTH, _BUTTON_HEIGHT);

        if (Widgets.ButtonText(buttonRect, "RGB"))
        {
            this._currentState = ColorPickerState.RGB;
        }

        buttonRect.x += _BUTTON_WIDTH;

        if (Widgets.ButtonText(buttonRect, "HSL"))
        {
            this._currentState = ColorPickerState.HSV;
        }

        buttonRect.x += _BUTTON_WIDTH;

        if (Widgets.ButtonText(buttonRect, "CMYK"))
        {
            this._currentState = ColorPickerState.CMYK;
        }
    }

    /// <summary>
    /// Renders the sliders in <see cref="Window" />.
    /// </summary>
    /// <param name="rect">An instance of a <see cref="Rect" /> that represents the bounds of the sliders.</param>
    public void DoColorSliders(Rect rect)
    {
        var listing_standard = new Listing_Standard();
        listing_standard.Begin(rect);

        if (_currentState is ColorPickerState.RGB)
        {
            #region Red Row

            if (listing_standard.SliderLabeledSettable("Red", ref this._red,
                bufferFormatter: (float el) => $"{el:F0}",
                formatter: (float el) => el * 255,
                unformatter: (float el) => el / 255,
                min: 0.0f, max: 1.0f,
                bufferMin: 0.0f, bufferMax: 255.0f,
                margin: _SLIDER_MARGIN))
            {
                _ = this.SetColor(red: this._red);
            }

            rect.y += _BUTTON_HEIGHT;

            #endregion Red Row

            #region Green Row

            if (listing_standard.SliderLabeledSettable("Green", ref this._green,
                bufferFormatter: (float el) => $"{el:F0}",
                formatter: (float el) => el * 255,
                unformatter: (float el) => el / 255,
                min: 0.0f, max: 1.0f,
                bufferMin: 0.0f, bufferMax: 255.0f,
                margin: _SLIDER_MARGIN))
            {
                _ = this.SetColor(green: this._green);
            }

            rect.y += _BUTTON_HEIGHT;

            #endregion Green Row

            #region Blue Row

            if (listing_standard.SliderLabeledSettable("Blue", ref this._blue,
                bufferFormatter: (float el) => $"{el:F0}",
                formatter: (float el) => el * 255,
                unformatter: (float el) => el / 255,
                min: 0.0f, max: 1.0f,
                bufferMin: 0.0f, bufferMax: 255.0f,
                margin: _SLIDER_MARGIN))
            {
                _ = this.SetColor(blue: this._blue);
            }

            rect.y += _BUTTON_HEIGHT * 4;

            #endregion Blue Row
        }
        else if (_currentState is ColorPickerState.HSV)
        {
            #region Hue Row

            if (listing_standard.SliderLabeledSettable("Hue", ref this._hue,
                bufferFormatter: (float el) => $"{el:F0}",
                formatter: (float el) => el * 255,
                unformatter: (float el) => el / 255,
                min: 0.0f, max: 1.0f,
                bufferMin: 0.0f, bufferMax: 255.0f,
                margin: _SLIDER_MARGIN))
            {
                _ = this.SetColor(hue: this._hue);
            }

            rect.y += _BUTTON_HEIGHT;

            #endregion Hue Row

            #region Saturation Row

            if (listing_standard.SliderLabeledSettable("Saturation", ref this._saturation,
                bufferFormatter: (float el) => $"{el:F0}",
                formatter: (float el) => el * 255,
                unformatter: (float el) => el / 255,
                min: 0.0f, max: 1.0f,
                bufferMin: 0.0f, bufferMax: 255.0f,
                margin: _SLIDER_MARGIN))
            {
                _ = this.SetColor(saturation: this._saturation);
            }

            rect.y += _BUTTON_HEIGHT;

            #endregion Saturation Row

            #region Value Row

            if (listing_standard.SliderLabeledSettable("Value", ref this._value,
                bufferFormatter: (float el) => $"{el:F0}",
                formatter: (float el) => el * 255,
                unformatter: (float el) => el / 255,
                min: 0.0f, max: 1.0f,
                bufferMin: 0.0f, bufferMax: 255.0f,
                margin: _SLIDER_MARGIN))
            {
                _ = this.SetColor(value: this._value);
            }

            rect.y += _BUTTON_HEIGHT * 4;

            #endregion Value Row
        }
        else if (_currentState is ColorPickerState.CMYK)
        {
            #region Cyan Row

            if (listing_standard.SliderLabeledSettable("Cyan", ref _cyan,
                bufferFormatter: (float el) => $"{el:F0}",
                formatter: (float el) => el * 255,
                unformatter: (float el) => el / 255,
                min: 0.0f, max: 1.0f,
                bufferMin: 0.0f, bufferMax: 255.0f,
                margin: _SLIDER_MARGIN))
            {
                _ = this.SetColor(cyan: this._cyan);
            }

            rect.y += _BUTTON_HEIGHT;

            #endregion Cyan Row

            #region Magenta Row

            if (listing_standard.SliderLabeledSettable("Magenta", ref this._magenta,
                bufferFormatter: (float el) => $"{el:F0}",
                formatter: (float el) => el * 255,
                unformatter: (float el) => el / 255,
                min: 0.0f, max: 1.0f,
                bufferMin: 0.0f, bufferMax: 255.0f,
                margin: _SLIDER_MARGIN))
            {
                _ = this.SetColor(magenta: this._magenta);
            }

            rect.y += _BUTTON_HEIGHT;

            #endregion Magenta Row

            #region Yellow Row

            if (listing_standard.SliderLabeledSettable("Yellow", ref _yellow,
                bufferFormatter: (float el) => $"{el:F0}",
                formatter: (float el) => el * 255,
                unformatter: (float el) => el / 255,
                min: 0.0f, max: 1.0f,
                bufferMin: 0.0f, bufferMax: 255.0f,
                margin: _SLIDER_MARGIN))
            {
                _ = this.SetColor(yellow: this._yellow);
            }

            rect.y += _BUTTON_HEIGHT;

            #endregion Yellow Row

            #region Key Row

            if (listing_standard.SliderLabeledSettable("Key", ref this._key,
                bufferFormatter: (float el) => $"{el:F0}",
                formatter: (float el) => el * 255,
                unformatter: (float el) => el / 255,
                min: 0.0f, max: 1.0f,
                bufferMin: 0.0f, bufferMax: 255.0f,
                margin: _SLIDER_MARGIN))
            {
                _ = this.SetColor(key: this._key);
            }

            rect.y += _BUTTON_HEIGHT;

            #endregion Key Row
        }

        #region Alpha Row

        if (listing_standard.SliderLabeledSettable("Alpha", ref this._alpha,
                bufferFormatter: (float el) => $"{el:F0}",
                formatter: (float el) => el * 255,
                unformatter: (float el) => el / 255,
                min: 0.0f, max: 1.0f,
                bufferMin: 0.0f, bufferMax: 255.0f,
                margin: _SLIDER_MARGIN))
        {
            _ = this.SetColor(alpha: this._alpha);
        }

        rect.y += _BUTTON_HEIGHT;

        #endregion Alpha Row

        if (listing_standard.AddLabeledTextFieldWithPrefix("Hex", ref this._hex,
            formatter: (string el) => $"#{el}",
            unformatter: (string el) => el.TrimStart('#'),
            comparisonType: StringComparison.OrdinalIgnoreCase))
        {
            if (!this.SetColor(hex: this._hex))
            {
                Color originalColor = GUI.color;
                GUI.color = Color.red;
                _ = listing_standard.SubLabel("Invalid hex color.", 1f);
                GUI.color = originalColor;
            }
        }

        listing_standard.End();
    }
}
