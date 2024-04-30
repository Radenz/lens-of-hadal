using DG.Tweening;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // TODO: refactor singletons
    public static PlayerController Instance;

    private Movement _movement;

    [SerializeField]
    private float _maxHealthPoints;

    [SerializeField]
    private float _maxStamina;

    #region Player Attributes
    private float _healthPoints = 0;
    private float HealthPoints
    {
        get => _healthPoints;
        set
        {
            _healthPoints = value;
            if (_healthPoints > _maxHealthPoints)
                _healthPoints = _maxHealthPoints;
            if (_healthPoints < 0)
                _healthPoints = 0;

            _healthBar.Value = _healthPoints;
        }
    }

    private float _stamina = 0;
    public float Stamina
    {
        get => _stamina;
        set
        {
            if (value < _stamina)
                _timeSinceLastStaminaDrain = 0;

            _stamina = value;
            if (_stamina > _maxStamina)
                _stamina = _maxStamina;
            if (_stamina < 0)
                _stamina = 0;

            _staminaBar.Value = _stamina;
        }
    }

    private int _dna = 0;
    private int _gold = 0;
    #endregion Player Attributes

    [SerializeField]
    private float _recoveryDelay = 7f;
    [SerializeField]
    private float _recoverySpeed = 5f;
    [SerializeField]
    private float _staminaRecoveryDelay = 5f;
    [SerializeField]
    private float _staminaRecoverySpeed = 10f;
    private float _timeSinceLastDamage = 0;
    private float _timeSinceLastStaminaDrain = 0;
    private bool _shouldRecover => _timeSinceLastDamage > _recoveryDelay;
    private bool _shouldRecoverStamina => _timeSinceLastStaminaDrain > _staminaRecoveryDelay;

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

    [SerializeField]
    private Bar _healthBar;

    [SerializeField]
    private Bar _staminaBar;
    #endregion Attributes HUD

    private void Awake()
    {
        Instance = this;
        _timeSinceLastDamage = 2f;
    }

    private void Start()
    {
        _movement = GetComponent<Movement>();
        _healthBar.MaxValue = _maxHealthPoints;
        _staminaBar.MaxValue = _maxStamina;
        HealthPoints = _maxHealthPoints;
        Stamina = _maxStamina;
    }

    private void Update()
    {
        _timeSinceLastDamage += Time.deltaTime;
        _timeSinceLastStaminaDrain += Time.deltaTime;
        if (_shouldRecover && HealthPoints < _maxHealthPoints)
            Recover();
        if (_shouldRecoverStamina && Stamina < _maxStamina)
            RecoverStamina();
    }

    private void Recover()
    {
        HealthPoints += Time.deltaTime * _recoverySpeed;
    }

    private void RecoverStamina()
    {
        Stamina += Time.deltaTime * _staminaRecoverySpeed;
    }

    public void Bounce(Vector2 direction, float strength)
    {
        _movement.Bounce(direction, strength);
    }

    public void Damage(float amount = 1)
    {
        HealthPoints -= amount;
        _timeSinceLastDamage = 0f;
        if (HealthPoints == 0)
            Die();
    }

    public void Respawn()
    {
        Time.timeScale = 1;
        _movement.Stop();
        HealthPoints = _maxHealthPoints;
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
