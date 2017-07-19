using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;

namespace ScrabbleGamePage
{
    public class PlayerPersonalArea : ContentView
    {
        private Grid gdPlayerTiles;
        private ColumnDefinition cd = new ColumnDefinition();
        private RowDefinition rd = new RowDefinition();
        private List<int> removedIndexes;

        public PlayerPersonalArea()
        {
            gdPlayerTiles = new Grid();

            gdPlayerTiles.ColumnSpacing = 0;

            gdPlayerTiles.Padding = 0;

            removedIndexes = new List<int>();

            gdPlayerTiles.Margin = 0;

            gdPlayerTiles.Padding = 0;

            this.Content = gdPlayerTiles;
        }
        

        public PlayerTile addPlayerTile(char alphabet)
        {
            PlayerTile pt = new PlayerTile(alphabet);

            gdPlayerTiles.Children.AddHorizontal(pt);
           
            return pt;
        }

        public void removePlayerTIle(PlayerTile pt)
        {
            gdPlayerTiles.Children.Remove(pt);
        }        
    }
}
