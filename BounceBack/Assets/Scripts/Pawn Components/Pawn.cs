using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Pawn : MonoBehaviour, IMovement, IShooter, IHealth
{
    [Header("Health Variables")]
    [SerializeField] private float currentHealth;
    [SerializeField] private float maxHealth;

    [Header("Movement Variables")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float turnSpeed;
    [SerializeField] private Rigidbody rb;
    private Vector3 mousePoint;

    [Header("Shooter Variables")]
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private RedirectBox swingBox;
    [SerializeField] private float fireCooldown;
    private float lastTimeShot;

    [Header("Misc Variables")]
    [SerializeField] private Controller controller;
    [SerializeField] private PlayerUIManager playerUIManager;
    public AudioClip effect;

    // Start is called before the first frame update
    public virtual void Start()
    {
        // Set up variables
        SetRigidBody(GetComponent<Rigidbody>());
        SetCurrentHealth(GetMaxHealth());
        SetLastTimeShot(Time.time);
    }

    // Update is called once per frame
    public virtual void Update()
    {
        
    }

    // Getters / Setters
    #region Getters / Setters

    #region Pawn
    public virtual void SetController(Controller _controller)
    {
        controller = _controller;
    }

    public virtual Controller GetController()
    {
        return controller;
    }
    #endregion Pawn

    #region Movement
    public virtual void SetRigidBody(Rigidbody rigidbody)
    {
        rb = rigidbody;
    }

    public virtual Rigidbody GetRigidBody()
    {
        return rb;
    }

    public virtual void SetMoveSpeed(float speed)
    {
        moveSpeed = speed;
    }

    public virtual float GetMoveSpeed()
    {
        return moveSpeed;
    }

    public virtual void SetTurnSpeed(float speed)
    {
        turnSpeed = speed;
    }

    public virtual float GetTurnSpeed()
    {
        return turnSpeed;
    }

    // Returns the point where the mouse is
    public virtual Vector3 GetMousePoint()
    {
        return mousePoint;
    }

    // Set the point where the mouse is
    public virtual void SetMousePoint(Vector3 targetPoint)
    {
        mousePoint = targetPoint;
    }
    #endregion Movement

    #region Shooter
    public virtual Transform GetFirePoint()
    {
        return firePoint;
    }
    public virtual RedirectBox GetSwingBox()
    {
        return swingBox;
    }

    public virtual GameObject GetBallPrefab()
    {
        return ballPrefab;
    }
    
    public virtual float GetFireCooldown()
    {
        return fireCooldown;
    }    

    public virtual void SetFireCooldown(float _fireCooldown)
    {
        fireCooldown = _fireCooldown;
    }

    public virtual float GetLastTimeShot()
    {
        return lastTimeShot;
    }

    public virtual void SetLastTimeShot(float _lastTimeShot)
    {
        lastTimeShot = _lastTimeShot;
    }
    #endregion

    #region Health
    public virtual float GetMaxHealth()
    {
        return maxHealth;
    }

    public virtual void SetMaxHealth(float _maxHealth)
    {
        maxHealth = _maxHealth;
    }

    public virtual float GetCurrentHealth()
    {
        return currentHealth;
    }

    public virtual void SetCurrentHealth(float _currentHealth)
    {
        currentHealth = _currentHealth;
    }

    public virtual void SetUIManager(PlayerUIManager ui)
    {
        playerUIManager = ui;
    }

    public virtual PlayerUIManager GetUIManager()
    {
        return playerUIManager;
    }
    #endregion

    #endregion Getters / Setters

    // Interface Implementations
    #region Interfaces

    // IMovement Implementations
    #region Movement
    public abstract void Move(Vector3 direction, float speed);
    public abstract void MoveForward();
    public abstract void MoveBackward();
    public abstract void MoveLeft();
    public abstract void MoveRight();
    public abstract void RotateToLookAt(Vector3 targetPoint);
    public abstract void RotateToMouse();
    #endregion

    // IShooter Implementations
    #region Shooter
    public abstract void Shoot();
    #endregion

    // IHealth IMplementations
    #region Health
    public abstract void TakeDamage(float amount);
    public abstract void Die();
    public abstract void Heal(float amount);
    #endregion

    #endregion Interfaces
}
