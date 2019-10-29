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
using UnityEngine.Networking;



public class Manager : MonoBehaviour
{
    //------------ Globals -------------
    // usage:
    //  private Manager GlobalVars;
    //  GlobalVars = FindObjectOfType<Manager>();

    //-------- Arch Pull and Turn -------
    float _PULL_FACTOR = 0.1f;
    public float PULL_FACTOR;

    float _TURN_FACTOR = 0.1f;
    public float TURN_FACTOR;

    float _MAX_TURN_ANGLE = 20.0f;
    public float MAX_TURN_ANGLE;

    int _MAX_PULL_FORCE = 35;
    public int MAX_PULL_FORCE;

    int _PULL_FORCE_TRIGGER = 15;
    public int PULL_FORCE_TRIGGER;

    int _PULL_FORCE_RELEASE = -1;
    public int PULL_FORCE_RELEASE;

    //------- Arch Animation ---------
    int _ARROW_RELEASE_START = 36;
    public int ARROW_RELEASE_START;

    int _ARROW_FREE = 37;
    public int ARROW_FREE;

    int _ANIM_END = 44;
    public int ANIM_END;

    //----- Arch mesh ID's

    int _ARCH_AND_ARROW_MESH_ID = 0;
    public int ARCH_AND_ARROW_MESH_ID;

    int _ARCH_MESH_ID = 1;
    public int ARCH_MESH_ID;

    string _ARCH_AND_ARROW_MESH_NAME = "arch";
    public string ARCH_AND_ARROW_MESH_NAME;

    string _ARCH_MESH_NAME = "arch_arrow";
    public string ARCH_MESH_NAME;

    float _MAX_X_DISPLACEMENT = 0.7f;
    public float MAX_X_DISPLACEMENT;

    public int level = 1;

    public int SOUND_SHOOT = 4; //fake... use for sound ID

    void Awake()
    {
        PULL_FACTOR =_PULL_FACTOR;
        TURN_FACTOR = _TURN_FACTOR;
        MAX_TURN_ANGLE = _MAX_TURN_ANGLE;
        MAX_PULL_FORCE = _MAX_PULL_FORCE;
        PULL_FORCE_TRIGGER = _PULL_FORCE_TRIGGER;
        PULL_FORCE_RELEASE = _PULL_FORCE_RELEASE;
        //------- Arch Animation ---------
        ARROW_RELEASE_START = _ARROW_RELEASE_START;
        ARROW_FREE = _ARROW_FREE;
        ANIM_END = _ANIM_END;
        ARCH_AND_ARROW_MESH_ID = _ARCH_AND_ARROW_MESH_ID;
        ARCH_MESH_ID = _ARCH_MESH_ID;
        ARCH_AND_ARROW_MESH_NAME = _ARCH_AND_ARROW_MESH_NAME;
        ARCH_MESH_NAME = _ARCH_MESH_NAME;
        MAX_X_DISPLACEMENT = _MAX_X_DISPLACEMENT;
        SOUND_SHOOT = 4;
        level = 1;
    }

    /*



        shooting cycle:

        ST_SHOOTING state

        Manager:
          -set state = ST_SHOOTING
          -call ArchParentAction.startShooting()
          -waits until get a colligion event

        ArchParentAction:
          - mouse down: keep pointing arrow
          - mouse release:
            - if force < trigger force wait another mouse down
            - if force >= trigger
              -let annimation proceed.
              - when animation handler get frame = ARROW_FREE then
                 - swap arch_arrow to arch_only visibility
                 - call ArrowAction startArrowLaunch
              -



         */



    float t;
    int NUM_ARROWS = 50;
    int arrows;
    int score = 0;
    ArchParentActions archParentActions;
    ArrowActions arrowActions;
    UIScript uiScript;
    MainLightActions mainLightActions;
    int PUMP_LEFT_ID = 0;
    int PUMP_CENTER_ID = 1;
    int PUMP_RIGHT_ID = 2;
    GameObject[] pumpkins = new GameObject[3];
    PumpkinActions[] pumpkinsActions = new PumpkinActions[3];
    bool[] pumpScored = new bool[3];
    int state = 0;
    private bool restartFlag = false;
    int ST_IDLE = 0;
    int ST_SHOOTING = 1;
    int ST_WAIT_REARM = 2;
    int ST_WAIT_RESTART = 3;
    int ST_WAIT_LEVEL = 4;

