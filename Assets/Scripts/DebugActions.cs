using System.ComponentModel;
using NaughtyAttributes;
using UnityEngine;

public class DebugActions : MonoBehaviour
{
    [Header("Exp")]
    [Label("Amount")]
    public int ExpAmount;
    [Button]
    public void GiveExp()
    {
        LevelManager.Instance.AddExp(ExpAmount);
    }

    [Header("Shop Item")]
    [Label("Id")]
    public string ItemId;
    [Button]
    public void UnlockItem()
    {
        EventManager.Instance.UnlockItem(ItemId);
    }

    [Button]
    public void AssembleItem()
    {
        EventManager.Instance.AssembleItem(ItemId);
    }
}
