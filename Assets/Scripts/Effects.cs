using UnityEngine;

public static class Effects
{
    public static int Bless(int currentHp, int previousHp)
    {
        if (currentHp < previousHp)
        {
            currentHp += (previousHp - currentHp) / 2;
        }

        return currentHp;
    }

    public static int DevotionDoubt(int currentDmg, int amount, bool sum)
    {
        if (sum)
        {
            currentDmg += amount;
        }
        else
        {
            currentDmg -= amount;
        }


        return currentDmg;
    }

    public static int Lament(int currentHp, int previousHp)
    {
        if (currentHp < previousHp)
        {
            currentHp -= (previousHp - currentHp) / 2;
        }

        return currentHp;
    }

    public static int Dogma(int dmg)
    {
        int x = Random.Range(0, 2);
        if (x == 0)
        {
            dmg = 0;
        }

        return dmg;
    }

    public static int Condemn(int hp, int amount)
    {
        hp -= amount;
        return hp;
    }

    public static int Faith(int faithCounter, int amount)
    {
        faithCounter += amount;
        return faithCounter;
    }

    public static int Cross(int currentMaxMana, int amount)
    {
        currentMaxMana -= amount;
        if (currentMaxMana < 1)
        {
            currentMaxMana = 1;
        }

        return currentMaxMana;
    }
}
