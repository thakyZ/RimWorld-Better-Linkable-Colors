#if DEBUG
using System;
using UnityEngine;
using Verse;
namespace Drummeur.BetterLinkableColors.Util;
/// <summary>
/// A generic <see cref="Window" /> to contain any information easily.
/// </summary>
internal class GenericWindow : Window
{
    /// <summary>
    /// An action to use to render the contents.
    /// </summary>
    private readonly Action<Window, Rect> _contents;

    /// <summary>
    /// Initializes a new instance of the <see cref="GenericWindow" /> class.
    /// </summary>
    /// <param name="contents">An action to use to render the contents.</param>
    public GenericWindow(Action<Window, Rect> contents)
    {
        this._contents = contents;
    }

    /// <summary>
    /// Renders the contents of the <see cref="Window" />.
    /// </summary>
    /// <param name="inRect">A <see cref="Rect" /> defining the bounds of the <see cref="Window" />.</param>
    public override void DoWindowContents(Rect inRect)
    {
        this._contents.Invoke(this, inRect);
    }
}
#endif