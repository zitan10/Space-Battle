using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurrentControl : MonoBehaviour
{
    private Transform _playerTransform;
    [SerializeField]
    private float _maxDistanceToLookForPlayer = 3000f;
    [SerializeField]
    private float _rotationSpeed = 200f;
    [SerializeField]
    private float _dotProductRotationLimit = 0f;
    [SerializeField]
    private Transform _gunTransform;
    [SerializeField]
    EnemyProjectile _projectile;
    [SerializeField]
    private Transform _barrelTransform;
    [SerializeField]
    float _projectileForce = 2500f;

    Transform _projectileDefaultPosition;
    private float _timer;
    private float _turrentWaitTime = 1f;

    private ObjectPool<EnemyProjectile> _projectilePool;
    private const int INITIAL_OBJECT_POOL_COUNT = 100;

    private Vector3 _startingForwardVector;

    void Start()
    {
        _playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();
        _startingForwardVector = transform.forward;
        _projectilePool = new ObjectPool<EnemyProjectile>(_projectile, _barrelTransform, INITIAL_OBJECT_POOL_COUNT);
        _projectileDefaultPosition = _projectile.transform;
        _timer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, _playerTransform.position);
        Vector3 playerDirection = (_playerTransform.position - transform.position).normalized;
        float dotProduct = Vector3.Dot(_startingForwardVector, playerDirection);

        Debug.Log("dot is: " + dotProduct);

        if (distanceToPlayer < _maxDistanceToLookForPlayer && dotProduct > _dotProductRotationLimit)
        {
            // Calculate the direction only on the y-axis
            Vector3 playerDirectionOnY = new Vector3(playerDirection.x, 0f, playerDirection.z).normalized;

            // Create a rotation only on the y-axis
            Quaternion targetRotation = Quaternion.LookRotation(playerDirectionOnY, Vector3.up);

            // Apply rotation using Slerp
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);

            RotateGun();
        }

    }

    private void RotateGun()
    {
        // Calculate the target rotation
        Quaternion targetRotation = Quaternion.LookRotation(_playerTransform.position - _gunTransform.position);

        // Wrap the target rotation on the x-axis within -180 to 180 degrees
        float targetRotationX = targetRotation.eulerAngles.x;
        if (targetRotationX > 180)
            targetRotationX -= 360;

        bool fireProjectile = (targetRotationX < 60 && targetRotationX  > -60);

        // Clamp the rotation on the x-axis if it exceeds the maximum value
        float clampedRotationX = Mathf.Clamp(targetRotationX, -60, 60);
        targetRotation = Quaternion.Euler(clampedRotationX, targetRotation.eulerAngles.y, targetRotation.eulerAngles.z);

        // Smoothly rotate towards the target rotation using Slerp
        _gunTransform.rotation = Quaternion.Slerp(_gunTransform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);


        if (fireProjectile)
        {
            FireProjectile();
        }
    }

    private void FireProjectile()
    {
        _timer += Time.deltaTime;
        if (_timer > _turrentWaitTime)
        {
            EnemyProjectile projectile = _projectilePool.GetObjectFromPool();
            projectile.SetCb(() => _projectilePool.ReturnObjectToPool(projectile));
            ResetProjectile(projectile);
            projectile.GetComponent<Rigidbody>().AddForce(_barrelTransform.forward * _projectileForce, ForceMode.Impulse);
            _timer = 0f;
        }
    }

    private void ResetProjectile(EnemyProjectile enemyProjectile)
    {
        enemyProjectile.transform.localPosition = _projectileDefaultPosition.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(LayerMask.LayerToName(collision.gameObject.layer) == "PlayerProjectile")
        {
            transform.gameObject.SetActive(false);
        }
    }

}
