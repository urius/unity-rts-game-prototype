using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selectable : MonoBehaviour
{
    [SerializeField]
    private bool isSelected = false;

    [SerializeField]
    private GameObject selection;
    [SerializeField]
    private Collider selectionArea;

    void Start()
    {
        selection.SetActive(isSelected);
    }

    void OnMouseDown()
    {
        if (isActiveAndEnabled)
        {
            var caneraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (selectionArea.Raycast(caneraRay, out var _, int.MaxValue))
            {
                isSelected = true;
                selection.SetActive(isSelected);
            }
        }
    }

    void OnMouseUp()
    {
        isSelected = false;
        selection.SetActive(isSelected);
    }

    // Editor only!
    void OnValidate()
    {
        Start();
    }
}
