using UnityEngine;

namespace Game
{
  public interface ICameraModel
  {
    void AddCamera(string key, Camera cam);
    Camera GetCameraByKey(string key);
  }
}