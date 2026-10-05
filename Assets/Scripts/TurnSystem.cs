using TMPro;
using UnityEngine;

public class TurnSystem : MonoBehaviour
{
    public bool activeTurn = true;
    public int turn = 0;
    public int maxMana = 1;
    public int bodyMana = 0;
    public int soulMana = 0;
    public int mindMana = 0;
    public TMP_Text bodyManaText;
    public TMP_Text soulManaText;
    public TMP_Text mindManaText;

    public Deck deck;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        deck = GameObject.Find("Deck").GetComponent<Deck>();
        turn++;
        bodyMana = maxMana;
        soulMana = maxMana;
        mindMana = maxMana;
        activeTurn = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (activeTurn)
        {
            bodyManaText.text = bodyMana.ToString();
            soulManaText.text = soulMana.ToString();
            mindManaText.text = mindMana.ToString();
        }
    }

    public void TurnChange()
    {
        if (activeTurn)
        {
            turn++;
            if(maxMana < 5)
            {
                maxMana++;
            }
            bodyMana = maxMana;
            soulMana = maxMana;
            mindMana = maxMana;
            activeTurn = false;
            deck.DiscardHand();
        }
        else
        {
            activeTurn = true;
            deck.StartCoroutine(deck.DrawNewHand());
        }
    }
}
