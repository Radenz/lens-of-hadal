using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class RewardAnnouncement : MonoBehaviour
{
    public string Title;

    public Item Item;
    public int Gold;

    [Header("References")]
    [SerializeField]
    private Image _image;
    [SerializeField]
    private TextMeshProUGUI _label;
    [SerializeField]
    private GameObject _goldLabelContainer;
    [SerializeField]
    private TextMeshProUGUI _goldLabel;

    private const float Offset = -160;

    [SerializeField]
    private AudioClip _sfx;

    private void Start()
    {
        AudioManager.Instance.PlaySFX(_sfx);
        EventManager.Instance.DisableCreatures();

        int usedComponents = 2;
        List<GameObject> components = new();

        if (Item == null)
        {
            _image.gameObject.SetActive(false);
            usedComponents--;
        }
        else
        {
            _image.sprite = Item.Sprite;
            _label.text = Item.Name;
            components.Add(_image.gameObject);
        }

        if (Gold == 0)
        {
            _goldLabelContainer.SetActive(false);
            usedComponents--;
        }
        else
        {
            _goldLabel.text = Gold.ToString();
            components.Add(_goldLabelContainer);
        }

        float offset = -Offset / 2 * (usedComponents - 1);
        foreach (GameObject component in components)
        {
            component.transform.localPosition = component.transform.localPosition.With(y: offset);
            offset += Offset;
        }
    }

    public void Close()
    {
        if (SceneTransitionSystem.Instance.CurrentScene == GameplayScene.World)
            EventManager.Instance.EnableCreatures();

        Destroy(gameObject);
    }
}
