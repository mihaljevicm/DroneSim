using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HinputClasses;

[RequireComponent(typeof(Rigidbody))]
public class DroneController : MonoBehaviour
{
    [Range(30f, 500f)]
    public float motorForce = 30f;
    public Transform[] motors;
    public DataManager dataManager;
    public EventManager eventManager;

    private Rigidbody _rb;
    private Transform _transform;
    private Quaternion _initialDroneRotation;
    private float _tiltAmountX;
    private float _tiltAmountZ;
    private float _rotateAmountY;
    private float _tiltVelocity;
    private float _rotationSpeed;
    private int _rotateDegrees = 20;
    private float _targetYRotation;

    private Gamepad gamepad { get { return Hinput.gamepad[0]; } }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _transform = transform;
    }

    private void Start()
    {
        _initialDroneRotation = _transform.localRotation;

        dataManager.LoadData();
        if (dataManager.data != null)
        {
            motorForce = dataManager.data.enginePower;
            _rotateDegrees = dataManager.data.maxTiltAngleXZ;
            _rb.mass = dataManager.data.droneMass;
            _rotationSpeed = dataManager.data.rotationSpeed;
        }
    }

    private void Update()
    {
        if(gamepad.B.justPressed)
        {
            eventManager.QuitGame();
        }
    }

    private void FixedUpdate()
    {
        VerticalMovement();
        AddThrust();
        Stabilization();
    }

    private void VerticalMovement()
    {
        //turn around Y axis
        if (gamepad.leftStick.horizontal != 0)
        {
            _targetYRotation += gamepad.leftStick.horizontal;

            _rotateAmountY = Mathf.SmoothDamp(_rotateAmountY, _targetYRotation, ref _tiltVelocity, _rotationSpeed);
        }

        //force along Y motor axis
        if (gamepad.leftStick.vertical > 0)
        {
            foreach (Transform motor in motors)
            {
                _rb.AddForceAtPosition(new Vector3(0, gamepad.leftStick.vertical * motorForce, 0), motor.position);
            }
        }

        //idle
        //else if (Mathf.Round(gamepad.leftStick.vertical) == 0 && Mathf.Round(_rb.velocity.y) != 0 )
        //{
        //    foreach (Transform motor in motors)
        //    {
        //        _rb.AddForceAtPosition(new Vector3(0, -Physics.gravity.y * _rb.mass/motors.Length, 0), motor.position, ForceMode.Force);
        //    }
        //}
    }

    private void AddThrust()
    {
        //Rotate along Z axis, tilt left and right
        if (gamepad.rightStick.horizontal != 0)
        {
            _rb.AddRelativeForce(Vector3.right * motorForce * gamepad.rightStick.horizontal);

            _tiltAmountZ = Mathf.SmoothDamp(_tiltAmountZ, _rotateDegrees * gamepad.rightStick.horizontal, ref _tiltVelocity, _rotationSpeed);

        }

            //Rotate along X axis, forward and backward
        if (gamepad.rightStick.vertical != 0)
        {
            _rb.AddRelativeForce(Vector3.forward * motorForce * gamepad.rightStick.vertical);

            _tiltAmountX = Mathf.SmoothDamp(_tiltAmountX, _rotateDegrees * gamepad.rightStick.vertical, ref _tiltVelocity, _rotationSpeed);
        }

    }

    private void Stabilization()
    {
        if(gamepad.rightStick.vertical == 0)
        {
            _tiltAmountX = Mathf.SmoothDamp(_tiltAmountX, _initialDroneRotation.x, ref _tiltVelocity, _rotationSpeed);
        }

        if(gamepad.rightStick.horizontal == 0)
        {
            _tiltAmountZ = Mathf.SmoothDamp(_tiltAmountZ, _initialDroneRotation.z, ref _tiltVelocity, _rotationSpeed);
        }

        _transform.rotation = Quaternion.Euler(new Vector3(_tiltAmountX, _rotateAmountY, -_tiltAmountZ));
    }
}
