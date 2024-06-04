using System.Collections.Generic;
using Core;
using Enum;
using UnityEngine;

namespace Game
{
  public class Block : MonoBehaviour
  {
    public GameObject explosionPrefab;
    private Rigidbody _rigidbody;

    public int blockLength;
    public float moveSpeed = 3.5f;

    private Vector2 _startTouch, _swipeDelta;
    private Vector3 _targetPosition;
    private Vector3 _currentPosition;
    private Vector3 _previousPosition;
    private Vector3 _nearestPosition;


    private bool _isMoving;
    private bool _isDragging;
    private static bool _swipeLeft, _swipeRight, _swipeUp, _swipeDown;
    private Color blockColor { get; set; }
    public Texture colorTexture { get; set; }
    private List<Direction> _moveDirections;
    private Direction _currentDirection;

    private static Block _selectedBlock;

    private void Awake()
    {
      _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
      _currentPosition = new Vector3();
      _selectedBlock = null;
      _isDragging = false;
      _isMoving = false;
      _nearestPosition = new Vector3();
      _previousPosition = transform.position;
    }

    void Update()
    {
      HandleInput();
      CalculateSwipeDelta();
      CheckSwipeMagnitude();
      HandleSwipeInputs();
      MoveBlock();
      if (!_isMoving)
      {
        _previousPosition = transform.position;
      }
    }

    public void InitializeBlock(List<int> directions, Color color,
      Texture texture, int length, Vector3 currentPosition)
    {
      _moveDirections = new List<Direction>();
      foreach (int dir in directions)
      {
        _moveDirections.Add((Direction)dir);
      }

      blockColor = color;
      GetComponent<Renderer>().material.mainTexture = texture;
      _targetPosition = transform.position;
      blockLength = length;
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
        StartMovement(Direction.Up);
      }
      else if (_swipeDown && _moveDirections.Contains(Direction.Down))
      {
        StartMovement(Direction.Down);
      }
      else if (_swipeLeft && _moveDirections.Contains(Direction.Left))
      {
        StartMovement(Direction.Left);
      }
      else if (_swipeRight && _moveDirections.Contains(Direction.Right))
      {
        StartMovement(Direction.Right);
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
      Vector3 worldTouchPos = GameManager.Instance.mainCamera
        .ScreenToWorldPoint(touchPos);
      worldTouchPos.z = transform.position.z;

      Vector3 blockLocalScale = transform.localScale;
      float halfWidth = blockLocalScale.x / 2;
      float halfHeight = blockLocalScale.y / 2;

      float widthAdjustment = 0;
      float heightAdjustment = 0;

      if (_moveDirections.Contains(Direction.Left) ||
          _moveDirections.Contains(Direction.Right))
      {
        widthAdjustment = (blockLength - 1) * blockLocalScale.x;
      }
      else if (_moveDirections.Contains(Direction.Up) ||
               _moveDirections.Contains(Direction.Down))
      {
        heightAdjustment = (blockLength - 1) * blockLocalScale.y;
      }

      Vector3 blockPosition = transform.position;

      return worldTouchPos.x > blockPosition.x - halfWidth &&
             worldTouchPos.x < blockPosition.x + halfWidth + widthAdjustment &&
             worldTouchPos.y > blockPosition.y - halfHeight - heightAdjustment &&
             worldTouchPos.y < blockPosition.y + halfHeight;
    }

    private void StartMovement(Direction direction)
    {
      _currentDirection = direction;

      switch (direction)
      {
        case Direction.Up:
          _targetPosition = transform.position + Vector3.up;
          break;
        case Direction.Right:
          _targetPosition = transform.position + Vector3.right;
          break;
        case Direction.Down:
          _targetPosition = transform.position + Vector3.down;
          break;
        case Direction.Left:
          _targetPosition = transform.position + Vector3.left;
          break;
      }

      if (_targetPosition != transform.position)
      {
        _isMoving = true;
      }
    }

    private void MoveBlock()
    {
      if (!_isMoving) return;
      _currentPosition = transform.position;
      transform.position = Vector3.MoveTowards(transform.position,
        _targetPosition, moveSpeed * Time.deltaTime);

      if (Vector3.Distance(transform.position, _targetPosition) < 0.1f)
      {
        transform.position = _targetPosition;
        Debug.Log("target Position: " + _targetPosition);


        StartMovement(_currentDirection);
      }
    }

