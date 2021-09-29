
using System.Collections.Generic;

public class HighScore
{
    public string playerName = "";
    public int highscore = 0;
    public bool isVirtual = false;

    public class NameComparer : IComparer<HighScore>
    {
        public int Compare(HighScore x, HighScore y)
        {
            return x.playerName.CompareTo(y.playerName);
        }
    }

    public class ScoreComparer : IComparer<HighScore>
    {
        public int Compare(HighScore x, HighScore y)
        {
            return -1 * x.highscore.CompareTo(y.highscore);
        }
    }

}
