using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.U2D;

public class squarePlacer : MonoBehaviour {
    public int cellSize;
    private const int initialCellSize = 128;

    public int gridWidth = 200;
    public int gridHeight = 200;

    private GameObject Cells;

    // Start is called before the first frame update
    void Start () {
        print("Scene builder started");

        cellSize = cellSize - (cellSize % 2);

        cellSize = math.min(Screen.width, Screen.height) / math.min(gridWidth, gridHeight);
        cellSize = math.max(1, cellSize);

        Cells = GameObject.Find("Cells");

        var random = new System.Random();

        for (int x = -gridWidth / 2; x < gridWidth / 2; x++) {
            for (int y = -gridHeight / 2; y < gridHeight / 2; y++) {
                var cell = PlaceSquareAtLocation(new Vector2(x, y));
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
            Vector2 position = new Vector2(mousePosition.x, mousePosition.y);

            PlaceSquareAtLocation(position);
        }
    }

    public GameObject PlaceSquareAtLocation(Vector2 position) {
        var gameObject = new GameObject();
        var spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        //spriteRenderer.color = Color.red;

        //var sprite = Sprite.Create(texture, new Rect(0, 0, textureSize, textureSize), Vector2.zero);

        var sprite = Resources.Load<Sprite>("Sprites/TestSquare1");

        spriteRenderer.sprite = sprite;
        gameObject.transform.position = position;
        gameObject.transform.parent = Cells.transform;
        //gameObject.transform.localScale = Vector3.one * cellSize / initialCellSize;
        gameObject.name = "Cell";

        return gameObject;
    }
}
