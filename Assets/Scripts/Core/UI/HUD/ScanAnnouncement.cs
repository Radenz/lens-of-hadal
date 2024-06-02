using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScanAnnouncement : MonoBehaviour
{
    public Creature Creature;
    public int EnergyPowder;
    public int Seaweed;
    public int ScrapMetal;

    [SerializeField]
    private Transform _containerTransform;
    [SerializeField]
    private Image _image;
    [SerializeField]
    private RectTransform _imageTransform;
    [SerializeField]
    private Bar _discoveryBar;
    [SerializeField]
    private GameObject _energyPowderRewardLabelContainer;
    [SerializeField]
    private TextMeshProUGUI _energyPowderRewardLabel;
    [SerializeField]
    private GameObject _seaweedRewardLabelContainer;
    [SerializeField]
    private TextMeshProUGUI _seaweedRewardLabel;
    [SerializeField]
    private GameObject _scrapMetalRewardLabelContainer;
    [SerializeField]
    private TextMeshProUGUI _scrapMetalRewardLabel;

    [SerializeField]
    private AudioClip _sfx;

    private const float MaxWidth = 160;
    private const float MaxHeight = 96;

    private void Start()
    {
        AudioManager.Instance.PlaySFX(_sfx);

        _image.sprite = Creature.Sprite;
        Vector2 spriteSize = Creature.Sprite.rect.size;
        float xMaxScale = MaxWidth / spriteSize.x;
        float yMaxScale = MaxHeight / spriteSize.y;
        float spriteScale = Mathf.Min(xMaxScale, yMaxScale);
        _imageTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, spriteSize.x * spriteScale);
        _imageTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, spriteSize.y * spriteScale);

        if (BestiaryManager.Instance.IsCreatureDiscovered(Creature))
        {
            _image.color = Color.white;
            _discoveryBar.gameObject.SetActive(false);
        }
        else
        {
            _discoveryBar.MaxValue = 100;
            _discoveryBar.Value = BestiaryManager.Instance.GetCreatureDNA(Creature);
        }

        int unusedCurrencies = 0;
        List<GameObject> usedLabels = new();

        if (EnergyPowder == 0)
        {
            unusedCurrencies++;
            _energyPowderRewardLabelContainer.SetActive(false);
        }
        else
        {
            usedLabels.Add(_energyPowderRewardLabelContainer);
        }
        _energyPowderRewardLabel.text = EnergyPowder.ToString();

        if (Seaweed == 0)
        {
            unusedCurrencies++;
            _seaweedRewardLabelContainer.SetActive(false);
        }
        else
        {
            usedLabels.Add(_seaweedRewardLabelContainer);
        }
        _seaweedRewardLabel.text = Seaweed.ToString();

        if (ScrapMetal == 0)
        {
            unusedCurrencies++;
            _scrapMetalRewardLabelContainer.SetActive(false);
        }
        else
        {
            usedLabels.Add(_scrapMetalRewardLabelContainer);
        }
        _scrapMetalRewardLabel.text = ScrapMetal.ToString();

        float offset = -140 * (usedLabels.Count - 1);

        foreach (GameObject label in usedLabels)
        {
            Vector3 position = label.transform.localPosition;
            position.x = offset;
            offset += 280;
            label.transform.localPosition = position;
        }

        Show();
    }

    private async void Show()
    {
        await _containerTransform.DOLocalMoveY(
            _containerTransform.localPosition.y - 230,
            0.4f
        ).AsyncWaitForCompletion();
        await Awaitable.WaitForSecondsAsync(3);
        await _containerTransform.DOLocalMoveY(
            _containerTransform.localPosition.y + 230,
            0.4f
        ).AsyncWaitForCompletion();
        Destroy(gameObject);
    }
}
