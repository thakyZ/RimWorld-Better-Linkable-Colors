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
    /// A class to provide the default values for the line colors.
    /// </summary>
    internal static class Defaults
    {
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
        internal static readonly Color SupplantedColor = Color.yellow;
    }
}
