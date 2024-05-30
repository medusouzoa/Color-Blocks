using Level;
using UnityEngine;

namespace Game
{
  public class CameraController : MonoBehaviour
  {
    private Camera _camera;
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

    void Start()
    {
      _camera = GetComponent<Camera>();
    }

    public void AdjustCameraSize()
    {
      float targetSize = LevelManager.Instance.colSize + LevelManager.Instance.rowSize;
      float xPosition = (LevelManager.Instance.colSize - 1) * 0.5f;

      Vector3 newPosition = new(xPosition, transform.position.y, transform.position.z);
      transform.position = newPosition;
      _camera.orthographicSize = targetSize;
    }
  }
}