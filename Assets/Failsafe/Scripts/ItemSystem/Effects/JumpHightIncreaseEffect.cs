using UnityEngine;

public class JumpHightIncreaseEffect : BaseEffect
{
    public float JumpHightIncreaseAmount;
    public override void Apply()
    {
        //to-do ����� ���������� ������ ������
        Debug.Log($"������ ������ ��������� �������� �� {JumpHightIncreaseAmount}");
    }
}