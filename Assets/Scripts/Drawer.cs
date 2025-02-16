using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drawer : MonoBehaviour
{
    public int num;
    public string type;
    public GameManager gameManager;
    private SpriteRenderer sp;
    private Color baseColor;
    

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        sp = GetComponent<SpriteRenderer>();
        baseColor = sp.color;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        
        if (!collision.gameObject.CompareTag("PaperHitBox")) return;

        
        Draggable draggable = collision.transform.parent.GetComponent<Draggable>();
        if (draggable != null)
        {
            sp.color = Color.white;
            // Check if Player release mouse
            if (!draggable.startDrag)
            {
                // Check if type match
                if (draggable.GetComponent<Document>().type == this.type)
                {
                    Debug.Log($"CORRECT!!! Object stored in the drawer {num}");
                    gameManager.ChangeMoney(5);
                    // gameManager.ChangeReputation(5);
                } 
                else
                {
                    Debug.Log($"WRONG!!! Object stored in the drawer {num}");
                    gameManager.ChangeMoney(-10);
                    gameManager.ChangeReputation(-5);
                }
                Destroy(draggable.gameObject);
                
            }
            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        sp.color = baseColor;
    }
}
