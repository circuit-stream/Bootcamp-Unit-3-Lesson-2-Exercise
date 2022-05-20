using System;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private GameController gameController;

    private Rigidbody ballRigidbody;
    private Transform ballSpawnAnchor;

    public void ThrowBall()
    {
        transform.parent = null;
        ballRigidbody.isKinematic = false;
    }

    public void Restart()
    {
        transform.parent = ballSpawnAnchor;

        ballRigidbody.isKinematic = true;
        ballRigidbody.velocity = Vector3.zero;
        ballRigidbody.angularVelocity = Vector3.zero;

        transform.localPosition = Vector3.zero;
        ballRigidbody.position = transform.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Pin pin = collision.gameObject.GetComponent<Pin>();

        if (pin != null)
            gameController.AddScore(pin);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FinishLine"))
            gameController.EndGame();
    }

    private void Awake()
    {
        ballRigidbody = GetComponent<Rigidbody>();
        ballSpawnAnchor = transform.parent;

        enabled = false;
    }
}
