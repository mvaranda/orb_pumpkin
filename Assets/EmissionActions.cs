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

public class EmissionActions : MonoBehaviour
{
    Material[] material;
    float lastTime;
    int idx = 0;
    int NUM_TEXT = 4;

    // Start is called before the first frame update
    void Start()
    {
        material = new Material[NUM_TEXT];
        material[0] = (Material)Resources.Load("EmissionMat1");
        material[1] = (Material)Resources.Load("EmissionMat2");
        material[2] = (Material)Resources.Load("EmissionMat3");
        material[3] = (Material)Resources.Load("EmissionMat4");


        if (material[0] != null && material[1] != null && material[2] != null)
        {
            //Debug.Log("****** All emission material loaded ******");
        }
        else
        {
            //Debug.Log("ERROR Loading emission materials");
        }
        lastTime = Time.time;


    }

    private int getNext()
    {
        int r = idx, i;

        for (i = 0; i < 5; i++) // protect against looping too long here
        {
            r = Random.Range(0, 3);
            if (r != idx) break;
        }

        return r;
    }

    // Update is called once per frame
    void Update()
    {

        //material.mainTexture = material[0];
        float now = Time.time;
        if ((now - lastTime) > 0.80)
        {
            Renderer r = GetComponent<Renderer>();
            r.material = material[getNext()];
            lastTime = now;
        }
        ////Debug.Break;
        //GetComponent<Renderer>().material.mainTexture = material[0];
    }
}
