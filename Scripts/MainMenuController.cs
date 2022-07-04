/* Copyright by: Cory Wolf */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MainMenuController : MonoBehaviour
{
    public bool nextLevel;
    public bool lastLevel;
    public bool quit;
    public bool options;
    public int amount;

    public void Called()
    {
        if (nextLevel) GameManager.instance.Next(amount);
        else if (lastLevel) GameManager.instance.Back(amount);
        else if (quit) GameManager.instance.Quit();
        else if (options) GameManager.instance.ActivateOptions(true);
    }

}