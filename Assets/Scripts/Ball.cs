using System;
using System.Collections;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private float jumpSpeed;
    [SerializeField] private MeshRenderer ballRenderer;
    [SerializeField] private TrailRenderer trailRenderer;

    [SerializeField] private Material ballMaterial;
    [SerializeField] private Material trailMaterial;
    [SerializeField] private Material ballSuperSpeedMaterial;
    [SerializeField] private Material trailSuperSpeedMaterial;
    [SerializeField] private Light light;
    [SerializeField] private Color lightNormalColor;
    [SerializeField] private Color lightSuperSpeedColor;


    public bool didTouchGroundAfterScoring;
    public int lastTouchedLevel;
    private bool isSuperSpeed;
    private Rigidbody rb;
    private bool isJumping;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        GameManager.instance.OnGameOver += () => Destroy(gameObject);
    }


    public void ToggleSuperSpeed(bool value)
    {
        isSuperSpeed = value;
        ballRenderer.material = isSuperSpeed ? ballSuperSpeedMaterial : ballMaterial;
        trailRenderer.material = isSuperSpeed ? trailSuperSpeedMaterial : trailMaterial;
        light.color = isSuperSpeed ? lightSuperSpeedColor : lightNormalColor;
    }

    private IEnumerator Jump()
    {
        rb.velocity = Vector3.zero;
        rb.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
        yield return new WaitForSeconds(0.1f);
        isJumping = false;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.transform.parent.CompareTag("HelixPart") && !isJumping)
        {
            isJumping = true;
            didTouchGroundAfterScoring = true;
            lastTouchedLevel = ScoreManager.instance.Score;

            StartCoroutine(Jump());

            if (isSuperSpeed)
            {
                ToggleSuperSpeed(false);
                GameManager.instance.SuperSpeedCollision();
            }
        }
        else if (collision.transform.CompareTag("Helix"))
        {
            didTouchGroundAfterScoring = false;
            ScoreManager.instance.Score++;
        }
    }
}