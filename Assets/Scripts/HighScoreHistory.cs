
using System.Collections.Generic;

public class HighScoreHistory
{
    public List<HighScore> highScores = new List<HighScore>();

    public List<string> GetAllNames()
    {
        List<string> result = new List<string>();
        foreach (HighScore h in highScores)
            result.Add(h.playerName);
        return (result);
    }

    public HighScore GetHighScore(string name)
    {
        return highScores.Find(h => h.playerName == name);
    }

    public bool AddName(string name)
    {
        if (GetHighScore(name) != null)
            return false;
        highScores.Add(new HighScore() { playerName = name });
        return true;
    }

    public void PlayerHasWon(string name)
    {
        HighScore score = GetHighScore(name);
        if (score != null)
            score.highscore++;
    }

}
