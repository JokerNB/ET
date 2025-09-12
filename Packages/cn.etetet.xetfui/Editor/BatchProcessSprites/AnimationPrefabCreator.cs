using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using UnityEditor.Animations;

public class AnimationPrefabCreator : EditorWindow
{
    private string sourceFolderPath = "Assets/Resources/Characters";
    private string outputPath = "Assets/Prefabs/Characters";
    private float frameRate = 12f;
    private bool createAnimatorController = true;
    private bool createPrefabs = true;

    [MenuItem("ET/XET/批量创建四方向动画和预制体")]
    public static void ShowWindow()
    {
        GetWindow<AnimationPrefabCreator>("动画批量创建工具");
    }

    void OnGUI()
    {
        GUILayout.Label("四方向动画批量创建工具", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        sourceFolderPath = EditorGUILayout.TextField("资源文件夹路径", sourceFolderPath);
        outputPath = EditorGUILayout.TextField("输出路径", outputPath);
        frameRate = EditorGUILayout.FloatField("帧率", frameRate);
        createAnimatorController = EditorGUILayout.Toggle("创建Animator控制器", createAnimatorController);
        createPrefabs = EditorGUILayout.Toggle("创建预制体", createPrefabs);

        EditorGUILayout.Space();

        if (GUILayout.Button("开始批量处理"))
        {
            ProcessAllCharacters();
        }

        if (GUILayout.Button("打开资源文件夹"))
        {
            EditorUtility.RevealInFinder(sourceFolderPath);
        }

        if (GUILayout.Button("打开输出文件夹"))
        {
            EditorUtility.RevealInFinder(outputPath);
        }
    }

    private void ProcessAllCharacters()
    {
        if (!Directory.Exists(sourceFolderPath))
        {
            Debug.LogError($"资源文件夹不存在: {sourceFolderPath}");
            return;
        }

        // 创建输出目录
        EnsureDirectoryExists(outputPath);

        // 获取所有角色文件夹
        string[] characterFolders = Directory.GetDirectories(sourceFolderPath);
        
        foreach (string characterFolder in characterFolders)
        {
            string characterName = Path.GetFileName(characterFolder);
            Debug.Log($"处理角色: {characterName}");
            
            ProcessCharacter(characterFolder, characterName);
        }

        Debug.Log("批量处理完成！");
        AssetDatabase.Refresh();
    }

    private void ProcessCharacter(string characterFolder, string characterName)
    {
        // 检查四个方向文件夹是否存在
        string[] directions = { "Down", "Up", "Left", "Right" };
        foreach (string direction in directions)
        {
            string directionPath = Path.Combine(characterFolder, direction);
            if (!Directory.Exists(directionPath))
            {
                Debug.LogWarning($"角色 {characterName} 缺少 {direction} 方向文件夹");
                return;
            }
        }

        // 创建角色专属的输出文件夹
        string characterOutputPath = Path.Combine(outputPath, characterName);
        EnsureDirectoryExists(characterOutputPath);

        // 创建GameObject
        GameObject characterObject = new GameObject(characterName);
        SpriteRenderer spriteRenderer = characterObject.AddComponent<SpriteRenderer>();
        
        // 设置默认sprite（使用Down方向的第一帧）
        string downPath = Path.Combine(characterFolder, "Down");
        string[] downSprites = Directory.GetFiles(downPath, "*.png");
        if (downSprites.Length > 0)
        {
            Sprite defaultSprite = AssetDatabase.LoadAssetAtPath<Sprite>(downSprites[0]);
            spriteRenderer.sprite = defaultSprite;
        }

        // 添加Animator组件
        Animator animator = characterObject.AddComponent<Animator>();

        // 创建Animator Controller
        RuntimeAnimatorController controller = null;
        if (createAnimatorController)
        {
            controller = CreateAnimatorController(characterFolder, characterName, characterOutputPath);
            animator.runtimeAnimatorController = controller;
        }

        // 创建预制体
        if (createPrefabs)
        {
            CreatePrefab(characterObject, characterName, characterOutputPath);
        }

        // 清理临时对象
        DestroyImmediate(characterObject);
    }

    private RuntimeAnimatorController CreateAnimatorController(string characterFolder, string characterName, string outputPath)
    {
        // 创建Animator Controller
        AnimatorController controller = AnimatorController.CreateAnimatorControllerAtPath(
            Path.Combine(outputPath, $"{characterName}_Controller.controller")
        );

        // 移除默认状态
        // controller.RemoveLayer(0);

        // 创建新图层
        var layer = controller.layers[0];
        // var layer = controller.AddLayer("Base Layer");

        // 创建Blend Tree
        var blendTree = new BlendTree();
        blendTree.name = "MovementBlend";
        blendTree.blendType = BlendTreeType.SimpleDirectional2D;
        blendTree.blendParameter = "MoveX";
        blendTree.blendParameterY = "MoveY";

        // 创建四个方向的动画
        CreateDirectionAnimation("Down", characterFolder, "Down", outputPath, blendTree, new Vector2(0, -1));
        CreateDirectionAnimation("Up", characterFolder, "Up", outputPath, blendTree, new Vector2(0, 1));
        CreateDirectionAnimation("Left", characterFolder, "Left", outputPath, blendTree, new Vector2(-1, 0));
        CreateDirectionAnimation("Right", characterFolder, "Right", outputPath, blendTree, new Vector2(1, 0));

        // 添加Blend Tree到控制器
        var blendTreeState = layer.stateMachine.AddState("Movement", Vector3.zero);
        blendTreeState.motion = blendTree;

        // 设置默认状态
        layer.stateMachine.defaultState = blendTreeState;

        // 创建参数
        controller.AddParameter("MoveX", AnimatorControllerParameterType.Float);
        controller.AddParameter("MoveY", AnimatorControllerParameterType.Float);
        controller.AddParameter("IsMoving", AnimatorControllerParameterType.Bool);

        return controller;
    }

    private void CreateDirectionAnimation(string animName, string characterFolder, string direction, string outputPath, BlendTree blendTree, Vector2 position)
    {
        string directionPath = Path.Combine(characterFolder, direction);
        string[] spriteFiles = Directory.GetFiles(directionPath, "*.png");
        
        if (spriteFiles.Length == 0)
        {
            Debug.LogWarning($"方向 {direction} 没有找到图片文件");
            return;
        }

        // 排序文件
        System.Array.Sort(spriteFiles);

        // 创建Animation Clip
        AnimationClip clip = new AnimationClip();
        clip.name = $"{Path.GetFileName(characterFolder)}_{animName}";
        clip.frameRate = frameRate;

        // 创建动画曲线
        EditorCurveBinding spriteBinding = new EditorCurveBinding
        {
            type = typeof(SpriteRenderer),
            path = "",
            propertyName = "m_Sprite"
        };

        ObjectReferenceKeyframe[] keyframes = new ObjectReferenceKeyframe[spriteFiles.Length];
        for (int i = 0; i < spriteFiles.Length; i++)
        {
            keyframes[i] = new ObjectReferenceKeyframe
            {
                time = i / frameRate,
                value = AssetDatabase.LoadAssetAtPath<Sprite>(spriteFiles[i])
            };
        }

        // 设置动画曲线
        AnimationUtility.SetObjectReferenceCurve(clip, spriteBinding, keyframes);

        // 设置循环
        AnimationClipSettings settings = AnimationUtility.GetAnimationClipSettings(clip);
        settings.loopTime = true;
        AnimationUtility.SetAnimationClipSettings(clip, settings);

        // 保存Animation Clip
        string clipPath = Path.Combine(outputPath, $"{clip.name}.anim");
        AssetDatabase.CreateAsset(clip, clipPath);

        // 添加到Blend Tree
        blendTree.AddChild(clip, position);
    }

    private void CreatePrefab(GameObject characterObject, string characterName, string outputPath)
    {
        string prefabPath = Path.Combine(outputPath, $"{characterName}.prefab");
        PrefabUtility.SaveAsPrefabAsset(characterObject, prefabPath);
        Debug.Log($"创建预制体: {prefabPath}");
    }

    private void EnsureDirectoryExists(string path)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
            AssetDatabase.Refresh();
        }
    }
}