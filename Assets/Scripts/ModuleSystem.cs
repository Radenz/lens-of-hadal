using System.Collections.Generic;
using UnityEngine;

// TODO: add DraggedModule's ModuleGrid source
public class ModuleSystem : Singleton<ModuleSystem>
{
    public static Module DraggedModule;
    public static Vector2Int DragPosition;

    private readonly List<ModuleGrid> _grids = new();

    public void RegisterGrid(ModuleGrid grid)
    {
        _grids.Add(grid);
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
}
