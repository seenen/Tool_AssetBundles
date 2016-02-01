using UnityEngine;
using System.Collections;
using System;
using System.Linq;

using AssetBundleEditor;
using CmdEditor;
using System.IO;
using UnityEditor;

class Main
{
    public static string mInFolder;
    public static string mOutFolder;

    public static LogFile _Log = new LogFile();

    /// <summary>
    /// 自启动，在编辑器中使用 
    /// </summary>
    /// <param name="path"></param>
    public static void EntrySelf(string srcfolderpath, string destfolderpath)
    {
        mInFolder = srcfolderpath.Replace("\\", "/");
        mOutFolder = destfolderpath.Replace("\\", "/"); 

        EntryPoint();
    }

    /// <summary>
    /// -batchmode 的方式启动
    /// </summary>
    public static void EntryCommand()
    {
        mInFolder = CommandLineReader.GetCustomArgument("InFolder").Replace("\\", "/");
        mOutFolder = CommandLineReader.GetCustomArgument("OutFolder").Replace("\\", "/");

        EntryPoint();
    }

    static string LogFile;

    static void  HandleLog (string logString, string stackTrace, LogType type) 
    {
        //  Log
        _Log.Write(LogFile, logString, type);

	}

    static void EntryPoint()
    {
        //  if not exist,then create
        if (!Directory.Exists(mOutFolder))
            Directory.CreateDirectory(mOutFolder);

        LogFile = mOutFolder + "/log.txt";

        Application.RegisterLogCallback(HandleLog);

        Debug.Log("LogFile ->" + LogFile);

        //  输出文件夹
        Debug.Log("mOutFolder ->" + mOutFolder);

        //  输入文件夹
        Debug.Log("InFolder ->" + mInFolder);

        if (!Directory.Exists(mInFolder))
        {
            Debug.LogError(mInFolder);

            return;
        }

        if (!mInFolder.Contains("Assets"))
        {
            Debug.LogError(mInFolder + " No Assets");

            return;
        }

        //  转换
        Transmit();
    }

    static void Transmit()
    {

        ABMgr mgr = new ABMgr();

        DirectoryInfo di = new DirectoryInfo(mInFolder);

        foreach (FileInfo fi in di.GetFiles())
        {
            //  判断
            if (fi.FullName.EndsWith(".meta"))
                continue;

            if (fi.FullName.EndsWith(ABConfig.extensionName))
                continue;

            string fifullname = fi.FullName.Replace("\\", "/");

            int index = fifullname.IndexOf("Assets/");

            if (index == -1)
            {
                Debug.LogError(fifullname + " No Assets");

                continue;
            }

            //  输出字符串拼接 
            string fifolder = Path.GetDirectoryName(fi.FullName).Replace("\\", "/");

            int childfolderindex = fifullname.IndexOf(mInFolder, 0);

            if (childfolderindex == -1)
            {
                Debug.LogError(childfolderindex + " [between] " + fifullname + " [and] " + mInFolder);

                continue;
            }

            string outfolder = mOutFolder + fifolder.Substring(mInFolder.Length);

            //Debug.Log("outfolder " + outfolder);

            //  输入资源 
            string assefullname = fifullname.Substring(index);

            UnityEngine.Object obj = AssetDatabase.LoadAssetAtPath(assefullname, typeof(UnityEngine.Object));

            if (obj == null)
            {
                Debug.LogError(assefullname + " [LoadAssetAtPath Failed] ");

                continue;
            }

            //  输出资源 
            string targetPath = outfolder + "/" + Path.GetFileNameWithoutExtension(fifullname) + ABConfig.extensionName;

            //Debug.Log("targetPath " + targetPath);

            bool suc = mgr.CreateAssetBundleOne(obj, targetPath);

            if (!suc)
            {
                Debug.LogError(fifullname + " [CreateAssetBundleOne Failed] " + targetPath);

                continue;

            }
            else
                Debug.Log("Create " + targetPath);

        }
    }
}
