using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newCharacter", menuName = "Characters", order = 1)]
public class CharacterData : ScriptableObject
{
    public Sprite Sprite;
    public string Name;
}
