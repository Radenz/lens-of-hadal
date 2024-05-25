using System.Threading.Tasks;
using Cinemachine;
using Common.Persistence;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class PlayerController : Singleton<PlayerController>, IBind<PlayerData>
{
    private PlayerData _data;
    private Transform _transform;
    private Movement _movement;

    public Vector3 Position => _transform.position;

    [Header("Consumables")]
    [SerializeField]
    private float _flareCooldown = 3f;
    [SerializeField]
    private float _sonarCooldown = 3f;
    [SerializeField]
    private GameObject _flarePrefab;
    [SerializeField]
    private GameObject _sonarPrefab;
    private bool _canDeployFlare = true;
    private bool _canDeploySonar = true;


    [Header("Setup")]
    [SerializeField]
    private CinemachineVirtualCamera _playerVCam;
    private CinemachineBasicMultiChannelPerlin _playerVCamNoise;

    [SerializeField]
    private Light2D _flashlight;
    [SerializeField]
    private SpriteRenderer _spriteRenderer;

    #region Player Attributes
    private float HealthPoints
    {
        get => _data.HealthPoints;
        set
        {
            _data.HealthPoints = value;
            if (_data.HealthPoints > _data.MaxHealthPoints)
                _data.HealthPoints = _data.MaxHealthPoints;
            if (_data.HealthPoints < 0)
                _data.HealthPoints = 0;

            float healthPercentage = _data.HealthPoints / _data.MaxHealthPoints;
            float inverseHealthPercentage = 1 - healthPercentage;
            float rawOverlayAlpha = inverseHealthPercentage * inverseHealthPercentage;
            float alpha = 0.5f * rawOverlayAlpha;
            _damageOverlay.color = new(1f, 1f, 1f, alpha);

            _debugHealth = value;
        }
    }

    private bool _isInvincible = false;
    public bool IsInvincible => _isInvincible;

    [SerializeField, ReadOnly]
    private float _debugHealth;

    public float Stamina
    {
        get => _data.Stamina;
        set
        {
            if (value < _data.Stamina)
                _timeSinceLastStaminaDrain = 0;

            _data.Stamina = value;
            if (_data.Stamina > _data.MaxStamina)
                _data.Stamina = _data.MaxStamina;
            if (_data.Stamina < 0)
                _data.Stamina = 0;

            _staminaBar.Value = _data.Stamina;
        }
    }

    private AutoFlip _flipper;
    private PlayerInputActions _playerInputActions;

    #endregion Player Attributes

    [Header("Attributes")]
    [SerializeField]
    private float _invincibilityDuration = 2f;
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

    private bool _disableActions = false;

    #region Attributes HUD
    [Header("HUD")]
    [SerializeField]
    private GameObject _hudContainer;
    [SerializeField]
    private TextMeshProUGUI _dnaHUDLabel;

    [SerializeField]
    private GameObject _deathOverlay;

    [SerializeField]
    private Image _damageOverlay;

    [SerializeField]
    private StaminaBar _staminaBar;
    #endregion Attributes HUD

    [Header("SFXs")]
    [SerializeField]
    private AudioClip _hurtSFX;
    [SerializeField]
    private AudioClip _deploySFX;

    protected override void Awake()
    {
        base.Awake();
        _transform = transform;
        _timeSinceLastDamage = 2f;
    }

    private void Start()
    {
        _flipper = GetComponent<AutoFlip>();
        _movement = GetComponent<Movement>();

        // TODO: use event
        _staminaBar.MaxValue = _data.MaxStamina;
        _staminaBar.Value = _data.Stamina;

        _playerInputActions = new();

        _playerInputActions.World.Enable();
        _playerInputActions.World.DeployFlare.performed += _ => DeployFlare();
        _playerInputActions.World.DeploySonar.performed += _ => DeploySonar();

        EventManager.Instance.FlashlightEquipped += OnFlashlightEquipped;
        EventManager.Instance.FlashlightUnequipped += OnFlashlightUnequipped;
        EventManager.Instance.PlayerActionsDisabled += OnDisableActions;
        EventManager.Instance.PlayerActionsEnabled += OnEnableActions;

        _playerVCamNoise = _playerVCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    void IBind<PlayerData>.Bind(PlayerData data)
    {
        _data = data;
        OnFlashlightEquipped(_data.FlashlightLevel);
    }

    private void Update()
    {
        if (_disableActions) return;

        _timeSinceLastDamage += Time.deltaTime;
        _timeSinceLastStaminaDrain += Time.deltaTime;
        if (_shouldRecover && HealthPoints < _data.MaxHealthPoints)
            Recover();
        if (_shouldRecoverStamina && Stamina < _data.MaxStamina)
            RecoverStamina();
    }

    // TODO: affect reveal light
    private void OnFlashlightEquipped(int level)
    {
        // TODO: refactor to static data
        switch (level)
        {
            case 1:
                _flashlight.intensity = 1f;
                _flashlight.pointLightOuterRadius = 5f;
                break;
            case 2:
                _flashlight.intensity = 1.1f;
                _flashlight.pointLightOuterRadius = 6f;
                break;
            case 3:
                _flashlight.intensity = 1.2f;
                _flashlight.pointLightOuterRadius = 7f;
                break;
        }
    }

    private void OnFlashlightUnequipped()
    {
        OnFlashlightEquipped(1);
    }

    private void OnDisableActions()
    {
        _disableActions = true;
        _movement.enabled = false;
        _hudContainer.SetActive(false);
    }

    private void OnEnableActions()
    {
        _disableActions = false;
        _movement.enabled = true;
        _hudContainer.SetActive(true);
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
        if (_isInvincible) return;

        AudioManager.Instance.PlaySFX(_hurtSFX);

        HealthPoints -= amount;
        _timeSinceLastDamage = 0f;
        if (HealthPoints == 0)
        {
            Die();
            return;
        }

        Shake();
        Blink();
        _isInvincible = true;
    }

    private async void Shake()
    {
        await Task.Yield();
        _playerVCamNoise.m_AmplitudeGain = 3;
        await Awaitable.WaitForSecondsAsync(0.4f);
        _playerVCamNoise.m_AmplitudeGain = 0;
    }

    private async void Blink()
    {
        await Task.Yield();
        float invincibleTime = _invincibilityDuration;
        Color color = _spriteRenderer.color;

        while (invincibleTime > 0)
        {
            color = color.With(a: 1 - color.a);
            _spriteRenderer.color = color;
            await Awaitable.WaitForSecondsAsync(invincibleTime > .2f ? .2f : invincibleTime);
            invincibleTime -= .2f;
        }

        _spriteRenderer.color = color.With(a: 1);
        _isInvincible = false;
    }

    public void Respawn()
    {
        Time.timeScale = 1;
        _movement.Stop();
        HealthPoints = _data.MaxHealthPoints;
        transform.position = _respawnPoint.position;
        CameraManager.Instance.ChooseCamera(_playerVCam);
        EventManager.Instance.Respawn();
    }

    private void Die()
    {
        Time.timeScale = 0;
        _deathOverlay.SetActive(true);
        EventManager.Instance.Die();
    }

    public void Shock(float duration)
    {
        _movement.Shock(duration);
    }

    public void DeployFlare()
    {
        if (_disableActions) return;
        if (!_canDeployFlare) return;
        if (ConsumablesManager.Instance.Flare == 0) return;
        ConsumablesManager.Instance.Flare -= 1;

        AudioManager.Instance.PlaySFX(_deploySFX);

        GameObject flare = Instantiate(_flarePrefab, _transform.position, Quaternion.identity);
        ArcLaunch arcLauncher = flare.GetComponent<ArcLaunch>();

        Vector2 direction = _playerInputActions.World.Move.ReadValue<Vector2>();

        if (direction.magnitude == 0)
        {
            float lookAngle = _flipper.Angle * Mathf.PI / 180;
            direction = new(Mathf.Cos(lookAngle), Mathf.Sin(lookAngle));
        }

        arcLauncher.Direction = direction;

        EventManager.Instance.DeployFlare();
        CooldownFlare();
    }

    private async void CooldownFlare()
    {
        _canDeployFlare = false;
        await Task.Yield();
        await Awaitable.WaitForSecondsAsync(_flareCooldown);
        _canDeployFlare = true;
        EventManager.Instance.CooldownFlare();
    }

    public void DeploySonar()
    {
        if (_disableActions) return;
        if (!_canDeploySonar) return;
        if (ConsumablesManager.Instance.SonarDrone == 0) return;
        ConsumablesManager.Instance.SonarDrone -= 1;

        AudioManager.Instance.PlaySFX(_deploySFX);

        GameObject sonar = Instantiate(_sonarPrefab, _transform.position, Quaternion.identity);
        ArcLaunch arcLauncher = sonar.GetComponent<ArcLaunch>();

        Vector2 direction = _playerInputActions.World.Move.ReadValue<Vector2>();

        if (direction.magnitude == 0)
        {
            float lookAngle = _flipper.Angle * Mathf.PI / 180;
            direction = new(Mathf.Cos(lookAngle), Mathf.Sin(lookAngle));
        }

        arcLauncher.Direction = direction;

        EventManager.Instance.DeploySonar();
        CooldownSonar();
    }

    private async void CooldownSonar()
    {
        _canDeploySonar = false;
        await Task.Yield();
        await Awaitable.WaitForSecondsAsync(_sonarCooldown);
        _canDeploySonar = true;
        EventManager.Instance.CooldownSonar();
    }
}
