using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ScrabbleGamePage
{
    class DictionaryInformation : StackLayout
    {

        public int Delay { get; set; }

        public DictionaryInformation(string w)
        {
            this.IsClippedToBounds = true;

            Label Word = new Label();

            Word.FontSize = Device.GetNamedSize(NamedSize.Medium, Word);

            Word.FontFamily = "MS SANS";

            Word.Text = w.ToUpper();
            /*
            Delay = 5000;

            TapGestureRecognizer tp = new TapGestureRecognizer();

            tp.NumberOfTapsRequired = 1;

            tp.Tapped += async (s,e) => {  };
            */
            this.Children.Add(Word);
        }

        public void AddMeanings(string type, string meaning)
        {
            FormattedString m = new FormattedString();
            
            

            m.Spans.Add(new Span() { Text = meaning, FontAttributes = FontAttributes.Bold });

            m.Spans.Add(new Span() { Text = "( " + type + " )", FontAttributes = FontAttributes.Italic });

            Label lblm = new Label();
            
            lblm.Margin = new Thickness(10);

            lblm.FontFamily = "Segoe WP";

            lblm.FormattedText = m;

            this.Children.Add(lblm);
         }
        

        private void DisplayMessage()
        {
            Delay = 5000;
            
        }

    }
}
