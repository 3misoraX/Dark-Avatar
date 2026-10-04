using UnityEngine;
using System.Collections.Generic;

public class Deck : MonoBehaviour
{
    public List<Cards> deck = new List<Cards>();
    public List<Cards> pool = new List<Cards>();
    public List<Cards> temp = new List<Cards>();
    public int x = 0;
    public int totalCards = 10;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for(int i = 0; i < totalCards; i++)
        {
            x = Random.Range(0, pool.Count);
            deck.Add(pool[x]);
        }
    }

    public void Shuffle()
    {
         for(int i = 0; i < deck.Count; i++)
        {
            int idx = Random.Range(0, deck.Count);
            temp.Add(deck[idx]);
            deck.RemoveAt(idx);
        }
        deck = temp;
        temp.Clear();
    }
}
