using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCan : MonoBehaviour
{
    private SpriteRenderer sp;
    private Color baseColor;

    private void Start()
    {
        sp = GetComponent<SpriteRenderer>();
        baseColor = sp.color;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        
        // if (!collision.gameObject.CompareTag("PaperHitBox")) return;

        
        Draggable draggable = collision.gameObject.GetComponent<Draggable>();
        if (draggable != null)
        {
            sp.color = Color.white;
            // Check if Player release mouse
            if (!draggable.IsMouseDown())
            {
                Destroy(draggable.gameObject);
                Debug.Log($"Object stored in Trash Can");
            }
            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        sp.color = baseColor;
    }
}
