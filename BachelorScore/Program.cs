using System;
using System.Net;
using System.IO;
//using System.Windows.Forms;


namespace BachelorScore
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
            Console.WriteLine("Enter contestant age:");
            double age = Convert.ToDouble(Console.ReadLine());
            double young = age / 2.0 + 7;
            Console.WriteLine($"You can date someone as young as {Math.Round(young, 1)}.");
            Console.WriteLine($"But how old can you date??\nAs old as...{Math.Round(2.0 * (age - 7))}.");
            */
            string smallTest = "DivideXXXXbyXXXXthisXXXX99.";
            int firstOccurence = smallTest.IndexOf("X");
            //Console.WriteLine(smallTest.Substring(firstOccurence, smallTest.Length));
            
            TestScrape(1); // # of pages to scrape
        }

        static void TestScrape(int pMax)
        {
            Console.WriteLine($"scrape up to {pMax} pages looking for contestants");

            string url = "https://abc.com/shows/the-bachelorette/cast?page=";
            for (int p = 1; p < pMax + 1; p++)
            {
                string pageUrl = url + p + "#";
                ScrapePage(pageUrl);
            }
        }

        static void ScrapePage(string url)
        {
            string result = null;
            System.Net.WebResponse response = null;
            StreamReader reader = null;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                response = request.GetResponse();
                reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.UTF8);
                result = reader.ReadToEnd();
                string secStart = "AnchorLink tile tile--hero";
                // problem: when page does not have contestants abc.com still sends response
                if (result.IndexOf(secStart) == -1)
                {
                    Console.WriteLine("...no contestants on page");
                }
                else
                {
                    string[] messySplit = result.Split(secStart);
                    string end = "style=\"animation-delay";
                    string start = "inactive tile--person tile--landscape\" tabindex=\"0\" aria-label=";
                    string[,] players = new string[35, 5]; //??Need to declare empty array {}; how to do max or know how big to make?
                                                           //players[0] = messySplit[0];//??? why can't this be assigned?
                    int i = 0;
                    foreach (string person in messySplit)
                    {
                        int s = person.IndexOf(start);
                        int e = person.IndexOf(end);


                        if (s != -1 && e != -1)
                        {
                            string personInfo = person.Substring(s + start.Length, e - (s + start.Length));
                            string[] splitPerson = personInfo.Split(";br /&gt");
                            //Console.WriteLine(splitPerson); //built in method for printing string array?
                            //Console.WriteLine(splitPerson[0]);
                            players[i, 0] = person.Substring(s + start.Length + 1, e - (s + start.Length + 3)); 
                        }
                        i = i + 1;
                        
                    }
                    //Console.WriteLine(players[0].Length);
                    Console.WriteLine($"finished gathering data on {i} players.");


                    foreach (string player in players)
                    {
                        
                        if (player != null)
                        {
                            Console.WriteLine(player);
                            string split = "&lt;br /&gt; ";
                            if (player.IndexOf(split) != -1)
                            {
                                player.Split(split);
                                Console.WriteLine(player.Split(split)[0].Trim());
                                //players[0, 1] = player.Split(split)[0].Trim();
                                Console.WriteLine(player.Split(split)[1].Trim());
                                Console.WriteLine(player.Split(split)[2].Trim());
                            }

                        }
                    }
                }


                // Console.WriteLine(messySplit[4]);
                //Console.WriteLine(result);
                //File.WriteAllText("C:/Users/ceilp/OneDrive/Documents/Programming/test1.txt", result);

            }
            catch (Exception ex)
            {
                // handle error
                //MessageBox.Show(ex.Message);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                if (response != null)
                    response.Close();
            }


        }



    }
}
