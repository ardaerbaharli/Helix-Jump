using System;
using System.Collections;
using Enums;
using UnityEngine;
using Utilities;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Ball ball;
    [SerializeField] private float offsetY;
    [SerializeField] private float passiveSpeedInitial;
    [SerializeField] public bool isPassiveSpeedOn;
    [SerializeField] private float cameraThreshold;

    private float passiveSpeed;
    private float ballLastPosY;

    private void Awake()
    {
        passiveSpeed = passiveSpeedInitial;
        ballLastPosY = ball.transform.position.y;
    }

    private void Start()
    {
        ScoreManager.instance.OnScored += OnScored;
        GameManager.instance.OnRewardedAdCompleted += OnRewardedAdCompletedCallback;
    }

    private void OnDestroy()
    {
        ScoreManager.instance.OnScored -= OnScored;
        GameManager.instance.OnRewardedAdCompleted -= OnRewardedAdCompletedCallback;
    }

    private void OnRewardedAdCompletedCallback()
    {
        StartCoroutine(MoveTheCameraToBall());
    }

    private IEnumerator MoveTheCameraToBall()
    {
        var targetPosY = ball.transform.position.y + offsetY;
        var targetPos = new Vector3(transform.position.x, targetPosY, transform.position.z);


        var duration = 1f;


        var startPosition = transform.position;
        var t1 = 0f;
        while (t1 < duration)
        {
            t1 += Time.deltaTime;
            transform.position = Vector3.Lerp(startPosition, targetPos, t1 / duration);
            yield return null;
        }

        CameraMoveFinished();
    }


    // Gets called by ArdaTween when the camera has finished moving to the ball
    public void CameraMoveFinished()
    {
        GameManager.instance.CameraMoveFinished();
        passiveSpeed = passiveSpeedInitial;
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