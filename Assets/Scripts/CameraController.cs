using System;
using System.Collections;
using Enums;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Ball ball;
    [SerializeField] private float offsetY;
    [SerializeField] private float passiveSpeed;
    [SerializeField] private bool isPassiveSpeedOn;
    [SerializeField] private float cameraThreshold;

    private float ballLastPosY;

    private void Start()
    {
        ScoreManager.instance.OnScored += OnScored;
    }

    private void OnScored(int score)
    {
        passiveSpeed += 0.05f;
    }

    private void Update()
    {
        if (GameManager.instance.State != GameState.Playing) return;

        ballLastPosY = ball.transform.position.y;
        var transformPosition = transform.position;

        if (isPassiveSpeedOn)
            transform.position += Vector3.down * (passiveSpeed * Time.deltaTime);

        if (ballLastPosY - transformPosition.y > cameraThreshold)
            GameManager.instance.GameOver();


        if (transformPosition.y > ballLastPosY + offsetY)
        {
            transform.position = new Vector3(transformPosition.x, ballLastPosY + offsetY,
                transformPosition.z);
        }
    }
}