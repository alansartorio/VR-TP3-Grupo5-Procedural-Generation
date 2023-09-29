using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AlanSartorio.GridPathGenerator;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class MapGenerator : MonoBehaviour
{
    private GridPathGenerator<Vector2Int> _generator;
    private Dictionary<Vector2Int, GameObject> _objects = new();
    private Dictionary<Vector2Int, GameObject[]> _nodeBorderObjects = new();
    private Dictionary<Vector2Int, Vector2Int?> _nodeParent = new();

    [SerializeField] private GameObject baseObject;
    [SerializeField] private GameObject entranceObject;
    [SerializeField] private GameObject arrowObject;
    [SerializeField] private GameObject borderObject;
    [SerializeField] private float gridSize;

    [NonSerialized] public UnityEvent<GridPathGenerator<Vector2Int>> OnMapChanged = new();

    void Start()
    {
        _generator = new GridPathGenerator<Vector2Int>(1, 5, new Vector2IntNeighborGetter(), Vector2Int.zero);
        var delta = _generator.Initialize();
        OnMapChanged.Invoke(_generator);
        ApplyDelta(delta);
        for (int i = 0; i < 10; i++)
        {
            var positions = _generator.GetEnableablePositions().ToArray();
            ExpandNode(positions[Random.Range(0, positions.Length)]);
        }
    }

    public void ExpandNode(Vector2Int pos)
    {
        var delta = _generator.EnableNode(pos);
        ApplyDelta(delta);
        OnMapChanged.Invoke(_generator);
    }

    private void ApplyDelta(NodesDelta<Vector2Int> delta)
    {
        foreach (var node in delta.removedNodes)
        {
            _nodeParent.Remove(node.Position);
        }

        foreach (var added in delta.addedNodes)
        {
            _nodeParent[added.node.Position] = added.parent?.Position;
        }

        foreach (var enabled in delta.enabledNodes)
        {
            AddNode(enabled.Position, _nodeParent[enabled.Position]);
        }

        // Replace borders with entrances where determined
        foreach (var enableable in _generator.GetEnableablePositions())
        {
            var parent = _nodeParent[enableable];
            if (parent != null && _nodeBorderObjects.TryGetValue(parent.Value, out var parentBorders))
            {
                var angleIndex = DeltaToAngleIndex(enableable - parent.Value);
                var border = parentBorders[angleIndex];
                var entrance = Instantiate(entranceObject, _objects[parent.Value].transform);
                entrance.transform.localPosition = border.transform.localPosition;
                entrance.transform.localRotation = border.transform.localRotation;
                entrance.GetComponent<EntranceBehaviour>().NodePosition = enableable;
                Destroy(border);
                parentBorders[angleIndex] = entrance;
            }
        }

        // foreach (var node in _objects.Values)
        // {
        //     node.GetComponent<NodeBehaviour>()?.Set(false);
        // }
        //
        // foreach (var node in generator.GetEnableablePositions().Select(p => _objects[p]))
        // {
        //     node.GetComponent<NodeBehaviour>()?.SetEnableable(true);
        // }
    }

    private int DeltaToAngleIndex(Vector2Int delta)
    {
        return (delta.x, delta.y) switch
        {
            (0, 1) => 0,
            (1, 0) => 1,
            (0, -1) => 2,
            (-1, 0) => 3,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private void CreateNodeBorders(Vector2Int pos, int angleIndex, Transform parent)
    {
        var positions = new Vector3[]
        {
            new(0, 1, -90),
            new(1, 0, 0),
            new(0, -1, 90),
            new(-1, 0, 180),
        };

        _nodeBorderObjects[pos] = new GameObject[4];
        for (var i = 0; i < 4; i++)
        {
            var data = positions[(i - angleIndex + 4) % 4];
            var border = Instantiate(borderObject, parent, false);
            border.transform.localPosition += new Vector3(data.x * gridSize / 2, 0, data.y * gridSize / 2);
            border.transform.localRotation *= Quaternion.Euler(0, data.z, 0);
            border.name = $"Node Border {i}";
            _nodeBorderObjects[pos][i] = border;
        }
    }

    private void AddNode(Vector2Int pos, Vector2Int? parent)
    {
        GameObject nodeObject;
        if (parent == null)
        {
            if (baseObject != null)
            {
                nodeObject = Instantiate(baseObject, transform, true);
            }
            else
            {
                nodeObject = new GameObject("Node");
                nodeObject.transform.SetParent(transform);
                CreateNodeBorders(pos, 0, nodeObject.transform);
            }
        }
        else
        {
            nodeObject = new GameObject("Node");
            nodeObject.transform.SetParent(transform, false);

            var borderLeft = Instantiate(borderObject, nodeObject.transform, false);
            var borderRight = Instantiate(borderObject, nodeObject.transform, false);

            borderLeft.transform.localPosition += new Vector3(-gridSize / 2, 0, -gridSize);
            borderRight.transform.localPosition += new Vector3(gridSize / 2, 0, -gridSize);

            var delta = pos - parent.Value;
            var angleIndex = DeltaToAngleIndex(delta);

            nodeObject.name = $"node: angle={angleIndex}";

            CreateNodeBorders(pos, angleIndex, nodeObject.transform);

            Destroy(_nodeBorderObjects[parent.Value][angleIndex]);
            Destroy(_nodeBorderObjects[pos][(angleIndex + 2) % 4]);

            var arrow = Instantiate(arrowObject, nodeObject.transform);
            arrow.transform.position = new Vector3(0, 0.01f, -gridSize);

            nodeObject.transform.localRotation = Quaternion.Euler(0, angleIndex * 90f, 0);
        }

        nodeObject.transform.localPosition = GetNodeOrigin(pos);
        _objects.Add(pos, nodeObject);
    }

    public Vector3 GetNodeOrigin(Vector2Int pos)
    {
        return new Vector3(pos.x, 0, pos.y) * gridSize * 2;
    }
}