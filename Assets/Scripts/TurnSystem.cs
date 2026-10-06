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
                faithMeter.GetComponentInChildren<TMP_Text>().text = faithCounter.ToString();
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
            foreach(Effect fx in activePlayerEffects)
            {
                if(fx.effectName == "condemn")
                {
                    Effects.Condemn(PlayerHealth.staticHp, fx.effectCount);
                    fx.countdown();
                }
                else if(fx.effectName == "spirit chains")
                {
                    foreach(Effect en in activePlayerEffects)
                    {
                        if(fx.effectName == "condemn"||fx.effectName == "devotion")
                        {
                            activePlayerEffects.Remove(fx);
                        }
                    }
                }

                fx.countdown();
                if(fx.effectCount <= 0)
                {
                    activePlayerEffects.Remove(fx);
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
            foreach(Effect fx in activePlayerEffects)
            {
                if(fx.effectName == "cross")
                {
                    Effects.Cross(maxMana, fx.effectCount);
                    activePlayerEffects.Remove(fx);
                    break;
                }
                else if(fx.effectName == "occult book")
                {
                    TurnSystem.faithCounter++;
                }
                else if(fx.effectName == "ceremonial dagger")
                {
                    DagaCeremonial();
                }
                else if(fx.effectName == "spirit chains")
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
            ai.MyTurn();
        }

        TurnChange();
    }

    public bool UseMana(int amount, char energyType)
    {
        bool activeAltar = false;
        foreach(Effect fx in activePlayerEffects)
        {
            if(fx.effectName == "cursed altar")
            {
                activeAltar = true;
            }
        }
        if(activeAltar)
        {
            if (energyType == 'b')
            {
                if (bodyMana - amount > 0)
                {
                    if(amount < 0)
                    {
                        amount = bodyMana;
                    }
                    bodyMana -= amount;
                    return true;
                }
                else
                {
                    PlayerHealth.staticHp -= amount;
                    return true;
                }
            }
            else if (energyType == 's')
            {
                if (soulMana - amount > 0)
                {
                    if (amount < 0)
                    {
                        amount = soulMana;
                    }
                    soulMana -= amount;
                    return true;
                }
                else
                {
                    PlayerHealth.staticHp -= amount;
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
                                    if (amount < 0)
                                    {
                                        amount = mindMana;
                                    }
                                    soulMana -= amount;
                                    return true;
                                }
                                else
                                {
                                    PlayerHealth.staticHp -= amount;
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
                else
                {
                    PlayerHealth.staticHp -= (amount*3);
                    return true;
                }
            }
            return false;
        }
        else
        {
            if(energyType == 'b')
            {
                if(bodyMana-amount > 0)
                {
                    bodyMana -= amount;
                    return true;
                }
            }
            else if(energyType == 's')
            {
                if (soulMana - amount > 0)
                {
                    soulMana -= amount;
                    return true;
                }
            }
            else if(energyType == 'm')
            {
                if (mindMana - amount < 0)
                {
                    foreach (Effect fx in activePlayerEffects)
                    {
                        if (fx.effectName == "energy seal")
                        {
                            if(bodyMana-amount < 0)
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
            else if(energyType == 't')
            {
                if(bodyMana-amount > 0 && soulMana-amount > 0 && mindMana - amount > 0)
                {
                    bodyMana -= amount;
                    soulMana -= amount;
                    mindMana -= amount;
                    return true;
                }
            }
            return false;
        }
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
