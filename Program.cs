using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System.Threading;

namespace TwitchSpamBot
{
    class Program
    {
        static Ledger[] ledger;
        static int[] ViewLimit;
        static IWebDriver driver;
        static int AdViews = 0;
        static string MSG;

        static void Main(string[] args)
        {

            start: // (☞ﾟ∀ﾟ)☞  ¯\_(ツ)_/¯ ¯\_(ツ)_/¯ (▀̿Ĺ̯▀̿ ̿)

            init();

            //ENGINE

            for (int i = 0; i < ledger.Length; i++)
            {
                ConnecToStream(ledger[i].Name);
                SendMessage(MSG);
                AnalyseResponse(i);
            }

            for (int i = 0; i < ledger.Length; i++)
            {
                if (ledger[i].visited == false)
                {
                    while (true)
                    {
                        if (Convert.ToDateTime(ledger[i].AcessTime) < DateTime.Now){
                            break;
                        }
                        else{
                            Thread.Sleep(10000);
                        }
                    }

                    ConnecToStream(ledger[i].Name);
                    SendMessage(MSG);
                    AnalyseResponse(i);
                }
            }

  //          driver.Quit();

            Console.ReadLine();
            goto start;
        }

        static void init()
        {
            {
                Console.Title = "TwitchSpammer By NotARobot";
                Console.ForegroundColor = ConsoleColor.Green;

                Console.WriteLine("  _   _           _                _____            _               _   ");
                Console.WriteLine(" | \\ | |         | |       /\\     |  __ \\          | |             | |  ");
                Console.WriteLine(" |  \\| |   ___   | |_     /  \\    | |__) |   ___   | |__     ___   | |_ ");
                Console.WriteLine(" | . \\`|  / _ \\  | __|   / /\\ \\   |  _  /   / _ \\  | '_ \\   / _ \\  | __|");
                Console.WriteLine(" | |\\  | | (_) | | |_   / ____ \\  | | \\ \\  | (_) | | |_) | | (_) | | |_ ");
                Console.WriteLine(" |_| \\_|  \\___/  \\__|  /_/    \\_\\ |_|  \\_\\  \\___/  |_.__/   \\___/  \\__|");
                Console.WriteLine("                                                                        ");
                Console.WriteLine();
            }

            begin:
            Console.Write("Choose game: ");
            string game = "fortnite";//Console.ReadLine();

            ViewLimit = new int[2];
            Console.Write("Set max viewers: ");
            ViewLimit[0] = Convert.ToInt32(Console.ReadLine());
            Console.Write("Set min viewers: ");
            ViewLimit[1] = Convert.ToInt32(Console.ReadLine());

            Console.Write("Spamming message: ");
            MSG = Console.ReadLine();
            Console.WriteLine();

            Console.WriteLine("Game: " + game);
            Console.WriteLine("Max view: " + ViewLimit[0]);
            Console.WriteLine("Max view: " + ViewLimit[1]);
            Console.WriteLine("Spam message: '" + MSG + "'");

            Console.Write("Are these correct settings? (y/n): ");
            string vertification = Console.ReadLine();

            if (vertification != "y")
            {
                goto begin;
            }

            ChromeOptions options = new ChromeOptions();
            ChromeDriverService service = ChromeDriverService.CreateDefaultService();
            service.SuppressInitialDiagnosticInformation = true;

            driver = new ChromeDriver(service, options);
            driver.Navigate().GoToUrl(Global.twitch + game);
            LogIn();

            Console.Write("Log in to the twitch and press Enter ...");
            Console.ReadLine();

            SeleniumHelper.WaitToLoadElements(".stream-thumbnail", driver);
            ledger = GetOnlineStreamers(ViewLimit);

            Console.WriteLine("Detected " + ledger.Length +" streamers from " + ViewLimit[0] + " to " + ViewLimit[1] + " viewers.");
        }

