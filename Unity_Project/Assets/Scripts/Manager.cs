using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    private bool _ballInPlay;
    private Rigidbody2D _ballRigidbody;
    private BoxCollider2D _fieldCollider;
    private Launcher _launcher;
    private Goal _leftGoal;
    private int _leftScore;
    private Goal _rightGoal;
    private int _rightScore;
    
    public GameObject Ball;
    public GameObject CenterLauncher;
    public GameObject LeftGoal;
    public Text LeftScore;
    public GameObject RightGoal;
    public Text RightScore;

    private void Start()
    {
        _leftGoal = LeftGoal.GetComponent<Goal>();
        _rightGoal = RightGoal.GetComponent<Goal>();
        _launcher = CenterLauncher.GetComponent<Launcher>();
        _ballInPlay = false;
        _leftScore = 0;
        _rightScore = 0;
        LeftScore.text = _leftScore.ToString();
        RightScore.text = _rightScore.ToString();

        StartCoroutine(StartShoot());
        _ballInPlay = true;
    }

    private void Update()
    {

        if (_leftGoal.MadeGoal && !_ballInPlay)
        {
            StartCoroutine(Shoot(_leftGoal, false));
            return;
        }

        if (_rightGoal.MadeGoal && !_ballInPlay)
        {
            StartShoot();
        }
        
        if (!_ballInPlay)
        {
            _ballInPlay = true;
            StartCoroutine(Shoot(_rightGoal, true));
            return;
        }

        if (_ballRigidbody != null && _ballRigidbody.position.y > 0.001)
            _ballRigidbody.AddForce(new Vector2(0.0f, 16.0f));
        
        if (_ballRigidbody != null && _ballRigidbody.position.y < -0.001)
            _ballRigidbody.AddForce(new Vector2(0.0f, -16.0f));

    }

    private IEnumerator Shoot(Goal side, bool left)
    {
        _ballInPlay = true;

        if (side.Equals(_leftGoal))
        {
            _rightScore++;
            RightScore.text = _rightScore.ToString();
        }
        else
        {
            _leftScore++;
            LeftScore.text = _leftScore.ToString();
        }

        yield return new WaitForSeconds(1.0f);

        side.MadeGoal = false;
        _launcher.LeftSide = left;
        _ballRigidbody = _launcher.Shoot(Ball);
    }

    private IEnumerator StartShoot()
    {
        yield return new WaitForSeconds(3.0f);
        
        
        var startingSide = Random.Range(0, 2);
        switch (startingSide)
        {
            case 0:
            {
                _launcher.LeftSide = false;
                _ballRigidbody = _launcher.Shoot(Ball);
                _ballInPlay = true;
                break;
            }
            case 1:
            {
                _launcher.LeftSide = true;
                _ballRigidbody = _launcher.Shoot(Ball);
                _ballInPlay = true;
                break;
            }
        }
    }


    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Ball")) return;
        
        _ballInPlay = true;
        //print("ball is in play?" + _ballInPlay);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Ball")) return;
        
        _ballInPlay = false;
        Destroy(transform.GetChild(4).GetChild(0).gameObject);
        //print("ball is in play?" + _ballInPlay);
    }
}