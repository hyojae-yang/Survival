using UnityEngine;

// Weapon ��ũ��Ʈ�� ��ӹ޴� �нú� ��ų
public class SpeedBoost : Weapon
{
    // ������ �̵� �ӵ� ������ (�ν����Ϳ��� ����)
    public float speedIncreasePerLevel = 1.0f;

    // �нú� ��ų�� ���� ������ �����ϴ� ����
    public int speedBoostLevel = 0;

    void Start()
    {
        // Start������ �ƹ� �۾��� ���� �ʽ��ϴ�.
        // �� ��ũ��Ʈ�� PlayerWeaponManager�� ���� OnLevelUp()�� ȣ��� ���� �۵��մϴ�.
    }

    // PlayerWeaponManager���� ȣ���� ������ �Լ�
    public void OnLevelUp()
    {
        // ������ 1 ������ŵ�ϴ�.
        speedBoostLevel++;
    }

    // PlayerWeaponManager�� �̵� �ӵ� ���ʽ� ���� ��û�� �� ȣ��Ǵ� �Լ�
    public float GetSpeedIncreaseBonus()
    {
        // ���� ������ speedIncreasePerLevel�� ���Ͽ� �� ���ʽ� ���� ��ȯ�մϴ�.
        return speedBoostLevel * speedIncreasePerLevel;
    }
}