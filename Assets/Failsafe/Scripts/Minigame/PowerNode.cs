using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct DirectionNodePair
{
    public Direction Direction;
    public PowerNode Node;
}

public enum Direction { Forward, Right, Back, Left }

public abstract class PowerNode : MonoBehaviour
{
    // ������ �� ������������
    protected Dictionary<Direction, PowerNode> Neighbors;
    [SerializeField] protected List<DirectionNodePair> NeighborsSerialized = new List<DirectionNodePair>();

    // ����� ����������� ��������� � ���� ���� (��������, ���� � �����)
    protected HashSet<Direction> ConnectedDirections;
    [SerializeField] protected List<Direction> ConnectedDirectionsSerialized = new List<Direction>();

    // ������� �� �������
    protected bool IsPowered = false;
    //public Direction FromDirection;
    protected bool PowerReceivedThisCycle = false;

    protected virtual void Awake()
    {
        Neighbors = new Dictionary <Direction, PowerNode>();
        foreach (var pair in NeighborsSerialized)
        {
            if (!Neighbors.ContainsKey(pair.Direction))
                Neighbors.Add(pair.Direction, pair.Node);
        }
        ConnectedDirections = new HashSet<Direction>(ConnectedDirectionsSerialized);
    }

    // ����� ������� ��������������� �������
    public void ReceivePower(Direction fromDirection)
    {
        if (IsPowered) return;
        Debug.Log(gameObject.name);
        if (!ConnectedDirections.Contains(Opposite(fromDirection)))
        {
            Debug.Log(ConnectedDirections.Contains(Opposite(fromDirection)));
            return; // ������� ������ �� � ������������ �������
        }
        Debug.Log(gameObject.name + "+");
        IsPowered = true;
        OnPowered();

        // �������������� ������ �� ������ ������������ (����� ����, ������ ������)
        foreach (var connectedDirection in ConnectedDirections)
        {
            if (connectedDirection == Opposite(fromDirection)) 
                continue;
            if (Neighbors.ContainsKey(connectedDirection))
            {
                Neighbors[connectedDirection].ReceivePower(connectedDirection);
            }
        }
    }

    // ����� ��� ������� ������� ������� (��������, � ��������� �����)
    public void StartPower()
    {
        if (IsPowered) return;
        IsPowered = true;
        OnPowered();

        foreach (var connectedDirecrion in ConnectedDirections)
        {
            if (Neighbors.ContainsKey(connectedDirecrion))
            {
                Neighbors[connectedDirecrion].ReceivePower(connectedDirecrion);
            }
        }
    }

    protected virtual void OnPowered()
    {
        Debug.Log($"{name} powered");
        gameObject.GetComponent<Renderer>().material.color = Color.red;
    }
    // ����� ��� ������ ������� (����������)
    public virtual void ResetPower()
    {
        IsPowered = false;
        PowerReceivedThisCycle = false;
        OnPowerLost();
    }

    // �����, ���������� ��� ���������� ������� (����� ��������������)
    protected virtual void OnPowerLost()
    {
        Debug.Log($"{name} lost power"); 
        gameObject.GetComponent<Renderer>().material.color = Color.white;
    }

    public static Direction Opposite(Direction dir)
    {
        switch (dir)
        {
            case Direction.Forward: return Direction.Back;
            case Direction.Back: return Direction.Forward;
            case Direction.Left: return Direction.Right;
            case Direction.Right: return Direction.Left;
        }
        return dir;
    }
}
