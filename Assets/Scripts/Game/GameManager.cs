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
    public List<Block> blocks { get; private set; } = new List<Block>();
    public List<Exit> exits { get; private set; } = new List<Exit>();
   

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