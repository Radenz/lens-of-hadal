using System;
using System.Collections.Generic;
using UnityEngine;

public class ModuleSystem : Singleton<ModuleSystem>
{
    public static Module DraggedModule;
    public static Vector2Int DragPosition;

    [Header("Item Names")]
    public string FlashlightLv2 = "Flashlight2";

    private readonly List<ModuleGrid> _grids = new();

    public void RegisterGrid(ModuleGrid grid)
    {
        _grids.Add(grid);
        grid.Place += OnModulePlaced;
    }

    public bool TryPlaceModule(Module module, Vector2 position)
    {
        foreach (ModuleGrid grid in _grids)
        {
            if (grid.ContainsPoint(position))
                return grid.TryPlace(module, position);
        }
        return false;
    }

    // TODO: hook up with its effects
    private void OnModulePlaced(ModuleGrid grid, Module module)
    {
        if (grid.Name == "Upgrade")
            EventManager.Instance.EquipItem(module.Name);

        if (grid.Name == "Inventory")
            EventManager.Instance.UnequipItem(module.Name);
    }
}
