using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class RedirectBox : MonoBehaviour
{
    public Pawn owner;
    public LayerMask ballLayer = 7;
    public AudioClip effect;

    public void ActivateRedirect()
    {
        // Get the collider presenting the box
        BoxCollider physicalBox = GetComponent<BoxCollider>();

        // Define the size and position of the triggerbox
        Vector3 hitBox = physicalBox.size;
        Vector3 hitBoxPos = physicalBox.transform.position;

        // Get all colliders within the trigger box;
        Collider[] hitColliders = Physics.OverlapBox(hitBoxPos, hitBox / 2, Quaternion.identity);

        foreach (var hitCollider in hitColliders)
        {
            // check if the collider is a bouncy ball
            BouncyBall ball = hitCollider.GetComponent<BouncyBall>();
            if (ball != null)
            {
                ball.Redirect(owner.GetMousePoint());
                ball.SetOwner(owner);
                ball.IncrementDamage();
                ball.IncrementSpeed();
            }
            // Play Sound effect
            AudioManager.Instance.Play(effect);
        }

        // Deactivate when done
        gameObject.SetActive(false);
    }

    public void ActivateObject()
    {
        gameObject.SetActive(true);
        ActivateRedirect();
    }
}
