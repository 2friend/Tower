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
    int roadId = int.Parse(roadNode.Attributes[PATH_ID_ATTRIBUTE_VAR].Value);

    XmlNodeList pathPoints = roadNode.SelectNodes("pathPoint");
    
    foreach (XmlNode pointNode in pathPoints)
    {
        int x = int.Parse(pointNode.Attributes["x"].Value);
        int y = int.Parse(pointNode.Attributes["y"].Value);

        Path pathInstance = new Path(roadId, x, y);
        path.Add(pathInstance);
    }
}

    Debug.Log("[Core] [Path] Reading Finished. Paths count: " + path.Count);

}
}


public class Path
{
    public int id;
    public int x; 
    public int y; 
    public Dictionary<int, Dictionary<EnemyBD, int>> enemysPerWave = new Dictionary<int, Dictionary<EnemyBD, int>>();

    public Path(int _id, int _x, int _y) 
    {
        id = _id;
        x = _x;
        y = _y;
    }
}
