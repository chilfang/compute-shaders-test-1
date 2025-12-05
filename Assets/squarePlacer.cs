using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class squarePlacer : MonoBehaviour
{
    Texture2D texture;
    int textureSize = 128;

    // Start is called before the first frame update
    void Start()
    {
        print("Scene builder started");

        texture = new Texture2D(textureSize, textureSize, TextureFormat.RGBA32, false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaceSquareAtMouseLocation(InputAction.CallbackContext context) {
        if (context.phase == InputActionPhase.Started) {
            Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 position = new Vector2(worldPoint.x, worldPoint.y);

            var gameObject = new GameObject();
            var spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
            //spriteRenderer.color = Color.red;

            var sprite = Sprite.Create(texture, new Rect(0, 0, textureSize, textureSize), Vector2.zero);

            spriteRenderer.sprite = sprite;
            gameObject.transform.position = position - Vector2.one / 2;
        }
    }
}
