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
            string[] pass1 = smallTest.Split("XXXX");
            int l = smallTest.Length;
            Console.WriteLine(smallTest.Substring(smallTest.IndexOf("X"), l - smallTest.IndexOf("X")));
            Console.WriteLine(pass1[2]);
            TestScrape();
        }

        static void TestScrape()
        {
            Console.WriteLine("Try to call me");
            string result = null;
            string url = "https://abc.com/shows/the-bachelorette/cast";
            System.Net.WebResponse response = null;
            StreamReader reader = null;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                response = request.GetResponse();
                reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.UTF8);
                result = reader.ReadToEnd();
                //Console.WriteLine(result);
                string secStart = "AnchorLink tile tile--hero";// - inactive tile--person tile--landscape";
                string[] messySplit = result.Split(secStart);
                Console.WriteLine(messySplit.Length);
                
                string end = "style=\"animation-delay";
                string start = "inactive tile--person tile--landscape\" tabindex=\"0\" aria-label=";

                string[,] players = new string[35,5]; //??Need to declare empty array {}; how to do max or know how big to make?
                //players[0] = messySplit[0];//??? why can't this be assigned?
                int i = 0;

                foreach (string person in messySplit)
                {
                    int s = person.IndexOf(start);
                    int e = person.IndexOf(end);
                    

                    if (s != -1 && e != -1)
                    {
                        Console.WriteLine($"start: {s}; i: {i}");
                        Console.WriteLine(person.Substring(s + start.Length, e - (s + start.Length)));
                        string personInfo = person.Substring(s + start.Length, e - (s + start.Length));
                        string[] splitPerson = personInfo.Split(";br /&gt");
                        Console.WriteLine("array length of " + splitPerson.Length);
                        //Console.WriteLine(splitPerson); //built in method for printing string array?
                        Console.WriteLine(splitPerson[0]);
                        //Console.WriteLine(splitPerson[1]); //?? can't print 2nd el in array?
                        //Console.WriteLine(splitPerson[2]);
                        players[i,0] = person.Substring(s + start.Length, e - (s + start.Length)); //?? why does this stop the loop when person array is 1-dim??
                    }
                    i = i + 1;
                    Console.WriteLine("New i: " + i);
                    Console.WriteLine("\n\n\n\n");
                }
                Console.WriteLine("out of loop");

                foreach (string player in players)
                {
                    if (player != null)
                    Console.WriteLine(player);
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
