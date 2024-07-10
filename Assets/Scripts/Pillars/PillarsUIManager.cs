using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PillarsUIManager : MonoBehaviour
{
    public void GoToMainMenu()
    {
        SceneManager.LoadScene(Constants.MAIN_MENU_SCENE_INDEX);
    }
    public void GoToGame(int index)
    {
        SceneManager.LoadScene(index);
    }
}
