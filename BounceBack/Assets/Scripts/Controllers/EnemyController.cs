using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : Controller
{
    #region Enemy Variables
    [Header("Enemy Variables")]
    [SerializeField] private GameObject target;
    private Vector3 targetPosition;
    private Vector3 moveDirection;
    [SerializeField] float changeDirCooldown;
    [SerializeField] float changeMinTime;
    [SerializeField] float changeMaxTime;
    private float lastChangeTime;
    #endregion

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        // Set up variables
        changeDirCooldown = 0;
        targetPosition = target.transform.position;
        moveDirection = targetPosition;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    public override void ProcessInputs()
    {
        // check if we have a target
        if (target == null)
        {
            return;
        }

        // Get target position
        targetPosition = target.transform.position;

        // Look at target
        pawn.RotateToLookAt(targetPosition);

        // Check if it's time to change direction
        if (Time.time > lastChangeTime + changeDirCooldown)
        {
            // Get random direction
            Vector3 randomDirection = Random.insideUnitCircle;
            // REmove height changes
            randomDirection.y = 0;
            // Normalize it
            randomDirection.Normalize();

            moveDirection = randomDirection;

            // Set this to current time
            lastChangeTime = Time.time;

            // Get new time
            changeDirCooldown = Random.Range(changeMinTime, changeMaxTime);
        }

        // Move it
        pawn.Move(moveDirection, pawn.GetMoveSpeed());

        // Shoot balls
        pawn.Shoot();
    }

    public override void PossessPawn(Pawn pawnToPossess)
    {
        base.PossessPawn(pawnToPossess);
    }

    // Getters / Setters
    #region Getters / Setters
    public Vector3 GetTargetPosition()
    {
        return targetPosition;
    }

    public void SetTargetPosition(Vector3 _targetPosition)
    {
        targetPosition = _targetPosition;
    }

    public void SetTarget(GameObject _target)
    {
        target = _target;
    }

    public GameObject GetTarget()
    {
        return target;
    }
    #endregion
}
