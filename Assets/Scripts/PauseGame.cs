using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseGame : MonoBehaviour
{
    public GameObject PauseMenu;
    public GameObject Gun;

    public GameObject MainMenu;
    public GameObject OptionsMenu;
    public GameObject QuitMenu;

    public Slider[] SoundSliders;
    public Toggle[] ResolutionToggles;
    public int[] ScreenWidths;

    private bool isPaused = false;
    private bool isGunPickedUp = false;

    public GameObject mainCamera;
    //private SimpleLUT sl;

    // Start is called before the first frame update
    void Start()
    {
        PauseMenu.SetActive(false);

        SoundSliders[0].value = AudioListener.volume;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPaused)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ResumeGame();
            }
        }
        else {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Debug.Log("Game is paused!");
                Time.timeScale = 0;
                isPaused = true;

                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                PauseMenu.SetActive(true);
                MainMenu.SetActive(true);
                OptionsMenu.SetActive(false);
                QuitMenu.SetActive(false);

                if (Gun.activeSelf) {
                    isGunPickedUp = true;
                }

                Gun.SetActive(false);
            }
        }
        
    }

    public void ResumeGame() {
        Debug.Log("Game is resumed!");
        Time.timeScale = 1;
        isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        PauseMenu.SetActive(false);

        if (isGunPickedUp)
        {
            Gun.SetActive(true);
        }
    }

    public void OpenOptions() {
        MainMenu.SetActive(false);
        OptionsMenu.SetActive(true);
    }

    public void CloseOptions()
    {
        MainMenu.SetActive(true);
        OptionsMenu.SetActive(false);
    }

    public void OpenQuit()
    {
        QuitMenu.SetActive(true);
        MainMenu.SetActive(false);
    }

    public void CloseQuit()
    {
        QuitMenu.SetActive(false);
        MainMenu.SetActive(true);
    }

    public void SetResolution(int i) 
    {
        if (ResolutionToggles[i].isOn) {
            float aspectRatio = 16 / 9f;
            Screen.SetResolution(ScreenWidths[i], (int)(ScreenWidths[i] / aspectRatio), false);
        }
    }

    public void SetMasterVoume(float value) 
    {
        AudioListener.volume = value;
    }

    public void QuitGame() {
        Application.Quit();
    }

    public void ExitToMainMenu() {
        SceneManager.LoadScene("MainMenu");
    }

   
}
