using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    [SerializeField]
    private float _aliveTime = 5f;

    private float _timer = 0f;

    private Action _cb;

    private void OnEnable()
    {
        _timer = 0f;
    }

    private void Update()
    {
        _timer += Time.deltaTime;
        if (_timer > _aliveTime)
        {
            _cb();
        }
    }

    public void SetCb(Action cb)
    {
        _cb = cb;
    }

    private void OnCollisionEnter(Collision collision)
    {
        _cb();
    }
}
