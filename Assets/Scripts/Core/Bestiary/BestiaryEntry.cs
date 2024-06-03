using Common.Persistence;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BestiaryEntry : MonoBehaviour, IBind<CreatureData>
{
    [SerializeField]
    private Creature _creature;

    private CreatureInstanceData _data;

    [SerializeField]
    private Image _spriteImage;
    [SerializeField]
    private Bar _dnaBar;
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
        EventManager.Instance.DNAGained += OnCreatureDNAGained;

        if (_dnaBar != null)
            _dnaBar.MaxValue = 100;
        if (_data != null)
            _dnaBar.Value = _data.DiscoveryProgress;

        // ? We do need to check because the object is set to inactive
        // ? This is to cover the scenario when the creature is not
        // ? discovered at Bind time but discovered later before
        // ? bestiary is opened (before event listener is registered)
        CheckIfDiscovered();
    }

    private void OnCreatureDiscovered(string id)
    {
        if (id == _creature.Id)
            _spriteImage.color = Color.white;
    }

    private void OnCreatureDNAGained(Creature creature, float dna)
    {
        if (creature != _creature) return;
        _dnaBar.Value = Mathf.Min(_dnaBar.Value + dna, 100);
    }

    public void Bind(CreatureData data)
    {
        _data = data.FromId(_creature.Id);
        CheckIfDiscovered();
    }

    private void CheckIfDiscovered()
    {
        if (_data == null) return;

        if (_data.IsDiscovered)
        {
            _spriteImage.color = Color.white;
            _nameLabel.text = _creature.Name;
            _descriptionsLabel.text = _creature.Description;
            _dnaBar.Value = 100;
        }
    }
}
