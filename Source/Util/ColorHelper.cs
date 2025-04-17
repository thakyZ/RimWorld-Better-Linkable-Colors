using System;
using UnityEngine;
using Verse;
namespace Drummeur.BetterLinkableColors.Util;
/// <summary>
/// A helper class for parsing colors.
/// </summary>
internal static class ColorHelper
{
    /// <summary>
    /// Converts a <see langword="string" /> to a <see cref="Color" />.
    /// </summary>
    /// <param name="str">The <see langword="string" /> to try to convert.</param>
    /// <returns><see cref="Color" /> when successful; otherwise, <see langword="null" />.</returns>
    internal static Color? ColorFromRgbString(string str)
    {
        return ColorUtility.TryParseHtmlString(str, out Color color) ? color : null;
    }

    /// <summary>
    /// Converts a <see cref="Color" /> to a _hex _value <see langword="string" />, with the format <c>#XXXXXX</c>.
    /// </summary>
    /// <param name="color">The <see cref="Color" /> to try to convert.</param>
    /// <returns>A _hex _value <see langword="string" />, with the format <c>#XXXXXX</c>.</returns>
    internal static string ToHtmlString(this Color color)
    {
        return $"#{ColorUtility.ToHtmlStringRGB(color)}";
    }

    /// <summary>
    /// Converts a <see langword="float" /> to the equivalent as an <see langword="int" />.
    /// </summary>
    /// <param name="rgbDecimal">A <see langword="float" /> representing the RGB decimal value.</param>
    /// <returns>An <see langword="int" /> representing the RGB value.</returns>
    internal static uint ToRGBInt(this float rgbDecimal)
    {
        return (uint)Math.Max(Math.Min(rgbDecimal * 255, 0), 255);
    }

    /// <summary>
    /// Converts an <see langword="int" /> to the equivalent as an <see langword="float" />.
    /// </summary>
    /// <param name="rgbInt">An <see langword="int" /> representing the RGB decimal value.</param>
    /// <returns>A <see langword="float" /> representing the RGB value.</returns>
    internal static float ToRGBDecimal(this int rgbInt)
    {
        return rgbInt / 255;
    }

    /// <summary>
    /// Converts a <see cref="Color" /> to an <see langword="uint" />.
    /// </summary>
    /// <param name="color">The <see cref="Color" /> to try to convert.</param>
    /// <returns>An <see langword="uint" /> representing the color.</returns>
    internal static uint ToInt(this Color color)
    {
        Log.Error($"in  = {color} ");
        var output = ((color.a.ToRGBInt() & 0xFF) << 24) + ((color.r.ToRGBInt() & 0xFF) << 16) + ((color.g.ToRGBInt() & 0xFF) << 8) + ((color.b.ToRGBInt() & 0xFF) << 0);
        Log.Error($"int = {output} ");
        return output;
    }

    /// <summary>
    /// Converts an <see langword="uint" /> to a <see cref="Color" />.
    /// </summary>
    /// <param name="color">The <see langword="uint" /> to try to convert.</param>
    /// <returns>A <see cref="Color" />, with the format <c>#XXXXXX</c>.</returns>
    internal static Color ToColor(this uint color)
    {
        Log.Error($"int = {color} ");
        var a = (color >> 24) & 0xFF;
        var r = (color >> 16) & 0xFF;
        var g = (color >> 8) & 0xFF;
        var b = (color >> 0) & 0xFF;

        var output = new Color(r, g, b, a);
        Log.Error($"out = {output} ");
        return output;
    }

    /// <summary>
    /// Calculated the new hue value for the given <see cref="Color" /> given the <paramref name="change" /> value.
    /// </summary>
    /// <param name="col">The <see cref="Color" /> to alter the hue of.</param>
    /// <param name="change">The new hue value of the <see cref="Color" />.</param>
    /// <returns>An instance of the <see cref="Color" />.</returns>
    public static Color SetHue(this Color col, float change)
    {
        Color.RGBToHSV(col, out _, out var saturation, out var value);
        var _col = Color.HSVToRGB(change, saturation, value);
        col.r = _col.r;
        col.g = _col.g;
        col.b = _col.b;
        return col;
    }

    /// <summary>
    /// Calculated the new saturation value for the given <see cref="Color" /> given the <paramref name="change" /> value.
    /// </summary>
    /// <param name="col">The <see cref="Color" /> to alter the saturation of.</param>
    /// <param name="change">The new saturation value of the <see cref="Color" />.</param>
    /// <returns>An instance of the <see cref="Color" />.</returns>
    public static Color SetSaturation(this Color col, float change)
    {
        Color.RGBToHSV(col, out var hue, out _, out var value);
        var _col = Color.HSVToRGB(hue, change, value);
        col.r = _col.r;
        col.g = _col.g;
        col.b = _col.b;
        return col;
    }

    /// <summary>
    /// Calculated the new value for the given <see cref="Color" /> given the <paramref name="change" /> value.
    /// </summary>
    /// <param name="col">The <see cref="Color" /> to alter the value of.</param>
    /// <param name="change">The new value of the <see cref="Color" />.</param>
    /// <returns>An instance of the <see cref="Color" />.</returns>
    public static Color SetValue(this Color col, float change)
    {
        Color.RGBToHSV(col, out var hue, out var saturation, out _);
        var _col = Color.HSVToRGB(hue, saturation, change);
        col.r = _col.r;
        col.g = _col.g;
        col.b = _col.b;
        return col;
    }

