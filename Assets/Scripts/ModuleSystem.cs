using System;
using System.Collections.Generic;
using UnityEngine;

public class ModuleSystem : Singleton<ModuleSystem>
{
    public static Module DraggedModule;
    public static Vector2Int DragPosition;

    public event Action<Module> Equip;
    public event Action<Module> Unequip;

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
            Equip?.Invoke(module);

        if (grid.Name == "Inventory")
            Unequip?.Invoke(module);
    }
}
