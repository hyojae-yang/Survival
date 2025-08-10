using UnityEngine;
using System.Collections.Generic;

public class Enemy : MonoBehaviour
{
    // 인스펙터에서 설정할 '기본 능력치'들로 변경
    public float baseMaxHealth = 1f; // 인스펙터에서 설정할 기본 최대 체력
    public float baseMoveSpeed = 3f; // 인스펙터에서 설정할 기본 이동 속도
    public int baseDamage = 10; // 인스펙터에서 설정할 기본 공격력
    public int expAmount = 1; // 이 적이 드랍할 기본 경험치량

    // 실제 게임에서 사용될 능력치 (GameManager 배율 적용 후)
    [HideInInspector] public float maxHealth;
    [HideInInspector] public float currentHealth;
    [HideInInspector] public float moveSpeed;
    [HideInInspector] public int damage;

    protected Transform player;

    protected virtual void Awake()
    {
        // GameManager의 배율을 적용하여 실제 능력치 설정
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
            Debug.LogWarning("GameManager 인스턴스를 찾을 수 없습니다. 적이 기본 능력치로 설정됩니다.");
        }
        currentHealth = maxHealth;
    }

    protected virtual void Start()
    {
        // Start()에서는 플레이어 참조만 찾도록 변경
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

    // 오브젝트 풀에서 꺼내질 때마다 호출될 초기화 함수 추가
    public virtual void OnObjectSpawn()
    {
        currentHealth = maxHealth;
        // 필요하다면 여기서 추가적인 초기화 로직을 넣을 수 있습니다.
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
        // 몬스터 사망 효과음 재생
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
            // 적을 오브젝트 풀로 반환
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
                // 플레이어의 무적 시간을 믿고, 바로 데미지를 줍니다!
                if (PlayerStats.Instance != null)
                {
                    PlayerStats.Instance.TakeDamage(damage);
                }
            }
        }
    }
}