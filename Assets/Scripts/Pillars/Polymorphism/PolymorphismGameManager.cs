using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class PolymorphismGameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI hordeText;
    [SerializeField] private TextMeshProUGUI lifeText;
    [SerializeField] private GameObject winCanvas;
    [SerializeField] private GameObject loseCanvas;
    [SerializeField] private Transform startPosition;
    [SerializeField] private GameObject player;
    public GameObject enemyType1Prefab;
    public GameObject enemyType2Prefab;
    public Transform[] spawnPoints;
    public Transform target;
    public int enemiesPerHorde = 20;
    public int totalHordes = 3;
    public int playerLives = 5;
    private int currentHorde = 0;
    private int enemiesDestroyed = 0;
    private bool spawning = false;
    public void StartGame()
    {
        player.transform.position = startPosition.position;
        if (!spawning)
        {
            StartCoroutine(SpawnHorde());
        }
    }
    IEnumerator SpawnHorde()
    {
        spawning = true;
        while (currentHorde < totalHordes && playerLives > 0)
        {
            enemiesDestroyed = 0;
            for (int i = 0; i < enemiesPerHorde; i++)
            {
                SpawnEnemy();
                yield return new WaitForSeconds(1.5f);
            }
            while (enemiesDestroyed < enemiesPerHorde && playerLives > 0)
            {
                yield return null;
            }
            currentHorde++;
            UpdateUIHealth();
            enemiesDestroyed = 0;
        }
        if (playerLives <= 0 && enemiesDestroyed == enemiesPerHorde)
        {
            loseCanvas.SetActive(true);
        }
        else
        {
            StartCoroutine(nameof(CheckGameCompleted));
            winCanvas.SetActive(true);
        }
    }
    void SpawnEnemy()
    {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject enemyPrefab = (Random.value > 0.5f) ? enemyType1Prefab : enemyType2Prefab;
        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        PolymorphismEnemy enemyAI = enemy.GetComponent<PolymorphismEnemy>();
        enemyAI.target = target;
        enemyAI.spawner = this;
    }
    public void EnemyReachedTarget()
    {
        playerLives--;
        if (playerLives < 0)
        {
            playerLives = 0;
        }
        UpdateUIHealth();
        if (playerLives <= 0)
        {
            StopAllCoroutines();
            loseCanvas.SetActive(true);
        }
    }
    public void EnemyDestroyed()
    {
        enemiesDestroyed++;
    }
    void UpdateUIHealth()
    {
        hordeText.text = $"Horda: {currentHorde+1} / {totalHordes}";
        lifeText.text = $"Vidas: {playerLives}";
    }

    private IEnumerator CheckGameCompleted()
    {
        var json = $"{{\"polymorphismGame\":\"True\"}}";
        using UnityWebRequest request = UnityWebRequest.Put($"{Constants.BASE_URI}progress/{User.UserId}", json);
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();
    }
}
