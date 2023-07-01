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
    float _projectileForce = 1500f;

    [SerializeField]
    PlayerProjectile _projectile;

    Transform _projectileDefaultPosition;

    ObjectPool<PlayerProjectile> _gunDownLProjectilePool;
    ObjectPool<PlayerProjectile> _gunDownRProjectilePool;
    ObjectPool<PlayerProjectile> _gunUpLProjectilePool;
    ObjectPool<PlayerProjectile> _gunUpRProjectilePool;

    private const int INITIAL_OBJECT_POOL_COUNT = 100;

    private void Awake()
    {
        _gunDownLProjectilePool = new ObjectPool<PlayerProjectile>(_projectile, _gunDownL, INITIAL_OBJECT_POOL_COUNT);
        _gunDownRProjectilePool = new ObjectPool<PlayerProjectile>(_projectile, _gunDownR, INITIAL_OBJECT_POOL_COUNT);
        _gunUpLProjectilePool = new ObjectPool<PlayerProjectile>(_projectile, _gunUpL, INITIAL_OBJECT_POOL_COUNT);
        _gunUpRProjectilePool = new ObjectPool<PlayerProjectile>(_projectile, _gunUpR, INITIAL_OBJECT_POOL_COUNT);

        _projectileDefaultPosition = _projectile.transform;
    }

    public void FireProjectile()
    {
        FireProjectileFromGun(_gunDownLProjectilePool, _gunDownL);
        FireProjectileFromGun(_gunDownRProjectilePool, _gunDownR);
        FireProjectileFromGun(_gunUpLProjectilePool, _gunUpL);
        FireProjectileFromGun(_gunUpRProjectilePool, _gunUpR);
    }

    private void FireProjectileFromGun(ObjectPool<PlayerProjectile> projectilePool, Transform gunPosition)
    {
        PlayerProjectile projectile = projectilePool.GetObjectFromPool();
        projectile.SetCb(() => projectilePool.ReturnObjectToPool(projectile));
        ResetProjectile(projectile);
        projectile.GetComponent<Rigidbody>().AddForce(gunPosition.transform.forward * _projectileForce, ForceMode.Impulse);
    }

    public void ReturnProjectileToPool(ObjectPool<PlayerProjectile> pool, PlayerProjectile playerProjectile)
    {
        pool.ReturnObjectToPool(playerProjectile);
    }

    private void ResetProjectile(PlayerProjectile playerProjectile)
    {
        playerProjectile.transform.localPosition = _projectileDefaultPosition.position;
        playerProjectile.Reset();
    }

}
