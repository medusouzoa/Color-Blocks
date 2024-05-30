using System;
using System.Collections.Generic;
using Level;
using RSG;
using Trash;
using UnityEngine;

public class GridViewManager : MonoBehaviour
{
  public int x { get; set; }
  public int y { get; set; }
  public List<List<GridVo>> grids { get; private set; }

  public static GridViewManager Instance { get; private set; }

  private void Awake()
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

  public void FillGridData()
  {
    int col = LevelManager.Instance.colSize;
    int row = LevelManager.Instance.rowSize;
    Debug.Log("col in Grid: "+col);
    grids = new List<List<GridVo>>();
    for (int i = 0; i < row; i++)
    {
      grids.Add(new List<GridVo>());
      for (int j = 0; j < col; j++)
      {
        List<GridVo> gridVos = grids[i];
        gridVos.Add(new GridVo
        {
          x = i,
          y = j
        });
      }
    }
  }

  public void CreateGrids()
  {
    List<Func<IPromise>> promises = new();

    // Fill promises with functions that instantiate grids

    for (int i = 0; i < grids.Count; i++)
    {
      List<GridVo> gridVos = grids[i];
      for (int j = 0; j < gridVos.Count; j++)
      {
        GridVo gridVo = gridVos[j];
        promises.Add(() => InstantiateGrid(gridVo.x, gridVo.y));
      }
    }

    Promise.Sequence(promises)
      .Then(() => Debug.Log("All grids created"))
      .Catch(exception => Debug.LogError(exception));
  }

  private IPromise InstantiateGrid(int i, int j)
  {
    return BundleFacade.Instance.InstantiateAndReturn("Grid", transform)
      .Then(result =>
      {
        result.transform.name = $"grid_{i}_{j}";
        x = i;
        y = j;
      });
  }
}