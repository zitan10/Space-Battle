using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ShipMovement : MonoBehaviour
{
    [SerializeField]
    //left/Right
    private float _yawTorque = 5000f;
    [SerializeField]
    private float _pitchTorque = 5000f;
    [SerializeField]
    private float _rollTorque = 500f;
    [SerializeField]
    private float _thrust = 1000f;
    [SerializeField]
    private float _boostMultiplyer = 10f;
    [SerializeField]
    private float _upThrust = 50f;
    [SerializeField]
    private float _strafeThrust = 1000f;
    [SerializeField, Range(0.001f, 0.999f)]
    private float _thrustGlideReduction = 0.999f;
    [SerializeField, Range(0.001f, 0.999f)]
    private float _upDownReduction = 0.111f;
    [SerializeField, Range(0.001f, 0.999f)]
    private float _leftRightGlideReduction = 0.111f;

    private float _glide = 0f;
    private float _verticalGlide = 0f;
    private float _horizontalGlide = 0f;

    public float ThrustInput;
    public float UpDownInput;
    public float StrafeInput;
    public float RollInput;
    public float BoostInput;
    public Vector3 PitchYaw;

    Rigidbody _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        HandlePlayerInput();
    }

    void HandlePlayerInput()
    {
        _rb.AddRelativeTorque(Vector3.forward * -RollInput * _rollTorque * Time.deltaTime);
        _rb.AddRelativeTorque(Vector3.right * Mathf.Clamp(-PitchYaw.y, -1f, 1f) * _pitchTorque * Time.deltaTime);
        _rb.AddRelativeTorque(Vector3.up * Mathf.Clamp(PitchYaw.x, -1f, 1f) * _yawTorque * Time.deltaTime);

        if(ThrustInput != 0f)
        {
            float currentThust;
            if (BoostInput > 0)
            {
                currentThust = _thrust * _boostMultiplyer;
            }
            else
            {
                currentThust = _thrust;
            }
            _rb.AddRelativeForce(Vector3.forward * ThrustInput * currentThust);
            _glide = _thrust;
        }
        else
        {
            _rb.AddRelativeForce(Vector3.forward * _glide);
            _glide = _thrustGlideReduction;
        }

        // Up/Down
        var upForce = (UpDownInput != 0f) ? Vector3.up * UpDownInput * _upThrust : Vector3.up * _verticalGlide;
        _rb.AddRelativeForce(upForce);
        _verticalGlide *= _upDownReduction;

        // horizontal
        if (StrafeInput != 0f)
        {
            _rb.AddRelativeForce(Vector3.right * StrafeInput * _strafeThrust);
            _horizontalGlide = StrafeInput * _strafeThrust;
        }
        else
        {
            _rb.AddRelativeForce(Vector3.right * _horizontalGlide);
            _horizontalGlide *= _leftRightGlideReduction;
        }

    }
}
