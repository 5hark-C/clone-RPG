using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_MainMenu : MonoBehaviour
{
    [SerializeField] private string sceneName = "MainScene";
    [SerializeField] private GameObject continueButton;
    [SerializeField] UI_FadeScreen fadeScreen;

    private void Start()
    {
        if(SaveManager.instance.HasSaveData() == false)
            continueButton.SetActive(false);
    }

    public void ContinueGame()
    {
        StartCoroutine(LOadSceneWithFadeEffect(1.5f));
    }

    public void NewGame()
    {
        SaveManager.instance.DeletSaveData();
        StartCoroutine(LOadSceneWithFadeEffect(1.5f));
    }

    public void ExitGame()
    {
        //Application.Quit();
    }

    IEnumerator LOadSceneWithFadeEffect(float _delay)
    {
        fadeScreen.FadeOut();

        yield return new WaitForSeconds(_delay);

        SceneManager.LoadScene(sceneName);
    }
}
