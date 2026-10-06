using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TurnSystem : MonoBehaviour
{
    public bool activeTurn = true;
    public int turn = 0;
    public int maxMana = 1;
    public int bodyMana = 0;
    public int soulMana = 0;
    public int mindMana = 0;
    public static int faithCounter = 0;
    public TMP_Text bodyManaText;
    public TMP_Text soulManaText;
    public TMP_Text mindManaText;
    public GameObject faithMeter;
    public static bool randomFxActive = false;

    [Serializable]
    public class Effect
    {
        public string effectName;
        public int effectCount;

        public Effect(string name, int count)
        {
            effectName = name;
            effectCount = count;
        }

        public void countdown()
        {
            effectCount--;
        }

        public void addCount(int amount)
        {
            effectCount += amount;
        }
    };
    public static List<Effect> activePlayerEffects = new List<Effect>();

    public List<GameObject> activeEnemies = new List<GameObject>();

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

        activeEnemies.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
    }

    // Update is called once per frame
    void Update()
    {
        if (activeTurn)
        {
            bodyManaText.text = bodyMana.ToString();
            soulManaText.text = soulMana.ToString();
            mindManaText.text = mindMana.ToString();
            if(faithCounter <= 0)
            {
                faithMeter.SetActive(false);
            }
            else
            {
                faithMeter.SetActive(true);
                faithMeter.GetComponentInChildren<TMP_Text>().text = "Faith: " + faithCounter.ToString();
            }

            foreach(Effect fx in activePlayerEffects)
            {
                if(fx.effectName == "occult book")
                {
                    OccultBook();
                }
            }
        }
    }

    public void TurnChange()
    {
        if (activeTurn)
        {
            activeTurn = false;
            deck.DiscardHand();
            for(int i = 0; i< activePlayerEffects.Count; i++)
            {
                if (activePlayerEffects[i].effectName == "condemn")
                {
                    Effects.Condemn(PlayerHealth.staticHp, activePlayerEffects[i].effectCount);
                    activePlayerEffects[i].countdown();
                }
                else if (activePlayerEffects[i].effectName == "spirit chains")
                {
                    for(int o = 0; o < activePlayerEffects.Count; o++)
                    {
                        if (activePlayerEffects[o].effectName == "condemn" || activePlayerEffects[o].effectName == "devotion")
                        {
                            activePlayerEffects.RemoveAt(o);
                        }
                    }
                }

                activePlayerEffects[i].countdown();
                if (activePlayerEffects[i].effectCount <= 0)
                {
                    activePlayerEffects.RemoveAt(i);
                }
            }
            EnemyTurns();
        }
        else
        {
            if (randomFxActive)
            {
                string[] effectList = {"bless", "devotion", "doubt", "lament", "condemn", "dogma", "faith", "cross"};
                int x = UnityEngine.Random.Range(0, effectList.Length);
                ApplyEffect(effectList[x], 1);
            }
            turn++;
            if(maxMana < 5)
            {
                maxMana++;
            }
            for(int i = 0; i < activePlayerEffects.Count; i++)
            {
                if (activePlayerEffects[i].effectName == "cross")
                {
                    Effects.Cross(maxMana, activePlayerEffects[i].effectCount);
                    activePlayerEffects.RemoveAt(i);
                    break;
                }
                else if (activePlayerEffects[i].effectName == "occult book")
                {
                    TurnSystem.faithCounter++;
                }
                else if (activePlayerEffects[i].effectName == "ceremonial dagger")
                {
                    DagaCeremonial();
                }
                else if (activePlayerEffects[i].effectName == "spirit chains")
                {
                    int x = soulMana;
                    if (UseMana(soulMana, 's') == true)
                    {
                        SpiritChains(x);
                    }
                }
            }

            bodyMana = maxMana;
            soulMana = maxMana;
            mindMana = maxMana;
            activeTurn = true;
            deck.StartCoroutine(deck.DrawNewHand());
        }
    }

    void EnemyTurns()
    {
        EnemyAI ai;
        foreach(GameObject enemy in activeEnemies)
        {
            ai = enemy.GetComponent<EnemyAI>();
            if (randomFxActive)
            {
                string[] effectList = { "bless", "devotion", "doubt", "lament", "condemn", "dogma", "faith" };
                int x = UnityEngine.Random.Range(0, effectList.Length);
                ai.ApplyEffect(effectList[x], 1);
            }
            ai.StartCoroutine(ai.MyTurn());
        }
    }

    public bool UseMana(int amount, char energyType)
    {
        bool activeAltar = false;
        for (int i = 0; i < activePlayerEffects.Count; i++)
        {
            if (activePlayerEffects[i].effectName == "cursed altar")
            {
                activeAltar = true;
            }
        }
            if (energyType == 'b')
            {
                if (bodyMana - amount >= 0)
                {
                    bodyMana -= amount;
                    return true;
                }
                else if (activeAltar && bodyMana - amount < 0)
                {
                    PlayerHealth.staticHp -= amount;
                    return true;
                }
            }
            else if (energyType == 's')
            {
                if (soulMana - amount >= 0)
                {
                    soulMana -= amount;
                    return true;
                }
                else if (activeAltar && soulMana - amount < 0)
                {
                    {
                        PlayerHealth.staticHp -= amount;
                        return true;
                    }
                }
                else if (energyType == 'm')
                {
                    if (mindMana - amount < 0)
                    {
                        for (int i = 0; i < activePlayerEffects.Count; i++)
                        {
                            if (activePlayerEffects[i].effectName == "energy seal")
                            {
                                if (bodyMana - amount < 0)
                                {
                                    if (soulMana - amount >= 0)
                                    {
                                        soulMana -= amount;
                                        return true;
                                    }
                                    else
                                    {
                                        if (activeAltar)
                                        {
                                            PlayerHealth.staticHp -= amount;
                                            return true;
                                        }
                                    }
                                }
                                else
                                {
                                    bodyMana -= amount;
                                    return true;
                                }
                            }
                        }
                    }
                    else if (mindMana - amount >= 0)
                    {
                        mindMana -= amount;
                        return true;
                    }
                }
                else if (energyType == 't')
                {
                    if (bodyMana - amount > 0 && soulMana - amount > 0 && mindMana - amount > 0)
                    {
                        bodyMana -= amount;
                        soulMana -= amount;
                        mindMana -= amount;
                        return true;
                    }
                    else
                    {
                        if (activeAltar)
                        {
                            PlayerHealth.staticHp -= (amount * 3);
                            return true;
                        }
                    }
                }
                Debug.Log("Not Enough Mana");
                return false;
            }
            else
            {
                if (energyType == 'b')
                {
                    if (bodyMana - amount > 0)
                    {
                        bodyMana -= amount;
                        return true;
                    }
                }
                else if (energyType == 's')
                {
                    if (soulMana - amount > 0)
                    {
                        soulMana -= amount;
                        return true;
                    }
                }
                else if (energyType == 'm')
                {
                    if (mindMana - amount < 0)
                    {
                        foreach (Effect fx in activePlayerEffects)
                        {
                            if (fx.effectName == "energy seal")
                            {
                                if (bodyMana - amount < 0)
                                {
                                    if (soulMana - amount > 0)
                                    {
                                        soulMana -= amount;
                                        return true;
                                    }
                                }
                                else
                                {
                                    bodyMana -= amount;
                                    return true;
                                }
                            }
                        }
                    }
                    else
                    {
                        mindMana -= amount;
                        return true;
                    }
                }
                else if (energyType == 't')
                {
                    if (bodyMana - amount > 0 && soulMana - amount > 0 && mindMana - amount > 0)
                    {
                        bodyMana -= amount;
                        soulMana -= amount;
                        mindMana -= amount;
                        return true;
                    }
                }
            }

        
            return false;
    }

    public static void ApplyEffect(string effect, int amount)
    {
        foreach (Effect fx in activePlayerEffects)
        {
            if (fx.effectName == effect)
            {
                fx.addCount(amount);
                return;
            }
        }
        activePlayerEffects.Add(new Effect(effect, amount));
    }

    public void OccultBook()
    {
        if(bodyMana <= 0)
        {
            bodyMana += faithCounter;
            faithCounter = 0;
        }
        else if (soulMana <= 0)
        {
            soulMana += faithCounter;
            faithCounter = 0;
        }
        else if(mindMana <= 0)
        {
            mindMana += faithCounter;
            faithCounter = 0;
        }
    }

    public void DagaCeremonial()
    {
        ApplyEffect("devotion", 2);
    }

    public void SpiritChains(int amount)
    {
        ApplyEffect("condemn", amount);
        ApplyEffect("devotion", amount);
    }
}
