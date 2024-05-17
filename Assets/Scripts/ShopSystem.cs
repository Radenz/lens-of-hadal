using System;
using System.Collections.Generic;
using UnityEngine;

public class ShopSystem : Singleton<ShopSystem>
{
    [SerializeField]
    private List<ShopItem> _items;

    [SerializeField]
    private ModuleGrid _upgradeSlot;

    [SerializeField]
    private ModuleGrid _inventorySlot;

    [SerializeField]
    private Transform _shopUITransform;

    [Header("Upgrade Modules")]
    [SerializeField]
    private GameObject _flashlightLv2;
    [SerializeField]
    private GameObject _flashlightLv3;
    [SerializeField]
    private GameObject _scannerLv2;
    [SerializeField]
    private GameObject _scannerLv3;

    private void Start()
    {
        EventManager.Instance.ShopItemUnlocked += UnlockItem;
        EventManager.Instance.ShopItemAssembled += AssembleItem;
    }

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

    // ? I hate this but time is tight
    public void AssembleItem(string id)
    {
        switch (id)
        {
            case ShopItems.DivingSuitLv2:
                ResizeUpgradeModule(1);
                return;
            case ShopItems.DivingSuitLv3:
                ResizeUpgradeModule(2);
                return;
            case ShopItems.Flare:
                ConsumablesManager.Instance.Flare += 1;
                return;
            case ShopItems.SonarDrone:
                ConsumablesManager.Instance.SonarDrone += 1;
                return;
        }

        GameObject obj = id switch
        {
            ShopItems.FlashlightLv2 => _flashlightLv2,
            ShopItems.FlashlightLv3 => _flashlightLv3,
            ShopItems.ScannerLv2 => _scannerLv2,
            ShopItems.ScannerLv3 => _scannerLv3,
            _ => throw new ArgumentException("Invalid item id"),
        };

        Module module = obj.GetComponent<Module>();
        _inventorySlot.Add(module);
    }

    private void ResizeUpgradeModule(int size)
    {
        _upgradeSlot.Expand((size, size));
    }
}
