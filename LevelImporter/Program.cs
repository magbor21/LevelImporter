using System;
using System.IO;

namespace LevelImporter 
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Console.WriteLine("Let's do this!");

            Importer();

        }

        static bool Importer() // Imports a text file and creastes a level file compatible with this weeks assignment
        {
            FileStream fsOut = File.OpenWrite("./../../../Levels.js");
            StreamReader fsIn = new StreamReader("./../../../Original.txt");
            int gameboards = 0;
            int width = 0;
            int height = 0;

            // starts out with a slightly bigger empty gameboard than the ones being imported just in case
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

            string topText = " /* SokoBan levels imported from text file using my LevelImporter (https://github.com/magbor21/LevelImporter) */ \n" +
                "\n" +
                "var tileMapArray = new TileMaps();\n" +
                "\n";
            byte[] outBytes = new System.Text.UTF8Encoding(true).GetBytes(topText);
            fsOut.Write(outBytes);



            while (!fsIn.EndOfStream)
            {
                
                string currentLine = fsIn.ReadLine();

                if (currentLine.Length == 0)
                    continue;

                if (currentLine[0] == ';') // The board is done and needs to be exported
                {

                    Greenify(ref spelplan, width, height); //Adds some grass, if you want to


                    string outRow = "tileMapArray.addMap(new TileMap(" + width + "," + height + ", ["; // Outrow contains what is to be written to the new file
                    
                    for (int i = 0; i < height; i++)
                    {
                        outRow += "[";
                        for (int j = 0; j < width; j++)
                        {
                            outRow += "\"" + spelplan[i, j] + "\"";
                            if (j < (width - 1)) 
                                outRow += ", ";
                        }
                        outRow += "]";
                        if (i < (height - 1))
                            outRow += ",\n";
                        
                    }
                    outRow += " ]));\n";

                    outBytes = new System.Text.UTF8Encoding(true).GetBytes(outRow);  // Filestream only writes in bytes
                    fsOut.Write(outBytes);


                    gameboards++;
                    Console.WriteLine("Gameboard: " + gameboards + " done"); // board is done.  

                    width = 0; // clear the variables and get ready for the next gameboard.
                    height = 0;

                    for (int k = 0; k < 20; k++)
                        for (int l = 0; l < 20; l++)
                            spelplan[k, l] = ' ';


                    continue;
                }

                        // if the line is "normal"
                for(int i = 0; i < currentLine.Length;i++)
                {
                    char currentChar = currentLine[i]; // Translate the characters and add them
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

                width = Math.Max(width, currentLine.Length); //checks if this is the widest part of the tileset.
                height++;


             }

            fsOut.Close(); // Close the files when you are done
            fsIn.Close();
            return true;
        }

        static bool Greenify(ref char[,] spelplan, int width, int height) // calculates and adds some grass
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
                    Grass(ref spelplan, 1, j, width, height); // And spread from there (in case of U shaped levels)

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

                    Grass(ref spelplan, x_pos - 1, y_pos, width, height); // Recursive
                    Grass(ref spelplan, x_pos + 1, y_pos, width, height);
                    Grass(ref spelplan, x_pos, y_pos -1, width, height);
                    Grass(ref spelplan, x_pos, y_pos+1, width, height);
                }
                

            }

        }

    }
    
}
