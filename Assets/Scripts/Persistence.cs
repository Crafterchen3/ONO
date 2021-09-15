using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Persistence
{
    private Game game;

    const string HighscoresKey = "Highscores";

    public Persistence(Game game)
    {
        this.game = game;
    }

    public void SaveHighScores()
    {
        if (game.highScoreHistory != null)
        {
            var writer = new System.Xml.Serialization.XmlSerializer(typeof(HighScoreHistory));
            var wfile = new StringWriter();
            writer.Serialize(wfile, game.highScoreHistory);
            wfile.Close();
            PlayerPrefs.SetString(HighscoresKey, wfile.ToString());
            PlayerPrefs.Save();
        }
    }

    public HighScoreHistory LoadHighScores()
    {
        if (PlayerPrefs.HasKey(HighscoresKey))
        {
            System.Xml.Serialization.XmlSerializer reader =  new System.Xml.Serialization.XmlSerializer(typeof(HighScoreHistory));
            StringReader file = new StringReader(PlayerPrefs.GetString(HighscoresKey));
            HighScoreHistory instance = (HighScoreHistory)reader.Deserialize(file);
            file.Close();

            return instance;
        }
        else
            return new HighScoreHistory();
    }

}
