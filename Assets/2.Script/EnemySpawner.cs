using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // �� ���� ���� (������)
    public float enemySpawnInterval = 1f; // �ʱ� ���� ����
    public float minEnemySpawnInterval = 0.5f; // �ּ� ���� ����
    private float enemyTimer = 0f;

    // ���� ���� ����
    public float treasureBoxSpawnInterval = 10f;
    private float treasureBoxTimer = 0f;

    // --- ������ ���� ���� ---
    public GameObject wizardPrefab;
    public float wizardSpawnStartTime = 180f;
    public float minWizardSpawnInterval = 20f;
    public float maxWizardSpawnInterval = 40f;
    private float wizardTimer = 0f;
    private float currentWizardSpawnInterval;

    // --- �� ���� ���� ---
    public GameObject golemPrefab;
    public float golemSpawnStartTime = 300f; // 5�� = 300��
    public float golemSpawnInterval = 60f; // 60�ʸ���
    private float golemTimer = 0f;

    // ī�޶��� ũ��
    private float camWidth;
    private float camHeight;

    // �÷��̾��� Transform ����
    private Transform playerTransform;

    void Start()
    {
        camHeight = Camera.main.orthographicSize;
        camWidth = camHeight * Camera.main.aspect;

        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            playerTransform = playerObject.transform;
        }
        else
        {
            Debug.LogError("Player ������Ʈ�� ã�� �� �����ϴ�. 'Player' �±׸� Ȯ�����ּ���.");
        }

        currentWizardSpawnInterval = Random.Range(minWizardSpawnInterval, maxWizardSpawnInterval);
    }

    void Update()
    {
        // GameManager���� ���� ���� �ð� ��������
        float currentElapsedTime = GameManager.Instance.gameElapsedTime;

        // �� ���� ���� ������ ����
        // ���� ���� ���� = �ʱ� ���� ���� - (�� ��� �ð� / Ư�� �ð�) * ������
        // ���⼭�� ���÷� 10��(600��)�� �������� ���������� �����ϵ��� ����
        enemySpawnInterval = Mathf.Max(minEnemySpawnInterval, 1f - (currentElapsedTime / 600f) * 0.5f);
        // 0���� �� 1��, 600���� �� 0.5�ʱ��� �پ��

        // ������ ���� ����
        enemyTimer += Time.deltaTime;
        if (enemyTimer >= enemySpawnInterval)
        {
            SpawnEnemy();
            enemyTimer = 0f;
        }

        // ���� ���� ���� (��ȭ ����)
        treasureBoxTimer += Time.deltaTime;
        if (treasureBoxTimer >= treasureBoxSpawnInterval)
        {
            SpawnTreasureBox();
            treasureBoxTimer = 0f;
        }

        // ������ ���� ���� (GameManager �ð� ���)
        if (currentElapsedTime >= wizardSpawnStartTime)
        {
            wizardTimer += Time.deltaTime;
            if (wizardTimer >= currentWizardSpawnInterval)
            {
                SpawnWizardGroup();
                wizardTimer = 0f;
                currentWizardSpawnInterval = Random.Range(minWizardSpawnInterval, maxWizardSpawnInterval);
            }
        }

        // �� ���� ���� (GameManager �ð� ���)
        if (currentElapsedTime >= golemSpawnStartTime)
        {
            golemTimer += Time.deltaTime;
            if (golemTimer >= golemSpawnInterval)
            {
                SpawnGolem();
                golemTimer = 0f;
            }
        }
    }

    // Spawn �Լ����� ObjectPoolManager�� ���� ���� �������Ƿ� ���� ����
    private void SpawnEnemy()
    {
        GameObject enemy = ObjectPoolManager.Instance.GetEnemy();
        if (enemy != null)
        {
            enemy.transform.position = GetRandomSpawnPositionOutsideCamera();
        }
    }

    private void SpawnTreasureBox()
    {
        GameObject treasureBox = ObjectPoolManager.Instance.GetTreasureBox();
        if (treasureBox != null)
        {
            treasureBox.transform.position = GetRandomSpawnPositionOutsideCamera();
        }
    }

    private void SpawnWizardGroup()
    {
        int numberOfWizardsToSpawn = Random.Range(3, 6);

        for (int i = 0; i < numberOfWizardsToSpawn; i++)
        {
            GameObject wizard = ObjectPoolManager.Instance.GetWizard();
            if (wizard != null)
            {
                wizard.transform.position = GetRandomSpawnPositionOutsideCamera();
            }
        }
    }

    private void SpawnGolem()
    {
        GameObject golem = ObjectPoolManager.Instance.GetGolem();
        if (golem != null)
        {
            golem.transform.position = GetRandomSpawnPositionOutsideCamera();
        }
    }

    private Vector3 GetRandomSpawnPositionOutsideCamera()
    {
        if (playerTransform == null)
        {
            return Vector3.zero;
        }

        Vector3 playerPos = playerTransform.position;
        Vector3 spawnPosition = Vector3.zero;
        float offset = 2f;

        float currentCamLeft = playerPos.x - camWidth;
        float currentCamRight = playerPos.x + camWidth;
        float currentCamBottom = playerPos.y - camHeight;
        float currentCamTop = playerPos.y + camHeight;

        int side = Random.Range(0, 4);

        switch (side)
        {
            case 0:
                spawnPosition = new Vector3(Random.Range(currentCamLeft, currentCamRight), currentCamTop + offset, 0);
                break;
            case 1:
                spawnPosition = new Vector3(Random.Range(currentCamLeft, currentCamRight), currentCamBottom - offset, 0);
                break;
            case 2:
                spawnPosition = new Vector3(currentCamLeft - offset, Random.Range(currentCamBottom, currentCamTop), 0);
                break;
            case 3:
                spawnPosition = new Vector3(currentCamRight + offset, Random.Range(currentCamBottom, currentCamTop), 0);
                break;
        }

        return spawnPosition;
    }
}