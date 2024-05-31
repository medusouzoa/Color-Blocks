using UnityEngine;

namespace ScriptableObjects
{
  public enum Direction
  {
    UpDown,
    RightLeft
  }

  [CreateAssetMenu(fileName = "TextureMapping", menuName = "ScriptableObjects/TextureMapping", order = 1)]
  public class TextureMapping : ScriptableObject
  {
    [System.Serializable]
    public class ColorTexturePair
    {
      public int colorValue;
      public Texture texture;
      public Direction direction;
    }

    public ColorTexturePair[] colorTexturePairs;

    public Texture GetTextureForColor(int colorValue, Direction direction)
    {
      foreach (ColorTexturePair pair in colorTexturePairs)
      {
        if (pair.colorValue == colorValue && pair.direction == direction)
        {
          return pair.texture;
        }
      }

      return null;
    }
  }
}