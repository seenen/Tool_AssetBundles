using UnityEngine;
using System.Collections;
using UnityEditor;

public class ABBatchWindow : EditorWindow 
{
    [MenuItem("Tool/ABBatchWindow", false)]
    public static void ABBatch()
    {
#if UNITY_IPHONE || UNITY_STANDALONE_OSX
        string InputPath = "/project/HeroeSource/GameEditors/Tool_AssetBundles/Assets/2dsound";
        string OutputPath = "/project/TestOut";
#elif UNITY_STANDALONE_WIN
        string InputPath = "E:/Heros/GameEditors/Tool_AssetBundles/Assets/TestScene/110101/Prefabs";
        string OutputPath = "E:/Heros/GameEditors/Tool_AssetBundles/Assets/TestScene/110101/Prefabs_Bunldes";
#endif	//	UNITY_STANDALONE_WIN

        Main.EntrySelf(InputPath, OutputPath);
    }
}
