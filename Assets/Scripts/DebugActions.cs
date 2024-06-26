using System.ComponentModel;
using NaughtyAttributes;
using UnityEngine;

public class DebugActions : MonoBehaviour
{
    private void Start()
    {
        Application.targetFrameRate = 60;
    }

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
    // [Button]
    // public void UnlockItem()
    // {
    //     EventManager.Instance.UnlockItem(ItemId);
    // }

    // [Button]
    // public void AssembleItem()
    // {
    //     EventManager.Instance.AssembleItem(ItemId);
    // }

    [Header("Mutant Anglerfish")]
    [Label("Spawn Point")]
    public Vector3 AnglerfishSpawnPoint;
    public GameObject AnglerfishPrefab;
    [Button]
    public void SpawnMutantAnglerfish()
    {
        Instantiate(AnglerfishPrefab, AnglerfishSpawnPoint, Quaternion.identity);
    }
}
