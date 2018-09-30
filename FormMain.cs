using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Globalization;
using System.Net;

using Newtonsoft.Json;

using Microsoft.Speech.Synthesis;
using Microsoft.Speech.Recognition;
using System.IO;

using VkNet;
using VkNet.Enums.Filters;
using VkNet.Model;



namespace VoiceControl
{
    public partial class FormMain : Form
    {
        public SpeechSynthesizer ss;
        public SpeechRecognitionEngine sre;
        VkApi vk;
        public bool active;


        public FormMain()
        {
            InitializeComponent();

            sre = new SpeechRecognitionEngine();
            ss = new SpeechSynthesizer();

            vk = new VkApi();

            ApiAuthParams _aap = new ApiAuthParams();
            _aap.Settings = Settings.All;
            _aap.ApplicationId = 6707008;
            _aap.Login = "79276088639";
            _aap.Password = "123456do";

            vk.Authorize(_aap);

            sre.SpeechRecognized += Sre_SpeechRecognized;
            sre.RecognizeCompleted += Sre_RecognizeCompleted;

            sre.SetInputToDefaultAudioDevice();
            ss.SetOutputToDefaultAudioDevice();

            active = false;
            ss.Rate = 0;

            Choices ch_startCommands = new Choices();
            foreach (string str_actPhrase in File.ReadAllLines(@"dictionary\active_phrases.txt"))
                ch_startCommands.Add("Васян "+str_actPhrase);

            foreach (string str_actPhrase in File.ReadAllLines(@"dictionary\passive_phrases.txt"))
                ch_startCommands.Add("Васян " + str_actPhrase);

            GrammarBuilder gb_startCommands = new GrammarBuilder(ch_startCommands);
            Grammar g_start = new Grammar(gb_startCommands);

            Choices ch_vk = new Choices();
            foreach (string str_questPhrase in File.ReadAllLines(@"dictionary\VkIds.txt"))
                ch_vk.Add(str_questPhrase.Split('|')[0]);


            Choices ch_vkSamples = new Choices();
            foreach (string str_vkSample in File.ReadAllLines(@"dictionary\vk_samples.txt"))
            {
                ch_vkSamples.Add(str_vkSample);
            }
            


            GrammarBuilder gb_vk = new GrammarBuilder();
            gb_vk.Append("Отправь");
            gb_vk.Append(ch_vk);
            gb_vk.Append(ch_vkSamples);
            Grammar g_vk = new Grammar(gb_vk);

            Choices ch_quests = new Choices();
            foreach (string str_questPhrase in File.ReadAllLines(@"dictionary\QuestAnswer.txt"))
                ch_quests.Add(str_questPhrase.Split('|')[0]);

            GrammarBuilder gb_quests = new GrammarBuilder(ch_quests);
            Grammar g_quests = new Grammar(gb_quests);


            Choices ch_openCommands = new Choices();
            ch_openCommands.Add(" ");
            foreach (string str_openPhrase in File.ReadAllLines(@"dictionary\Open_phrases.txt"))
                ch_openCommands.Add(str_openPhrase);



            Choices ch_closeCommands = new Choices();
            foreach (string str_closePhrase in File.ReadAllLines(@"dictionary\Close_phrases.txt"))
                ch_closeCommands.Add(str_closePhrase);


            Choices ch_programs = new Choices();
            foreach (string str_program in File.ReadAllLines(@"dictionary\Programs.txt"))
                ch_programs.Add(str_program.Split('|')[0]);


            foreach (string site in File.ReadAllLines(@"dictionary\Sites.txt"))
                ch_programs.Add(site.Split('|')[0]);


            GrammarBuilder gb_openPrograms = new GrammarBuilder();
            gb_openPrograms.Append(ch_openCommands);
            gb_openPrograms.Append(ch_programs);
            Grammar g_open = new Grammar(gb_openPrograms);


            GrammarBuilder gb_closePrograms = new GrammarBuilder();
            gb_closePrograms.Append(ch_closeCommands);
            gb_closePrograms.Append(ch_programs);
            Grammar g_close = new Grammar(gb_closePrograms);


            GrammarBuilder gb_anectod = new GrammarBuilder();
            gb_anectod.Append(new Choices(" ", "Расскажи шутку", "Расскажи анекдот", "Расcмеши меня"));
            Grammar g_anectod = new Grammar(gb_anectod);

            GrammarBuilder gb_analys = new GrammarBuilder();
            gb_analys.Append(new Choices(" ",
                "Что там с погодой",
                "Сколько время",
                "Какой день недели",
                "Какое сегодня число"));
            Grammar g_analys = new Grammar(gb_analys);

            sre.LoadGrammarAsync(g_start);
            sre.LoadGrammarAsync(g_close);
            sre.LoadGrammarAsync(g_open);
            sre.LoadGrammarAsync(g_quests);
            sre.LoadGrammarAsync(g_vk);
            sre.LoadGrammarAsync(g_anectod);
            sre.LoadGrammarAsync(g_analys);

            sre.RecognizeAsync(RecognizeMode.Multiple);

        }

