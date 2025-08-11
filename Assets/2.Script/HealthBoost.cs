using UnityEngine;

// Weapon ��ũ��Ʈ�� ��ӹ޴� �нú� ��ų
public class HealthBoost : Weapon
{
    // ������ ü�� ���ʽ� ���� (�ν����Ϳ��� ����)
    public float healthMultiplierBonus = 1.0f;

    // �нú� ��ų�� ���� ������ �����ϴ� ����
    public int healthBoostLevel = 0;

    void Start()
    {
        // Start������ �ƹ� �۾��� ���� �ʽ��ϴ�.
        // �� ��ũ��Ʈ�� PlayerWeaponManager�� ���� OnLevelUp()�� ȣ��� ���� �۵��մϴ�.
    }

    // PlayerWeaponManager���� ȣ���� ������ �Լ�
    public void OnLevelUp()
    {
        // ������ 1 ������ŵ�ϴ�.
        healthBoostLevel++;
    }

    // PlayerWeaponManager�� ü�� ���ʽ� ���� ��û�� �� ȣ��Ǵ� �Լ�
    public float GetHealthMultiplierBonus()
    {
        // ���� ������ healthMultiplierBonus�� ���Ͽ� �� ���ʽ� ���� ��ȯ�մϴ�.
        // ���� ���, 1�����̸� 100% (1.0f), 2�����̸� 200% (2.0f) ���ʽ�.
        return healthBoostLevel * healthMultiplierBonus;
    }
}