using DG.Tweening;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // TODO: refactor singletons
    public static PlayerController Instance;

    private Movement _movement;

    [SerializeField]
    private int _defaultHealthPoints;

    #region Player Attributes
    private int _healthPoints = 0;
    private int _dna = 0;
    private int _gold = 0;
    #endregion Player Attributes

    [SerializeField]
    private float _hudUpdateDuration = 0.6f;
    private float _animatedDna = 0;

    #region Attributes HUD
    [SerializeField]
    private TextMeshProUGUI _dnaHUDLabel;
    #endregion Attributes HUD

    private void Awake()
    {
        Instance = this;
        _healthPoints = _defaultHealthPoints;
    }

    private void Start()
    {
        _movement = GetComponent<Movement>();
    }

    public void Bounce(Vector2 direction, float strength)
    {
        _movement.Bounce(direction, strength);
    }

    public void Damage()
    {
        // TODO: impl health system
        _healthPoints--;
        Debug.Log("Player is damaged");
    }

    public void Shock(float duration)
    {
        _movement.Shock(duration);
    }

    public void AddDNA(int amount)
    {
        _dna += amount;
        DOTween.To(
            () => _animatedDna,
            dna => _dnaHUDLabel.text = Mathf.RoundToInt(dna).ToString(),
            _dna,
            _hudUpdateDuration
        )
            .SetDelay(Announcer.Instance.Duration);
    }
}
