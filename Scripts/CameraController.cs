/* Copyright by: Cory Wolf */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CameraController : MonoBehaviour
{

    public Transform head;
    [HideInInspector]
    public Transform target;
    private RaycastHit hit;
    private KeyCode interactKey;
    private KeyCode distractKey;
    public float distance;

    public static CameraController instance;

    private void Awake()
    {
        instance = this;
    }


    private void Start()
    {
        target = head;
        this.interactKey = SettingsManager.instance.interactKey;
        this.distractKey = SettingsManager.instance.distractKey;
    }

    void Update()
    {
        Ray ray = new Ray();
        ray.origin = this.transform.position;
        ray.direction = this.transform.TransformDirection(Vector3.forward);

        Debug.DrawRay(ray.origin, ray.direction * 100);
        if (Physics.Raycast(ray, out hit, distance))
        {
            if (GameManager.instance.InteractiveCheck(hit.collider.gameObject) || GameManager.instance.PlayerCheck(hit.collider.gameObject))
                GameManager.instance.LookEffects(hit.collider.gameObject);
            else
                GameManager.instance.DisableLookEffects();

            if (Input.GetKeyDown(interactKey))
            {
                if (GameManager.instance.InteractiveCheck(hit.collider.gameObject) || GameManager.instance.PlayerCheck(hit.collider.gameObject))
                {
                    Check();
                }
            }
        }
        else
            GameManager.instance.DisableLookEffects();

        if (Input.GetKeyDown(distractKey) && !GameManager.instance.PlayerCheck(GameManager.instance.prevObject))
        {
            GameManager.instance.CauseDistraction();
        }


        transform.position = target.position;
    }

    void Check()
    {
        MainMenuController MC = hit.collider.gameObject.GetComponent<MainMenuController>();
        if (MC != null)
        {
            MC.Called();
            return;
        }
        GameManager.instance.InteractiveEffects(hit.collider.gameObject);
    }

}