using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;
using TMPro;

public class EnemyAI : MonoBehaviour
{
    public int hp;
    private int previousHp;
    public int attackId;
    public TurnSystem turns;
    public int dmg;
    public static bool doubleDmgActive = false;
    public bool usesEffect;
    public int skill1Objective;

    public int skill2Objective;

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

    public List<Effect> activeEffects = new List<Effect>();
    public Effect skill1Effect;
    public Effect skill2Effect;
    public Effect attackEffect;
    public TMP_Text hpText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        turns = GameObject.Find("TurnSystem").GetComponent<TurnSystem>();
    }

    private void Update()
    {
        hpText.text = "Opponent HP: " + hp;
    }

    public void MyTurn()
    {
        attackId = UnityEngine.Random.Range(0, 3); // 0 = main attack, 1 = Skill 1, 2 = skill 2/Secondary attack
        switch (attackId)
        {
            case 0:
                Attack(attackEffect.effectName, attackEffect.effectCount);
                break;
            case 1:
                Skill(skill1Objective, skill1Effect);
                break;
            case 2: 
                if(skill2Objective >= 3)
                {

                    Attack(skill2Effect.effectName, skill2Effect.effectCount);
                }
                else
                {
                    Skill(skill2Objective, skill2Effect);
                }
                break;
        }

        for(int i = 0; i < activeEffects.Count; i++)
        {
            activeEffects[i].effectCount--;
            if (activeEffects[i].effectCount <= 0)
            {
                activeEffects.RemoveAt(i);
            }
        }
    }

    void Attack(string effectToApply = null, int count = 0)
    {
        if (usesEffect && effectToApply != null)
        {
            TurnSystem.ApplyEffect(effectToApply, count);
        }

        foreach (Effect fx in activeEffects)
        {
            if (fx.effectName == "devotion")
            {
                dmg = Effects.DevotionDoubt(dmg, fx.effectCount, true);
                PlayerHealth.TakeDamage(dmg);
                dmg = Effects.DevotionDoubt(dmg, fx.effectCount, false);
                return;
            }
            else if (fx.effectName == "doubt")
            {
                dmg = Effects.DevotionDoubt(dmg, fx.effectCount, false);
                PlayerHealth.TakeDamage(dmg);
                dmg = Effects.DevotionDoubt(dmg, fx.effectCount, true);
                return;
            }
        }

        PlayerHealth.TakeDamage(dmg);
    }

    void Skill(int objective, Effect effectToApply) //0 = self buff, 1 = ally buff, 2 = player debuff
    {
        switch (objective)
        {
            case 0:
                ApplyEffect(effectToApply.effectName, effectToApply.effectCount);
                break;
            case 1:
                List<GameObject> allies = new List<GameObject>();
                allies.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
                foreach(GameObject ally in allies)
                {
                    ally.GetComponent<EnemyAI>().ApplyEffect(effectToApply.effectName, effectToApply.effectCount);
                }
                break;
            case 2:
                TurnSystem.ApplyEffect(effectToApply.effectName, effectToApply.effectCount);
                break;
            default:
                return;
        }
    }

    public void ApplyEffect(string effect, int amount)
    {
        for(int i = 0; i < activeEffects.Count; i++)
        {
            if (activeEffects[i].effectName == effect)
            {
                activeEffects[i].addCount(amount);
                return;
            }
        }
        activeEffects.Add(new Effect(effect, amount));
    }

    public void TakeDamage(int damage)
    {
        if (doubleDmgActive)
        {
            damage *= 2;
        }
        previousHp = hp;
        hp -= damage;

        foreach(Effect fx in activeEffects)
        {
            if(fx.effectName == "bless")
            {
                hp = Effects.Bless(hp, previousHp);
            }
            else if (fx.effectName == "lament")
            {
                hp = Effects.Lament(hp, previousHp);
            }
        }

        if(hp <= 0)
        {
            SceneManager.LoadScene(2);
        }
    }
}
