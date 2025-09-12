using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Animations;

public class SpriteToAnimationCreator : EditorWindow
{
    private Texture2D sourceTexture;
    private float frameRate = 4f;
    private DefaultAsset outputFolder;
    private DefaultAsset outputPrefabFolder;
    private DefaultAsset atlasSourceFolder;

    [MenuItem("ET/XET/从已切割Sprite创建四方向动画")]
    public static void ShowWindow()
    {
        GetWindow<SpriteToAnimationCreator>("Sprite转动画工具");
    }

    void OnGUI()
    {
        GUILayout.Label("已切割Sprite动画创建工具", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        sourceTexture = (Texture2D)EditorGUILayout.ObjectField("源纹理", sourceTexture, typeof(Texture2D), false);
        frameRate = EditorGUILayout.FloatField("帧率", frameRate);
        outputFolder = (DefaultAsset)EditorGUILayout.ObjectField("动画输出文件夹", outputFolder, typeof(DefaultAsset), false);
        outputPrefabFolder = (DefaultAsset)EditorGUILayout.ObjectField("预制体输出文件夹", outputPrefabFolder, typeof(DefaultAsset), false);
        atlasSourceFolder = (DefaultAsset)EditorGUILayout.ObjectField("图集文件夹", atlasSourceFolder, typeof(DefaultAsset), false);

        EditorGUILayout.Space();

        EditorGUILayout.Space();

        if (GUILayout.Button("创建动画和Animator"))
        {
            if (sourceTexture != null && outputFolder != null)
            {
                CreateAnimationsFromSlicedTexture(sourceTexture);
            }
            else
            {
                EditorUtility.DisplayDialog("错误", "请选择源纹理和输出文件夹", "确定");
            }
        }

        if (GUILayout.Button("根据资源文件夹批量创建动画及预制体"))
        {
            if (this.atlasSourceFolder != null)
            {
                this.CreateByAllSourceTextures();
            }
        }
    }

    private string GetOutputPath()
    {
        if (outputFolder == null) return "Assets";
        string folderPath = AssetDatabase.GetAssetPath(outputFolder);
        return string.IsNullOrEmpty(folderPath) ? "Assets" : folderPath;
    }

    private string GetPrefabOutputPath()
    {
        if (outputPrefabFolder == null) return "Assets";
        string folderPath = AssetDatabase.GetAssetPath(this.outputPrefabFolder);
        return string.IsNullOrEmpty(folderPath) ? "Assets" : folderPath;
    }

    private void CreateByAllSourceTextures()
    {
        string folderPath = AssetDatabase.GetAssetPath(this.atlasSourceFolder);
        var files = Directory.GetFiles(folderPath, "*.*", SearchOption.AllDirectories).Where(path => !path.EndsWith(".meta")).ToArray();
        foreach (string filePath in files)
        {
            Texture2D assets = AssetDatabase.LoadAssetAtPath<Texture2D>(filePath);
            CreateAnimationsFromSlicedTexture(assets);
        }
    }

    private void CreateAnimationsFromSlicedTexture(Texture2D texture2D)
    {
        string texturePath = AssetDatabase.GetAssetPath(texture2D);
        UnityEngine.Object[] assets = AssetDatabase.LoadAllAssetsAtPath(texturePath);

        // 收集所有Sprite并按方向分类
        var directionSprites = ClassifySpritesByDirection(assets, texture2D);

        if (directionSprites.Count == 0)
        {
            EditorUtility.DisplayDialog("错误", "未找到有效的Sprite，请确保纹理已正确切割", "确定");
            return;
        }

        // 获取输出路径
        string characterName = "Role_" + texture2D.name.Split("_")[1];
        string outputPath = GetOutputPath();
        string outputPrefabPath = this.GetPrefabOutputPath();
        string characterFolder = Path.Combine(outputPath, characterName);
        string characterPrefabFolder = Path.Combine(outputPrefabPath, characterName);
        EnsureDirectoryExists(characterFolder);
        EnsureDirectoryExists(characterPrefabFolder);

        // 创建Animator Controller和动画
        AnimatorController controller = CreateAnimatorControllerWithAnimations(characterName, characterFolder, directionSprites);

        // 创建预览用的GameObject（可选）
        CreatePreviewObject(characterName, characterPrefabFolder, directionSprites, controller);

        AssetDatabase.Refresh();
        // EditorUtility.DisplayDialog("完成", $"已为 {characterName} 创建动画文件到: {characterFolder}", "确定");
    }

    private void CreateAnimationClipsOnly(Texture2D texture2D)
    {
        string texturePath = AssetDatabase.GetAssetPath(texture2D);
        UnityEngine.Object[] assets = AssetDatabase.LoadAllAssetsAtPath(texturePath);

        // 收集所有Sprite并按方向分类
        var directionSprites = ClassifySpritesByDirection(assets, texture2D);

        if (directionSprites.Count == 0) return;

        // 获取输出路径
        string characterName = "Role_" + texture2D.name.Split("_")[1];
        string outputPath = GetOutputPath();
        string characterFolder = Path.Combine(outputPath, characterName);
        EnsureDirectoryExists(characterFolder);

        // 仅为每个方向创建动画文件
        foreach (var direction in directionSprites)
        {
            CreateDirectionAnimation(direction.Key, direction.Value, characterFolder, characterName);
        }

        AssetDatabase.Refresh();
        EditorUtility.DisplayDialog("完成", $"已创建 {directionSprites.Count} 个动画文件到: {characterFolder}", "确定");
    }

    private Dictionary<string, List<Sprite>> ClassifySpritesByDirection(UnityEngine.Object[] assets, Texture2D texture2D)
    {
        Dictionary<string, List<Sprite>> directionSprites = new Dictionary<string, List<Sprite>>();

        // 常见的命名模式
        string[] directionPatterns = { "down", "up", "left", "right", "d", "u", "l", "r", "s", "n", "w", "e" };
        string[] directionNames = { "Down", "Up", "Left", "Right" };

        foreach (UnityEngine.Object asset in assets)
        {
            if (asset is Sprite sprite && asset.name != texture2D.name)
            {
                string spriteName = sprite.name.ToLower();
                string foundDirection = null;

                // 检查常见的命名模式
                // for (int i = 0; i < directionPatterns.Length; i++)
                // {
                //     if (spriteName.Contains(directionPatterns[i]))
                //     {
                //         foundDirection = directionNames[i % 4]; // 映射到标准方向名
                //         break;
                //     }
                // }
                spriteName = spriteName.Split("_")[2];
                // 如果没找到模式，尝试通过数字序号判断
                {
                    // 假设命名格式为: name_0, name_1, name_2, name_3 等
                    if (spriteName.Equals("0") || spriteName.Equals("1") || spriteName.Equals("2") || spriteName.Equals("3"))
                        foundDirection = "Down";
                    else if (spriteName.Equals("12") || spriteName.Equals("13") || spriteName.Equals("14") || spriteName.Equals("15"))
                        foundDirection = "Up";
                    else if (spriteName.Equals("4") || spriteName.Equals("5") || spriteName.Equals("6") || spriteName.Equals("7"))
                        foundDirection = "Left";
                    else if (spriteName.Equals("8") || spriteName.Equals("9") || spriteName.Equals("10") || spriteName.Equals("11"))
                        foundDirection = "Right";
                }

                if (foundDirection != null)
                {
                    if (!directionSprites.ContainsKey(foundDirection))
                        directionSprites[foundDirection] = new List<Sprite>();

                    directionSprites[foundDirection].Add(sprite);
                }
            }
        }

        // 为每个方向的Sprite排序
        foreach (var direction in directionSprites.Keys.ToList())
        {
            directionSprites[direction] = directionSprites[direction]
                    .OrderBy(s => s.name)
                    .ToList();
        }

        return directionSprites;
    }

    private AnimatorController CreateAnimatorControllerWithAnimations(string characterName, string outputPath,
    Dictionary<string, List<Sprite>> directionSprites)
    {
        // 创建Animator Controller
        AnimatorController controller =
                AnimatorController.CreateAnimatorControllerAtPath(Path.Combine(outputPath, $"{characterName}_Controller.controller"));

        // 移除默认状态并添加新图层
        // controller.RemoveLayer(0);
        // var layer = controller.AddLayer("Base Layer");
        var layer = controller.layers[0];
        var blendTreeState = controller.CreateBlendTreeInController("Movement", out var blendTree);

        // 创建Blend Tree
        // var blendTree = new BlendTree();
        // blendTree.name = "MovementBlend";
        blendTree.blendType = BlendTreeType.SimpleDirectional2D;
        blendTree.blendParameter = "MoveX";
        blendTree.blendParameterY = "MoveY";

        // 为每个方向创建动画并添加到Blend Tree
        foreach (var direction in directionSprites)
        {
            AnimationClip clip = CreateDirectionAnimation(direction.Key, direction.Value, outputPath, characterName);
            if (clip != null)
            {
                Vector2 position = GetDirectionPosition(direction.Key);
                blendTree.AddChild(clip, position);
            }
        }

        // 添加Blend Tree到控制器
        // var blendTreeState = layer.stateMachine.AddState("Movement", 0, 0);
        // blendTreeState.motion = blendTree;
        layer.stateMachine.defaultState = blendTreeState;

        // 添加参数
        controller.RemoveParameter(0);
        controller.AddParameter("MoveX", AnimatorControllerParameterType.Float);
        controller.AddParameter("MoveY", AnimatorControllerParameterType.Float);

        return controller;
    }

    private AnimationClip CreateDirectionAnimation(string direction, List<Sprite> sprites, string outputPath, string characterName)
    {
        if (sprites.Count == 0) return null;

        // 创建Animation Clip
        AnimationClip clip = new AnimationClip();
        clip.name = $"{characterName}_{direction}";
        clip.frameRate = frameRate;

        // 创建Sprite动画曲线
        EditorCurveBinding spriteBinding = new EditorCurveBinding
        {
            type = typeof(SpriteRenderer),
            path = "",
            propertyName = "m_Sprite"
        };

        ObjectReferenceKeyframe[] keyframes = new ObjectReferenceKeyframe[sprites.Count];
        for (int i = 0; i < sprites.Count; i++)
        {
            keyframes[i] = new ObjectReferenceKeyframe
            {
                time = i / frameRate,
                value = sprites[i]
            };
        }

        AnimationUtility.SetObjectReferenceCurve(clip, spriteBinding, keyframes);

        // 设置循环
        AnimationClipSettings settings = AnimationUtility.GetAnimationClipSettings(clip);
        settings.loopTime = true;
        AnimationUtility.SetAnimationClipSettings(clip, settings);

        // 保存动画文件
        string clipPath = Path.Combine(outputPath, $"{clip.name}.anim");
        AssetDatabase.CreateAsset(clip, clipPath);

        return clip;
    }

    private Vector2 GetDirectionPosition(string direction)
    {
        switch (direction.ToLower())
        {
            case "down": return new Vector2(0, -1);
            case "up": return new Vector2(0, 1);
            case "left": return new Vector2(-1, 0);
            case "right": return new Vector2(1, 0);
            default: return Vector2.zero;
        }
    }

    private void CreatePreviewObject(string characterName, string outputPath, Dictionary<string, List<Sprite>> directionSprites, AnimatorController controller)
    {
        // 创建预览用的GameObject
        GameObject previewObject = new GameObject(characterName + "_Preview");
        SpriteRenderer spriteRenderer = previewObject.AddComponent<SpriteRenderer>();
        Animator animator = previewObject.AddComponent<Animator>();

        // 设置默认Sprite
        if (directionSprites.ContainsKey("Down") && directionSprites["Down"].Count > 0)
        {
            spriteRenderer.sprite = directionSprites["Down"][1];
        }

        // 设置Animator Controller
        animator.runtimeAnimatorController = controller;

        // 保存为预制体（可选）
        string prefabPath = Path.Combine(outputPath, $"{characterName}.prefab");
        PrefabUtility.SaveAsPrefabAsset(previewObject, prefabPath);

        DestroyImmediate(previewObject);
    }

    private void EnsureDirectoryExists(string path)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        else
        {
            Directory.Delete(path, true);
            Directory.CreateDirectory(path);
        }
    }
}