/* Copyright by: Cory Wolf */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SettingsManager : MonoBehaviour
{

    public static SettingsManager instance;

    [Header("Inputs")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode crouchKey = KeyCode.LeftShift;
    public KeyCode interactKey = KeyCode.E;
    public KeyCode distractKey = KeyCode.Q;
    public KeyCode pauseKey = KeyCode.Escape;

    [Header("Values")]
    public float mouseSens = 2f;
    public float isMute;


    void Awake()
    {
        instance = this;
    }

}