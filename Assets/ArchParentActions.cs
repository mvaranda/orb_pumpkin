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

public class ArchParentActions : MonoBehaviour
{
    Manager M;
    bool shootingEnable = false;
    Vector3 downPos;
    bool mouseBtDown = false;
    float pullForce_f = 0.0f;
    public float turn_f = 0.0f;
    public int pullForce = 0;
    int last_pullForce = 0;
    GameObject arch;
    GameObject arch_arrow;
    Renderer arch_renderer;
    Renderer arch_arrow_renderer;
    GameObject fire;
    Renderer fire_renderer;
    public bool mouseDownFlag = false;
    public bool mouseUpFlag = false;
    ArchActions archActions;
    bool motionEnabled = true;

    // Start is called before the first frame update
    void Start()
    {
        M = FindObjectOfType<Manager>();
        archActions = FindObjectOfType<ArchActions>();
        arch = GameObject.Find("arch/arch_only");
        arch_arrow = GameObject.Find("arch_arrow");
        arch_renderer = arch.GetComponent<Renderer>();
        arch_arrow_renderer = arch_arrow.GetComponent<Renderer>();
        fire = GameObject.Find("arch/Armature/Bone.009/fire");
        fire_renderer = fire.GetComponent<Renderer>();

        downPos = Input.mousePosition;
        showMesh(M.ARCH_AND_ARROW_MESH_ID);
    }

    public void showMesh(int arch_mesh_id)
    {

        if (arch_mesh_id == M.ARCH_AND_ARROW_MESH_ID)
        {
            //show arch_arrow and hide arch
            arch_arrow_renderer.enabled = true;
            fire_renderer.enabled = true;
            arch_renderer.enabled = false;
        }
        else if (arch_mesh_id == M.ARCH_MESH_ID)
        {
            arch_arrow_renderer.enabled = false;
            fire_renderer.enabled = false;
            arch_renderer.enabled = true;
        }
        else
        {
            arch_arrow_renderer.enabled = false;
            fire_renderer.enabled = false;
            arch_renderer.enabled = false;
        }

    }

    public void startShooting()
    {
        shootingEnable = true;
        pullForce_f = 0.0f;
        pullForce = 0;
        //showMesh(M.ARCH_AND_ARROW_MESH_ID);
    }

    // Update is called once per frame
    void Update()
    {
        if (shootingEnable == false)
        {
            mouseDownFlag = false;
            mouseUpFlag = false;
            mouseBtDown = false;

            return;
        }

        Vector2 pos = Input.mousePosition;

        if (mouseDownFlag == true) {
            mouseDownFlag = false;
            mouseBtDown = true;
            Debug.Log("ArchParentActions: DOWN");
            downPos = pos;
            pullForce_f = 0.0f;
            pullForce = 0;
            showMesh(M.ARCH_AND_ARROW_MESH_ID);
        }
        if (mouseUpFlag == true) {
            Debug.Log("ArchParentActions: UP");
            mouseUpFlag = false;
            mouseBtDown = false;

            if (pullForce >= M.PULL_FORCE_TRIGGER)
            {
                // let animation go
                //Debug.Log("UP shooting");
                archActions.updatePullForce(pullForce, true);
            }
            else
            {
                //Debug.Log("UP shooting with PULL_FORCE_TRIGGER");
                pullForce = M.PULL_FORCE_TRIGGER;
                archActions.updatePullForce(pullForce, true);
                //return;
            }
        }

        if (mouseBtDown == true)
        {
            turn_f += (pos.x - downPos.x) * M.TURN_FACTOR;
            if (turn_f > M.MAX_TURN_ANGLE) { turn_f = M.MAX_TURN_ANGLE; }
            if (turn_f < -M.MAX_TURN_ANGLE) { turn_f = -M.MAX_TURN_ANGLE; }

            pullForce_f -= (pos.y - downPos.y) * M.PULL_FACTOR;
            if (pullForce_f > M.MAX_PULL_FORCE) { pullForce_f = M.MAX_PULL_FORCE; }
            if (pullForce_f < 0) { pullForce_f = 0; }

            pullForce = (int) pullForce_f;
            if (last_pullForce != pullForce)
            {
                ////Debug.Log("pullForce = " + pullForce.ToString());
                last_pullForce = pullForce;
            }

            archActions.updatePullForce(pullForce, false);

        }
        downPos = pos;

        //------------ turn the Arch according to turn_f -------------
        transform.rotation = Quaternion.Euler(0,turn_f, 0);

    } // Update end

    public void arrowReleased()
    {
        FindObjectOfType<ArrowActions>().startArrowLaunch(turn_f, pullForce);
        showMesh(M.ARCH_MESH_ID);
        shootingEnable = false;
        M.playSound(M.SOUND_SHOOT);

    }

    public void setTurn(float turn)
    {
        turn_f = turn;
        transform.rotation = Quaternion.Euler(0, turn_f, 0);
    }
    public void setMotionEnabled(bool b)
    {
        motionEnabled = b;
    }
}
