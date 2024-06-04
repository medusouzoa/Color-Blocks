using System;
using System.Linq;
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
      return (from pair in colorTexturePairs
        where
          pair.colorValue == colorValue && pair.direction == direction
        select
          pair.texture).FirstOrDefault();
    }
  }
}