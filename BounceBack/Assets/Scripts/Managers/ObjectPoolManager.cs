using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager Instance;
    public List<GameObject> pool;
    public int numObjects;
    public int minNumObj;
    public GameObject pfObject;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        numObjects = minNumObj;
        InitPool();
    }

    public void InitPool()
    {
        // Create our pool
        pool = new List<GameObject>();

        for (int i = 0; i < numObjects; i++)
        {
            // Instantiate an object
            GameObject temp = Instantiate(pfObject);

            // Start Inactive
            temp.SetActive(false);

            // Add it to the pool
            pool.Add(temp);
        }
    }

    public void ResetPool()
    {
        foreach (GameObject obj in pool)
        {
            Destroy(obj.gameObject);
        }
            InitPool();
    }
    
    public void ResetNumObj()
    {
        numObjects = minNumObj;
    }

    public void IncrementPool()
    {
        numObjects += 1;
    }

    public GameObject GetObject(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        // Search for an inactive GameObject
        foreach (GameObject obj in pool)
        {
            if (!obj.activeInHierarchy)
            {
                // Set the values
                obj.name = prefab.name;
                Transform objTransform = obj.GetComponent<Transform>();
                objTransform.position = position;
                objTransform.rotation = rotation;

                // Activate it
                obj.SetActive(true);

                // Return the new object
                return obj;
            }
        }

        // Return null if there are no inactive objects
        return null;
    }
}
