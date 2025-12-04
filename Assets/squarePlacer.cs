using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class squarePlacer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        print("Scene builder started");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnFire(InputAction.CallbackContext context) {
        Debug.Log(context);
        Debug.Log("Fired");
    }
}
