using System;
using System.IO;

namespace LevelImporter 
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Console.WriteLine("nu kör vi!");

            Importer();
          

        }

        static bool Importer()
        {
            FileStream fsOut = File.OpenWrite("./../../../levels.js");
            StreamReader fsIn = new StreamReader("./../../../Original.txt");
            int gameboards = 0;
            int width = 0;
            int height = 0;
            char[,] spelplan = new char[20, 20] {   {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},
                                                        {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},
                                                        {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},
                                                        {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},
                                                        {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},
                                                        {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},
                                                        {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},
                                                        {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},
                                                        {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},
                                                        {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},
                                                        {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},
                                                        {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},
                                                        {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},
                                                        {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},
                                                        {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},
                                                        {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},
                                                        {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},
                                                        {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},
                                                        {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},
                                                        {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '}   };

            string topText = " /* SokoBan levels imported from text file using the LevelImporter "


            while (!fsIn.EndOfStream)
            {
                
                string currentLine = fsIn.ReadLine();

                if (currentLine.Length == 0)
                    continue;

                if (currentLine[0] == ';')
                {

                    Greenify(ref spelplan, width, height); //Adds some grass, if you want to


                    string outRow = "new TileMaps(" + width + "," + height + ", [";
                    
                    for (int i = 0; i < height; i++)
                    {
                        outRow += "[";
                        for (int j = 0; j < width; j++)
                        {
                            outRow += "\"" + spelplan[i, j] + "\"";
                            if (j < (width - 1)) //inget extra komma på slutet av raden
                                outRow += ", ";
                        }
                        outRow += "]";
                        if (i < (height - 1))
                            outRow += ",\n";
                        
                    }
                    outRow += " ]);\n";

                    byte[] outBytes = new System.Text.UTF8Encoding(true).GetBytes(outRow);
                    fsOut.Write(outBytes);
                    //fsOut.Write()

                    gameboards++;///
                    Console.WriteLine("Gameboard: " + gameboards + " done");
                    width = 0;
                    height = 0;

                    for (int k = 0; k < 20; k++)
                        for (int l = 0; l < 20; l++)
                            spelplan[k, l] = ' ';


                    continue;
                }

                for(int i = 0; i < currentLine.Length;i++)
                {
                    char currentChar = currentLine[i];
                    switch (currentChar)
                    {
                        case '#':
                            spelplan[height, i] = 'W';
                            break;
                        case '$':
                            spelplan[height, i] = 'B';
                            break;
                        case '@':
                            spelplan[height, i] = 'P';
                            break;
                        case '.':
                            spelplan[height, i] = 'G';
                            break;
                        default:
                            spelplan[height, i] = ' ';
                            break;
                    }
                    
                }

                width = Math.Max(width, currentLine.Length);
                height++;


             }

            fsOut.Close();
            fsIn.Close();
            return true;
        }

        static bool Greenify(ref char[,] spelplan, int width, int height) 
        {

            for (int i = 0; i < height; i++) //left and right 
            {
                if (spelplan[i, 0] == ' ')
                {
                    spelplan[i, 0] = '.';
                    Grass(ref spelplan, i, 1, width, height); // And spread from there
                }
                if (spelplan[i, width - 1] == ' ')
                {
                    spelplan[i, width - 1] = '.';
                    Grass(ref spelplan, i, width - 2, width, height);
                }

                }
            
            for (int j = 0; j < width; j++) // top and bottom
            {
                if (spelplan[0, j] == ' ')
                {
                    spelplan[0, j] = '.';
                    Grass(ref spelplan, 1, j, width, height); // And spread from there (in case of u shaped levels)

                }
                if (spelplan[height - 1, j] == ' ')
                {
                    spelplan[height - 1, j] = '.';
                    Grass(ref spelplan, height - 2, j, width, height);
                }
            }
                         
            return true;
        }

        static void Grass(ref char[,] spelplan, int x_pos, int y_pos, int width, int height) 
            //spreads grass but avoids the outer line to avoid to jump outside the array
        {
            if (x_pos > 0 && x_pos < height && y_pos > 0 && y_pos < width)
            {
                if (spelplan[x_pos, y_pos] == ' ')
                {
                    spelplan[x_pos, y_pos] = '.';

                    Grass(ref spelplan, x_pos - 1, y_pos, width, height);
                    Grass(ref spelplan, x_pos + 1, y_pos, width, height);
                    Grass(ref spelplan, x_pos, y_pos -1, width, height);
                    Grass(ref spelplan, x_pos, y_pos+1, width, height);
                }
                

            }

        }

    }
    
}
