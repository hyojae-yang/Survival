using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // ������ ����� �÷��̾ public ������ ����
    // �ν����� â���� �÷��̾� ������Ʈ�� �巡���ؼ� �־��ּ���.
    public Transform player;

    // ī�޶��� ������(offset)�� ����
    // ī�޶� �÷��̾�κ��� �󸶳� ������ ������ ���մϴ�.
    // X, Y�� 0���� �ΰ� Z�� -10 ������ �����ϸ� 2D ���ӿ� �����մϴ�.
    Vector3 offset=new Vector3(0,0,-10);

    // LateUpdate()�� ��� Update() �Լ��� ȣ��� �� ȣ��˴ϴ�.
    // �÷��̾��� �������� ��� ���� �Ŀ� ī�޶� ���󰡴� ���� �ڿ������� ������
    // Update() ��� LateUpdate()�� ����մϴ�.
    void LateUpdate()
    {
        // �÷��̾��� ��ġ�� �������� ���Ͽ� ī�޶��� ��ġ�� ������Ʈ�մϴ�.
        transform.position = player.position + offset;
    }
}