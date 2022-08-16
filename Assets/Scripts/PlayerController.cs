using Enums;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Helix activeHelix;
    private float moveX;
    private Vector3 lastTapPos;
    public bool didSuperSpeedCollideHappened;


    private void Update()
    {
        if (GameManager.instance.State != GameState.Playing) return;
        if (didSuperSpeedCollideHappened) return;
        if (Input.GetMouseButton(0))
        {
            var curTapPos = Input.mousePosition;

            if (lastTapPos == Vector3.zero)
                lastTapPos = curTapPos;

            var delta = lastTapPos.x - curTapPos.x;
            lastTapPos = curTapPos;

            transform.Rotate(Vector3.up * delta);
            activeHelix.Rotate(Vector3.up * delta);
        }

        if (Input.GetMouseButtonUp(0))
        {
            lastTapPos = Vector3.zero;
        }
    }

    public void SuperSpeedCollision()
    {
        didSuperSpeedCollideHappened = true;
        activeHelix.BlowUp();
    }
}