using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class SoundDetection : MonoBehaviour
{
    [SerializeField] private float baseDetectionRadiusNear = 10f; // ������ ����������� �����
    [SerializeField] private float baseDetectionRadiusMedium = 20f; // ���� ����������� �����
    [SerializeField] private float baseDetectionRadiusFar = 30f; // ������ ����������� �����


    private void OnCollisionEnter(Collision collision)
    {
        // ������� ���� ������ � ������� baseDetectionRadiusFar
        Collider[] colliders = Physics.OverlapSphere(transform.position, baseDetectionRadiusFar);

        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Enemy"))
            {
                float distance = Vector3.Distance(transform.position, col.transform.position);

                if (distance <= baseDetectionRadiusNear)
                {
                    // ������ ��� ������� ���� (��������, ���������� �������)
                    Debug.Log($"��������� {col.name} � ������� ����! (����������: {distance})");
                }
                else if (distance <= baseDetectionRadiusMedium)
                {
                    // ������ ��� ������� ���� (��������, ����������)
                    Debug.Log($"��������� {col.name} � ������� ����. (����������: {distance})");
                }
                else if (distance <= baseDetectionRadiusFar)
                {
                    // ������ ��� ������� ���� (��������, ����������� �������)
                    Debug.Log($"��������� {col.name} � ������� ����. (����������: {distance})");
                }
            }
        }
    }
}


