using UnityEngine;

public class Golem : Enemy
{
    // 골렘 스크립트에는 별도의 능력치 설정 로직이 필요 없습니다.
    // 모든 능력치 설정 및 배율 적용은 Enemy.cs의 Awake() 함수에서 처리됩니다.
    // 따라서 Start() 함수를 오버라이드할 필요가 없습니다.

    // 만약 골렘 고유의 기능이 있다면, 이 안에 추가할 수 있습니다.
    // protected override void Die()
    // {
    //     // 부모 클래스의 Die() 함수를 먼저 호출
    //     base.Die();
    //     
    //     // 예: 골렘이 죽을 때 폭발 효과를 낸다거나 하는 고유 로직
    // }
}