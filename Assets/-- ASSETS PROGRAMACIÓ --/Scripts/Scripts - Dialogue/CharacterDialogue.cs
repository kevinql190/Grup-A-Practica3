using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDialogue : MonoBehaviour
{
    public CharacterData CharacterData;

    private void Start()
    {
        GetComponent<SpriteRenderer>().sprite = CharacterData.Sprite;
    }
}
