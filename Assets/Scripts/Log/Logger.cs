using System;
using System.IO;
using UnityEngine;

public class Logger
{
    public static string LogFileName = Application.dataPath + "/simulation.log";

    public static void Log(string logLevel, string logText)
    {
        using (StreamWriter writer = File.AppendText(LogFileName))
        {
            string logTime = DateTime.Now.ToString("yyyy.MM.dd HHHH:mm:ss:fff");
            writer.WriteLine("[" + logLevel + "][" + logTime + "]::" + logText);
        }
    }
    
    
    
}