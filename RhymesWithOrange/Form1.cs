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

        /* As user types, echo the text typed in the button */
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string buttonTextTemplate = "What rhymes with '{0}'?";
            string word = wordTextBox.Text;
            getRhymesButton.Text = String.Format(buttonTextTemplate, word);
        }

        /* Read the word typed. Start a BackgroundWorker to search the web for rhymes. 
         * How do you change button1_Click to the actual name of the button ? 
         */
        private void button1_Click(object sender, EventArgs e)
        {
            resultsLabel.Text = "Searching....";

            string word = wordTextBox.Text;

            //Create a BackgroundWorker to search in the background and report back once 
            //request is complete. This is needed otherwise the GUI will freeze and be 
            //unresponsive until the request completes.
            BackgroundWorker webWorker = new BackgroundWorker();
            //Permit the background task to be cancelled
            webWorker.WorkerSupportsCancellation = true;
            //What is the task to be done in the background? The argument is the function to call
            webWorker.DoWork += new DoWorkEventHandler(webWorker_DoWork);
            //And what should happen when the task is done? Again, this is a function to call
            //We can't update the GUI in this function because we don't have results yet
            webWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(webWorker_RunWorkerCompleted);

            //If the worker is already working, cancel it - the user has clicked the button again
            //and presumably wants to search for another word
            if (webWorker.IsBusy)
            {
                webWorker.CancelAsync();
            }
            //Run the given task asynchronously - in the background
            //The parameter 'word' ends up in webWorker_DoWork by magic
            webWorker.RunWorkerAsync(word);
             
        }

        //Turns the result list into a String. I know there's a list GUI component but am lazy.
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

        //This is the function called to make the request. This happens in the background
        //so doesn't affect the GUI's responsiveness
        //So it's not magic. the parameter 'work' that's passed in when the background worker
        //starts ends up in the DoWorkEventArgs e parameter and can be accessed by e.Argument

        private void webWorker_DoWork(Object sender, DoWorkEventArgs e)
        {
            try
             {
                String word = e.Argument as String;
                //Rhymebrain provides an API - a way to send requests to their website
                //and get responses back as data, instead of web pages. In this case,
                //if you send this URL to rhymebrain, you'll get a response in JSON
                //JSON is organized text. Copy the full URL into your browser to 
                //see the JSON response.  
                //e.g. http://rhymebrain.com/talk?function=getRhymes&word=orange

                String urlBase = "http://rhymebrain.com/talk?function=getRhymes&word={0}";
                String url = String.Format(urlBase, word);

                //Library classes = create a web request for this URL...
                WebRequest request = WebRequest.Create(url);
                //And collect the response
                WebResponse resp = request.GetResponse();

                //The response could be 0 or a zillion bytes or anything in between
                //so gets returned as a Stream of byte after byte after byte.
                //Our code can then read the stream of bytes, in chunks of managable amounts of bytes, 
                // so it is not overwhelmed by all the data. 
                //We are going to assume that the response is a manageble size so 
                //we'll use a StreamReader to just read the whole thing. 
                Stream data = resp.GetResponseStream();
                StreamReader reader = new StreamReader(data);
                //We'll end up with a String of the JSON text data.
                String json = reader.ReadToEnd();
                //Always close resources when done.
                resp.Close();
                //That NewtonSoft package converts JSON into C# objects with the same properties
                //There'sa Rhyme.cs class in this project. Look at the attributes - they match
                //the attributes you see in the JSON returned. 
                //Deserialzing is the process of conveting JSON into objects that can be used in code.
                List<Rhyme> rhymeObjectsList = JsonConvert.DeserializeObject<List<Rhyme>>(json);

                //We only care about the word, so extract those into a new List of Strings
                List<string> rhymingWordsList = new List<string>();

                foreach (Rhyme rhyme in rhymeObjectsList)
                {
                    rhymingWordsList.Add(rhyme.word);
                }

                //And set e.Result to our list. This will, again by magic, be available to 
                //the webWorker_RunWorkerCompleted function. Note that this function was 
                //specified as the function to call when this task was complete
                e.Result = rhymingWordsList;
            }
            catch (Exception ex)
            {
                //A lot of things could go wrong - no internet connection, the URL could be malformed, 
                //the data returned in a format we don't expect...
                //log to the console...
                Console.WriteLine("Error fetching rhymes: " + ex);
                //... and re-throw error, to be handled by webWorker_RunWorkerCompleted method
                //If you see your code pausing here, because of an error, just hit the continue 
                //button to keep running the program. VisualStudio just wants you to know there was an error.
                throw ex;
            }

        }

        private void webWorker_RunWorkerCompleted(Object sender, RunWorkerCompletedEventArgs e)
        {
            //If an exception was thrown, it will be provided as e.Error. 
            //If no exception, e.Error will be null. So before trying to use the results,
            //Should check that there was no error. 
            if (e.Error == null)
            {
                //Now we have results! Can modify the GUI.
                //This e.Result is the same e.Result in the dowork method. 
                List<String> resultsList = e.Result as List<String>;
                resultsLabel.Text = createDisplayString(resultsList);
            }
            else
            {
                //Oh dear - an error. Display a message. 
                resultsLabel.Text = "Oh no! An error fetching rhymes";
            }
        }
    }
}

