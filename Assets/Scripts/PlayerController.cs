using DG.Tweening;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    // TODO: refactor singletons
    public static PlayerController Instance;

    private Transform _transform;
    private Movement _movement;

    [SerializeField]
    private float _maxHealthPoints;

    [SerializeField]
    private float _maxStamina;

    [SerializeField]
    private Light2D _flashlight;

    #region Player Attributes
    [SerializeField, ReadOnly]
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

            float healthPercentage = _healthPoints / _maxHealthPoints;
            float inverseHealthPercentage = 1 - healthPercentage;
            float rawOverlayAlpha = inverseHealthPercentage * inverseHealthPercentage;
            float alpha = 0.5f * rawOverlayAlpha;
            _damageOverlay.color = new(1f, 1f, 1f, alpha);
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

    private AutoFlip _flipper;
    private PlayerInputActions _playerInputActions;

    private int _dna = 0;
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

    private bool _disableActions = false;

    #region Attributes HUD
    [SerializeField]
    private TextMeshProUGUI _dnaHUDLabel;

    [SerializeField]
    private GameObject _deathOverlay;

    [SerializeField]
    private Image _damageOverlay;

    [SerializeField]
    private Bar _staminaBar;
    #endregion Attributes HUD

    [SerializeField]
    private GameObject _flarePrefab;
    [SerializeField]
    private GameObject _sonarPrefab;

    private void Awake()
    {
        Instance = this;
        _transform = transform;
        _timeSinceLastDamage = 2f;
    }

    private void Start()
    {
        _flipper = GetComponent<AutoFlip>();
        _movement = GetComponent<Movement>();
        _staminaBar.MaxValue = _maxStamina;
        HealthPoints = _maxHealthPoints;
        Stamina = _maxStamina;

        _playerInputActions = new();

        _playerInputActions.World.Enable();
        _playerInputActions.World.DeployFlare.performed += _ => DeployFlare();
        _playerInputActions.World.DeploySonar.performed += _ => DeploySonar();

        EventManager.Instance.FlashlightEquipped += OnFlashlightEquipped;
        EventManager.Instance.FlashlightUnequipped += OnFlashlightUnequipped;
        EventManager.Instance.PlayerActionsDisabled += OnDisableActions;
        EventManager.Instance.PlayerActionsEnabled += OnEnableActions;
    }

    private void Update()
    {
        if (_disableActions) return;

        _timeSinceLastDamage += Time.deltaTime;
        _timeSinceLastStaminaDrain += Time.deltaTime;
        if (_shouldRecover && HealthPoints < _maxHealthPoints)
            Recover();
        if (_shouldRecoverStamina && Stamina < _maxStamina)
            RecoverStamina();
    }

    private void OnFlashlightEquipped(int level)
    {
        if (level == 2)
        {
            _flashlight.intensity = 1.1f;
            _flashlight.pointLightOuterRadius = 6f;
            return;
        }

        if (level == 3)
        {
            _flashlight.intensity = 1.2f;
            _flashlight.pointLightOuterRadius = 7f;
        }
    }

    private void OnDisableActions()
    {
        _disableActions = true;
        _movement.enabled = false;
    }

    private void OnEnableActions()
    {
        _disableActions = false;
        _movement.enabled = true;
    }

    private void OnFlashlightUnequipped()
    {
        _flashlight.intensity = 1f;
        _flashlight.pointLightOuterRadius = 5f;
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

    // TODO: check quantity & reduce
    public void DeployFlare()
    {
        if (_disableActions) return;
        GameObject flare = Instantiate(_flarePrefab, _transform.position, Quaternion.identity);
        ArcLaunch arcLauncher = flare.GetComponent<ArcLaunch>();

        Vector2 direction = _playerInputActions.World.Move.ReadValue<Vector2>();

        if (direction.magnitude == 0)
        {
            float lookAngle = _flipper.Angle * Mathf.PI / 180;
            direction = new(Mathf.Cos(lookAngle), Mathf.Sin(lookAngle));
        }

        arcLauncher.Direction = direction;
    }

    // TODO: check quantity & reduce
    public void DeploySonar()
    {
        if (_disableActions) return;
        GameObject sonar = Instantiate(_sonarPrefab, _transform.position, Quaternion.identity);
        ArcLaunch arcLauncher = sonar.GetComponent<ArcLaunch>();

        Vector2 direction = _playerInputActions.World.Move.ReadValue<Vector2>();

        if (direction.magnitude == 0)
        {
            float lookAngle = _flipper.Angle * Mathf.PI / 180;
            direction = new(Mathf.Cos(lookAngle), Mathf.Sin(lookAngle));
        }

        arcLauncher.Direction = direction;
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
