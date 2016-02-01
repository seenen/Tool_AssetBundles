using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace AssetBundleEditor
{
    public class ABWindow : EditorWindow
    {
        [MenuItem("Tool/AssetBundles", false)]
        public static void ABWindowMain()
        {
            thisWindow = (ABWindow)EditorWindow.GetWindow(typeof(ABWindow));
            thisWindow.title = "BundleWindow";
            thisWindow.Show();
        }
        static ABWindow thisWindow;

        ABMgr m_BundleMgr;

        void Load()
        {
            if (listDatas == null)
                listDatas = new List<string>();

            if (m_BundleMgr == null)
                m_BundleMgr = new ABMgr();

        }

        void Unload()
        {
            Caching.CleanCache();

            if (listDatas != null) { listDatas.Clear(); listDatas = null; }
            if (m_BundleMgr != null) { m_BundleMgr.Close(); m_BundleMgr = null; }
        }

        string platformSel;

        List<string> listDatas = new List<string>();

        public GUISkin skin;

        Vector2 scrollPos = Vector2.zero;

        void OnGUI()
        {
            if (skin == null)
                skin = new GUISkin();

            GUILayout.Box("Zhang Shining Publish at 2013-12-27 17:35", skin.FindStyle("Box"), GUILayout.ExpandWidth(false));

            BundleObject();

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
                    thisWindow.m_BundleMgr.CreateAssetBunldesALL();
                }

                if (GUILayout.Button("Pack More Bundles", GUILayout.ExpandWidth(true)))
                {
                    thisWindow.m_BundleMgr.ExecCreateAssetBunldes();
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

        void BundleScene()
        {
            //GUILayout.Box("Pack Scene Bundle To(/AssetBunldes/Output/*.*)", GUILayout.ExpandWidth(true));

            //GUILayout.BeginHorizontal();
            //GUILayout.Label("\t", GUILayout.ExpandWidth(false));
            //if (GUILayout.Button("Pack A Scene Bundle(Scene.assetsbundle)", GUILayout.ExpandWidth(true)))
            //{
            //    thisWindow.m_BundleMgr.CreateAssetBundlesScene();
            //}
            //GUILayout.EndHorizontal();
        }

        void BundleShow()
        {
            scrollPos = GUILayout.BeginScrollView(scrollPos, GUILayout.ExpandWidth(true));

            foreach (string e in listDatas)
            {
                GUILayout.Label(e, GUILayout.ExpandWidth(true));
            }

            GUILayout.EndScrollView();
        }

        void OnInspectorUpdate() { Repaint(); }

        void OnFocus() { Load(); }

        void OnDisable() { Unload(); }
    }
}