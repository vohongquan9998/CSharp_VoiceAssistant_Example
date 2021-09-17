using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Speech.Synthesis;
using System.Speech.Recognition;
using System.Diagnostics;
namespace AssistantBotV1
{
    public partial class Form1 : Form
    {
        String[] grammarFile = (File.ReadAllLines(@"..\..\assets\grammar.txt"));
        String[] responseFile = (File.ReadAllLines(@"..\..\assets\response.txt"));

        SpeechSynthesizer speechSynth = new SpeechSynthesizer();
        SpeechRecognitionEngine speechRecognition = new SpeechRecognitionEngine();
        Choices grammarList = new Choices();
        bool isPress;
        string[] processLink = { "Google", "Facebook", "Bing" };
        bool isOpenCommand = false;
        Core core = new Core();
        public Form1()
        {
           
            grammarList.Add(grammarFile);
            Grammar grammar = new Grammar(new GrammarBuilder(grammarList));
            try { speechRecognition.RequestRecognizerUpdate();
                speechRecognition.LoadGrammar(grammar);
                speechRecognition.SpeechRecognized += rec_SpeechRecognized;
                speechRecognition.SetInputToDefaultAudioDevice();
                speechRecognition.RecognizeAsync(RecognizeMode.Multiple);
            } catch { return; }
            isPress = true;
            speechSynth.SelectVoiceByHints(VoiceGender.Female);
            InitializeComponent();

        }

        public void say(String text) { speechSynth.SpeakAsync(text); }

        private void rec_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            String result = e.Result.Text;
            int resp = Array.IndexOf(grammarFile, result);
            Random r = new Random();
            if (isPress)
            {
                if (responseFile[resp].IndexOf('+') == 0)
                {
                    List<string> responses = responseFile[resp].Replace('+', ' ').Split('/').Reverse().ToList();                         
                    say(responses[r.Next(responses.Count)]);                 
                }
                else if (responseFile[resp].IndexOf('$') == 0)
                {
                    for (int i = 0; i < processLink.Length; i++)
                    {
                        if (result.Contains(processLink[i]))
                        {
                            say(processLink[i] + " is opening");
                            string https = "https://www.";
                            string com = ".com/";
                            Process.Start(https + processLink[i].ToLower() + com);
                        }
                    }
                }
                else
                {

                    if (responseFile[resp].IndexOf('*') == 0)
                    {

                        if (result.Contains("time"))
                        {
                            say(core.time());
                        }
                        if (result.Contains("day"))
                        {
                            say(core.day());
                        }
                        if (result.Contains("Show"))
                        {

                            if (!isOpenCommand)
                            {
                                isOpenCommand = true;
                                say("Yes sir");
                                core.showCommands(txtCommand,grammarFile);
                            }
                            else
                            {
                                say("My command already open");
                            }
                        }
                        if (result.Contains("Hide"))
                        {
                            if (isOpenCommand)
                            {
                                isOpenCommand = false;
                                say("Done");
                                txtCommand.Visible = false;
                                txtCommand.Text = "";
                            }
                            else
                            {
                                say("My command already close");
                            }
                        }
                        if (result.Contains("math"))
                        {
                            say("Sure Please enter your number to calculate");
                            groupBox1.Visible = true;
                        }
                    }

                    else
                    {
                        say(responseFile[resp]);
                    }
                        
                }
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnVoice_Click(object sender, EventArgs e)
        {
            isPress = true;
        }
    }
}
