using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainManuUIManager : MonoBehaviour
{
    private int currentPilar;
    [SerializeField] private GameObject pilarsMenu;
    [SerializeField] private GameObject activitiesMenu;
    [SerializeField] private GameObject onlineCanvases;

    private void Start()
    {
        if (User.UserId != null)
        {
            onlineCanvases.SetActive(true);
        }
    }

    public void OpenPilarActivitiesMenu(int pilar)
    {
        currentPilar = pilar;
        pilarsMenu.SetActive(false);
        activitiesMenu.SetActive(true);
    }
    public void GoToTheory()
    {
        switch (currentPilar)
        {
            case 1:
                SceneManager.LoadScene(Constants.ABSTRACTION_THEORY_SCENE_INDEX);
                break;
            case 2:
                SceneManager.LoadScene(Constants.ENCAPSULATION_THEORY_SCENE_INDEX);
                break;
            case 3:
                SceneManager.LoadScene(Constants.INHERITANCE_THEORY_SCENE_INDEX);
                break;
            case 4:
                SceneManager.LoadScene(Constants.POLYMORPHISM_THEORY_SCENE_INDEX);
                break;
        }
    }
    public void GoToGame()
    {
        switch (currentPilar)
        {
            case 1:
                SceneManager.LoadScene(Constants.ABSTRACTION_GAME_SCENE_INDEX);
                break;
            case 2:
                SceneManager.LoadScene(Constants.ENCAPSULATION_GAME_SCENE_INDEX);
                break;
            case 3:
                SceneManager.LoadScene(Constants.INHERITANCE_GAME_SCENE_INDEX);
                break;
            case 4:
                SceneManager.LoadScene(Constants.POLYMORPHISM_GAME_SCENE_INDEX);
                break;
        }
    }
    public void Back()
    {
        activitiesMenu.SetActive(false);
        pilarsMenu.SetActive(true);
    }
    public void GoToTutorial()
    {
        SceneManager.LoadScene(Constants.TUTORIAL_SCENE_INDEX);
    }
    public void QuitApp()
    {
        Application.Quit();
    }
}
