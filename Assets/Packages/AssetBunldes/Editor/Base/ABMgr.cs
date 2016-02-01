using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

namespace AssetBundleEditor
{

    public class ABMgr
    {
#region WatchBundle
        public Dictionary<string, ABLoad> allbundleloads = new Dictionary<string, ABLoad>();

        /// <summary>
        /// 根据bundle的全路径，来加载bundles 
        /// </summary>
        /// <param name="bundlepathname"></param>
        /// <returns></returns>
        public Object[] LoadBundles(string bundlepathname)
        {
            ABLoad load = null;

            if (allbundleloads.ContainsKey(bundlepathname))
            {
                load = (ABLoad)allbundleloads[bundlepathname];

                Debug.Log(bundlepathname);

            }
            else
            {
                string targetPath = bundlepathname;

                // AssetBundle 存储路径
                Debug.Log(targetPath);

                load = new ABLoad(targetPath);

                allbundleloads[bundlepathname] = load;
            }

            Object[] all = load.LoadAll();

            return all;

        }

        public void Close()
        {
            foreach (ABLoad e in allbundleloads.Values)
            {
                e.Close();
            }
        }

        List<string> listDatas = new List<string>();

        public List<string> Reload(string sPathFolderName)
        {
            Close();
            listDatas.Clear();
#if !UNITY_WEBPLAYER
            //  查找所有文件
            DirectoryInfo diParent = new DirectoryInfo(sPathFolderName);
            foreach (FileInfo fi in diParent.GetFiles(".assetBundles"))
            {
                if (!fi.FullName.EndsWith(".assetBundles"))
                    continue;

                Object[] objects = LoadBundles("file://" + fi.FullName);

                listDatas.Add(Path.GetFileName(fi.FullName));

                foreach (Object e in objects)
                {
                    Debug.Log(e.name);

                    listDatas.Add("\t" + e.name + "\t[" + e.GetType() + "] ");
                }
            }
#else
            EditorUtility.DisplayDialog("AssetBunlde", "UNITY_WEBPLAYER is Forbidden", "Close");
#endif

            return listDatas;
        }

        public List<string> Reload(List<string> fullnames)
        {
            Close();
            listDatas.Clear();

            foreach (string fi in fullnames)
            {
                if (!fi.EndsWith(".assetBundles"))
                    continue;

                listDatas.Add(Path.GetFileName(fi));

                foreach (Object e in LoadBundles("file://" + fi))
                {
                    Debug.Log("file://" + fi + "   -> " + e.name);

                    listDatas.Add("\t" + e.name + "\t[" + e.GetType() + "] ");
                }
            }

            return listDatas;
        }
#endregion	

#region Create
        public void CreateAssetBunldesALL()
        {
            Caching.CleanCache();

            // AssetBundle 存储路径
#if UNITY_IPHONE || UNITY_STANDALONE_OSX
            string targetPath = ABConfig.OutputFolderIPhone + "ALL" + ABConfig.extensionName;
#elif UNITY_STANDALONE_WIN
            string targetPath = ABConfig.OutputFolderWindows32 + "ALL" + ABConfig.extensionName;
#endif	//	UNITY_STANDALONE_WIN

            Object[] SelectedAsset = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);

            AssetDatabase.Refresh();

            CreateAssetBunldesALL(SelectedAsset, targetPath);
        }

        public void ExecCreateAssetBunldes()
        {
            //取得在 Project 视图中选择的资源(包含子目录中的资源)
            Object[] SelectedAsset = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
            if (SelectedAsset == null)
            {
                EditorUtility.DisplayDialog("AssetBunlde", "Nothing is Selected", "Close");

                return;
            }

            foreach (Object obj in SelectedAsset)
            {
#if UNITY_IPHONE || UNITY_STANDALONE_OSX
            string targetPath = ABConfig.OutputFolderIPhone + obj.name + ABConfig.extensionName;
#elif UNITY_STANDALONE_WIN
            string targetPath = ABConfig.OutputFolderWindows32 + obj.name + ABConfig.extensionName;
#endif	//	UNITY_STANDALONE_WIN

                if (File.Exists(targetPath)) File.Delete(targetPath);

#if UNITY_IPHONE || UNITY_STANDALONE_OSX
                //建立 AssetBundle
                if (BuildPipeline.BuildAssetBundle(	obj, 
													null, 
													targetPath, 
													BuildAssetBundleOptions.CollectDependencies,// | BuildAssetBundleOptions.CompleteAssets, 
													BuildTarget.iPhone))
                {
                    Debug.Log(targetPath + "建立完成");
                }
                else
                {
                    Debug.LogError(obj.name + "建立失败");
                }
#elif UNITY_STANDALONE_WIN
                //建立 AssetBundle
                if (BuildPipeline.BuildAssetBundle(	obj, 
													null, 
													targetPath, 
													BuildAssetBundleOptions.CollectDependencies,// | BuildAssetBundleOptions.CompleteAssets, 
													BuildTarget.StandaloneWindows))
                {
                    Debug.Log(targetPath + "建立完成");

                }
                else
                {
                    Debug.LogError(obj.name + "建立失败");
                }
#endif	//	UNITY_STANDALONE_WIN
				AssetDatabase.Refresh();

                EditorUtility.DisplayDialog("AssetBunlde", "BuildTarget.StandaloneWindows Over, Look Red Log!", "Close");

            }

        }

