using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.IO;
using System;
using TMPro;
using System.Linq;



public class PathController : MonoBehaviour
{

    private const string FOLDER_PATH = "Data";
    private const string PATH_FILE_PATH = "Paths";
    private const string PATH_ID_ATTRIBUTE_VAR = "id";

    private const string PATH_OBJECT_VAR = "path";


     public List<Path> path = new List<Path>();

    void Start()
    {
        ReadPathFile();  
    }
 private void ReadPathFile()
{
    TextAsset xmlFile = Resources.Load<TextAsset>(FOLDER_PATH + "/" + PATH_FILE_PATH);
    XmlDocument xmlDoc = new XmlDocument();
    xmlDoc.LoadXml(xmlFile.text);

    XmlNodeList roadNodes = xmlDoc.SelectNodes("//road");
    path.Clear();

    foreach (XmlNode roadNode in roadNodes)
    {
        int roadId = int.Parse(roadNode.Attributes["id"].Value);
        Path pathInstance = new Path(roadId);

        XmlNodeList pathPoints = roadNode.SelectNodes("pathPoint");
        
        foreach (XmlNode pointNode in pathPoints)
        {
            int x = int.Parse(pointNode.Attributes["x"].Value);
            int y = int.Parse(pointNode.Attributes["y"].Value);
        }

        path.Add(pathInstance);
    }

    Debug.Log("[Core] [Path] Reading Finished" + path + path.x + path.y);
}
}


public class Path
{
    public int id;
    public Dictionary<int, Dictionary<EnemyBD, int>> enemysPerWave = new Dictionary<int, Dictionary<EnemyBD, int>>();

    public Path(int _id)
    {
        id = _id;
    }
}

