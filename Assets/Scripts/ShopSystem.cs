using System;
using System.Collections.Generic;
using Common.Persistence;
using UnityEngine;

public class ShopSystem : Singleton<ShopSystem>, IBind<ItemData>
{
    private ItemData _data;

    [SerializeField]
    private List<ShopItem> _items;
    private readonly Dictionary<string, ShopItem> _shopItems = new();

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

    protected override void Awake()
    {
        base.Awake();
        foreach (ShopItem item in _items)
        {
            _shopItems[item.Id] = item;
        }
    }

    private void Start()
    {
        EventManager.Instance.ShopItemUnlocked += UnlockItem;
        EventManager.Instance.ShopItemAssembled += AssembleItem;
    }

    void IBind<ItemData>.Bind(ItemData data)
    {
        _data = data;
        foreach (ShopItem item in _items)
        {
            item.Bind(_data);
        }
    }

    public void UnlockItem(string id)
    {
        ShopItem item = _shopItems[id];
        item.gameObject.SetActive(true);
        _data.FromId(id).IsUnlocked = true;
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
