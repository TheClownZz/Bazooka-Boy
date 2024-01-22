using UnityEngine;
using DG.Tweening;

public class Bomb : MonoBehaviour
{
    const string ground = "Ground";

    public Rigidbody rb;
    [SerializeField] Transform scaner;
    [SerializeField] GameObject explosionPf;

    [SerializeField] LayerMask layerMask;

    [SerializeField] float explosionFore = 300;
    [SerializeField] float minForceDistance = 0.5f;
    bool firstCollision;

    private void Awake()
    {
        firstCollision = false;
    }


    public void OnInit()
    {
        rb.isKinematic = true;
    }



    public void Shoot(Vector3 force)
    {
        rb.isKinematic = false;
        rb.AddForce(force);
    }

    void Explosion()
    {
        var clone = Instantiate(explosionPf);
        clone.transform.position = transform.position;
        DOVirtual.DelayedCall(3, () =>
        {
            Destroy(clone);
        });

        Scan();

        Destroy(gameObject);

    }

    void Scan()
    {
        float radius = scaner.lossyScale.x / 2;
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius, layerMask);
        foreach (Collider col in hitColliders)
        {

            if (col.tag == ground)
            {
                GameObject go = col.gameObject;

                Model result = CSG.Perform(CSG.BooleanOp.Subtraction, go, scaner.gameObject);
                var composite = new GameObject();
                composite.tag = go.tag;
                composite.name = go.name;
                composite.AddComponent<MeshFilter>().sharedMesh = result.mesh;
                composite.AddComponent<MeshCollider>().sharedMesh = result.mesh;
                composite.AddComponent<MeshRenderer>().sharedMaterials = result.materials.ToArray();

                Destroy(go);

            }
            else
            {
                Character character = col.GetComponent<Character>();
                if (character)
                {
                    Vector3 dir = character.center.position - transform.position;
                    dir.z = 0;
                    Vector3 forceStr = dir.normalized * explosionFore * radius / Mathf.Max(minForceDistance, dir.magnitude);
                    character.HitBomb(forceStr);
                }

            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!firstCollision)
        {
            firstCollision = true;
            DOVirtual.DelayedCall(2, () =>
            {
                Explosion();
            });
        }
    }
}
