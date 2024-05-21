using UnityEngine;

[CreateAssetMenu(fileName = "Reward", menuName = "Reward", order = 0)]
public class Reward : ScriptableObject
{
    public int Exp;
    public Item Item;
    public int Gold;

    public void Give()
    {
        if (Gold > 0)
            CurrencySystem.Instance.Gold += Gold;
        if (Item != null)
            ShopSystem.Instance.ShowItem(Item);

        Announcer.Instance.AnnounceReward(this);

        if (Exp > 0)
            LevelManager.Instance.AddExp(Exp);
    }
}
