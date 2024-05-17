using NaughtyAttributes;
using UnityEngine;

// ? Ideally, this should contain reward config as well, but
// refactoring the established scannable system will take more time
[CreateAssetMenu(fileName = "Creature", menuName = "Creature", order = 0)]
public class Creature : ScriptableObject
{
    public string Id;
    public string Name;
    public Sprite Sprite;
    public float BestiarySpriteScale;
    [MinMaxSlider(0, 100)]
    public Vector2 DNAPerScan;
}
