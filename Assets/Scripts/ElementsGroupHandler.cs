using System;
using UnityEngine;
using UnityEngine.UI;

public class ElementsGroupHandler : MonoBehaviour
{
    [SerializeField]
    private CellPlacer cellPlacer;

    public void SwitchToAir (Boolean toggleStatus) {
        if (toggleStatus) {
            cellPlacer.SwitchBrushToElement(CellPlacer.CellType.Air);
        }
    }

    public void SwitchToSand (Boolean toggleStatus) {
        if (toggleStatus) {
            cellPlacer.SwitchBrushToElement(CellPlacer.CellType.Sand);
        }
    }

    public void SwitchToStone (Boolean toggleStatus) {
        if (toggleStatus) {
            cellPlacer.SwitchBrushToElement(CellPlacer.CellType.Stone);
        }
    }
}
