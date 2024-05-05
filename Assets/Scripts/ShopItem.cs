using TMPro;
using UnityEngine;

[ExecuteAlways]
public class ShopItem : MonoBehaviour
{
    [Header("Item Properties")]
    [SerializeField]
    private string _name;

    [Header("Cost")]
    [SerializeField]
    private int _gold;
    [SerializeField]
    private int _energyPowder;
    [SerializeField]
    private int _seaweed;
    [SerializeField]
    private int _scrapMetal;


    [Header("UI Elements")]
    [SerializeField]
    private TextMeshProUGUI _nameLabel;
    [SerializeField]
    private HoldActionButton _unlockButton;
    [SerializeField]
    private HoldActionButton _assembleButton;

    private void Start()
    {
        _unlockButton.Execute += OnUnlock;
        _assembleButton.Execute += OnAssemble;
    }

    private void OnValidate()
    {
        _nameLabel.text = _name;
    }

    private void OnUnlock()
    {
        if (CurrencySystem.Instance.Gold < _gold)
            return;

        CurrencySystem.Instance.Gold -= _gold;

        _unlockButton.gameObject.SetActive(false);
        _assembleButton.gameObject.SetActive(true);
    }

    private void OnAssemble()
    {
        if (CurrencySystem.Instance.EnergyPowder < _energyPowder
            || CurrencySystem.Instance.Seaweed < _seaweed
            || CurrencySystem.Instance.ScrapMetal < _scrapMetal)
            return;
    }
}
