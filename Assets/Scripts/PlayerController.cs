using System;
using Enums;
using NaughtyAttributes;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform map;
    
    public Helix activeHelix;
    private float moveX;
    private Vector3 lastTapPos;
    [SerializeField] private float minRotationSpeed;
    [SerializeField] private float maxRotationSpeed;

    private void Update()
    {
        if (GameManager.instance.State != GameState.Playing) return;
        if (Input.GetMouseButton(0))
        {
            var curTapPos = Input.mousePosition;

            if (lastTapPos == Vector3.zero)
                lastTapPos = curTapPos;

            var delta = lastTapPos.x - curTapPos.x;
            delta = Math.Clamp(delta, minRotationSpeed, maxRotationSpeed);
            lastTapPos = curTapPos;

            transform.Rotate(Vector3.up * delta);
            map.Rotate(Vector3.up * delta);
        }

        if (Input.GetMouseButtonUp(0))
        {
            lastTapPos = Vector3.zero;
        }
    }
}