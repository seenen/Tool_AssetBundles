using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace AssetBundleEditor
{
    public class ABSceneWindow : EditorWindow
    {
        [MenuItem("Tool/ABSceneWindow", false)]
        public static void Main()
        {
            thisWindow = (ABSceneWindow)EditorWindow.GetWindow(typeof(ABSceneWindow));
            thisWindow.title = "ABSceneWindow";
            thisWindow.Show();
        }
        static ABSceneWindow thisWindow;

        static SceneData thisSceneData = new SceneData();

        static ABMgr m_BundleMgr = new ABMgr();

        void Load()
        {
            if (listDatas == null)
                listDatas = new List<string>();

            //if (m_BundleMgr == null)
            //    m_BundleMgr = new ABMgr();

        }

        void Unload()
        {
            Caching.CleanCache();

            if (listDatas != null) { listDatas.Clear(); listDatas = null; }
            if (m_BundleMgr != null) { m_BundleMgr.Close(); m_BundleMgr = null; }

            thisSceneData = new SceneData();

            objScene = null;
        }

        string platformSel;

        List<string> listDatas = new List<string>();

        public GUISkin skin;

        Vector2 scrollPos = Vector2.zero;

        void OnGUI()
        {
            if (skin == null)
                skin = new GUISkin();

            BundleScene();

            BundleShow();

        }

        void BundleObject()
        {
            GUILayout.Box("Pack Bundle To(/AssetBunldes/Output/*.*)", GUILayout.ExpandWidth(true));

            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("\t", GUILayout.ExpandWidth(false));
                if (GUILayout.Button("Pack A Bundle(ALL.assetsbundle)", GUILayout.ExpandWidth(true)))
                {
                    m_BundleMgr.CreateAssetBunldesALL();
                }

                if (GUILayout.Button("Pack More Bundles", GUILayout.ExpandWidth(true)))
                {
                    m_BundleMgr.ExecCreateAssetBunldes();
                }
            }
            GUILayout.EndHorizontal();

            GUILayout.Box("Unpack Bundle From(/AssetBunldes/Output/*.*)", GUILayout.ExpandWidth(true));// .Width(300));

            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("\t", GUILayout.ExpandWidth(false));

                if (GUILayout.Button("Unpack Selection AssetsBundles", GUILayout.ExpandWidth(true)))
                {
                    List<string> fullnames = new List<string>();

                    foreach (Object e in Selection.objects)
                    {
                        //  sub "Assets"
                        string prefixpath = Application.dataPath.Substring(0, Application.dataPath.Length - 6);

                        string path = prefixpath + AssetDatabase.GetAssetOrScenePath(e);

                        fullnames.Add(path);
                    }
                    listDatas = m_BundleMgr.Reload(fullnames);
                }
            }
            GUILayout.EndHorizontal();
        }

        Object objScene;

        void BundleScene()
        {
            thisSceneData.scene = EditorGUILayout.ObjectField("Scenes Prefab", thisSceneData.scene, typeof(GameObject), true, GUILayout.ExpandWidth(true)) as GameObject;

            thisSceneData.scene_nav = EditorGUILayout.ObjectField("Scenes Nav Prefab", thisSceneData.scene_nav, typeof(GameObject), true, GUILayout.ExpandWidth(true)) as GameObject;

            thisSceneData.scene_Lightmap_high = EditorGUILayout.ObjectField("Scenes LightMap High Texture", thisSceneData.scene_Lightmap_high, typeof(Texture2D), true, GUILayout.ExpandWidth(true)) as Texture2D;

            thisSceneData.scene_Lightmap_low = EditorGUILayout.ObjectField("Scenes LightMap Low Texture", thisSceneData.scene_Lightmap_low, typeof(Texture2D), true, GUILayout.ExpandWidth(true)) as Texture2D;

            GUILayout.Box("Pack Scene Bundle To(/AssetBunldes/Output/*.*)", GUILayout.ExpandWidth(true));

            GUILayout.BeginHorizontal();
            GUILayout.Label("\t", GUILayout.ExpandWidth(false));
            if (GUILayout.Button("Pack A Scene Bundle(Scene.assetsbundle)", GUILayout.ExpandWidth(true)))
            {
                string errorcode;

                if (thisSceneData == null ||
                    !thisSceneData.IsValid(out errorcode))
                {
                    EditorUtility.DisplayDialog("AssetBundle", errorcode, "Close");

                    return;
                }

                string obj = m_BundleMgr.CreateAssetBundlesScene(thisSceneData);

                obj = obj.Substring(obj.IndexOf("Assets/"));

                Debug.Log(obj);

                objScene = AssetDatabase.LoadAssetAtPath(obj, typeof(Object));

                UpdateBundle(new Object[1] { objScene });
                //objScene = AssetDatabase.LoadAssetAtPath("Assets/Packages/AssetBunldes/Output/Scene/110101.assetBundles", typeof(Object));
            }
            GUILayout.EndHorizontal();

            objScene = EditorGUILayout.ObjectField("obj", objScene, typeof(Object), true, GUILayout.ExpandWidth(true));
        }

        void BundleShow()
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("\t", GUILayout.ExpandWidth(false));

                if (GUILayout.Button("Unpack Selection AssetsBundles", GUILayout.ExpandWidth(true)))
                {
                    UpdateBundle(Selection.objects);
                }
            }
            GUILayout.EndHorizontal();

            scrollPos = GUILayout.BeginScrollView(scrollPos, GUILayout.ExpandWidth(true));

            foreach (string e in listDatas)
            {
                GUILayout.Label(e, GUILayout.ExpandWidth(true));
            }

            GUILayout.EndScrollView();
        }

        void UpdateBundle(Object[] objects)
        {
            List<string> fullnames = new List<string>();

            foreach (Object e in objects)
            {
                //  sub "Assets"
                string prefixpath = Application.dataPath.Substring(0, Application.dataPath.Length - 6);

                string path = prefixpath + AssetDatabase.GetAssetOrScenePath(e);

                fullnames.Add(path);
            }
            listDatas = m_BundleMgr.Reload(fullnames);

        }

        void OnInspectorUpdate() { Repaint(); }

        void OnFocus() { Load(); }

        void OnDisable() { Unload(); }
    }
}