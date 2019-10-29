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

public class ArrowActions : MonoBehaviour
{
    Manager _M;
    float speed = 15.0f; // speed_y: 0.5 ~ 4.0, gravity -20
                         // speed_y: 0 ~ 3.0, gravity -10

    public float speed_y = 7.614f * 0.3f;
    public float hight = 30f;

    // Start is called before the first frame update
    void Start()
    {
        _M = FindObjectOfType<Manager>();
        Transform t = GetComponent<Transform>();
        t.position = new Vector3(0, -0.5f, 14);
        Physics.gravity = new Vector3(0, -10.0f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //
    // https://answers.unity.com/questions/449741/trying-to-fire-an-arrow.html
    //

    public void startArrowLaunch(float y, int pullForce)
    {
        //Debug.Log("startArrowLaunch: Got event y = " + y.ToString());
        showArrow(true);
         speed_y = 3.0f * (float)(pullForce - _M.PULL_FORCE_TRIGGER) / (float)(_M.MAX_PULL_FORCE - _M.PULL_FORCE_TRIGGER);
        Transform t = GetComponent<Transform>();
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        t.position = new Vector3(0, -0.5f, 14);
        t.rotation = Quaternion.Euler(-90.0f, y, 0);
        //GetComponent<Rigidbody>().AddForce(new Vector3(0, 0, -1), 1.0f);//  , t.forward * 1);
        float rad = (y + 90.0f) * (Mathf.PI) / 180.0f;
        float x = Mathf.Cos(rad) * speed;
        float z = Mathf.Sin(rad) * speed;
        //rb.velocity = new Vector3(0, 0, -10);
        //rb.velocity = new Vector3(x, speed_y, -z);
        rb.AddForce(new Vector3(x, speed_y, -z) * 200f);
        rb.transform.rotation.SetLookRotation(t.position + rb.velocity);

    }

    void OnCollisionEnter(Collision collision)
    {
        string obj_name = collision.gameObject.name;
        //print("detected colligion: " + obj_name);
        _M.colligionDetected(collision);
    }

    public void showArrow(bool show)
    {
        GetComponentInChildren<Renderer>().enabled = show;
    }

    //void FixedUpdate()
    //{
    //    Transform t = GetComponent<Transform>();
    //    Rigidbody rb = GetComponent<Rigidbody>();
    //    t.LookAt(t.position + rb.velocity);
    //}
}