        private void Sre_RecognizeCompleted(object sender, RecognizeCompletedEventArgs e)
        {
           
        }

        private void Sre_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            bool endSpeech = false;
            float confidence = e.Result.Confidence;
            string result = e.Result.Text;

            if (confidence < 0.6)
                return;

            foreach (string open_phrase in File.ReadAllLines(@"dictionary\active_phrases.txt"))
                if (result.IndexOf(open_phrase) != -1)
                {
                    richTextBox_messages.Text += e.Result.Text + "\r\n";
                    ss.SpeakAsync(GetRandomStr(File.ReadAllLines(@"dictionary\Hello_Phrases.txt"), 0));
                    active = true;
                    return;
                }

            foreach (string passive_phrase in File.ReadAllLines(@"dictionary\passive_phrases.txt"))
                if (result.IndexOf(passive_phrase) != -1)
                {
                    if (active)
                    {
                        richTextBox_messages.Text += e.Result.Text + "\r\n";
                        ss.SpeakAsync(GetRandomStr(File.ReadAllLines(@"dictionary\Goodbay_Phrases.txt"), 0));
                        active = false;
                        return;
                    }
                }

            if (!active)
                return;

            foreach (string open_phrase in File.ReadAllLines(@"dictionary\Open_phrases.txt"))
                foreach (string str_program in File.ReadAllLines(@"dictionary\Programs.txt"))
                    if (result.IndexOf(str_program.Split('|')[0]) != -1 && result.IndexOf(open_phrase) >= 0 && !endSpeech)
                    {
                        ss.SpeakAsync("Открываю");
                        Process.Start(str_program.Split('|')[1]);
                        endSpeech = true;
                    }

            foreach (string open_phrase in File.ReadAllLines(@"dictionary\Open_phrases.txt"))
                foreach (string site in File.ReadAllLines(@"dictionary\Sites.txt"))
                {
                    if (result.IndexOf(site.Split('|')[0]) != -1 && result.IndexOf(open_phrase) >= 0 && !endSpeech)
                    {
                        ss.SpeakAsync("Открываю");
                        Process.Start(site.Split('|')[1]);
                        endSpeech = true;
                    }
                }

            foreach (string close_phrase in File.ReadAllLines(@"dictionary\Close_phrases.txt"))
                foreach (string str_program in File.ReadAllLines(@"dictionary\Programs.txt"))
                    if (result.IndexOf(str_program.Split('|')[0]) != -1 && result.IndexOf(close_phrase) >= 0 && !endSpeech)
                    {
                        string str = str_program.Replace("\\", "|");
                        string[] processes = str.Split('|');
                        string process = processes[processes.Length - 1].Replace(".exe", "");

                        Process[] proc = Process.GetProcessesByName(process);
                        if(proc.Length != 0)
                        foreach (Process pr in proc)
                        {
                            ss.SpeakAsync("Закрываю");
                            pr.Kill();
                        }

                        endSpeech = true;
                    }