    private void ReturnToNearestIntegerPosition()
    {
      if (blockLength == 2)
      {
        if (_moveDirections.Contains(Direction.Left) ||
            _moveDirections.Contains(Direction.Right))
        {
          float xStart = Mathf.Round(transform.position.x);
          Debug.Log("xStart: " + xStart);
          float xEnd = Mathf.Round(transform.position.x);
          Debug.Log("xEnd: " + xEnd);
          float y = Mathf.Round(transform.position.y);
          _nearestPosition = new Vector3((xStart + xEnd) / 2, y, transform.position.z);
        }
        else if (_moveDirections.Contains(Direction.Up) ||
                 _moveDirections.Contains(Direction.Down))
        {
          float yStart = Mathf.Round(transform.position.y);
          float yEnd = Mathf.Round(transform.position.y);
          float x = Mathf.Round(transform.position.x);
          _nearestPosition = new Vector3(x, (yStart + yEnd) / 2, transform.position.z);
        }
        else
        {
          _nearestPosition = new Vector3(Mathf.Round(transform.position.x),
            Mathf.Round(transform.position.y), transform.position.z);
        }
      }
      else
      {
        _nearestPosition = new Vector3(Mathf.Round(transform.position.x),
          Mathf.Round(transform.position.y), transform.position.z);
      }

      transform.position = _nearestPosition;
      _targetPosition = _nearestPosition;
      GameManager.Instance.CheckGameState();
      Debug.Log("Returning to nearest position: " + _nearestPosition);
    }

    private Vector3 GetDirectionVector(Direction direction)
    {
      switch (direction)
      {
        case Direction.Up:
          return Vector3.up;
        case Direction.Right:
          return Vector3.right;
        case Direction.Down:
          return Vector3.down;
        case Direction.Left:
          return Vector3.left;
        default:
          return Vector3.zero;
      }
    }

    private void TriggerExplosion(Direction collisionDirection)
    {
      if (explosionPrefab != null)
      {
        GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        ParticleSystem ps = explosion.GetComponent<ParticleSystem>();
        if (ps != null)
        {
          // Set the start color of the particle system to the block color
          var mainModule = ps.main;
          mainModule.startColor = blockColor;

          // Rotate the particle system based on the collision direction
          switch (collisionDirection)
          {
            case Direction.Up:
              explosion.transform.rotation = Quaternion.Euler(0, 0, 0);
              break;
            case Direction.Right:
              explosion.transform.rotation = Quaternion.Euler(0, 0, 90);
              break;
            case Direction.Down:
              explosion.transform.rotation = Quaternion.Euler(0, 0, 180);
              break;
            case Direction.Left:
              explosion.transform.rotation = Quaternion.Euler(0, 0, -90);
              break;
          }

          ps.Play();
          Destroy(explosion, ps.main.duration);
        }
      }
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

    private void OnTriggerEnter(Collider other)
    {
      Debug.Log($"Collision detected with: {other.gameObject.name}");

      if (other.CompareTag("Block"))
      {
        Debug.Log("Trigger collision with another block");
        _isMoving = false;
        ReturnToNearestIntegerPosition();
        Debug.Log("previous Position: " + _previousPosition);
        if (_nearestPosition != _previousPosition)
        {
          GameManager.Instance.ReduceMove();
        }
      }
      else if (other.CompareTag("Gate"))
      {
        Exit exit = other.GetComponent<Exit>();
        if (blockColor == exit.gateColor)
        {
          // TriggerExplosion();
          ReturnToNearestIntegerPosition();
          Debug.Log("previous Position: " + _previousPosition);
          GameManager.Instance.RemoveBlocks(this);
          if (_nearestPosition != _previousPosition)
          {
            GameManager.Instance.ReduceMove();
          }

          Destroy(gameObject);
        }
        else
        {
          _isMoving = false;
          ReturnToNearestIntegerPosition();
          if (_nearestPosition != _previousPosition)
          {
            GameManager.Instance.ReduceMove();
          }
        }
      }
    }

    private Vector3 RoundVector3(Vector3 vector)
    {
      return new Vector3(Mathf.Round(vector.x), Mathf.Round(vector.y), Mathf.Round(vector.z));
    }
  }
}