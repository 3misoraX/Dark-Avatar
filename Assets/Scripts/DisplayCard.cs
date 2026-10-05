using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class DisplayCard : MonoBehaviour
{
    //display and extra info
    public Cards card;
    public Deck deck;
    [HideInInspector]
    public List<string> typeList = new List<string> { "Attack", "Skill", "Relic", "Curse" };
    [HideInInspector]
    public List<Color> cardColor = new List<Color> 
    {
        new Color(178/255f, 87/255f, 86/255f, 255/255f), //body = red
        new Color(204/255f, 185/255f, 111/255f, 255/255f), //mind = yellow
        new Color(79/255f, 145/255f, 149/255f, 255/255f), //spirit = cyan
        new Color(72/255f, 46/255f, 144/255f, 255/255f) //trinity = purple
    };
    
    //displayers for card info
    public TMP_Text cardNameText;
    public TMP_Text descriptionText;
    public TMP_Text typeText;
    public TMP_Text costText;
    public Image cardBody;
    public Image cardImage;

    public GameObject hand;
    public int cardTotal;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        deck = GameObject.Find("Deck").GetComponent<Deck>();
        hand = GameObject.Find("Hand");
        cardTotal = deck.totalCards;

        if (this.CompareTag("Clone"))
        {
            card = deck.deck[0];
            deck.deck.RemoveAt(0);
            this.tag = "Untagged";
        }

        //filling card with info
        cardNameText.text = card.cardName;
        descriptionText.text = card.description;
        costText.text = card.cost.ToString();
        switch (card.type)
        {
            case 'a':
                typeText.text = typeList[0];
                break;
            case 's':
                typeText.text = typeList[1];
                break;
            case 'r':
                typeText.text = typeList[2];
                break;
            case 'c':
                typeText.text = typeList[3];
                break;
            default:
                typeText.text = "N/A";
                break;
        }
        switch (card.energyType)
        {
            case 'b':
                cardBody.color = cardColor[0];
                break;
            case 'm':
                cardBody.color = cardColor[1];
                break;
            case 's':
                cardBody.color = cardColor[2];
                break;
            case 't':
                cardBody.color = cardColor[3];
                break;
            default:
                cardBody.color = Color.lightGray;
                break;
        }

        if(card.image != null)
        {
            cardImage.sprite = card.image;
        }
    }

    void Update()
    {
        if (this.CompareTag("Clone") && deck.deck.Count > 0)
        {
            card = deck.deck[0];
            deck.deck.RemoveAt(0);
            this.tag = "Untagged";
        }
    }
}
