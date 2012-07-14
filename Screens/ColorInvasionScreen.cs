using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using JScreenTest.Game;
using JScreenTest.Rendering;
using JScreenTest.ScreenManagement;

namespace JScreenTest.Screens
{

    class ColorInvasionScreen : ButtonScreen
    {
        #region Fields

        //Width should be odd, Height should be even
        const int GRID_WIDTH = 21; //31
        const int GRID_HEIGHT = 20; //30
        const int GRID_SPACES = GRID_WIDTH * GRID_HEIGHT;

        //How many moves the ai should look ahead
        const int AI_DEPTH = 3;

        //Space between grid and edge of window
        int xBuffer = 50;
        int yBuffer = 50;

        //Button fields
        //int buttonCount = 7;
        //int buttonSize = 60;
        //int buttonXBuffer = 250;
        //int buttonYPosition = 625;

        //Diamond Color Codes
        const int RED = 0;
        const int ORANGE = 1;
        const int YELLOW = 2;
        const int GREEN = 3;
        const int TEAL = 4;
        const int BLUE = 5;        
        const int INDIGO = 6;
        
        //Diamond Colors
        Color RED_COLOR = new Color(204, 0, 0);
        Color ORANGE_COLOR = new Color(237, 95, 33);
        Color YELLOW_COLOR = new Color(250, 227, 0);
        Color GREEN_COLOR = new Color(91, 156, 10);
        Color BLUE_COLOR = new Color(10, 13, 156);
        Color INDIGO_COLOR = new Color(80, 10, 156);
        Color TEAL_COLOR = new Color(10, 153, 156);

        float diamondSize;

        //Current color for each player
        int p1color;
        int p2color;

        //Current size for each player
        int p1count;
        int p2count;

        int currentTurn;
        bool gameOver;

        int[,] gameGrid;

        Sprite diamond;

        Texture2D diamond3;
        Texture2D whitePixel;
        Texture2D mouseCursor;
        Texture2D buttonTexture;
        Texture2D buttonSelection;
        Texture2D xIcon;

        Song song;

        Random r;

        MouseState mousePosition;

        #endregion

        #region Generic Screen Methods

        public override void initialize()
        {
            mousePosition = Mouse.GetState();

            gameGrid = new int[GRID_WIDTH, GRID_HEIGHT];
            diamondSize = (float)Math.Round((gd.Viewport.Width - xBuffer * 2)/(0.5f + GRID_WIDTH), 0);
            if (diamondSize % 2 != 0)
                diamondSize--;
            xBuffer = (int)(gd.Viewport.Width - (GRID_WIDTH + 0.5f) * diamondSize)/2;
            Debug.WriteLine("Diamondsize: " + diamondSize);

            randomizeGrid();

            p1color = gameGrid[0, 0];
            p2color = gameGrid[GRID_WIDTH-1, GRID_HEIGHT-1];

            p1count = 1;
            p2count = 1;

            currentTurn = 1;
            gameOver = false;

            buttonSizeX = 60;
            buttonSizeY = 60;
            buttonBufferX = 250;
            buttonBufferY = 625;

            r = new Random();

            buttons = new List<Button>();
            buttons.Add(new Button(buttonTexture, RED_COLOR, new Rectangle()));
            buttons.Add(new Button(buttonTexture, ORANGE_COLOR, new Rectangle()));
            buttons.Add(new Button(buttonTexture, YELLOW_COLOR, new Rectangle()));
            buttons.Add(new Button(buttonTexture, GREEN_COLOR, new Rectangle()));
            buttons.Add(new Button(buttonTexture, TEAL_COLOR, new Rectangle()));
            buttons.Add(new Button(buttonTexture, BLUE_COLOR, new Rectangle()));
            buttons.Add(new Button(buttonTexture, INDIGO_COLOR, new Rectangle()));

            placeButtonsHorizontal();

            
            manager.addScreen(new ScrollingTextScreen("Start!", Color.Orange));


        }

