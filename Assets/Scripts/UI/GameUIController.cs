using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
  public class GameUIController : MonoBehaviour
  {
    public static GameUIController instance;

    [SerializeField]
    private GameObject inGamePanel;

    [SerializeField]
    private GameObject winPanel;

    [SerializeField]
    private GameObject gameOverPanel;
    
    public TextMeshProUGUI nextLevelText;

    private void Start()
    {
      inGamePanel.SetActive(true);
      gameOverPanel.SetActive(false);
    }

    private void Awake()
    {
      CreateInstance();
    }

    private void CreateInstance()
    {
      if (instance == null)
      {
        instance = this;
      }
    }


    public void OnOpenGameOverPanel()
    {
      inGamePanel.SetActive(false);
      gameOverPanel.SetActive(true);
    }

    public void OnOpenWinPanel()
    {
      inGamePanel.SetActive(false);
      winPanel.SetActive(true);
    }

    public void SetLevelText(string level)
    {
      nextLevelText.text = level;
    }

    public void OnOpenNextLevelPanel()
    {
      inGamePanel.SetActive(true);
      winPanel.SetActive(false);
    }

    public void OnReplayLevel()
    {
      inGamePanel.SetActive(true);
      gameOverPanel.SetActive(false);
    }

    public void OnLoadEndGameScene()
    {
      SceneManager.LoadScene("Scenes/EndGameScene");
    }
  }
}