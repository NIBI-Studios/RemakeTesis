using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    public void GoToMainMenu()
    {
        SceneManager.LoadScene(Constants.MAIN_MENU_SCENE_INDEX);
    }
}
