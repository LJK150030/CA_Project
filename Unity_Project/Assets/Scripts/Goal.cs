using UnityEngine;

public class Goal : MonoBehaviour
{
    [HideInInspector] public bool MadeGoal;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ball"))
            MadeGoal = true;
    }
}