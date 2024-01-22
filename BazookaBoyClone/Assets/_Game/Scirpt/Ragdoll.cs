using System.Collections.Generic;
using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    const string ragdollLayer = "Ragdoll";

    [SerializeField] List<Rigidbody> ragdollRigidList;

    public void ActiveRagdoll(bool value)
    {

        foreach (var rb in ragdollRigidList)
        {
            rb.isKinematic = !value;
        }
    }

    public void Force(Vector3 force)
    {
        foreach(var rb in ragdollRigidList)
        {
            rb.AddForce(force);
        }

    }

    private void OnValidate()
    {

        if (ragdollRigidList.Count == 0)
        {
            ragdollRigidList.AddRange(transform.GetChild(0).GetComponentsInChildren<Rigidbody>());
            foreach (var rb in ragdollRigidList)
            {
                rb.gameObject.layer = LayerMask.NameToLayer(ragdollLayer);
                rb.constraints = RigidbodyConstraints.None;
            }
        }

    }
}
