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

    private const string SOUND_PATH = "Data/SoundsResources";
    private const string SOUND_OBJ_VAR = "sound";
    private const string SOUND_ID_ATT = "id";
    private const string SOUND_PATH_ATT = "path";

    public void LoadSprites()
    {
        ResourcesDB.sprites.Clear();

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
                ResourcesDB.sprites.Add(_sprite);
            }
        }

        reader.Close();
        Debug.Log("[Loading] [Sprites] Reading Finished, Loaded: " + ResourcesDB.sprites.Count + " Sprites!");
    }

    public void LoadSounds()
    {
        ResourcesDB.sounds.Clear();

        TextAsset binary = Resources.Load<TextAsset>(SOUND_PATH);

        if (binary == null)
        {
            Debug.LogError("[!] [Loading] [Sounds] No Such File: %" + SOUND_PATH + "% To Read!");
            return;
        }

        XmlTextReader reader = new XmlTextReader(new StringReader(binary.text));
        Debug.Log("[Loading] [Sounds] Reading Sprites File: " + SOUND_PATH + ".xml");

        int index = 0;

        while (reader.Read())
        {
            index++;
            if (reader.IsStartElement(SOUND_OBJ_VAR))
            {
                string _soundId = reader.GetAttribute(SOUND_ID_ATT);
                if (_soundId == null)
                    Debug.LogError("[!] [Loading] [Sounds] Sound With Index: %" + index.ToString() + "% Have No ID!");

                string _soundPath = reader.GetAttribute(SOUND_PATH_ATT);
                if (_soundPath == null)
                    Debug.LogError("[!] [Loading] [Sounds] Sound With ID: %" + _soundId + "% Have No Path!");

                AudioClip _sprite = Resources.Load<AudioClip>(_soundPath);
                if (_sprite == null)
                {
                    Debug.LogError("[!] [Loading] [Sounds] Failed To Load Sound With ID: %" + _soundId + "% No Such Path: %" + _soundPath);
                    continue;
                }

                _sprite.name = _soundId;
                ResourcesDB.sounds.Add(_sprite);
            }
        }

        reader.Close();
        Debug.Log("[Loading] [Sounds] Reading Finished, Loaded: " + ResourcesDB.sounds.Count + " Sounds!");
    }
    // TODO: Effects and Sounds Loading
    public Sprite GetSpriteByID(string _name)
    {
        Sprite _sprite = null;

        foreach(Sprite item in ResourcesDB.sprites)
        {
            if (item.name == _name)
                _sprite = item;
        }

        if (_sprite == null)
            Debug.LogError("[!] [Loading] [Sprites] No Such Sprite ID: %" + _name + "% To Return!");

        return _sprite;
    }

    public AudioClip GetSoundByID(string _name)
    {
        AudioClip _sound = null;

        foreach (AudioClip item in ResourcesDB.sounds)
        {
            if (item.name == _name)
                _sound = item;
        }

        if (_sound == null)
            Debug.LogError("[!] [Loading] [Sounds] No Such Sound ID: %" + _name + "% To Return!");

        return _sound;
    }
}

public static class ResourcesDB
{
    public static List<Sprite> sprites = new List<Sprite>();
    public static List<AudioClip> sounds = new List<AudioClip>();
}