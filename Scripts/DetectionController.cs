/* Copyright by: Cory Wolf */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DetectionController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(GameManager.instance.PlayerCheck(other.gameObject) && PlayerController.instance.isFps)
            {
            GameManager.instance.Caught();
        }
    }
}