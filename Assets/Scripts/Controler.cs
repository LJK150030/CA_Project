using UnityEngine;

public class Controler : MonoBehaviour
{
    private float _currentTime;
    private Vector2 _displacementVector;
    private Vector2 _inputForce;
    private Vector3 _inputLeftDirection;
    private Vector3 _inputRightDirection;
    private Vector2 _shiftDirection;
    private Transform _leftPoint;
    private float _lerpAngle;
    private Vector2 _midpointDvector;
    private float _newAngle;
    private float _oldInputAngle;
    private float _oldAngle;
    private Rigidbody2D _paddleCenterRigidbody;
    private Rigidbody2D _paddlesRigidbody;
    private Transform _rightPoint;
    private float _rate;
    private Transform _square;
    private SpriteRenderer _squareSprite;
    private float _timeLeft;

    public bool LeftPlayer = true;
    public string AButton = "AButton_P1";
    public string LJoyX = "Joystick_L_X_P1";
    public string LJoyY = "Joystick_L_Y_P1";
    public string RJoyX = "Joystick_R_X_P1";
    public string RJoyY = "Joystick_R_Y_P1";
    public string Trigger = "Triggers_P1";
    public float Magnification;
    public float Threshold;
    public float SecondsBetweenMovements;

    private void Start()
    {
        _paddleCenterRigidbody = transform.GetChild(0).GetComponent<Rigidbody2D>();
        _paddlesRigidbody = transform.GetChild(0).GetChild(0).GetComponent<Rigidbody2D>();
        _square = transform.GetChild(1);
        _squareSprite = _square.GetComponent<SpriteRenderer>();
        _leftPoint = transform.GetChild(2);
        _rightPoint = transform.GetChild(3);
        _currentTime = 0.0f;
        _oldAngle = 0.0f;
        _oldInputAngle = 0.0f;
        _rate = 1.0f/SecondsBetweenMovements;
        _shiftDirection = Vector2.zero;
    }

    //Update moves the objects to it's destination
    private void Update()
    {
        //Show the square to indicate which side the player is on
        _squareSprite.enabled = Input.GetButton(AButton);

        //Set the point to the center with an offset based on the controller input
        _leftPoint.position = _paddleCenterRigidbody.position +
                              new Vector2(_inputLeftDirection.x, _inputLeftDirection.y) * Magnification;
        _rightPoint.position = _paddleCenterRigidbody.position +
                               new Vector2(_inputRightDirection.x, _inputRightDirection.y) * Magnification;

        //apply a rotation to the paddles
        _paddlesRigidbody.MoveRotation(_lerpAngle);
        _paddleCenterRigidbody.MovePosition(_shiftDirection);

        //Apply a force to the paddle
        //_paddleCenterRigidbody.AddForce(new Vector2(_inputForce.x, 0.0f));
    }

    //Late update calculates where the object is going to go
    private void LateUpdate()
    {
        //Get the players input
        if (LeftPlayer)
        {
            _inputLeftDirection.x = Input.GetAxis(LJoyX) * -1.0f;
            _inputLeftDirection.y = Input.GetAxis(LJoyY);
            _inputRightDirection.x = Input.GetAxis(RJoyX) * -1.0f;
            _inputRightDirection.y = Input.GetAxis(RJoyY);
            _shiftDirection.x = Input.GetAxis(Trigger) * -1.75f;
        }
        else
        {
            _inputLeftDirection.x = Input.GetAxis(LJoyX) * -1.0f;
            _inputLeftDirection.y = Input.GetAxis(LJoyY);
            _inputRightDirection.x = Input.GetAxis(RJoyX) * -1.0f;
            _inputRightDirection.y = Input.GetAxis(RJoyY);
            _shiftDirection.x = Input.GetAxis(Trigger) * 1.75f;
        }

        //if the input is less than the threshold, then do not apply it
        if (_inputLeftDirection.magnitude < Threshold)
            _inputLeftDirection = new Vector2(0.0f, 0.0f);

        if (_inputRightDirection.magnitude < Threshold)
            _inputRightDirection = new Vector2(0.0f, 0.0f);

        //calculating the displacment vector between the two points
        _displacementVector = _leftPoint.position - _rightPoint.position;
        _displacementVector.Normalize();

        //calculate the current angle the input is in
        _newAngle = Mathf.Atan2(_displacementVector.y, _displacementVector.x) * Mathf.Rad2Deg;
        
        //lerp the paddles to the new location
        
       
        
        //if the rotation is not the same from the last frame
        if (Mathf.Abs(_newAngle - _oldInputAngle) > 0.1)
        {
            _oldInputAngle = _newAngle;
            _currentTime = 0.0f;
            _oldAngle = _paddlesRigidbody.rotation;
            _currentTime += Time.deltaTime * _rate;
        }
        else
        {
            _currentTime += Time.deltaTime * _rate;

            if (_currentTime >= 1.0f)
                _currentTime = 1.0f;
        }
        
        
        _lerpAngle = Mathf.LerpAngle(_oldAngle, _newAngle, _currentTime);
        
            

        //finding the midpoint of the displacemnt vector
        _midpointDvector = new Vector2((_leftPoint.position.x + _rightPoint.position.x) / 2,
            (_leftPoint.position.y + _rightPoint.position.y) / 2);
        //creating the vector between the midpoint and the center of the paddles
        _inputForce = _midpointDvector - _paddleCenterRigidbody.position;
    }

    private void OnDrawGizmos()
    {
        if (_leftPoint == null || _rightPoint == null) return;
        
        Gizmos.color = Color.green;
        Gizmos.DrawLine(_leftPoint.position, _rightPoint.position);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(_paddlesRigidbody.position, _midpointDvector);
    }
}