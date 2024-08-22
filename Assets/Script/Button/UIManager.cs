using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    public static int coinShop;
    [SerializeField]
    private GameObject[] listLevel;

    public GameObject levelCh;
    public GameObject modePlay;
    public static bool isPause = false;
    public GameObject noitceSave;
    public GameObject noitceMoney;
    public static bool isSaveMe = false;
    public GameObject shopPanel;
    public GameObject pausePanel;
    public GameObject settingPanel;
    public GameObject newHighScoreNoitce;
    public GameObject pauseButton;
    public Slider _musicSlider, _sfxSlider;
    public TextMeshProUGUI saveCoin;

    
    public static bool unlockMap2 = false;
    public static bool unlockMap3 = false;
    public static bool unlockMap4 = false;
    public static bool unlockMap5 = false;
    private void Awake()
    {
        isPause = false;
        coinShop = PlayerPrefs.GetInt("CoinShop", 0);

        // Load unlocked maps status
        unlockMap2 = PlayerPrefs.GetInt("UnlockMap2", 0) == 1;
        unlockMap3 = PlayerPrefs.GetInt("UnlockMap3", 0) == 1;
        unlockMap4 = PlayerPrefs.GetInt("UnlockMap4", 0) == 1;
        unlockMap5 = PlayerPrefs.GetInt("UnlockMap5", 0) == 1;



        if (listLevel == null || listLevel.Length == 0)
        {
            //Debug.LogError("listLevel is null or not properly initialized in Awake!");
        }
        else
        {
            ActivateUnlockedLevels();
        }
    }
    private void Start()
    {
        if (listLevel == null || listLevel.Length == 0)
        {
            //Debug.LogError("listLevel is null or not properly initialized in Start!");
            return; // Thoát hàm nếu listLevel không hợp lệ
        }
        saveCoin.text = $"{coinShop}";
        // Đảm bảo thanh trượt hiển đúng giá trị âm lượng hiện tại khi Play
        _musicSlider.value = AudioManager.instance.musicVolume;
        _sfxSlider.value = AudioManager.instance.sfxVolume;

        // lưu giá trị thanh trượt thay đổi
        _musicSlider.onValueChanged.AddListener(MusicVolume);
        _sfxSlider.onValueChanged.AddListener(FSXVolume);

        ActivateUnlockedLevels();
    }

    private void Update()
    {
        PlayerPrefs.SetInt("CoinShop", coinShop);
        PlayerPrefs.Save();
        saveCoin.text = $"{coinShop}";

        if (PlayerMovement.unlockMap)
        {
            switch (TileManager.currentScene)
            {
                case "Level1":
                    unlockMap2 = true;
                    PlayerPrefs.SetInt("UnlockMap2", 1);
                    break;
                case "Level2":
                    unlockMap3 = true;
                    PlayerPrefs.SetInt("UnlockMap3", 1);
                    break;
                case "Level3":
                    unlockMap4 = true;
                    PlayerPrefs.SetInt("UnlockMap4", 1);
                    break;
                case "Level4":
                    unlockMap5 = true;
                    PlayerPrefs.SetInt("UnlockMap5", 1);
                    break;
            }
            PlayerPrefs.Save();

            // Update level activation after unlocking
            ActivateUnlockedLevels();

        }
    }

    private void ActivateUnlockedLevels()
    {
        if (listLevel == null || listLevel.Length == 0)
        {
            //Debug.LogError("listLevel is null or not properly initialized in ActivateUnlockedLevels!");
            return; // Thoát hàm nếu listLevel không hợp lệ
        }

        if (unlockMap2 && listLevel.Length > 1 && listLevel[1] != null)
        {
            listLevel[1].SetActive(true);
        }

        if (unlockMap3 && listLevel.Length > 2 && listLevel[2] != null)
        {
            listLevel[2].SetActive(true);
        }

        if (unlockMap4 && listLevel.Length > 3 && listLevel[3] != null)
        {
            listLevel[3].SetActive(true);
        }

        if (unlockMap5 && listLevel.Length > 4 && listLevel[4] != null)
        {
            listLevel[4].SetActive(true);
        }
    }

    public static int TakeCoinShop(int x)
    {
        return coinShop += x;
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
    }
    public void ShopPanel()
    {
        shopPanel.SetActive(true);

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
    public void Buy()
    {
        if (coinShop >= 200)
        {
            AudioManager.instance.PlaySFX("Coin Drop");
            isSaveMe = true;
            noitceSave.SetActive(false);
            CoinUse(200);
            PlayerPrefs.SetInt("CoinShop", coinShop);
            PlayerPrefs.Save();
            Time.timeScale = 1f;
        }
        else
        {
            AudioManager.instance.PlaySFX("Noitce");
            noitceMoney.SetActive(true);
        }
    }
    public void NoSetup()
    {
        noitceMoney.SetActive(false);
        newHighScoreNoitce.SetActive(false);
        noitceSave.SetActive(false);
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
            isPause = true;
            pausePanel.SetActive(true);
        }
    }
    public void SaveMe()
    {
        noitceSave.SetActive(true);
    }
    public void ResumeGame()
    {
        isPause = false;
        isSaveMe = true;
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
    }
    public void PlayGame()
    {
        modePlay.SetActive(true);
    }
    public void MenuGame()
    {
        Time.timeScale += 1f;
        SceneManager.LoadScene("Menu");
    }
    public void Back()
    {
        settingPanel.SetActive(false);
        if (shopPanel != null)
        {
            shopPanel.SetActive(false);
        }
        if (levelCh != null)
        {
            levelCh.SetActive(false);
        }
    }

    public static int CoinUse(int x)
    {
        return coinShop -= x;
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Time.timeScale = 1f;
    }

    public void Level(int levelIndex)
    {
        // Đảm bảo chỉ số level nằm trong phạm vi mảng
        if (levelIndex >= 0 && levelIndex < listLevel.Length)
        {
            // Dừng nhạc Menu
            AudioManager.instance.StopMusic("Menu Theme");

            // Kiểm tra và chuyển đến scene tương ứng với levelIndex
            string sceneName = "";
            switch (levelIndex)
            {
                case 0:
                    sceneName = "Level1";
                    break;
                case 1:
                    sceneName = "Level2";
                    break;
                case 2:
                    sceneName = "Level3";
                    break;
                case 3:
                    sceneName = "Level4";
                    break;
                case 4:
                    sceneName = "Level5";
                    break;
                default:
                    Debug.LogError("Level index out of range!");
                    return;
            }

            // Chuyển scene
            SceneManager.LoadScene(sceneName);
            Time.timeScale = 1f;
        }
        else
        {
            Debug.LogError("Level index out of bounds!");
        }
    }

    public void ModeSur()
    {
        AudioManager.instance.StopMusic("Menu Theme");
        Time.timeScale = 1f;
        SceneManager.LoadScene("Play");
    }
    public void ModeLevel()
    {
        levelCh.SetActive(true);
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
