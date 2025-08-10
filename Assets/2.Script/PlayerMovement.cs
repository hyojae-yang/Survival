using UnityEngine;
using UnityEngine.SceneManagement; // �� ��ȯ�� ���� �߰�

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;

    private Rigidbody2D rb;
    private Vector2 movement;
    private PlayerStats playerStats;

    // �÷��̾��� ������ �̵� ����
    public Vector2 lastMoveDirection = Vector2.right; // �⺻�� ����

    [Header("Movement Bounds")]
    public float minX = -59f;
    public float maxX = 59f;
    public float minY = -42f;
    public float maxY = 42f;

    // �÷��̾� �ڽ��� �ݶ��̴��� �������� ���� ����
    private BoxCollider2D playerCollider;

    // --- �Ͻ����� ��� ���� ���� �߰� ---
    [Header("Pause Panel")]
    public GameObject pausePanel;
    private bool isGamePaused = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerStats = GameObject.FindWithTag("Player").GetComponent<PlayerStats>();
        playerCollider = GetComponent<BoxCollider2D>();

        if (rb == null) Debug.LogError("Rigidbody2D�� Player ������Ʈ�� �����ϴ�!");
        if (playerStats == null) Debug.LogError("PlayerStats�� Player ������Ʈ�� �����ϴ�!");
        if (playerCollider == null) Debug.LogError("BoxCollider2D�� Player ������Ʈ�� �����ϴ�!");

        // ���� ���� �� �Ͻ����� �г��� �����ִ��� Ȯ��
        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }
    }

    void Update()
    {
        // --- �����̽��� �Է� ���� �� �Ͻ�����/�簳 ��� �߰� ---
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

        // �Ͻ����� ���¿����� �̵� �Է��� ó������ �ʵ��� ����
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
        // --- �Ͻ����� ���¿����� ���� ������Ʈ�� ���� ---
        if (isGamePaused)
        {
            return;
        }

        // 1. �÷��̾� �̵� �ӵ� �� �̵�
        if (playerStats != null)
        {
            moveSpeed = playerStats.moveSpeed;
        }
        rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);

        // 2. �÷��̾� ��ġ�� ���ǵ� ���� ���� ����
        ClampPlayerPosition();

        // 3. �÷��̾� ȸ�� ���� (���� �ڵ� ����)
        if (movement.x != 0)
        {
            Vector3 originalScale = transform.localScale;
            originalScale.x = Mathf.Abs(originalScale.x) * -Mathf.Sign(movement.x);
            transform.localScale = originalScale;
        }
    }

    // �÷��̾� ��ġ�� �����ϴ� �Լ�
    private void ClampPlayerPosition()
    {
        Vector3 currentPosition = transform.position;

        // �÷��̾� �ݶ��̴��� ũ�⸦ ����Ͽ� ��輱 ���
        float clampedX = Mathf.Clamp(currentPosition.x, minX + playerCollider.bounds.extents.x, maxX - playerCollider.bounds.extents.x);
        float clampedY = Mathf.Clamp(currentPosition.y, minY + playerCollider.bounds.extents.y, maxY - playerCollider.bounds.extents.y);

        transform.position = new Vector3(clampedX, clampedY, currentPosition.z);
    }

    // --- �Ͻ�����/�簳/Ÿ��Ʋ�� �̵� �Լ� �߰� ---

    private void PauseGame()
    {
        isGamePaused = true;
        Time.timeScale = 0; // ���� �ð� ����
        if (pausePanel != null)
        {
            pausePanel.SetActive(true);
        }
    }

    // ��ư�� ����� �Լ�
    public void ResumeGame()
    {
        isGamePaused = false;
        Time.timeScale = 1; // ���� �ð� �簳
        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }
        // ��ư Ŭ�� ȿ����
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySFX(SoundManager.Instance.buttonSFX);
        }
    }

    // ��ư�� ����� �Լ�
    public void GoToTitle()
    {
        // Ÿ��Ʋ�� ���ư��� ���� ���� �ð��� ������� �ǵ����� ��
        Time.timeScale = 1;
        // ��ư Ŭ�� ȿ����
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySFX(SoundManager.Instance.buttonSFX);
        }
        SceneManager.LoadScene("TitleScene");
    }

    // �����Ϳ��� �̵� ��輱�� �ð������� �����ִ� ����� �Լ�
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;

        Vector3 center = new Vector3((minX + maxX) / 2, (minY + maxY) / 2, 0);
        Vector3 size = new Vector3(maxX - minX, maxY - minY, 0);

        Gizmos.DrawWireCube(center, size);
    }
}