using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float speed = 15f;
    public bool isStealthed;
    public Material stealthMaterial;
    public Material normalMaterial;

    public GameObject lightDetector;

    // Start is called before the first frame update
    void Start()
    {
        isStealthed = false;
    }

    // Update is called once per frame
    void Update()
    {
        /*if(!isStealthed && (lightDetector.GetComponent<DetectLight>().lightLevel <= 1))
        {
            isStealthed = true;
            gameObject.GetComponent<Renderer>().material = stealthMaterial;
        }
        else if(isStealthed && (lightDetector.GetComponent<DetectLight>().lightLevel >= 2))
        {
            isStealthed = false;
            gameObject.GetComponent<Renderer>().material = normalMaterial;
        }*/
        if(!isStealthed && (lightDetector.GetComponent<DetectLight>().lightLevel <= 1))
        {
            isStealthed = true;
            gameObject.GetComponent<Renderer>().material.Lerp(gameObject.GetComponent<Renderer>().material, stealthMaterial,3);
        }
        else if(isStealthed && (lightDetector.GetComponent<DetectLight>().lightLevel >= 2))
        {
            isStealthed = false;
            gameObject.GetComponent<Renderer>().material.Lerp(gameObject.GetComponent<Renderer>().material, normalMaterial, 3);
        }
    }
}
