using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using JScreenTest.ScreenManagement;

namespace JScreenTest.Screens
{
    class TetrisScreen : Screen
    {
        const int BOARD_WIDTH = 10;
        const int BOARD_HEIGHT = 20;
        const int BOARD_OFFSET_X = 40;
        const int BOARD_OFFSET_Y = 40;

        const int TILE_SIZE = 20; //Board is [10*20,20*20] or [200,400] pixels
        
        const int UPDATE_DELAY = 250;

        //Game board values
        const int BLANK_SPACE = 0;
        const int ACTIVE_PIECE = 1;
        const int RED_PIECE = 2;
        const int ORANGE_PIECE = 3;
        const int YELLOW_PIECE = 4;
        const int GREEN_PIECE = 5;
        const int BLUE_PIECE = 6;
        const int INDIGO_PIECE = 7;
        const int VIOLET_PIECE = 8;

        Color RED_COLOR = new Color(204, 0, 0);
        Color ORANGE_COLOR = new Color(237, 95, 33);
        Color YELLOW_COLOR = new Color(250, 227, 0);
        Color GREEN_COLOR = new Color(91, 156, 10);
        Color BLUE_COLOR = new Color(10, 13, 156);
        Color INDIGO_COLOR = new Color(80, 10, 156);
        Color VIOLET_COLOR = new Color(153, 10, 156);

        const int MOVE_RIGHT = 0;
        const int MOVE_UP = 1;
        const int MOVE_LEFT = 2;
        const int MOVE_DOWN = 3;

        int[,] gameboard;
        Vector2[] activePieces;
        int currentPiece;
        Color currentColor;
        int axisPiece;
        int nextPiece;

        Song tetrisTheme;

        Texture2D whitePixel;
        Texture2D tetrisTile;

        //Timer used to determine whether it's time to update the board
        int oldTime;
        //Timer used to determine whether input should be handled or not.
        int lastMove;

        Random r;

        Keys[] keyList = new Keys[] 
            { Keys.W, Keys.A, Keys.S, Keys.D, Keys.Left, Keys.Right, Keys.Up, Keys.Down, Keys.Escape };

        public override void initialize()
        {
            gameboard = new int[BOARD_WIDTH, BOARD_HEIGHT];
            activePieces = new Vector2[4];

            oldTime = (int)Global.gameTime.TotalGameTime.TotalMilliseconds;
            lastMove = oldTime;

            r = new Random(oldTime);

            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(tetrisTheme);

            nextPiece = r.Next(2, 9);
            spawnPiece();
        }

        public override void  load()
        {
            whitePixel = content.Load<Texture2D>("whitePixel");
            tetrisTile = content.Load<Texture2D>(@"Tetris/tetrisTile");

            tetrisTheme = content.Load<Song>(@"Tetris/tetrisTheme");
        }

        public override void  update()
        {         
            handleInput();

            if (oldTime + UPDATE_DELAY < Global.gameTime.TotalGameTime.TotalMilliseconds)
            {
                oldTime = (int)Global.gameTime.TotalGameTime.TotalMilliseconds;  

                bool moved = tryMovePiece(MOVE_DOWN);

                //If the block didn't move, it's inactive
                if (!moved)
                {
                    for (int i = 0; i < activePieces.Length; i++)
                    {
                        gameboard[(int)activePieces[i].X, (int)activePieces[i].Y] = currentPiece;
                    }

                    lineCheck();
                    spawnPiece();
                }
            }
        }

        public override void handleInput()
        {
            bool keyPressed = false;

            foreach (Keys key in keyList)
            {
                if (Keyboard.GetState().IsKeyDown(key))
                {
                    keyPressed = true;
                }
            }

            if (keyPressed && lastMove + UPDATE_DELAY / 2 < Global.gameTime.TotalGameTime.TotalMilliseconds)
            {
                lastMove = (int)Global.gameTime.TotalGameTime.TotalMilliseconds;

                if (Keyboard.GetState().IsKeyDown(Keys.A) || Keyboard.GetState().IsKeyDown(Keys.Left))
                {
                    tryMovePiece(MOVE_LEFT);
                }
                if (Keyboard.GetState().IsKeyDown(Keys.D) || Keyboard.GetState().IsKeyDown(Keys.Right))
                {
                    tryMovePiece(MOVE_RIGHT);
                }
                if (Keyboard.GetState().IsKeyDown(Keys.W) || Keyboard.GetState().IsKeyDown(Keys.Up))
                {
                    tryRotatePiece();
                }
                if (Keyboard.GetState().IsKeyDown(Keys.S) || Keyboard.GetState().IsKeyDown(Keys.Down))
                {
                    tryMovePiece(MOVE_DOWN);
                }

            }

        }

