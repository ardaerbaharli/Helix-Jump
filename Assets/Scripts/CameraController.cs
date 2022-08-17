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
        GameManager.instance.OnPlayerContinuesPlaying += OnPlayerContinuesPlaying;
    }

    private void OnPlayerContinuesPlaying()
    {
        MoveTheCameraToBall();
    }

    private void MoveTheCameraToBall()
    {
        var targetPosY = ball.transform.position.y + offsetY;
        var targetPos = new Vector3(transform.position.x, targetPosY, transform.position.z);
        ArdaTween.MoveTo(gameObject, ArdaTween.Hash("targetPosition", targetPos,
            "duration", 1f, "onComplete", "CameraMoveFinished"));
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