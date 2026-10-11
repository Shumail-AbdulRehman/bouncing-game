using UnityEngine;

// Stores the initial state of a physical ball; never supplies artificial bounce forces.
[RequireComponent(typeof(Rigidbody), typeof(SphereCollider))]
public class DropExperiment : MonoBehaviour
{
    public string displayName;
    public int contactCount { get; private set; }
    public float Height => transform.position.y;
    private Rigidbody rigidBody;
    private Vector3 releasePoint;

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        releasePoint = transform.position;
    }

    void OnCollisionEnter(Collision hit)
    {
        if (hit.contactCount > 0 && hit.GetContact(0).normal.y > 0.5f)
            contactCount++;
    }

    public void ReleaseAgain()
    {
        rigidBody.position = releasePoint;
        rigidBody.rotation = Quaternion.identity;
        rigidBody.linearVelocity = Vector3.zero;
        rigidBody.angularVelocity = Vector3.zero;
        rigidBody.WakeUp();
        contactCount = 0;
    }
}
