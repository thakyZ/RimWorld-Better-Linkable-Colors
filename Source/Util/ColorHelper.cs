﻿using System;
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
        else
        {
            return null;
        }
    }

    internal static string ToHtmlString(this Color color)
    {
        return $"#{ColorUtility.ToHtmlStringRGB(color)}";
    }

    internal static class Defaults
    {
        internal static Color ActiveColor = Color.green;
        internal static Color InactiveColor = Color.red;
        internal static Color PotentialColor = Color.blue;
        internal static Color SupplantedColor = Color.yellow;
    }
}
