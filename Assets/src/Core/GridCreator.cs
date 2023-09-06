using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.IO;

public class GridCreator : MonoBehaviour
{
    private const string FOLDEDR_PATH = "Assets\\Resources\\Data";
    private const string PATH_FILE_PATH = "Paths";
    public List<Transform> path = new List<Transform>();

    public bool rememberPath;

    public void SaveGrid()
    {
        XmlWriterSettings settings = new XmlWriterSettings();
        settings.Indent = true;

        using (XmlWriter writer = XmlWriter.Create(FOLDEDR_PATH + "/" + PATH_FILE_PATH + ".xml", settings))
        {
            writer.WriteStartDocument();
            writer.WriteStartElement("root");

            writer.WriteStartElement("road");
            writer.WriteAttributeString("id", "1");
            for (int i = 0; i < path.Count; i++)
            {
                writer.WriteStartElement("pathPoint");
                writer.WriteAttributeString("x", (path[i].position.x).ToString());
                writer.WriteAttributeString("y", (path[i].position.y).ToString());
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.WriteEndDocument();
        }
    }

    private void Update()
    {
        if (rememberPath)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                Collider2D hitCollider = Physics2D.OverlapPoint(mousePosition);

                if (hitCollider != null)
                {
                    hitCollider.gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
                    path.Add(hitCollider.gameObject.transform);
                }
            }
        }
    }
}
