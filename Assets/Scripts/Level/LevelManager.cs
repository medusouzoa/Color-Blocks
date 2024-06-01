using System.Collections.Generic;
using Game;
using Newtonsoft.Json;
using ScriptableObjects;
using UnityEngine;

namespace Level
{
  public class LevelManager : MonoBehaviour
  {
    public TextAsset[] levelFiles;
    public GameObject block1Prefab;
    public GameObject block2Prefab;
    public GameObject exitPrefab;
    public GameObject gridTilePrefab;
    public TextureMapping textureMappingOne;
    public TextureMapping textureMappingTwo;
    public Transform parent;

    private Dictionary<int, Level> _levels;

    public int colSize { get; private set; }
    public int rowSize { get; private set; }
    public static LevelManager Instance { get; private set; }

    private void Awake()
    {
      _levels = new Dictionary<int, Level>();
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
      LoadLevel(3);
    }

    public void LoadLevel(int index)
    {
      if (!_levels.ContainsKey(index))
      {
        LoadLevelFile(index);
      }

      Level level = _levels[index];
      SetGridDimensions(level);
      GenerateGrid();
      CameraController.Instance.AdjustCameraSize();

      LoadBlocks(level.movableInfo);
      LoadExits(level.exitInfo);

      GameManager.Instance.SetMoveLimit(level.moveLimit);
      GameManager.Instance.SetLevelNumber(index + 1);
    }

    private void LoadLevelFile(int index)
    {
      if (index < 0 || index >= levelFiles.Length)
      {
        Debug.LogError($"Invalid level index: {index}");
        return;
      }

      Level level = JsonConvert.DeserializeObject<Level>(levelFiles[index].text);
      _levels[index] = level;
    }


    private void SetGridDimensions(Level level)
    {
      rowSize = level.rowCount;
      colSize = level.colCount;
      Debug.Log($"rowSize: {rowSize}, colSize: {colSize}");
    }

    private void LoadBlocks(List<MovableInfo> movableInfos)
    {
      foreach (MovableInfo blockInfo in movableInfos)
      {
        Quaternion rotation = DetermineBlockRotation(blockInfo.direction);
        GameObject blockObject = InstantiateBlock(blockInfo, rotation);
        Block block = blockObject.GetComponent<Block>();
        Vector3 position = new(blockInfo.col, -blockInfo.row, 0);
        DoubleDirection direction = GetDirection(blockInfo.direction);

        Texture blockTexture = GetBlockTexture(blockInfo, direction);
        block.InitializeBlock(blockInfo.direction, GetColorFromInt(blockInfo.colors),
          blockTexture, blockInfo.length, position);

        GameManager.Instance.AddBlock(block);
      }
    }

    private DoubleDirection GetDirection(List<int> directions)
    {
      if (directions.Contains(0) && directions.Contains(2))
      {
        return DoubleDirection.UpDown;
      }

      if (directions.Contains(1) && directions.Contains(3))
      {
        return DoubleDirection.RightLeft;
      }

      return DoubleDirection.None;
    }

    private Texture GetBlockTexture(MovableInfo blockInfo, DoubleDirection direction)
    {
      if (blockInfo.length == 1)
      {
        return textureMappingOne.GetTextureForColor(blockInfo.colors, direction);
      }
      else if (blockInfo.length == 2)
      {
        return textureMappingTwo.GetTextureForColor(blockInfo.colors, direction);
      }

      return null;
    }

    private void LoadExits(List<ExitInfo> exitInfos)
    {
      foreach (ExitInfo exitInfo in exitInfos)
      {
        Vector3 exitPosition = CalculateExitPosition(exitInfo);
        Quaternion exitRotation = CalculateExitRotation(exitInfo.direction);

        GameObject exitObject = Instantiate(exitPrefab, exitPosition, exitRotation);
        exitObject.transform.parent = parent.transform;
        exitObject.GetComponent<Renderer>().material.color = GetColorFromInt(exitInfo.colors);
        Exit exit = exitObject.GetComponent<Exit>();
        exit.SetColor(GetColorFromInt(exitInfo.colors));
        exit.SetDirection((Direction)exitInfo.direction);
        GameManager.Instance.AddExit(exit);
      }
    }

    private Vector3 CalculateExitPosition(ExitInfo exitInfo)
    {
      float x = exitInfo.col;
      float y = -exitInfo.row;
      float z = -0.7f;

      switch (exitInfo.direction)
      {
        case 0: // Up
          y = 0.75f;
          break;
        case 1: // Right
          x = colSize - 0.25f;
          break;
        case 2: // Down
          y = -rowSize + 0.25f;
          break;
        case 3: // Left
          x = -0.75f;
          break;
      }

      return new Vector3(x, y, z);
    }

    private Quaternion CalculateExitRotation(int direction)
    {
      switch (direction)
      {
        case 0: // Up
          return Quaternion.Euler(90, 0, 0);
        case 1: // Right
          return Quaternion.Euler(0, -90, -90);
        case 2: // Down
          return Quaternion.Euler(90, 0, 0);
        case 3: // Left
          return Quaternion.Euler(0, -90, -90);
        default:
          return Quaternion.identity;
      }
    }

    private void GenerateGrid()
    {
      foreach (Transform child in transform)
      {
        Destroy(child.gameObject);
      }

      for (int row = 0; row < rowSize; row++)
      {
        for (int col = 0; col < colSize; col++)
        {
          Vector3 position = new(col, -row, 1);
          Instantiate(gridTilePrefab, position, Quaternion.identity, transform);
        }
      }
    }

    private GameObject InstantiateBlock(MovableInfo blockInfo, Quaternion rotation)
    {
      GameObject blockPrefab = blockInfo.length == 1 ? block1Prefab : block2Prefab;

      GameObject blockObject = Instantiate(blockPrefab,
        new Vector3(blockInfo.col, -blockInfo.row, 0), rotation);
      blockObject.transform.parent = parent.transform;
      return blockObject;
    }


    private static Quaternion DetermineBlockRotation(ICollection<int> directions)
    {
      if (directions.Contains(0) && directions.Contains(2))
      {
        return Quaternion.Euler(0, 90, -90);
      }

      if (directions.Contains(1) && directions.Contains(3))
      {
        return Quaternion.Euler(-90, 0, 0);
      }


      return Quaternion.identity;
    }

    private Level GetLevel(int index)
    {
      return _levels.TryGetValue(index, out Level level) ? level : null;
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