using System;
using System.Net;
using System.IO;
//using System.Windows.Forms;
using Npgsql;


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

            TestScrape(5); // # of pages to scrape
            //TestConnection();
            //SqlOverConnection();
            //string[] person = new string[4] { "Paul Atreides", "15", "Caladan", "bloody messiah" };
           // InsertMyData("theCakeIsALie", person);
        }

        private static NpgsqlConnection GetConnection(string password)
        {
            return new NpgsqlConnection($"Server=localhost;Port=5432;User ID=postgres;Password={password};Database=bachbase;");
        }

        private static void InsertMyData(string password, string[] person)
        {
            using NpgsqlConnection con = GetConnection(password);
            con.Open();
            var sql = "INSERT INTO contestants(name, age, hometown, occupation) VALUES(@name, @age, @hometown, @occupation)";
            using (var cmd3 = new NpgsqlCommand(sql, con))
            {
                cmd3.Parameters.AddWithValue("name", person[0]);
                cmd3.Parameters.AddWithValue("age", Convert.ToInt32(person[1]));
                cmd3.Parameters.AddWithValue("hometown", person[2]);
                cmd3.Parameters.AddWithValue("occupation", person[3]);
                cmd3.Prepare();
                cmd3.ExecuteNonQuery();
                
             
            }
            con.Close();
        }


        private static void SqlOverConnection()
        {
            //https://www.npgsql.org/doc/basic-usage.html parameters  
            Console.WriteLine("database password:");
            string password = Console.ReadLine();
            using NpgsqlConnection con = GetConnection(password);
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = "INSERT INTO contestants (Name) VALUES ('Celine')"; /*(0,'" + password + "')";*/ //vulnerable to sql injection
            cmd.Connection = con;
            con.Open();

            var sql = "INSERT INTO contestants(Name, Occupation) VALUES(@name, @price)";
            using (var cmd3 = new NpgsqlCommand(sql, con))
            {

                cmd3.Parameters.AddWithValue("name", "BMW");
                cmd3.Parameters.AddWithValue("price", 36600);
                cmd3.Prepare();
                cmd3.ExecuteNonQuery();
            }





            /*
            using var cmd2 = new NpgsqlCommand("INSERT INTO table (col1) VALUES (@p1), (@p2)", con)
            {
                Parameters =
                {
                    new("p1", "some_value"),
                    new("p2", "some_other_value")
                };
            }
            */



            try
                {
                    int aff = cmd.ExecuteNonQuery();
                    Console.WriteLine(aff + " rows were affected.");
                }
                catch
                {
                    //MessageBox.Show("Error encountered during INSERT operation.");
                    Console.WriteLine("failed attempted INSERT :(");
                }
                finally
                {
                    con.Close();
                }

            }

    private static void TestConnection()
        {
            Console.WriteLine("database password:");
            string password = Console.ReadLine();

            using (NpgsqlConnection con = GetConnection(password))
            {
                con.Open();
                if (con.State == System.Data.ConnectionState.Open)
                {
                    Console.WriteLine("connection worked!");
                    

                }
                else { Console.WriteLine("unable to connect"); }
            }
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
            //Console.WriteLine("database password:");
            string password = "theCakeIsALie"; //Console.ReadLine();
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
                    string start = "inactive tile--person tile--landscape";
                    string end = "href=\"/shows";
                    string[,] players = new string[35, 5]; //??Need to declare empty array {}; how to do max or know how big to make?
                                                           //players[0] = messySplit[0];//??? why can't this be assigned?
                    string[,] playersClean = new string[,] { }; //can I fill this with any number of string[]s?

                    

                    int i = 0;
                    foreach (string person in messySplit)
                    {
                        int s = person.IndexOf(start);
                        int e = person.IndexOf(end);
                        //Console.WriteLine("s, e: " + s + ", " + e); // are the start and end keys working?

                        if (s != -1 && e != -1)
                        {
                            string personInfo = person.Substring(s + start.Length, e - (s + start.Length));
                            string[] splitPerson = personInfo.Split(";br /&gt");
                            //Console.WriteLine(splitPerson); //built in method for printing string array?
                            //Console.WriteLine(splitPerson[0]);
                            players[i, 0] = person.Substring(s + start.Length + 1, e - (s + start.Length + 3));
                            Console.WriteLine(person.Substring(s + start.Length + 1, e - (s + start.Length + 3)));
                        }
                        i = i + 1;
                        
                    }
                    //Console.WriteLine(players[1,0]);
                    Console.WriteLine($"finished gathering data on {i} players.");


                    foreach (string player in players)
                    {
                        //Console.WriteLine(player);
                        if (player != null)
                        {
                           // Console.WriteLine(player);
                            string split = "&lt;br /&gt; ";
                            if (player.IndexOf(split) != -1)
                            {
                                // pesky names with initials
                                string s1;
                                if ((player.Split(split)[0].Trim()).IndexOf(".") == -1)
                                {
                                    s1 = " ";
                                }
                                else
                                {
                                    s1 = ". ";
                                }

                                Console.WriteLine("I am here");
                                string[] person = new string[4];
                                person[0] = (player.Split(split)[0].Trim()).Split(s1)[0];
                                person[1] = (player.Split(split)[0].Trim()).Split(s1)[1];
                                person[2] = player.Split(split)[1].Trim();
                                person[3] = player.Split(split)[2].Trim();
                                Array.ForEach(person, Console.WriteLine);
                                InsertMyData(password, person);




                                string line = (player.Split(split)[0].Trim()).Split(s1)[0]
                                    + "," + (player.Split(split)[0].Trim()).Split(s1)[1]
                                    + "," + player.Split(split)[1].Trim()
                                    + ",'" + player.Split(split)[2].Trim() + "'";
                                //Console.WriteLine(line);
                                // fixed StreamWriter to append by adding true as second argument
                                using (StreamWriter writer = new StreamWriter("C:/Users/ceilp/OneDrive/Documents/Programming/contestants.txt", true))
                                {
                                    writer.WriteLine(line);
                                }

                                
                               
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
