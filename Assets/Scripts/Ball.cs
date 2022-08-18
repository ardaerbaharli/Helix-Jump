using System.Collections;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private float jumpSpeed;
    [SerializeField] private MeshRenderer ballRenderer;
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private Light light;

    public bool didTouchGroundAfterScoring;
    public int lastTouchedLevel;
    private bool isSuperSpeed;
    private Rigidbody rb;
    private bool isJumping;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    

    public void ToggleSuperSpeed(bool value)
    {
        isSuperSpeed = value;
        var mats = LevelDesign.instance.GetBallMaterials(value);
        ballRenderer.material = (Material) mats[0];
        trailRenderer.material = (Material) mats[1];
        light.color = (Color) mats[2];
    }

    private IEnumerator Jump()
    {
        SoundManager.instance.PlayBallJump();
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        yield return new WaitForEndOfFrame();
        rb.AddForce(Vector3.up * jumpSpeed, ForceMode.VelocityChange);
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(0.25f);
        isJumping = false;
    }


    private void OnTriggerEnter(Collider c)
    {
        if (c.transform.parent.CompareTag("HelixPart") && !isJumping)
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
        else if (c.transform.CompareTag("Helix"))
        {
            didTouchGroundAfterScoring = false;
            ScoreManager.instance.Score++;
        }
    }
}