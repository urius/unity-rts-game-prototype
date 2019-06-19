using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCameraByTouch : MonoBehaviour
{
    public Collider moveRestriction;

    [SerializeField]
    private LayerMask _draggableLayers;
    private bool _isDragging = false;
    private Vector3 _startDragPoint;
    private Camera _camera;
    private Plane _groundPlane;
    private Vector3 _dragOffsetVector;

    void Awake()
    {
        _camera = GetComponent<Camera>();

        _groundPlane = new Plane(Vector3.up, Vector3.zero);
    }
    void Start()
    {
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (GetDraggableMousePoint(out var hitPoint))
            {
                _startDragPoint = hitPoint;
                _isDragging = true;
            }
        }
        else if (_isDragging)
        {
            if (Input.GetMouseButton(0) && GetDraggableMousePoint(out var hitPoint))
            {
                _dragOffsetVector = hitPoint - _startDragPoint;
                var newPosition = _camera.transform.position - _dragOffsetVector;

                if (moveRestriction != null && !moveRestriction.bounds.Contains(newPosition))
                {
                    newPosition = moveRestriction.ClosestPointOnBounds(newPosition);
                }

                _camera.transform.position = newPosition;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                _isDragging = false;
            }
        }
    }

    private bool GetDraggableMousePoint(out Vector3 point)
    {
        point = Vector3.zero;
        var screenRay = _camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(screenRay, out var hit, int.MaxValue))
        {
            var hitLayerName = LayerMask.LayerToName(hit.transform.gameObject.layer);
            if ((LayerMask.GetMask(hitLayerName) & _draggableLayers.value) != 0)
            {
                _groundPlane.Raycast(screenRay, out var enter);
                point = screenRay.GetPoint(enter);
                return true;
            }
        }
        return false;
    }
}
