using System.Collections.Generic;
using UnityEngine;

public class ShopSystem : Singleton<ShopSystem>
{
    [SerializeField]
    private List<ShopItem> _items;

    public void UnlockItem(string id)
    {
        foreach (ShopItem item in _items)
        {
            if (item.Id == id)
            {
                item.gameObject.SetActive(true);
            }
        }
    }
}
