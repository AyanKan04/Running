using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject pausePanel;
    public GameObject settingPanel;
    public GameObject newHighScoreNoitce;
    public GameObject pauseButton;
    public Slider _musicSlider, _sfxSlider;


    private void Start()
    {
        // Đảm bảo thanh trượt hiển đúng giá trị âm lượng hiện tại khi Play
        _musicSlider.value = AudioManager.instance.musicVolume;
        _sfxSlider.value = AudioManager.instance.sfxVolume;

        // lưu giá trị thanh trượt thay đổi
        _musicSlider.onValueChanged.AddListener(MusicVolume);
        _sfxSlider.onValueChanged.AddListener(FSXVolume);
    }

    private void Update()
    {
        if (PlayerManager.gameOver)
        {
            pauseButton.SetActive(false);
        }
    }
    public void ReplayGame()
    {
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
       
    }
    public void Settings()
    {
        settingPanel.SetActive(true);
        if (SceneManager.GetActiveScene().name == "Play")
        {
            pausePanel.SetActive(false);
        }
        
    }
    public void SetupNewHighScore()
    {
        newHighScoreNoitce.SetActive(true);
    }
    public void YesSetup()
    {
        PlayerManager.ResetHighScore();
        newHighScoreNoitce.SetActive(false);
    }
    public void NoSetup()
    {
        newHighScoreNoitce.SetActive(false);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void PauseGame()
    {
        if (!PlayerManager.gameOver)
        {
            Time.timeScale = 0f;
            pauseButton.SetActive(false);
            pausePanel.SetActive(true);
        }        
    }
    public void ResumeGame()
    {
        pauseButton.SetActive(true);
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
    }
    public void PlayGame()
    {
        AudioManager.instance.StopMusic("Menu Theme");
        Time.timeScale = 1f;
        SceneManager.LoadScene("Play");
    }
    public void MenuGame()
    {
        Time.timeScale += 1f;
        SceneManager.LoadScene("Menu");
    }
    public void Back()
    {        
        settingPanel.SetActive(false);
        if (SceneManager.GetActiveScene().name == "Play")
        {
            pausePanel.SetActive(true);
        }
    }


    public void ToggleMusic()
    {
        AudioManager.instance.ToggleMusic();
    }
    public void ToggleSFX()
    {
        AudioManager.instance.ToggleFSX();
    }
    public void MusicVolume(float volume)
    {
        AudioManager.instance.MucsicVolume(volume);
    }
    public void FSXVolume(float volume)
    {
        AudioManager.instance.SFXVolume(volume);
    }
}
