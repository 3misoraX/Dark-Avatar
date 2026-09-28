using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[System.Serializable]
public class Cards
{
    public int id;
    public string cardName;
    public string description;
    public char type;
    public char energyType;
    public int cost;

    public Cards()
    {
        // Constructor logic here
    }

    public Cards(int Id, string CardName, string CardDescription, char Type, char EnergyType, int Cost)
    {
        id = Id;
        cardName = CardName;
        description = CardDescription;
        type = Type;
        energyType = EnergyType;
        cost = Cost;
    }
}