        public override void draw()
        {
            Color drawColor = new Color(255, 0, 0);

            //Draw board
            sb.Draw(whitePixel, new Rectangle(BOARD_OFFSET_X, BOARD_OFFSET_Y, BOARD_WIDTH * TILE_SIZE, BOARD_HEIGHT * TILE_SIZE), Color.Black);

            for (int i = 0; i < BOARD_HEIGHT; i++)
            {
                for (int j = 0; j < BOARD_WIDTH; j++)
                {
                    switch (gameboard[j, i])
                    {
                        case ACTIVE_PIECE:
                            drawColor = this.currentColor;
                            break;
                        case RED_PIECE:
                            drawColor = RED_COLOR;
                            break;
                        case ORANGE_PIECE:
                            drawColor = ORANGE_COLOR;
                            break;
                        case YELLOW_PIECE:
                            drawColor = YELLOW_COLOR;
                            break;
                        case GREEN_PIECE:
                            drawColor = GREEN_COLOR;
                            break;
                        case BLUE_PIECE:
                            drawColor = BLUE_COLOR;
                            break;
                        case INDIGO_PIECE:
                            drawColor = INDIGO_COLOR;
                            break;
                        case VIOLET_PIECE:
                            drawColor = VIOLET_COLOR;
                            break;
                        default:
                            break;

                    }

                    if (gameboard[j, i] != 0)
                    {
                        sb.Draw
                            (
                            tetrisTile,
                            new Rectangle(BOARD_OFFSET_X + j * TILE_SIZE,BOARD_OFFSET_Y + i * TILE_SIZE,
                                TILE_SIZE, TILE_SIZE),
                            drawColor
                            );
                    }
                }
            }





        }

        public bool checkMovePiece(int direction)
        {
            bool canMove = true;

            for (int i = 0; i < activePieces.Length; i++)
            {
                switch (direction)
                {
                    case MOVE_RIGHT:
                        if (activePieces[i].X == BOARD_WIDTH - 1 || gameboard[(int)activePieces[i].X + 1, (int)activePieces[i].Y] > 1)
                        {
                            canMove = false;
                        }
                        break;
                    case MOVE_UP:
                        if (activePieces[i].Y == 0 || gameboard[(int)activePieces[i].X, (int)activePieces[i].Y - 1] > 1)
                        {
                            canMove = false;
                        }
                        break;
                    case MOVE_LEFT:
                        if (activePieces[i].X == 0 || gameboard[(int)activePieces[i].X - 1, (int)activePieces[i].Y] > 1)
                        {
                            canMove = false;
                        }
                        break;
                    case MOVE_DOWN:
                        if (activePieces[i].Y == BOARD_HEIGHT - 1 || gameboard[(int)activePieces[i].X, (int)activePieces[i].Y + 1] > 1)
                        {
                            canMove = false;
                        }
                        break;
                    default:
                        throw new Exception("tryMovePiece direction unacceptable value: " + direction + ". 0-3 supported.");
                }
            }

            return canMove;
        }

        public void lineCheck()
        {
            for (int i = BOARD_HEIGHT - 1; i >= 0; i--)
            {
                bool isLine = true;

                for (int j = 0; j < BOARD_WIDTH; j++)
                {
                    if (gameboard[j, i] == 0)
                        isLine = false;
                }

                if (isLine)
                {
                    //Move everything above down
                    for (int m = i; m > 0; m--)
                    {
                        for (int n = 0; n < BOARD_WIDTH; n++)
                        {
                            gameboard[n, m] = gameboard[n, m-1];
                        }
                    }

                    //Clear the top
                    for (int p = 0; p < BOARD_WIDTH - 1; p++)
                    {
                        gameboard[p,0] = 0;
                    }

                    i++;
                }
            }
        }

