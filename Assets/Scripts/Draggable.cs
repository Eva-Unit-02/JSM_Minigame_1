using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Draggable : MonoBehaviour
{
    private Vector3 mousePositionOffset;
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D coll;
    private bool startDrag;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        coll = GetComponent<BoxCollider2D>();
    }
    private Vector3 GetMouseWorldPositon()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Collider2D[] collidersHit = Physics2D.OverlapPointAll(GetMouseWorldPositon());

            // Check if current collider is being hit
            if (!collidersHit.Contains(coll)) return;

            foreach (Collider2D collider in collidersHit)
            {
                if (!collider.gameObject.GetComponent<Draggable>()) continue;

                SpriteRenderer otherRenderer = collider.GetComponent<SpriteRenderer>();

                // Skip if the other collider doesn't have a SpriteRenderer
                if (otherRenderer == null)
                    continue;

                // Compare Sorting Layers
                if (otherRenderer.sortingLayerID > spriteRenderer.sortingLayerID)
                    return; // Other object is on a higher Sorting Layer

                // If Sorting Layers are the same, compare Order in Layer
                if (otherRenderer.sortingLayerID == spriteRenderer.sortingLayerID &&
                    otherRenderer.sortingOrder > spriteRenderer.sortingOrder)
                    return; // Other object is on a higher Order in Layer
            }

            // This object is the topmost
            startDrag = true;
            mousePositionOffset = transform.position - GetMouseWorldPositon();
        }

        if (Input.GetMouseButton(0))
        {
            if (startDrag)
            {
                transform.position = GetMouseWorldPositon() + mousePositionOffset;
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            startDrag = false;
        }
    }

    public bool IsMouseDown()
    {
        return Input.GetMouseButton(0);
    }
}