using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : MonoBehaviour
{
    public GameObject idol;
    public GameObject[] spawns;


    private void Start()
    {
        int spawnIndex = Random.Range(0, spawns.Length - 1);
        idol.transform.SetPositionAndRotation(spawns[spawnIndex].transform.position, spawns[spawnIndex].transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
