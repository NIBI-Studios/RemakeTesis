using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PolymorphismEnemy;

public class PolymorphismBullet : MonoBehaviour
{
    [SerializeField] private EnemyType type;
    [SerializeField] private int damage = 1;
    private void Start()
    {
        GetComponent<Rigidbody>().AddForce(transform.forward * 1500f);
        Destroy(this.gameObject, 5f);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<IDamagable>(out var damagable))
        {
            damagable.DealDamage(damage, type);
        }
    }
}
