using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Character : MonoBehaviour
{
    const string die = "Die";

    public Rigidbody rb;
    public Transform center;
    [SerializeField] Animator anim;
    [SerializeField] Ragdoll ragdoll;
    [SerializeField] Collider bodyCollider;

    protected bool isDead;

    private void Awake()
    {
        OnInit();
    }

    protected virtual void OnInit()
    {
        isDead = false;
        ActiveRagdoll(false);
    }

    public void ActiveRagdoll(bool value)
    {
        anim.enabled = !value;
        rb.isKinematic = value;
        bodyCollider.enabled = !value;
        ragdoll.ActiveRagdoll(value);
    }

    public virtual void HitBomb(Vector3 force)
    {
        isDead = true;
        anim.Play(die);
        DOVirtual.DelayedCall(0.1f, () =>
        {
            ActiveRagdoll(true);
            ragdoll.Force(force);
        });

        DOVirtual.DelayedCall(3, () =>
        {
            Destroy(gameObject);
        });
    }


    protected virtual void OnValidate()
    {
        if (!anim)
        {
            anim = GetComponentInChildren<Animator>();
        }

        if (!rb)
        {
            rb = GetComponent<Rigidbody>();
        }

        if (!bodyCollider)
        {
            bodyCollider = GetComponent<Collider>();
        }

        if (!ragdoll)
        {
            ragdoll = GetComponent<Ragdoll>();
        }

    }
}
