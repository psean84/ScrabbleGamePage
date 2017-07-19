using System ;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;


namespace ScrabbleGamePage
{
    public partial class MainPage : ContentPage
    {

        #region Declaration of global variables

        private PlayerTile selectedTile;

        private PlayerPersonalArea ppa;

        private int col, row;

        private bool checkrow, checkcol;

        private ComputerPlayer compPlayer;

        private GameTiles gt;

        public bool FirstMove { get; set; }

        private bool gameEnded;

        private bool continueFirstMove;

        ComputerPlayer.WordAtBoard compPlayerWord;

        private Player player1;

        private Player computer;

        private struct Index { public int Row; public int Column; };

        private struct PlayerTilesOnBoard { public Index characterIndex; public char tileCharacter; }

        private List<Index> listOfCurrentPlayedIndices;

        private EnglishDictionary ed;
        
        private bool isValidWord;

        private static int MAXPLAYERTILES;

        private bool _blankTileEncountered;

        private int _blankRow, _blankColumn;

        private ComputerPlayer.WordAtBoard _wab;

        private List<PlayerTilesOnBoard> _currentplayerTiles;

        bool _isBoardTileincluded;

        bool _isGapPresent;

        #endregion

        /*************************************************************************************************************************************
         *                                                                                                                                   *
         *                                                      Start of the Program                                                         *
         *                                                                                                                                   *    
         * ***********************************************************************************************************************************/

