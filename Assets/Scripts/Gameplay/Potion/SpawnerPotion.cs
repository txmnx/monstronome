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
    public Transform[] freeModePrefabs = new Transform[2];
    
    public SoundEngineTuner soundEngineTuner;
    public ArticulationManager articulationManager;
    public ReframingManager reframingManager;

    [Header("Spawn")]
    public float timeBetweenSpawn = 2.0f;
    public Vector3 windForce;
    public AK.Wwise.Event SFXOnSpawn;
    
    
    private Queue<(Transform, SoundEngineTuner.PotionType)> m_SpawnQueue;
    private bool m_IsSpawning = false;
    

    private void Awake()
    {
        m_SpawnQueue = new Queue<(Transform, SoundEngineTuner.PotionType)>();
        windForce = transform.TransformDirection(windForce);
    }

    public void SpawnPotion(InstrumentFamily.ArticulationType type)
    {
        Spawn(articulationPrefabs[(int)type], SoundEngineTuner.PotionType.Articulation);
    }
    
    public void SpawnPotion(ReframingPotion.ReframingPotionType type)
    {
        Spawn(reframingPrefabs[(int)type], SoundEngineTuner.PotionType.Reframing);
    }
    
    public void SpawnPotion(FreeModeManager.FreeModePotion type)
    {
        Spawn(freeModePrefabs[(int)type], SoundEngineTuner.PotionType.Solo);
    }

    private void Spawn(Transform prefab, SoundEngineTuner.PotionType type)
    {
        m_SpawnQueue.Enqueue((prefab, type));
        if (!m_IsSpawning) {
            StartCoroutine(DelaySpawn());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Potion")) {
            other.GetComponent<BreakableObject>().ApplyWind(windForce);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Potion")) {
            other.GetComponent<BreakableObject>().DisableWind();
        }
    }

    private IEnumerator DelaySpawn()
    {
        m_IsSpawning = true;
        yield return new WaitForSeconds(timeBetweenSpawn);

        (Transform, SoundEngineTuner.PotionType) prefab = m_SpawnQueue.Dequeue();
        
        Transform potion = Instantiate(prefab.Item1, spawnerTransform.position, spawnerTransform.rotation);
        if (prefab.Item2 == SoundEngineTuner.PotionType.Articulation) {
            ArticulationPotion articulation = potion.GetComponent<ArticulationPotion>();
            articulation.articulationManager = articulationManager;
            articulation.spawnerPotion = this;
            articulation.OnSpawn();
        }
        else if (prefab.Item2 == SoundEngineTuner.PotionType.Reframing) {
            ReframingPotion reframing = potion.GetComponent<ReframingPotion>();
            reframing.reframingManager = reframingManager;
            reframing.spawnerPotion = this;
            reframing.OnSpawn();
        }
        else if (prefab.Item2 == SoundEngineTuner.PotionType.Solo) {
            SoloPotion solo = potion.GetComponent<SoloPotion>();
            solo.soundEngineTuner = soundEngineTuner;
            solo.spawnerPotion = this;
            solo.OnSpawn();
        }

        SoundEngineTuner.SetSwitchPotionType(prefab.Item2, gameObject);
        SFXOnSpawn.Post(gameObject);

        if (m_SpawnQueue.Count > 0) {
            StartCoroutine(DelaySpawn());
        }
        else {
            m_IsSpawning = false;
        }
    }
}
