using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;

public class ResourcesLoader : MonoBehaviour
{
    private const string SPRITE_PATH = "Data/SpritesResources";
    private const string SPRITE_OBJ_VAR = "sprite";
    private const string SPRITE_ID_ATT = "id";
    private const string SPRITE_PATH_ATT = "path";

    public List<Sprite> sprites = new List<Sprite>();

    public void LoadSprites()
    {
        sprites.Clear();

        TextAsset binary = Resources.Load<TextAsset>(SPRITE_PATH);

        if (binary == null)
        {
            Debug.LogError("[!] [Loading] [Sprites] No Such File: %" + SPRITE_PATH + "% To Read!");
            return;
        }

        XmlTextReader reader = new XmlTextReader(new StringReader(binary.text));
        Debug.Log("[Loading] [Sprites] Reading Sprites File: " + SPRITE_PATH + ".xml");

        int index = 0;

        while (reader.Read())
        {
            index++;
            if (reader.IsStartElement(SPRITE_OBJ_VAR))
            {
                string _spriteId = reader.GetAttribute(SPRITE_ID_ATT);
                if (_spriteId == null)
                    Debug.LogError("[!] [Loading] [Sprites] Sprite With Index: %" + index.ToString() + "% Have No ID!");

                string _spritePath = reader.GetAttribute(SPRITE_PATH_ATT);
                if (_spritePath == null)
                    Debug.LogError("[!] [Loading] [Sprites] Sprite With ID: %" + _spriteId + "% Have No Path!");

                Sprite _sprite = Resources.Load<Sprite>(_spritePath);
                if (_sprite == null)
                {
                    Debug.LogError("[!] [Loading] [Sprites] Failed To Load Sprite With ID: %" + _spriteId + "% No Such Path: %" + _spritePath);
                    continue;
                }

                _sprite.name = _spriteId;
                sprites.Add(_sprite);
            }
        }

        reader.Close();
        Debug.Log("[Loading] [Sprites] Reading Finished, Loaded: " + sprites.Count + " Sprites!");
    }
}
