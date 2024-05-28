using UnityEngine;

namespace Level
{
  public class Exit : MonoBehaviour
  {
    public enum Direction
    {
      Up,
      Right,
      Down,
      Left
    }

    private Direction _exitDirection;

    public void SetDirection(Direction direction)
    {
      _exitDirection = direction;
    }
  }
}