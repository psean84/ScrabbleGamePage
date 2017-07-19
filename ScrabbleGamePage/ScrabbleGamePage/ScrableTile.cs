using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ScrabbleGamePage
{
    public class ScrableTile : Frame
    {
        public enum TileValue { Normal, DoubleLetter, DoubleWord, TrippleLetter, TrippleWord, Start  };

        public bool isReady { get; set; }

        public bool isVisible { get; set; }

        public int ROW { get; set; }

        public int COL { get; set; }

        private char scrabbleCharacter;

        TapGestureRecognizer SingleTapped;

        public Image tileImage;

        public AbsoluteLayout altile;

        private Image bgImage;

        private bool isStartTile;

        public ScrableTile(TileValue TV = TileValue.Normal)
        {
            altile = new AbsoluteLayout { Margin = 0, Padding = 0, BackgroundColor = Color.Transparent, };

            tileImage = new Image { Aspect = Aspect.Fill, Opacity = 0, Margin = 0, };

            bgImage = new Image { Margin = 0, Opacity = 1, Aspect = Aspect.AspectFit, };

            isReady = false;

            isVisible = false;

            isStartTile = false;

            this.Padding = 0;

            this.Margin = 0;

            HasShadow = false;

            if (TV == TileValue.Normal)
            {
                this.BackgroundColor = Color.FromRgba(240, 220, 140, 100);
                this.OutlineColor = Color.FromRgb(240, 220, 140);
            }
            else
            {
                bool setImage = false;

                if (TV == TileValue.DoubleWord)
                {
                    this.BackgroundColor = Color.FromRgba(200, 140, 140, 70);
                    this.OutlineColor = Color.FromRgb(205, 140, 40);

                    bgImage.Source = Device.OnPlatform(
                    iOS: ImageSource.FromFile("Images/DW.png"),
                    Android: ImageSource.FromFile("DW.png"),
                    WinPhone: ImageSource.FromFile("Images//DW.png")
                    );

                    setImage = true;
                }
                else if (TV == TileValue.DoubleLetter)
                {
                    this.BackgroundColor = Color.FromRgba(10, 100, 55, 70);
                    this.OutlineColor = Color.FromRgb(10, 100, 55);

                    bgImage.Source = Device.OnPlatform(
                    iOS: ImageSource.FromFile("Images/DL.png"),
                    Android: ImageSource.FromFile("DL.png"),
                    WinPhone: ImageSource.FromFile("Images//DL.png")
                    );
                    setImage = true;
                }
                else if (TV == TileValue.TrippleLetter)
                {
                    this.BackgroundColor = Color.FromRgba(15, 190, 160, 70);
                    this.OutlineColor = Color.FromRgb(15, 190, 160);

                    bgImage.Source = Device.OnPlatform(
                    iOS: ImageSource.FromFile("Images/TL.png"),
                    Android: ImageSource.FromFile("TL.png"),
                    WinPhone: ImageSource.FromFile("Images//TL.png")
                    );
                    setImage = true;
                }
                else if (TV == TileValue.TrippleWord)
                {

                    this.BackgroundColor = Color.FromRgba(255, 80, 80, 70);
                    this.OutlineColor = Color.FromRgb(255, 80, 80);

                    bgImage.Source = Device.OnPlatform(
                    iOS: ImageSource.FromFile("Images/TW.png"),
                    Android: ImageSource.FromFile("tw.png"),
                    WinPhone: ImageSource.FromFile("Images//TW.png")
                    );
                    setImage = true;
                }
                else if(TV == TileValue.Start)
                {
                    this.BackgroundColor = Color.FromRgba(240, 220, 140, 100);
                    this.OutlineColor = Color.FromRgb(240, 220, 140);

                    bgImage.Source = Device.OnPlatform(
                    iOS: ImageSource.FromFile("Images/start.png"),
                    Android: ImageSource.FromFile("start.png"),
                    WinPhone: ImageSource.FromFile("Images//start.png")
                    );
                    isStartTile = true;
                    //altile.LowerChild(bgImage);
                }                

                altile.Children.Add(bgImage);

               
                this.Content = altile;

            }

            SingleTapped = new TapGestureRecognizer();

            SingleTapped.Tapped +=  (sender, args) =>
            {
                if (!isVisible)
                {
                    ((ScrabbleGamePage.MainPage)App.Current.MainPage).setTileisReady(ROW, COL);

                    if (isReady)
                        OnIsReady();
                }
                else
                    onIsVisible();
                
            };

            this.GestureRecognizers.Add(SingleTapped);
    
        }

        private void onIsVisible()
        {
            isVisible = false;

            isReady = false;

            ((ScrabbleGamePage.MainPage)App.Current.MainPage).setPlayerTile(scrabbleCharacter, ROW, COL);

            altile.Children.Remove(tileImage);

            tileImage = new Image { Aspect = Aspect.Fill, Opacity = 0, Margin = 0, };

            this.Content = altile;
        }


        private void OnIsReady()
        {
            char alphabet = ((ScrabbleGamePage.MainPage)App.Current.MainPage).getImage();
            
            scrabbleCharacter = alphabet;
            isReady = false;

            isVisible = true;

            string imgFileName;

            if (alphabet != ' ')
                imgFileName = alphabet.ToString();
            else
                imgFileName = "Blank";


            tileImage.Source = Device.OnPlatform(
            iOS: ImageSource.FromFile("Images/" + imgFileName + ".png"),
            Android: ImageSource.FromFile(imgFileName + ".png"),
            WinPhone: ImageSource.FromFile("Images//" + imgFileName + ".png")
            );


            altile.Children.Add(tileImage);

            if(!isStartTile)
                altile.LowerChild(tileImage);

            this.Content = altile;

            tileImage.FadeTo(0.9, 100, Easing.CubicIn);

            ((ScrabbleGamePage.MainPage)App.Current.MainPage).removeSelectedTile();
        }

        public async Task setTile(char alphabet)
        {
            scrabbleCharacter = alphabet;
            
            string imgFileName;

            if (alphabet != ' ')
                imgFileName = alphabet.ToString().ToUpper();
            else
                imgFileName = "Blank";


            tileImage.Source = Device.OnPlatform(
            iOS: ImageSource.FromFile("Images/" + imgFileName + ".png"),
            Android: ImageSource.FromFile(imgFileName + ".png"),
            WinPhone: ImageSource.FromFile("Images//" + imgFileName + ".png")
            );

            
            this.Content = altile;


            altile.Children.Add(tileImage);


            await tileImage.FadeTo(1, 250, Easing.BounceIn);

            

            finalise();
        }

        public void finalise()
        {
            this.GestureRecognizers.Remove(SingleTapped);
        }
        
    }
    
}
