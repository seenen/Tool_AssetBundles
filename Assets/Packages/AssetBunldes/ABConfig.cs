using UnityEngine;

namespace AssetBundleEditor
{

    public sealed class ABConfig : MonoBehaviour
    {
        public static string OutputFolderWindows32 = Application.dataPath + "/StreamingAssets/PC/res/";
        public static string OutputFolderIPhone = Application.dataPath + "/StreamingAssets/IOS/res/";

        public static string extensionName = ".assetBundles";

        public static string OutputFolderSceneWindows = Application.dataPath + "/StreamingAssets/PC/res/";
        public static string OutputFolderSceneIOS = Application.dataPath + "/StreamingAssets/IOS/res/";
    }

    public enum BundlePlatform
    {
        PC = 0,
        IOS,
    }

    public class SceneData
    {
        public GameObject scene;
        public GameObject scene_nav;
        public Texture2D scene_Lightmap_high;
        public Texture2D scene_Lightmap_low;

        public bool IsValid(out string errorcode)
        {
            errorcode = "no error";

            if (!scene)
            {
                errorcode = "no scene";
                return false;
            }

            if (!scene_nav || 
                !scene_nav.name.EndsWith("_nav") ||
                !scene_nav.name.StartsWith(scene.name))
            {
                errorcode = "no scene_nav";
                return false;
            }

            if (!scene_Lightmap_high || 
                !scene_Lightmap_high.name.Contains("high") ||
                !scene_Lightmap_high.name.StartsWith("LightmapFar-")
                )
            {
                errorcode = "no scene_Lightmap_high";
                return false;
            }

            if (!scene_Lightmap_low ||
                !scene_Lightmap_low.name.Contains("low") ||
                !scene_Lightmap_low.name.StartsWith("LightmapFar-")
                )
            {
                errorcode = "no scene_Lightmap_low";
                return false;
            }

            return true;
        }
    }
}