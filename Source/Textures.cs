using UnityEngine;
using Verse;
namespace Drummeur.BetterLinkableColors;
/// <summary>
/// A class to wrap loading and handling <see cref="Texture2D" /> instances.
/// </summary>
[StaticConstructorOnStartup]
public static class Textures
{
    /// <inheritdoc cref="Materials.Loaded" />
    private static bool _loaded = false;

    /// <summary>
    /// Gets a <see langword="bool" /> determining if the <see cref="Material" />s have been loaded.
    /// </summary>
    public static bool Loaded => _loaded;

#pragma warning disable IDE1006 // Naming Styles
    /// <summary>
    /// Gets the texture for the structure link line.
    /// </summary>
    public static Texture2D LineTex = null!;

    /// <summary>
    /// Gets the texture for the color picker slider line.
    /// </summary>
    public static Texture2D ColorHue = null!;

    /// <summary>
    /// Gets the texture for the color picker position dot.
    /// </summary>
    public static Texture2D ColorPicker = null!;
#pragma warning restore IDE1006 // Naming Styles

    /// <summary>
    /// A static constructor for the <see cref="Textures" /> class.
    /// </summary>
    static Textures()
    {
        LoadTextures();
    }

    /// <summary>
    /// Reloads each <see cref="Texture2D" /> in the <see cref="Textures" /> class when the <see cref="LongEventHandler" /> is finished.
    /// </summary>
    public static void Reset()
    {
        LongEventHandler.ExecuteWhenFinished(LoadTextures);
    }

    /// <summary>
    /// Loads each <see cref="Texture2D" /> in the <see cref="Textures" /> class.
    /// </summary>
    private static void LoadTextures()
    {
        _loaded = false;
        LineTex = ContentFinder<Texture2D>.Get("UI/Overlays/ThingLine");
        ColorHue = ContentFinder<Texture2D>.Get("UI/ColorTools/ColorHue");
        ColorPicker = ContentFinder<Texture2D>.Get("UI/ColorTools/ColorCog");
        _loaded = true;
    }
}
