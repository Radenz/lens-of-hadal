using System.Collections.Generic;
using UnityEngine;

public class Sonar : MonoBehaviour
{
    [SerializeField]
    private GameObject _sonarMarker;

    private readonly HashSet<Transform> _detectedCreatures = new();

    private void Start()
    {
        EventManager.Instance.SonarPinged += OnSonarPinged;
    }

    private void OnDestroy()
    {
        EventManager.Instance.SonarPinged -= OnSonarPinged;
    }

    private void OnSonarPinged()
    {
        Debug.Log($"Ping Received. Will spawn {_detectedCreatures.Count} markers.");
        foreach (Transform creatureTransform in _detectedCreatures)
        {
            Instantiate(_sonarMarker, creatureTransform.position, Quaternion.identity);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Creature")) return;
        _detectedCreatures.Add(other.transform);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Creature")) return;
        _detectedCreatures.Remove(other.transform);
    }
}
