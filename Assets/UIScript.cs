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
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
    public Text score_txt;
    public Text arrows_txt;
    public Text gameOver_txt;
    public Button newGame_bt;
    public Button share_bt;
    public Button mute_bt;
//    public InputField nickname_ed;
    Vector3 newGame_bt_pos;
    Vector3 share_bt_pos;
    Vector3 nickname_ed_pos;


    // Start is called before the first frame update
    void Start()
    {
        // Get all text references
        Text[] all_texts = GetComponentsInChildren<Text>();
        ////Debug.Log("all_texts len = " + all_texts.Length.ToString());
        foreach (Text r in all_texts)
        {
            ////Debug.Log("found text " + r.name);
            if (r.name == "Score_txt")
            {
                score_txt = r;
            }
            if (r.name == "GameOver_txt")
            {
                gameOver_txt = r;
            }
            if (r.name == "Arrows_txt")
            {
                arrows_txt = r;
            }
            
        }

//        nickname_ed = GetComponentInChildren<InputField>();
//        nickname_ed_pos = nickname_ed.transform.position;

        Button[] all_buttons = GetComponentsInChildren<Button>();
        ////Debug.Log("all_buttons len = " + all_buttons.Length.ToString());
        foreach (Button r in all_buttons)
        {
            ////Debug.Log("found button " + r.name);
            if (r.name == "NewGame_bt")
            {
                newGame_bt = r;
                newGame_bt_pos = newGame_bt.transform.position;
            }
            if (r.name == "Share_bt")
            {
                share_bt = r;
                share_bt_pos = share_bt.transform.position;
                share_bt.interactable = false; // Disable Share button 
            }
            if (r.name == "Mute_bt")
            {
                mute_bt = r;
            }

        }

        score_txt.text = "Score: 0";

        // hide buttons
        showButtons(false);

    }

    public void showButtons(bool show)
    {
        if (show == false)
        {
            Vector3 hidepos = new Vector3(share_bt_pos.x, share_bt_pos.y - 1000f, share_bt_pos.z);
            newGame_bt.transform.position = hidepos;
            share_bt.transform.position = hidepos;
 //           nickname_ed.transform.position = hidepos;
            gameOver_txt.text = "";
        }
        else
        {
            newGame_bt.transform.position = newGame_bt_pos;
//            if (getNickname().Length > 0)
//            {
//                share_bt.transform.position = share_bt_pos;
//            }
//            nickname_ed.transform.position = nickname_ed_pos;
            gameOver_txt.text = "Game Over";
        }


    }

    public void scoreUpdate(int score)
    {
        score_txt.text = "Score: " + score.ToString();
    }

    public void arrowsUpdade(int n)
    {
        arrows_txt.text = "Arrows: " + n.ToString();
    }

    public void enableShareButton(bool b)
    {
        share_bt.interactable = b;
        if (b == true)
        {
            share_bt.transform.position = share_bt_pos;
        }
    }

    public string getNickname()
    {
        return "dummy"; //nickname_ed.text;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
