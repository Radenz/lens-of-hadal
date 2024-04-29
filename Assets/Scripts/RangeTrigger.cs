using System;
using UnityEngine;

public class RangeTrigger : MonoBehaviour
{
    public event Action Entered;
    public event Action Stay;
    public event Action Exited;

    [SerializeField]
    private string _tag;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_tag.Length != 0 && !other.CompareTag(_tag)) return;
        Entered?.Invoke();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (_tag.Length != 0 && !other.CompareTag(_tag)) return;
        Stay?.Invoke();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (_tag.Length != 0 && !other.CompareTag(_tag)) return;
        Exited?.Invoke();
    }
}