        public override void load()
        {

            whitePixel = content.Load<Texture2D>("whitePixel");
            diamond = new Sprite(content.Load<Texture2D>(@"ColorInvasion/diamond2"), 31, new int[] {1000, 50, 50, 50, 50, 50, 50, 50, 50}, Color.White);
            mouseCursor = content.Load<Texture2D>(@"mouseCursor");
            buttonTexture = content.Load<Texture2D>(@"ColorInvasion/paintbrushButton");
            buttonSelection = content.Load<Texture2D>(@"ColorInvasion/targetSelector");
            xIcon = content.Load<Texture2D>(@"ColorInvasion/xIcon");
            diamond3 = content.Load<Texture2D>(@"ColorInvasion/diamond3");

            song = content.Load<Song>(@"Songs/DST-BlueChill");

            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(song);
        }

        public override void  update()
        {
            handleInput();

            mousePosition = Mouse.GetState();

            diamond.update();
        }

        public override void  handleInput()
        {
            if (Global.isKeyPressed(Keys.Escape))
            {
                manager.addScreen(new ExitScreen(this, "GAME PAUSED"));
            }

            buttonCheck(true);
        }

        public override void buttonClicked(int position)
        {
            //If it's the player's turn and they aren't picking an invalid color
            if (currentTurn == 1 && position != p2color && position != p1color && !gameOver)
            {
                doTurn(1, position);

                int aiBest = aiFindBestMove(2);
                while (aiBest == -1 || aiBest == p1color)
                {
                    aiBest = r.Next(0, 7);
                }
                doTurn(2, aiBest);
            }
        }

        public override void draw()
        {
            Color currentColor;

            for (int i = 0; i < GRID_HEIGHT; i++)
            {
                for (int j = 0; j < GRID_WIDTH; j++)
                {
                    switch (gameGrid[j, i])
                    {
                        case RED:
                            currentColor = RED_COLOR;
                            break;
                        case ORANGE:
                            currentColor = ORANGE_COLOR;
                            break;
                        case YELLOW:
                            currentColor = YELLOW_COLOR;
                            break;
                        case GREEN:
                            currentColor = GREEN_COLOR;
                            break;
                        case TEAL:
                            currentColor = TEAL_COLOR;
                            break;
                        case BLUE:
                            currentColor = BLUE_COLOR;
                            break;
                        case INDIGO:
                            currentColor = INDIGO_COLOR;
                            break;
                        default:
                            currentColor = Color.Black;
                            break;
                    }

                    if (i % 2 == 0)
                    {
                        diamond.draw(
                            sb,
                            new Rectangle(
                                (int)Math.Round(xBuffer + j * diamondSize),
                                (int)Math.Round(yBuffer + i * diamondSize / 2),
                                (int)Math.Round(diamondSize),
                                (int)Math.Round(diamondSize)),
                            currentColor);
                    }
                    else
                    {
                        diamond.draw(
                            sb,
                            new Rectangle(
                                (int)Math.Round(xBuffer + (diamondSize)*(0.5f + j)), 
                                (int)Math.Round(yBuffer + i * diamondSize / 2), 
                                (int)Math.Round(diamondSize), 
                                (int)Math.Round(diamondSize)),
                            currentColor);
                    }
                }
            }

            foreach (Button button in buttons)
            {
                button.draw(sb);
            }

            sb.Draw(buttonSelection, buttons.ElementAt(p1color).rectangle, Color.White);
            sb.Draw(xIcon, buttons.ElementAt(p2color).rectangle, Color.Black);

            sb.Draw(mouseCursor, new Rectangle(mousePosition.X, mousePosition.Y, mouseCursor.Width / 4, mouseCursor.Height / 4), Color.White);
        
        }

        #endregion

        #region Specific Screen Methods

