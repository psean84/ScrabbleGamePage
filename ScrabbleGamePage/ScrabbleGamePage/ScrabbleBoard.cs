using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using Xamarin.Forms;

using Xamarin.Forms;

namespace ScrabbleGamePage
{
    public class ScrabbleBoard : AbsoluteLayout
    {
        public ScrableTile[,] Board;

        private List<int> TWList = new List<int> { 0, 7, 14, 105, 119, 210, 217, 224 };
        private List<int> TLList = new List<int> { 20, 24, 76, 80, 84, 88, 136, 140, 144, 148, 200, 204 };
        private List<int> DLList = new List<int> { 3, 11, 36, 38, 45, 52, 59, 92, 96, 98, 102, 108, 116, 122, 126, 128, 132, 165, 172, 179, 186, 188, 213, 221 };
        private List<int> DWList = new List<int> { 16, 28, 32, 42, 48, 56, 64, 70, 154, 160, 168, 176, 182, 192, 196, 208 };


        int ROWS = 15;
        int COLS = 15;

        private static bool _isBoardBig;

        public ScrabbleBoard()
        {

            Board = new ScrableTile[ROWS, COLS];

            _isBoardBig = false;

            int index = 0;

            for(int i=0; i < ROWS; i++)
            {
                for(int j=0; j < COLS; j++)
                {
                    ScrableTile s;

                    if (TWList.Contains(index))
                        s = new ScrableTile(ScrableTile.TileValue.TrippleWord);
                    else if (TLList.Contains(index))
                        s = new ScrableTile(ScrableTile.TileValue.TrippleLetter);
                    else if (DLList.Contains(index))
                        s = new ScrableTile(ScrableTile.TileValue.DoubleLetter);
                    else if (DWList.Contains(index))
                        s = new ScrableTile(ScrableTile.TileValue.DoubleWord);
                    else if (index == 112)
                        s = new ScrableTile(ScrableTile.TileValue.Start);
                    else
                        s = new ScrableTile();

                    Board[i, j] = s;

                    s.ROW = i;

                    s.COL = j;

                    s.Margin = 0;

                    s.Padding = 0;
                   
                    this.Children.Add(s);

                    index++;
                }
            }

            this.SizeChanged += (sender, args) =>
           {

               double tileWidth;

               double tileHeight;

               if (this.Width > this.Height)
               {
                   tileWidth = this.Height / COLS;

                   tileHeight = tileWidth;
               }
               else
               {

                   tileWidth = this.Width / COLS;

                   tileHeight = tileWidth;
               }
               foreach (ScrableTile st in this.Children)
               {  
                   Rectangle bounds = new Rectangle(st.COL * tileWidth, st.ROW * tileHeight, tileWidth, tileHeight);

                   AbsoluteLayout.SetLayoutBounds(st, bounds);
               }

           };

            if (Device.OS == TargetPlatform.Android)
            {
                TapGestureRecognizer doubleTapped = new TapGestureRecognizer();
                doubleTapped.NumberOfTapsRequired = 2;
                doubleTapped.Tapped += async (s, e) =>
                {                   

                    double scale;

                    if (this.Width > this.Height)
                    {
                        scale = this.Width / this.Height;
                    }
                    else
                    {
                        scale = this.Height / this.Width;
                    }
                    if (_isBoardBig)
                    {
                        await this.ScaleTo(1);
                        _isBoardBig = false;
                    }
                    else
                        await this.ScaleTo(scale);
                };

                this.GestureRecognizers.Add(doubleTapped);
            }

            
        }
    }
}
