using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Collections.Generic;

public class LogFile
{
    static private Dictionary<string, bool> LogFileDic = new Dictionary<string, bool>();

    //  写文件 
    string WriteFile(string logfilename)
    {
#if UNITY_IPHONE || UNITY_ANDROID
        return "";
#endif

        if (LogFileDic.ContainsKey(logfilename))
            return logfilename;

        FileInfo TheFile = new FileInfo(logfilename);   
        if (TheFile.Exists)   
            TheFile.Delete();

        StreamWriter fileWriter = File.CreateText(logfilename); 
        fileWriter.Close();

        LogFileDic.Add(logfilename, true);

        return logfilename;
    }

    public void Write(string logfilename, string log, LogType lt)
    {
#if UNITY_IPHONE || UNITY_ANDROID
        return;
#endif
        string filePathName = WriteFile(logfilename);

        FileStream fs = new FileStream(filePathName, FileMode.Append);
        StreamWriter sw = new StreamWriter(fs);
        //开始写入 
        sw.WriteLine("");
        //
        string str = "[";
        str += System.DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");//默认当天时间。
        str += "]";
        str += "\t";
        str += lt.ToString();
        str += "\t";
        str += log;

        sw.Write(str);
        //清空缓冲区 
        sw.Flush();
        //关闭流 
        sw.Close();
        fs.Close();
    }
 }
