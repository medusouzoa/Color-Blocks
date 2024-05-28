using System.Collections.Generic;

namespace Level
{
  [System.Serializable]
  public class Level
  {
    public int moveLimit;
    public int rowCount;
    public int colCount;
    public List<CellInfo> cellInfo;
    public List<MovableInfo> movableInfo;
    public List<ExitInfo> exitInfo;
  }

  [System.Serializable]
  public class CellInfo
  {
    public int row;
    public int col;
  }

  [System.Serializable]
  public class MovableInfo
  {
    public int row;
    public int col;
    public List<int> direction;
    public int length;
    public int colors;
  }

  [System.Serializable]
  public class ExitInfo
  {
    public int row;
    public int col;
    public int direction;
    public int colors;
  }
}