using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.Design;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Purchasing;
using UnityEngine;
using UnityEngine.UI;

public class Draggable : MonoBehaviour
{
    private Vector3 mousePositionOffset;
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D coll;
    private GameObject shadow;
    private GameManager gameManager;
    public bool startDrag;
    public float dragLag = 0.3f;
    public float floatDistance;
    

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        coll = GetComponent<BoxCollider2D>();
        gameManager = FindObjectOfType<GameManager>();
        shadow = transform.GetChild(1).gameObject;

        gameManager.OnPaperAdded += PutCurrentPaperToFront;
    }
    private Vector3 GetMouseWorldPositon()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void Update()
    {
        if (startDrag)
        {
            transform.position = Vector3.Lerp(transform.position, GetMouseWorldPositon() + mousePositionOffset, 0.3f);
            // transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, new Vector3(0, 0, -Input.GetAxis("Mouse X")*20), 0.3f);
            // transform.eulerAngles = new Vector3(-Input.GetAxis("Mouse Y")*200, -Input.GetAxis("Mouse X")*200, -Input.GetAxis("Mouse X")*20);
            Quaternion targetRotation = Quaternion.Euler(0, 0, -Input.GetAxis("Mouse X") * 30);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 0.3f);


            if (Input.GetMouseButtonDown(0))
            {
                startDrag = false;
                shadow.SetActive(false);
                transform.rotation = Quaternion.Euler(0, 0, 0);
                StartCoroutine(LerpDown(transform, floatDistance, 0.1f));

                return;
            }
        }

        if (!startDrag)
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
                mousePositionOffset = transform.position - GetMouseWorldPositon() + new Vector3(0, floatDistance, 0);
                shadow.SetActive(true);

                PutCurrentPaperToFront();
                
                
            }
        }
    }

    public void PutCurrentPaperToFront()
    {
        if (!startDrag) return;
        gameManager.topOrder += 2;

        spriteRenderer.sortingOrder = gameManager.topOrder;
        // Setting order for shadow
        shadow.GetComponent<SpriteRenderer>().sortingOrder = gameManager.topOrder-1;
    }

    public bool IsMouseDown()
    {
        return Input.GetMouseButton(0);
    }

    IEnumerator LerpDown(Transform obj, float distance, float duration)
    {
        Vector3 startPos = obj.position;
        Vector3 targetPos = startPos + Vector3.down * distance; // Move down

        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            obj.position = Vector3.Lerp(startPos, targetPos, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        obj.position = targetPos; // Ensure it snaps to the final position
    }

}