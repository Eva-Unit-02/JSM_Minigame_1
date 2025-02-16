using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public int topOrder = 5;
    public int[] distances;
    public GameObject[] papers;
    public float moveSpeed;
    public int reputation = 100;
    public TextMeshProUGUI reputationDisplay;
    public int money = 100;
    public TextMeshProUGUI moneyDisplay;
    private float timer = 0f;
    public GameObject gameOverScreen;
    private float initialSpawnInterval = 2.0f;
    private float spawnInterval = 1.5f;
    void Update()
    {
        timer += Time.deltaTime;
        
        if (timer >= spawnInterval)
        {
            timer = 0;

            // spawnInterval = initialSpawnInterval / Mathf.Pow(Mathf.Log(topOrder, 2), 2);
            if (spawnInterval >= 1f) spawnInterval -= 0.05f;

            Vector2 spawnPosition = GetRandomPositionOnScreen();
            GameObject newPrefab = Instantiate(papers[Random.Range(0, papers.Length)], spawnPosition + new Vector2(0, Camera.main.orthographicSize * 2), Quaternion.identity);
            newPrefab.GetComponent<SpriteRenderer>().sortingOrder = topOrder;
            topOrder++;
            StartCoroutine(MovePrefab(newPrefab, spawnPosition));
        }

    }

    

    Vector2 GetRandomPositionOnScreen()
    {
        // Get a random position within the screen bounds
        float randomX = Random.Range(0.3f, 0.7f); // Random X within the screen (10% margin)
        float randomY = Random.Range(0.1f, 0.4f); // Random Y within the screen (10% margin)

        // Convert the random viewport position to world coordinates
        return Camera.main.ViewportToWorldPoint(new Vector2(randomX, randomY));
    }

    void SpawnPrefab()
    {

    }

    IEnumerator MovePrefab(GameObject prefab, Vector2 targetPosition)
    {
        Draggable draggable = prefab.GetComponent<Draggable>();
        // Move the prefab until it reaches the target position
        while ((Vector2)prefab.transform.position != targetPosition)
        {
            if (draggable.startDrag) yield break;
            // check if below screen
            // if (Camera.main.WorldToViewportPoint(prefab.transform.position).y < -0.1)
            // {
            //     ChangeReputation(-5);
            //     Destroy(prefab);
            //     yield break;
            // }
            // Move the prefab toward the target position
            prefab.transform.position = Vector2.MoveTowards(prefab.transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // Wait for the next frame
            yield return null;
        }

        // Optionally, destroy the prefab when it reaches the target position
        // Destroy(prefab);
    }

    public void ChangeMoney(int amt)
    {
        money += amt;
        moneyDisplay.text = "Money: " + money;

    }

    public void ChangeReputation(int amt)
    {
        reputation += amt;
        reputationDisplay.text = "Reputation: " + reputation; 
        if (reputation < 0)
        {
            gameOverScreen.SetActive(true);
        }
    }
}
