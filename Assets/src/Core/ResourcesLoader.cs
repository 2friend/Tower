using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEditor;
using UnityEngine;

public class ResourcesLoader : MonoBehaviour
{
    private const string SPRITE_PATH = "ResourcesSprites";
    private const string SPRITE_GROUP_VAR = "group";
    private const string SPRITE_OBJ_VAR = "sprite";
    private const string SPRITE_ID_ATT = "id";
    private const string SPRITE_PATH_ATT = "path";

    private const string SOUND_PATH = "ResourcesSounds";
    private const string SOUND_OBJ_VAR = "sound";
    private const string SOUND_ID_ATT = "id";
    private const string SOUND_PATH_ATT = "path";

    public void LoadSprites()
    {
        TextAsset binary = Resources.Load<TextAsset>(SPRITE_PATH);

        if (binary == null)
        {
            Debug.LogError($"[!] [Loading] [Sprites] No Such File: %{SPRITE_PATH}% To Read!");
            return;
        }

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(binary.text);

        XmlNodeList groups = xmlDoc.GetElementsByTagName(SPRITE_GROUP_VAR);
        int index = 0;
        foreach (XmlNode group in groups)
        {
            string _groupId = group.Attributes[SPRITE_ID_ATT].Value;
            string _groupPath = group.Attributes["pre_path"].Value;

            List<Sprite> _sprites = new List<Sprite>();

            XmlNodeList sprites = group.SelectNodes(SPRITE_OBJ_VAR);
            foreach (XmlNode spriteNode in sprites)
            {
                index++;
                string _spriteId = spriteNode.Attributes[SPRITE_ID_ATT].Value;
                string _spritePath = spriteNode.Attributes[SPRITE_PATH_ATT].Value;

                Sprite _sprite = Resources.Load<Sprite>(_groupPath + _spritePath);
                if (_sprite == null)
                {
                    Debug.LogError($"[!] [Loading] [Sprites] Failed To Load Sprite With ID: %{_spriteId}% No Such Path: %{_spritePath}");
                    continue;
                }

                if (GameConstant.extendedLogs)
                    Debug.Log($"[Loading] [Sprites] Loaded Sprite: %{_spriteId}% In Atlas: %{_groupId}%");

                _sprite.name = _spriteId;
                _sprites.Add(_sprite);
            }

            Texture2D atlasTexture = CreateAtlasList(_groupId, _sprites);
            index = 0;
        }

        Debug.Log($"[Loading] [Sprites] Reading Finished, Loaded: %{ResourcesDB.atlases.Keys.Count}% Atlases!");
    }

    public Texture2D CreateAtlasList(string _atlas, List<Sprite> _sprites)
    {
        Texture2D atlas = new Texture2D(2048, 2048, TextureFormat.ARGB32, false);

        if (atlas == null)
        {
            Debug.LogError("[!] [Atlases] Failed To Create Atlas Texture!");
            return null;
        }

        if (ResourcesDB.atlases.ContainsKey(_atlas))
        {
            Debug.LogError($"[!] [Atlases] Atlas: {_atlas} Is Already Created!");
            return null;
        }

        Texture2D[] texturesToPack = GetTexturesFromSprites(_sprites);

        if (texturesToPack == null || texturesToPack.Length == 0)
        {
            Debug.LogError("[!] [Atlases] No Valid Textures To Pack!");
            return null;
        }

        Rect[] rects = atlas.PackTextures(texturesToPack, 2, 2048);

        if (rects == null || rects.Length == 0 || rects.Length != texturesToPack.Length)
        {
            Debug.LogError("[!] [Atlases] Error In Packing Textures Into Atlas!");
            return null;
        }

        for (int i = 0; i < _sprites.Count; i++)
        {
            if (i < rects.Length && _sprites[i] != null)
            {
                ResourcesDB.spriteRects[_sprites[i].name] = rects[i];
            }
            else
            {
                Debug.LogError($"[!] [Atlases] Issue At Index {i}. Not Enough Rects Or Sprite Is Null! Rects.Length: {rects.Length}, Sprites.Count: {_sprites.Count}");
                return null;
            }
        }

        atlas.Apply();

        ResourcesDB.atlases.Add(_atlas, atlas);

        return atlas;
    }

    protected Texture2D[] GetTexturesFromSprites(List<Sprite> _sprites)
    {
        Texture2D[] textures = new Texture2D[_sprites.Count];

        for (int i = 0; i < _sprites.Count; i++)
        {
            if (_sprites[i].rect.width > 2048 || _sprites[i].rect.height > 2048)
            {
                Debug.LogWarning($"[?] [Atlases] Sprite: %{_sprites[i].name}% To Big To Pack In Atlas!");
                continue;
            }

            textures[i] = new Texture2D((int)_sprites[i].rect.width, (int)_sprites[i].rect.height);
            textures[i].SetPixels(_sprites[i].texture.GetPixels((int)_sprites[i].rect.x, (int)_sprites[i].rect.y,
                                                              (int)_sprites[i].rect.width, (int)_sprites[i].rect.height));
            textures[i].Apply();
        }

        return textures;
    }

    public Sprite GetSpriteFromAtlas(string atlas, string spriteName)
    {
        Texture2D _atlas = null;

        Rect spriteRect;

        if (ResourcesDB.atlases.ContainsKey(atlas))
            _atlas = ResourcesDB.atlases[atlas];
        else
        {
            Debug.LogError($"[!] [Atlases] No Such Atlas %{atlas}% Found!");
            return null;
        }

        if (ResourcesDB.spriteRects.TryGetValue(spriteName, out spriteRect))
        {
            Sprite sprite = Sprite.Create(_atlas, spriteRect, Vector2.zero);
            return sprite;
        }
        else
        {
            Debug.LogError($"[!] [Atlases] Sprite: %{spriteName}% Not Found In Atlas!");
            return null;
        }
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
    public AudioClip GetSoundByID(string _name)
    {
        AudioClip _sound = null;

        foreach (AudioClip item in ResourcesDB.sounds)
        {
            if (item.name == _name)
                _sound = item;
        }

        if (_sound == null)
            Debug.LogError($"[!] [Loading] [Sounds] No Such Sound ID: %{_name}% To Return!");

        return _sound;
    }
}

public static class ResourcesDB
{
    public static Dictionary<string, Texture2D> atlases = new Dictionary<string, Texture2D>();
    public static Dictionary<string, Rect> spriteRects = new Dictionary<string, Rect>();
    public static List<AudioClip> sounds = new List<AudioClip>();
}