        #region AI Methods
        /// <summary>
        /// Check the next 6 possible moves based on a grid and return the best one
        /// </summary>
        /// <returns>Size of best move. -1 indicates stalemate</returns>
        public int aiCheckNextMove(int[,] grid, int currentSize, int player, int lastColor, int currentDepth)
        {
            int bestMove = 0;

            for (int i = 0; i < 7; i++)
            {
                if (i != lastColor)
                {
                    int[,] tempGrid = (int[,])grid.Clone();
                    expandRegion(tempGrid, player, lastColor, i);
                    int newSize = checkRegion(tempGrid, player, i).Count;
                    int growth = newSize - currentSize;

                    if (currentDepth == AI_DEPTH)
                    {
                        if (growth > bestMove)
                        {
                            bestMove = growth;
                        }
                    }
                    else if (growth > 0)
                    {
                        int bestNextMove = aiCheckNextMove(tempGrid, newSize, player, i, currentDepth + 1);
                        if (bestNextMove + growth > bestMove)
                        {
                            bestMove = bestNextMove + growth;
                        }
                    }
                }
            }

            return bestMove;
        }

        public int aiFindBestMove(int player)
        {
            int bestMove = -1;
            int bestSize = 0;

            int pColor;
            int pCount;

            if (player == 1)
            {
                pColor = p1color;
                pCount = p1count;
            }
            else
            {
                pColor = p2color;
                pCount = p2count;
            }

            for (int i = 0; i < 7; i++)
            {
                if (i != p1color && i != p2color)
                {
                    int[,] tempGrid = (int[,])gameGrid.Clone();
                    expandRegion(tempGrid, player, pColor, i);
                    int newSize = checkRegion(tempGrid, player, i).Count;

                    if (newSize > pCount)
                    {
                        int bestNextSize = aiCheckNextMove(tempGrid, newSize, player, i, 2);
                        bestNextSize += (newSize - pCount);

                        if (bestNextSize > bestSize)
                        {
                            bestSize = bestNextSize;
                            bestMove = i;
                        }
                    }

                }
            }

            return bestMove;
        }

        #endregion       

        public bool checkGameOver()
        {
            

            if (p1count + p2count == GRID_SPACES)
            {

                if (p1count > p2count)
                {
                    manager.addScreen(new ExitScreen(this, "You win, (" + p1count + " - " + p2count + ")."));
                }
                else if (p1count < p2count)
                {
                    manager.addScreen(new ExitScreen(this, "You lost, (" + p1count + " - " + p2count + ")."));
                }
                else
                {
                    manager.addScreen(new ExitScreen(this, "Tie game, (" + p1count + " - " + p2count + ")."));
                }

                return true;
            }

            return false;
        }

        public bool checkStalemate()
        {
            //Find unclaimed spaces
            List<Vector2> unclaimedList = getUnclaimedRegion();
            List<Vector2> p1region = checkRegion(gameGrid, 1, p1color);
            List<Vector2> p2region = checkRegion(gameGrid, 2, p2color);

            List<Vector2> checkedList = new List<Vector2>();
            bool stalemate = true;

            //For each unclaimed position, see if both sides can take it.
            foreach(Vector2 position in unclaimedList)
            {
                if (stalemate)
                {
                    //[0] = foundP1, [1] = foundP2
                    bool[] results = new bool[2];

                    results = checkStalemate4way(position, checkedList, p1region, p2region);

                    if (results[0] && results[1])
                    {
                        stalemate = false;
                    }                 

                }
                
            }

            return stalemate;
        }

        public bool[] checkStalemate4way(Vector2 position, List<Vector2> checkedList, List<Vector2> p1region, List<Vector2> p2region)
        {
            bool[] results = new bool[2] { false, false };

            checkedList.Add(position);

            Vector2[] directions = get4directions(position);

            foreach (Vector2 direction in directions)
            {
                //If the new position is in the bounds of the grid...
                if (
                    direction.X >= 0 &&
                    direction.X < GRID_WIDTH &&
                    direction.Y >= 0 &&
                    direction.Y < GRID_HEIGHT)
                {
                    //See if it's already been checked...
                    bool alreadyChecked = false;
                    foreach (Vector2 vector in checkedList)
                    {
                        if (direction.X == vector.X && direction.Y == vector.Y)
                        {
                            alreadyChecked = true;
                        }
                    }

                    //If not already checked...
                    if (!alreadyChecked)
                    {
                        //If this spot is in p1's region...
                        if (p1region.Contains(direction))
                        {
                            results[0] = true;
                        }
                        //Else if this spot is p2's color
                        else if (p2region.Contains(direction))
                        {
                            results[1] = true;
                        }
                        //This spot is unclaimed
                        else
                        {
                            bool[] newResults = checkStalemate4way(direction, checkedList, p1region, p2region);
                            results[0] = results[0] || newResults[0];
                            results[1] = results[1] || newResults[1];
                        }
                    }
                }
            }

            return results;
        }

