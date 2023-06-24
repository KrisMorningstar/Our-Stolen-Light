using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectLight : MonoBehaviour
{
    public Camera lightCamera;

    private Rect lightRect;
    private Texture2D lightTexture;
    private RenderTexture targetTexture;

    private int textureDimensions = 1;
    private float delay = 0.2f;

    private Color lightColour;
    [HideInInspector]
    public float lightLevel;


    // Start is called before the first frame update
    void Start()
    {
        
        LightLevelInstantiator();
    }

    //Instantiates texture variables and starts 
    void LightLevelInstantiator()
    {
        lightTexture = new Texture2D(textureDimensions, textureDimensions, TextureFormat.RGB24, false);
        targetTexture = new RenderTexture(textureDimensions, textureDimensions, 24);
        lightRect = new Rect(0f, 0f, textureDimensions, textureDimensions);

        StartCoroutine(LightLevelDetector(delay));
    }

    IEnumerator LightLevelDetector(float _delay)
    {
        while (true)
        {
            lightCamera.targetTexture = targetTexture;
            lightCamera.Render();
            RenderTexture.active = targetTexture;
            lightTexture.ReadPixels(lightRect, 0, 0);

            RenderTexture.active = null;
            lightCamera.targetTexture = null;

            lightColour = lightTexture.GetPixel(textureDimensions / 2, textureDimensions / 2);
            lightLevel = Mathf.Round((lightColour.r + lightColour.g + lightColour.b) / 3 * 10);
            //Debug.Log(lightLevel);

            yield return new WaitForSeconds(_delay);
        }
    }
}
