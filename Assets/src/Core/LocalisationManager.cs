using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.IO;
using TMPro;

public class LocalisationManager : MonoBehaviour
{
    [SerializeField]
    private bool _extendedLogs;

    private const string TEXT_FILE_PATH = "Texts";
    private const string TEXT_ELEMENT = "text";

    private List<string> _localesFiles = new List<string>();
    private Dictionary<string, string> _texts = new Dictionary<string, string>();
    public List<TextMeshProUGUI> textObjects = new List<TextMeshProUGUI>();

    private void Awake()
    {
        ReadAllLocalsFile();
        ReadAndFillTexts(_localesFiles[0]);
    }

    private void ReadAllLocalsFile()
    {
        Object[] resources = Resources.LoadAll(TEXT_FILE_PATH, typeof(Object));

        foreach (Object resource in resources)
        {
            _localesFiles.Add(resource.name);

            Debug.Log("[Core] [Locales] Added New Local: %" + resource.name + "%");
        }
    }

    private void ReadAndFillTexts(string local)
    {
        TextAsset binary = Resources.Load<TextAsset>(TEXT_FILE_PATH + '\\' + local);

        if (binary == null)
        {
            Debug.LogError("[!] [Core] [Texts] No Such Text File: %" + local + "%");
            return;
        }

        XmlTextReader reader = new XmlTextReader(new StringReader(binary.text));

        Debug.Log("[Core] [Texts] Reading Texts File: %" + TEXT_FILE_PATH + "%.xml");

        _texts.Clear();
        int index = 0;
        while (reader.Read())
        {
            index++;
            if (reader.IsStartElement(TEXT_ELEMENT))
            {
                string _textId = reader.GetAttribute("id");
                if (_textId == "")
                {
                    Debug.LogWarning("[?] [Core] [Texts] String Number: %" + index.ToString() + "% Dont Have Id!");
                    continue;
                }
                string _textValue = reader.ReadInnerXml();

                _texts.TryAdd(_textId, _textValue);

                if(_extendedLogs)
                    Debug.Log("[Core] [Texts] Loaded New Text: %" + _textId + "%. Text Value: %" + _textValue + "%");
            }
        }
        reader.Close();
        
        Debug.Log("[Core] [Texts] Reading Finished, Loaded: %" + _texts.Keys.Count + "%" + " Texts!");
    }

    public void ChangeLocale(string local)
    {
        ReadAndFillTexts(local);

        foreach(TextMeshProUGUI text in textObjects)
        {
            text.text = GetTextByKey(text.text);
        }

        Debug.Log("[Core] [Locales] Locale Changed To: %" + local + "%");
    }

    public string GetTextByKey(string key)
    {
        if (_texts.ContainsKey(key))
            return _texts.GetValueOrDefault(key);
        else
            return key;
    }
}
