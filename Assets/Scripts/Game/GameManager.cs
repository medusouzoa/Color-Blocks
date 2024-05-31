using System;
using System.Collections.Generic;
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
    public Camera mainCamera;
    private int _currentLevel = 1;
    private int _remainingMoves;
    public List<Block> blocks { get; private set; }
    public List<Exit> exits { get; private set; }


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

      blocks = new List<Block>();
      exits = new List<Exit>();
    }

    private void Update()
    {
      if (_remainingMoves == 0)
      {
        movesText.text = "";
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
      if (_remainingMoves > 0)
      {
        _remainingMoves--;
        UpdateUI();
        CheckGameState();
        if (_remainingMoves == 0)
        {
          // Handle game over or level restart logic here
          Debug.Log("No more moves left!");
        }
      }

      LevelCompletionCheck();
    }

    void CheckGameState()
    {
      if (_remainingMoves <= 0)
      {
        Debug.Log("Fail State: Out of Moves");
        // Handle fail state
      }

      if (blocks.Count == 0)
      {
        Debug.Log("Win State: Level Completed");
        _currentLevel++;
        blocks.Clear();
        DestroyAllExits();
        LevelManager.Instance.LoadLevel(_currentLevel - 1);
      }
    }

    void LevelCompletionCheck()
    {
      if (blocks.Count == 0)
      {
        Debug.Log("Win State: Level Completed");
        _currentLevel++;
        blocks.Clear();
        DestroyAllExits();
        LevelManager.Instance.LoadLevel(_currentLevel - 1);
      }
    }

    public void AddBlock(Block block)
    {
      blocks.Add(block);
    }

    public void RemoveBlocks(Block block)
    {
      blocks.Remove(block);
    }

    public void AddExit(Exit exit)
    {
      exits.Add(exit);
    }

    public void DestroyAllExits()
    {
      foreach (Exit exit in exits)
      {
        if (exit != null)
        {
          Destroy(exit.gameObject);
        }
      }

      exits.Clear();
    }
  }
}