using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy Wave Config")]
public class WaveConfig : ScriptableObject
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] GameObject pathPrefab;
    [SerializeField] float timeBetweenSpawns = 0.5f;
    [SerializeField] float spawnDelayRandomFactor = 0.3f;
    [SerializeField] int numOfEnemies = 5;
    [SerializeField] float moveSpeed = 2f;

    public GameObject GetEnemyPrefab() { return enemyPrefab; }
    public List<Transform> GetWaypoints() {
        var WaveWayPoints = new List<Transform>();
        foreach (Transform waypoint in pathPrefab.transform)
        {
            WaveWayPoints.Add(waypoint);
        }
        return WaveWayPoints;
    }
    public float GetTimeBetweenSpawns() { return timeBetweenSpawns; }
    public float GetSpawnDelayFactor() { return spawnDelayRandomFactor; }
    public int GetNumofEnemies() { return numOfEnemies; }
    public float GetMoveSpeed() { return moveSpeed; }
}
