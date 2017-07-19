using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ScrabbleGamePage
{
    public class PlayerTile : Frame 
    {
        public Image tileImage { private set; get; }
        public bool isSelected = false;
        public string tileCharacter;

        public PlayerTile(char alphabet)
        {


            tileCharacter = alphabet.ToString();
            
            tileImage = new Image { Aspect = Aspect.Fill, Opacity = 95,  };
            this.Margin = 1;
            this.Padding = 2;
            string imgFileName;

            if (alphabet != ' ')
                imgFileName = alphabet.ToString();
            else
                imgFileName = "Blank";

            tileImage.Source = Device.OnPlatform(
                iOS: ImageSource.FromFile("Images/" + imgFileName + ".png"),
                Android: ImageSource.FromFile(imgFileName + ".png"),
                WinPhone: ImageSource.FromFile("Images//"+imgFileName+".png")           
                );
            
            this.Content = tileImage;

            TapGestureRecognizer OnSingleTap = new TapGestureRecognizer { NumberOfTapsRequired = 1, };

            OnSingleTap.Tapped += (sender, args) =>
             {

                 Grid gd = (Grid) this.Parent;
                 foreach(PlayerTile p in gd.Children)
                 {
                     if (p.isSelected)
                     {
                         p.isSelected = false;
                         p.BackgroundColor = Color.Transparent;
                         break;
                     }
                 }
                 this.BackgroundColor = Color.Black;
                 this.isSelected = true;
             };

            this.GestureRecognizers.Add(OnSingleTap);
        }
        

    }
}
