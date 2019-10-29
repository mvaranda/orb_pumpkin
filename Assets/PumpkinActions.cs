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

public class PumpkinActions : MonoBehaviour
{
    Light light;
    Renderer renderer;
    Manager M;
    float x_angle = 0;
    float y_angle = 0;
    float delta_rotation_speed;
    float rotation_speed_level_2 = 150f;
    float rotation_speed_level_3 = 250f;
    float y_offset = 0.75666f;
    bool motionEnabled = true;

    // Start is called before the first frame update
    void Start()
    {
        M = FindObjectOfType<Manager>();

        Light[] lights = GetComponentsInChildren<Light>();
        //Debug.Log("Pumpkin lights len = " + lights.Length.ToString());
        light = lights[0];

        Renderer[] renderes = GetComponentsInChildren<Renderer>();
        //Debug.Log("Pumpkin renders len = " + renderes.Length.ToString());
        foreach (Renderer r in renderes) {
            if (r.name == "fire")
            {
                renderer = r;
                break;
            }
        }

        if (light == null || renderer == null)
        {
            //Debug.LogError("Could not get light and renderer");
            return;
        }

        fireOn(false);

        //print("x = " + (Mathf.Cos(x_angle * Mathf.PI / 180f) * M.MAX_X_DISPLACEMENT).ToString());
        x_angle = Random.Range(0f, 359f);
        delta_rotation_speed = Random.Range(0.8f, 1.2f);
    }

    public void setMotionEnabled(bool b)
    {
        motionEnabled = b;
    }

    // Update is called once per frame
    void Update()
    {
        if (motionEnabled == true)
        {

            if (M.level == 2)
            {
                // move sidewise:
                float x = Mathf.Cos(x_angle * Mathf.PI / 180f) * M.MAX_X_DISPLACEMENT;
                transform.localPosition = new Vector3(x, 0, 0);
                x_angle += (rotation_speed_level_2 * delta_rotation_speed) * Time.deltaTime;
            }

            else if (M.level == 3)
            {
                // move sidewise:
                float x = Mathf.Cos(x_angle * Mathf.PI / 180f) * M.MAX_X_DISPLACEMENT;
                transform.localPosition = new Vector3(x, 0, 0);
                x_angle += (rotation_speed_level_3 * delta_rotation_speed) * Time.deltaTime;
            }

            else if (M.level == 4)
            {
                // move sidewise:
                float x = Mathf.Cos(x_angle * Mathf.PI / 180f) * M.MAX_X_DISPLACEMENT;
                float y = Mathf.Sin(x_angle * Mathf.PI / 180f) * M.MAX_X_DISPLACEMENT;
                transform.localPosition = new Vector3(x, y + y_offset, 0);
                x_angle += (rotation_speed_level_2 * delta_rotation_speed) * Time.deltaTime;
            }

            else if (M.level >= 5)
            {
                // move sidewise:
                float x = Mathf.Cos(x_angle * Mathf.PI / 180f) * M.MAX_X_DISPLACEMENT;
                float y = Mathf.Sin(x_angle * Mathf.PI / 180f) * M.MAX_X_DISPLACEMENT;
                transform.localPosition = new Vector3(x, y + y_offset, 0);
                x_angle += (rotation_speed_level_3 * delta_rotation_speed) * Time.deltaTime;
            }
        }
    }

    public void fireOn( bool b)
    {
        //Component l = GetComponentInChildren(Light);
        //Light l = GetComponent<>().find("Point");
        //Light light = GameObject.Find("Point").GetComponent<Light>();
        //Renderer renderer = GameObject.Find("fire").GetComponent<Renderer>();
        if (b == false)
        {
            //Debug.Log("Fire OFF;");
            light.intensity = 0f;
            renderer.enabled = false;
        }
        else
        {
            //Debug.Log("Fire ON;");
            light.intensity = 1f;
            renderer.enabled = true;
        }
    }

    public void resetPosition()
    {
        x_angle = 0f;
        y_angle = 0f;
        transform.localPosition = new Vector3(0f, 0f, 0f);
    }
}
