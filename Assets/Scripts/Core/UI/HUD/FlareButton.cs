using Common.Persistence;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Flarebutton : MonoBehaviour, IBind<ConsumableData>
{
    [SerializeField]
    private Image _button;
    [SerializeField]
    private TextMeshProUGUI _text;
    private int _cachedQuantity = 0;

    private void Start()
    {
        EventManager.Instance.ShopItemPurchased += OnItemPurchased;
        EventManager.Instance.FlareDeployed += OnDeployed;
        EventManager.Instance.FlareCooldownFinished += OnCooldownFinished;
        EventManager.Instance.FlareQuantityChanged += OnQuantityChanged;
    }

    private void OnDeployed()
    {
        SetAlpha(0.2f);
    }

    private void OnCooldownFinished()
    {
        if (_cachedQuantity > 0)
            SetAlpha(1);
    }

    private void OnQuantityChanged(int quantity)
    {
        _cachedQuantity = quantity;
        _text.text = quantity.ToString();

        if (quantity == 0)
            SetAlpha(0.2f);

        if (quantity > 0)
            SetAlpha(1);
    }

    private void OnItemPurchased(string id)
    {
        if (id == ShopItems.Flare)
        {
            _button.enabled = true;
            _text.enabled = true;
        }
    }

    void IBind<ConsumableData>.Bind(ConsumableData data)
    {
        if (data.IsFlareBought)
        {
            _button.enabled = true;
            _text.enabled = true;
        }
    }

    private void SetAlpha(float alpha)
    {
        Color color = _button.color;
        color.a = alpha;
        _button.color = color;

        color = _button.color;
        color.a = alpha;
        _text.color = color;
    }
}
