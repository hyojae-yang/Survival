using UnityEngine;

public class Slime : Enemy
{
    // 슬라임 스크립트에는 별도의 능력치 설정 로직이 필요 없습니다.
    // 모든 능력치 설정 및 배율 적용은 Enemy.cs의 Awake() 함수에서 처리됩니다.
    // 따라서 Start() 함수를 오버라이드할 필요가 없습니다.

    // 몬스터가 죽었을 때 호출되는 함수를 오버라이드하여,
    // 필요하다면 슬라임 고유의 추가적인 행동을 구현할 수 있습니다.
    protected override void Die()
    {
        // 부모 클래스인 Enemy.cs의 Die() 함수를 호출하여
        // 경험치 드롭 및 오브젝트 풀 반환 로직을 실행합니다.
        base.Die();
    }
}