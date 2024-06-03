using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TrailerIntro : MonoBehaviour
{
    [SerializeField]
    private Light2D _playerLight;
    [SerializeField]
    private SpriteRenderer _playerSprite;

    [SerializeField]
    private GameObject _sonar;

    [SerializeField]
    private GameObject _piranha;
    [SerializeField]
    private GameObject _eel;
    [SerializeField]
    private GameObject _bladefish;

    [SerializeField]
    private Transform _sonarPlacement;

    [SerializeField]
    private List<Transform> _spawnPoints;

    private List<GameObject> _creatures = new();
    private GameObject _instSonar;

    private async void Start()
    {
        await Awaitable.WaitForSecondsAsync(1.5f);

        await DOTween.To(
             () => _playerLight.intensity,
             intensity => _playerLight.intensity = intensity,
             1f,
             1
         )
         .OnUpdate(() =>
         {
             _playerSprite.color = Color.white.With(a: _playerLight.intensity);
         })
         .SetEase(Ease.InQuad).AsyncWaitForCompletion();

        _instSonar = Instantiate(_sonar, _sonarPlacement.position, Quaternion.identity);

        // await Awaitable.WaitForSecondsAsync(2f);
        // Spawn batch: 1 3 6
        Debug.Log("?");

        _creatures.Add(Instantiate(_piranha, _spawnPoints[0].position, _spawnPoints[0].rotation));

        await Awaitable.WaitForSecondsAsync(2f);
        Debug.Log("??");

        _creatures.Add(Instantiate(_eel, _spawnPoints[1].position, _spawnPoints[1].rotation));
        _creatures.Add(Instantiate(_piranha, _spawnPoints[2].position, _spawnPoints[2].rotation));

        await Awaitable.WaitForSecondsAsync(2f);
        Debug.Log("???");

        _creatures.Add(Instantiate(_bladefish, _spawnPoints[3].position, _spawnPoints[3].rotation));
        _creatures.Add(Instantiate(_piranha, _spawnPoints[4].position, _spawnPoints[4].rotation));
        _creatures.Add(Instantiate(_eel, _spawnPoints[5].position, _spawnPoints[5].rotation));

        await Awaitable.WaitForSecondsAsync(2f);

        await DOTween.To(
             () => _playerLight.intensity,
             intensity => _playerLight.intensity = intensity,
             0f,
             1
         )
         .OnUpdate(() =>
         {
             _playerSprite.color = Color.white.With(a: _playerLight.intensity);
         })
         .SetEase(Ease.InQuad).AsyncWaitForCompletion();

        await Awaitable.WaitForSecondsAsync(.5f);
        _creatures.ForEach(c => Destroy(c));
        Destroy(_instSonar);
    }
}
