using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPawn : Pawn
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        // Setup HealthUI Variable
        if (GetUIManager() != null)
        {
            GetUIManager().healthBar.fillAmount = 1;
        }
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    // Interface Implementations
    #region Interface Implementations

    // IMovement Implementations
    #region Movement Implementations
    public override void Move(Vector3 direction, float speed)
    {
        Vector3 moveVector = direction.normalized * speed * Time.deltaTime;

        Vector3 changedVector = GetRigidBody().transform.InverseTransformDirection(moveVector);

        GetRigidBody().MovePosition(GetRigidBody().position + changedVector);
    }

    public override void MoveBackward()
    {
        Move(-GetRigidBody().transform.forward, GetMoveSpeed());
    }

    public override void MoveForward()
    {
        Move(GetRigidBody().transform.forward, GetMoveSpeed());
    }

    public override void MoveLeft()
    {
        Move(-GetRigidBody().transform.right, GetMoveSpeed());
    }

    public override void MoveRight()
    {
        Move(GetRigidBody().transform.right, GetMoveSpeed());
    }

    public override void RotateToLookAt(Vector3 targetPoint)
    {
        // Finde the vector from our position to the target point
        Vector3 lookVector = targetPoint - GetRigidBody().transform.position;

        // Find the rotation that will look down that vector with world up being the up direction
        Quaternion lookRotation = Quaternion.LookRotation(lookVector, Vector3.up);

        // Rotate slightly towards that target rotation
        GetRigidBody().transform.rotation = Quaternion.RotateTowards(GetRigidBody().transform.rotation, lookRotation, GetTurnSpeed() * Time.deltaTime);
    }

    public override void RotateToMouse()
    {
        if (GetRigidBody() == null)
        {
            return;
        }

        // Create the Ray from the mouse position in the direction the camera is facing
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Create a plane at our feet, and a normal world up
        Plane footPlane = new Plane(Vector3.up, GetRigidBody().transform.position);

        // Find the distance down that ray that the plane and ray intersect
        float distanceToIntersect;

        // Find where they intersect
        footPlane.Raycast(mouseRay, out distanceToIntersect);

        // Find the intersection point
        Vector3 intersectionPoint = mouseRay.GetPoint(distanceToIntersect);

        // Look at the intersection point
        RotateToLookAt(intersectionPoint);

        // Make the mouse point public for public use
        SetMousePoint(intersectionPoint);
    }
    #endregion

    // IShooter Implementations
    #region Shooter Implementations
    public override void Shoot()
    {
        if (GetSwingBox() != null)
        {
            if (Time.time > GetLastTimeShot() + GetFireCooldown())
            {
                // Activate the box
                GetSwingBox().ActivateObject();

                // Play sound effect
                AudioManager.Instance.Play(effect);

                // Set lastTimeShot to current time
                SetLastTimeShot(Time.time);
            }
        }
    }

    public void OnPulledTrigger()
    {

    }
    #endregion

    // IHealth Implementations
    #region Health Implementations
    public override void TakeDamage(float amount)
    {
        // Minus damage from health
        SetCurrentHealth(GetCurrentHealth() - amount);
        // Make sure it stays within parameters
        SetCurrentHealth(Mathf.Clamp(GetCurrentHealth(), 0, GetMaxHealth()));

        // Change the health bar to match
        if (GetUIManager() != null)
        {
            GetUIManager().healthBar.fillAmount = GetCurrentHealth() / GetMaxHealth();
        }

        // Dead if dead
        if (GetCurrentHealth() <= 0)
        {
            Die();
        }
    }

    public override void Die()
    {
        // Game Over
        GameManager.Instance.ChangeState(GameManager.GameState.GameOver);

        // Destory game obejct
        Destroy(gameObject);
    }

    public override void Heal(float amount)
    {
        // Add healing to health
        SetCurrentHealth(GetCurrentHealth() + amount);
        // Make sure it stays within health parameters
        SetCurrentHealth(Mathf.Clamp(GetCurrentHealth(), 0, GetMaxHealth()));

        // Change the health bar to match
        if (GetUIManager() != null)
        {
            GetUIManager().healthBar.fillAmount = GetCurrentHealth() / GetMaxHealth();
        }
    }
    #endregion

    #endregion
}
