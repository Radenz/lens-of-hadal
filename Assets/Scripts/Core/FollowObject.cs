using UnityEngine;

public class FollowObject : MonoBehaviour
{
    [SerializeField]
    private Transform _objectToFollow;
    private Transform _transform;

    private void Awake()
    {
        _transform = transform;
    }

    private void Update()
    {
        _transform.position = _objectToFollow.position;
    }
}
