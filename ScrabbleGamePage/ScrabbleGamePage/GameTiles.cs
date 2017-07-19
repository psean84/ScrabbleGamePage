using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Principal;


namespace ScrabbleGamePage
{
    class GameTiles
    {
        private const int ROW = 15;
        private const int COL = 15;

        private static char[] Tiles = new char[100] {
                                        'A', 'A', 'A', 'A', 'A', 'A', 'A', 'A', 'A', 'B',
                                        'B', 'C', 'C', 'D', 'D', 'D', 'D', 'E', 'E', 'E',
                                        'E', 'E', 'E', 'E', 'E', 'E', 'E', 'E', 'E', 'F',
                                        'F', 'G', 'G', 'G', 'H', 'H', 'I', 'I', 'I', 'I',
                                        'I', 'I', 'I', 'I', 'I', 'J', 'K', 'L', 'L', 'L',
                                        'L', 'M', 'M', 'N', 'N', 'N', 'N', 'N', 'N', 'O',
                                        'O', 'O', 'O', 'O', 'O', 'O', 'O', 'P', 'P', 'Q',
                                        'R', 'R', 'R', 'R', 'R', 'R', 'S', 'S', 'S', 'S',
                                        'T', 'T', 'T', 'T', 'T', 'T', 'U', 'U', 'U', 'U',
                                        'V', 'V', 'W', 'W', 'X', 'Y', 'Y', 'Z', ' ', ' '
        };

        private static readonly Dictionary<int, List<char>> tileValueDictionary = new Dictionary<int, List<char>>() {
            { 1,  new List<char>() { 'A', 'E', 'I', 'O', 'U', 'N', 'R','T','L','S' } },
            { 2,  new List<char>() { 'D', 'G' } },
            { 3,  new List<char>() { 'B', 'C', 'M', 'P' } },
            { 4,  new List<char>() { 'F', 'H', 'V', 'W', 'Y' } },
            { 5,  new List<char>() { 'K' } },
            { 8,  new List<char>() { 'J', 'X' } },
            { 10, new List<char>() { 'Q', 'Z' } },
        };

        bool _isFirstMove;

        private readonly List<int> TWList = new List<int> { 0, 7, 14, 105, 119, 210, 217, 224 };
        private readonly List<int> TLList = new List<int> { 20, 24, 76, 80, 84, 88, 136, 140, 144, 148, 200, 204 };
        private readonly List<int> DLList = new List<int> { 3, 11, 36, 38, 45, 52, 59, 92, 96, 98, 102, 108, 116, 122, 126, 128, 132, 165, 172, 179, 186, 188, 213, 221 };
        private readonly List<int> DWList = new List<int> { 16, 28, 32, 42, 48, 56, 64, 70, 154, 160, 168, 176, 182, 192, 196, 208 };
        private List<int> ListBlankTiles;
        private char[,] BoardCharacters;

        public static List<char> randomIndexList;

        public List<string> PlayedWordList { get; private set; }

        public void NewGame()
        {
            _isFirstMove = true;
            randomIndexList = Tiles.ToList();
            PlayedWordList = new List<string>();
            BoardCharacters = new char[ROW, COL];
            ListBlankTiles = new List<int>();
            Shuffle();
        }

        public string getRemainingNumberOfTiles()
        {
            return randomIndexList.Count.ToString();
        }
        
        private int getTIleValue(char c)
        {
            int value = 0;

            foreach(var val in tileValueDictionary)
            {
                if (val.Value.Contains(c))
                {
                    value = val.Key;
                    break;
                }
            }

            return value;
        }

        private void Shuffle()
        {
            randomIndexList = randomIndexList.OrderBy(g => Guid.NewGuid()).ToList();
        }

        public char GetNewTile()
        {
            char ch = randomIndexList.First();
            randomIndexList.RemoveAt(0);
            Shuffle();
            return ch;
        }

        public bool tilesCountToZero()
        {
            return (randomIndexList.Count == 0);
        }

        public void setBoardCharacter(int row, int column, char character)
        {
            BoardCharacters[row, column] = character;
        }

        public void addUsedWord(string word)
        {
            PlayedWordList.Add(word);
        }

        public void setBlankTile(int index)
        {
            ListBlankTiles.Add(index);
        }

        public int calculatePlayerScore(string playerWord, ComputerPlayer.WordOrientation orientation, int startIndex)
        {
            int score = 0;

            try
            {
                int index = startIndex;
                bool DW = false;
                bool TW = false;

                int multiplicationFactor = 0;

                foreach (char c in playerWord.ToCharArray())
                {

                    int tileValue = 0;

                    if (ListBlankTiles.Contains(index))
                        tileValue = 0;
                    else
                    {
                        foreach (var it in tileValueDictionary)
                        {
                            if (it.Value.Contains(char.ToUpper(c)))
                            {
                                tileValue = it.Key;
                                break;
                            }
                        }
                    }

                    if (TWList.Contains(index))
                    {
                        TW = true;
                        multiplicationFactor++;
                    }
                    if (_isFirstMove)
                    {
                        _isFirstMove = false;
                        DW = true;
                    }

                    if (DWList.Contains(index))
                    {
                        DW = true;
                        multiplicationFactor++;
                    }



                    if (TLList.Contains(index))
                        score = score + (tileValue * 3);
                    else if (DLList.Contains(index))
                        score = score + (tileValue * 2);
                    else
                        score = score + tileValue;

                    if (orientation == ComputerPlayer.WordOrientation.Horizontal)
                        index++;
                    else
                        index += 15;
                }

                if (TW)
                    score = score * 3 * multiplicationFactor;

                if (DW)
                    score = score * 2 * multiplicationFactor;

            }
            catch (Exception ex)
            {

            }

            return score;

        }

        public char[] getBoardCharacterByColumn(int columnNumber)
        {
            char[] bc = new char[15];

            for (int i = 0; i < 15; i++)
                bc[i] = char.ToLower(BoardCharacters[i, columnNumber]);

            return bc;
        }

        public char[] getBoardCharacterByRow(int rowNumber)
        {
            char[] bc = new char[15];

            for (int i = 0; i < 15; i++)
                bc[i] = char.ToLower(BoardCharacters[rowNumber, i]);

            return bc;
        }

        public void removeBoardCharacter(int r,int c)
        {
            BoardCharacters[r, c] = '\0';
        }

        public bool isBoardTileEmpty(int r, int c)
        {
            return BoardCharacters[r, c] == '\0';
        }
    }

}
