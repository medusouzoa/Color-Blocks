using System.Collections.Generic;
using Game;
using UI;
using UnityEngine;

namespace Core
{
  public class GameManager : MonoBehaviour
  {
    public static GameManager Instance { get; private set; }


    public Camera mainCamera;
    private int _currentLevel = 1;
    private int _remainingMoves;
    public bool isGameOver;
    private int _levelCount;
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
        GameUIController.instance.UpdateMoveText("");
      }
    }

    public void ReplayLevel()
    {
      GameUIController.instance.OnReplayLevel();
      LevelManager.Instance.LoadLevel(_currentLevel - 1);
    }

    public void SetMoveLimit(int moveLimit)
    {
      _remainingMoves = moveLimit;
      GameUIController.instance.UpdateUI(_currentLevel, _remainingMoves);
    }

    public void SetLevelNumber(int levelNumber)
    {
      _currentLevel = levelNumber;
      GameUIController.instance.UpdateUI(_currentLevel, _remainingMoves);
    }


    public void ReduceMove()
    {
      if (_remainingMoves > 0)
      {
        _remainingMoves--;
        GameUIController.instance.UpdateUI(_currentLevel, _remainingMoves);
        CheckGameState();
        if (_remainingMoves == 0)
        {
          CheckGameState();
          isGameOver = true;
          DestroyAllBlocks();
          DestroyAllExits();
          LevelManager.Instance.ClearAllChildObjects();
          GameUIController.instance.OnOpenGameOverPanel();
          Debug.Log("No more moves left!");
        }
      }
      else
      {
        CheckGameState();
      }
    }

    public void CheckGameState()
    {
      if (blocks.Count == 0)
      {
        Debug.Log("Win State: Level Completed");
        blocks.Clear();
        DestroyAllExits();
        LevelCompletionCheck();
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
        LevelManager.Instance.ClearAllChildObjects();
        if (_levelCount == _currentLevel)
        {
          GameUIController.instance.OnLoadEndGameScene();
        }
        else
        {
          GameUIController.instance.SetLevelText("Next Level: " + _currentLevel);
          GameUIController.instance.OnOpenWinPanel();
        }
      }
    }

    public void OnLoadNextLevel()
    {
      GameUIController.instance.OnOpenNextLevelPanel();
      LevelManager.Instance.LoadLevel(_currentLevel - 1);
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

    public void SetLevelCount(int count)
    {
      _levelCount = count;
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