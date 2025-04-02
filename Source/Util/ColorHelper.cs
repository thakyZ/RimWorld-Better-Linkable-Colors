using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace drummeur.linkablecolors.Util;

internal static class ColorHelper
{
    internal static Color? ColorFromRgbString(string str)
    {
        return ColorUtility.TryParseHtmlString(str, out Color color) ? color : null;
    }

    internal static string ToHtmlString(this Color color)
    {
        return $"#{ColorUtility.ToHtmlStringRGB(color)}";
    }

    internal static class Defaults
    {
        internal static readonly Color ActiveColor = Color.green;
        internal static readonly Color InactiveColor = Color.red;
        internal static readonly Color PotentialColor = Color.blue;
        internal static readonly Color SupplantedColor = Color.yellow;
    }
}
