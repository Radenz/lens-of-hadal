using NaughtyAttributes;
using UnityEngine;

public class DebugActions : MonoBehaviour
{

    public int ExpAmount;
    [Button]
    public void GiveExp()
    {
        LevelManager.Instance.AddExp(ExpAmount);
    }
}
