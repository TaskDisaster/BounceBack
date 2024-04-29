using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMovement
{
    void Move(Vector3 direction, float speed);
    void MoveForward();
    void MoveBackward();
    void MoveLeft();
    void MoveRight();
    void RotateToLookAt(Vector3 targetPoint);
    void RotateToMouse();
}
