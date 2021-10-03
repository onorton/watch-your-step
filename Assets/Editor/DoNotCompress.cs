using UnityEditor;
using UnityEngine;
using System.Collections;

public class DoNotCompressTextures : AssetPostprocessor
{
    void OnPreprocessTexture()
    {
        TextureImporter textureImporter = (TextureImporter)assetImporter;
        textureImporter.textureCompression = TextureImporterCompression.Uncompressed;
    }
}