        static Ledger[] GetOnlineStreamers(int[] ViewLimit)
        {
            Ledger[] ledger = null;
            List<Targets> targets = new List<Targets>();
            Targets target = new Targets();
            bool EverythingLoaded = false;
            IList<IWebElement> all = driver.FindElements(By.CssSelector(".preview-card-stat.tw-c-text-overlay.tw-flex.tw-pd-x-05"));
            IList<IWebElement> all_names = driver.FindElements(By.CssSelector(".tw-link.tw-link--inherit"));

            if (all.Count == all_names.Count/2)
            {
                while (EverythingLoaded == false)
                {

                    int viewers = 0;
                    for (int i = 0; i < all.Count; i++)
                    {

                        string NamesPartion = all_names[(i*2)+1].Text;
                        string[] ViewsPartion = all[i].Text.Split(' ');

                        viewers = Convert.ToInt32(ViewsPartion[0].Replace(",", string.Empty));

                        if (viewers < ViewLimit[0] && viewers > ViewLimit[1])
                        {
                            target.name = NamesPartion;
                            target.Views = viewers;
                            targets.Add(target);
                        }

                    }

                    if (viewers > ViewLimit[1])
                    {
                        SeleniumHelper.ScrollToLast(all.Count - 1, driver);
                        Thread.Sleep(300);
                        all = driver.FindElements(By.CssSelector(".preview-card-stat.tw-c-text-overlay.tw-flex.tw-pd-x-05"));
                        all_names = driver.FindElements(By.CssSelector(".tw-link.tw-link--inherit"));
                    }
                    else
                    {
                        EverythingLoaded = true;
                    }

                }

                ledger = new Ledger[targets.Count];

                for (int i = 0; i < targets.Count; i++)
                {
                    ledger[i].Name = targets[i].name;
                    ledger[i].viewers = targets[i].Views;
                    ledger[i].AcessTime = null;
                    ledger[i].visited = false;
                }
            }
            else
            {
                Console.WriteLine("Listen my. I/language pls/ so I'm going to make this as short as posible. Well names count arent equalt to views count. So fix it in code please.Ja chcem");
            }

            return ledger;
        }

        static void SetTimeout(int time,int which )
        {
            ledger[which].AcessTime = DateTime.Now.AddMinutes(time + 1);
        }

        static void RemoveSubject(int witch)
        {
            ledger[witch].visited = true;
        }

        static void ConnecToStream(string name)
        {
            driver.Navigate().GoToUrl(Global.twitch_ + name);
            SeleniumHelper.WaitToLoadElements(".room-selector__header.tw-align-items-center.tw-border-b.tw-border-l.tw-border-r.tw-c-background-alt-2.tw-flex.tw-flex-shrink-0.tw-full-width.tw-justify-content-between.tw-pd-l-2.tw-pd-r-1", driver);
            SeleniumHelper.WaitToLoadElements(".tw-textarea.tw-textarea--no-resize", driver);
        }

        static void SendMessage(string text)
        {
            IWebElement TextField = driver.FindElement(By.CssSelector(".tw-textarea.tw-textarea--no-resize"));
            TextField.SendKeys(text);
            Thread.Sleep(1000);
            IWebElement button = driver.FindElement(By.CssSelector("[data-a-target='chat-send-button']"));
            button.Click();
        }

        static void Follow()
        {
            IList<IWebElement> buttons = driver.FindElements(By.ClassName("tw-button__text"));
            foreach (var item in buttons)
            {
                if (item.Text == "Follow")
                {
                    item.Click();
                }
            }

            Thread.Sleep(2000);
        }

        static void AnalyseResponse(int i)
        {
            Thread.Sleep(5000);
            try
            {
                IList<IWebElement> responses = driver.FindElements(By.CssSelector(".chat-line__status"));
                IWebElement response = responses[0];



                foreach (var item in responses)
                {
                    string[] IwantgoSleep = item.Text.Split(' ');
                    if (IwantgoSleep[0] == "This")
                    {
                        response = item;
                    }
                }

                string text = response.Text;
                string[] dataText = text.Split(' ');

                if (StringOperations.ContainWord(dataText, "Follow"))
                {
                    int minutes = StringOperations.IsNumericAndReturnMinutes(dataText);

                    if (minutes > 0 && StringOperations.ContainWord(dataText, "minutes"))
                    {
                        Follow();
                        SetTimeout(minutes, i);
                        Console.WriteLine("Followed " + ledger[i].Name + " and set timeout for "+minutes+" minutes");

                    }
                    else if(dataText[4] == "followers-only")
                    {
                        Follow();
                        SetTimeout(1, i);
                        Console.WriteLine("Followed " + ledger[i].Name);
                    }
                    else
                    {
                        RemoveSubject(i);
                        Console.WriteLine(ledger[i].Name + " was removed (probably subscribe only mode) " + minutes);
                    }
                }
                else
                {
                    if (dataText[0] != "This")
                    {
                        AdViews += ledger[i].viewers;
                        Console.WriteLine(ledger[i].Name + " was spammed (-<*_*)-<  get " + ledger[i].viewers +" views. Total: "+AdViews);
                    }
                    else
                    {
                        Console.WriteLine(ledger[i].Name + " was removed for whatever reason");
                    }
                    RemoveSubject(i);
                }
            }
            catch
            {
                AdViews += ledger[i].viewers;
                RemoveSubject(i);
                Console.WriteLine(ledger[i].Name + " was spammed (-<*_*)-<  get " + ledger[i].viewers + " views. Total: " + AdViews);

            }
        } 

        static void LogIn()
        {
            IList<IWebElement> buttons = driver.FindElements(By.ClassName("tw-button__text"));
            foreach (var item in buttons)
            {
                if (item.Text == "Log in")
                {
                    item.Click();
                }
            }
        }
    }
}
