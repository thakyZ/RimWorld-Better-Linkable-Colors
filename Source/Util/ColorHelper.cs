using System;
using UnityEngine;
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
    /// Converts a <see cref="Color" /> to a hex value <see langword="string" />, with the format <c>#XXXXXX</c>.
    /// </summary>
    /// <param name="color">The <see cref="Color" /> to try to convert.</param>
    /// <returns>A hex value <see langword="string" />, with the format <c>#XXXXXX</c>.</returns>
    internal static string ToHtmlString(this Color color)
    {
        return $"#{ColorUtility.ToHtmlStringRGB(color)}";
    }

    /// <summary>
    /// Converts a <see langword="float" /> to the equivalent as an <see langword="int" />.
    /// </summary>
    /// <param name="rgbDecimal">A <see langword="float" /> representing the RGB decimal value.</param>
    /// <returns>An <see langword="int" /> representing the RGB value.</returns>
    internal static int ToRGBInt(this float rgbDecimal)
    {
        return (int)Math.Max(Math.Min(rgbDecimal * 255, 0), 255);
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
    /// Converts a <see cref="Color" /> to an <see langword="int" />.
    /// </summary>
    /// <param name="color">The <see cref="Color" /> to try to convert.</param>
    /// <returns>An <see langword="int" /> representing the color.</returns>
    internal static int ToInt(this Color color)
    {
        return ((color.a.ToRGBInt() & 0xFF) << 24) + ((color.r.ToRGBInt() & 0xFF) << 16) + ((color.g.ToRGBInt() & 0xFF) << 8) + ((color.b.ToRGBInt() & 0xFF) << 0);
    }

    /// <summary>
    /// Converts an <see langword="int" /> to a <see cref="Color" />.
    /// </summary>
    /// <param name="color">The <see langword="int" /> to try to convert.</param>
    /// <returns>A <see cref="Color" />, with the format <c>#XXXXXX</c>.</returns>
    internal static Color ToColor(this int color)
    {
        var a = (color >> 24) & 0xFF;
        var r = (color >> 16) & 0xFF;
        var g = (color >> 8) & 0xFF;
        var b = (color >> 0) & 0xFF;

        return new Color(r, g, b, a);
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
