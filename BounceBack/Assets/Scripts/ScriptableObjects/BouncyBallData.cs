using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( fileName = "BouncyBall", menuName = "Entities/BouncyBall", order = 1)]
public class BouncyBallData : ScriptableObject
{
    public float speed;
    public float minSpeed;
    public float maxSpeed;
    public float damage;
    public float minDamage;
    public float maxDamage;
    public int bounces;
    public bool infiniteBounces;
}
