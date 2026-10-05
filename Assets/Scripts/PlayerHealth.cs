using UnityEngine;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    public static int staticMaxHp;
    public static int staticHp;
    public int maxHp = 30;
    public int hp;
    public TMP_Text hpText;
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

        if(hp >= maxHp)
        {
            hp = maxHp;
        }

        hpText.text = hp.ToString() + "/" + maxHp.ToString();
    }
}
