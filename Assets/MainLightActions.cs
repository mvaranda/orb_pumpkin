/******************************************
 *
 *             ORB Pumpkin Game
 *
 * By: Marcelo Varanda
 * Copyrights 2019 - All rights reserved
 *
 * License:
 * The game usage, code, and art materials are licensed under
 * Creative Commons Attribution-NonCommercial 3.0
 * Unported (CC BY-NC 3.0):
 *
 * https://creativecommons.org/licenses/by-nc/3.0/
 *
 * Some graphics are inspired by ORBCOMM logos and
 * ORBCOMM product packaging. Those are
 *   "Copyright 2017 ORBCOMM Inc. All rights reserved".
 *
 *
 ******************************************
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainLightActions : MonoBehaviour
{
    float lowlight = 0.4f;
    float minIntensity = 0.57f;
    float maxIntensity = 0.63f;
    public float lightIntensity = 0.5f;
    float t;
    Light light;
    bool flickerEnable = true;

    //float random;

    void Start()
    {
        //random = Random.Range(0.0f, 65535.0f);
        //random = Random.Range(minIntensity, maxIntensity);
        t = Time.time;
        light = GetComponent<Light>();
    }

    public void enableFlicker(bool b)
    {
        flickerEnable = b;
        if (b == false)
        {
            light.intensity = lowlight;
        }

    }

    void Update()
    {
        //float noise = Mathf.PerlinNoise(random, Time.time);
        //GetComponent<Light>().intensity = Mathf.Lerp(minIntensity, maxIntensity, noise);

        if (flickerEnable == false) return;
        float now = Time.time;
        if ((now - t) > 0.100f)
        {
            lightIntensity = Random.Range(minIntensity, maxIntensity); ;
            //GetComponent<Light>().intensity = lightIntensity;
            light.intensity = lightIntensity;
            ////Debug.Log("Chenge light to " + lightIntensity.ToString() );
            t = now;
        }
        
    }
}