        public MainPage()
        {
            try
            {
                InitializeComponent();

                NavigationPage.SetHasNavigationBar(this, false);
                
                TapGestureRecognizer submitRecognizer = new TapGestureRecognizer();

                submitRecognizer.Tapped += async (sender, args) => { await OnSubmitTapped(); };

                submitRecognizer.NumberOfTapsRequired = 1;

                imgSubmit.GestureRecognizers.Add(submitRecognizer);
                
                frMessagePlaceHolder.PropertyChanged += (s, e) =>
                {
                    Frame f = (Frame)s;

                    if (f.Opacity == 0)
                    {
                        f.IsVisible = false;
                        f.VerticalOptions = LayoutOptions.Start;
                        f.Opacity = 0.8;
                    }
                    
                };
                
                InitializeVariables();

                InitializeGameElements();
                

            }
            catch (Exception ex)
            {

            }
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //                   Initializing Global variables and Game                                                                        //
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private void InitializeVariables()
        {

            gt = new GameTiles();

            gt.NewGame();

            ppa = new PlayerPersonalArea();

            for (int i = 0; i < 7; i++)
            {
                PlayerTile pt = ppa.addPlayerTile(gt.GetNewTile());

                TapGestureRecognizer singleTapped = new TapGestureRecognizer();

                singleTapped.Tapped += (sender, args) =>
                {
                    selectedTile = pt;
                };

                pt.GestureRecognizers.Add(singleTapped);
            }

            MAXPLAYERTILES = ((Grid)ppa.Content).Children.Count;

            ppa.Margin = 0;

            ppa.Padding = 0;

            stPlayerGrid.Children.Add(ppa);


            FirstMove = true;

            continueFirstMove = false;

            selectedTile = null;
            
            gameEnded = false;

            compPlayer = new ComputerPlayer();

            player1 = new Player("You");

            computer = new Player("Computer");

            pName.Text = player1.playerName + ": ";

            cName.Text = computer.playerName + ": ";

            pScore.Text = player1.CurrentScore;

            cScore.Text = computer.CurrentScore;

            ed = new EnglishDictionary();

            compPlayer.englishWords = ed.AllWords;

        }

        private void InitializeGameElements()
        { 
            row = -1;

            col = -1;

            checkrow = false;

            checkcol = false;

            isValidWord = true;

            _isBoardTileincluded = false;

            _blankTileEncountered = false;

            _wab = new ComputerPlayer.WordAtBoard();

            _blankRow = 0;

            _blankColumn = 0;

            listOfCurrentPlayedIndices = new List<Index>();

            _currentplayerTiles = new List<PlayerTilesOnBoard>();
        }
        
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //                    Function setBlankTile takes two argument that are row and column number                                      //
        //                    to tell GameTile Object that even a character is present at that place                                       //
        //                    that is a Blank character donot include in score calculation                                                 //
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public void setBlankTile(int r, int c)
        {
            gt.setBlankTile((r * 15) + c);

            _blankRow = r;

            _blankColumn = c;

            _blankTileEncountered = true;
        }
        
        //Cheack if the tile placed by the player is valid. If valid place the tile on board
        public void setTileisReady(int r, int c)
        {
            if (selectedTile != null)
            {
                if (FirstMove)
                { 
                    if (r == 7 && c == 7)
                    {
                        FirstMove = false;
                        row = 7;
                        col = 7;
                        checkrow = true;
                        checkcol = false;
                        continueFirstMove = true;
                    }
                    else
                    {
                        displayMessage("Start with the star on the board", Color.Blue);
                    }
                }
                else if (row == -1 || col == -1)
                {
                    row = r;
                    col = c;
                    checkrow = true;
                }
                else
                {
                    if (row == r)
                    {
                        if (r == 7)
                        {
                            if (continueFirstMove)
                            {
                                if (c >= 7)
                                {
                                    checkrow = true;
                                    col = -2;
                                }
                            }
                            else
                            {
                                checkrow = true;
                                col = -2;
                            }
                        }
                        else
                        {
                            checkrow = true;
                            col = -2;
                        }
                    }
                    else if (col == c)
                    {
                        checkcol = true;
                        row = -2;
                    }
                    
                }

                if (checkcol || checkrow)
                {
                    Board.Board[r, c].isReady = true;

                    if (selectedTile.tileCharacter == " ")
                        setBlankTile(r, c);

                    Index currentIndex = new Index();
                    currentIndex.Row = r;
                    currentIndex.Column = c;
                    listOfCurrentPlayedIndices.Add(currentIndex);

                    PlayerTilesOnBoard pTOnB = new PlayerTilesOnBoard();
                    pTOnB.characterIndex = currentIndex;
                    pTOnB.tileCharacter = selectedTile.tileCharacter.ToCharArray()[0];
                    _currentplayerTiles.Add(pTOnB);
                }
            }
            checkrow = false;
            checkcol = false;

        }
    
        //Get the Tile character that is placed on Board
        public char getImage()
        {
            return selectedTile.tileCharacter.ToCharArray()[0];
        }

        //Remove Selected Tile From Player Personal Area
        public void removeSelectedTile()
        {
            selectedTile.FadeTo(0, 150, Easing.CubicIn);

            selectedTile.Scale = 0;

            selectedTile.IsEnabled = false;
            
            selectedTile = null;
        }
        
        //Set PlayerTile in Player's Personal Area
        public void setPlayerTile(char a, int r, int c)
        {
            Grid gdPlayerGRid = (Grid)ppa.Content;

            int charCountInPPA = 1;

            foreach(PlayerTile p in gdPlayerGRid.Children)
            {
                if (a == p.tileCharacter.ToCharArray()[0] && p.Opacity == 0)
                {
                    p.IsEnabled = true;
                    p.Scale = 1;
                    p.FadeTo(1);
                    selectedTile = p;

                    gt.removeBoardCharacter(r, c);
                    compPlayer.InsertBoardCharacter(r, c, '\0');

                    Index i = new Index();
                    i.Row = r;
                    i.Column = c;

                    listOfCurrentPlayedIndices.Remove(i);

                    PlayerTilesOnBoard ptob = new PlayerTilesOnBoard();
                    ptob.characterIndex = i;
                    ptob.tileCharacter = a;
                    _currentplayerTiles.Remove(ptob);

                    break;
                }
                
            }

            foreach (PlayerTile p in gdPlayerGRid.Children)
                if (p.Opacity > 0)
                    charCountInPPA++;

            if (r == 7 && c == 7)
            {
                FirstMove = true;
                continueFirstMove = false;
            }

            if (charCountInPPA >= MAXPLAYERTILES)
            {
                row = -1;
                col = -1;
                isValidWord = true;
            }
        }

        //Display Message in middle of the board
        private async Task displayMessage(String s, Color color)
        {
            frMessagePlaceHolder.VerticalOptions = LayoutOptions.Center;

            StackLayout stMessage = new StackLayout();

            Label lblMessage = new Label();

            lblMessage.Text = s.ToUpper();

            lblMessage.TextColor = color;

            lblMessage.FontAttributes = FontAttributes.Bold;

            lblMessage.FontSize = Device.GetNamedSize(NamedSize.Medium, lblMessage);

            stMessage.Children.Add(lblMessage);

            frMessagePlaceHolder.Content = stMessage;

            frMessagePlaceHolder.Opacity = 0.9;

            frMessagePlaceHolder.IsVisible = true;
                        
            await frMessagePlaceHolder.FadeTo(0, 2000, Easing.CubicInOut);
            
        }
        
        private void calculatePlayerScore()
        {
            List<string> columnWords = new List<string>();

            List<string> rowWords = new List<string>();


            try
            {
                //when word is horizontally placed and word length is greater than 1
                if (row >= 0 && col < 0)
                    _currentplayerTiles = _currentplayerTiles.OrderBy(x => x.characterIndex.Column).ToList();
                

                //when word is vertically placed and word length is greater than 1
                if (col >= 0 && row < 0)
                    _currentplayerTiles = _currentplayerTiles.OrderBy(x => x.characterIndex.Row).ToList();
                

                createStringFromCurrentIndices();                
            }
            
            catch (Exception ex)
            {

            }
        }
        
        private void createStringFromCurrentIndices()
        {
            Index currentStartindex = _currentplayerTiles.First().characterIndex;

            Index currentEndindex = _currentplayerTiles.Last().characterIndex;
            
            int si = 0, ei = 0;
            
            List<string> playerpossibleWords = new List<string>();

            char[] playerCharArray = new char[15];

            char[] boardArray = new char[15];

            string playerString ="";

            bool isRowString = false;

            bool isColumnString = false;

            int rowNumber = 0;

            int columnNumber = 0;

            try
            {
                if (currentStartindex.Row == 7 && currentStartindex.Column == 7)
                {
                    _wab.Orientation = ComputerPlayer.WordOrientation.Horizontal;
                    _wab.Index.Row = 7;
                    _wab.Index.Column = 7;
                    int i = 7;
                    foreach(PlayerTilesOnBoard p in _currentplayerTiles)
                    {
                        if(p.characterIndex.Column != i)
                        {
                            _isGapPresent = true;
                            break;
                        }

                        i++;
                        playerString += p.tileCharacter;
                    }
                    playerpossibleWords.Add(playerString.ToLower());
                }
                else
                {
                    if (currentStartindex.Row == currentEndindex.Row)
                    {
                        si = currentStartindex.Column;

                        ei = currentEndindex.Column;

                        _wab.Index.Row = currentStartindex.Row;

                        _wab.Orientation = ComputerPlayer.WordOrientation.Horizontal;

                        boardArray = gt.getBoardCharacterByRow(currentStartindex.Row);

                        foreach (PlayerTilesOnBoard p in _currentplayerTiles)
                            playerCharArray[p.characterIndex.Column] = p.tileCharacter;


                        playerString = getStringFromBoard(si, ei, playerCharArray, boardArray);

                        string bsString = beforeStartString(boardArray, si);

                        string aeString = afterEndString(boardArray, ei);

                        if (_isBoardTileincluded)
                            playerpossibleWords.Add(playerString);

                        if (aeString.Length > 0)
                            playerpossibleWords.Add(playerString + aeString);

                        if (bsString.Length > 0)
                            playerpossibleWords.Add(bsString + playerString);

                        if (bsString.Length > 0 && aeString.Length > 0)
                            playerpossibleWords.Add(bsString + playerString + aeString);

                        isRowString = true;

                        rowNumber = currentEndindex.Row;
                    }


                    if (currentStartindex.Column == currentEndindex.Column)
                    {
                        si = currentStartindex.Row;

                        ei = currentEndindex.Row;

                        boardArray = gt.getBoardCharacterByColumn(currentStartindex.Column);

                        foreach (PlayerTilesOnBoard p in _currentplayerTiles)
                            playerCharArray[p.characterIndex.Row] = p.tileCharacter;

                        _wab.Index.Column = currentStartindex.Column;

                        _wab.Orientation = ComputerPlayer.WordOrientation.Vertical;

                        playerString = getStringFromBoard(si, ei, playerCharArray, boardArray);

                        string bsString = beforeStartString(boardArray, si);

                        string aeString = afterEndString(boardArray, ei);


                        if (_isBoardTileincluded)
                            playerpossibleWords.Add(playerString);

                        if (aeString.Length > 0)
                            playerpossibleWords.Add(playerString + aeString);

                        if (bsString.Length > 0)
                            playerpossibleWords.Add(bsString + playerString);

                        if (bsString.Length > 0 && aeString.Length > 0)
                            playerpossibleWords.Add(bsString + playerString + aeString);

                        isColumnString = true;

                        columnNumber = currentStartindex.Column;
                    }
                }


                if (_isGapPresent)
                    return;

                List<string> tmp = new List<string>();

                if (_blankTileEncountered)
                {
                    List<string> alphabets = new List<string> { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };

                    foreach (string s in playerpossibleWords)
                    {
                        foreach (string ch in alphabets)
                            tmp.Add(s.Replace(" ", ch).ToLower());
                    }
                }
                else
                {
                    foreach (string s in playerpossibleWords)
                        tmp.Add(s.ToLower());
                }

                playerpossibleWords = ed.AllWords.Intersect(tmp).OrderByDescending(x => x.Length).ToList();

                if (playerpossibleWords.Count > 0)
                {
                    isValidWord = true;

                    _isBoardTileincluded = true;

                    string selectedWord = playerpossibleWords.First();

                    setWordOnBoard();

                    _wab.Word = selectedWord;

                    char[] rowArray = new char[15];

                    char[] columnArray = new char[15];

                    if (isRowString)
                        rowArray = gt.getBoardCharacterByRow(rowNumber);

                    if (isColumnString)
                        columnArray = gt.getBoardCharacterByColumn(columnNumber);

                    if (isRowString)
                    {
                        int index = getStartOftheString(rowArray, _wab.Word);

                        if (index > 0)
                            _wab.Index.Column = index;
                    }

                    if (isColumnString)
                    {
                        int index = getStartOftheString(columnArray, _wab.Word);

                        if (index > 0)
                            _wab.Index.Row = index;
                    }


                }
                else
                {
                    isValidWord = false;
                    if (ed.AllWords.Contains(playerString))
                        _isBoardTileincluded = false;
                }
            }
            catch(Exception ex)
            {

            }

        }
        
        private int getStartOftheString(char[] array, string s)
        {

            char[] tmpCharArray = s.ToCharArray();

            int index = array.ToList().FindIndex(x => x == tmpCharArray[0]);

            bool isPresent = false;

            try
            {
                for (int i = index, j = 0; i < 15; i++, j++)
                {
                    if (j == tmpCharArray.Length - 1)
                        break;

                    isPresent = true;

                    if (array[i] != tmpCharArray[j])
                    {
                        isPresent = false;
                        j = 0;
                        i = array.ToList().FindIndex(i, x => x == tmpCharArray[0]);

                        if (i < 1)
                            break;
                    }
                }
                                
            }
            catch(Exception ex)
            {

            }

            if (!isPresent)
                index = -1;

            return index;
        }

        private void setWordOnBoard()
        {
            foreach(PlayerTilesOnBoard p in _currentplayerTiles)
            {
                gt.setBoardCharacter(p.characterIndex.Row, p.characterIndex.Column, char.ToUpper(p.tileCharacter));
                compPlayer.InsertBoardCharacter(p.characterIndex.Row, p.characterIndex.Column, char.ToUpper(p.tileCharacter));
            }
        }

        private string beforeStartString(char[] boardArray, int st)
        {
            string s = "";

            for(int i = st - 1; i >= 0; i--)
            {
                if (boardArray[i] == '\0')
                    break;

                s = boardArray[i] + s;
            }

            return s;
        }

        private string afterEndString(char[] boardArray, int e)
        {
            string s = "";

            for (int i = e + 1; i < 15; i++)
            {
                if (boardArray[i] == '\0')
                    break;

                s = s + boardArray[i];
            }

            return s;
        }

        private string getStringFromBoard(int start, int end, char[] playerArray, char[] charArrayFromBoard)
        {
            string s = "";
                                    
            for(int i=start; i <= end; i++)
            {
                if(charArrayFromBoard[i] != '\0')
                {
                    s += charArrayFromBoard[i];
                    _isBoardTileincluded = true;

                }else if(playerArray[i] != '\0')
                {
                    s += playerArray[i];
                }
                else
                {
                    _isGapPresent = true;
                    break;
                }
            }

            return s;
        }
        
        //Remove tap events from the tiles placed on board
        private void finalisePlayerTiles()
        {
            foreach (Index x in listOfCurrentPlayedIndices)
            {
                Board.Board[x.Row, x.Column].finalise();
            }
            listOfCurrentPlayedIndices.Clear();
            listOfCurrentPlayedIndices = new List<Index>();
        }

        private async Task invalidMessageDisplay()
        {

            string errMessage = "Invalid Scrabble Word";

            await displayMessage(errMessage, Color.Red);
        }

        private async Task computerPlay()
        {

            for (int i = 0; i < compPlayer.RequirednumberOfTiles; i++)
            {
                if (!gt.tilesCountToZero())
                    compPlayer.AddPlayerCharacter(gt.GetNewTile());
                else
                {
                    gameEnded = true;

                    break;
                }
            }



            lblRemainingTiles.Text = gt.getRemainingNumberOfTiles();

            if (FirstMove)
            {
                compPlayer.FirstMove = true;

                FirstMove = false;
            }

            compPlayer.setComputerWord();

            if (!compPlayer.isComputerWordNull)
            {
                string strCW = compPlayer.ComputerWord.Word;

                DictionaryInformation di = new DictionaryInformation(strCW);

                List<Word> lstWords = ed.DictionaryWords.FindAll(x => x.word.ToLower() == strCW);

                foreach (Word w in lstWords)
                {
                    di.AddMeanings(w.wordtype, w.definition);
                }

                frMessagePlaceHolder.Content = di;
                                
                compPlayerWord = compPlayer.ComputerWord;

                int computerRowIndex = compPlayerWord.Index.Row;

                int computerColumnIndex = compPlayerWord.Index.Column;

                foreach (char ch in compPlayerWord.Word)
                {
                    if (gt.isBoardTileEmpty(computerRowIndex, computerColumnIndex))
                    {
                        if (!_blankTileEncountered)
                            await Board.Board[computerRowIndex, computerColumnIndex].setTile(ch);
                        else
                        {
                            if (computerColumnIndex != _blankColumn && computerRowIndex != _blankRow)
                            {
                                await Board.Board[computerRowIndex, computerColumnIndex].setTile(ch);
                            }
                            else
                            {
                                await Board.Board[computerRowIndex, computerColumnIndex].setTile(' ');

                                _blankRow = 0;

                                _blankColumn = 0;

                                _blankTileEncountered = false;
                            }
                        }

                        compPlayer.InsertBoardCharacter(computerRowIndex, computerColumnIndex, ch);

                        gt.setBoardCharacter(computerRowIndex, computerColumnIndex, ch);
                    }

                    if (compPlayerWord.Orientation == ComputerPlayer.WordOrientation.Horizontal)
                        computerColumnIndex++;
                    else
                        computerRowIndex++;

                }

                computer.addToScore(gt.calculatePlayerScore(compPlayerWord.Word, compPlayerWord.Orientation, (compPlayerWord.Index.Row * 15) + (compPlayerWord.Index.Column)));

                cScore.Text = computer.CurrentScore;
            }

            frWait.IsVisible = false;

        }
        
        private async Task OnSubmitTapped()
        {
            try

            {
                calculatePlayerScore();

                if (isValidWord)
                {
                    int plScore = gt.calculatePlayerScore(_wab.Word, _wab.Orientation, (_wab.Index.Row * 15) + _wab.Index.Column);
                    
                    player1.addToScore(plScore);
                    
                    pScore.Text = player1.CurrentScore;
                    
                    frWait.IsVisible = true;
                                        
                    await computerPlay();
                                        
                    await displayDictionaryInformation();

                    finalisePlayerTiles();

                    InitializeGameElements();

                    repopulatePlayerTiles();

                    if (gameEnded)
                    {
                        if (computer.Score > player1.Score)
                        {
                            frGameFinished.BackgroundColor = Color.Red;

                            lblFinishMessage.Text = "YOU LOST";
                        }
                        else
                        {
                            frGameFinished.BackgroundColor = Color.Green;

                            lblFinishMessage.Text = "YOU WON";
                        }

                        await endGameAnimation();
                    }
                }
                else
                {
                    if (_isBoardTileincluded)
                        await displayMessage("Include Board Tile in your word", Color.Green);
                    else
                        await invalidMessageDisplay();
                    
                }


                lblRemainingTiles.Text = gt.getRemainingNumberOfTiles();
            }

            catch (Exception ex)

            {
                frWait.IsVisible = false;
            }
        }

        private void Board_SizeChanged(object sender, EventArgs e)
        {
            
        }

        private async Task displayDictionaryInformation()
        {
            frMessagePlaceHolder.IsVisible = true;

            await frMessagePlaceHolder.FadeTo(0.8, 3000, Easing.SinIn);

            await frMessagePlaceHolder.FadeTo(0, 500, Easing.SinOut);

            frMessagePlaceHolder.Opacity = 0.8;
        }

        private void repopulatePlayerTiles()
        {
            Grid gdPlayerGRid = (Grid)ppa.Content;

            List<char> playerCharacter = new List<char>();

            foreach (PlayerTile p in gdPlayerGRid.Children)
            {
                if (p.Opacity == 0)
                {
                    if (!gt.tilesCountToZero())
                        playerCharacter.Add(gt.GetNewTile());
                    else
                        gameEnded = true;
                }
                else
                    playerCharacter.Add(p.tileCharacter.ToCharArray()[0]);
            }


            ppa.Content = null;

            gdPlayerGRid = new Grid();

            foreach (char a in playerCharacter)
            {
                PlayerTile pt = new PlayerTile(a);

                TapGestureRecognizer singleTapped = new TapGestureRecognizer();

                singleTapped.Tapped += (s, args) =>
                {
                    selectedTile = pt;
                };

                pt.GestureRecognizers.Add(singleTapped);

                gdPlayerGRid.Children.AddHorizontal(pt);

            }
            ppa.Content = gdPlayerGRid;

            MAXPLAYERTILES = gdPlayerGRid.Children.Count;

            stPlayerGrid.Children.Add(ppa);
            

            if (continueFirstMove)
                continueFirstMove = false;

            row = -1;

            col = -1;

        }

        private async Task endGameAnimation()
        {
            frGameFinished.Opacity = 0.1;

            frGameFinished.IsVisible = true;

            frGameFinished.FadeTo(0.9, 1000, Easing.SpringOut);

            await frGameFinished.ScaleTo(1, 1000, Easing.CubicOut);

        }

    }
}
