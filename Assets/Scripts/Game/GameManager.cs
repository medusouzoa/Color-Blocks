using Level;
using TMPro;
using UnityEngine;

namespace Game
{
  public class GameManager : MonoBehaviour
  {
    public static GameManager Instance { get; private set; }

    public TextMeshProUGUI levelText;
    public TextMeshProUGUI movesText;
    private int _currentLevel = 1;
    private int _remainingMoves;

    void Awake()
    {
      if (Instance == null)
      {
        Instance = this;
      }
      else
      {
        Destroy(gameObject);
      }
    }

    public void SetMoveLimit(int moveLimit)
    {
      _remainingMoves = moveLimit;
      UpdateUI();
    }

    public void SetLevelNumber(int levelNumber)
    {
      _currentLevel = levelNumber;
      UpdateUI();
    }

    void UpdateUI()
    {
      levelText.text = "Level: " + _currentLevel;
      movesText.text = "Moves: " + _remainingMoves;
    }

    public void ReduceMove()
    {
      _remainingMoves--;
      UpdateUI();
      CheckGameState();
    }

    void CheckGameState()
    {
      if (_remainingMoves <= 0)
      {
        Debug.Log("Fail State: Out of Moves");
        // Handle fail state
      }

      if (FindObjectsOfType<Block>().Length == 0)
      {
        Debug.Log("Win State: Level Completed");
        _currentLevel++;
        LevelManager.Instance.LoadLevel(_currentLevel);
      }
    }
  }
}