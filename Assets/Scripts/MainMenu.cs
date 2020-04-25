using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject PrimaryMenu;
    public GameObject OptionsMenu;
    public GameObject QuitMenu;
    public GameObject ControlsMenu; 

    public Slider[] SoundSliders;
    public Toggle[] ResolutionToggles;
    public int[] ScreenWidths;

    private void Start()
    {
        SoundSliders[0].value = AudioListener.volume;
    }

    public void OpenOptions()
    {
        PrimaryMenu.SetActive(false);
        OptionsMenu.SetActive(true);
    }

    public void CloseOptions()
    {
        PrimaryMenu.SetActive(true);
        OptionsMenu.SetActive(false);
    }

    public void OpenQuit()
    {
        QuitMenu.SetActive(true);
        PrimaryMenu.SetActive(false);
    }

    public void CloseQuit()
    {
        QuitMenu.SetActive(false);
        PrimaryMenu.SetActive(true);
    }

    public void SetResolution(int i)
    {
        if (ResolutionToggles[i].isOn)
        {
            float aspectRatio = 16 / 9f;
            Screen.SetResolution(ScreenWidths[i], (int)(ScreenWidths[i] / aspectRatio), false);
        }
    }

    public void SetMasterVoume(float value)
    {
        AudioListener.volume = value;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ShowControls() {
        ControlsMenu.SetActive(true);
        PrimaryMenu.SetActive(false);
    }

    public void LoadGame() {
        SceneManager.LoadScene("PreMaze");
    }
}
