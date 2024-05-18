using Common.Persistence;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShopItem : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private Item _item;
    public Item Item => _item;

    public void OnPointerClick(PointerEventData eventData)
    {
        ShopSystem.Instance.ShowItem(_item);
    }
}