    int SCORE_GOLD = 5;
    int SCORE_SILVER = 3;
    int SCORE_NORMAL = 1;
    int SOUND_MISSING = 0; //fake... use for sound ID
    int SOUND_THANKS = 2; //fake... use for sound ID


    float BONUS_TIME = 45f;
    float time_bonus_ck;

    float delay_start = 0;

    AudioSource gold_sound;
    AudioSource silver_sound;
    AudioSource launch_sound;
    AudioSource normal_sound;
    AudioSource dunk_sound;
    AudioSource thanks_sound;

    AudioSource game_over_sound;
    AudioSource flute_sound;
    AudioSource sax_sound;


    // Start is called before the first frame update
    void Start()
    {
        t = Time.time;

        uiScript = FindObjectOfType<UIScript>();

        archParentActions = FindObjectOfType<ArchParentActions>();
        arrowActions = FindObjectOfType<ArrowActions>();

        mainLightActions = FindObjectOfType<MainLightActions>();

        pumpkins[PUMP_LEFT_ID] = GameObject.Find("empty_pumpkin_left/pumpkin");
        pumpkinsActions[PUMP_LEFT_ID] = pumpkins[PUMP_LEFT_ID].GetComponent<PumpkinActions>();

        pumpkins[PUMP_CENTER_ID] = GameObject.Find("empty_pumpkin/pumpkin");
        pumpkinsActions[PUMP_CENTER_ID] = pumpkins[PUMP_CENTER_ID].GetComponent<PumpkinActions>();

        pumpkins[PUMP_RIGHT_ID] = GameObject.Find("empty_pumpkin_right/pumpkin");
        pumpkinsActions[PUMP_RIGHT_ID] = pumpkins[PUMP_RIGHT_ID].GetComponent<PumpkinActions>();

        arrows = NUM_ARROWS;
        uiScript.arrowsUpdade(arrows);

        gold_sound = GameObject.Find("ManagerDummy/gold").GetComponent<AudioSource>();
        silver_sound = GameObject.Find("ManagerDummy/silver").GetComponent<AudioSource>();
        launch_sound = GameObject.Find("ManagerDummy/launch").GetComponent<AudioSource>();
        normal_sound = GameObject.Find("ManagerDummy/normal").GetComponent<AudioSource>();
        dunk_sound = GameObject.Find("ManagerDummy/dunk").GetComponent<AudioSource>();
        thanks_sound = GameObject.Find("ManagerDummy/thanks").GetComponent<AudioSource>();

        game_over_sound = GameObject.Find("ManagerDummy/game_over_banjo").GetComponent<AudioSource>();
        flute_sound = GameObject.Find("ManagerDummy/level2_flute").GetComponent<AudioSource>();
        sax_sound = GameObject.Find("ManagerDummy/level3_sax").GetComponent<AudioSource>();

        AudioSource[] allAudio = FindObjectsOfType<AudioSource>();
        foreach (AudioSource a in allAudio) a.Stop();

    }

    void setMotionEnabled(bool b)
    {
        pumpkinsActions[0].setMotionEnabled(b);
        pumpkinsActions[1].setMotionEnabled(b);
        pumpkinsActions[2].setMotionEnabled(b);
    }

    void pumpkinsOff()
    {
        int i;
        for (i = 0; i < pumpkins.Length; i++)
        {
            pumpkinFireOn(i, false);
            pumpScored[i] = false;
        }
    }

    bool mouse_state = false;
    bool mouse_down_flag = false;
    bool mouse_up_flag = false;

