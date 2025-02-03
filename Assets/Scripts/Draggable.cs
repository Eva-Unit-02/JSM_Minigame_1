using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draggable : MonoBehaviour
{
    Vector3 mousePositionOffset;

    private Vector3 GetMouseWorldPositon()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
    
    private void OnMouseDown()
    {
        mousePositionOffset = transform.position - GetMouseWorldPositon();
    }

    private void OnMouseDrag()
    {
        transform.position = GetMouseWorldPositon() + mousePositionOffset;
    }

    public bool IsMouseDown()
    {
        return Input.GetMouseButton(0);
    }



}
