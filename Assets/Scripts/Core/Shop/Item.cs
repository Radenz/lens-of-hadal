using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Item", order = 0)]
public class Item : ScriptableObject
{
    public Sprite Sprite;
    public string Id;
    public string Name;
    [ResizableTextArea]
    public string Description;
    [Header("Unlock Cost")]
    public int Gold;
    [Header("Assemble Cost")]
    public int EnergyPowder;
    public int Seaweed;
    public int ScrapMetal;
}
