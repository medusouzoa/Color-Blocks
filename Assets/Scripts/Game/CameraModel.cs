using System.Collections.Generic;
using UnityEngine;

namespace Game
{
  public class CameraModel :ICameraModel
  {
    public Dictionary<string, Camera> sceneCameras;

    public CameraModel()
    {
      sceneCameras = new Dictionary<string, Camera>();
    }

    public void AddCamera(string key, Camera cam)
    {
      if (!sceneCameras.ContainsKey(key))
      {
        sceneCameras.Add(key, cam);
      }
      else
      {
        Debug.LogWarning($"Camera with key '{key}' already exists in the model.");
      }
    }

    public Camera GetCameraByKey(string key)
    {
      if (sceneCameras.TryGetValue(key, out Camera camera))
      {
        return camera;
      }
      else
      {
        Debug.LogWarning($"Camera with key '{key}' not found in the model.");
        return null;
      }
    }
  }
}