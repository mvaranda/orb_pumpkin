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

using UnityEngine;
using System.Collections.Generic;
using System;

// ref: https://riptutorial.com/unity3d/example/23089/singleton-implementation-through-base-class

public abstract class Singleton<T> : MonoBehaviour
{

    private static Dictionary<Type, object> _singletons
        = new Dictionary<Type, object>();

    public static T Instance
    {
        get
        {
            return (T)_singletons[typeof(T)];
        }
    }

    void OnEnable()
    {
        if (_singletons.ContainsKey(GetType()))
        {
            Destroy(this);
        }
        else
        {
            _singletons.Add(GetType(), this);
            DontDestroyOnLoad(this);
        }
    }
}
