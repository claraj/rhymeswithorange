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

            BackgroundWorker webWorker = new BackgroundWorker();
            webWorker.WorkerSupportsCancellation = true;
            webWorker.DoWork += new DoWorkEventHandler(webWorker_DoWork);
            webWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(webWorker_RunWorkerCompleted);

            if (webWorker.IsBusy)
            {
                webWorker.CancelAsync();
            }

            webWorker.RunWorkerAsync(word);
           
             
        }


        private string createDisplayString(List<string> rhymes)
        {

            string results = "Computers think these words rhyme:\n";
            foreach (String word in rhymes)
            {
                results += word;
                results += "\n";
            }

            return results;
        }

//        private List<String> findRhymes(string word)
        private void webWorker_DoWork(Object sender, DoWorkEventArgs e)
        {
            try
            {
                String word = e.Argument as String;
                String urlBase = "http://rhymebrain.com/talk?function=getRhymes&word={0}";
                String url = String.Format(urlBase, word);

                WebRequest request = WebRequest.Create(url);
                WebResponse resp = request.GetResponse();

                Stream data = resp.GetResponseStream();
                StreamReader reader = new StreamReader(data);

                String json = reader.ReadToEnd();

                resp.Close();

                List<Rhyme> rhymeObjectsList = JsonConvert.DeserializeObject<List<Rhyme>>(json);

                List<string> rhymingWordsList = new List<string>();

                foreach (Rhyme rhyme in rhymeObjectsList)
                {
                    rhymingWordsList.Add(rhyme.word);
                }

                e.Result = rhymingWordsList;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching rhymes: " + ex);
                //Log and rethrow. Will become e.Error in RunWorkerCompleted
                throw ex;
            }

        }

        private void webWorker_RunWorkerCompleted(Object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                List<String> resultsList = e.Result as List<String>;
                resultsLabel.Text = createDisplayString(resultsList);
            }
            else
            {
                resultsLabel.Text = "Oh no! An error fetching rhymes";
            }
        }
    }
}

