using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BouncyBall : MonoBehaviour
{
    public BouncyBallData data;
    [SerializeField] private float speed;
    [SerializeField] private float minSpeed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float damage;
    [SerializeField] private float minDamage;
    [SerializeField] private float maxDamage;
    [SerializeField] private int bounces;
    [SerializeField] private bool infiniteBounces;
    [SerializeField] private Pawn owner;
    [SerializeField] private Rigidbody rb;

    public UnityEvent OnTriggerEvent;
    public UnityEvent OnBounceEvent;

    public AudioClip sfx;

    private void Awake()
    {
        // Setup Variables
        SetDamage(GetMinDamage());
        SetSpeed(GetMinSpeed());

        // Other
        OnBounceEvent.AddListener(Bounce);
        OnBounceEvent.AddListener(PlaySound);
        SetRigidBody(GetComponent<Rigidbody>());
    }

    private void Start()
    {

    }

    private void Update()
    {

    }

    // Bouncy Ball Methods
    public void Bounce()
    {
        // Remove one bounce
        bounces -= 1;
    }

    public void PlaySound()
    {
        AudioManager.Instance.Play(sfx);
    }

    public void AddBounce()
    {
        bounces += 1;
    }

    public void Activation()
    {
        SetDamage(GetMinDamage());
        SetSpeed(GetMinSpeed());
        rb.AddForce(rb.transform.forward.normalized * speed, ForceMode.VelocityChange);
    }

    public void AddBounce(int amount)
    {
        bounces += amount;
    }

    public void Reverse()
    {
        // Go the opposite way
        rb.velocity = -rb.velocity;
    }

    public void ResetVelocity()
    {
        rb.velocity = Vector3.zero;
    }

    public void IncrementDamage()
    {
        damage += 10;
        damage = Mathf.Clamp(damage, minDamage, maxDamage);
    }

    public void IncrementDamage(int amount)
    {
        damage += amount;
        damage = Mathf.Clamp(damage, minDamage, maxDamage);
    }

    public void IncrementSpeed()
    {
        speed += 5;
        speed = Mathf.Clamp(speed, minSpeed, maxSpeed);
        Vector3 direction = rb.velocity;
        rb.velocity = Vector3.zero;
        rb.AddForce(direction.normalized * speed, ForceMode.VelocityChange);
    }

    public void IncrementSpeed(int amount)
    {
        speed += amount;
        speed = Mathf.Clamp(speed, minSpeed, maxSpeed);
        Vector3 direction = rb.velocity;
        rb.velocity = Vector3.zero;
        rb.AddForce(direction.normalized * speed, ForceMode.VelocityChange);
    }

    public void Redirect(Vector3 position)
    {
        // Get the new direction to specified point
        Vector3 newDirection = position - rb.position;
        // Normalize the new direction
        newDirection.Normalize();
        // Match the newDirection's magnitude with the current speed
        newDirection *= rb.velocity.magnitude;
        // set the velocity to the new direction;
        rb.velocity = newDirection;
    }

    public void OnTriggerEnter(Collider other)
    {
        // Check if it has a FactionComp
        if (other.GetComponent<FactionComp>() == null)
        {
            return;
        }

        // Get the faction
        Faction otherFaction = FactionManager.Instance.GetFaction(other.gameObject);

        // Get owner faction
        Faction ownerFaction = FactionManager.Instance.GetFaction(owner);

        // Ignore same faction
        if (otherFaction == ownerFaction)
        {
            return;
        }

        // Get pawn
        Pawn otherPawn = other.GetComponent<Pawn>();

        // Check if it isn't a pawn
        if (otherPawn == null)
        {
            return;
        }

        // Damage pawn
        otherPawn.TakeDamage(damage);

        // Invoke UnityEvent
        OnTriggerEvent.Invoke();

        // Deactivate self once done
        gameObject.SetActive(false);
    }

    public void OnCollisionEnter(Collision other)
    {
        if (bounces <= 0 && infiniteBounces == false)
        {
            gameObject.SetActive(false);
        }

        OnBounceEvent.Invoke();
    }

    public void Initialize(BouncyBallData data)
    {
        SetSpeed(data.speed);
        SetMinSpeed(data.minSpeed);
        SetMaxSpeed(data.maxSpeed);
        SetDamage(data.damage);
        SetMinDamage(data.minDamage);
        SetMaxDamage(data.maxDamage);
        SetBounces(data.bounces);
    }

    // Getters / Setters
    #region Getters / Setters
    public float GetSpeed()
    {
        return speed;
    }

    public void SetSpeed(float _speed)
    {
        speed = _speed;
    }

    public float GetMinSpeed()
    {
        return minSpeed;
    }

    public void SetMinSpeed(float _minSpeed)
    {
        minSpeed = _minSpeed;
    }

    public float GetMaxSpeed()
    {
        return maxSpeed;
    }

    public void SetMaxSpeed(float _maxSpeed)
    {
        maxSpeed = _maxSpeed;
    }

    public float GetDamage()
    {
        return damage;
    }

    public void SetDamage(float _damage)
    {
        damage = _damage;
    }

    public float GetMinDamage()
    {
        return minDamage;
    }

    public void SetMinDamage(float _minDamage)
    {
        minDamage = _minDamage;
    }

    public float GetMaxDamage()
    {
        return maxDamage;
    }

    public void SetMaxDamage(float _maxDamage)
    {
        maxDamage = _maxDamage;
    }

    public int GetBounces()
    {
        return bounces;
    }

    public void SetBounces(int _bounces)
    {
        bounces = _bounces;
    }

    public Rigidbody GetRigidBody()
    {
        return rb;
    }

    public void SetRigidBody(Rigidbody _rb)
    {
        rb = _rb;
    }

    public void SetInfiniteBounces(bool _infiniteBounces)
    {
        infiniteBounces = _infiniteBounces;
    }

    public Pawn GetOwner()
    {
        return owner;
    }

    public void SetOwner(Pawn _owner)
    {
        owner = _owner;
    }
    #endregion
}
