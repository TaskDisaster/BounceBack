using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Controller
{
    public KeyCode moveForwardKey;
    public KeyCode moveBackwardKey;
    public KeyCode moveRightKey;
    public KeyCode moveLeftKey;
    public KeyCode swingKey;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        // Connect the player to the UIMAnager
        pawn.SetUIManager(FindAnyObjectByType<PlayerUIManager>());
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    public override void ProcessInputs()
    {
        // Rotation
        pawn.RotateToMouse();

        // Movement
        if (Input.GetKey(moveForwardKey))
        {
            pawn.MoveForward();
        }
        else if (Input.GetKey(moveBackwardKey))
        {
            pawn.MoveBackward();
        }

        if (Input.GetKey(moveRightKey))
        {
            pawn.MoveRight();
        }
        else if (Input.GetKey(moveLeftKey))
        {
            pawn.MoveLeft();
        }

        if (Input.GetKey(swingKey))
        {
            pawn.Shoot();
        }

    }
}