        public void spawnPiece()
        {
            currentPiece = nextPiece;

            switch (nextPiece)
            {
                case RED_PIECE:
                    gameboard[4, 0] = 1;
                    gameboard[5, 0] = 1;
                    gameboard[5, 1] = 1;
                    gameboard[6, 1] = 1;

                    activePieces[0] = new Vector2(4f, 0f);
                    activePieces[1] = new Vector2(5f, 0f);
                    activePieces[2] = new Vector2(5f, 1f);
                    activePieces[3] = new Vector2(6f, 1f);

                    axisPiece = 2;

                    currentColor = RED_COLOR;
                    break;
                case ORANGE_PIECE:
                    gameboard[6, 0] = 1;
                    gameboard[4, 1] = 1;
                    gameboard[5, 1] = 1;
                    gameboard[6, 1] = 1;
                    
                    activePieces[0] = new Vector2(6f, 0f);
                    activePieces[1] = new Vector2(4f, 1f);
                    activePieces[2] = new Vector2(5f, 1f);
                    activePieces[3] = new Vector2(6f, 1f);

                    axisPiece = 2;

                    currentColor = ORANGE_COLOR;
                    break;
                case YELLOW_PIECE:
                    gameboard[4, 0] = 1;
                    gameboard[5, 0] = 1;
                    gameboard[4, 1] = 1;
                    gameboard[5, 1] = 1;
                    
                    activePieces[0] = new Vector2(4f, 0f);
                    activePieces[1] = new Vector2(5f, 0f);
                    activePieces[2] = new Vector2(4f, 1f);
                    activePieces[3] = new Vector2(5f, 1f);

                    axisPiece = 0;

                    currentColor = YELLOW_COLOR;
                    break;
                case GREEN_PIECE:
                    gameboard[5, 0] = 1;
                    gameboard[6, 0] = 1;
                    gameboard[4, 1] = 1;
                    gameboard[5, 1] = 1;

                    
                    activePieces[0] = new Vector2(5f, 0f);
                    activePieces[1] = new Vector2(6f, 0f);
                    activePieces[2] = new Vector2(4f, 1f);
                    activePieces[3] = new Vector2(5f, 1f);

                    axisPiece = 3;

                    currentColor = GREEN_COLOR;
                    break;
                case BLUE_PIECE:
                    gameboard[4, 0] = 1;
                    gameboard[4, 1] = 1;
                    gameboard[5, 1] = 1;
                    gameboard[6, 1] = 1;
                    
                    activePieces[0] = new Vector2(4f, 0f);
                    activePieces[1] = new Vector2(4f, 1f);
                    activePieces[2] = new Vector2(5f, 1f);
                    activePieces[3] = new Vector2(6f, 1f);

                    axisPiece = 2;

                    currentColor = BLUE_COLOR;
                    break;
                case INDIGO_PIECE:
                    gameboard[4, 0] = 1;
                    gameboard[5, 0] = 1;
                    gameboard[6, 0] = 1;
                    gameboard[7, 0] = 1;
                    
                    activePieces[0] = new Vector2(4f, 0f);
                    activePieces[1] = new Vector2(5f, 0f);
                    activePieces[2] = new Vector2(6f, 0f);
                    activePieces[3] = new Vector2(7f, 0f);

                    axisPiece = 1;

                    currentColor = INDIGO_COLOR;
                    break;
                case VIOLET_PIECE:
                    gameboard[5, 0] = 1;
                    gameboard[4, 1] = 1;
                    gameboard[5, 1] = 1;
                    gameboard[6, 1] = 1;
                    
                    activePieces[0] = new Vector2(5f, 0f);
                    activePieces[1] = new Vector2(4f, 1f);
                    activePieces[2] = new Vector2(5f, 1f);
                    activePieces[3] = new Vector2(6f, 1f);

                    axisPiece = 2;

                    currentColor = VIOLET_COLOR;
                    break;
            }


            nextPiece = r.Next(2, 9);
            
        }

        /// <summary>
        /// Tries to move a piece.
        /// </summary>
        /// <param name="direction"></param>
        public bool tryMovePiece(int direction)
        {
            int xMove = 0;
            int yMove = 0;

            switch (direction)
            {
                case MOVE_RIGHT:
                    xMove = 1;
                    break;
                case MOVE_UP:
                    yMove = -1;
                    break;
                case MOVE_LEFT:
                    xMove = -1;
                    break;
                case MOVE_DOWN:
                    yMove = 1;
                    break;
                default:
                    throw new Exception("tryMovePiece direction unacceptable value: " + direction + ". 0-3 supported.");
            }            

            //Check if the piece can move
            bool canMove = checkMovePiece(direction);

            //If the piece can move...
            if (canMove)
            {
                //Clear the piece from the board.
                for (int j = 0; j < activePieces.Length; j++)
                {
                    gameboard[(int)activePieces[j].X, (int)activePieces[j].Y] = 0;
                }

                //Move the piece
                for (int j = 0; j < activePieces.Length; j++)
                {
                    activePieces[j].X += xMove;
                    activePieces[j].Y += yMove;
                }

                //Set the piece on the board
                for (int j = 0; j < activePieces.Length; j++)
                {
                    gameboard[(int)activePieces[j].X, (int)activePieces[j].Y] = 1;
                }
            }

            return canMove;

        }

        public bool tryRotatePiece()
        {
            bool canRotate = true;

            Vector2[] ghostLocation = new Vector2[4]; 


            //O-piece, do nothing
            if (currentPiece != 4)
            {
                //Find ghost location of rotation
                for (int i = 0; i < activePieces.Length; i++)
                {
                    if (i == axisPiece)
                    {
                        ghostLocation[i] = activePieces[axisPiece];
                    }
                    else
                    {
                        ghostLocation[i].Y = activePieces[axisPiece].Y + activePieces[i].X - activePieces[axisPiece].X;
                        ghostLocation[i].X = activePieces[axisPiece].X + activePieces[axisPiece].Y - activePieces[i].Y;
                    }

                    //Check if the ghost location is available
                    try
                    {
                        if (gameboard[(int)ghostLocation[i].X, (int)ghostLocation[i].Y] > ACTIVE_PIECE)
                        {
                            canRotate = false;
                        }
                    }
                    catch (Exception e)
                    {
                        canRotate = false;
                    }
                }

                //If rotation is allowed, rotate
                if (canRotate)
                {
                    for (int i = 0; i < activePieces.Length; i++)
                    {
                        gameboard[(int)activePieces[i].X, (int)activePieces[i].Y] = 0;
                    }

                    for (int i = 0; i < activePieces.Length; i++)
                    {
                        activePieces[i] = ghostLocation[i];
                        gameboard[(int)activePieces[i].X, (int)activePieces[i].Y] = 1;
                    }
                }
            }

            return canRotate;
        }
    }
}
