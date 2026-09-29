using UnityEngine;

[CreateAssetMenu (fileName = "New Card", menuName = "Card")]
public class Cards : ScriptableObject
{
    public int id;
    public string cardName;
    public string description;
    public char type; ///a for attack, s for skill, r for reliq c for curse
    public char energyType;
    public int cost;

    public Sprite image;
}
