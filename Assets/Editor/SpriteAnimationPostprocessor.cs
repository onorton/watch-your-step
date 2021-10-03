using System.IO;
using UnityEditor;
using UnityEngine;

public class SpriteAnimationPostprocessor : AssetPostprocessor
{

    // Update clip if sprites are updated
    void OnPostprocessSprites(Texture2D texture, Sprite[] sprites)
    {
        var animationPath = SpriteAnimationGenerator.GetAnimationPath(assetPath);

        // Recreate animation if it exists
        if (File.Exists(animationPath))
        {
            var importer = (TextureImporter)TextureImporter.GetAtPath(assetPath);
            var animationClip = AssetDatabase.LoadAssetAtPath<AnimationClip>(animationPath);

            // Delay needed so texture can be loaded
            EditorApplication.delayCall += () => { SpriteAnimationGenerator.UpdateAnimationClip(animationClip, assetPath, animationPath); };
        }
    }

    // Try slicing spritesheet if texture is edited and animation exists
    void OnPostprocessTexture(Texture2D texture)
    {
        var animationPath = SpriteAnimationGenerator.GetAnimationPath(assetPath);

        // Recreate animation if it exists
        if (File.Exists(animationPath))
        {
            AssetDatabase.SaveAssets();
            var importer = (TextureImporter)TextureImporter.GetAtPath(assetPath);
            var animationClip = AssetDatabase.LoadAssetAtPath<AnimationClip>(animationPath);

            // Should all be the same size so just get first one
            var firstSpriteRect = importer.spritesheet[0];

            // Delay needed so texture can be loaded
            EditorApplication.delayCall += () => { SpriteAnimationGenerator.SliceSpritesheet(assetPath, new Vector2(firstSpriteRect.rect.width, firstSpriteRect.rect.height)); };
        }
    }
}
