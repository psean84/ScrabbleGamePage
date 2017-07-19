using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;

namespace ScrabbleGamePage
{
    class ComputerPlayer
    {
        private const int ROW = 15;

        private const int COL = 15;

        public List<string> englishWords { get; set; }

        private List<string> validWords;

        private char[] PlayerCharacter;

        private char[,] BoardCharacters;

        private List<string> PnCofCharacters;

        public WordAtBoard ComputerWord { get; private set; }

        public bool isComputerWordNull;

        private bool blankTileEncountered;

        public enum WordOrientation { Horizontal, Vertical };

        public struct WordStartIndex { public int Row; public int Column; }

        public struct WordAtBoard { public WordOrientation Orientation; public WordStartIndex Index; public string Word; }

        public int RequirednumberOfTiles { get; private set; }

        private List<string> usedWords;

        public bool FirstMove { get; set; }

        public ComputerPlayer()
        {
            BoardCharacters = new char[ROW, COL];

            PlayerCharacter = new char[7];

            setRequiredNumberOfTiles();

            usedWords = new List<string>();
        }


        public void InsertBoardCharacter(int Row, int Column, char Character)
        {
            BoardCharacters[Row, Column] = Character;
        }

        public void AddPlayerCharacter(char character)
        {
            int i = 0;
            for (; i <=6 ; i++)
            {
                if (PlayerCharacter[i] == '\0')
                    break;
            }

            
            PlayerCharacter[i] = char.ToLower(character);           
            
        }

