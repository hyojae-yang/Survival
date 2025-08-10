using UnityEngine;

public class GameManager : MonoBehaviour
{
    // �̱��� ����: ��𼭵� ���� GameManager�� ������ �� �ְ� �մϴ�.
    // MainScene������ �����ؾ� �ϹǷ� DontDestroyOnLoad�� �����մϴ�.
    public static GameManager Instance;

    [Header("Game Time & Difficulty")]
    public float gameElapsedTime; // ������ ���۵� ���� �� ��� �ð�
    public int difficultyLevel = 0; // ���� ���̵� ���� (0���� ����)

    public float difficultyInterval; // ���̵� ��� �ֱ� (10�� = 600��)
    private float nextDifficultyCheckTime; // ���� ���̵� ��� üũ �ð�

    [Header("Enemy Stat Multipliers")]
    public float enemyHealthMultiplier = 1f; // �� ü�� ����
    public float enemyDamageMultiplier = 1f; // �� ���ݷ� ����
    public float expMultiplier = 1f; // ����ġ ����

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // GameManager�� MainScene������ �ʿ��ϹǷ� DontDestroyOnLoad�� �����մϴ�.
            // DontDestroyOnLoad�� SettingsManager���� ����Ǿ�� �մϴ�.
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // 1. SettingsManager���� �ʱ� ���̵� �������� �����ɴϴ�.
        if (SettingsManager.Instance != null)
        {
            int initialDifficulty = SettingsManager.Instance.selectedDifficulty;
            SetInitialDifficulty(initialDifficulty);
            Debug.Log($"�ʱ� ���̵��� {initialDifficulty}�� �����Ǿ����ϴ�.");
        }
        else
        {
            Debug.LogWarning("SettingsManager�� ã�� �� �����ϴ�. �⺻ ���̵��� �����մϴ�.");
            SetInitialDifficulty(0); // SettingsManager�� ���� ���, �⺻ ���̵� 0���� ����
        }

        // 2. ���� ���� �״�� ���� ���̵� ��� üũ �ð��� �����մϴ�.
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

    // �ʱ� ���̵��� ���� ������ �����ϴ� ���ο� �Լ�
    void SetInitialDifficulty(int initialDifficulty)
    {
        // SettingsManager�� ��Ӵٿ� ���� ���� ������ �����մϴ�.
        switch (initialDifficulty)
        {
            case 0: // ����
                enemyHealthMultiplier = 1f;
                enemyDamageMultiplier = 1f;
                expMultiplier = 1f;
                difficultyLevel = 0;
                break;
            case 1: // ����
                enemyHealthMultiplier = 1.5f;
                enemyDamageMultiplier = 1.5f;
                expMultiplier = 1.5f;
                difficultyLevel = 0;
                break;
            case 2: // �����
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

        // ��� ������ 2�辿 ���� (���� ����)
        enemyHealthMultiplier *= 2f;
        enemyDamageMultiplier *= 2f;
        expMultiplier *= 2f;

        Debug.Log($"Difficulty Increased! Level: {difficultyLevel}, Health Multiplier: {enemyHealthMultiplier}, Damage Multiplier: {enemyDamageMultiplier}, Exp Multiplier: {expMultiplier}");
    }
}