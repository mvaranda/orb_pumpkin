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

public class ArchActions : MonoBehaviour
{
    Manager _M;
    private int pullForce = 0;
    bool release = false;
    public float playTime = 0;
    public int anim_state = 0;

    // Start is called before the first frame update
    void Start()
    {
        _M = FindObjectOfType<Manager>();

        //AnimationClip clip;
        //Animator anim;

        //anim = GetComponent<Animator>();
        //anim.Play("ArchAnimation", 0, 0f);
        //clip = anim.runtimeAnimatorController.animationClips[0];

        //// new event created
        //AnimationEvent evt, evt2, evt3;
        //evt = new AnimationEvent();
        //evt2 = new AnimationEvent();
        //evt3 = new AnimationEvent();

        //evt.intParameter = _M.ARROW_RELEASE_START;
        //evt.time = (float)_M.ARROW_RELEASE_START / (float)_M.ANIM_END;
        //evt.functionName = "animEventHandler";
        //clip.AddEvent(evt);

        //evt2.intParameter = _M.ARROW_FREE;
        //evt2.time = (float)_M.ARROW_FREE / (float) _M.ANIM_END;
        //evt2.functionName = "animEventHandler";
        //clip.AddEvent(evt2);

        //evt3.intParameter = _M.ANIM_END;
        //evt3.time = 1.0f;
        //evt3.functionName = "animEventHandler";
        //clip.AddEvent(evt3);

    }

    public void processAnimation()
    {
        Animator anim = GetComponent<Animator>();
        float f = anim.GetCurrentAnimatorStateInfo(0).normalizedTime;
        playTime = f;
        if ( anim_state == 0)
        {
            if (f >= (float)_M.ARROW_RELEASE_START / (float)_M.ANIM_END ) {
                anim_state = 1;
                animEventHandler(_M.ARROW_RELEASE_START);
                return;
            }
        }
        if (anim_state == 1)
        {
            if (f >= ((float)_M.ARROW_FREE / (float)_M.ANIM_END) ) {
                anim_state = 2;
                animEventHandler(_M.ARROW_FREE);
                return;
            }
        }
        if (anim_state == 2)
        {
            if (f >= 1.0f) {
                anim_state = 4;
                animEventHandler(_M.ANIM_END);
                return;
            }
        }
    }

    public void animEventHandler(int i)
    {
        if (release == false) return;
        //print("PrintEvent: " + i + " called at: " + Time.time);
        if (i == _M.ARROW_RELEASE_START)
        {
            print("animEventHandler: ARROW_RELEASE_START called at: " + Time.time);
            FindObjectOfType<ArchParentActions>().arrowReleased();
            //Animator a = GetComponent<Animator>();
            //a.enabled = false;
        }
        else if (i == _M.ARROW_FREE)
        {
            //print("animEventHandler: ARROW_FREE called at: " + Time.time);
            //FindObjectOfType<ArchParentActions>().arrowReleased();
            //Animator a = GetComponent<Animator>();
            //a.enabled = false;
        }
        else if (i == _M.ANIM_END)
        {
            // stop animator
            //FindObjectOfType<ArchParentActions>().arrowReleased();
            Animator a = GetComponent<Animator>();
            a.Play("ArchAnimation", 0, 0f);
            a.enabled = false;
            print("animEventHandler: ANIM_END called at: " + Time.time);
        }
        else
        {
            print("animEventHandler: ??? called at: " + Time.time);
        }
    }

    // Update is called once per frame
    void Update()
    {
        processAnimation();

        if (release == true) return;
        Animator a = GetComponent<Animator>();
        if (a.enabled == false) a.enabled = true;
        a.Play("ArchAnimation", 0, (float) pullForce/_M.ANIM_END);
    }
    
    public void updatePullForce(int val, bool _release)
    {
        release = _release;
        pullForce = val;
        Animator a = GetComponent<Animator>();
        if (release == true)
        {
            //Animator a = GetComponent<Animator>();
            //a.Play("ArchAnimation", 0, 1.0f);// (float)pullForce / _M.ANIM_END);
            //playTime = 1.0f;
            //AnimationClip[] clips = a.runtimeAnimatorController.animationClips;

            // upen release we always go to free animation
            a.Play("ArchAnimation", 0, (float)_M.ARROW_FREE / (float) _M.ANIM_END);
            
        }
        else
        {
            if (anim_state == 4 || anim_state == 1)
            {
                anim_state = 0; // rearm state
                a.Play("ArchAnimation", 0, (float)0);
            }
        }
    }

    //public void resetState()
    //{
    //    anim_state = 0;
    //}
}
