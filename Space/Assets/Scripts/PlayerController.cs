using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ShipMovement))]
public class PlayerController : MonoBehaviour
{

    ShipMovement _shipMovement;
    PlayerShipWeopon _playerShipWeopon;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        _shipMovement = GetComponent<ShipMovement>();
        _playerShipWeopon = GetComponent<PlayerShipWeopon>();
    }

    private void Update()
    {
        _shipMovement.ThrustInput = Input.GetAxis("Vertical");
        _shipMovement.StrafeInput = Input.GetAxis("Horizontal");
        _shipMovement.UpDownInput = Input.GetAxis("UpDown");
        _shipMovement.RollInput = Input.GetAxis("Roll");
        _shipMovement.BoostInput = Input.GetAxis("Boost");
        _shipMovement.PitchYaw = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            _playerShipWeopon.FireProjectile();
        
    }

}
