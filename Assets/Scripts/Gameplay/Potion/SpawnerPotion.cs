using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Utility to spawn potion at a position
 */
public class SpawnerPotion : MonoBehaviour
{
    [Header("Anchors")]
    public Transform spawnerTransform;
    
    [Header("Prefabs")]
    public Transform[] articulationPrefabs = new Transform[3];
    public Transform[] reframingPrefabs = new Transform[6];

    [Header("Spawn")]
    public float timeBetweenSpawn = 2.0f;

    private Queue<Transform> m_SpawnQueue;
    private bool m_IsSpawning = false;

    private void Awake()
    {
        m_SpawnQueue = new Queue<Transform>();
    }

    public void SpawnPotion(InstrumentFamily.ArticulationType type)
    {
        Spawn(articulationPrefabs[(int)type]);
    }
    
    public void SpawnPotion(ReframingPotion.ReframingPotionType type)
    {
        Spawn(reframingPrefabs[(int)type]);
    }

    private void Spawn(Transform prefab)
    {
        if (m_IsSpawning) {
            m_SpawnQueue.Enqueue(prefab);
        }
        else {
            StartCoroutine(DelaySpawn());
        }
    }
    
    private IEnumerator DelaySpawn()
    {
        m_IsSpawning = true;
        yield return new WaitForSeconds(timeBetweenSpawn);
        Instantiate(m_SpawnQueue.Dequeue(), spawnerTransform.position, spawnerTransform.rotation);

        if (m_SpawnQueue.Count > 0) {
            StartCoroutine(DelaySpawn());
        }
        else {
            m_IsSpawning = false;
        }
    }
}
