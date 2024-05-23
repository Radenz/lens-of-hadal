using System.Collections.Generic;
using UnityEngine;

public class Sonar : MonoBehaviour
{
    [SerializeField]
    private GameObject _sonarMarker;
    [SerializeField]
    private GameObject _spriteOutlineMarker;

    // private readonly HashSet<Transform> _detectedCreatures = new();
    private readonly HashSet<CreatureDespawner> _detectedCreatures = new();

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
        foreach (CreatureDespawner creature in _detectedCreatures)
        {
            Instantiate(_sonarMarker, creature.Position, Quaternion.identity);
            GameObject obj = Instantiate(
                _spriteOutlineMarker,
                creature.SpriteTransform.position,
                creature.SpriteTransform.rotation
                );

            obj.transform.localScale = creature.SpriteTransform.lossyScale;
            obj.GetComponent<SpriteRenderer>().sprite = creature.Creature.Sprite;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Creature")) return;
        _detectedCreatures.Add(other.GetComponent<CreatureDespawner>());
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Creature")) return;
        _detectedCreatures.Remove(other.GetComponent<CreatureDespawner>());
    }
}
