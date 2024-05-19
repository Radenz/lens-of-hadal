using NaughtyAttributes;
using UnityEngine;

// ? Ideally, this should contain reward config as well, but
// refactoring the established scannable system will take more time
[CreateAssetMenu(fileName = "Creature", menuName = "Creature", order = 0)]
public class Creature : ScriptableObject
{
    public string Id;
    public string Name;
    public string Description;
    public Sprite Sprite;
    public float BestiarySpriteScale;
    [Header("Rewards")]
    public int ExpPerScan;
    [MinMaxSlider(0, 100)]
    public Vector2 DNAPerScan;
    [MinMaxSlider(0, 100)]
    public Vector2Int EnergyPowderPerScan;
    [MinMaxSlider(0, 100)]
    public Vector2Int SeaweedPerScan;
    [MinMaxSlider(0, 100)]
    public Vector2Int ScrapMetalPerScan;
}
