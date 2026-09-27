using UnityEngine;
using System.Collections.Generic;

public class CardDB : MonoBehaviour
{
    public static List<Cards> cardList = new List<Cards>();

    void Awake()
    {
        cardList.Add(new Cards(0, "Stone Toss", "Deals 1 Dmg", 'A', 'G', 1));
    }
}
