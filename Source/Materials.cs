using UnityEngine;
using Verse;
using Drummeur.BetterLinkableColors.Util;
namespace Drummeur.BetterLinkableColors;
/// <summary>
/// A class to wrap loading and handling <see cref="Material" /> instances.
/// </summary>
[StaticConstructorOnStartup]
public static class Materials
{
    /// <inheritdoc cref="Materials.Loaded" />
    private static bool _loaded = false;

    /// <summary>
    /// Gets a <see langword="bool" /> determining if the <see cref="Material" />s have been loaded.
    /// </summary>
    public static bool Loaded => _loaded;

#pragma warning disable IDE1006 // Naming Styles
    /// <summary>
    /// Gets the _value for the active line using the reference to the material from the <see cref="MaterialPool" /> using the _value from the <see cref="Settings.ActiveColorLabel" /> setting.
    /// </summary>
    internal static Material ActiveLineMat = null!;

    /// <summary>
    /// Gets the _value for the inactive line using the reference to the material from the <see cref="MaterialPool" /> using the _value from the <see cref="Settings.InactiveColorLabel" /> setting.
    /// </summary>
    internal static Material InactiveLineMat = null!;

    /// <summary>
    /// Gets the _value for the potential line using the reference to the material from the <see cref="MaterialPool" /> using the _value from the <see cref="Settings.PotentialColorLabel" /> setting.
    /// </summary>
    internal static Material PotentialLineMat = null!;

    /// <summary>
    /// Gets the _value for the supplanted line using the reference to the material from the <see cref="MaterialPool" /> using the _value from the <see cref="Settings.OverriddenColorLabel" /> setting.
    /// </summary>
    internal static Material OverriddenLineMat = null!;
#pragma warning restore IDE1006 // Naming Styles

    /// <summary>
    /// Loads a material from a given <see cref="Texture2D" />, <see cref="Shader" />, and <see cref="Color" />.
    /// </summary>
    /// <param name="texture">An instance of a <see cref="Texture2D" /> to use when rendering the <see cref="Material" />.</param>
    /// <param name="shader">An instance of a <see cref="Shader" /> to apply to the <see cref="Material" />.</param>
    /// <param name="color">An instance of a <see cref="Color" /> to apply to the <paramref name="texture" />.</param>
    /// <returns>An instance of a <see cref="Material" />.</returns>
    private static Material LoadMaterial(Texture2D texture, Shader shader, Color color)
        => MaterialPool.MatFrom(texture, shader, color);

    /// <summary>
    /// Loads a material from a given <see cref="Texture2D" />, <see langword="bool" />, and <see cref="Color" />.
    /// </summary>
    /// <param name="texture">An instance of a <see cref="Texture2D" /> to use when rendering the <see cref="Material" />.</param>
    /// <param name="useSolidLine">
    /// A <see langword="bool" /> that determines if the shader should be,
    /// <see cref="ShaderDatabase.SolidColor" /> if <see langword="true" />,
    /// or <see cref="ShaderDatabase.Transparent" /> if <see langword="false" />.
    /// </param>
    /// <param name="color">An instance of a <see cref="Color" /> to apply to the <paramref name="texture" />.</param>
    /// <returns>An instance of a <see cref="Material" />.</returns>
    private static Material LoadMaterial(Texture2D texture, bool useSolidLine, Color color)
        => LoadMaterial(texture, useSolidLine ? ShaderDatabase.SolidColor : ShaderDatabase.Transparent, color);

    /// <summary>
    /// Loads a material from a given <see cref="Color" /> with the <see cref="Texture2D" /> <see cref="Textures.LineTex" />,
    /// and the <see langword="bool" /> <see cref="Settings.UseSolidLineShader" />.
    /// </summary>
    /// <param name="color">An instance of a <see cref="Color" /> to apply to the <paramref name="Material" />'s texture.</param>
    /// <returns>An instance of a <see cref="Material" />.</returns>
    private static Material LoadMaterial(Color color)
        => LoadMaterial(Textures.LineTex, Settings.UseSolidLineShader, color);

    /// <summary>
    /// A static constructor for the <see cref="Materials" /> class.
    /// </summary>
    static Materials()
    {
        LoadMaterials();
    }

    /// <summary>
    /// Reloads each <see cref="Material" /> in the <see cref="Materials" /> class when the <see cref="LongEventHandler" /> is finished.
    /// </summary>
    public static void Reset()
    {
        LongEventHandler.ExecuteWhenFinished(LoadMaterials);
    }

    /// <summary>
    /// Loads each <see cref="Material" /> in the <see cref="Materials" /> class.
    /// </summary>
    private static void LoadMaterials()
    {
        _loaded = false;
        ActiveLineMat = LoadMaterial(Settings.ActiveColor);
        InactiveLineMat = LoadMaterial(Settings.InactiveColor);
        PotentialLineMat = LoadMaterial(Settings.PotentialColor);
        OverriddenLineMat = LoadMaterial(Settings.OverriddenColor);
        _loaded = true;
    }
}
