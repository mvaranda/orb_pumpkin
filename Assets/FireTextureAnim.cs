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

public class FireTextureAnim : MonoBehaviour
{
    Texture2D[] textures;
    float lastTime;
    int idx = 0;
    int NUM_TEXT = 5;

    // Start is called before the first frame update
    void Start()
    {
        textures = new Texture2D[NUM_TEXT];
        textures[0] = (Texture2D)Resources.Load("fire_1");
        textures[1] = (Texture2D)Resources.Load("fire_2");
        textures[2] = (Texture2D)Resources.Load("fire_3");
        textures[3] = (Texture2D)Resources.Load("fire_4");
        textures[4] = (Texture2D)Resources.Load("fire_5");

        if (textures[0] != null && textures[1] != null && textures[2] != null)
        {
            //Debug.Log("****** All Fire textures loaded ******");
        }
        else
        {
            //Debug.Log("ERROR Loading Osc textures");
        }
        lastTime = Time.time;


    }

    private int getNext()
    {
        int r=idx, i;

        for (i=0; i<5; i++) // protect against looping too long here
        {
            r = Random.Range(0, 4);
            if (r != idx) break;
        }

        return r;
    }

    // Update is called once per frame
    void Update()
    {

        //material.mainTexture = textures[0];
        float now = Time.time;
        if ((now - lastTime) > 0.100)
        {
            Renderer r = GetComponent<Renderer>();
            r.material.mainTexture = textures[getNext()];
            lastTime = now;
        }
        ////Debug.Break;
        //GetComponent<Renderer>().material.mainTexture = textures[0];
    }

}
