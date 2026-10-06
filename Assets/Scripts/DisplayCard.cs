using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

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
        new Color(118/255f, 38/255f, 185/255f, 255/255f) //trinity = purple
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
    public int curseActive = 0;

    public TurnSystem turns;

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
        if(card.cost < 0)
        {
            costText.text = "X";
        }
        else
        {
            costText.text = card.cost.ToString();
        }
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

        turns = GameObject.Find("TurnSystem").GetComponent<TurnSystem>();
    }

    public void Use(EnemyAI ai)
    {
        switch (card.id)
        {
            case 0:
                Attack(3);
                break;
            case 1:
                Attack(3);
                ai.ApplyEffect("lament", 1);
                break;
            case 2:
                Attack(4);
                ai.ApplyEffect("condemn", 6);
                break;
            case 3:
                Attack(5);
                PlayerHealth.Heal(5);
                break;
            case 4:
                Attack(2);
                GameObject.Find("Deck").GetComponent<Deck>().discard.Add(card);
                break;
            case 5:
                Attack(TurnSystem.faithCounter);
                TurnSystem.faithCounter = 0;
                break;
            case 6:
                Attack(6);
                TurnSystem.ApplyEffect("cross", 2);
                break;
            case 7:
                TurnSystem.ApplyEffect("condemn", GameObject.Find("TurnSystem").GetComponent<TurnSystem>().maxMana);
                break;
            case 8:
                TurnSystem.ApplyEffect("faith", GameObject.Find("TurnSystem").GetComponent<TurnSystem>().maxMana);
                break;
            case 9:
                TurnSystem.ApplyEffect("bless", 1);
                break;
            case 10:
                ai.ApplyEffect("lament", 2);
                ai.ApplyEffect("doubt", 2);
                break;
            case 11:
                ai.ApplyEffect("dogma", 3);
                break;
            case 12:
                BlindFaith();
                break;
            case 13:
                PlayerHealth.Heal(GameObject.Find("TurnSystem").GetComponent<TurnSystem>().maxMana);
                break;
            case 14:
                GameObject.Find("Deck").GetComponent<Deck>().Draw(2);
                break;
            case 15:
                TurnSystem.ApplyEffect("doubt", 2);
                break;
            case 16:
                TurnSystem.ApplyEffect("occult book", 4);
                break;
            case 17:
                TurnSystem.ApplyEffect("dogma", 3);
                TurnSystem.ApplyEffect("bless", 3);
                break;
            case 18:
                TurnSystem.ApplyEffect("energy seal", 4);
                break;
            case 19:
                TurnSystem.ApplyEffect("ceremonial dagger", 5);
                break;
            case 20:
                TurnSystem.ApplyEffect("vampirism", 4);
                break;
            case 21:
                TurnSystem.ApplyEffect("spirit chains", 4);
                break;
            case 22:
                TurnSystem.ApplyEffect("thorn whip", 4);
                break;
            case 23:
                TurnSystem.ApplyEffect("cursed altar", 4);
                break;
            case 24:
                curseActive = 1;
                TurnSystem.randomFxActive = false;
                break;
            case 25:
                curseActive = 2;
                TurnSystem.randomFxActive = true;
                break;
        }
    }

    void Update()
    {
        if(card.cost <= 0)
        {
            if(card.energyType == 'b')
            {
                card.cost = turns.bodyMana;
            }
            else if(card.energyType == 's')
            {
                card.cost = turns.soulMana;
            }
            else if(card.energyType == 'm')
            {
                card.cost = turns.mindMana;
            }
            else
            {
                card.cost = turns.maxMana;
            }
        }

        if(curseActive != 1)
        {
            PlayerHealth.doubleDmgActive = false;
            EnemyAI.doubleDmgActive = false;
        }
        if (this.CompareTag("Clone") && deck.deck.Count > 0)
        {
            card = deck.deck[0];
            deck.deck.RemoveAt(0);
            this.tag = "Untagged";
        }

        if(curseActive == 1)
        {
            Teophobia();
        }
    }
            

    public void Attack(int dmg)
    {
        foreach(TurnSystem.Effect fx in TurnSystem.activePlayerEffects)
        {
            if (fx.effectName == "vampirism")
            {
                PlayerHealth.Heal(dmg / 2);
            }
            else if(fx.effectName == "dogma")
            {
                int x = UnityEngine.Random.Range(0, 2);
                if(x == 0)
                {
                    return;
                }
            }
            else if(fx.effectName == "doubt")
            {
                dmg = Effects.DevotionDoubt(dmg, fx.effectCount, false);
                transform.parent.gameObject.GetComponent<EnemyAI>().TakeDamage(dmg);
                dmg = Effects.DevotionDoubt(dmg, fx.effectCount, true);
            }
            else if(fx.effectName == " devotion")
            {
                dmg = Effects.DevotionDoubt(dmg, fx.effectCount, true);
                transform.parent.gameObject.GetComponent<EnemyAI>().TakeDamage(dmg);
                dmg = Effects.DevotionDoubt(dmg, fx.effectCount, false);
            }
            else if( fx.effectName == "thorn whip")
            {
                transform.parent.gameObject.GetComponent<EnemyAI>().ApplyEffect("lament", 1);
            }
        }
        transform.parent.gameObject.GetComponent<EnemyAI>().TakeDamage(dmg);
    }

    public void DealEffect(string effectName, int count)
    {
        transform.parent.gameObject.GetComponent<EnemyAI>().ApplyEffect(effectName, count);
    }

    public void BlindFaith()
    {
        TurnSystem.faithCounter *= 2;
    }

    public void Teophobia()
    {
        PlayerHealth.doubleDmgActive = true;
        EnemyAI.doubleDmgActive = true;
    }
}
