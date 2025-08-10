using UnityEngine;

public class GameManager : MonoBehaviour
{
    // 싱글톤 패턴: 어디서든 쉽게 GameManager에 접근할 수 있게 합니다.
    // MainScene에서만 존재해야 하므로 DontDestroyOnLoad를 제거합니다.
    public static GameManager Instance;

    [Header("Game Time & Difficulty")]
    public float gameElapsedTime; // 게임이 시작된 이후 총 경과 시간
    public int difficultyLevel = 0; // 현재 난이도 레벨 (0부터 시작)

    public float difficultyInterval; // 난이도 상승 주기 (10분 = 600초)
    private float nextDifficultyCheckTime; // 다음 난이도 상승 체크 시간

    [Header("Enemy Stat Multipliers")]
    public float enemyHealthMultiplier = 1f; // 적 체력 배율
    public float enemyDamageMultiplier = 1f; // 적 공격력 배율
    public float expMultiplier = 1f; // 경험치 배율

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // GameManager는 MainScene에서만 필요하므로 DontDestroyOnLoad를 제거합니다.
            // DontDestroyOnLoad는 SettingsManager에만 적용되어야 합니다.
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // 1. SettingsManager에서 초기 난이도 설정값을 가져옵니다.
        if (SettingsManager.Instance != null)
        {
            int initialDifficulty = SettingsManager.Instance.selectedDifficulty;
            SetInitialDifficulty(initialDifficulty);
            Debug.Log($"초기 난이도가 {initialDifficulty}로 설정되었습니다.");
        }
        else
        {
            Debug.LogWarning("SettingsManager를 찾을 수 없습니다. 기본 난이도로 시작합니다.");
            SetInitialDifficulty(0); // SettingsManager가 없을 경우, 기본 난이도 0으로 설정
        }

        // 2. 기존 로직 그대로 다음 난이도 상승 체크 시간을 설정합니다.
        nextDifficultyCheckTime = difficultyInterval;
    }

    void Update()
    {
        gameElapsedTime += Time.deltaTime;

        if (gameElapsedTime >= nextDifficultyCheckTime)
        {
            IncreaseDifficulty();
            nextDifficultyCheckTime += difficultyInterval;
        }
    }

    // 초기 난이도에 따라 배율을 설정하는 새로운 함수
    void SetInitialDifficulty(int initialDifficulty)
    {
        // SettingsManager의 드롭다운 값에 따라 배율을 설정합니다.
        switch (initialDifficulty)
        {
            case 0: // 쉬움
                enemyHealthMultiplier = 1f;
                enemyDamageMultiplier = 1f;
                expMultiplier = 1f;
                difficultyLevel = 0;
                break;
            case 1: // 보통
                enemyHealthMultiplier = 1.5f;
                enemyDamageMultiplier = 1.5f;
                expMultiplier = 1.5f;
                difficultyLevel = 0;
                break;
            case 2: // 어려움
                enemyHealthMultiplier = 2.0f;
                enemyDamageMultiplier = 2.0f;
                expMultiplier = 2.0f;
                difficultyLevel = 0;
                break;
        }
    }

    void IncreaseDifficulty()
    {
        difficultyLevel++;

        // 모든 배율을 2배씩 증가 (기존 로직)
        enemyHealthMultiplier *= 2f;
        enemyDamageMultiplier *= 2f;
        expMultiplier *= 2f;

        Debug.Log($"Difficulty Increased! Level: {difficultyLevel}, Health Multiplier: {enemyHealthMultiplier}, Damage Multiplier: {enemyDamageMultiplier}, Exp Multiplier: {expMultiplier}");
    }
}