    /// <summary>
    /// Calculated the new CMYK cyan value for the given <see cref="Color" /> given the <paramref name="change" /> value.
    /// </summary>
    /// <param name="col">The <see cref="Color" /> to alter the CMYK cyan value of.</param>
    /// <param name="change">The new CMYK cyan value of the <see cref="Color" />.</param>
    /// <returns>An instance of the <see cref="Color" />.</returns>
    public static Color SetCyan(this Color col, float change)
    {
        RGBToCMYK(col, out _, out var magenta, out var yellow, out var key);
        Color _col = CMYKToRGB(change, magenta, yellow, key);
        col.r = _col.r;
        col.g = _col.g;
        col.b = _col.b;
        return col;
    }

    /// <summary>
    /// Calculated the new CMYK magenta value for the given <see cref="Color" /> given the <paramref name="change" /> value.
    /// </summary>
    /// <param name="col">The <see cref="Color" /> to alter the CMYK magenta value of.</param>
    /// <param name="change">The new CMYK magenta value of the <see cref="Color" />.</param>
    /// <returns>An instance of the <see cref="Color" />.</returns>
    public static Color SetMagenta(this Color col, float change)
    {
        RGBToCMYK(col, out var cyan, out _, out var yellow, out var key);
        Color _col = CMYKToRGB(cyan, change, yellow, key);
        col.r = _col.r;
        col.g = _col.g;
        col.b = _col.b;
        return col;
    }

    /// <summary>
    /// Calculated the new CMYK yellow value for the given <see cref="Color" /> given the <paramref name="change" /> value.
    /// </summary>
    /// <param name="col">The <see cref="Color" /> to alter the CMYK yellow value of.</param>
    /// <param name="change">The new CMYK yellow value of the <see cref="Color" />.</param>
    /// <returns>An instance of the <see cref="Color" />.</returns>
    public static Color SetYellow(this Color col, float change)
    {
        RGBToCMYK(col, out var cyan, out var magenta, out _, out var key);
        Color _col = CMYKToRGB(cyan, magenta, change, key);
        col.r = _col.r;
        col.g = _col.g;
        col.b = _col.b;
        return col;
    }

    /// <summary>
    /// Calculated the new CMYK key value for the given <see cref="Color" /> given the <paramref name="change" /> value.
    /// </summary>
    /// <param name="col">The <see cref="Color" /> to alter the CMYK key value of.</param>
    /// <param name="change">The new CMYK key value of the <see cref="Color" />.</param>
    /// <returns>An instance of the <see cref="Color" />.</returns>
    public static Color SetKey(this Color col, float change)
    {
        RGBToCMYK(col, out var cyan, out var magenta, out var yellow, out _);
        Color _col = CMYKToRGB(cyan, magenta, yellow, change);
        col.r = _col.r;
        col.g = _col.g;
        col.b = _col.b;
        return col;
    }

    /// <summary>
    /// Converts a given <see cref="Color" /> to the different CMYK values.
    /// </summary>
    /// <param name="col">The <see cref="Color" /> to get the CMYK values of.</param>
    /// <param name="cyan">A <see langword="Float" /> representing the CMYK cyan value.</param>
    /// <param name="magenta">A <see langword="Float" /> representing the CMYK magenta value.</param>
    /// <param name="yellow">A <see langword="Float" /> representing the CMYK yellow value.</param>
    /// <param name="key">A <see langword="Float" /> representing the CMYK key value.</param>
    public static void RGBToCMYK(Color col, out float cyan, out float magenta, out float yellow, out float key)
    {
        key = 1 - Math.Max(Math.Max(col.r, col.g), col.b);
        cyan = (1 - key is 0) ? 0 : (1 - col.r - key) / (1 - key);
        magenta = (1 - key is 0) ? 0 : (1 - col.g - key) / (1 - key);
        yellow = (1 - key is 0) ? 0 : (1 - col.b - key) / (1 - key);
    }

    /// <summary>
    /// Converts a given set of CMYK values to a new <see cref="Color" />.
    /// </summary>
    /// <param name="cyan">A <see langword="Float" /> representing the CMYK cyan value.</param>
    /// <param name="magenta">A <see langword="Float" /> representing the CMYK magenta value.</param>
    /// <param name="yellow">A <see langword="Float" /> representing the CMYK yellow value.</param>
    /// <param name="key">A <see langword="Float" /> representing the CMYK key value.</param>
    /// <returns>An instance of the <see cref="Color" />.</returns>
    public static Color CMYKToRGB(float cyan, float magenta, float yellow, float key)
    {
        return new Color((1 - cyan) * (1 - key), (1 - magenta) * (1 - key), (1 - yellow) * (1 - key));
    }

    /// <summary>
    /// A class to provide the default values for the line colors.
    /// </summary>
    internal static class Defaults
    {
#pragma warning disable IDE1006 // Naming Styles
        /// <summary>
        /// A <see cref="Color" /> to use for the link lines that are active.
        /// </summary>
        internal static readonly Color ActiveColor = Color.green;

        /// <summary>
        /// A <see cref="Color" /> to use for the link lines that are inactive.
        /// </summary>
        internal static readonly Color InactiveColor = Color.red;

        /// <summary>
        /// A <see cref="Color" /> to use for the link lines that can be connected when attempting to place a blueprint.
        /// </summary>
        internal static readonly Color PotentialColor = Color.blue;

        /// <summary>
        /// A <see cref="Color" /> to use for the link lines that are being replace with another linkage when attempting to place a blueprint.
        /// </summary>
        internal static readonly Color OverriddenColor = Color.yellow;
#pragma warning restore IDE1006 // Naming Styles
    }
}
