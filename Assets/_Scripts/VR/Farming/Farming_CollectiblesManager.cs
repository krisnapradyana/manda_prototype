using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//attach this script to game manager
//it is intended to instantiate and manage prefabs / variety of resource
public class Farming_CollectiblesManager : MonoBehaviour
{
    //public itemType valueableType;
    public string prefabName;     // Name of the prefab
    public GameObject prefab;     // The prefab itself
    public int numberOfCopies;    // Number of copies to instantiate

    public int availablePrefabs;
    [HideInInspector] public GameObject[] instantiatedPrefabs;

    // List to store multiple prefab data entries
    private int instantiatedCounter;

    void Start()
    {
        InstantiatePrefabs();
    }

    void InstantiatePrefabs()
    {
        instantiatedPrefabs = new GameObject[numberOfCopies];

        int instantiateCounter = 0;
        for (int i = 0; i < numberOfCopies; i++)
        {
            GameObject newPrefab = Instantiate(prefab);
            newPrefab.transform.parent = gameObject.transform;

            newPrefab.name = $"{prefabName}_{i}";
            instantiatedPrefabs[i] = newPrefab;

            instantiateCounter++;
        }

        if (instantiateCounter == numberOfCopies)
        {
            Debug.Log($"Success: {prefabName} instantiated {instantiateCounter}x");
        }
        else
        {
            Debug.LogWarning($"Error: {prefabName} instantiated {instantiateCounter}x");
        }

        availablePrefabs = instantiatedCounter;
    }

    public void CheckPrefabsAvailability()
    {
        int availableCount = 0;

        foreach (GameObject prefab in instantiatedPrefabs)
        {
            if (!prefab.activeSelf)
            {
                availableCount++;
            }
        }

        availablePrefabs = availableCount;
    }

    public void FetchFromPool(int numberNeeded, GameObject placeToSpawn)
    {
        //Debug.Log($"Number Needed: {numberNeeded}, Availability: {availablePrefabs}");
        if (numberNeeded > availablePrefabs)
        {
            numberNeeded = availablePrefabs;
        }

        int fetched = 0;
        foreach (GameObject prefab in instantiatedPrefabs)
        {
            if (!prefab.activeSelf)
            {
                fetched++;
                prefab.transform.position = placeToSpawn.transform.position;
                prefab.SetActive(true);

                if (fetched >= numberNeeded)
                {
                    break; // Stop once we've fetched the required number
                }
            }
        }

        availablePrefabs -= fetched;
        //Debug.Log($"Successfully fetched: {fetched}");
    }

    public void ReturnToPool(GameObject prefab)
    {
        prefab.SetActive(false);

        availablePrefabs++;
    }
}
