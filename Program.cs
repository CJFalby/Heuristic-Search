using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Runtime.InteropServices;
using System.Reflection.PortableExecutable;
using System.Drawing;

namespace AlgorithmsAndDataStructures
{
    class Project6
    {
        public static void Main()
        {
            double[,] lorries = new double[3, 30];
            int numOfRestarts = 0;
            double[] brickWeights = readInData();
            lorries = randomRestart(brickWeights, numOfRestarts, lorries);
            lorryWeights(brickWeights, lorries);
        }
        //read in data from csv file
        public static double[] readInData()
        {
            string filePath = "C:\\Users\\charl\\OneDrive\\Desktop\\Term 1\\Algorithms and Data Structures\\Coursework\\Project 6\\dataset2.csv";

            StreamReader reader = new StreamReader(File.OpenRead(filePath));
            string[] brickWeightsString = new string[30];
            double[] brickWeights = new double[30];
            int count = 0;

            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                brickWeightsString[count] += line;
                count++;
            }

            count = 0;
            foreach (string item in brickWeightsString)
            {
                double weightDouble = Convert.ToDouble(item);
                brickWeights[count] += weightDouble;
                count++;
            }
            return brickWeights;
        }
        //sorts the bricks into the lorries
        public static double[,] randomRestart(double[] brickWeights, int numOfRestarts, double[,] lorries)
        {
            Random rnd = new Random();
            double fitness = 0;
            int count0 = 0;
            int count1 = 0;
            int count2 = 0;
            for (int i = 0; i < 1000; i++)
            {
                //if first round randomly assign each number in the array to lorry 0-2
                if (numOfRestarts == 0)
                {
                    foreach (double item in brickWeights)
                    {
                        int intoArray = rnd.Next(0, 3);//0-2
                        if (intoArray == 0)
                        {
                            lorries[intoArray, count0] += item;
                            count0++;
                        }
                        else if (intoArray == 1)
                        {
                            lorries[intoArray, count1] += item;
                            count1++;
                        }
                        else
                        {
                            lorries[intoArray, count2] += item;
                            count2++;
                        }
                    }
                    fitness = lorryWeights(brickWeights, lorries);
                    numOfRestarts++;
                }
                else
                {
                    int count = 0;
                    double change = fitness * 100;
                    //make sure to round to integer
                    change = Math.Round(fitness);
                    int changeChance = 100 - Convert.ToInt32(change);
                    int changeOrNo = 0;
                    foreach (double item in lorries)
                    {
                        changeOrNo = rnd.Next(0, changeChance);
                        //theres a 1 in changeChance chance it will need to be swapped 
                        // so if its 0 the position lorry the brickpile is in will move
                        if (changeOrNo == 0)
                        {
                            count = count % 30;
                            if (lorries[0, count] == item && lorries[0, count] != 0)
                            {
                                lorries[0, count] = 0;
                                for (int j = 0; j < 29; j++)
                                {
                                    if (lorries[1, j] == 0)
                                    {
                                        lorries[1, j] = item;
                                        count++;
                                        break;
                                    }
                                }
                            }
                            else if (lorries[1, count] == item && lorries[1, count] != 0)
                            {
                                lorries[1, count] = 0;
                                for (int j = 0; j < 29; j++)
                                {
                                    if (lorries[2, j] == 0)
                                    {
                                        lorries[2, j] = item;
                                        count++;
                                        break;
                                    }
                                }
                            }
                            else if (lorries[2, count] == item && lorries[2, count] != 0)
                            {
                                lorries[2, count] = 0;
                                for (int j = 0; j < 29; j++)
                                {
                                    if (lorries[0, j] == 0)
                                    {
                                        lorries[0, j] = item;
                                        count++;
                                        break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            count++;
                        }
                    }
                    fitness = lorryWeights(brickWeights, lorries);
                    numOfRestarts++;
                }
            }
            return lorries;
        }

        //finds totalweight of each lorry
        public static double lorryWeights(double[] brickWeights, double[,] lorries)
        {
            double lorry0 = 0;
            double lorry1 = 0;
            double lorry2 = 0;
            double sum = 0;
            foreach (double item in brickWeights)
            {
                sum += item;
            }
            double average = sum / 3;
            Console.WriteLine(average);

            for (int lorry = 0; lorry < 3; lorry++)
            {
                for (int brickWeight = 0; brickWeight < 30; brickWeight++)
                {
                    if (lorry == 0)
                    {
                        lorry0 += lorries[lorry, brickWeight];
                    }
                    else if (lorry == 1)
                    {
                        lorry1 += lorries[lorry, brickWeight];
                    }
                    else
                    {
                        lorry2 += lorries[lorry, brickWeight];
                    }
                }
            }

            Console.WriteLine("lorry1  :" + lorry0);
            Console.WriteLine("lorry2  :" + lorry1);
            Console.WriteLine("lorry3  :" + lorry2);

            double fitness = getFitness(average, lorry0, lorry1, lorry2);

            return fitness;
        }
        //the percentage of how close it is to the average
        public static double getFitness(double average, double lorry0, double lorry1, double lorry2)
        {
            double diff = Math.Abs(lorry0 - average) + Math.Abs(lorry1 - average) + Math.Abs(lorry2 - average);
            diff /= 3;
            Console.WriteLine("diff: " + diff);
            double fitness = (diff / average);
            Console.WriteLine("fitness: " + fitness);

            return fitness;
        }
    }
}










