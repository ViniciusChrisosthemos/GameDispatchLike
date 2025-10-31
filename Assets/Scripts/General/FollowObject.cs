using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private Transform _source;

    [SerializeField] private bool _keepOffset = true;

    private Vector3 _offset;

    private void Awake()
    {
        _offset = _keepOffset ? (_source.position - _target.position) : Vector3.zero;
    }

    private void Update()
    {
        _source.position = _target.position + _offset;
        _source.rotation = _target.rotation;
    }
}