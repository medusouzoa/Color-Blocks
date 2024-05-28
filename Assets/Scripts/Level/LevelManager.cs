using System.Collections.Generic;
using Game;
using Newtonsoft.Json;
using UnityEngine;

namespace Level
{
  public class LevelManager : MonoBehaviour
  {
    public TextAsset[] levelFiles;
    public GameObject blockPrefab;
    public GameObject exitPrefab;
    private List<Level> _levels;
    public static LevelManager Instance { get; private set; }

    private void Awake()
    {
      _levels = new List<Level>();
      if (Instance == null)
      {
        Instance = this;
      }
      else
      {
        Destroy(gameObject);
      }
    }

    private void Start()
    {
      LoadLevels();
      LoadLevel(0);
    }

    private void LoadLevels()
    {
      foreach (TextAsset file in levelFiles)
      {
        Level level = JsonConvert.DeserializeObject<Level>(file.text);
        _levels.Add(level);
      }
    }

    public void LoadLevel(int index)
    {
      Level level = GetLevel(index);

      // Instantiate blocks
      foreach (MovableInfo blockInfo in level.movableInfo)
      {
        Quaternion rotation = DetermineRotation(blockInfo.direction);
        GameObject blockObject = Instantiate(blockPrefab, new Vector3(blockInfo.col,
          -blockInfo.row, 0), rotation);
        Block block = blockObject.GetComponent<Block>();
        block.InitializeBlock(blockInfo.direction);
        blockObject.GetComponent<Renderer>().material.color = GetColorFromInt(blockInfo.colors);
      }

      // Instantiate exits
      foreach (ExitInfo exitInfo in level.exitInfo)
      {
        // Calculate the adjusted position based on the direction
        Vector3 exitPosition = new Vector3(exitInfo.col, exitInfo.row, -0.7f);
        Quaternion exitRotation = Quaternion.identity;

        switch (exitInfo.direction)
        {
          case 0: // Up
            exitPosition.y = 1;
            exitRotation = Quaternion.Euler(90, 0, 0);
            break;
          case 1: // Right
            exitPosition.x = level.colCount;
            exitPosition.y -= 1;
            exitRotation = Quaternion.Euler(0, -90, -90);
            break;
          case 2: // Down
            exitPosition.y = -level.rowCount;
            exitRotation = Quaternion.Euler(90, 0, 0);
            break;
          case 3: // Left
            exitPosition.x = -1;
            exitPosition.y -= 1;
            exitRotation = Quaternion.Euler(0, -90, -90);
            break;
        }

        // Instantiate the exit object at the adjusted position with the correct rotation
        GameObject exitObject = Instantiate(exitPrefab, exitPosition, exitRotation);
        exitObject.GetComponent<Renderer>().material.color = GetColorFromInt(exitInfo.colors);
        Exit exit = exitObject.GetComponent<Exit>();
        exit.SetDirection((Exit.Direction)exitInfo.direction);
      }

      GameManager.Instance.SetMoveLimit(level.moveLimit);
      GameManager.Instance.SetLevelNumber(index + 1);
    }

    private static Quaternion DetermineRotation(ICollection<int> directions)
    {
      if (directions.Contains(0) && directions.Contains(2))
      {
        return Quaternion.Euler(0, 90, -90);
      }
      else if (directions.Contains(1) && directions.Contains(3))
      {
        return Quaternion.Euler(-90, 0, 0);
      }
      else
      {
        return Quaternion.identity;
      }
    }

    private Level GetLevel(int index)
    {
      return _levels[index];
    }

    private static Color GetColorFromInt(int color)
    {
      return color switch
      {
        0 => Color.red,
        1 => Color.green,
        2 => Color.blue,
        3 => Color.yellow,
        4 => new Color(0.5f, 0, 0.5f),
        _ => Color.white
      };
    }
  }
}