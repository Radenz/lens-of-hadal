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
        EventManager.Instance.ShopItemShown += UnlockItem;
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
        if (id == "DivingSuit2")
        {
            ResizeUpgradeModule(1);
            return;
        }

        if (id == "DivingSuit3")
        {
            ResizeUpgradeModule(2);
            return;
        }

        if (id == "Flashlight2")
        {
            GameObject obj = Instantiate(_flashlightLv2, _shopUITransform);
            Module module = obj.GetComponent<Module>();
            _inventorySlot.Add(module);
            return;
        }

        if (id == "Flashlight3")
        {
            GameObject obj = Instantiate(_flashlightLv3, _shopUITransform);
            Module module = obj.GetComponent<Module>();
            _inventorySlot.Add(module);
            return;
        }

        if (id == "Scanner2")
        {
            GameObject obj = Instantiate(_scannerLv2, _shopUITransform);
            Module module = obj.GetComponent<Module>();
            _inventorySlot.Add(module);
            return;
        }

        if (id == "Scanner3")
        {
            GameObject obj = Instantiate(_scannerLv3, _shopUITransform);
            Module module = obj.GetComponent<Module>();
            _inventorySlot.Add(module);
            return;
        }

        if (id == "Flare")
        {
            ConsumablesManager.Instance.Flare += 1;
            return;
        }

        if (id == "SonarDrone")
        {
            ConsumablesManager.Instance.SonarDrone += 1;
            return;
        }
    }

    private void ResizeUpgradeModule(int size)
    {
        _upgradeSlot.Expand((size, size));
    }
}
