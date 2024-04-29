using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPawn : Pawn
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
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

        // Lock to ZX plane
        lookVector.y = 0;

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
        if (GetBallPrefab() != null)
        {
            if (Time.time > GetLastTimeShot() + GetFireCooldown())
            {
                // Instantiate our projectile
                GameObject newBall = ObjectPoolManager.Instance.GetObject(GetBallPrefab(), GetFirePoint().position, GetFirePoint().rotation);
                if (newBall == null)
                {
                    // Set the lastTimeShot to current time
                    SetLastTimeShot(Time.time);
                    return;
                }

                // Get balldata
                BouncyBallData data = Resources.Load("ScriptableObjects/BasicBall") as BouncyBallData;
                if (data == null)
                {
                    Debug.LogWarning("Missing BouncyBallData!");
                    return;
                }

                // Set the force
                Rigidbody rb = newBall.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.velocity = Vector3.zero;
                }

                // Initialize variables from data
                BouncyBall bouncyBall = newBall.GetComponent<BouncyBall>();
                if (bouncyBall != null)
                {
                    bouncyBall.Initialize(data);
                    bouncyBall.SetOwner(this);
                    bouncyBall.Activation();
                }

                // Play sound effect
                AudioManager.Instance.Play(effect);

                // Set the lastTimeShot to current time
                SetLastTimeShot(Time.time);
            }
        }
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

        if (GetCurrentHealth() <= 0)
        {
            Die();
        }
    }

    public override void Die()
    {
        // Destory game obejct
        Destroy(gameObject);
    }

    public override void Heal(float amount)
    {
        // Add healing to health
        SetCurrentHealth(GetCurrentHealth() + amount);
        // Make sure it stays within health parameters
        SetCurrentHealth(Mathf.Clamp(GetCurrentHealth(), 0, GetMaxHealth()));
    }
    #endregion

    #endregion
}
