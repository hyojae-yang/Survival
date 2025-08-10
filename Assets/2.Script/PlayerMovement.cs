using UnityEngine;
using UnityEngine.SceneManagement; // 씬 전환을 위해 추가

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;

    private Rigidbody2D rb;
    private Vector2 movement;
    private PlayerStats playerStats;

    // 플레이어의 마지막 이동 방향
    public Vector2 lastMoveDirection = Vector2.right; // 기본값 설정

    [Header("Movement Bounds")]
    public float minX = -59f;
    public float maxX = 59f;
    public float minY = -42f;
    public float maxY = 42f;

    // 플레이어 자신의 콜라이더를 가져오기 위한 변수
    private BoxCollider2D playerCollider;

    // --- 일시정지 기능 관련 변수 추가 ---
    [Header("Pause Panel")]
    public GameObject pausePanel;
    private bool isGamePaused = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerStats = GameObject.FindWithTag("Player").GetComponent<PlayerStats>();
        playerCollider = GetComponent<BoxCollider2D>();

        if (rb == null) Debug.LogError("Rigidbody2D가 Player 오브젝트에 없습니다!");
        if (playerStats == null) Debug.LogError("PlayerStats가 Player 오브젝트에 없습니다!");
        if (playerCollider == null) Debug.LogError("BoxCollider2D가 Player 오브젝트에 없습니다!");

        // 게임 시작 시 일시정지 패널이 꺼져있는지 확인
        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }
    }

    void Update()
    {
        // --- 스페이스바 입력 감지 및 일시정지/재개 기능 추가 ---
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGamePaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }

        // 일시정지 상태에서는 이동 입력이 처리되지 않도록 방지
        if (!isGamePaused)
        {
            movement.x = Input.GetAxis("Horizontal");
            movement.y = Input.GetAxis("Vertical");

            if (movement.x != 0 || movement.y != 0)
            {
                lastMoveDirection = movement.normalized;
            }
        }
    }

    void FixedUpdate()
    {
        // --- 일시정지 상태에서는 물리 업데이트를 중지 ---
        if (isGamePaused)
        {
            return;
        }

        // 1. 플레이어 이동 속도 및 이동
        if (playerStats != null)
        {
            moveSpeed = playerStats.moveSpeed;
        }
        rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);

        // 2. 플레이어 위치를 정의된 범위 내로 제한
        ClampPlayerPosition();

        // 3. 플레이어 회전 로직 (기존 코드 유지)
        if (movement.x != 0)
        {
            Vector3 originalScale = transform.localScale;
            originalScale.x = Mathf.Abs(originalScale.x) * -Mathf.Sign(movement.x);
            transform.localScale = originalScale;
        }
    }

    // 플레이어 위치를 제한하는 함수
    private void ClampPlayerPosition()
    {
        Vector3 currentPosition = transform.position;

        // 플레이어 콜라이더의 크기를 고려하여 경계선 계산
        float clampedX = Mathf.Clamp(currentPosition.x, minX + playerCollider.bounds.extents.x, maxX - playerCollider.bounds.extents.x);
        float clampedY = Mathf.Clamp(currentPosition.y, minY + playerCollider.bounds.extents.y, maxY - playerCollider.bounds.extents.y);

        transform.position = new Vector3(clampedX, clampedY, currentPosition.z);
    }

    // --- 일시정지/재개/타이틀로 이동 함수 추가 ---

    private void PauseGame()
    {
        isGamePaused = true;
        Time.timeScale = 0; // 게임 시간 정지
        if (pausePanel != null)
        {
            pausePanel.SetActive(true);
        }
    }

    // 버튼과 연결될 함수
    public void ResumeGame()
    {
        isGamePaused = false;
        Time.timeScale = 1; // 게임 시간 재개
        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }
        // 버튼 클릭 효과음
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySFX(SoundManager.Instance.buttonSFX);
        }
    }

    // 버튼과 연결될 함수
    public void GoToTitle()
    {
        // 타이틀로 돌아가기 전에 게임 시간을 원래대로 되돌려야 함
        Time.timeScale = 1;
        // 버튼 클릭 효과음
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySFX(SoundManager.Instance.buttonSFX);
        }
        SceneManager.LoadScene("TitleScene");
    }

    // 에디터에서 이동 경계선을 시각적으로 보여주는 기즈모 함수
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;

        Vector3 center = new Vector3((minX + maxX) / 2, (minY + maxY) / 2, 0);
        Vector3 size = new Vector3(maxX - minX, maxY - minY, 0);

        Gizmos.DrawWireCube(center, size);
    }
}