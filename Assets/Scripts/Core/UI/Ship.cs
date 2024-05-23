using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Ship : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _shipRenderer;
    [SerializeField]
    private List<GameObject> _parts;

    private Color _dim = new(.25f, .25f, .25f, 1f);

    public void HighlightParts()
    {
        _shipRenderer.DOColor(_dim, 2f).SetEase(Ease.InQuad);
        foreach (GameObject part in _parts)
        {
            part.SetActive(true);
        }
    }

    public void UnhighlightParts()
    {
        _shipRenderer.DOColor(Color.white, 2f);
        foreach (GameObject part in _parts)
        {
            part.SetActive(false);
        }
    }
}
