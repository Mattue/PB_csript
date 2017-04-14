using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace PB_script
{
    class Program
    {

        static string get_data_from_line(string line)
        {
            var match = Regex.Match(line, @"(?<=(\>))(\d|\.|\,)*");
            if (match.Success)
            {
                //Console.WriteLine(match.ToString());
                return line = match.ToString();
            }

            return null;
        }

        static void Main()
        {
            Console.WriteLine("Hello!");
            string input_file_count;
            Console.Write("Enter file count: ");
            input_file_count = Console.ReadLine();
            
            //readed line
            string line = "";
            //line with data only
            string data_line = "";

            //parametrs
            string data_param = "";
            string acc_param = "";

            //result line
            string exit_line = null;

            StreamReader file;

            //open input file

            //reading parametrs

            bool incert = true;

            Console.Write("Enter data_param :");

            while (incert)
            {
                data_param = Console.ReadLine();
                if (data_param.Length == 10)
                {
                    incert = !incert;
                }
                else
                {
                    Console.WriteLine("Not data entrered! Enter again!");
                }
            }

            data_param = data_param.Substring(2);

            incert = true;

            Console.Write("Enter acc_param :");

            while (incert)
            {
                acc_param = Console.ReadLine();
                if (acc_param.Length == 5)
                {
                    incert = !incert;
                }
                else
                {
                    Console.WriteLine("Not acc_param entrered! Enter again!");
                }
            }

            string output_file_name;

            Console.Write("Enter output file name: ");
            output_file_name = Console.ReadLine();

            StreamWriter sw = new StreamWriter(output_file_name + ".txt", true, System.Text.Encoding.Default);

            Console.WriteLine("Working");

            int i = 1;

            while (i <= Convert.ToInt32(input_file_count))
            {
                Console.WriteLine("Reading file " + i);

                try
                {
                    file = new StreamReader(i + ".ttt");
                }
                catch (FileNotFoundException)
                {
                    Console.WriteLine("File Not Found.");
                    Console.ReadLine();
                    return;
                }

                //working cicle
                while ((line = file.ReadLine()) != null)
                {

                    //geting needed line
                    var match = Regex.Match(line, @"(DATA)+|(DT_ACCOUNT)+|(KT_ACCOUNT)+|(SUM)+");
                    if (!match.Success)
                    {
                        continue;
                    }

                    //get data from line
                    data_line = get_data_from_line(line);

                    //check for data equals
                    if (line.IndexOf("<DATA>") >= 0)
                    {
                        if (exit_line != null)
                        {
                            //Console.WriteLine(exit_line.Substring(0, exit_line.Length - 1));
                            sw.WriteLine(exit_line.Substring(0, exit_line.Length - 1));
                        }

                        exit_line = null;
                        if (data_line.IndexOf(data_param) >= 0)
                        {
                            exit_line = exit_line + data_line + ",";
                            continue;
                        }
                        else
                        {
                            continue;
                        }
                    }

                    //check for params equals
                    if (exit_line != null)
                    {
                        if (line.IndexOf("KT_ACCOUNT") >= 0)
                        {
                            exit_line = exit_line + data_line + ",";
                            if (!(exit_line.IndexOf(acc_param) >= 0))
                            {
                                exit_line = null;
                                continue;
                            }
                        }

                        exit_line = exit_line + data_line + ",";
                    }

                }

                //print last line
                if (exit_line != null)
                    sw.WriteLine(exit_line.Substring(0, exit_line.Length - 1));

                file.Close();

                i++;
            }

            Console.WriteLine("Finished");

            sw.Close();

            Console.ReadLine();

            Console.WriteLine("Repeat?");
            if (Console.ReadLine() == "y")
            {
                Main();
            }
        }
    }
}