using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DisplayCard : MonoBehaviour
{
    //display and extra info
    public List<Cards> displayCard = new List<Cards>();
    public int displayId = 0;
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

    //containers for card info
    public int id;
    public string cardName;
    public string description;
    public char type;
    public char energyType;
    public int cost;

    //displayers for card info
    public TMP_Text cardNameText;
    public TMP_Text descriptionText;
    public TMP_Text typeText;
    public TMP_Text costText;
    public Image cardBody;
    public Image cardImage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        displayCard[0] = CardDB.cardList[displayId];
    }

    // Update is called once per frame
    void Update()
    {
        //Getting card info
        id = displayCard[0].id;
        cardName = displayCard[0].cardName;
        description = displayCard[0].description;
        type = displayCard[0].type;
        energyType = displayCard[0].energyType;
        cost = displayCard[0].cost;

        //filling card with info
        cardNameText.text = cardName;
        descriptionText.text = description;
        costText.text = cost.ToString();
        switch (type)
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
        switch (energyType)
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
    }
}
