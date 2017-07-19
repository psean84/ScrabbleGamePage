using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;

namespace ScrabbleGamePage
{
    [Newtonsoft.Json.JsonObject]
    public class Word
    {
        public string word;
        public string wordtype;
        public string definition;

        public Word()
        {
            
        }
    }

    class EnglishDictionary
    {
        public List<Word> DictionaryWords { get; private set; }
        public List<string> AllWords { get; private set; }

        public EnglishDictionary()
        {

            DictionaryWords = new List<Word>();

            AllWords = new List<string>();

            PopulateDictionary();
        }
        
        private void PopulateDictionary()
        {

            try
            {

                var assembly = typeof(ComputerPlayer).GetTypeInfo().Assembly;

                string resourcePrefix = "ScrabbleGamePage.";

#if __IOS__
resourcePrefix = ScrabbleGamePage.iOS.";
#endif
#if __ANDROID__
resourcePrefix = "ScrabbleGamePage.Android.";
#endif
                var stream = assembly.GetManifestResourceStream(resourcePrefix + "Resources.Data.eng-words.json");

                StreamReader ewReader = new StreamReader(stream);

                while (!ewReader.EndOfStream)
                {
                    Word w = Newtonsoft.Json.JsonConvert.DeserializeObject<Word>(ewReader.ReadLine());

                    w.word = w.word.ToLower();

                    DictionaryWords.Add(w);

                    AllWords.Add(w.word);
                }

                ewReader.Dispose();
            }
            catch (Exception ex)
            {

            }
        }
    }
    
}
