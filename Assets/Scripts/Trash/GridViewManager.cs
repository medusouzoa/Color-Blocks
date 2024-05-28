using System;
using System.Collections.Generic;
using RSG;
using UnityEngine;

public class GridViewManager : MonoBehaviour
{
  public int x { get; set; }
  public int y { get; set; }

  private void Awake()
  {
    // CreateGrids();
  }

  public void CreateGrids(List<List<GridVo>> gridData, BundleFacade bundleFacade)
  {
    List<Func<IPromise>> promises = new();

    // Fill promises with functions that instantiate grids

    for (int i = 0; i < gridData.Count; i++)
    {
      List<GridVo> gridVos = gridData[i];
      for (int j = 0; j < gridVos.Count; j++)
      {
        GridVo gridVo = gridVos[j];
        promises.Add(() => InstantiateGrid(bundleFacade, gridVo.x, gridVo.y));
      }
    }

    Promise.Sequence(promises)
      .Then(() => Debug.Log("All grids created"))
      .Catch(exception => Debug.LogError(exception));
  }

  private IPromise InstantiateGrid(BundleFacade bundleFacade, int i, int j)
  {
    return bundleFacade.InstantiateAndReturn("Grid", transform)
      .Then(result =>
      {
        result.transform.name = $"grid_{i}_{j}";
        x = i;
        y = j;
      });
  }
}