    void processMouse()
    {
        bool mouse_now = Input.GetMouseButton(0);
        if (mouse_state != mouse_now)
        {
            if (mouse_now == true)
            {
                mouse_down_flag = true;
            }
            else
            {
                mouse_up_flag = true;
            }
            mouse_state = mouse_now;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // time bonus for the first 100 minute -> up to 5 times
        if (time_bonus_ck > 1.0f)
        {
            time_bonus_ck -= Time.deltaTime;
        }

        processMouse();
        if (state == ST_IDLE)
        {

            if (mouse_down_flag == true)
            {
                mouse_down_flag = false;
                mouse_up_flag = false;
                pumpkinsOff();
                setMotionEnabled(true);

                archParentActions.mouseDownFlag = true;
                mainLightActions.enableFlicker(true);
                state = ST_SHOOTING;
                time_bonus_ck = BONUS_TIME;

                archParentActions.startShooting();
            }

        }

        else if (state == ST_WAIT_REARM)
        {
            if (mouse_down_flag == true)
            {
                mouse_down_flag = false;
                mouse_up_flag = false;
                archParentActions.mouseDownFlag = true;
                mainLightActions.enableFlicker(true);
                state = ST_SHOOTING;

                archParentActions.startShooting();
            }
        }

        else if (state == ST_SHOOTING)
        {
            if (mouse_down_flag == true)
            {
                mouse_down_flag = false;
                mouse_up_flag = false;
                archParentActions.mouseDownFlag = true;
                mainLightActions.enableFlicker(true);
            }
            else if (mouse_up_flag == true)
            {
                mouse_up_flag = false;
                archParentActions.mouseUpFlag = true;
            }

        }

        else if (state == ST_WAIT_RESTART)
        {
            if (restartFlag == true)
            {
                setMotionEnabled(true);
                restartFlag = false;
                level = 1;
                score = 0;
                arrows = NUM_ARROWS;
                uiScript.arrowsUpdade(arrows);
                uiScript.scoreUpdate(score);
                state = ST_IDLE;
                uiScript.showButtons(false);
                archParentActions.setTurn(0f);
                pumpkinsOff();
                pumpkins[0].GetComponent<PumpkinActions>().resetPosition();
                pumpkins[1].GetComponent<PumpkinActions>().resetPosition();
                pumpkins[2].GetComponent<PumpkinActions>().resetPosition();
            }
        }
        else if (state == ST_WAIT_LEVEL)
        {
            if (Time.time - delay_start < 2.0f) { return;  }
            state = ST_IDLE; // for now
            pumpkinsOff();
            archParentActions.setTurn(0f);
            setMotionEnabled(true);
        }


    }

    private void pumpkinFireOn(int pump_id, bool on_off)
    {
        pumpkinsActions[pump_id].fireOn(on_off);
    }

    int getPumpID(string pump_name)
    {
        int id = 0;
        if (pump_name == "empty_pumpkin")
            id = PUMP_CENTER_ID;
        else if (pump_name == "empty_pumpkin_right")
            id = PUMP_RIGHT_ID;
        else if (pump_name == "empty_pumpkin_left")
            id = PUMP_LEFT_ID;
        return id;
    }

    private void addScore(string colliderName)
    {
        float timeBonus_f = 1 + (time_bonus_ck * 4f) / BONUS_TIME; // BONUS_TIME time is up to 5 times
        int bonus = (int)(timeBonus_f * 5f * level);


        //Debug.Log("Score for " + colliderName + ", bonus = " + bonus.ToString());
        if (colliderName == "c_good_gold")
        {
            score += (SCORE_GOLD * bonus);
            playSound(SCORE_GOLD);
        }
        else if (colliderName == "c_good_silver")
        {
            score += (SCORE_SILVER * bonus);
            playSound(SCORE_SILVER);
        }
        else
        {
            score += (SCORE_NORMAL * bonus);
            playSound(SCORE_NORMAL);
        }
        uiScript.scoreUpdate(score);
    }

    public void playSound(int id)
    {
        if (id == SCORE_GOLD)
        {
            gold_sound.Play();
        }
        else if (id == SCORE_SILVER)
        {
            silver_sound.Play();
        }
        else if (id == SCORE_NORMAL)
        {
            normal_sound.Play();
        }
        else if (id == SOUND_MISSING)
        {
            dunk_sound.Play();
        }
        else if (id == SOUND_THANKS)
        {
            thanks_sound.Play();
        }
        else if (id == SOUND_SHOOT)
        {
            launch_sound.Play();
        }

    }

    public void colligionDetected(Collision collision)
    {
        string c_obj_name = collision.gameObject.name;

        if (state != ST_SHOOTING) return;

        mainLightActions.enableFlicker(false);

        state = ST_WAIT_REARM;

        if (c_obj_name.IndexOf("c_good", 0, System.StringComparison.Ordinal) == 0)
        {
            string pumpkin_name = collision.gameObject.transform.parent.gameObject.transform.parent.name;
            // pumpkin_name either: empty_pumpkin, empty_pumpkin_right or empty_pumpkin_left

            // hide arrow to fake that it gets inside the pump.
            arrowActions.showArrow(false);

            int pumpID = getPumpID(pumpkin_name);
            if (pumpScored[pumpID] == false)
            {
                pumpkinFireOn(pumpID, true);
                addScore(c_obj_name);
                pumpScored[pumpID] = true;

                // if all pumps were scored we are done with this level
                //print("pumpScored[0] = " + pumpScored[0].ToString() + ", pumpScored[1] = " + pumpScored[1].ToString() + ", pumpScored[2] = " + pumpScored[2].ToString());
                if (pumpScored[0] == true && pumpScored[1] == true && pumpScored[2] == true)
                {
                    //Debug.Log("LEVEL COMPLETED");
                    level++;
                    if (level > 3) sax_sound.Play();
                    else flute_sound.Play();
                    setMotionEnabled(false);
                    state = ST_WAIT_LEVEL;
                    delay_start = Time.time;
                }
            }
            
        }
        else
        {
            playSound(SOUND_MISSING);
        }

        arrows -= 1;
        uiScript.arrowsUpdade(arrows);
        if (arrows == 0)
        {
            uiScript.showButtons(true);
            setMotionEnabled(false);
            game_over_sound.Play();
            state = ST_WAIT_RESTART;
        }
    }

    public void onMuteButton()
    {
        //Debug.Log("Lets Mute/Unmute");
    }
    string SHARE_URL = "http://www.cglabs.ca/score.php/";
    //string SHARE_URL = "http://www.cglabs.ca/score.php"; // for using upload

    public void onShareButton()
    {
        //Debug.Log("Lets share");
        thanks_sound.Play();
        //StartCoroutine(Upload());

        string url = "?name=" + System.Net.WebUtility.UrlEncode(uiScript.getNickname()) + "&score=";
        url += score.ToString() + "&key=51" + ((score * 7) - 4).ToString();
        url = SHARE_URL + url;
        //Debug.Log("Share URL: " + url);
        Application.OpenURL(url);
    }

    public void onNewGameButton()
    {
        //Debug.Log("Lets Restart Game");
        restartFlag = true;
    }

    public void onNicknameChange()
    {
        //Debug.Log("onNicknameChange event");
        if (uiScript.getNickname().Length == 0)
        {
            uiScript.enableShareButton(false);
        }
        else
        {
            uiScript.enableShareButton(true);
        }
        
    }

    IEnumerator Upload()
    {
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        string form_data = "name=" + System.Net.WebUtility.UrlEncode(uiScript.getNickname()) + "&score=";
        form_data += score.ToString() + "&key=51" + ((score * 7) - 4).ToString();
        formData.Add(new MultipartFormDataSection(form_data));

        UnityWebRequest www = UnityWebRequest.Post(SHARE_URL, formData);

        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            //Debug.Log(www.error);
        }
        else
        {
            //Debug.Log("Form upload complete!");
        }
    }
}
