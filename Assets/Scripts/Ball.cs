using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private GameController gameController;

    private Rigidbody ballRigidbody;

    // The ball spawn anchor is constantly moving left to right, using the animator to do so.
    private Transform ballSpawnAnchor;

    public void Restart()
    {
        transform.parent = ballSpawnAnchor;

        // By setting the rigidbody to kinematic, we allow the ball to follow the spawn anchor movement animation
        // without disrupting the physics simulation
        ballRigidbody.isKinematic = true;

        // Kinematic objects aren't affected by external forces, and are driven by code to initiate their physical movement
        // But since the ball may already have some velocities, these would keep affecting it
        // That's why we make sure to set it back to 0
        ballRigidbody.velocity = Vector3.zero;
        ballRigidbody.angularVelocity = Vector3.zero;

        // Making sure that both the transform and physical simulation have their position updated.
        transform.localPosition = Vector3.zero;
        ballRigidbody.position = transform.position;
    }

    public void ThrowBall()
    {
        transform.parent = null;
        ballRigidbody.isKinematic = false;
    }

    // The pins have a capsule collider, that when hit with the ball will trigger this method:
    // https://docs.unity3d.com/ScriptReference/Collider.OnCollisionEnter.html
    private void OnCollisionEnter(Collision collision)
    {
        Pin pin = collision.gameObject.GetComponent<Pin>();

        if (pin != null)
            gameController.AddScore(pin);
    }

    // The finish line on the other hand, has a trigger added to a child object called "TriggerArea"
    // When the ball enters that trigger area, this method is called:
    // https://docs.unity3d.com/ScriptReference/Collider.OnTriggerEnter.html
    private void OnTriggerEnter(Collider other)
    {
        // To help identify which object the trigger belongs to, we add a tag to it:
        // https://docs.unity3d.com/Manual/Tags.html
        if (other.CompareTag("FinishLine"))
            gameController.EndGame();
    }

    private void Awake()
    {
        ballRigidbody = GetComponent<Rigidbody>();
        ballSpawnAnchor = transform.parent;

        // This object doesn't have any update methods, so keeping it enabled is just a waste of resources.
        enabled = false;
    }
}
