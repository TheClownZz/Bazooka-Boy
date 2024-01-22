using UnityEngine;

public class Player : Character
{
    [SerializeField] float bombSpeed = 5;
    [SerializeField] float coolDown = 3;
    [SerializeField] Transform bombPf;
    [SerializeField] GameObject gunGo;
    [SerializeField] Transform shooter;
    [SerializeField] Transform holder;
    [SerializeField] Trajectory simulation;

    Camera cam;
    float shootTime;

    Vector3 velocity;
    protected override void OnInit()
    {
        base.OnInit();
        cam = Camera.main;
        shootTime = Time.time - coolDown;
    }
    public override void HitBomb(Vector3 force)
    {
        base.HitBomb(force);
        gunGo.transform.SetParent(null);
        Rigidbody gunRG = gunGo.GetComponent<Rigidbody>();

        gunRG.isKinematic = false;
        gunRG.AddForce(force);

        gunGo.GetComponent<Collider>().isTrigger = false;
        simulation.Hide();

    }

    private void LateUpdate()
    {
        if (isDead) return;
        ControllGun();
        if (Time.time - shootTime > coolDown)
        {
            if (Input.GetMouseButtonDown(0))
            {
                ShootGun();
                simulation.Hide();

            }
            else
            {
                simulation.SimulatePath(shooter, velocity, bombPf.transform.localScale.x);
            }
        }

    }

    private void ShootGun()
    {
        shootTime = Time.time;
        Transform clone = Instantiate(bombPf);
        clone.transform.position = shooter.position;
        Bomb bomb = clone.GetComponent<Bomb>();
        bomb.rb.velocity = velocity;
    }
    private void ControllGun()
    {
        Vector3 mouseWorldPosition = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
        Vector3 direction = mouseWorldPosition - holder.position;

        float angle = Mathf.Atan2(-direction.y, direction.x) * Mathf.Rad2Deg;

        angle = Mathf.Clamp(angle, -90, 90);

        holder.localRotation = Quaternion.Euler(0, 0, angle);

        Vector3 dir = shooter.position - holder.position;
        dir.z = 0;
        velocity = bombSpeed * dir.normalized;

    }

}