        public void ComputePnC()
        {

            PnCofCharacters = new List<string>();
            
            blankTileEncountered = PlayerCharacter.ToList().Contains(' ');

            if (!blankTileEncountered)
            {

                #region Single Character List

                Dictionary<int, string> DictForSingleCharacter = new Dictionary<int, string>();

                for (int i = 0; i < 7; i++)
                {
                    DictForSingleCharacter.Add(i, PlayerCharacter[i].ToString());
                }
                PnCofCharacters.AddRange(DictForSingleCharacter.Values);

                #endregion

                #region  Two character's list

                Dictionary<List<int>, string> DictForTwoCharacter = new Dictionary<List<int>, string>();

                for (int si = 0; si < 7; si++)  //for selected Index loop
                {
                    for (int i = 0; i < 7; i++)
                    {
                        if (si == i)
                            continue;
                        string str = PlayerCharacter[si].ToString() + PlayerCharacter[i];
                        DictForTwoCharacter.Add(new List<int>() { si, i }, str);
                    }
                }

                PnCofCharacters.AddRange(DictForTwoCharacter.Values);

                #endregion

                #region Three Charecter's List

                Dictionary<List<int>, string> DictForThreeCharacter = new Dictionary<List<int>, string>();

                foreach (var item in DictForTwoCharacter)
                {
                    for (int i = 0; i < 7; i++)
                    {
                        if (item.Key.Contains(i))
                            continue;

                        string str = item.Value + PlayerCharacter[i];

                        List<int> temp = new List<int>();
                        temp.AddRange(item.Key);
                        temp.Add(i);

                        DictForThreeCharacter.Add(temp, str);

                    }
                }

                PnCofCharacters.AddRange(DictForThreeCharacter.Values);


                #endregion

                #region Four Charecter's List

                Dictionary<List<int>, string> DictForFourCharacter = new Dictionary<List<int>, string>();


                foreach (var item in DictForThreeCharacter)
                {
                    for (int i = 0; i < 7; i++)
                    {
                        if (item.Key.Contains(i))
                            continue;

                        string str = item.Value + PlayerCharacter[i];

                        List<int> temp = new List<int>();
                        temp.AddRange(item.Key);
                        temp.Add(i);

                        DictForFourCharacter.Add(temp, str);

                    }
                }
                PnCofCharacters.AddRange(DictForFourCharacter.Values);

                #endregion

                #region Five Charecter's List

                Dictionary<List<int>, string> DictForFiveCharacter = new Dictionary<List<int>, string>();

                foreach (var item in DictForFourCharacter)
                {
                    for (int i = 0; i < 7; i++)
                    {
                        if (item.Key.Contains(i))
                            continue;

                        string str = item.Value + PlayerCharacter[i];

                        List<int> temp = new List<int>();
                        temp.AddRange(item.Key);
                        temp.Add(i);

                        DictForFiveCharacter.Add(temp, str);

                    }
                }

                PnCofCharacters.AddRange(DictForFiveCharacter.Values);

                #endregion

                #region Six Charecter's List

                Dictionary<List<int>, string> DictForSixCharacter = new Dictionary<List<int>, string>();

                foreach (var item in DictForFiveCharacter)
                {
                    for (int i = 0; i < 7; i++)
                    {
                        if (item.Key.Contains(i))
                            continue;

                        string str = item.Value + PlayerCharacter[i];

                        List<int> temp = new List<int>();
                        temp.AddRange(item.Key);
                        temp.Add(i);

                        DictForSixCharacter.Add(temp, str);

                    }
                }

                PnCofCharacters.AddRange(DictForSixCharacter.Values);

                #endregion

                #region Seven Charecter's List

                Dictionary<List<int>, string> DictForSevenCharacter = new Dictionary<List<int>, string>();

                foreach (var item in DictForSixCharacter)
                {
                    for (int i = 0; i < 7; i++)
                    {
                        if (item.Key.Contains(i))
                            continue;
                        string str = item.Value + PlayerCharacter[i];

                        List<int> temp = new List<int>();
                        temp.AddRange(item.Key);
                        temp.Add(i);

                        DictForSevenCharacter.Add(temp, str);

                    }
                }

                PnCofCharacters.AddRange(DictForSevenCharacter.Values);

                #endregion

            }
            else
            {

                List<string> playerCharList = new List<string>();

                List<string> alphabets = new List<string> { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };

                Dictionary<List<int>, string> tempDict = new Dictionary<List<int>, string>();

                List<char> tmpPlList = PlayerCharacter.ToList();

                while(tmpPlList.Contains(' '))
                    tmpPlList.Remove(' ');

                for (int i=0; i< tmpPlList.Count(); i++)
                {
                    tempDict.Add(new List<int>() { i }, tmpPlList[i].ToString());
                }

                for (int count = 0; count < tmpPlList.Count(); count++)
                {
                    Dictionary<List<int>, string> tmp = new Dictionary<List<int>, string>();
                    foreach (var item in tempDict)
                    {
                        for (int i = 0; i < tmpPlList.Count(); i++)
                        {
                            if (item.Key.Contains(i))
                                continue;

                            List<int> tmpintList = new List<int>();
                            tmpintList.AddRange(item.Key);
                            tmpintList.Add(i);
                            tmp.Add(tmpintList, item.Value + tmpPlList[i]);
                        }
                    }
                    tempDict = tmp;
                    playerCharList.AddRange(tmp.Values);
                }

                for (int i = 0; i < (PlayerCharacter.Count() - tmpPlList.Count); i++)
                {
                    foreach (string s in playerCharList)
                    {
                        foreach (string c in alphabets)
                        {
                            PnCofCharacters.Add(s + c);
                            PnCofCharacters.Add(c + s);
                            for (int index = 1; index < s.Length - 1; index++)
                            {
                                PnCofCharacters.Add(s.Insert(index, c));
                            }
                        }
                    }
                }

                PnCofCharacters = englishWords.Intersect(PnCofCharacters).ToList();
            }
        }

