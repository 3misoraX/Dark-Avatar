using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Unity.VisualScripting;

public class Deck : MonoBehaviour
{
    public List<Cards> deck = new List<Cards>();
    public List<Cards> pool = new List<Cards>();
    public List<Cards> temp = new List<Cards>();
    public List<Cards> discard = new List<Cards>();
    public List<Cards> exile = new List<Cards>();
    public int x = 0;
    public int totalCards = 10;
    public int handSize = 5;

    public GameObject hand;
    public GameObject handCard;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for(int i = 0; i < totalCards; i++)
        {
            x = Random.Range(0, pool.Count);
            deck.Add(pool[x]);
        }

        StartCoroutine(StartGame());
    }

    public void Shuffle()
    {
         for(int i = 0; i < deck.Count; i++)
        {
            int idx = Random.Range(0, deck.Count);
            temp.Add(deck[idx]);
            deck.RemoveAt(idx);
        }
        deck.AddRange(temp);
        temp.Clear();
    }

    public void DiscardHand()
    {
        foreach (Transform child in hand.transform)
        {
            discard.Add(child.GetComponent<DisplayCard>().card);
            Destroy(child.gameObject);
        }
    }

    public IEnumerator DrawNewHand(int extra = 0)
    {
        int count = handSize + extra;
        if (count > 12)
            count = 12;

        for (int i = 0; i < count; i++)
        {
            if (deck.Count <= 0 && discard.Count > 0)
            {
                deck.AddRange(discard);
                discard.Clear();
                Shuffle();
            }
        
            yield return new WaitForSeconds(0.5f);
            Instantiate(handCard, transform.position, transform.rotation);
        }
    }

    public void Draw(int amount)
    {
        for(int i = 0; i < amount; i++)
        {
            if(deck.Count > hand.transform.childCount || hand.transform.childCount < 10)
             {
                if (deck.Count <= 0 && discard.Count > 0)
                {
                    deck.AddRange(discard);
                    discard.Clear();
                    Shuffle();
                }

                Instantiate(handCard, transform.position, transform.rotation);
            }
        }
    }

    IEnumerator StartGame()
    {
        Shuffle();

        if (handSize > 10)
            handSize = 10;

        for(int i = 0; i < handSize; i++)
        {
            yield return new WaitForSeconds(0.5f);
            Instantiate(handCard, transform.position, transform.rotation);
        }
    }
}