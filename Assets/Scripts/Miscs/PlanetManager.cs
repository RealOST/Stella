using System.Collections.Generic;
using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetManager : MonoBehaviour
{
    [SerializeField] private GameObject sun;
    [SerializeField] private GameObject mercury;
    [SerializeField] private GameObject venus;
    [SerializeField] private GameObject earth;
    [SerializeField] private GameObject mars;
    [SerializeField] private GameObject jupiter;
    [SerializeField] private GameObject saturn;
    [SerializeField] private GameObject uranus;
    [SerializeField] private GameObject neptune;

    private float interval = 8f;

    private List<GameObject> allPlanets;
    private Queue<GameObject> planetQueue;

    private void Awake()
    {
        allPlanets = new List<GameObject> { sun, mercury, venus, earth, mars, jupiter, saturn, uranus, neptune };
        ShuffleAndQueuePlanets();
    }

    private void Start()
    {
        StartCoroutine(ShuffledPlanetLoop());
    }

    private IEnumerator ShuffledPlanetLoop()
    {
        while (true)
        {
            if (planetQueue.Count == 0)
            {
                ShuffleAndQueuePlanets();
            }

            GameObject currentPlanet = planetQueue.Dequeue();

            PoolManager.Release(currentPlanet);

            // currentPlanet.SetActive(true);
            Debug.Log("Activated planet: " + currentPlanet.name);

            yield return new WaitForSeconds(interval);
            interval = Random.Range(6f,8f);
        }
    }

    private void ShuffleAndQueuePlanets()
    {
        List<GameObject> shuffled = new List<GameObject>(allPlanets);
        for (int i = 0; i < shuffled.Count; i++)
        {
            GameObject temp = shuffled[i];
            int randomIndex = Random.Range(i, shuffled.Count);
            shuffled[i] = shuffled[randomIndex];
            shuffled[randomIndex] = temp;
        }

        planetQueue = new Queue<GameObject>(shuffled);
        Debug.Log("New planet queue shuffled!");
    }
}