            foreach (string quest_phrase in File.ReadAllLines(@"dictionary\QuestAnswer.txt"))
                if (result.IndexOf(quest_phrase.Split('|')[0]) != -1 && !endSpeech)
                {   
                    ss.SpeakAsync(GetRandomStr(quest_phrase.Split('|'),1));
                    endSpeech = true;
                }


            if (result.IndexOf("Расскажи анекдот") != -1 || result.IndexOf("Расскажи шутку") != -1 && !endSpeech)
            {
                ss.SpeakAsync(GetRandomStr(File.ReadAllLines(@"dictionary\Anecdotes.txt"),0));
                endSpeech = true;
            }

            if (result.IndexOf("Отправь") != -1)
                foreach (string vk_id in File.ReadAllLines(@"dictionary\VkIds.txt"))
                    foreach (string vk_message in File.ReadAllLines(@"dictionary\vk_samples.txt"))
                        if (result.IndexOf(vk_id.Split('|')[0]) != -1 && result.IndexOf(vk_message)!= -1 && !endSpeech)
                        {

                            long id = long.Parse(vk_id.Split('|')[1]);
                            vk.Messages.Send(new VkNet.Model.RequestParams.MessagesSendParams
                            {
                                UserId = id,
                                Message = vk_message
                            });

                            ss.SpeakAsync("Отправил");
                            endSpeech = true;
                        }


            if(result.IndexOf("Что там с погодой") != -1 &&
                result.IndexOf("Как там погода") != -1 &&
                result.IndexOf("Как погода") != -1 &&
                !endSpeech)
            {
                string url = "http://api.openweathermap.org/data/2.5/weather?q=Samara&units=metric&appid=ab5b104df8931de824bc07d41626e733";

                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);

                HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                string response;

                using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
                {
                    response = streamReader.ReadToEnd();
                }

                WeatherResponse weatherResponse = JsonConvert.DeserializeObject<WeatherResponse>(response);

                ss.SpeakAsync("Погода в Самаре " + weatherResponse.Main.Temp + " градусов пол Цельсию");
                endSpeech = true;
            }

            if (result.IndexOf("Сколько время") != -1 &&
                result.IndexOf("Какой час") != -1 &&
                result.IndexOf("Время") != -1
                && !endSpeech)
            {
                ss.SpeakAsync(DateTime.Now.ToShortTimeString());
                endSpeech = true;
            }


            if (result.IndexOf("Какой день недели") != -1 && !endSpeech)
            {
                string nameDay = "";
                switch(DateTime.Now.DayOfWeek)
                {
                    case DayOfWeek.Monday: nameDay = "Понедельник"; break;
                    case DayOfWeek.Tuesday: nameDay = "Вторник"; break;
                    case DayOfWeek.Wednesday: nameDay = "Среда"; break;
                    case DayOfWeek.Thursday: nameDay = "Четверг"; break;
                    case DayOfWeek.Friday: nameDay = "Пятница"; break;
                    case DayOfWeek.Saturday : nameDay = "Суббота"; break;
                    case DayOfWeek.Sunday: nameDay = "Воскресенье"; break;
                }

                ss.SpeakAsync(nameDay);
                endSpeech = true;
            }

            if(endSpeech == true)
            {
                richTextBox_messages.Text += e.Result.Text + "\r\n";
                endSpeech = false;
            }
        }

        public string GetRandomStr(string[] strings, int i)
        {
            int z = new Random().Next(i, strings.Length);
            return strings[z];
        }
        public class TemperatureInfo
        {
            public float Temp { get; set; }
        }
        public class WeatherResponse
        {
            public TemperatureInfo Main { get; set; }
        }
    }
}