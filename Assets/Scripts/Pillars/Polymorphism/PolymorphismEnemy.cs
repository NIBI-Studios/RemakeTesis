using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class PolymorphismEnemy : MonoBehaviour, IDamagable
{
    public enum EnemyType { Ice, Fire };
    public EnemyType enemyType;
    public Transform target;
    public PolymorphismGameManager spawner;
    private NavMeshAgent agent;
    public int maxHealth = 3;
    private int currentHealth;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(target.position);
    }
    void Update()
    {
        if (Vector3.Distance(transform.position, target.position) < 1f)
        {
            spawner.EnemyReachedTarget();
            Destroy(gameObject);
        }
    }
    void OnDestroy()
    {
        spawner.EnemyDestroyed();
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    public void Heal(int healAmount)
    {
        currentHealth = Mathf.Min(currentHealth + healAmount, maxHealth);
    }
    void Die()
    {
        Destroy(gameObject);
    }

    public void DealDamage(int damage, EnemyType type)
    {
        if (this.enemyType == type)
        {
            currentHealth = Mathf.Clamp(currentHealth + damage, 1, maxHealth);
            UpdateUIHealth();
            return;
        }
        switch (this.enemyType)
        {
            case EnemyType.Fire:
                if (type == EnemyType.Ice)
                {
                    TakeDamage(1);
                }
                break;
            case EnemyType.Ice:
                if (type == EnemyType.Fire)
                {
                    TakeDamage(1);
                }
                break;
        }
        UpdateUIHealth();
    }

    private void UpdateUIHealth()
    {
        Image healthBar = GetComponentInChildren<Image>();
        healthBar.fillAmount = (float)currentHealth / maxHealth;
    }
}
