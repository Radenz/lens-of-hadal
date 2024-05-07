using UnityEngine;

public class Decay : MonoBehaviour
{
    [SerializeField]
    private float _lifeTime;

    private void Start()
    {
        Timer.Instance.SetTimer(() => Destroy(gameObject), _lifeTime);
    }
}
