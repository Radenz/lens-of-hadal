using UnityEngine;

[CreateAssetMenu(fileName = "Reward", menuName = "Reward", order = 0)]
public class Reward : ScriptableObject
{
    public int Exp;
    public Item Item;
    public int Gold;

    public void Give(string title = "LEVEL UP")
    {
        if (Gold > 0)
            CurrencySystem.Instance.Gold += Gold;
        if (Item != null)
            EventManager.Instance.UnlockItem(Item.Id);

        Announcer.Instance.AnnounceReward(this, title);

        if (Exp > 0)
            LevelManager.Instance.AddExp(Exp);
    }
}
