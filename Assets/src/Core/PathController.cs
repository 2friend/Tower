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

    private const string PATH_OBJECT_VAR = "pathPoint";


    public static List<WavePath> paths = new List<WavePath>();

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
        paths.Clear();

        foreach (XmlNode roadNode in roadNodes)
        {
            int roadId = int.Parse(roadNode.Attributes[PATH_ID_ATTRIBUTE_VAR].Value);
            WavePath pathInstance = new WavePath(roadId);
            
            XmlNodeList pathPoints = roadNode.SelectNodes(PATH_OBJECT_VAR);

            foreach (XmlNode pointNode in pathPoints)
            {
                int x = int.Parse(pointNode.Attributes["x"].Value);
                int y = int.Parse(pointNode.Attributes["y"].Value);
                PathPoint pathPoint = new PathPoint(x, y);
                pathInstance.pathPoints.Add(pathPoint);
            }
            paths.Add(pathInstance);
            Debug.Log("[Core] [Paths] Readed new Path ID: %" + pathInstance.id + "% with count of points: %" + pathInstance.pathPoints.Count + "%");
        }

    }
}


public class WavePath
{
    public int id;
    public List<PathPoint> pathPoints = new List<PathPoint>();

    public WavePath(int _id)
    {
        id = _id;
    }
}

public class PathPoint
{
    public int x;
    public int y;

    public PathPoint(int _x, int _y)
    {
        x = _x;
        y = _y;
    }
}
