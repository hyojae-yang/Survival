using UnityEngine;
using System.Collections.Generic;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager Instance;

    // ������ ������
    public GameObject enemyPrefab;
    public GameObject expPrefab;
    public GameObject knifePrefab;
    public GameObject healthItemPrefab;
    public GameObject treasureBoxPrefab;
    public GameObject wizardPrefab;
    public GameObject golemPrefab;

    // Ǯ(Queue) ������
    private Queue<GameObject> enemyPool = new Queue<GameObject>();
    private Queue<GameObject> expPool = new Queue<GameObject>();
    private Queue<GameObject> knifePool = new Queue<GameObject>();
    private Queue<GameObject> healthItemPool = new Queue<GameObject>();
    private Queue<GameObject> treasureBoxPool = new Queue<GameObject>();
    private Queue<GameObject> wizardPool = new Queue<GameObject>();
    private Queue<GameObject> golemPool = new Queue<GameObject>();

    // �ν����Ϳ��� Ǯ ����� �����ϴ� ������
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
            // �̹� �ٸ� ObjectPoolManager �ν��Ͻ��� �����ϸ� ���� ������Ʈ �ı�
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // InitPools()�� OnSceneLoaded���� ȣ��� ���̹Ƿ� ���⼭ �ּ� ó�� �Ǵ� ����
        // InitPools(); 
    }

    // �� �ε� �ø��� Ǯ�� �ʱ�ȭ�ϰų� �����ϴ� �Լ�
    // SceneManager.sceneLoaded �̺�Ʈ�� ����Ͽ� ����ϸ� �����ϴ�.
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
        // ���� ���� ���� �ε�� ���� Ǯ�� �ʱ�ȭ�ϰų� �����մϴ�.
        // ���� ��� "MainScene"�� ���� �ʱ�ȭ
        if (scene.name == "MainScene")
        {
            ClearAllPools(); // ���� Ǯ�� ������Ʈ���� �ı��ϰ� �ʱ�ȭ
            InitPools();
        }
    }

    // ��� Ǯ�� ������Ʈ�� �ı��ϰ� Ǯ�� ���ϴ�.
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
            if (obj != null) // Ȥ�� �� Null üũ
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

    // --- Ǯ �ʱ�ȭ �Լ��� (������ ����) ---
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

    // --- Ǯ���� ������Ʈ�� �������� �Լ��� (�ı��� ������Ʈ ó�� ���� �߰�) ---

    // Enemy
    public GameObject GetEnemy()
    {
        while (enemyPool.Count > 0)
        {
            GameObject enemy = enemyPool.Dequeue();
            if (enemy != null) // ������Ʈ�� ��ȿ���� Ȯ��
            {
                enemy.SetActive(true);
                Enemy enemyScript = enemy.GetComponent<Enemy>();
                if (enemyScript != null)
                {
                    enemyScript.OnObjectSpawn(); // ���� �� �ʱ�ȭ ���� ȣ��
                }
                return enemy;
            }
            // null�̸� Ǯ���� �����ϰ� ���� ������Ʈ�� �õ� (������ �ݺ�)
        }

        // Ǯ�� ��ȿ�� ������Ʈ�� ������ ���� ����
        GameObject newEnemy = Instantiate(enemyPrefab);
        newEnemy.SetActive(true);
        // ���� ������ ������Ʈ�� Ǯ�� �ٽ� ���� �ʽ��ϴ�. (Ǯ�� Ȯ�� ����)
        Enemy newEnemyScript = newEnemy.GetComponent<Enemy>();
        if (newEnemyScript != null)
        {
            newEnemyScript.OnObjectSpawn();
        }
        return newEnemy;
    }

    public void ReturnEnemy(GameObject enemy)
    {
        if (enemy != null) // ��ȯ�Ϸ��� ������Ʈ�� ��ȿ���� Ȯ��
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