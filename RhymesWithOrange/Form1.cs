using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RhymesWithOrange
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string buttonTextTemplate = "What rhymes with '{0}'?";
            string word = wordTextBox.Text;
            getRhymesButton.Text = String.Format(buttonTextTemplate, word);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            resultsLabel.Text = "Searching....";
            string word = wordTextBox.Text;
            List<String> rhymes = findRhymes(word);
            if (rhymes != null)
            {
                resultsLabel.Text = createDisplayString(rhymes);
            }
            else
            {
                resultsLabel.Text = "Oh no! An error fetching rhymes";
            } 
        }

        private string createDisplayString(List<string> rhymes)
        {

            string results = "Here are some words that rhyme:\n";
            foreach (String word in rhymes)
            {
                results += word;
                results += "\n";
            }

            return results;
        }

        private List<String> findRhymes(string word)
        {
            try
            {
                String urlBase = "http://rhymebrain.com/talk?function=getRhymes&word={0}&maxResults=10";
                String url = String.Format(urlBase, word);

                WebRequest request = WebRequest.Create(url);
                WebResponse resp = request.GetResponse();

                Stream data = resp.GetResponseStream();
                StreamReader reader = new StreamReader(data);

                string json = reader.ReadToEnd();

                resp.Close();

                List<Rhyme> rhymeObjectsList = JsonConvert.DeserializeObject<List<Rhyme>>(json);

                List<string> rhymingWordsList = new List<string>();

                foreach (Rhyme rhyme in rhymeObjectsList)
                {
                    rhymingWordsList.Add(rhyme.word);
                }

                return rhymingWordsList;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error fetching rhymes: " + e);
                return null;
            }

        }

        private void label2_Click(object sender, EventArgs e)
        {
        
        }
    }
}
