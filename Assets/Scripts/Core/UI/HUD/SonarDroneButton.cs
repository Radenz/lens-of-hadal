using Common.Persistence;
using UnityEngine;
using UnityEngine.UI;

public class SonarDroneButton : MonoBehaviour, IBind<ConsumableData>
{
    [SerializeField]
    private Image _button;
    private int _cachedQuantity = 0;

    private void Start()
    {
        EventManager.Instance.ShopItemPurchased += OnItemPurchased;
        EventManager.Instance.SonarDeployed += OnDeployed;
        EventManager.Instance.SonarCooldownFinished += OnCooldownFinished;
        EventManager.Instance.SonarQuantityChanged += OnQuantityChanged;
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

        if (quantity == 0)
            SetAlpha(0.2f);

        if (quantity > 0)
            SetAlpha(1);
    }

    private void OnItemPurchased(string id)
    {
        if (id == ShopItems.SonarDrone) _button.enabled = true;
    }

    void IBind<ConsumableData>.Bind(ConsumableData data)
    {
        if (data.IsSonarDroneBought)
            _button.enabled = true;
    }

    private void SetAlpha(float alpha)
    {
        Color color = _button.color;
        color.a = alpha;
        _button.color = color;
    }
}
