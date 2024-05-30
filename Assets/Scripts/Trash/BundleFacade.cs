using System;
using RSG;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Trash
{
  public class BundleFacade : MonoBehaviour
  {
    public static BundleFacade Instance { get; private set; }

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

    public IPromise Instantiate(string key, RuleTile.TilingRuleOutput.Transform parent)
    {
      Promise promise = new();
      AsyncOperationHandle<GameObject> asyncOperationHandle = Addressables.InstantiateAsync(key);
      asyncOperationHandle.Completed += handle =>
      {
        if (handle.Status != AsyncOperationStatus.Succeeded)
        {
          promise.Reject(new Exception("Panel Couldn't Created"));
        }
        else
        {
          promise.Resolve();
        }
      };
      return promise;
    }

    public IPromise<GameObject> InstantiateAndReturn(string key, Transform parent)
    {
      Promise<GameObject> promise = new();

      // Debug.LogWarning("InstantiateAndReturn>");

      AsyncOperationHandle<GameObject> asyncOperationHandle = Addressables.InstantiateAsync(key, parent);
      asyncOperationHandle.Completed += handle =>
      {
        if (handle.Status != AsyncOperationStatus.Succeeded)
        {
          promise.Reject(new Exception("Panel Couldn't Created"));
        }
        else
        {
          promise.Resolve(handle.Result);
        }
      };
      return promise;
    }
  }
}