        public bool CreateAssetBundleOne(Object obj, string targetPath)
        {
            Object[] all = new Object[1] { obj };

            return CreateAssetBunldesALL(all, targetPath);

        }
#endregion

        #region Scene
        public string CreateAssetBundlesScene(SceneData thisSceneData)
        {
            Caching.CleanCache();

            // AssetBundle 存储路径
#if UNITY_IPHONE || UNITY_STANDALONE_OSX
            string targetPath = ABConfig.OutputFolderSceneIOS +  thisSceneData.scene.name + ABConfig.extensionName;
#elif UNITY_STANDALONE_WIN
            string targetPath = ABConfig.OutputFolderSceneWindows + thisSceneData.scene.name + ABConfig.extensionName;
#endif	//	UNITY_STANDALONE_WIN

            AssetDatabase.Refresh();

            AssetDatabase.GetAssetPath((Object)thisSceneData.scene);
            AssetDatabase.GetAssetPath((Object)thisSceneData.scene_nav);
            AssetDatabase.GetAssetPath((Object)thisSceneData.scene_Lightmap_high);
            AssetDatabase.GetAssetPath((Object)thisSceneData.scene_Lightmap_low);

            Object[] SelectedAsset = new Object[4];
            SelectedAsset[0] = (Object)thisSceneData.scene;
            SelectedAsset[1] = (Object)thisSceneData.scene_nav;
            SelectedAsset[2] = (Object)thisSceneData.scene_Lightmap_high;
            SelectedAsset[3] = (Object)thisSceneData.scene_Lightmap_low;

            //Object[] SelectedAsset = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);

            CreateAssetBunldesALL(SelectedAsset, targetPath);

            return targetPath;
        }
        #endregion

        #region Inner
        bool CreateAssetBunldesALL(Object[] SelectedAsset, string targetPath)
        {

            if (SelectedAsset == null || SelectedAsset.Length == 0)
            {
                //EditorUtility.DisplayDialog("AssetBunlde", "Nothing is Selected", "Close");
                Debug.LogError("Nothing is Selected or targetPath is invalid");

                return false;
            }

            if (File.Exists(targetPath))
            {
                //if (EditorUtility.DisplayDialog("AssetBunlde", "Target is Exist, Is Delete?", "Delete", "No Delete"))
                {
                    File.Delete(targetPath);

                    AssetDatabase.Refresh();

                }
            }

#if UNITY_IPHONE || UNITY_STANDALONE_OSX
            if (BuildPipeline.BuildAssetBundle(	null, 
												SelectedAsset, 
												targetPath, 
												BuildAssetBundleOptions.CollectDependencies,// | BuildAssetBundleOptions.CompleteAssets, 
												BuildTarget.iPhone))
            {
                AssetDatabase.Refresh();

                return true;

            }
            else
            {
                AssetDatabase.Refresh();

                return false;

                //EditorUtility.DisplayDialog("AssetBunlde", "BuildTarget.iPhone Failed", "Close");
            }
#elif UNITY_STANDALONE_WIN
            if (BuildPipeline.BuildAssetBundle(null,
                                                SelectedAsset,
                                                targetPath,
                                                BuildAssetBundleOptions.CollectDependencies,// | BuildAssetBundleOptions.CompleteAssets, 
                                                BuildTarget.StandaloneWindows))
            {
                AssetDatabase.Refresh();

                return true;

            }
            else
            {
                AssetDatabase.Refresh();

                return false;

            }
#endif	//	UNITY_STANDALONE_WIN

        }
        #endregion
    }
}