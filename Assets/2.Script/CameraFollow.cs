using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // 추적할 대상인 플레이어를 public 변수로 설정
    // 인스펙터 창에서 플레이어 오브젝트를 드래그해서 넣어주세요.
    public Transform player;

    // 카메라의 오프셋(offset)을 설정
    // 카메라가 플레이어로부터 얼마나 떨어져 있을지 정합니다.
    // X, Y는 0으로 두고 Z만 -10 정도로 설정하면 2D 게임에 적합합니다.
    Vector3 offset=new Vector3(0,0,-10);

    // LateUpdate()는 모든 Update() 함수가 호출된 후 호출됩니다.
    // 플레이어의 움직임이 모두 계산된 후에 카메라가 따라가는 것이 자연스럽기 때문에
    // Update() 대신 LateUpdate()를 사용합니다.
    void LateUpdate()
    {
        // 플레이어의 위치에 오프셋을 더하여 카메라의 위치를 업데이트합니다.
        transform.position = player.position + offset;
    }
}