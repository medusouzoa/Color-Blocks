using Core;
using UnityEngine;

namespace CameraControl
{
  public class CameraController : MonoBehaviour
  {
    public UnityEngine.Camera mainCamera;
    public static CameraController Instance { get; private set; }

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
    public void AdjustCameraSize()
    {
      float targetSize = LevelManager.Instance.colSize + LevelManager.Instance.rowSize;
      float xPosition = (LevelManager.Instance.colSize - 1) * 0.5f;

      Vector3 newPosition = new(xPosition, transform.position.y, transform.position.z);
      transform.position = newPosition;
      mainCamera.orthographicSize = targetSize;
    }
  }
}