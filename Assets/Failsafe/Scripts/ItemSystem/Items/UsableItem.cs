using UnityEngine;

public class UsableItem:Item, IUsable
{
    // �������
    public BaseEffect[] effects = null;
    public void Use()
    {
        // to-do ��������
        foreach (var effect in effects)
        {
            effect.Apply();
        }
        Debug.Log("������� �����������");
    }
}