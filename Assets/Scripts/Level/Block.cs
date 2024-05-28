using UnityEngine;
using System.Collections.Generic;
using Game;

public class Block : MonoBehaviour
{
  public enum Direction
  {
    Up,
    Right,
    Down,
    Left
  }

  private List<Direction> _moveDirections;
  private readonly float _moveSpeed = 5f;
  private Vector3 _targetPosition;
  private bool _isMoving = false;
  private bool _isDragging = false;
  private Vector2 _startTouch, _swipeDelta;
  private static Block _selectedBlock = null;
  public static bool tap;
  private static bool _swipeLeft, _swipeRight, _swipeUp, _swipeDown;

  void Update()
  {
    HandleInput();
    CalculateSwipeDelta();
    CheckSwipeMagnitude();
    HandleSwipeInputs();
    MoveBlock();
  }

  public void InitializeBlock(List<int> directions)
  {
    _moveDirections = new List<Direction>();
    foreach (int dir in directions)
    {
      _moveDirections.Add((Direction)dir);
    }

    _targetPosition = transform.position;
  }

  private void CalculateSwipeDelta()
  {
    _swipeDelta = Vector2.zero;

    if (_isDragging)
    {
      if (Input.touches.Length > 0)
        _swipeDelta = Input.touches[0].position - _startTouch;
      else if (Input.GetMouseButton(0))
        _swipeDelta = (Vector2)Input.mousePosition - _startTouch;
    }
  }

  private void HandleSwipeInputs()
  {
    if (_isMoving || _selectedBlock != this) return;

    if (_swipeUp && _moveDirections.Contains(Direction.Up))
    {
      Move(Direction.Up);
    }
    else if (_swipeDown && _moveDirections.Contains(Direction.Down))
    {
      Move(Direction.Down);
    }
    else if (_swipeLeft && _moveDirections.Contains(Direction.Left))
    {
      Move(Direction.Left);
    }
    else if (_swipeRight && _moveDirections.Contains(Direction.Right))
    {
      Move(Direction.Right);
    }
  }

  private void CheckSwipeMagnitude()
  {
    if (_swipeDelta.magnitude > 70)
    {
      float x = _swipeDelta.x;
      float y = _swipeDelta.y;

      if (Mathf.Abs(x) > Mathf.Abs(y))
      {
        if (x < 0)
          _swipeLeft = true;
        else
          _swipeRight = true;
      }
      else
      {
        if (y < 0)
          _swipeDown = true;
        else
          _swipeUp = true;
      }

      Reset();
    }
  }

  private void HandleInput()
  {
    ResetFlags();

    if (Input.GetMouseButtonDown(0))
    {
      Vector2 touchPos = Input.mousePosition;
      if (IsTouchingBlock(touchPos))
      {
        _isDragging = true;
        _startTouch = touchPos;
        _selectedBlock = this;
      }
    }
    else if (Input.GetMouseButtonUp(0))
    {
      _isDragging = false;
      Reset();
    }
    else if (Input.touches.Length > 0)
    {
      if (Input.touches[0].phase == TouchPhase.Began)
      {
        Vector2 touchPos = Input.touches[0].position;
        if (IsTouchingBlock(touchPos))
        {
          _isDragging = true;
          _startTouch = touchPos;
          _selectedBlock = this;
        }
      }
      else if (Input.touches[0].phase == TouchPhase.Ended ||
               Input.touches[0].phase == TouchPhase.Canceled)
      {
        _isDragging = false;
        Reset();
      }
    }
  }

  private bool IsTouchingBlock(Vector2 touchPos)
  {
    Vector3 worldTouchPos = GameManager.Instance.mainCamera.ScreenToWorldPoint(touchPos);
    float halfWidth = transform.localScale.x / 2;
    float halfHeight = transform.localScale.y / 2;

    return worldTouchPos.x > transform.position.x - halfWidth &&
           worldTouchPos.x < transform.position.x + halfWidth &&
           worldTouchPos.y > transform.position.y - halfHeight &&
           worldTouchPos.y < transform.position.y + halfHeight;
  }

  private void Reset()
  {
    _startTouch = _swipeDelta = Vector2.zero;
    _isDragging = false;
  }

  private void ResetFlags()
  {
    _swipeLeft = _swipeRight = _swipeUp = _swipeDown = false;
  }

  private void Move(Direction direction)
  {
    if (_isMoving) return;

    switch (direction)
    {
      case Direction.Up:
        _targetPosition = transform.position + Vector3.up;
        Debug.Log("swipe up");
        break;
      case Direction.Right:
        _targetPosition = transform.position + Vector3.right;
        Debug.Log("swipe right");
        break;
      case Direction.Down:
        _targetPosition = transform.position + Vector3.down;
        Debug.Log("swipe down");
        break;
      case Direction.Left:
        _targetPosition = transform.position + Vector3.left;
        Debug.Log("swipe left");
        break;
    }

    _isMoving = true;
    //GameManager.Instance.ReduceMove(); // Uncomment if you have a move count manager
  }

  private void MoveBlock()
  {
    if (!_isMoving) return;

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