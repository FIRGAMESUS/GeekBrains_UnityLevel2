using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private int _damage;
    private float _destructionTime = 10f;
    [SerializeField] protected GameObject hitParticle;

    public int Damage { get => _damage; set => _damage = value; }
    public float DestructionTime { get => _destructionTime; set => _destructionTime = value; }

    void Awake()
    {
        Destroy(gameObject, _destructionTime);
        hitParticle = Resources.Load<GameObject>("Prefabs/Flare");
    }

    private void SetDamage(ISetDamage obj)
    {
        if (obj != null)
        {
            obj.SetDamage(_damage);
        }
    }

    private void CreateParticleHit(Collision other)
    {
        GameObject tempHit = Instantiate(hitParticle, transform.position, Quaternion.identity);
        tempHit.transform.parent = other.transform;
        Destroy(tempHit, 0.5f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            return;
        }
        else
        {
            SetDamage(collision.gameObject.GetComponent<ISetDamage>());
            CreateParticleHit(collision);
            Destroy(gameObject);
        }
    }
}
