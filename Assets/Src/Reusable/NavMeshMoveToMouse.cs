using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshMoveToMouse : MonoBehaviour
{
    private NavMeshAgent _navMeshAgent;
    private bool _chargedForMove = false;

    void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void OnMouseDown()
    {
        _chargedForMove = true;
    }

    void OnMouseUp()
    {
        if (_chargedForMove)
        {
            _chargedForMove = false;
            if (isActiveAndEnabled)
            {
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out var hit))
                {
                    var direction = Vector3.Normalize(hit.point - transform.position);

                    var targetPosition = hit.point;
                    targetPosition.y = transform.position.y;

                    _navMeshAgent.SetDestination(targetPosition);
                    //_moveToPointScript.targetPosition = targetPosition;
                }
            }
        }
    }


    void OnDrawGizmos()
    {
        if(_navMeshAgent == null) {
            return;
        }

        for (var i = 0; i < _navMeshAgent.path.corners.Length; i++)
        {
            var corner = _navMeshAgent.path.corners[i];
            Gizmos.DrawCube(corner, new Vector3(5, 5, 5));
        }
        Debug.DebugBreak();
    }
}
