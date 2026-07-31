using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject gemaPrefab;
    [SerializeField] private GameObject trampaPrefab;

    [Header("Referencia")]
    [SerializeField] private Transform player;
    [SerializeField] private Transform spawnParent;

    [Header("Intervalo de spawn")]
    [SerializeField] private float minSpawnInterval = 1f;
    [SerializeField] private float maxSpawnInterval = 4f;

    [Header("Posiciones justas")]
    [SerializeField] private float horizontalSpawnLimit = 7.5f;
    [SerializeField] private float minDistanceBelowPlayer = 7f;
    [SerializeField] private float maxDistanceBelowPlayer = 13f;
    [SerializeField] private float minVerticalSpacing = 3f;
    [SerializeField] private int maxPositionAttempts = 12;

    [Header("Limpieza")]
    [SerializeField] private float destroyDistanceAbovePlayer = 15f;

    private readonly List<Transform> spawnedObjects = new();

    private void Start()
    {
        if (player == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            player = playerObject != null ? playerObject.transform : null;
        }

        if (player == null)
        {
            Debug.LogError("SpawnManager necesita una referencia al Player.", this);
            enabled = false;
            return;
        }

        if (spawnParent == null)
        {
            GameObject fondoTotal = GameObject.Find("FondoTotal");
            spawnParent = fondoTotal != null ? fondoTotal.transform : null;
        }

        if (spawnParent == null)
        {
            Debug.LogError("SpawnManager necesita una referencia al objeto FondoTotal.", this);
            enabled = false;
            return;
        }

        StartSpawnLoop(gemaPrefab);
        StartSpawnLoop(trampaPrefab);
    }

    private void Update()
    {
        for (int i = spawnedObjects.Count - 1; i >= 0; i--)
        {
            Transform spawnedObject = spawnedObjects[i];
            if (spawnedObject == null)
            {
                spawnedObjects.RemoveAt(i);
                continue;
            }

            if (spawnedObject.position.y > player.position.y + destroyDistanceAbovePlayer)
            {
                Destroy(spawnedObject.gameObject);
                spawnedObjects.RemoveAt(i);
            }
        }
    }

    private void StartSpawnLoop(GameObject prefab)
    {
        if (prefab == null)
        {
            Debug.LogWarning("Falta asignar uno de los prefabs del SpawnManager.", this);
            return;
        }

        StartCoroutine(SpawnLoop(prefab));
    }

    private IEnumerator SpawnLoop(GameObject prefab)
    {
        while (true)
        {
            float interval = Random.Range(minSpawnInterval, maxSpawnInterval);
            yield return new WaitForSeconds(interval);

            if (TryGetFairSpawnPosition(out Vector3 spawnPosition))
            {
                GameObject spawnedObject = Instantiate(prefab, spawnParent);
                spawnedObject.transform.SetPositionAndRotation(spawnPosition, Quaternion.identity);
                spawnedObjects.Add(spawnedObject.transform);
            }
        }
    }

    private bool TryGetFairSpawnPosition(out Vector3 spawnPosition)
    {
        for (int attempt = 0; attempt < maxPositionAttempts; attempt++)
        {
            float x = Random.Range(-horizontalSpawnLimit, horizontalSpawnLimit);
            float y = player.position.y - Random.Range(minDistanceBelowPlayer, maxDistanceBelowPlayer);
            spawnPosition = new Vector3(x, y, -1f);

            if (HasEnoughVerticalSpace(spawnPosition.y))
            {
                return true;
            }
        }

        spawnPosition = default;
        return false;
    }

    private bool HasEnoughVerticalSpace(float candidateY)
    {
        foreach (Transform spawnedObject in spawnedObjects)
        {
            if (spawnedObject != null && Mathf.Abs(candidateY - spawnedObject.position.y) < minVerticalSpacing)
            {
                return false;
            }
        }

        return true;
    }
}
