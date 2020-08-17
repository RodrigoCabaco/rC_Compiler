﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.InteropServices;

namespace rC
{
    public class rCompiler
    {
        //receive every variable for further changes;
        public static void Compile(List<string> code, 
            List<string> numberNames, 
            List<double> numberValues, 
            List<string> strNames, 
            List<string> strValues)
        {

            //values and indicators
           
            int id = 0;
            //read code line by line
            foreach (var line in code)
            {
                if (line.Contains("number") && line.Contains(">>") && line.ToLower().StartsWith("for") == false && line.ToLower().Contains("in range %") == false && line.Contains("$>") == false)
                {
                    try
                    {
                        if (numberNames.Contains(line.Split(' ')[1].Split('>').First()))
                        {
                            numberValues[numberNames.IndexOf(line.Split(' ')[1].Split('>').First())] = Convert.ToDouble(line.Split('>').Last().Split(' ').Last());
                        }
                        else
                        {
                            numberNames.Add(line.Split(' ')[1].Split('>').First());
                            numberValues.Add(Convert.ToDouble(line.Split('>').Last().Split(' ').Last()));
                        }
                    }
                    catch
                    {
                        int errorLine = code.IndexOf(line);
                        Console.WriteLine($"Invalid Syntax (Line {errorLine--})");
                    }
                }

                if (line.Contains("str") && line.Contains(">>") && line.ToLower().StartsWith("for") == false && line.ToLower().Contains("in range %") == false && line.Contains("$>") == false)
                {
                    try
                    {
                        if (strNames.Contains(line.Split(' ')[1].Split('>').First()))
                        {
                            strValues[strNames.IndexOf(line.Split(' ')[1].Split('>').First())] = line.Split('>').Last();
                        }
                        else
                        {
                            strNames.Add(line.Split(' ')[1].Split('>').First());
                            strValues.Add(line.Split('>').Last());
                        }

                    }

                    catch
                    {
                        int errorLine = code.IndexOf(line);
                        Console.WriteLine($"Invalid Syntax (Line {errorLine--})");
                    }
                }

                if (line.Contains("list(str)") && line.ToLower().StartsWith("for") == false && line.ToLower().Contains("in range %") == false && line.Contains("$>") == false)
                {
                    List<List<string>> listsStr = new List<List<string>>();
                }






                //Write 

                if (line.ToLower().Contains("newline") || line.ToLower().Contains("newln"))
                {
                    Console.WriteLine("");
                }
                if (line.Contains("Write") && line.Contains("&>") && line.Contains("<&") && line.Contains("WriteStr") == false && line.Contains("WriteNum") == false && line.ToLower().StartsWith("for") == false && line.ToLower().Contains("in range %") == false && line.Contains("$>") == false)
                {
                    //check if it is a number 
                    var matchesNumber = numberNames.Where(x => line.Contains(line.Split(new[] { "Write &>" }, StringSplitOptions.None).Last().ToString().Split(new[] { "<&" }, StringSplitOptions.None).First().ToString()));

                    Console.Write(line.Split(new[] { "Write &>" }, StringSplitOptions.None).Last().ToString().Split(new[] { "<&" }, StringSplitOptions.None).First().ToString() + " ");

                }
                else if (line.Contains("WriteStr") && line.Contains("&>") && line.ToLower().StartsWith("for") == false && line.ToLower().Contains("in range %") == false && line.Contains("$>") == false)
                {
                    foreach (var name in strNames)
                    {
                        try
                        {
                            if (line.Contains(name))
                            {
                                Console.Write(strValues[strNames.IndexOf(name)] + line.Split(new[] { name }, StringSplitOptions.None)[1].Split(new[] { "<&" }, StringSplitOptions.None).First());
                            }
                        }
                        catch
                        {
                            Console.WriteLine("Invalid Syntax on Line " + code.IndexOf(line));
                        }
                    }

                }
                else if (line.Contains("WriteNum") && line.Contains("&>") && line.ToLower().StartsWith("for") == false && line.ToLower().Contains("in range %") == false && line.Contains("$>") == false)
                {
                    foreach (var name in numberNames)
                    {
                        if (line.Contains(name))
                        {
                            try
                            {
                                Console.Write(numberValues[numberNames.IndexOf(name)] + line.Split(new[] { name }, StringSplitOptions.None)[1].Split(new[] { "<&" }, StringSplitOptions.None).First());
                            }
                            catch
                            {
                                Console.WriteLine("Invalid Syntax on Line " + code.IndexOf(line));
                            }
                        }
                    }





                    //save

                }
                else if (line == "save(this)")
                {
                    id = new Random().Next(1, 10000);
                    StreamWriter writer = File.CreateText("code.id(" + id + ").txt");
                    foreach (var lineToSave in code)
                    {
                        writer.WriteLine(lineToSave);
                    }
                    if (id != 0)
                    {
                        Program.WriteId(id);
                    }

                    writer.Close();
                }
                else if (line == "save = false")
                {

                }

                //TODO REMOVE CODE WHEN EXECUTED
                else if (line.ToLower().StartsWith("for") && line.ToLower().Contains("in range %") && line.Contains("$>"))
                {
                    int range = Convert.ToInt32(line.ToLower().Split(new[] { "in range %" }, StringSplitOptions.None).Last().Split(new[] { " $>" }, StringSplitOptions.None).First());
                    string looper = line.ToLower().Split(new[] { "for" }, StringSplitOptions.None).Last().Split(new[] { "in range %" }, StringSplitOptions.None).First();
                    var getContent = line.Split(new[] { "$>" }, StringSplitOptions.None);
                    List<string> loopContent = new List<string>();

                    foreach (var content in getContent)
                    {
                        if (content.ToLower().StartsWith("for") == false && content.ToLower().Contains("in range %") == false && content.Contains("$>") == false)
                        {
                            loopContent.Add(content);
                        }
                    }
                    ForLoop(range, looper, loopContent, numberNames, numberValues, strNames, strValues);
                }







            }

        }
        //receive every variable;
        public static void ForLoop(int range, 
            string looper, 
            List<string> loopContent, 
            List<string> numberNames,
            List<double> numberValues, 
            List<string> strNames, 
            List<string> strValues)
        {
            for (int x = 0; x < range; x++)
            {
                Compile(loopContent, numberNames, numberValues, strNames, strValues);
            }
        }

    }
}
