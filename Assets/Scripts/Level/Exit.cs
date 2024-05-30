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
    public Color gateColor { get; private set; }
    public void SetDirection(Direction direction)
    {
      _exitDirection = direction;
    }
    public void SetColor(Color color)
    {
      gateColor = color;
    }
  }
}