using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrabbleGamePage
{
    class Player
    {
        public string CurrentScore { get { return this.Score.ToString(); } private set {  } }
        public int Score { get; set; }
        public string playerName;

        public Player(string name="")
        {
            Score = 0;

            playerName = name;

        }
        
        public void addToScore(int score)
        {
            Score += score;
        }

    }
}
