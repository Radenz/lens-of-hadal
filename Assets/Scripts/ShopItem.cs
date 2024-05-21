using Common.Persistence;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private Item _item;
    public Item Item => _item;

    private void Start()
    {
        Image image = GetComponent<Image>();
        image.sprite = _item.Sprite;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ShopSystem.Instance.ShowItem(_item);
    }
}
