using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drawer : MonoBehaviour
{
    public int num;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("PaperHitBox")) return;
        
        Draggable draggable = collision.transform.parent.GetComponent<Draggable>();
        if (draggable != null && !draggable.IsMouseDown())
        {
            
            Destroy(draggable.gameObject);
            Debug.Log($"Object stored in the drawer {num}");
        }
    }
}
