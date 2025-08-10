using UnityEngine;
using System.Collections.Generic;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager Instance;

    // 프리팹 변수들
    public GameObject enemyPrefab;
    public GameObject expPrefab;
    public GameObject knifePrefab;
    public GameObject healthItemPrefab;
    public GameObject treasureBoxPrefab;
    public GameObject wizardPrefab;
    public GameObject golemPrefab;

    // 풀(Queue) 변수들
    private Queue<GameObject> enemyPool = new Queue<GameObject>();
    private Queue<GameObject> expPool = new Queue<GameObject>();
    private Queue<GameObject> knifePool = new Queue<GameObject>();
    private Queue<GameObject> healthItemPool = new Queue<GameObject>();
    private Queue<GameObject> treasureBoxPool = new Queue<GameObject>();
    private Queue<GameObject> wizardPool = new Queue<GameObject>();
    private Queue<GameObject> golemPool = new Queue<GameObject>();

    // 인스펙터에서 풀 사이즈를 설정하는 변수들
    public int enemyPoolSize = 50;
    public int expPoolSize = 10;
    public int knifePoolSize = 20;
    public int healthItemPoolSize = 5;
    public int treasureBoxPoolSize = 10;
    public int wizardPoolSize = 20;
    public int golemPoolSize = 5;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            // 이미 다른 ObjectPoolManager 인스턴스가 존재하면 현재 오브젝트 파괴
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // InitPools()는 OnSceneLoaded에서 호출될 것이므로 여기서 주석 처리 또는 제거
        // InitPools(); 
    }

    // 씬 로드 시마다 풀을 초기화하거나 정리하는 함수
    // SceneManager.sceneLoaded 이벤트에 등록하여 사용하면 좋습니다.
    private void OnEnable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
    {
        // 메인 게임 씬이 로드될 때만 풀을 초기화하거나 정리합니다.
        // 예를 들어 "MainScene"일 때만 초기화
        if (scene.name == "MainScene")
        {
            ClearAllPools(); // 기존 풀의 오브젝트들을 파괴하고 초기화
            InitPools();
        }
    }

    // 모든 풀의 오브젝트를 파괴하고 풀을 비웁니다.
    private void ClearAllPools()
    {
        ClearPool(enemyPool);
        ClearPool(expPool);
        ClearPool(knifePool);
        ClearPool(healthItemPool);
        ClearPool(treasureBoxPool);
        ClearPool(wizardPool);
        ClearPool(golemPool);
    }

    private void ClearPool(Queue<GameObject> pool)
    {
        while (pool.Count > 0)
        {
            GameObject obj = pool.Dequeue();
            if (obj != null) // 혹시 모를 Null 체크
            {
                Destroy(obj);
            }
        }
    }

    private void InitPools()
    {
        InitEnemyPool(enemyPoolSize);
        InitExpPool(expPoolSize);
        InitKnifePool(knifePoolSize);
        InitHealthItemPool(healthItemPoolSize);
        InitTreasureBoxPool(treasureBoxPoolSize);
        InitWizardPool(wizardPoolSize);
        InitGolemPool(golemPoolSize);
    }

    // --- 풀 초기화 함수들 (기존과 동일) ---
    public void InitEnemyPool(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab);
            enemy.SetActive(false);
            enemyPool.Enqueue(enemy);
        }
    }

    public void InitExpPool(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject exp = Instantiate(expPrefab);
            exp.SetActive(false);
            expPool.Enqueue(exp);
        }
    }

    public void InitKnifePool(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject knife = Instantiate(knifePrefab);
            knife.SetActive(false);
            knifePool.Enqueue(knife);
        }
    }

    public void InitHealthItemPool(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject item = Instantiate(healthItemPrefab);
            item.SetActive(false);
            healthItemPool.Enqueue(item);
        }
    }

    public void InitTreasureBoxPool(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject box = Instantiate(treasureBoxPrefab);
            box.SetActive(false);
            treasureBoxPool.Enqueue(box);
        }
    }

    public void InitWizardPool(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject wizard = Instantiate(wizardPrefab);
            wizard.SetActive(false);
            wizardPool.Enqueue(wizard);
        }
    }

    public void InitGolemPool(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject golem = Instantiate(golemPrefab);
            golem.SetActive(false);
            golemPool.Enqueue(golem);
        }
    }

    // --- 풀에서 오브젝트를 가져오는 함수들 (파괴된 오브젝트 처리 로직 추가) ---

    // Enemy
    public GameObject GetEnemy()
    {
        while (enemyPool.Count > 0)
        {
            GameObject enemy = enemyPool.Dequeue();
            if (enemy != null) // 오브젝트가 유효한지 확인
            {
                enemy.SetActive(true);
                Enemy enemyScript = enemy.GetComponent<Enemy>();
                if (enemyScript != null)
                {
                    enemyScript.OnObjectSpawn(); // 스폰 시 초기화 로직 호출
                }
                return enemy;
            }
            // null이면 풀에서 제거하고 다음 오브젝트를 시도 (루프가 반복)
        }

        // 풀에 유효한 오브젝트가 없으면 새로 생성
        GameObject newEnemy = Instantiate(enemyPrefab);
        newEnemy.SetActive(true);
        // 새로 생성된 오브젝트는 풀에 다시 넣지 않습니다. (풀의 확장 개념)
        Enemy newEnemyScript = newEnemy.GetComponent<Enemy>();
        if (newEnemyScript != null)
        {
            newEnemyScript.OnObjectSpawn();
        }
        return newEnemy;
    }

    public void ReturnEnemy(GameObject enemy)
    {
        if (enemy != null) // 반환하려는 오브젝트가 유효한지 확인
        {
            enemy.SetActive(false);
            enemyPool.Enqueue(enemy);
        }
    }

    // Wizard
    public GameObject GetWizard()
    {
        while (wizardPool.Count > 0)
        {
            GameObject wizard = wizardPool.Dequeue();
            if (wizard != null)
            {
                wizard.SetActive(true);
                Enemy wizardScript = wizard.GetComponent<Enemy>();
                if (wizardScript != null)
                {
                    wizardScript.OnObjectSpawn();
                }
                return wizard;
            }
        }
        GameObject newWizard = Instantiate(wizardPrefab);
        newWizard.SetActive(true);
        Enemy newWizardScript = newWizard.GetComponent<Enemy>();
        if (newWizardScript != null)
        {
            newWizardScript.OnObjectSpawn();
        }
        return newWizard;
    }

    public void ReturnWizard(GameObject wizard)
    {
        if (wizard != null)
        {
            wizard.SetActive(false);
            wizardPool.Enqueue(wizard);
        }
    }

    // Golem
    public GameObject GetGolem()
    {
        while (golemPool.Count > 0)
        {
            GameObject golem = golemPool.Dequeue();
            if (golem != null)
            {
                golem.SetActive(true);
                Enemy golemScript = golem.GetComponent<Enemy>();
                if (golemScript != null)
                {
                    golemScript.OnObjectSpawn();
                }
                return golem;
            }
        }
        GameObject newGolem = Instantiate(golemPrefab);
        newGolem.SetActive(true);
        Enemy newGolemScript = newGolem.GetComponent<Enemy>();
        if (newGolemScript != null)
        {
            newGolemScript.OnObjectSpawn();
        }
        return newGolem;
    }

    public void ReturnGolem(GameObject golem)
    {
        if (golem != null)
        {
            golem.SetActive(false);
            golemPool.Enqueue(golem);
        }
    }

    // Exp
    public GameObject GetExp()
    {
        while (expPool.Count > 0)
        {
            GameObject exp = expPool.Dequeue();
            if (exp != null)
            {
                exp.SetActive(true);
                return exp;
            }
        }
        GameObject newExp = Instantiate(expPrefab);
        newExp.SetActive(true);
        return newExp;
    }

    public void ReturnExp(GameObject exp)
    {
        if (exp != null)
        {
            exp.SetActive(false);
            expPool.Enqueue(exp);
        }
    }

    // Knife
    public GameObject GetKnife()
    {
        while (knifePool.Count > 0)
        {
            GameObject knife = knifePool.Dequeue();
            if (knife != null)
            {
                knife.SetActive(true);
                return knife;
            }
        }
        GameObject newKnife = Instantiate(knifePrefab);
        newKnife.SetActive(true);
        return newKnife;
    }

    public void ReturnKnife(GameObject knife)
    {
        if (knife != null)
        {
            knife.SetActive(false);
            knifePool.Enqueue(knife);
        }
    }

    // Health Item
    public GameObject GetHealthItem()
    {
        while (healthItemPool.Count > 0)
        {
            GameObject item = healthItemPool.Dequeue();
            if (item != null)
            {
                item.SetActive(true);
                return item;
            }
        }
        GameObject newItem = Instantiate(healthItemPrefab);
        newItem.SetActive(true);
        return newItem;
    }

    public void ReturnHealthItem(GameObject item)
    {
        if (item != null)
        {
            item.SetActive(false);
            healthItemPool.Enqueue(item);
        }
    }

    // Treasure Box
    public GameObject GetTreasureBox()
    {
        while (treasureBoxPool.Count > 0)
        {
            GameObject box = treasureBoxPool.Dequeue();
            if (box != null)
            {
                box.SetActive(true);
                return box;
            }
        }
        GameObject newBox = Instantiate(treasureBoxPrefab);
        newBox.SetActive(true);
        return newBox;
    }

    public void ReturnTreasureBox(GameObject box)
    {
        if (box != null)
        {
            box.SetActive(false);
            treasureBoxPool.Enqueue(box);
        }
    }
}