        public void doTurn(int player, int newColor)
        {
            if (!gameOver)
            {
                expandRegion(gameGrid, player, player == 1 ? p1color : p2color, newColor);

                switch (player)
                {
                    case 1:
                        p1color = newColor;
                        p1count = checkRegion(gameGrid, player, newColor).Count;
                        break;
                    case 2:
                        p2color = newColor;
                        p2count = checkRegion(gameGrid, player, newColor).Count;
                        break;
                    default:
                        break;
                }

                if (checkStalemate())
                {
                    resolveStalemate();
                }

                if (checkGameOver())
                {
                    gameOver = true;
                }
            }
        }

        public Vector2[] get4directions(Vector2 position)
        {
            int NW = 0;
            int NE = 1;
            int SW = 2;
            int SE = 3;

            Vector2[] directions = new Vector2[4];
            directions[NW] = position;
            directions[NE] = position;
            directions[SW] = position;
            directions[SE] = position;

            if (position.Y % 2 == 0)
            {
                directions[NW].X -= 1;
                directions[NW].Y -= 1;

                directions[NE].Y -= 1;

                directions[SW].X -= 1;
                directions[SW].Y += 1;

                directions[SE].Y += 1;
            }
            else
            {
                directions[NW].Y -= 1;

                directions[NE].X += 1;
                directions[NE].Y -= 1;

                directions[SW].Y += 1;

                directions[SE].X += 1;
                directions[SE].Y += 1;
            }

            return directions;
        }

        /// <summary>
        /// When no spaces are contested by both players
        /// </summary>
        public void resolveStalemate()
        {
            List<Vector2> unclaimedList = getUnclaimedRegion();

            List<Vector2> p1region = checkRegion(gameGrid, 1, p1color);
            List<Vector2> p2region = checkRegion(gameGrid, 2, p2color);

            foreach (Vector2 vector in unclaimedList)
            {
                List<Vector2> checkedList = new List<Vector2>();
                int color = resolveStalemate4way(vector, checkedList, p1region, p2region);
                gameGrid[(int)vector.X, (int)vector.Y] = color;
            }

            p1count = checkRegion(gameGrid, 1, p1color).Count;
            p2count = checkRegion(gameGrid, 2, p2color).Count;
        }

        public int resolveStalemate4way(Vector2 position, List<Vector2> checkedList, List<Vector2> p1region, List<Vector2> p2region)
        {
            checkedList.Add(position);
            Vector2[] directions = get4directions(position);

            foreach (Vector2 direction in directions)
            {
                if (
                    direction.X >= 0 &&
                    direction.X < GRID_WIDTH &&
                    direction.Y >= 0 &&
                    direction.Y < GRID_HEIGHT)
                {
                    if (p1region.Contains(direction))
                    {
                        return p1color;
                    }
                    else if (p2region.Contains(direction))
                    {
                        return p2color;
                    }
                    else
                    {
                        if (!checkedList.Contains(direction))
                        {
                            return resolveStalemate4way(direction, checkedList, p1region, p2region);
                        }
                    }
                }
            }

            //Should never get here.
            System.Diagnostics.Debug.WriteLine("ERROR: ColorInvasionScreen, resolveStalemate4way, 131414");
            return -1;
        }

        #region Region Checking
        /// <summary>
        /// Returns a List<Vector2> of a player's region. Doesn't change anything.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="color"></param>
        public List<Vector2> checkRegion(int[,] grid, int player, int color)
        {
            List<Vector2> checkedList = new List<Vector2>();

            Vector2 position = Vector2.Zero;
            if (player == 2)
            {
                position.X = GRID_WIDTH - 1;
                position.Y = GRID_HEIGHT - 1;
            }

            check4way(grid, position, color, checkedList);

            return checkedList;
        }

