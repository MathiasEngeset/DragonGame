using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DragonGame1
{
    [Serializable]
    public class HighScore
    {
        //List that holds on to the best scores.
        private List<Score> HighScoreList;

        public HighScore() {
            HighScoreList = new List<Score>();
        }

        public void SetHighScore(List<Score> highScore) {
            HighScoreList = highScore;
        }

        public List<Score> GetHighScore() {
            return HighScoreList;
        }

    }

    [Serializable]
    public class Score {
        public string PlayerName { get; set; }
        public int Points { get; set; }
    }
}
