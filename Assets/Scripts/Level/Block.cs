using UnityEngine;
using System.Collections.Generic;

public class Block : MonoBehaviour
{
  public enum Direction { Up, Right, Down, Left }
  private List<Direction> _moveDirections;
  private readonly float _moveSpeed = 5f;
  private Vector3 _targetPosition;
  private bool _isMoving = false;

  void Update()
  {
    if (_isMoving)
    {
      MoveBlock();
    }
  }

  public void InitializeBlock(List<int> directions)
  {
    _moveDirections = new List<Direction>();
    foreach (int dir in directions)
    {
      _moveDirections.Add((Direction)dir);
    }
  }

  public void Move(Direction direction)
  {
    if (!_isMoving && _moveDirections.Contains(direction))
    {
      switch (direction)
      {
        case Direction.Up:
          _targetPosition = transform.position + Vector3.forward; // Assuming z-axis for up
          break;
        case Direction.Right:
          _targetPosition = transform.position + Vector3.right;
          break;
        case Direction.Down:
          _targetPosition = transform.position + Vector3.back; // Assuming z-axis for down
          break;
        case Direction.Left:
          _targetPosition = transform.position + Vector3.left;
          break;
      }
      _isMoving = true;
    }
  }

  void MoveBlock()
  {
    transform.position = Vector3.MoveTowards(transform.position, _targetPosition, _moveSpeed * Time.deltaTime);
    if (transform.position == _targetPosition)
    {
      _isMoving = false;
      CheckForExit();
    }
  }

  void CheckForExit()
  {
    // Implement the logic to check if the block is at the correct exit and handle destruction
  }
}