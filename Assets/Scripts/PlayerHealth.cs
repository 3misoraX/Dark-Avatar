using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public static int staticMaxHp;
    public static int staticHp;
    public int maxHp = 30;
    public int hp;
    public static int pHp;
    public TMP_Text hpText;
    public static bool doubleDmgActive = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        staticMaxHp = maxHp;
        staticHp = staticMaxHp;
    }

    // Update is called once per frame
    void Update()
    {
        if(maxHp != staticMaxHp)
        {
            maxHp = staticMaxHp;
        }

        hp = staticHp;

        if(staticHp >= maxHp)
        {
            staticHp = maxHp;
        }

        hpText.text = hp.ToString() + "/" + maxHp.ToString();

        if(hp <= 0)
        {
            SceneManager.LoadScene(0);
        }
    }

    public static void TakeDamage(int dmg)
    {
        if (doubleDmgActive)
        {
            dmg *= 2;
        }
        if(TurnSystem.faithCounter > 0)
        {
            TurnSystem.faithCounter -= dmg;
            if(TurnSystem.faithCounter <= 0)
            {
                TurnSystem.faithCounter = 0;
            }
            return;
        }

        pHp = staticHp;
        staticHp -= dmg;

        foreach(TurnSystem.Effect fx in TurnSystem.activePlayerEffects)
        {
            if (fx.effectName == "bless")
            {
                Effects.Bless(staticHp, pHp);
            }
            else if (fx.effectName == "lament")
            {
                Effects.Bless(staticHp, pHp);
            }
        }

    }

    public static void Heal(int amount)
    {
        staticHp += amount;
    }
}
