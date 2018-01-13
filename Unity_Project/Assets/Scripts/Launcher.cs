using UnityEngine;

public class Launcher : MonoBehaviour
{
    [HideInInspector] public bool LeftSide;
    private Rigidbody2D _ballRigidbody2D;
    private Vector2 _appliedForce;

    public Rigidbody2D Shoot(GameObject ball)
    {
        _ballRigidbody2D = Instantiate(ball, transform.position, Quaternion.identity, transform).GetComponent<Rigidbody2D>();
        _appliedForce = LeftSide ? new Vector2(0.0f, 100.0f) : new Vector2(0.0f, -100.0f);
        _ballRigidbody2D.AddForce(_appliedForce);

        return _ballRigidbody2D;
    }
}
