using System;
using System.Collections;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private float jumpSpeed;


    private Rigidbody rb;
    private bool isJumping;

    private void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        GameManager.instance.OnGameOver += Stop;
        GameManager.instance.OnPlayerContinuesPlaying += Continue;
    }

    private void Continue()
    {
        rb.useGravity = true;
    }

    private void Stop()
    {
        if (rb == null)
        {
            print("ball rb null");
            rb = GetComponent<Rigidbody>();
        }

        rb.useGravity = false;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }


    private IEnumerator Jump()
    {
        if (isJumping) yield break;
        isJumping = true;
        SoundManager.instance.PlayBallJump();
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        yield return new WaitForEndOfFrame();
        rb.AddForce(Vector3.up * jumpSpeed, ForceMode.VelocityChange);
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(0.25f);
        isJumping = false;
    }

    private bool didScore;

    private void OnTriggerEnter(Collider c)
    {
        if (c.transform.CompareTag("Hoop"))
        {
            if (didScore) return;
            print("hoop");
            didScore = true;
            ScoreManager.instance.Score++;
        }
        else if (c.transform.CompareTag("Circle"))
        {
            if (didScore) return;

            print("circle");
            StartCoroutine(Jump());
        }
        else if (c.transform.CompareTag("Helix"))
        {
            if (didScore)
            {
                didScore = false;
                return;
            }
            print("helix");

            GameManager.instance.GameOver();
        }
        else if (c.transform.parent.CompareTag("HelixPart"))
        {
            StartCoroutine(Jump());
        }
    }

   
}