/* Copyright by: Cory Wolf */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{

    public static GameManager instance;
    [HideInInspector]
    public GameObject prevObject;
    public GameObject playerMesh;
    public GameObject crossH;
    public GameObject distractText;
    private bool canActivate = true;
    private GameObject[] guards;
    public GameObject endGameScreen;
    public bool paused;
    public GameObject pauseMenu;
    [HideInInspector]
    public bool endGame;
    [HideInInspector]
    public bool optionsOn;
    public GameObject optionsMenu;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        endGame = false;
        prevObject = PlayerController.instance.gameObject;
        playerMesh.SetActive(false);
        distractText.SetActive(false);
    }
    private void Update()
    {
        guards = GameObject.FindGameObjectsWithTag("Guard");
        if(Input.GetKeyDown(SettingsManager.instance.pauseKey) && !endGame && !optionsOn)
        {
            if (!paused)
                PauseGame();
            else
                ResumeGame();
        }

        else if(Input.GetKeyDown(SettingsManager.instance.pauseKey) && optionsOn)
        {
            ActivateOptions(false);
            Time.timeScale = 1;
            PlayerController.instance.stopCamera = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        PlayerController.instance.stopCamera = true;
        paused = true;
    }

    void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        PlayerController.instance.stopCamera = false;
        Cursor.lockState = CursorLockMode.Locked;
        paused = false;
    }

    public bool InteractiveCheck(GameObject gameob)
    {
        if (gameob.tag == "Interactive")
            return true;
        else
            return false;
    }

    public bool PlayerCheck(GameObject gameOb)
    {
        if (gameOb.tag == "Player")
            return true;
        else
            return false;
    }

    public bool DisabledCheck(GameObject gameOb)
    {
        if (gameOb.tag == "Disabled")
            return true;
        else
            return false;
    }

    public void FreezeRotation(Rigidbody rb, bool active)
    {
        rb.freezeRotation = active;
    }


    public void InteractiveEffects(GameObject gameOb)
    {
        if (DisabledCheck(gameOb)) return;

        if (PlayerCheck(gameOb))
        {
            DisplayTextUI(false);
            playerMesh.SetActive(false);
            CameraController.instance.target = CameraController.instance.head;
            FreezeRotation(PlayerController.instance.rb, true);
            PlayerController.instance.transform.localRotation = new Quaternion(0f, 0f, 0f, 0f);
        }
        else
        {
            DisplayTextUI(true);
            playerMesh.SetActive(true);
            PlayerController.instance.rb.velocity = Vector3.one;
            FreezeRotation(PlayerController.instance.rb, false);
            CameraController.instance.target = gameOb.transform;
        }


        PlayerController.instance.isFps = PlayerCheck(gameOb);


        //gameOb.layer = LayerMask.NameToLayer("Ignore Raycast");
        //prevObject.layer = LayerMask.NameToLayer("Visible");
        prevObject = gameOb;

    }

    public void CauseDistraction()
    {
        prevObject.tag = "Disabled";
        Distraction(prevObject.transform);
        InteractiveEffects(PlayerController.instance.gameObject);
    }


    void Distraction(Transform location)
    {

        float shortestDistance = Mathf.Infinity;
        GameObject nearestGuard = null;

        foreach (GameObject guard in guards)
        {
            float distanceToGuard = Vector3.Distance(location.position, guard.transform.position);

            if (distanceToGuard < shortestDistance)
            {
                shortestDistance = distanceToGuard;
                nearestGuard = guard;
            }
        }

        if (nearestGuard != null)
        {
            Transform effectedGuard = nearestGuard.transform;
            effectedGuard.GetComponent<GuardController>().ChangeDestination(location);
            

        }
    }

    public void LookEffects(GameObject gameOb)
    {
        crossH.SetActive(true);
    }

    public void DisableLookEffects()
    {
        crossH.SetActive(false);
        canActivate = true;
    }

    public void DisplayTextUI(bool active)
    {
        if(active)
            distractText.SetActive(true);
        else
            distractText.SetActive(false);
    }


    public void Caught()
    {
        endGame = true;
        Debug.Log("GameOver");
        Pause();
        endGameScreen.SetActive(true);
    }

    public void Pause()
    {
        PlayerController.instance.stopCamera = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void Reset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        ResumeGame();
    }

    public void Back(int amount)
    {
        ResumeGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - amount);
    }

    public void Next(int amount)
    {
        ResumeGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + amount);
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Exit");
    }

    public void ActivateOptions(bool active)
    {
        optionsMenu.SetActive(active);
        optionsOn = active;
        Pause();
    }

}