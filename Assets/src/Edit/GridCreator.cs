using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.IO;
using TMPro;
using System;

public class GridCreator : MonoBehaviour
{
    private const string FOLDEDR_PATH = "Assets\\Resources\\Data";
    private const string PATH_FILE_PATH = "Paths";
    public List<Transform> path = new List<Transform>();
    public TMP_InputField idText;

    public bool canBePressed;

    public int pathId;

    private void Start()
    {
        canBePressed = true;
    }

    public void SaveGrid()
    {
        string filePath = FOLDEDR_PATH + "/" + PATH_FILE_PATH + ".xml";

        XmlDocument doc = new XmlDocument();
        if (File.Exists(filePath))
        {
            doc.Load(filePath);
        }
        else
        {
            XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            XmlElement root = doc.CreateElement("root");
            doc.AppendChild(xmlDeclaration);
            doc.AppendChild(root);
        }

        string pathIdString = pathId.ToString();
        XmlNode existingRoad = doc.SelectSingleNode($"//road[@id='{pathIdString}']");

        XmlElement roadElement = doc.CreateElement("road");
        roadElement.SetAttribute("id", pathIdString);

        for (int i = 0; i < path.Count; i++)
        {
            XmlElement pathPointElement = doc.CreateElement("pathPoint");
            pathPointElement.SetAttribute("x", (path[i].position.x + 9).ToString());
            pathPointElement.SetAttribute("y", (path[i].position.y + 5).ToString());
            roadElement.AppendChild(pathPointElement);
        }

        if (existingRoad != null)
        {
            doc.DocumentElement.ReplaceChild(roadElement, existingRoad);
        }
        else
        {
            doc.DocumentElement.AppendChild(roadElement);
        }

        doc.Save(filePath);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && canBePressed)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Collider2D hitCollider = Physics2D.OverlapPoint(mousePosition);

            if (hitCollider != null)
            {
                if (path.Contains(hitCollider.gameObject.transform))
                {
                    path.Remove(hitCollider.gameObject.transform);
                    hitCollider.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
                }
                else
                {
                    path.Add(hitCollider.gameObject.transform);
                    hitCollider.gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
                }
            }
        }
    }

    public void ChangePathId()
    {
        pathId = Convert.ToInt32(idText.text);
    }
}
