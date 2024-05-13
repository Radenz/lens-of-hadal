using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using Pathfinding;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class MutantAnglerfishAI : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D _rigidbody;

    [SerializeField]
    private float _roamingRadius;
    [SerializeField]
    private float _retreatRadius;
    private float _aggresionRadius;

    [SerializeField]
    private float _lureLightIntensity = 0.5f;


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

    private void Start()
    {
        _transform = transform;

        // _aggresionRadius = _aggresionRange.GetComponent<CircleCollider2D>().radius;

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

    private async void Emerge()
    {
        await Task.Yield();
        _isMoving = true;
        _state = MutantAnglerfishState.Emerged;
        _sprite.SetActive(true);
        Vector3 position = _player.RandomOnRadius(8f);
        _transform.position = position;
        await Shine().AsyncWaitForCompletion();
        Vector3 direction = _player.position - _transform.position;
        Vector3 finalPosition = direction.normalized * 8 + _player.position;
        await _transform.DOMove(finalPosition, 2f).SetEase(Ease.Linear).AsyncWaitForCompletion();
        _isMoving = false;
    }

    private async void Hide()
    {
        await Task.Yield();
        _isMoving = true;
        _state = MutantAnglerfishState.Hidden;
        await Dim().AsyncWaitForCompletion();
        _sprite.SetActive(false);
        await Task.Delay(2000);
        _isMoving = false;
    }

    private Tween Shine(float duration = 1f)
    {
        return DOTween.To(() => _lure.intensity, intensity => _lure.intensity = intensity, _lureLightIntensity, duration).SetEase(Ease.InQuad);
    }

    private Tween Dim(float duration = 1f)
    {
        return DOTween.To(() => _lure.intensity, intensity => _lure.intensity = intensity, 0f, duration).SetEase(Ease.OutQuad);
    }
}

public enum MutantAnglerfishState
{
    Emerged,
    Hidden
}
