using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    public Transform target;
    public float speed;
    public float distance;

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            // Calculate where our camera wants to be
            Vector3 newPosition = new Vector3(target.position.x, target.position.y + distance, target.position.z);

            // Move towards the target position
            transform.position = Vector3.Lerp(transform.position, newPosition, speed * Time.deltaTime);
        }

    }
}
