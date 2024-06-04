using Enum;
using UnityEngine;

namespace Game
{
  public class Exit : MonoBehaviour
  {
  
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