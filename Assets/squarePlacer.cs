using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class squarePlacer : MonoBehaviour {
    public int cellSize;
    private const int initialCellSize = 128;
    private float cellScale;

    public int gridWidth = 200;
    public int gridHeight = 200;

    private GameObject CellsHolderObject;

    public GameObject[,] CellsArray;

    // Start is called before the first frame update
    void Start () {
        print("Scene builder started");

        cellSize = cellSize - (cellSize % 2);

        cellSize = math.min(Screen.width, Screen.height) / math.min(gridWidth, gridHeight);
        cellSize = math.max(1, cellSize);

        cellScale = (float) cellSize / initialCellSize * 1.5f;

        CellsHolderObject = GameObject.Find("Cells");

        CellsArray = new GameObject[gridWidth,gridHeight];

        var random = new System.Random();

        for (int x = -gridWidth / 2; x < gridWidth / 2; x++) {
            for (int y = -gridHeight / 2; y < gridHeight / 2; y++) {
                var cell = PlaceSquareAtLocation(x, y);
                var spriteRenderer = cell.GetComponent<SpriteRenderer>();

                float colorNum = (float) random.Next(30, 70) / 100;
                spriteRenderer.color = new Color(colorNum, colorNum, colorNum);


            }
        }

        //Cells.transform.localScale = Vector3.one * cellSize / initialCellSize;
    }

    // Update is called once per frame
    void Update () {

    }

    public void PlaceSquareAtMouseLocation(InputAction.CallbackContext context) {
        if (context.phase == InputActionPhase.Started) {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            PlaceSquareAtLocation(mousePosition.x, mousePosition.y);
        }
    }

    public GameObject PlaceSquareAtLocation(float x, float y, bool addToArray = true) {
        x = math.round(x);
        y = math.round(y);

        var position = new Vector2(x * cellScale, y * cellScale);
        var gameObject = new GameObject();
        var spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        //spriteRenderer.color = Color.red;

        //var sprite = Sprite.Create(texture, new Rect(0, 0, textureSize, textureSize), Vector2.zero);

        var sprite = Resources.Load<Sprite>("Sprites/TestSquare1");

        spriteRenderer.sprite = sprite;
        gameObject.transform.position = position;
        gameObject.transform.parent = CellsHolderObject.transform;
        gameObject.transform.localScale = Vector3.one * cellScale;
        gameObject.name = "Cell";

        if (addToArray) {
            CellsArray[(int) x + gridWidth / 2, (int) y + gridHeight / 2] = gameObject;
        }

        return gameObject;
    }
}
