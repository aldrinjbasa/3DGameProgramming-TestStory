using UnityEngine;
using UnityEngine.Sprites;

[System.Serializable]
public class DamageSpriteHandler
{
    public string name;
    public Sprite damageSprite;

    [HideInInspector]
    public SpriteRenderer source;
}

