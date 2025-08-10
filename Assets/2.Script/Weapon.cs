using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum WeaponType { Active, Passive }

public class Weapon : MonoBehaviour
{
    // ���� ����
    public string weaponName;
    public string weaponDescription;
    public Sprite weaponIcon;
    public WeaponType weaponType;
    public int maxLevel = 5;

    // ���� ����
    public float baseDamage;
    public float damageIncreasePerLevel;
    public float baseCooldown;
    public float cooldownDecreasePerLevel;

    // �߰��� ����: ���� ����
    public float baseRangeIncrease;          // ���������� ��� �߰� ������ ���� ��
    public float rangeIncreasePerLevel;      // ������ �� �߰� ������ �󸶳� ��������

    // ���� ����
    public int currentLevel;

    // �ʱ� �⺻ ���ݷ� (�ʱ�ȭ�� ���)
    private float initialBaseDamage;
    // �ʱ� �⺻ ��Ÿ�� (�ʱ�ȭ�� ���)
    private float initialBaseCooldown;
    // �ʱ� �⺻ �߰� ���� (�ʱ�ȭ�� ���)
    private float initialBaseRangeIncrease;

    private void Start()
    {
        InitializeWeapon();
    }

    public void InitializeWeapon()
    {
        initialBaseDamage = baseDamage;
        initialBaseCooldown = baseCooldown;
        initialBaseRangeIncrease = baseRangeIncrease; // �ʱ� �߰� ���� ����
        currentLevel = 1;
    }

    public float GetCurrentDamage()
    {
        return initialBaseDamage + (damageIncreasePerLevel * (currentLevel - 1));
    }

    public float GetCurrentCooldown()
    {
        return Mathf.Max(0, initialBaseCooldown - (cooldownDecreasePerLevel * (currentLevel - 1)));
    }

    // �߰��� �Լ�: ���� ������ ���� �߰� ���� ���� ������
    public float GetCurrentRangeIncrease()
    {
        return initialBaseRangeIncrease + (rangeIncreasePerLevel * (currentLevel - 1));
    }

    public virtual void LevelUp() // virtual Ű���� �߰�: �ڽ� Ŭ�������� �������̵� �����ϵ���
    {
        if (currentLevel >= maxLevel)
        {
            return;
        }

        currentLevel++;
        // ������ �� baseRangeIncrease�� �Բ� ������Ŵ
        // baseDamage�� baseCooldown�� GetCurrent~ �Լ����� ���ǹǷ� ���⼭�� ���� �������� �ʽ��ϴ�.
    }
}