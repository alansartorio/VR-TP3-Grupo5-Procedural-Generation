using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

[RequireComponent(typeof(GameStateManager))]
public class ExpandMapState : MonoBehaviour
{
    [SerializeField] private InputActionReference pointerPosition;
    [SerializeField] private InputActionReference interact;
    [SerializeField] private MapGenerator mapGenerator;
    private GameStateManager _gameStateManager;

    private void Awake()
    {
        _gameStateManager = GetComponent<GameStateManager>();
    }

    private void OnEnable()
    {
        interact.action.performed += InteractPerformed;
    }

    private void OnDisable()
    {
        interact.action.performed -= InteractPerformed;
    }
    
    private void InteractPerformed(InputAction.CallbackContext obj)
    {
        var pos = pointerPosition.action.ReadValue<Vector2>();
        var ray = Camera.main.ScreenPointToRay(pos);

        if (!Physics.Raycast(ray, out var hitInfo))
            return;

        var hitTransform = hitInfo.transform;
        if (!hitTransform.CompareTag("Entrance"))
            return;

        var entrance = hitTransform.GetComponent<EntranceBehaviour>();
        var nodePos = entrance.NodePosition;
        
        mapGenerator.ExpandNode(nodePos);
        
        _gameStateManager.PlayerExpandedMap();
    }
}
