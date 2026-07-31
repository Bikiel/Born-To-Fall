using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject gemaPrefab;
    [SerializeField] private GameObject trampaPrefab;

    [Header("Probabilidades Spawn (%)")]
    [Range(0f, 100f)] public float gemaSpawnChance = 90f;
    [Range(0f, 100f)] public float trampaSpawnChance = 10f;

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

        // Iniciamos un único bucle global de generación
        StartCoroutine(SpawnLoop());
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

    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            float interval = Random.Range(minSpawnInterval, maxSpawnInterval);
            yield return new WaitForSeconds(interval);

            if (TryGetFairSpawnPosition(out Vector3 spawnPosition))
            {
                // Decidimos cuál prefab instanciar en base al porcentaje
                GameObject prefabToSpawn = SelectPrefabByWeight();

                if (prefabToSpawn != null)
                {
                    GameObject spawnedObject = Instantiate(prefabToSpawn, spawnParent);
                    spawnedObject.transform.SetPositionAndRotation(spawnPosition, Quaternion.identity);
                    spawnedObjects.Add(spawnedObject.transform);
                }
            }
        }
    }

    private GameObject SelectPrefabByWeight()
    {
        float totalWeight = gemaSpawnChance + trampaSpawnChance;
        if (totalWeight <= 0f) return gemaPrefab; // Fallback por seguridad

        float randomValue = Random.Range(0f, totalWeight);

        // Si la tirada cae dentro de la probabilidad de la gema, saca la gema; de lo contrario, la trampa
        if (randomValue < gemaSpawnChance)
        {
            return gemaPrefab;
        }
        else
        {
            return trampaPrefab;
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
