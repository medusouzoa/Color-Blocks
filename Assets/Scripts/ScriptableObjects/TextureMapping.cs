using System;
using Enum;
using UnityEngine;

namespace ScriptableObjects
{
  [CreateAssetMenu(fileName = "TextureMapping", menuName = "ScriptableObjects/TextureMapping", order = 1)]
  public class TextureMapping : ScriptableObject
  {
    [Serializable]
    public class ColorTexturePair
    {
      public int colorValue;
      public Texture texture;
      public DoubleDirection direction;
    }

    public ColorTexturePair[] colorTexturePairs;

    public Texture GetTextureForColor(int colorValue, DoubleDirection direction)
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