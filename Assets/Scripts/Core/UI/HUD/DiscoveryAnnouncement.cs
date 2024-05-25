using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DiscoveryAnnouncement : MonoBehaviour
{
    public Creature Creature;

    [Header("References")]
    [SerializeField]
    private GameObject _container;
    [SerializeField]
    private TextMeshProUGUI _nameLabel;
    [SerializeField]
    private TextMeshProUGUI _nameLabelOutline;
    [SerializeField]
    private Image _image;

    [SerializeField]
    private AudioClip _sfx;

    private void Start()
    {
        AudioManager.Instance.PlaySFX(_sfx);
        _image.sprite = Creature.Sprite;
        _nameLabel.text = Creature.Name;
        _nameLabelOutline.text = Creature.Name;

        _container.transform.DOPunchScale(Vector3.one / 2, 1f);

        Decay();
    }

    private async void Decay()
    {
        await Awaitable.WaitForSecondsAsync(3f);
        await _container.transform.DOScale(Vector3.zero, 0.2f).AsyncWaitForCompletion();
        Destroy(gameObject);
    }
}
