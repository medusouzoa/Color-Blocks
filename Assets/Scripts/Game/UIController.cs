using TMPro;
using UnityEngine;

namespace Game
{
  public class UIController : MonoBehaviour
  {
    public static UIController instance;

    [SerializeField]
    private GameObject inGamePanel;

    [SerializeField]
    private GameObject gameOverPanel;

    public TextMeshProUGUI levelText;
    public TextMeshProUGUI movesText;

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

    public void OnReplayLevel()
    {
      inGamePanel.SetActive(true);
      gameOverPanel.SetActive(false);
    }

    public void UpdateUI(int currentLevel, int remainingMoves)
    {
      levelText.text = "Level: " + currentLevel;
      movesText.text = "Moves: " + remainingMoves;
    }

    public void UpdateMoveText(string move)
    {
      movesText.text = move;
    }
  }
}