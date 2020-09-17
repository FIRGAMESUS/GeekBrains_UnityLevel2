using UnityEngine;

public class Gun : BaseWeapon
{
    private int bulletCount = 30;
    private int currentBullCount = 0;
    private float shootDistance = 1000f;
    private int damage = 20;

    public KeyCode Reload = KeyCode.R;

    [SerializeField] private bool prefab;
    [SerializeField] private GameObject bullet;
    private LineRenderer line;

    protected override void Awake()
    {
        base.Awake();

        _fire = true;
        currentBullCount = bulletCount;

        if(prefab)
        {
            bullet = Resources.Load<GameObject>("Prefabs/bullet");
            line = GetComponent<LineRenderer>();
            line.startWidth = 0.02f;
            line.endWidth = 0.02f;
        }
    }

    private Vector3 GetDirection(Vector3 HitPoint, Vector3 bulletPos)
    {
        Vector3 decr = HitPoint - bulletPos;
        float dist = decr.magnitude;
        return decr / dist;
    }

    public override void Fire()
    {
       if(_fire && currentBullCount > 0)
        {
            GOAnimator.SetTrigger("shoot");
            muzzleFlash.Play();
            currentBullCount--;
            RaycastHit hit;
            Ray ray = new Ray(TMCam.position, TMCam.forward);

            if(prefab)
            {
                GameObject temp = Instantiate(bullet, gunT.position, Quaternion.identity);
                Rigidbody BulletRG = temp.GetComponent<Rigidbody>();
                temp.GetComponent<Bullet>().Damage = damage;
                temp.GetComponent<Bullet>().DestructionTime = 10f;
                line.SetPosition(0, gunT.position);

                if(Physics.Raycast(ray, out hit, shootDistance))
                {
                    BulletRG.AddForce(GetDirection(hit.point, gunT.position) * 100, ForceMode.Impulse);
                    line.SetPosition(1, hit.point);
                }
                else
                {
                    BulletRG.AddForce(GetDirection(ray.GetPoint(1000f), gunT.position) * 100, ForceMode.Impulse);
                    line.SetPosition(1, ray.GetPoint(1000f));
                }
            }
            else
            {
                if (Physics.Raycast(ray, out hit, shootDistance))
                {
                    if (hit.collider.tag == "Player")
                    {
                        return;
                    }
                    else
                    {
                        SetDamage(hit.collider.GetComponent<ISetDamage>());
                    }
                }
                CreateParticleHit(hit);
            }
        }
    }


    private void SetDamage(ISetDamage obj)
    {
        if(obj!=null)
        {
            obj.SetDamage(damage);
        }
    }

    private void CreateParticleHit(RaycastHit hit)
    {
        GameObject tempHit = Instantiate(hitParticle, hit.point, Quaternion.identity);
        tempHit.transform.parent = hit.transform;
        Destroy(tempHit, 0.5f);
    }

    private void ReloadBullet()
    {
        _fire = true;
        currentBullCount = bulletCount;
    }

    void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            Fire();
        }

        if(Input.GetKeyDown(Reload))
        {
            _fire = false;
            GOAnimator.SetTrigger("reload");
        }
    }
}