        public void setComputerWord()
        {
            try
            {
                string str = "";

                List<string> extendedPossibleStringList = new List<string>();

                List<WordAtBoard> listOfPossibleWords = new List<WordAtBoard>();

                List<WordAtBoard> listOfBoardWords = new List<WordAtBoard>();

                WordAtBoard validWord = new WordAtBoard();

                validWords = new List<string>();

                ComputePnC();

                for (int row = 0; row < 15; row++)
                {
                    WordAtBoard w = new WordAtBoard();

                    for (int col = 0; col < 15; col++)
                    {
                        str = "";

                        WordStartIndex ind = new WordStartIndex();

                        ind.Column = col;

                        ind.Row = row;

                        w.Index = ind;

                        while (BoardCharacters[row, col] != '\0')
                        {
                            str += BoardCharacters[row, col];
                            col++;
                            if (col >= 15)
                            {
                                str = "";
                                break;

                            }
                        }
                        if (str != "")
                        {
                            w.Orientation = WordOrientation.Horizontal;
                            w.Word = str.ToLower();
                            listOfBoardWords.Add(w);
                        }

                    }
                }

                for (int col = 0; col < 15; col++)
                {
                    for (int row = 0; row < 15; row++)
                    {
                        str = "";

                        WordStartIndex ind = new WordStartIndex();
                        ind.Row = row;
                        ind.Column = col;

                        WordAtBoard wd = new WordAtBoard();
                        wd.Index = ind;

                        while (BoardCharacters[row, col] != '\0')
                        {
                            str += BoardCharacters[row, col];
                            row++;
                            if (row >= 15)
                            {
                                str = "";
                                break;
                            }
                        }
                        if (str != "")
                        {
                            wd.Orientation = WordOrientation.Vertical;
                            wd.Word = str.ToLower();
                            listOfBoardWords.Add(wd);
                        }

                    }
                }

                if (FirstMove)
                {
                    FirstMove = false;
                    string s = englishWords.Intersect(PnCofCharacters).OrderByDescending(x => x.Length).First();
                    WordAtBoard w = new WordAtBoard();
                    w.Orientation = WordOrientation.Horizontal;
                    w.Index.Row = 7;
                    w.Index.Column = 7;
                    w.Word = s;
                    validWords.Add(s);
                    listOfPossibleWords.Add(w);
                }
                else if (!blankTileEncountered)
                {
                    #region Huge region to process for all Non Blank character tiles 

                    foreach (string s in PnCofCharacters)
                    {
                        foreach (var wd in listOfBoardWords)
                        {
                            string cWord = wd.Word + s;

                            extendedPossibleStringList.Add(cWord);

                            WordAtBoard w = new WordAtBoard();

                            w = wd;

                            w.Word = cWord;

                            listOfPossibleWords.Add(w);

                            cWord = s + wd.Word;

                            extendedPossibleStringList.Add(cWord);

                            w.Word = cWord;

                            if (wd.Orientation == WordOrientation.Horizontal)
                            {

                                w.Index.Column = wd.Index.Column - s.Length;

                                listOfPossibleWords.Add(w);

                                for (int i = 1; i < s.Length - 1; i++)
                                {
                                    s.Insert(i, wd.Word);

                                    extendedPossibleStringList.Add(s);

                                    w.Index.Column = wd.Index.Column - i;

                                }
                            }
                            else
                            {

                                w.Index.Row = wd.Index.Row - s.Length;

                                listOfPossibleWords.Add(w);

                                for (int i = 1; i < s.Length - 1; i++)
                                {

                                    s.Insert(i, wd.Word);

                                    extendedPossibleStringList.Add(s);

                                    w.Index.Row = wd.Index.Row - i;

                                    listOfPossibleWords.Add(w);

                                }

                            }
                        }
                    }

                    validWords.AddRange(englishWords.Intersect(extendedPossibleStringList).OrderByDescending(x => x.Length).ToList());

                    #endregion
                }
                else
                {

                    PnCofCharacters = PnCofCharacters.Distinct().ToList();

                    PnCofCharacters = PnCofCharacters.OrderByDescending(x => x.Length).ToList();

                    foreach(string s in PnCofCharacters)
                    {
                        foreach(var item in listOfBoardWords)
                        {
                            if (s.Contains(item.Word))
                            {
                                WordAtBoard w = new WordAtBoard();
                                int index = s.IndexOf(item.Word);
                                if(item.Orientation == WordOrientation.Horizontal)
                                {
                                    w.Orientation = WordOrientation.Horizontal;
                                    w.Index.Row = item.Index.Row;
                                    w.Index.Column = item.Index.Column - index;
                                }
                                else
                                {
                                    w.Orientation = WordOrientation.Vertical;
                                    w.Index.Row = item.Index.Row - index;
                                    w.Index.Column = item.Index.Column;
                                }
                                w.Word = s;
                                listOfPossibleWords.Add(w);
                                validWords.Add(s);
                            }
                        }
                    }
                }


                bool isValid = false;

                foreach (string word in validWords)
                {
                    validWord = listOfPossibleWords.Find(x => x.Word == word);

                    if (validWord.Word == null)
                        continue;
                    
                    if (usedWords.Contains(validWord.Word))
                        continue;


                    int row = validWord.Index.Row;

                    int col = validWord.Index.Column;

                    foreach (char c in validWord.Word)
                    {
                        if (row >= 15 || col >= 15 || row < 0 || col < 0)
                        {
                            isValid = false;
                            break;
                        }

                        if (c == char.ToLower(BoardCharacters[row, col]) || BoardCharacters[row, col] == '\0')
                        {
                            if (validWord.Orientation == WordOrientation.Horizontal)
                                col++;
                            else
                                row++;
                        }
                        else
                        {
                            isValid = false;
                            break;
                        }


                        isValid = true;
                    }
                    if (isValid)
                        break;
                }

                if (validWord.Word != null)
                {
                    usedWords.Add(validWord.Word);

                    ComputerWord = validWord;

                    isComputerWordNull = false;

                    extractUsedCharacters();
                }
                else
                {
                    isComputerWordNull = true;
                }

                listOfBoardWords.Clear();

                listOfPossibleWords.Clear();

                validWords.Clear();

                extendedPossibleStringList.Clear();

                PnCofCharacters.Clear();
                
                setRequiredNumberOfTiles();

            }
            catch (Exception ex)
            {

                validWords.Clear();

                PnCofCharacters.Clear();

            }
        }
        
