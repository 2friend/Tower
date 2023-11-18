using System;
using System.IO;
using UnityEngine;

public class Logger : MonoBehaviour
{
    private string logFilePath;
    private string logsFolder;

    void Awake()
    {
        string projectRootPath = Directory.GetParent(Application.dataPath).FullName;

        logsFolder = Path.Combine(projectRootPath, "GameLogs");
        if (!Directory.Exists(logsFolder))
        {
            Directory.CreateDirectory(logsFolder);
        }

        string[] logFiles = Directory.GetFiles(logsFolder);

        if (logFiles.Length >= 5)
        {
            DeleteOldLogFiles(logFiles, 5);
        }

        string fileName = string.Format("log_{0:yyyyMMdd_HHmmss}_{1}.log", DateTime.Now, Application.version);

        logFilePath = Path.Combine(logsFolder, fileName);

        WriteLog("[!] GAME STARTED [!]\n");
    }

    void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    void HandleLog(string logText, string stackTrace, LogType type)
    {
        string logMessage = string.Format("{0}: {1}\n", DateTime.Now, logText);

        WriteLog(logMessage);
    }

    void WriteLog(string logMessage)
    {
        using (StreamWriter writer = new StreamWriter(logFilePath, true))
        {
            writer.Write(logMessage);
        }
    }

    void DeleteOldLogFiles(string[] files, int keepCount)
    {
        Array.Sort(files, (a, b) => File.GetCreationTime(b).CompareTo(File.GetCreationTime(a)));

        for (int i = keepCount; i < files.Length; i++)
        {
            File.Delete(files[i]);
        }
    }
}
