using System;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class CellPlacer : MonoBehaviour {

    //--------------------------------[[ Structs ]]--------------------------------

    public struct GridPositionInfo {
        public float x, y;
        public int gridX, gridY;

        public GridPositionInfo (float x, float y, int gridX, int gridY) {
            this.x = x;
            this.y = y;
            this.gridX = gridX;
            this.gridY = gridY;
        }
    }

    public class CellInfo {
        public int gridX, gridY;
        public GameObject gameObject;
        public CellType cellType;

        public SpriteRenderer spriteRenderer;

        public CellInfo (int gridX, int gridY, GameObject gameObject, CellType cellType) {
            this.gridX = gridX;
            this.gridY = gridY;
            this.gameObject = gameObject;
            this.spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            this.cellType = cellType;
        }
    }

    //--------------------------------[[ Enums ]]--------------------------------

    public enum CellType {
        Air = 0,
        Sand = 1,
        Stone = 2,
    }

    //--------------------------------[[ Variables ]]--------------------------------

    public int cellSize;
    private const int initialCellSize = 128;
    private float cellScale;

    public int gridWidth = 200;
    public int gridHeight = 200;
    public float gridScale = 2f;

    private GameObject CellsHolderObject;

    public CellInfo[,] CellsArray;

    private bool holdingClick = false;

    private System.Random random = new System.Random();

    //brush stuff
    public int brushSize = 5;
    public CellType brushType = CellType.Sand;


    //--------------------------------[[ Engine ]]--------------------------------

    void Start () {
        print("Scene builder started");

        cellSize = math.min(Screen.width, Screen.height) / math.min(gridWidth, gridHeight);
        cellSize = math.max(1, cellSize);

        cellScale = (float) cellSize / initialCellSize * gridScale;

        CellsHolderObject = GameObject.Find("Cells");

        CellsArray = new CellInfo[gridWidth, gridHeight];

        for (int x = -gridWidth / 2; x < gridWidth / 2; x++) {
            for (int y = -gridHeight / 2; y < gridHeight / 2; y++) {
                CellInfo cell;
                try {
                    cell = (CellInfo) PlaceCellAtLocation(x, y);
                }
                catch {
                    throw new Exception("Couldn't make cell??? what????");
                }

                var spriteRenderer = cell.gameObject.GetComponent<SpriteRenderer>();

                //float colorNum = (float) random.Next(30, 70) / 100;
                //spriteRenderer.color = new Color(colorNum, colorNum, colorNum);
                spriteRenderer.color = Color.white;


            }
        }
    }

    void Update () {
        //-----------------------[[ Physics ]]-----------------------
        for (int x = 0; x < gridWidth; x++) {
            for (int y = 0; y < gridHeight; y++) {
                CellInfo cell = CellsArray[x, y];

                switch (cell.cellType) {
                    case CellType.Air:
                        break;

                    case CellType.Stone:
                        break;

                    case CellType.Sand:
                        if (y > 0 && CellsArray[x, y - 1].cellType == CellType.Air) {
                            SwapCells(x, y, x, y - 1);
                        }
                        break;
                }
            }
        }



        //-----------------------[[ Click stuff ]]-----------------------
        if (holdingClick) {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            float x = mousePosition.x / cellScale;
            float y = mousePosition.y / cellScale;


            if (brushSize == 1) {
                CellInfo cell = GetCellAtLocation(x, y);

                if (cell != null) {
                    SetCellToType(cell, brushType);
                }
            } else {
                for (float brushX = (-brushSize / 2) + x; brushX < (brushSize / 2) + x; brushX++) {
                    for (float brushY = (-brushSize / 2) + y; brushY < (brushSize / 2) + y; brushY++) {
                        if (random.Next(0, 100) < 75) { continue; }
                        
                        CellInfo cell = GetCellAtLocation(brushX, brushY);

                        if (cell == null) { continue; }

                        SetCellToType(cell, brushType);
                    }
                }
            }
        }
    }

    public void OnValidate () {
        print("hi");
    }

    //--------------------------------[[ Functions ]]--------------------------------

    //----------------[[ Inputs ]]----------------

    public void OnClick (InputAction.CallbackContext context) {
        if (context.phase == InputActionPhase.Started) {
            holdingClick = true;
        }
        else if (context.phase == InputActionPhase.Canceled || (context.interaction is TapInteraction && context.phase == InputActionPhase.Performed)) {
            holdingClick = false;
        }
    }

    public void BrushSlider (Single value) {
        ChangeBrushSize((int) value);
    }

    //----------------[[ Settings stuff ]]----------------

    public void ChangeBrushSize (int newBrushSize) {
        brushSize = math.max(1, newBrushSize);
    }

    public void SwitchBrushToElement (CellType cellType) {
        brushType = cellType;
        print($"Switch to {cellType}");
    }

    //----------------[[ Grid Stuff ]]----------------

    public GridPositionInfo ConvertScaledWorldToGrid (float x, float y) {
        x = math.round(x);
        y = math.round(y);

        return new GridPositionInfo(x, y, (int) x + (gridWidth / 2), (int) y + (gridHeight / 2));
    }

    public CellInfo GetCellAtLocation (float _x, float _y) {
        var gridPositions = ConvertScaledWorldToGrid(_x, _y);

        int gridX = gridPositions.gridX;
        int gridY = gridPositions.gridY;

        if ((gridX >= gridWidth || gridY >= gridHeight) || (gridX < 0 || gridY < 0)) {
            //print($"Attempt to get outside grid: ({gridX}, {gridY})");
            return null;
        }

        CellInfo cell;

        try {
            cell = CellsArray[gridX, gridY];
        } catch {
            print($"Null reference(?) at ({gridX}, {gridY}) | THIS SHOULD NOT HAPPEN");
            cell = PlaceCellAtLocation(_x, _y);
        }

        return cell;
    }

    public CellInfo PlaceCellAtLocation (float _x, float _y, Color? color = null) {
        if (color == null) {
            color = Color.white;
        }

        var gridPositions = ConvertScaledWorldToGrid(_x, _y);

        float x = gridPositions.x;
        float y = gridPositions.y;

        int gridX = gridPositions.gridX;
        int gridY = gridPositions.gridY;

        if (gridX >= gridWidth || gridY >= gridHeight) {
            print($"Placed outside grid: ({gridX}, {gridY})");
            return null;
        }

        Vector2 position = new Vector2(x * cellScale, y * cellScale);
        GameObject gameObject = new GameObject();
        SpriteRenderer spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        spriteRenderer.color = (Color) color;

        var sprite = Resources.Load<Sprite>("Sprites/TestSquare1");

        spriteRenderer.sprite = sprite;
        gameObject.transform.position = position;
        gameObject.transform.parent = CellsHolderObject.transform;
        gameObject.transform.localScale = Vector3.one * cellScale;
        gameObject.name = "Cell";

        CellsArray[gridX, gridY] = new CellInfo(gridX, gridY, gameObject, CellType.Air);

        return CellsArray[gridX, gridY];
    }

    public CellInfo SetCellToType (CellInfo cell, CellType cellType) {
        if (cell.cellType == cellType) { return cell; }

        switch (cellType) {
            case CellType.Air:
                cell.cellType = CellType.Air;
                cell.spriteRenderer.color = Color.white;
                break;

            case CellType.Sand:
                cell.cellType = CellType.Sand;
                cell.spriteRenderer.color = new Color(1, (float) random.Next(85, 100) / 100, (float) random.Next(0, 75) / 100);
                break;

            case CellType.Stone:
                cell.cellType = CellType.Stone;
                cell.spriteRenderer.color = Color.gray;
                break;

            default:
                throw new Exception($"Unknown cellType: {cellType}");
        }

        return cell;
    }

    public void SwapCells (int gridX1, int gridY1, int gridX2, int gridY2) {
        CellInfo cell1 = CellsArray[gridX1, gridY1];
        CellType cellType1 = cell1.cellType;
        Color color1 = cell1.spriteRenderer.color;

        CellInfo cell2 = CellsArray[gridX2, gridY2];

        cell1.cellType = cell2.cellType;
        cell1.spriteRenderer.color = cell2.spriteRenderer.color;

        cell2.cellType = cellType1;
        cell2.spriteRenderer.color = color1;
    }
}
