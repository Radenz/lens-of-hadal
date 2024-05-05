using TMPro;
using UnityEngine;
using UnityEngine.Events;

[ExecuteAlways]
public class ShopItem : MonoBehaviour
{
    [Header("Item Properties")]
    [SerializeField]
    private string _id;
    public string Id => _id;
    [SerializeField]
    private string _name;
    public string Name => _name;

    [Header("Cost")]
    [SerializeField]
    private int _gold;
    [SerializeField]
    private int _energyPowder;
    [SerializeField]
    private int _seaweed;
    [SerializeField]
    private int _scrapMetal;

    // TODO: hook up all items
    [Header("Action")]
    [SerializeField]
    private UnityEvent _onAssemble;

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

        // TODO: give player upgrade module, etc.
    }
}
