using DG.Tweening;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // TODO: refactor singletons
    public static PlayerController Instance;

    private Movement _movement;

    [SerializeField]
    private int _maxHealthPoints;

    #region Player Attributes
    private float _healthPoints = 0;
    private int _dna = 0;
    private int _gold = 0;
    #endregion Player Attributes

    [SerializeField]
    private float _recoveryDelay = 2f;
    [SerializeField]
    private float _recoverySpeed = 10f;
    private float _timeSinceLastDamage = 0;
    private bool _shouldRecover => _timeSinceLastDamage > _recoveryDelay;

    [SerializeField]
    private Transform _respawnPoint;

    [SerializeField]
    private float _hudUpdateDuration = 0.6f;
    private float _animatedDna = 0;

    #region Attributes HUD
    [SerializeField]
    private TextMeshProUGUI _dnaHUDLabel;

    [SerializeField]
    private GameObject _deathOverlay;
    #endregion Attributes HUD

    private void Awake()
    {
        Instance = this;
        _healthPoints = _maxHealthPoints;
        _timeSinceLastDamage = 2f;
    }

    private void Start()
    {
        _movement = GetComponent<Movement>();
    }

    private void Update()
    {
        _timeSinceLastDamage += Time.deltaTime;
        if (_shouldRecover) Recover();
    }

    private void Recover()
    {
        _healthPoints += Time.deltaTime * _recoverySpeed;
        if (_healthPoints > _maxHealthPoints)
            _healthPoints = _maxHealthPoints;
    }

    public void Bounce(Vector2 direction, float strength)
    {
        _movement.Bounce(direction, strength);
    }

    public void Damage(float amount = 1)
    {
        _healthPoints -= amount;
        _timeSinceLastDamage = 0f;

        if (_healthPoints < 0)
            Die();
    }

    public void Respawn()
    {
        Time.timeScale = 1;
        _movement.Stop();
        _healthPoints = _maxHealthPoints;
        transform.position = _respawnPoint.position;
    }

    private void Die()
    {
        Time.timeScale = 0;
        _deathOverlay.SetActive(true);
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
            dna =>
            {
                _animatedDna = dna;
                _dnaHUDLabel.text = Mathf.RoundToInt(dna).ToString();
            },
            _dna,
            _hudUpdateDuration
        )
            .SetDelay(Announcer.Instance.Duration);
    }
}
