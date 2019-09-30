using System;
using System.Collections.Generic;
using System.Threading;

class Program
{
    static char[,] screen = new char[20, 70];
    const char BALL = (char)9673;//79;
    const char PADDLE = (char)9608;
    const char WALL = (char)9608;
    const char BG = (char)183;

    const int Player1 = 0;
    const int Player2 = 69;

    static int[] ballPosition = {10, 35};
    static int[] p1PaddlePosition = {8, 9, 10, 11};
    static int[] p2PaddlePosition = {8, 9, 10, 11};
    static int[] BallSpeed = {0,1};
    static bool playing = true;

    static void Main()
    {
        InitBord();
        DrawBoard();
        while(playing)
        {
            if(Console.KeyAvailable)
            {
                GameLoop();
            }
        }

    }

    static void GameLoop()
    {
        ConsoleKeyInfo cki = Console.ReadKey(true);
        bool move = true;
        while(!Console.KeyAvailable)
        { 
            if(cki.KeyChar == 'q')
                {
                    playing = false;
                }
            if(move && cki.Key.ToString() == "UpArrow")
                {
                    MovePaddle(Player2, -1); 
                    move = false;    
                }
                 else if(move && cki.Key.ToString() == "DownArrow")
                {
                    MovePaddle(Player2, 1);
                    move = false;
                }  
            if(move && cki.Key.ToString() == "W")
                {
                    MovePaddle(Player1, -1); 
                    move = false;    
                }
                 else if(move && cki.Key.ToString() == "S")
                {
                    MovePaddle(Player1, 1);
                    move = false;
                } 
            Thread.Sleep(50);
            MoveBall(ballPosition, BallSpeed);
            DrawBoard();                 
            
            
        }    
    }

    static void InitBord()
    {
        for(int i=0; i<screen.GetLength(0); i++) 
        {
            for(int j=0; j<screen.GetLength(1); j++)
            {
                screen[i, j] = BG;
                if(i == 0)
                {
                    screen[0,j] = WALL;
                }
                else if(i == screen.GetLength(0)-1)
                {
                    screen[i,j] = WALL;
                }       
            }
        }

        screen[ballPosition[0], ballPosition[1]] = BALL;

        for(int i=0; i<p1PaddlePosition.Length; i++)
        {
            screen[p1PaddlePosition[i], Player1]  = PADDLE;
            screen[p1PaddlePosition[i], Player2] = PADDLE;
        }
    }

    static void DrawBoard()
    {
        Console.Clear();
        screen[p1PaddlePosition[0], Player1] = PADDLE;
        screen[p1PaddlePosition[1], Player1] = PADDLE;
        screen[p1PaddlePosition[2], Player1] = PADDLE;
        screen[p1PaddlePosition[3], Player1] = PADDLE;

        screen[p2PaddlePosition[0], Player2] = PADDLE;
        screen[p2PaddlePosition[1], Player2] = PADDLE;
        screen[p2PaddlePosition[2], Player2] = PADDLE;
        screen[p2PaddlePosition[3], Player2] = PADDLE;
        string boardString = "\r";
        for(int i=0; i<screen.GetLength(0); i++) 
        {
            for(int j=0; j<screen.GetLength(1); j++)
            {
                boardString += screen[i,j];
            }
            boardString += "\n";
        }
        Console.WriteLine(boardString);
    }

    static void MoveBall(int[] currentLocation, int[] speed)
    {
        Random rand = new Random();
        if(speed[1] < -3) speed[1] = -3;
        if(speed[1] > 3) speed[1] = 3;
        int randInt = rand.Next(-1,2);
        int[] newPos = {currentLocation[0] + speed[0], currentLocation[1] + speed[1]};

        if(newPos[0] <= Math.Abs(BallSpeed[0]))
        {
            BallSpeed[0] *= -1;
        } else if(newPos[0] >= screen.GetLength(0)-Math.Abs(BallSpeed[0]))
        {
            BallSpeed[0] *= -1;
        }
        else if(newPos[1] >= screen.GetLength(1)-Math.Abs(BallSpeed[1]))
        {
            if(screen[newPos[0], newPos[1]]  == PADDLE)
            {
                
                if(newPos[0] == p2PaddlePosition[0])
                {
                    BallSpeed[0] = -1;
                    BallSpeed[1] *= -1;
                } 
                else if(newPos[0] == p2PaddlePosition[1] || newPos[0] == p2PaddlePosition[2] )
                {
                    BallSpeed[0] = 0;
                    BallSpeed[1] = -1;
                }
                else if(newPos[0] == p2PaddlePosition[3])
                {
                    BallSpeed[0] = 1;
                    BallSpeed[1] *= -1;
                }
                
            }
           
        }
        else if(newPos[1] <= Math.Abs(BallSpeed[1])-1)
        {
            if(screen[newPos[0], newPos[1]]  == PADDLE)
            {
                if(newPos[0] == p1PaddlePosition[0])
                {
                    BallSpeed[0] = -1;
                    BallSpeed[1] *= -1;
                } 
                else if(newPos[0] == p1PaddlePosition[1] || newPos[0] == p1PaddlePosition[2] )
                {
                    BallSpeed[0] = 0;
                    BallSpeed[1] = 1;
                }
                else if(newPos[0] == p1PaddlePosition[3])
                {
                    BallSpeed[0] = 1;
                    BallSpeed[1] *= -1;
                }
            }
        }
        else 
        {
            screen[newPos[0], newPos[1]] = BALL;
            screen[currentLocation[0], currentLocation[1]] = BG;
            ballPosition = newPos;
        }
    }

    static void MovePaddle(int player, int direction)
    {
        if(player == Player1)
        {
            if(p1PaddlePosition[0]+direction > 0 && p1PaddlePosition[3]+direction < screen.GetLength(0)-1)
            {
                screen[p1PaddlePosition[0], Player1] = BG;
                screen[p1PaddlePosition[1], Player1] = BG;
                screen[p1PaddlePosition[2], Player1] = BG;
                screen[p1PaddlePosition[3], Player1] = BG;
                p1PaddlePosition[0] += direction;
                p1PaddlePosition[1] += direction;
                p1PaddlePosition[2] += direction;
                p1PaddlePosition[3] += direction;
                
            }
        }

        if(player == Player2)
        {
            if(p2PaddlePosition[0]+direction > 0 && p2PaddlePosition[3]+direction < screen.GetLength(0)-1)
            {
                screen[p2PaddlePosition[0], Player2] = BG;
                screen[p2PaddlePosition[1], Player2] = BG;
                screen[p2PaddlePosition[2], Player2] = BG;
                screen[p2PaddlePosition[3], Player2] = BG;
                p2PaddlePosition[0] += direction;
                p2PaddlePosition[1] += direction;
                p2PaddlePosition[2] += direction;
                p2PaddlePosition[3] += direction;
                
            }
        }
    }
}