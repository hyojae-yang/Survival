using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // 적 스폰 설정 (슬라임)
    public float enemySpawnInterval = 1f; // 초기 스폰 간격
    public float minEnemySpawnInterval = 0.5f; // 최소 스폰 간격
    private float enemyTimer = 0f;

    // 상자 스폰 설정
    public float treasureBoxSpawnInterval = 10f;
    private float treasureBoxTimer = 0f;

    // --- 마법사 스폰 설정 ---
    public GameObject wizardPrefab;
    public float wizardSpawnStartTime = 180f;
    public float minWizardSpawnInterval = 20f;
    public float maxWizardSpawnInterval = 40f;
    private float wizardTimer = 0f;
    private float currentWizardSpawnInterval;

    // --- 골렘 스폰 설정 ---
    public GameObject golemPrefab;
    public float golemSpawnStartTime = 300f; // 5분 = 300초
    public float golemSpawnInterval = 60f; // 60초마다
    private float golemTimer = 0f;

    // 카메라의 크기
    private float camWidth;
    private float camHeight;

    // 플레이어의 Transform 참조
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
            Debug.LogError("Player 오브젝트를 찾을 수 없습니다. 'Player' 태그를 확인해주세요.");
        }

        currentWizardSpawnInterval = Random.Range(minWizardSpawnInterval, maxWizardSpawnInterval);
    }

    void Update()
    {
        // GameManager에서 현재 게임 시간 가져오기
        float currentElapsedTime = GameManager.Instance.gameElapsedTime;

        // 적 스폰 간격 점진적 감소
        // 현재 스폰 간격 = 초기 스폰 간격 - (총 경과 시간 / 특정 시간) * 감소율
        // 여기서는 예시로 10분(600초)을 기준으로 선형적으로 감소하도록 설정
        enemySpawnInterval = Mathf.Max(minEnemySpawnInterval, 1f - (currentElapsedTime / 600f) * 0.5f);
        // 0초일 때 1초, 600초일 때 0.5초까지 줄어듬

        // 슬라임 스폰 로직
        enemyTimer += Time.deltaTime;
        if (enemyTimer >= enemySpawnInterval)
        {
            SpawnEnemy();
            enemyTimer = 0f;
        }

        // 상자 스폰 로직 (변화 없음)
        treasureBoxTimer += Time.deltaTime;
        if (treasureBoxTimer >= treasureBoxSpawnInterval)
        {
            SpawnTreasureBox();
            treasureBoxTimer = 0f;
        }

        // 마법사 스폰 로직 (GameManager 시간 사용)
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

        // 골렘 스폰 로직 (GameManager 시간 사용)
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

    // Spawn 함수들은 ObjectPoolManager를 통해 적을 가져오므로 변경 없음
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