using NaughtyAttributes;
using UnityEngine;
using Utilities;
using Random = UnityEngine.Random;

public class HelixPart : MonoBehaviour
{
    [MinMaxSlider(5f, 20f)] public Vector2 force;
    [MinMaxSlider(5f, 20f)] public Vector2 radius;
    [MinMaxSlider(-3f, 3f)] public Vector2 upwardsModifier;
    [SerializeField] private Material normalMaterial;
    [SerializeField] private Material superSpeedBlowUpMaterial;

    public PooledObject pooledObject;
    private Rigidbody rb;
    private BoxCollider _boxCollider;
    private MeshRenderer renderer;

    private void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
        renderer = GetComponent<MeshRenderer>();
        _boxCollider = GetComponentInChildren<BoxCollider>();
        _boxCollider.isTrigger = true;

        transform.rotation = Quaternion.identity;

        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            transform.rotation = Quaternion.identity;
        }
    }

    private bool changedMaterial;

    public void BlowUp(bool isSuperSpeed)
    {
        if (isSuperSpeed)
        {
            changedMaterial = true;
            renderer.material = superSpeedBlowUpMaterial;
        }

        _boxCollider.isTrigger = false;
        var point = new Vector3(0, transform.position.y, 0);
        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.None;
        var f = Random.Range(force.x, force.y);
        var r = Random.Range(radius.x, radius.y);
        var u = Random.Range(upwardsModifier.x, upwardsModifier.y);
        rb.AddExplosionForce(f, point, r, u, ForceMode.Impulse);
    }

    public void SelfDestroy()
    {
        if (changedMaterial)
            renderer.material = normalMaterial;
        ObjectPool.instance.TakeBack(pooledObject);
    }
}