using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class CardDB : MonoBehaviour
{
    public static List<Cards> cardList = new List<Cards>();

    void Awake()
    {
        cardList.Add(new Cards(0, "Stone Toss", "Deals 1 Dmg", 'a', 'b', 1));
        cardList.Add(new Cards(1, "Magic Shield", "Protects from 1 Dmg", 's', 's', 1));
        cardList.Add(new Cards(2, "Stone Idol", "Gain 1 Devotion", 'r', 'm', 3));
        cardList.Add(new Cards(3, "Fire Arena", "Body Cards Deal 1 Dmg to self", 'c', 't', 5));
    }
}
