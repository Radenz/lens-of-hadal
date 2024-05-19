using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using Pathfinding;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class MutantAnglerfishAI : MonoBehaviour
{
    [Header("Config")]
    [SerializeField]
    private float _damage = 50;
    [SerializeField]
    private float _dashImpulse = 20;

    [Header("Others")]
    [SerializeField]
    private Rigidbody2D _rigidbody;

    [SerializeField]
    private Scannable _scannable;

    private Vector2 _spawnBoundMin;
    private Vector2 _spawnBoundMax;

    [SerializeField]
    private float _roamingRadius;
    [SerializeField]
    private float _retreatRadius;

    [SerializeField]
    private float _lureLightIntensity = 0.5f;

    [SerializeField]
    private RangeTrigger _attackHitbox;

    [SerializeField]
    private AIPath _ai;

    [SerializeField]
    private GameObject _sprite;

    private Transform _transform;
    private Transform _player;

    public Vector3 SpawnPoint;

    [SerializeField]
    private Light2D _lure;

    private MutantAnglerfishState _state = MutantAnglerfishState.Hidden;
    private bool _isMoving = false;
    private bool _canAttack = false;

    private ManagedTween _lightTween = new();
    private bool _isDestroyed = false;

    private void Start()
    {
        _transform = transform;

        GameObject obj = GameObject.FindGameObjectWithTag("MutantAnglerfishSpawnBound");
        BoxCollider2D boundCollider = obj.GetComponent<BoxCollider2D>();
        _spawnBoundMin = boundCollider.bounds.min;
        _spawnBoundMax = boundCollider.bounds.max;

        // _aggresionRadius = _aggresionRange.GetComponent<CircleCollider2D>().radius;
        _attackHitbox.Entered += OnAttack;
        // TODO: refactor, use singleton player
        _player = GameObject.FindGameObjectWithTag("Player").transform;

        // _ai.maxSpeed = _speed;
        // _ai.destination = _transform.RandomWithinRadius(_roamingRadius);
        _ai.enabled = false;

        _isMoving = true;
        Shine(2f).OnComplete(Hide);
    }

    private void Update()
    {
        if (_isMoving) return;
        if (_state == MutantAnglerfishState.Hidden)
        {
            Emerge();
            return;
        }
        Hide();
    }

    private void OnDestroy()
    {
        _lightTween.Kill();
        _isDestroyed = true;
    }

    private void OnAttack()
    {
        if (!_canAttack) return;
        PlayerController.Instance.Damage(_damage);
    }

    private Vector2 PickEmergePoint()
    {
        float x = Random.Range(_spawnBoundMin.x, _spawnBoundMax.x);
        float y = Random.Range(_spawnBoundMin.y, _spawnBoundMax.y);
        return new Vector2(x, y);
    }

    private async void Emerge()
    {
        _scannable.gameObject.SetActive(true);
        await Task.Yield();

        _isMoving = true;
        _state = MutantAnglerfishState.Emerged;
        _sprite.SetActive(true);
        _transform.position = PickEmergePoint();
        _canAttack = true;

        Tween shine = Shine();
        _lightTween.Kill();
        _lightTween.Play(shine);
        await shine.AsyncWaitForCompletion();

        Vector3 direction = _player.position - _transform.position;
        _rigidbody.AddForce(direction.normalized * _dashImpulse);
        await Awaitable.WaitForSecondsAsync(1.5f);
        _isMoving = false;
    }

    private async void Hide()
    {
        await Task.Yield();
        _isMoving = true;
        _state = MutantAnglerfishState.Hidden;

        Tween dim = Dim();
        _lightTween.Kill();
        _lightTween.Play(dim);
        await dim.AsyncWaitForCompletion();
        if (_isDestroyed) return;

        _scannable.gameObject.SetActive(false);
        _canAttack = false;
        _sprite?.SetActive(false);
        _rigidbody.velocity = Vector3.zero;

        await Awaitable.WaitForSecondsAsync(2f);
        _isMoving = false;
    }

    private Tween Shine(float duration = 1f)
    {
        return DOTween.To(
            () => _lure.intensity,
            intensity => _lure.intensity = intensity,
            _lureLightIntensity,
            duration
        ).SetEase(Ease.InQuad);
    }

    private Tween Dim(float duration = 1f)
    {
        return DOTween.To(
            () => _lure.intensity,
            intensity => _lure.intensity = intensity,
            0f,
            duration
        ).SetEase(Ease.OutQuad);
    }
}

public enum MutantAnglerfishState
{
    Emerged,
    Hidden
}
