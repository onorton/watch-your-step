using UnityEditor;
using UnityEngine;

public class DoNotFilterSprites : AssetPostprocessor
{
    void OnPreprocessTexture()
    {
        if (assetPath.Contains("Sprites"))
        {
            var textureImporter = (TextureImporter)assetImporter;
            textureImporter.filterMode = FilterMode.Point;
        }
    }
}