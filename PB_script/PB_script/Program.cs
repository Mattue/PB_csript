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
            string input_file_name;
            Console.Write("Enter file name: ");
            input_file_name = Console.ReadLine();

            string line = "";
            string data_line = "";

            string data_param = "";
            string acc_param = "";

            string exit_line = null;

            StreamReader file;

            try
            {
                file = new StreamReader(input_file_name);
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("File Not Found.");
                Console.ReadLine();
                return;
            }

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

            StreamWriter sw = new StreamWriter("output.txt");

            //
            while ((line = file.ReadLine()) != null)
            {
                var match = Regex.Match(line, @"(DATA)+|(DT_ACCOUNT)+|(KT_ACCOUNT)+|(SUM)+");
                if (!match.Success)
                {
                    continue;
                }

                data_line = get_data_from_line(line);
                //Console.WriteLine(data_line);

                if (line.IndexOf("<Rec") >= 0)
                {
                    Console.WriteLine("REC");
                    continue;
                }

                if (line.IndexOf("<DATA>") >= 0)
                {
                    if (exit_line != null)
                    {
                        Console.WriteLine(exit_line.Substring(0, exit_line.Length - 1));
                        sw.WriteLine(exit_line.Substring(exit_line.Length - 1, 1));
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

                if (exit_line != null)
                {
                    exit_line = exit_line + data_line + ",";
                }

                /*if (exit_line != null)
				{
					if ((line.IndexOf("DT_ACCOUNT") >= 0)  || (line.IndexOf("KT_ACCOUNT") >= 0))
					{
						if (data_line.IndexOf(acc_param) >= 0)
						{
							exit_line = exit_line + data_line + ",";
						}
					}
				}*/

                //sw.Write(bata_line);
                //sw.Write(",");

                //if (line.IndexOf("SUM") >= 0)
                //sw.WriteLine();
            }

            if (exit_line != null)
                sw.WriteLine(exit_line.Substring(exit_line.Length - 1, 1));

            Console.WriteLine(exit_line);

            sw.Close();
            file.Close();

            Console.ReadLine();
        }
    }
}