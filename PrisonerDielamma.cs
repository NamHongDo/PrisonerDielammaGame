/*  Name:     Nam Do and Shehab Hasan 
    Date:     12/02/2015
    Content:  A C# program that use delegate type*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrisonerDilemmaGame
{
    class PrisonerGame
    {
        static int count = 0; //global variable used for iteration to add points for each player and stragety conditions
        static int[] playerScore = new int[6];//arrays to store player and computer score
        static int[] computerScore = new int[6];
        static string playerChoice;
        static string previousPlayerChoice;
        static int totalPlayerScore = 0;
        static int totalComputerScore = 0;
        public delegate string Del();//initialize a delegate variable
        static int level;   //global variable to choose difficulty level

        static void Main(string[] args)
        {

            int maxTurns = 6;
            int turn = 0;
            string n;
            Console.WriteLine("Prisoner's Dilemma Game, choose the game difficulty\n1-Beginner Mode  2-Intermediate Mode  3-Hard Mode 4-Nightmare Mode 5-Hell Mode\nYou have 6 turns, make wise choices!");
            while (true)//loop to check wheter the input is a number and in range of 1-4
            {
                Console.Write("Please enter a number (1,2,3,4,5):");
                n = Console.ReadLine();
                bool isNumeric = int.TryParse(n, out level);
         
                if (isNumeric==false)
                {
                    Console.WriteLine("You did not enter the correct number! Please try again");
                    continue;                   
                }
                if (level >= 1 && level <= 5)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("You did not enter the correct number! Please try again");
                    continue;
                }
            }
            Del handler; //create a delegate object
            if (level==1)//nest if statements to assign appropriate strategy method accordingly to the player's choice to delegate object 
            {
                Console.WriteLine("\nBeginner mode was chosen! Good Luck!");
                handler = strategy1;
            }
            else if (level == 2)
            {
                Console.WriteLine("\nIntermediate mode was chosen! Good Luck!");
                handler = strategy2;
            }
            else if (level == 3)
            {
                Console.WriteLine("\nHard mode was chosen! Good Luck!");
                handler = strategy3;
            }
            else if (level == 4)
            {
                Console.WriteLine("\nNightmare mode was chosen! Good Luck!");
                handler = tit_for_tat;
            }
            else
            {
                Console.WriteLine("\nHell mode was chosen! Good Luck!");
                handler = meanTFT;
            }

            while (turn < maxTurns)//the game loop, 6 times in total
            {
                previousPlayerChoice = playerChoice;//this variable is used for one of the computer strategy
                Console.Write("\nTurn " + (turn+1) + ". Type C to cooperate, D to Defect, followed by Enter:  ");
                playerChoice = Console.ReadLine().ToUpper();
                if (!playerChoice.Equals("C") && !playerChoice.Equals("D"))//if statement to prevent player input anything else beside "C" or "D"
                {
                    Console.WriteLine("Error! The character you typed in is not 'C' or 'D'!");
                    continue;//if the input is not "C" or "D", return to the begining of the loop
                }
                compareScores(playerChoice, handler);//compare player's and computer's choices
                turn++;
            }

            if (totalPlayerScore > totalComputerScore)//nexted if statements to determine whether the player has won the game
            {
                Console.WriteLine(" \nCongratulations! You won!");
            }
            else if (totalPlayerScore < totalComputerScore)
            {
                Console.WriteLine(" \nToo bad! You lost!");
            }
            else
            {
                Console.WriteLine(" \nDraw!");
            }

            Console.Write("See if you can score higher next time! Press Enter to exit");
            Console.ReadLine();
        }

        public static string strategy1()//computer strategy against player
        {
            Random rnd = new Random();//random answer, no strategy
            int num = rnd.Next(0, 1);
            if (num == 0)
            {
                return "C";
            }
            else
            {
                return "D";
            }
        }

        public static string strategy2()//Defects the first 3 times
        {
            if (count <= 3)
                return "D";
            else
            {
                return "C";
            }
        }

        public static string strategy3()//Cheats the first time, then behaves based on count.
        {
            if (count == 0)
            {
                return playerChoice;
            }
            else if (count == 2)
            {
                return "C";
            }
            else if (count > 4)
            {
                return "D";
            }
            else
            {
                return previousPlayerChoice;
            }
        }

        public static string tit_for_tat()//first computer choice is always "C", all following choice is the copy of the player's previous choice
        {
            if (previousPlayerChoice==null)
            {
                return "C";
            }
            else
            {
                return previousPlayerChoice;
            }
        }

        public static string meanTFT()//Randomly defects 1/3 of the time.  Otherwise, uses tit for tat strategy
        {
            Random rnd = new Random();
            if (3 <= rnd.Next(9))
                return "D";
            else
                return tit_for_tat();
        }


        public static void compareScores(String str, Del delObject)// method to compare player's choice and computer's choice to determine the score for each player
        {
            String computerChoice = delObject();
            Console.WriteLine("Your Choice is : " + str + "                Computer Choice is :" + computerChoice);

            if (str.Equals("C") && computerChoice.Equals("C"))      //if both players cooperate, 3 points for each player
            {
                playerScore[count] = 3;
                computerScore[count] = 3;
            }
            else if (str.Equals("C") && computerChoice.Equals("D")) //if player cooperates when computer defects, player get 5, computer get 0
            {
                playerScore[count] = 0;
                computerScore[count] = 5;
            }
            else if (str.Equals("D") && computerChoice.Equals("C")) //if player defects when computer cooperates, player get 0, computer get 5
            {
                playerScore[count] = 5;
                computerScore[count] = 0;
            }
            else                                //if both players defect, 1 point for each player
            {
                playerScore[count] = 1;
                computerScore[count] = 1;
            }
            totalPlayerScore = totalPlayerScore + playerScore[count]; //adding up players scores after each turn
            totalComputerScore = totalComputerScore + computerScore[count];
            Console.WriteLine("Your score is : " + totalPlayerScore + "                 Computer score is : " + totalComputerScore);
            count++;
        }
    }
}
