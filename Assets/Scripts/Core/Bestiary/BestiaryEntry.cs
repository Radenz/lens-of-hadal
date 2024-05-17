using TMPro;
using UnityEngine;
using UnityEngine.UI;

// TODO: IBind<CreatureData>
public class BestiaryEntry : MonoBehaviour
{
    [SerializeField]
    private Creature _creature;

    [SerializeField]
    private Image _spriteImage;
    [SerializeField]
    private TMP_Text _nameLabel;
    [SerializeField]
    private TMP_Text _descriptionsLabel;


    private void Start()
    {
        _spriteImage.sprite = _creature.Sprite;
        RectTransform imageTransform = (RectTransform)_spriteImage.transform;
        imageTransform.SetSizeWithCurrentAnchors(
            RectTransform.Axis.Horizontal,
            _creature.Sprite.rect.width * _creature.BestiarySpriteScale
        );
        imageTransform.SetSizeWithCurrentAnchors(
            RectTransform.Axis.Vertical,
            _creature.Sprite.rect.height * _creature.BestiarySpriteScale
        );
        EventManager.Instance.CreatureDiscovered += OnCreatureDiscovered;
    }

    private void OnCreatureDiscovered(string id)
    {
        _spriteImage.color = Color.white;
    }
}
