using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Collider))]
public class LightDetector : MonoBehaviour
{
    [Header("��������� �����������")]
    [SerializeField] private Transform[] _detectionPoints; // ����� �� ���� ������
    [SerializeField] private LayerMask _lightLayer; // ���� ���������� �����
    [SerializeField] private LayerMask _obstructionLayer; // ���� �����������
    [SerializeField] private LayerMask _ignoreLayer; // ���� ������ (������������)

    [Header("�������")]
    [SerializeField] private bool _visualize = true;
    [SerializeField][Range(0, 1)] private float _illuminationLevel;

    private Collider _detectorCollider;
    private HashSet<Collider> _activeLights = new HashSet<Collider>();

    public float IlluminationLevel => _illuminationLevel;

    private void Awake()
    {
        _detectorCollider = GetComponent<Collider>();
        _detectorCollider.isTrigger = true;

        Debug.Log($"[���������] �������� �����. ����� �����������: {_detectionPoints?.Length ?? 0}");
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"��������� ������: {other.name}\n" +
                     $"���: {other.tag}\n" +
                     $"����: {LayerMask.LayerToName(other.gameObject.layer)}\n" +
                     $"Has Rigidbody: {other.attachedRigidbody != null}");

        if (!IsValidLight(other)) return;

        _activeLights.Add(other);
        Debug.Log($"[���������] ��������� �������� �����: {other.name}");
        UpdateIllumination();
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log($"[���������] ��������� �������� �����: {other.name} (�������� � ��������)");
        if (!IsValidLight(other)) return;
        UpdateIllumination();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!IsValidLight(other)) return;

        _activeLights.Remove(other);
        Debug.Log($"[���������] �������� ����� �������: {other.name}");
        UpdateIllumination();
    }

    private bool IsValidLight(Collider col)
    {
        if (col == null) return false;

        // ��������� ��� � ���� ����� ���������� ������ Unity
        return col.CompareTag("Light") &&
               _lightLayer == (_lightLayer | (1 << col.gameObject.layer));
    }

    private void UpdateIllumination()
    {
        if (_activeLights.Count == 0 || _detectionPoints == null)
        {
            _illuminationLevel = 0f;
            return;
        }

        int visibleHits = 0;
        int totalChecks = _detectionPoints.Length * _activeLights.Count;
        LayerMask raycastMask = _obstructionLayer & ~_ignoreLayer;

        foreach (var point in _detectionPoints)
        {
            if (point == null) continue;

            foreach (var lightCollider in _activeLights)
            {
                if (lightCollider == null) continue;

                Vector3 direction = lightCollider.transform.position - point.position;
                if (!Physics.Raycast(point.position, direction.normalized, direction.magnitude, raycastMask))
                {
                    visibleHits++;

                    if (_visualize)
                        Debug.DrawLine(point.position, lightCollider.transform.position, Color.green, 0.1f);
                }
                else if (_visualize)
                {
                    Debug.DrawLine(point.position, lightCollider.transform.position, Color.red, 0.1f);
                }
            }
        }

        _illuminationLevel = totalChecks > 0 ? (float)visibleHits / totalChecks : 0f;

        Debug.Log($"[���������] ����������: " +
                 $"�����: {_detectionPoints.Length}, " +
                 $"����������: {_activeLights.Count}, " +
                 $"���������: {visibleHits}/{totalChecks}, " +
                 $"������������: {_illuminationLevel:P0}");
    }

    private void OnDrawGizmosSelected()
    {
        if (_detectionPoints == null) return;

        Gizmos.color = Color.cyan;
        foreach (var point in _detectionPoints)
        {
            if (point != null)
                Gizmos.DrawWireSphere(point.position, 0.1f);
        }
    }
}