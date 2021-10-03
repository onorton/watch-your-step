using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.IO;
using System.Linq;
using System.Globalization;

public class SpriteAnimationGenerator : EditorWindow
{

    private Vector2Int _frameDimensions = Vector2Int.zero;
    private int _frameRate = 12;
    private bool _loop = true;

    [MenuItem("Assets/Create Animation")]
    private static void ShowWindow()
    {
        var window = GetWindow<SpriteAnimationGenerator>();
        window.titleContent = new GUIContent("Create Animation");
        window.Show();
    }

    [MenuItem("Assets/Create Animation", true)]
    private static bool ValidateMultipleTexture2D()
    {
        if (Selection.activeObject is Texture2D texture)
        {
            var path = AssetDatabase.GetAssetPath(texture);
            var importer = (TextureImporter)TextureImporter.GetAtPath(path);
            return importer.spriteImportMode == SpriteImportMode.Multiple;
        }

        return false;
    }


    private void OnGUI()
    {
        _frameDimensions = EditorGUILayout.Vector2IntField("Frame Dimensions", _frameDimensions);
        _frameRate = EditorGUILayout.IntField("Frame Rate", _frameRate);
        _loop = EditorGUILayout.Toggle("Loop Animation", _loop);

        GUI.enabled = _frameDimensions.x > 0 && _frameDimensions.y > 0 && _frameRate > 0;


        if (GUILayout.Button("Create Animation"))
        {
            var texture = Selection.activeObject as Texture2D;
            var assetPath = AssetDatabase.GetAssetPath(texture);
            CreateAnimationClip(assetPath, _frameRate, _loop);
            SliceSpritesheet(assetPath, _frameDimensions);
        }
        GUI.enabled = true;
    }

    public static string GetAnimationPath(string texturePath)
    {
        string filenameNoExtension = Path.GetFileNameWithoutExtension(texturePath);
        return $"Assets/Animations/{CultureInfo.InvariantCulture.TextInfo.ToTitleCase(filenameNoExtension).Replace("_", " ")}.anim";
    }

    public static void SliceSpritesheet(string assetPath, Vector2 frameDimensions)
    {
        var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(assetPath);
        string filenameNoExtension = Path.GetFileNameWithoutExtension(assetPath);
        var animationPath = GetAnimationPath(assetPath);
        var importer = (TextureImporter)TextureImporter.GetAtPath(assetPath);

        Rect[] rects = InternalSpriteUtility.GenerateGridSpriteRectangles(texture, Vector2.zero, frameDimensions, Vector2.zero, false);

        List<SpriteMetaData> metas = new List<SpriteMetaData>();
        int rectNum = 0;

        foreach (Rect rect in rects)
        {
            SpriteMetaData meta = new SpriteMetaData();
            meta.rect = rect;
            meta.name = filenameNoExtension + "_" + rectNum++;
            metas.Add(meta);
        }

        // Only save if the number of sprites changes to avoid infinite reloads.
        if (metas.ToArray().Length != importer.spritesheet.Length)
        {
            importer.spritesheet = metas.ToArray();
            EditorUtility.SetDirty(importer);
            importer.SaveAndReimport();
            AssetDatabase.Refresh();
        }

    }

    private static void CreateAnimationClip(string assetPath, int frameRate, bool loop)
    {
        var animationPath = GetAnimationPath(assetPath);

        var animationClip = new AnimationClip();

        AnimationUtility.SetAnimationClipSettings(animationClip, new AnimationClipSettings { loopTime = loop });
        animationClip.frameRate = frameRate;
        animationClip.name = Path.GetFileNameWithoutExtension(animationPath);

        AssetDatabase.CreateAsset(animationClip, animationPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

    }

    public static void UpdateAnimationClip(AnimationClip animationClip, string assetPath, string animationPath)
    {
        var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(assetPath);
        var importer = (TextureImporter)TextureImporter.GetAtPath(assetPath);

        EditorCurveBinding spriteBinding = new EditorCurveBinding();
        spriteBinding.type = typeof(SpriteRenderer);
        spriteBinding.path = "";
        spriteBinding.propertyName = "m_Sprite";

        var sprites = AssetDatabase.LoadAllAssetRepresentationsAtPath(assetPath);

        var keyFrames = importer.spritesheet.Select((s, index) =>
        {
            var objectReferenceKeyFrame = new ObjectReferenceKeyframe();
            objectReferenceKeyFrame.time = index / animationClip.frameRate;
            objectReferenceKeyFrame.value = sprites[index];
            return objectReferenceKeyFrame;
        }).ToArray();

        AnimationUtility.SetObjectReferenceCurve(animationClip, spriteBinding, keyFrames);
        var curveBinding = AnimationUtility.GetObjectReferenceCurveBindings(animationClip)[0];
        var curve = AnimationUtility.GetObjectReferenceCurve(animationClip, curveBinding);


        var overrideControllerFiles = Directory.GetFiles("Assets/Animations", "*.overrideController", SearchOption.AllDirectories);
        var overrideControllers = overrideControllerFiles.Select(f => AssetDatabase.LoadAssetAtPath<AnimatorOverrideController>(f));

        var overrideControllersThatNeedUpdates = new Dictionary<AnimatorOverrideController, (string, AnimationClip)>();

        foreach (var overrideController in overrideControllers)
        {

            foreach (var controllerClip in overrideController.runtimeAnimatorController.animationClips)
            {

                if (controllerClip.name == animationClip.name)
                {

                    overrideControllersThatNeedUpdates[overrideController] = (controllerClip.name, overrideController[controllerClip.name]);
                }

                if (overrideController[controllerClip.name].name == animationClip.name)
                {
                    overrideControllersThatNeedUpdates[overrideController] = (controllerClip.name, animationClip);
                }
            }
        }

        var controllerFiles = Directory.GetFiles("Assets/Animations", "*.controller", SearchOption.AllDirectories);
        var controllers = controllerFiles.Select(f => AssetDatabase.LoadAssetAtPath<UnityEditor.Animations.AnimatorController>(f));
        foreach (var controller in controllers)
        {
            foreach (var layer in controller.layers)
            {
                foreach (var state in layer.stateMachine.states)
                {
                    if (state.state.motion.name == animationClip.name)
                    {
                        state.state.motion = animationClip;
                    }
                }
            }
        }

        foreach (var item in overrideControllersThatNeedUpdates)
        {
            item.Key[item.Value.Item1] = item.Value.Item2;
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
