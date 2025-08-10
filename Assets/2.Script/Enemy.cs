using UnityEngine;
using System.Collections.Generic;

public class Enemy : MonoBehaviour
{
    // �ν����Ϳ��� ������ '�⺻ �ɷ�ġ'��� ����
    public float baseMaxHealth = 1f; // �ν����Ϳ��� ������ �⺻ �ִ� ü��
    public float baseMoveSpeed = 3f; // �ν����Ϳ��� ������ �⺻ �̵� �ӵ�
    public int baseDamage = 10; // �ν����Ϳ��� ������ �⺻ ���ݷ�
    public int expAmount = 1; // �� ���� ����� �⺻ ����ġ��

    // ���� ���ӿ��� ���� �ɷ�ġ (GameManager ���� ���� ��)
    [HideInInspector] public float maxHealth;
    [HideInInspector] public float currentHealth;
    [HideInInspector] public float moveSpeed;
    [HideInInspector] public int damage;

    protected Transform player;

    protected virtual void Awake()
    {
        // GameManager�� ������ �����Ͽ� ���� �ɷ�ġ ����
        if (GameManager.Instance != null)
        {
            maxHealth = baseMaxHealth * GameManager.Instance.enemyHealthMultiplier;
            moveSpeed = baseMoveSpeed * GameManager.Instance.enemyDamageMultiplier;
            damage = (int)(baseDamage * GameManager.Instance.enemyDamageMultiplier);
        }
        else
        {
            maxHealth = baseMaxHealth;
            moveSpeed = baseMoveSpeed;
            damage = baseDamage;
            Debug.LogWarning("GameManager �ν��Ͻ��� ã�� �� �����ϴ�. ���� �⺻ �ɷ�ġ�� �����˴ϴ�.");
        }
        currentHealth = maxHealth;
    }

    protected virtual void Start()
    {
        // Start()������ �÷��̾� ������ ã���� ����
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
    }

    protected virtual void Update()
    {
        if (player != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
        }
    }

    // ������Ʈ Ǯ���� ������ ������ ȣ��� �ʱ�ȭ �Լ� �߰�
    public virtual void OnObjectSpawn()
    {
        currentHealth = maxHealth;
        // �ʿ��ϴٸ� ���⼭ �߰����� �ʱ�ȭ ������ ���� �� �ֽ��ϴ�.
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        // ���� ��� ȿ���� ���
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySFX(SoundManager.Instance.monsterDieSFX);
        }

        if (ObjectPoolManager.Instance != null)
        {
            GameObject exp = ObjectPoolManager.Instance.GetExp();
            if (exp != null)
            {
                exp.transform.position = transform.position;
                Exp expScript = exp.GetComponent<Exp>();
                if (expScript != null)
                {
                    expScript.SetExpAmount(expAmount);
                }
            }
            // ���� ������Ʈ Ǯ�� ��ȯ
            if (gameObject.GetComponent<Slime>() != null)
            {
                ObjectPoolManager.Instance.ReturnEnemy(gameObject);
            }
            else if (gameObject.GetComponent<Wizard>() != null)
            {
                ObjectPoolManager.Instance.ReturnWizard(gameObject);
            }
            else if (gameObject.GetComponent<Golem>() != null)
            {
                ObjectPoolManager.Instance.ReturnGolem(gameObject);
            }
            else
            {
                ObjectPoolManager.Instance.ReturnEnemy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.CompareTag("Player"))
            {
                // �÷��̾��� ���� �ð��� �ϰ�, �ٷ� �������� �ݴϴ�!
                if (PlayerStats.Instance != null)
                {
                    PlayerStats.Instance.TakeDamage(damage);
                }
            }
        }
    }
}