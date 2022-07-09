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
                string secStart = "AnchorLink tile tile--hero - inactive tile--person tile--landscape";
                string[] messySplit = result.Split(secStart);
                Console.WriteLine(messySplit[1]);

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
