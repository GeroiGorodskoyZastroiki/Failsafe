using UnityEngine;

public class HealthIncreaseEffect : BaseEffect
{
    public int HealthAmount;
    public override void Apply()
    {
        //to-do ����� ���������� �������� ������
        Debug.Log($"�������� ��������� �������� �� {HealthAmount}");
    }
}