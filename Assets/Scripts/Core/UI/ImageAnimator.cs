using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageAnimator : MonoBehaviour
{
    [SerializeField]
    private Image _image;

    [SerializeField]
    private List<Sprite> _sprites;

    [SerializeField]
    private float _fps;

    private float Delay => 1f / _fps;
    private float _time = 0f;
    private int _index = 0;

    private void Start()
    {
        Debug.Log("Fps: " + _fps);
    }

    private void Update()
    {
        _time += Time.unscaledDeltaTime;
        while (_time >= Delay)
        {
            _time -= Delay;
            _index += 1;
            _index %= _sprites.Count;
            _image.sprite = _sprites[_index];
        }
    }
}
