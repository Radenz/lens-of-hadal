using System.Collections.Generic;
using Common.Persistence;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemMenu : MonoBehaviour, IBind<ItemInstanceData>
{
    Item _item;
    ItemInstanceData _data;

    public bool DisableOnAssemble = false;

    [Header("UI Containers")]
    [SerializeField] private GameObject _unlockCostLabels;
    [SerializeField] private GameObject _assembleCostLabels;
    [SerializeField] private GameObject _energyPowderLabelContainer;
    [SerializeField] private GameObject _seaweedLabelContainer;
    [SerializeField] private GameObject _scrapMetalLabelContainer;
    private List<GameObject> AssembleCostContainers => new()
    {
        _energyPowderLabelContainer,
        _seaweedLabelContainer,
        _scrapMetalLabelContainer,
    };

    [Header("UI Elements")]
    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _nameLabel;
    [SerializeField] private TextMeshProUGUI _descriptionLabel;
    [SerializeField] private TextMeshProUGUI _goldLabel;
    [SerializeField] private TextMeshProUGUI _energyPowderLabel;
    [SerializeField] private TextMeshProUGUI _seaweedLabel;
    [SerializeField] private TextMeshProUGUI _scrapMetalLabel;

    [SerializeField]
    private HoldActionButton _unlockButton;
    [SerializeField]
    private HoldActionButton _assembleButton;

    private void Start()
    {
        _unlockButton.Execute += OnUnlock;
        _assembleButton.Execute += OnAssemble;
    }

    private void OnUnlock()
    {
        if (!CanUnlock())
            return;

        CurrencySystem.Instance.Gold -= _item.Gold;

        EventManager.Instance.BuyItem(_item.Id);

        _unlockCostLabels.SetActive(false);
        _assembleCostLabels.SetActive(true);

        _unlockButton.gameObject.SetActive(false);
        _assembleButton.gameObject.SetActive(true);

        _data.IsPurchased = true;
    }

    private void OnAssemble()
    {
        if (!CanAssemble())
            return;

        CurrencySystem.Instance.EnergyPowder -= _item.EnergyPowder;
        CurrencySystem.Instance.Seaweed -= _item.Seaweed;
        CurrencySystem.Instance.ScrapMetal -= _item.ScrapMetal;

        EventManager.Instance.AssembleItem(_item);

        if (DisableOnAssemble)
            ConfigureLabels(false, false);

        if (_data.TryDowncast(out UpgradableItemData upgradableItemData))
            upgradableItemData.IsAssembled = true;
    }

    public void Bind(Item item)
    {
        _item = item;

        _nameLabel.text = item.Name;
        _descriptionLabel.text = item.Description;
        _image.sprite = item.Sprite;

        // ? Costs
        _goldLabel.text = item.Gold.ToString();
        _energyPowderLabel.text = item.EnergyPowder.ToString();
        _seaweedLabel.text = item.Seaweed.ToString();
        _scrapMetalLabel.text = item.ScrapMetal.ToString();

        _energyPowderLabelContainer.SetActive(item.EnergyPowder != 0);
        _seaweedLabelContainer.SetActive(item.Seaweed != 0);
        _scrapMetalLabelContainer.SetActive(item.ScrapMetal != 0);

        List<GameObject> assembleCostContainers = AssembleCostContainers;
        int activeContainers = assembleCostContainers.FindAll(c => c.activeSelf).Count;

        float offset = -60 * (activeContainers - 1);

        foreach (GameObject container in assembleCostContainers)
        {
            if (!container.activeSelf) continue;

            Vector3 position = container.transform.localPosition;
            position.x = offset;
            offset += 120;
        }

        if (!CanUnlock()) _unlockButton.Disable();
        if (!CanAssemble()) _assembleButton.Disable();
    }

    public void Bind(ItemInstanceData data)
    {
        _data = data;

        if (_data.TryDowncast(out UpgradableItemData upgradableItemData)
            && upgradableItemData.IsAssembled)
        {
            // gameObject.SetActive(false);
            ConfigureLabels(showUnlock: true, showAssemble: true);
            return;
        }

        if (_data.IsPurchased)
        {
            ConfigureLabels(showAssemble: true);
            return;
        }

        if (_data.IsUnlocked)
        {
            ConfigureLabels(showUnlock: true);
        }
    }


    private void ConfigureLabels(bool showUnlock = false, bool showAssemble = false)
    {
        _unlockButton.gameObject.SetActive(showUnlock);
        _unlockCostLabels.SetActive(showUnlock);
        _assembleButton.gameObject.SetActive(showAssemble);
        _assembleCostLabels.SetActive(showAssemble);
    }

    private bool CanUnlock()
    {
        return CurrencySystem.Instance.Gold >= _item.Gold;
    }

    private bool CanAssemble()
    {
        return CurrencySystem.Instance.EnergyPowder >= _item.EnergyPowder
            && CurrencySystem.Instance.Seaweed >= _item.Seaweed
            && CurrencySystem.Instance.ScrapMetal >= _item.ScrapMetal;
    }
}