        public void check4way(int[,] grid, Vector2 position, int color, List<Vector2> checkedList)
        {
            checkedList.Add(position);

            Vector2[] directions = get4directions(position);           

            foreach (Vector2 direction in directions)
            {
                //If the new position is in the bounds of the grid...
                if (
                    direction.X >= 0 &&
                    direction.X < GRID_WIDTH &&
                    direction.Y >= 0 &&
                    direction.Y < GRID_HEIGHT)
                {
                    //See if it's already been checked...
                    bool alreadyChecked = false;
                    foreach (Vector2 vector in checkedList)
                    {
                        if (direction.X == vector.X && direction.Y == vector.Y)
                        {
                            alreadyChecked = true;
                        }
                    }

                    //If not already checked and color matches
                    if (!alreadyChecked && grid[(int)direction.X, (int)direction.Y] == color)
                    {
                            check4way(grid, direction, color, checkedList);
                    }
                }
            }
        }

        public List<Vector2> getClaimedRegion()
        {
            return checkRegion(gameGrid, 1, p1color).Union(checkRegion(gameGrid, 2, p2color)).ToList();
        }

        public List<Vector2> getUnclaimedRegion()
        {
            List<Vector2> unclaimedList = new List<Vector2>();
            List<Vector2> claimedList = getClaimedRegion();
            Vector2 temp;

            for (int i = 0; i < GRID_HEIGHT; i++)
            {
                for (int j = 0; j < GRID_WIDTH; j++)
                {
                    temp.X = j;
                    temp.Y = i;

                    if (!claimedList.Contains(temp))
                    {
                        unclaimedList.Add(temp);
                    }
                }
            }

            return unclaimedList;
        }

        #endregion

        #region Region Growing
        /// <summary>
        /// Expands a player's region based on a color selection
        /// </summary>
        /// <param name="player">1- Player1, 2- Player2</param>
        /// <param name="oldColor">0-6 => Red-Indigo</param>
        public void expandRegion(int[,] grid, int player, int oldColor, int newColor)
        {
            Vector2 position = Vector2.Zero;
            if (player == 2)
            {
                position.X = GRID_WIDTH - 1;
                position.Y = GRID_HEIGHT - 1;
            }

            grow4way(grid, position, oldColor, newColor);
        }

        public void grow4way(int[,] grid, Vector2 position, int oldColor, int newColor)
        {
            grid[(int)position.X, (int)position.Y] = newColor;

            Vector2[] directions = get4directions(position);
           
            foreach (Vector2 direction in directions)
            {
                //If the new position is in the bounds of the grid...
                if (
                    direction.X >= 0 &&
                    direction.X < GRID_WIDTH &&
                    direction.Y >= 0 &&
                    direction.Y < GRID_HEIGHT)
                {
                    //If the color matches the old color...
                    if (grid[(int)direction.X, (int)direction.Y] == oldColor)
                    {
                            grow4way(grid, direction, oldColor, newColor);
                    }
                }
            }
        }

        #endregion

        #region Grid Modifications
        public void randomizeGrid()
        {
            Random r = new Random();

            for (int i = 0; i < GRID_HEIGHT; i++)
            {
                for (int j = 0; j < GRID_WIDTH; j++)
                {
                    gameGrid[j, i] = r.Next(0, 7);
                }
            }

            //Make sure the starting positions aren't the same color
            while (gameGrid[0, 0] == gameGrid[GRID_WIDTH - 1, GRID_HEIGHT - 1])
            {
                gameGrid[GRID_WIDTH - 1, GRID_HEIGHT - 1] = r.Next(0, 7);
            }
        }

        /// <summary>
        /// Sets the entire grid to a value
        /// </summary>
        /// <param name="value"></param>
        public void setGrid(int value)
        {
            for (int i = 0; i < GRID_HEIGHT; i++)
            {
                for (int j = 0; j < GRID_WIDTH; j++)
                {
                    gameGrid[j, i] = value;
                }
            }
        }

        #endregion

        #endregion

    }
}
