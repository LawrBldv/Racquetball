using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] planetPrefabs; // Array to hold the 8 prefabs
    [SerializeField] private float[] spawnWeights; // Array to hold the weights for each prefab
    [SerializeField] private float spawnInterval = 2f; // Time interval between spawns

    private MeshCollider meshCollider; // Reference to the mesh collider of the plane

    private void Start()
    {
        // Get the MeshCollider component attached to this GameObject
        meshCollider = GetComponent<MeshCollider>();

        // Check if planetPrefabs array and spawnWeights array are set and have the same length
        if (planetPrefabs == null || planetPrefabs.Length == 0 || spawnWeights == null || spawnWeights.Length != planetPrefabs.Length)
        {
            Debug.LogError("Planet prefabs and spawn weights must be assigned and have the same length!");
            return;
        }

        // Start spawning planets at regular intervals
        InvokeRepeating(nameof(SpawnPlanet), spawnInterval, spawnInterval);
    }

    private void SpawnPlanet()
    {
        // Select a random prefab based on the weighted probability
        GameObject randomPrefab = SelectRandomPrefab();

        // Get a random position within the bounds of the mesh collider
        Vector3 spawnPosition = GetRandomPositionWithinBounds();

        // Instantiate the selected prefab at the random position
        Instantiate(randomPrefab, spawnPosition, Quaternion.identity);
    }

    private GameObject SelectRandomPrefab()
    {
        // Calculate the total weight
        float totalWeight = 0f;
        foreach (float weight in spawnWeights)
        {
            totalWeight += weight;
        }

        // Generate a random value between 0 and the total weight
        float randomValue = Random.Range(0, totalWeight);

        // Select the prefab based on the random value
        for (int i = 0; i < planetPrefabs.Length; i++)
        {
            if (randomValue < spawnWeights[i])
            {
                return planetPrefabs[i];
            }
            randomValue -= spawnWeights[i];
        }

        // Fallback to the last prefab (should not happen in normal conditions)
        return planetPrefabs[planetPrefabs.Length - 1];
    }

    private Vector3 GetRandomPositionWithinBounds()
    {
        // Get the bounds of the mesh collider
        Bounds bounds = meshCollider.bounds;

        // Generate random X and Z positions within the bounds
        float randomX = Random.Range(bounds.min.x, bounds.max.x);
        float randomZ = Random.Range(bounds.min.z, bounds.max.z);

        // Return the random position, keeping Y position at the height of the plane
        return new Vector3(randomX, bounds.min.y, randomZ);
    }
}
