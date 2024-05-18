using System;
using System.Collections.Generic;
using Common.Persistence;
using NaughtyAttributes;
using UnityEngine;

public class ShopSystem : Singleton<ShopSystem>, IBind<ItemData>
{
    private ItemData _data;

    [SerializeField]
    private List<GameObject> _items;


    [Header("References")]
    [SerializeField]
    private GameObject _shopItemMenuPrefab;

    [SerializeField]
    private GameObject _shelfItemContainer;
    [SerializeField]
    private GameObject _shelfItemMenu;

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
    }

    private void Start()
    {
        EventManager.Instance.ShopItemUnlocked += UnlockItem;
        EventManager.Instance.ShopItemAssembled += AssembleItem;
    }

    void IBind<ItemData>.Bind(ItemData data)
    {
        _data = data;
    }

    public void ShowItem(Item item)
    {
        _shelfItemContainer.SetActive(false);
        _shelfItemMenu.SetActive(true);
        GameObject obj = Instantiate(_shopItemMenuPrefab, _shelfItemMenu.transform);
        ShopItemMenu menu = obj.GetComponent<ShopItemMenu>();
        menu.Bind(item);
        menu.Bind(_data.FromId(item.Id));
    }

    public void CloseItem()
    {
        _shelfItemContainer.SetActive(true);
        _shelfItemMenu.SetActive(false);
    }

    public void UnlockItem(string id)
    {
        _data.FromId(id).IsUnlocked = true;

        foreach (GameObject item in _items)
        {
            if (item.GetComponent<ShopItem>().Item.Id == id)
                item.SetActive(true);
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

        GameObject prefab = id switch
        {
            ShopItems.FlashlightLv2 => _flashlightLv2,
            ShopItems.FlashlightLv3 => _flashlightLv3,
            ShopItems.ScannerLv2 => _scannerLv2,
            ShopItems.ScannerLv3 => _scannerLv3,
            _ => throw new ArgumentException("Invalid item id"),
        };

        GameObject obj = Instantiate(prefab, _shopUITransform);
        Module module = obj.GetComponent<Module>();
        _inventorySlot.Add(module);
    }

    private void ResizeUpgradeModule(int size)
    {
        _upgradeSlot.Expand((size, size));
    }
}
