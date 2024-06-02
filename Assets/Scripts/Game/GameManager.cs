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


    public Camera mainCamera;
    private int _currentLevel = 1;
    private int _remainingMoves;
    public bool isGameOver;
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

    private void Start()
    {
      isGameOver = false;
    }

    private void Update()
    {
      if (_remainingMoves == 0)
      {
        UIController.instance.UpdateMoveText("");
      }
    }

    public void ReplayLevel()
    {
      UIController.instance.OnReplayLevel();
      LevelManager.Instance.LoadLevel(_currentLevel - 1);
    }

    public void SetMoveLimit(int moveLimit)
    {
      _remainingMoves = moveLimit;
      UIController.instance.UpdateUI(_currentLevel, _remainingMoves);
    }

    public void SetLevelNumber(int levelNumber)
    {
      _currentLevel = levelNumber;
      UIController.instance.UpdateUI(_currentLevel, _remainingMoves);
    }


    public void ReduceMove()
    {
      if (_remainingMoves > 0)
      {
        _remainingMoves--;
        UIController.instance.UpdateUI(_currentLevel, _remainingMoves);
        CheckGameState();
        if (_remainingMoves == 0)
        {
          isGameOver = true;
          // Handle game over or level restart logic here
          LevelManager.Instance.ClearAllChildObjects();
          DestroyAllBlocks();
          DestroyAllExits();
          UIController.instance.OnOpenGameOverPanel();
          Debug.Log("No more moves left!");
        }
      }

      LevelCompletionCheck();
    }

    void CheckGameState()
    {
      if (_remainingMoves <= 0)
      {
        isGameOver = true;
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
      if (blocks.Count == 0 && !isGameOver)
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

    public void DestroyAllBlocks()
    {
      foreach (Block block in blocks)
      {
        if (block != null)
        {
          Destroy(block.gameObject);
        }
      }

      blocks.Clear();
    }
  }
}