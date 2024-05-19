using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class RewardAnnouncement : MonoBehaviour
{
    public string Title;

    public Sprite Sprite;
    public string SpriteDescription;

    public int Gold;

    [Header("References")]
    [SerializeField]
    private Image _image;
    [SerializeField]
    private GameObject _goldLabelContainer;
    [SerializeField]
    private TextMeshProUGUI _goldLabel;

    private const float Offset = 50;

    private void Start()
    {
        EventManager.Instance.DisableCreatures();

        int usedComponents = 2;
        List<GameObject> components = new();

        if (Sprite == null)
        {
            _image.gameObject.SetActive(false);
            usedComponents--;
        }
        else
        {
            components.Add(_image.gameObject);
        }

        if (Gold == 0)
        {
            _goldLabelContainer.SetActive(false);
            usedComponents--;
        }
        else
        {
            components.Add(_goldLabelContainer);
        }

        float offset = -Offset / 2 * (usedComponents - 1);
        foreach (GameObject component in components)
        {
            component.transform.localPosition = component.transform.localPosition.Add(y: offset);
            offset += Offset;
        }
    }

    public void Close()
    {
        if (SceneTransitionSystem.Instance.CurrentScene != GameplayScene.World)
            EventManager.Instance.EnableCreatures();

        Destroy(gameObject);
    }
}