        private void extractUsedCharacters()
        {
            int rowIndex = ComputerWord.Index.Row;

            int colIndex = ComputerWord.Index.Column;

            int ind;
            try
            {
                if (ComputerWord.Orientation == WordOrientation.Horizontal)
                {
                    foreach (char c in ComputerWord.Word.ToCharArray())
                    {
                        if (BoardCharacters[rowIndex, colIndex] == '\0')
                        {
                            ind = PlayerCharacter.ToList().IndexOf(c);

                            if(ind >= 0)
                                PlayerCharacter[ind] = '\0';
                            else
                            {
                                if (blankTileEncountered)
                                {
                                    ind = PlayerCharacter.ToList().IndexOf(' ');
                                    ((ScrabbleGamePage.MainPage)App.Current.MainPage).setBlankTile(rowIndex, colIndex);
                                    PlayerCharacter[ind] = '\0';
                                }
                            }
                        }

                        colIndex++;
                    }
                }
                else
                {
                    foreach (char c in ComputerWord.Word.ToCharArray())
                    {
                        if (BoardCharacters[rowIndex, colIndex] == '\0')
                        {
                            ind = PlayerCharacter.ToList().IndexOf(c);

                            if (ind >= 0)
                                PlayerCharacter[ind] = '\0';
                            else
                            {
                                if (blankTileEncountered)
                                {
                                    ind = PlayerCharacter.ToList().IndexOf(' ');
                                    ((ScrabbleGamePage.MainPage)App.Current.MainPage).setBlankTile(rowIndex, colIndex);
                                    PlayerCharacter[ind] = '\0';
                                }
                            }
                        }

                        rowIndex++;
                    }
                }
                
            }
            catch (Exception ex)
            {

            }
        
        }

        public void setRequiredNumberOfTiles()
        {
            int count = 0;
            blankTileEncountered = false;
            foreach(char c in PlayerCharacter)
            {
                if (c == '\0')
                    count++;
            }

            RequirednumberOfTiles = count;
        }


    }
}
