using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShipWeopon : MonoBehaviour
{

    [SerializeField]
    Transform _gunDownL;
    [SerializeField]
    Transform _gunDownR;
    [SerializeField]
    Transform _gunUpL;
    [SerializeField]
    Transform _gunUpR;

    [SerializeField]
    GameObject _projectile;

    public void FireProjectile()
    {

        var spawnProjectile1 = GameObject.Instantiate(_projectile, _gunDownL);
        spawnProjectile1.GetComponent<Rigidbody>().AddForce(spawnProjectile1.transform.forward * 1000f, ForceMode.Impulse);

        var spawnProjectile2 = GameObject.Instantiate(_projectile, _gunDownR);
        spawnProjectile2.GetComponent<Rigidbody>().AddForce(spawnProjectile2.transform.forward * 1000f, ForceMode.Impulse);

        var spawnProjectile3 = GameObject.Instantiate(_projectile, _gunUpL);
        spawnProjectile3.GetComponent<Rigidbody>().AddForce(spawnProjectile3.transform.forward * 1000f, ForceMode.Impulse);

        var spawnProjectile4 = GameObject.Instantiate(_projectile, _gunUpR);
        spawnProjectile4.GetComponent<Rigidbody>().AddForce(spawnProjectile4.transform.forward * 1000f, ForceMode.Impulse);
    }

}
