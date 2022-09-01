using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Project5Edwards_z1861935
{
    public partial class Form1 : Form
    {

        public static int X_UNIT;
        public static int Y_UNIT;

        private Point SelectedSquare = new Point(-1, -1);
        private List<Point> PossibleMoves = new List<Point>();
        private bool whiteTurn = true;
        private int Turn = 0;
        private int Time = 0;
        private bool EndGame = false;
        private Timer secondTimer;


        Piece[,] board = new Piece[8, 8];
        public Form1()
        {
            InitializeComponent();

            X_UNIT = Canvas.Width / 8;
            Y_UNIT = Canvas.Width / 8;

            Canvas.Width = X_UNIT * 8;
            Canvas.Height = Y_UNIT * 8;

            initBoard();
            Check_Label.Text = "";
            CheckMate_Label.Text = "";
            secondTimer = new Timer();
            secondTimer.Interval = 1000;
            secondTimer.Tick += new EventHandler(TimerTick);
            secondTimer.Enabled = true;


            Canvas.Refresh();
        }

        /// <summary>
        /// Event handler for the game timer. Updates the timer variable and the
        /// time displayed on screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void TimerTick(object sender, EventArgs args)
        {
            Time++;
            int min = Time / 60;
            int seconds = Time % 60;

            Time_Label.Text = String.Format("Time: {0:00}:{1:00}", min, seconds);
        }

        /// <summary>
        /// Initializes the board to the starting state
        /// </summary>
        private void initBoard()
        {
            //placing pawns

            for (int i = 0; i < 8; ++i)
            {
                for (int j = 0; j < 8; ++j)
                {
                    board[j, i] = null;
                }
            }

            for (int i = 0; i < 8; ++i)
            {
                board[i, 1] = new Piece(true, Piece.Type.Pawn);
                board[i, 6] = new Piece(false, Piece.Type.Pawn);
            }
            

            
            //placing rooks
            board[0, 0] = new Piece(true, Piece.Type.Rook);
            board[7, 0] = new Piece(true, Piece.Type.Rook);
            board[0, 7] = new Piece(false, Piece.Type.Rook);
            board[7, 7] = new Piece(false, Piece.Type.Rook);

            //placing knights
            board[1, 0] = new Piece(true, Piece.Type.Knight);
            board[6, 0] = new Piece(true, Piece.Type.Knight);
            board[1, 7] = new Piece(false, Piece.Type.Knight);
            board[6, 7] = new Piece(false, Piece.Type.Knight);

            //placing bishops
            board[2, 0] = new Piece(true, Piece.Type.Bishop);
            board[5, 0] = new Piece(true, Piece.Type.Bishop);
            board[2, 7] = new Piece(false, Piece.Type.Bishop);
            board[5, 7] = new Piece(false, Piece.Type.Bishop);
            
            //placing queens
            board[3, 0] = new Piece(true, Piece.Type.Queen);
            board[3, 7] = new Piece(false, Piece.Type.Queen);

            //placing kings
            board[4, 0] = new Piece(true, Piece.Type.King);
            board[4, 7] = new Piece(false, Piece.Type.King);
            
        }

        /// <summary>
        /// Event handler to refresh the the chess board
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Canvas_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            
            DrawBoard(g);
            DrawPeices(g);


        }

        /// <summary>
        /// Helper function for Canvas_Paint()
        /// handles the drawing of the peices on the board
        /// </summary>
        /// <param name="g"></param>
        private void DrawPeices(Graphics g)
        {
            for (int i = 0; i < 8; ++i)
            {

                for (int j = 0; j < 8; ++j)
                {
                    if (board[j, i] != null)
                    {
                        board[j, i].DrawPeice(j, i, g);
                    }
                }
            }
        }

        /// <summary>
        /// Helper function for Canvas_paint()
        /// draws the chess board along with any special
        /// coloring that may be needed for selected squars and
        /// possibble moves
        /// </summary>
        /// <param name="g"></param>
        private void DrawBoard(Graphics g)
        {
            bool blacksquare = false;
            Brush blackbrush = new SolidBrush(Color.LightGreen);
            Brush whitebrush = new SolidBrush(Color.LightGray);

            //draw the main board
            for (int i = 0; i < 8; ++i)
            {
                for (int j = 0; j < 8; ++j)
                {
                    Brush current = whitebrush;
                    if (blacksquare)
                    {
                        current = blackbrush;
                    }
                    blacksquare = !blacksquare;

                    Rectangle drawRect = new Rectangle(j * X_UNIT, i * Y_UNIT, X_UNIT, Y_UNIT);
                    g.FillRectangle(current, drawRect);
                }
                blacksquare = !blacksquare;
            }

            //if the player selected a square color it light blue
            if (SelectedSquare.X != -1)
            {
                using (Brush bluebrush = new SolidBrush(Color.LightBlue))
                {
                    Rectangle drawRect = new Rectangle(SelectedSquare.X * X_UNIT, SelectedSquare.Y * Y_UNIT, X_UNIT, Y_UNIT);
                    g.FillRectangle(bluebrush, drawRect);
                }
            }

            //if there are possible moves color them yellow
            foreach (Point p in PossibleMoves)
            {
                using (Brush yellowbrush = new SolidBrush(Color.LightYellow))
                {
                    Rectangle drawRect = new Rectangle(p.X * X_UNIT, p.Y * Y_UNIT, X_UNIT, Y_UNIT);
                    g.FillRectangle(yellowbrush, drawRect);
                }

            }

            blackbrush.Dispose();
            whitebrush.Dispose();
        }

        /// <summary>
        /// Event handler for when the user clicks
        /// handles most of the turn logic
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Canvas_MouseDown(object sender, MouseEventArgs e)
        {
            //if the game is over dont do anything
            if (EndGame)
                return;

            //get the square they clicked
            int xClick = e.X / X_UNIT;
            int yClick = e.Y / Y_UNIT;

            foreach (Point p in PossibleMoves)
            {
                if (p.X == xClick && p.Y == yClick)
                {
                    //need to make a deep copy of the board
                    Piece[,] tempBoard = new Piece[8, 8];
                    for (int i = 0; i < 8; ++i)
                    {
                        for (int j = 0; j < 8; ++j)
                        {
                            if (board[i, j] != null)
                                tempBoard[i, j] = board[i, j];
                            else
                                tempBoard[i, j] = null;
                        }
                    }

                    //make the move on the temp boatd to see if it is a legal move
                    tempBoard[SelectedSquare.X, SelectedSquare.Y].HasMoved = true;
                    tempBoard[p.X, p.Y] = tempBoard[SelectedSquare.X, SelectedSquare.Y];
                    tempBoard[SelectedSquare.X, SelectedSquare.Y] = null;


                    //precheck so you dont put yourself in check
                    if (IsCheck(tempBoard) == 2 && whiteTurn)
                    {
                        MessageBox.Show("WHITE! You cant put yourself in check!");
                        return;
                    }else if (IsCheck(tempBoard) == 1 && !whiteTurn)
                    {
                        MessageBox.Show("BLACK! You cant put yourself in check!");
                        return;
                    }

                    //if legal then actually make the move
                    board[SelectedSquare.X, SelectedSquare.Y].HasMoved = true;
                    board[p.X, p.Y] = board[SelectedSquare.X, SelectedSquare.Y];
                    board[SelectedSquare.X, SelectedSquare.Y] = null;

                    //reset some global varibles and resources
                    PossibleMoves.Clear();
                    SelectedSquare = new Point(-1, -1);
                    ChangeTurn();
                    Canvas.Refresh();
                    Turn++;
                    
                    //see if the move just made puts the other person in check
                    if(IsCheck(tempBoard) == 1 && !whiteTurn)
                        Check_Label.Text = "Black is in Check!";
                    else if (IsCheck(tempBoard) == 2 && whiteTurn)
                        Check_Label.Text = "White is in Check!";
                    else
                        Check_Label.Text = "";

                    //if the move results in checkmate call the helper function to end the game
                    if (CheckIfCheckMate(board))
                    {
                        GameOverCheckmate();
                        return;
                    }

                    return;
                }
            }

            //if they clicked on an empty space dont do anything
            if (board[xClick, yClick] == null)
                return;

            //if they clicked on one of their peices selected it
            if (board[xClick, yClick].White == whiteTurn)
            {
                PossibleMoves = GetPossibleMoves(xClick, yClick, board);
                SelectedSquare = new Point(xClick, yClick);
            }

            Canvas.Refresh();
        }

        /// <summary>
        /// Helper function to handle ending the game
        /// </summary>
        private void GameOverCheckmate()
        {
            EndGame = true;
            int min = Time / 60;
            int seconds = Time % 60;
            int numBlack = 0;
            int numWhite = 0;

            //find the amount of peices remaining on the board
            for (int i = 0; i < 8; ++i)
            {
                for (int j = 0; j < 8; ++j)
                {
                    if (board[j, i] == null)
                        continue;
                    else if (board[j, i].White)
                        numWhite++;
                    else if (board[j, i].Black)
                        numBlack++;
                }
            }

            
            secondTimer.Stop();


            //display the ending summary
            CheckMate_Label.Text = "Checkmate! " + (whiteTurn? "Black" : "White") + " wins the game!\n" +
                "End Time: " + String.Format("{0:00}:{1:00}", min, seconds) + "\n" +
                "Total Turns: " + Turn + "\n" +
                "Black Lost " + (16 - numBlack) + " pieces\n" +
                "White Lost " + (16 - numWhite) + " piece\n" +
                "Press \"Surrender\" to reset game.";

            Surrender_Button.BackColor = Color.Yellow;

        }

        /// <summary>
        /// Given a board check to see if it is in a checkmate state
        /// </summary>
        /// <param name="b">board to check</param>
        /// <returns></returns>
        private bool CheckIfCheckMate(Piece[,] b)
        {
            //need to see if there are any valid moves left for current player to make

            Piece[,] tempBoard;

            //check each peice
            for (int i = 0; i < 8; ++i)
            {
                for (int j = 0; j < 8; ++j)
                {
                    Piece curr = board[j, i];
                    if (curr != null && curr.White == whiteTurn) //need current peice to not be null and to match current players color
                    {
                        List<Point> currMoves = GetPossibleMoves(j, i, b);
                        foreach (Point p in currMoves)
                        {
                            //need a copy of the board
                            tempBoard = CopyBoard(b);

                            tempBoard[p.X, p.Y] = tempBoard[j, i];
                            tempBoard[j, i] = null;
                            if (IsCheck(tempBoard) == 0) //if that move results in a non checked state return false
                            {
                                return false;
                            }

                        }
                    }
                }
            }

            return true; //if all moves result in a check state => checkmate

        }

        /// <summary>
        /// helper function to make a deep copy of the chess board
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        private Piece[,] CopyBoard(Piece[,] b)
        {
            Piece[,] tempBoard = new Piece[8, 8];
            for (int i = 0; i < 8; ++i)
            {
                for (int j = 0; j < 8; ++j)
                {
                    if (b[i, j] != null)
                        tempBoard[i, j] = b[i, j];
                    else
                        tempBoard[i, j] = null;
                }
            }

            return tempBoard;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        private int IsCheck(Piece[,] b)
        {
            //check if black is in check
            //PrintDebugBoard(b);

            List<Point> moves = new List<Point>();
            for (int i = 0; i < 8; ++i)
            {
                for (int j = 0; j < 8; ++j)
                {
                    if (b[j, i] != null && b[j, i].White) //look for all the white pieces moves
                    {
                        //MessageBox.Show("Getting moves for peice at (" + j + ", " + i + ")\n" +
                        //  "of piece type: " + b[j,i].PieceType);
                        moves = GetPossibleMoves(j, i, b).ToList();
                        foreach (Point p in moves)
                        {
                            if (b[p.X, p.Y] != null && b[p.X, p.Y].PieceType == Piece.Type.King)
                            {
                                //MessageBox.Show("Black King at (" + p.X + ", " + p.Y + ") \n " +
                                  //  "is being attacked by " + b[j,i].PieceType + " at (" + j + ", " + i + ")");
                                return 1;
                            }
                        }
                        moves.Clear();
                    }
                }
            }

            //check if white is being attacked

            for (int i = 0; i < 8; ++i)
            {
                for (int j = 0; j < 8; ++j)
                {
                    if (b[j, i] != null && b[j, i].Black) //look for all the white pieces moves
                    {
                        //MessageBox.Show("Getting moves for peice at (" + j + ", " + i + ")\n" +
                        //  "of piece type: " + b[j,i].PieceType);
                        moves = GetPossibleMoves(j, i, b).ToList();
                        foreach (Point p in moves)
                        {
                            if (b[p.X, p.Y] != null && b[p.X, p.Y].PieceType == Piece.Type.King)
                            {
                                //MessageBox.Show("White King at (" + p.X + ", " + p.Y + ") \n " +
                                  //  "is being attacked by " + b[j, i].PieceType + " at (" + j + ", " + i + ")");
                                return 2;
                            }
                        }
                        moves.Clear();
                    }
                }
            }

            moves.Clear();



            return 0;
        }

        /// <summary>
        /// Helper function used to change the boards look
        /// to match the color of the current player
        /// </summary>
        private void ChangeTurn()
        {
            whiteTurn = !whiteTurn;

            if (whiteTurn)
            {
                this.BackColor = Color.White;
                Turn_Label.Text = "White's Turn";
                Turn_Label.ForeColor = Color.Black;
                Turn_Label.BackColor = Color.White;
                Time_Label.ForeColor = Color.Black;
                Time_Label.BackColor = Color.White;
                Check_Label.ForeColor = Color.Black;
                Check_Label.BackColor = Color.White;
            }
            else //black
            {
                this.BackColor = Color.Black;
                Turn_Label.Text = "Black's Turn";
                Turn_Label.ForeColor = Color.White;
                Turn_Label.BackColor = Color.Black;
                Time_Label.ForeColor = Color.White;
                Time_Label.BackColor = Color.Black;
                Check_Label.ForeColor = Color.White;
                Check_Label.BackColor = Color.Black;
            }
        }

        /// <summary>
        /// Utility function that returns a list of points of all
        /// possible moves that a peice at x,y could do. If no peice
        /// is present it returns an empty list.
        /// </summary>
        /// <param name="x">x cord</param>
        /// <param name="y">y cord</param>
        /// <param name="board">board to check on</param>
        /// <returns>list of moves for that space</returns>
        private List<Point> GetPossibleMoves(int x, int y, Piece[,] board)
        {
            List<Point> moves = new List<Point>();
            if (board[x, y] == null)
                return moves;

            if (board[x, y].PieceType == Piece.Type.King)
            {
                moves = moves.Concat(GetPossibleKingMoves(x, y, board)).ToList();
            }
            else if (board[x, y].PieceType == Piece.Type.Bishop)
            {
                moves = moves.Concat(GetPossibleBishopMoves(x, y, board)).ToList();
            }
            else if (board[x, y].PieceType == Piece.Type.Pawn)
            {
                moves = moves.Concat(GetPossiblePawnMoves(x, y, board)).ToList();
            }
            else if (board[x, y].PieceType == Piece.Type.Rook)
            {
                moves = moves.Concat(GetPossibleRookMoves(x, y, board)).ToList();
            }
            else if (board[x, y].PieceType == Piece.Type.Knight)
            {
                moves = moves.Concat(GetPossibleKnightMoves(x, y, board)).ToList();
            }
            else if (board[x, y].PieceType == Piece.Type.Queen)
            {
                moves = moves.Concat(GetPossibleQueenMoves(x, y, board)).ToList();
            }

            return moves;
        }

        /// <summary>
        /// Helper function for GetPossibleMoves for queens.
        /// The x,y coord must be that of a queen on the board.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private List<Point> GetPossibleQueenMoves(int x, int y, Piece[,] b)
        {

            return GetPossibleRookMoves(x, y, b).Concat(GetPossibleBishopMoves(x, y, b)).ToList();

        }

        /// <summary>
        /// Helper function for GetPossibleMoves for knights.
        /// The x,y coord must be that of a knight on the board.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private List<Point> GetPossibleKnightMoves(int x, int y, Piece[,] b)
        {
            List<Point> moves = new List<Point>();
            Piece _this = b[x, y];

            //topleft
            if ((y - 2 >= 0 && x - 1 >= 0) && ( b[x - 1, y - 2] == null || b[x - 1, y - 2].Black != _this.Black))
                moves.Add(new Point(x - 1, y - 2));
            if ((y - 1 >= 0 && x - 2 >= 0) && (b[x - 2, y - 1] == null || b[x - 2, y - 1].Black != _this.Black))
                moves.Add(new Point(x - 2, y - 1));

            //top right
            if ((y - 1 >= 0 && x + 2 < 8) && (b[x + 2, y - 1] == null || b[x + 2, y - 1].Black != _this.Black))
                moves.Add(new Point(x + 2, y - 1));
            if ((y - 2 >= 0 && x + 1 < 8) && (b[x + 1, y - 2] == null || b[x + 1, y - 2].Black != _this.Black))
                moves.Add(new Point(x + 1, y - 2));

            //bottom right
            if ((y + 2 < 8 && x + 1 < 8) && (b[x + 1, y + 2] == null || b[x + 1, y + 2].Black != _this.Black))
                moves.Add(new Point(x + 1, y + 2));
            if ((y + 1 < 8 && x + 2 < 8) && (b[x + 2, y + 1] == null || b[x + 2, y + 1].Black != _this.Black))
                moves.Add(new Point(x + 2, y + 1));

            //bottom left
            if ((y + 1 < 8 && x - 2 >= 0) && (b[x - 2, y + 1] == null || b[x - 2, y + 1].Black != _this.Black))
                moves.Add(new Point(x - 2, y + 1));
            if ((y + 2 < 8 && x - 1 >= 0) && (b[x - 1, y + 2] == null || b[x - 1, y + 2].Black != _this.Black))
                moves.Add(new Point(x - 1, y + 2));

            return moves;
        }

        /// <summary>
        /// Helper function for GetPossibleMoves for rooks.
        /// The x,y coord must be that of a rook on the board.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private List<Point> GetPossibleRookMoves(int x, int y, Piece[,] b)
        {
            List<Point> moves = new List<Point>();
            Piece _this = b[x, y];

            int cx = x; //current value getting checked on x
            int cy = y; //current value getting checked on y

            //top
            cy--;
            while (cy >= 0)
            {
                if (b[x, cy] == null)
                {
                    moves.Add(new Point(cx, cy));
                }
                else if (b[x, cy].Black == _this.Black)
                    break;
                else //peice of different color
                {
                    moves.Add(new Point(x, cy));
                    break;
                }

                cy--;
            }

            //bottom
            cy = y + 1;
            while (cy < 8)
            {
                if (b[x, cy] == null)
                {
                    moves.Add(new Point(cx, cy));
                }
                else if (b[x, cy].Black == _this.Black)
                    break;
                else //peice of different color
                {
                    moves.Add(new Point(x, cy));
                    break;
                }

                cy++;
            }

            //left 
            cx = x - 1;
            while (cx >= 0)
            {
                if (b[cx, y] == null)
                {
                    moves.Add(new Point(cx, y));
                }
                else if (b[cx, y].Black == _this.Black)
                    break;
                else //peice of different color
                {
                    moves.Add(new Point(cx, y));
                    break;
                }

                cx--;
            }

            //right
            cx = x + 1;
            while (cx < 8)
            {
                if (b[cx, y] == null)
                {
                    moves.Add(new Point(cx, y));
                }
                else if (b[cx, y].Black == _this.Black)
                    break;
                else //peice of different color
                {
                    moves.Add(new Point(cx, y));
                    break;
                }

                cx++;
            }


            return moves;
        }

        /// <summary>
        /// Helper function for GetPossibleMoves for pawns.
        /// The x,y coord must be that of a pawn on the board.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private List<Point> GetPossiblePawnMoves(int x, int y, Piece[,] b)
        {
            List<Point> moves = new List<Point>();
            Piece _this = b[x, y];

            //black peices move down
            if (_this.Black)
            {
                if (y + 1 > 7) //at bottom
                    return moves;

                if (b[x, y + 1] == null) //move foward
                {
                    moves.Add(new Point(x, y + 1));

                    if (y + 2 < 8 && b[x, y + 2] == null && _this.HasMoved == false) //can move two spaces if havent moved before
                        moves.Add(new Point(x, y + 2));
                }

                //attacking
                if (x + 1 < 8 && b[x + 1, y + 1] != null && b[x + 1, y + 1].Black != _this.Black)
                    moves.Add(new Point(x + 1, y + 1));

                if (x - 1 >= 0 && b[x - 1, y + 1] != null && b[x - 1, y + 1].Black != _this.Black)
                    moves.Add(new Point(x - 1, y + 1));

            }
            else //white peices move up
            {
                if (y - 1 < 0) //at top
                    return moves;

                if (b[x, y - 1] == null) //moving forward
                {
                    moves.Add(new Point(x, y - 1));
                    if (y - 2 >= 0 && b[x, y - 2] == null && _this.HasMoved == false)
                        moves.Add(new Point(x, y - 2));
                }

                //attacking
                if (x + 1 < 8 && b[x + 1, y - 1] != null && b[x + 1, y - 1].Black != _this.Black)
                    moves.Add(new Point(x + 1, y - 1));

                if (x - 1 >= 0 && b[x - 1, y - 1] != null && b[x - 1, y - 1].Black != _this.Black)
                    moves.Add(new Point(x - 1, y - 1));

            }


            return moves;
        }

        /// <summary>
        /// Helper function for GetPossibleMoves for bishops.
        /// The x,y coord must be that of a bishop on the board.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private List<Point> GetPossibleBishopMoves(int x, int y, Piece[,] b)
        {
            List<Point> moves = new List<Point>();
            Piece _this = b[x, y];
            int cx = x; //current value getting checked on x
            int cy = y; //current value getting checked on y

            //top right
            cy--;
            cx++;
            while (cx < 8 && cy >= 0)
            {
                if (b[cx, cy] == null)
                {
                    moves.Add(new Point(cx, cy));
                }
                else if (b[cx, cy].Black == _this.Black)
                    break;
                else
                {
                    moves.Add(new Point(cx, cy));
                    break;
                }
                
                cy--;
                cx++;
            }

            //bottom right
            cx = x + 1;
            cy = y + 1;
            while (cx < 8 && cy < 8)
            {
                if (b[cx, cy] == null)
                {
                    moves.Add(new Point(cx, cy));
                }
                else if (b[cx, cy].Black == _this.Black)
                    break;
                else
                {
                    moves.Add(new Point(cx, cy));
                    break;
                }

                cy++;
                cx++;
            }

            //top left
            cx = x - 1;
            cy = y - 1;
            while (cx >= 0 && cy >= 0)
            {
                if (b[cx, cy] == null)
                {
                    moves.Add(new Point(cx, cy));
                }
                else if (b[cx, cy].Black == _this.Black)
                    break;
                else
                {
                    moves.Add(new Point(cx, cy));
                    break;
                }

                cy--;
                cx--;
            }

            //bottom left

            cy = y + 1;
            cx = x - 1;
            while (cx >= 0 && cy < 8)
            {
                if (b[cx, cy] == null)
                {
                    moves.Add(new Point(cx, cy));
                }
                else if (b[cx, cy].Black == _this.Black)
                    break;
                else
                {
                    moves.Add(new Point(cx, cy));
                    break;
                }

                cy++;
                cx--;
            }

            return moves;
        }

        /// <summary>
        /// Helper function for GetPossibleMoves for kings.
        /// The x,y coord must be that of a king on the board.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private List<Point> GetPossibleKingMoves(int x, int y, Piece[,] b)
        {
            List<Point> moves = new List<Point>();

            //check left verticals
            if (x - 1 >= 0)
            {
                for (int i = -1; i < 2; ++i)
                {
                    try
                    {
                        if (b[x - 1, y + i] == null || b[x - 1, y + i].Black != b[x, y].Black)
                            moves.Add(new Point(x - 1, y + i));
                    }
                    catch (IndexOutOfRangeException) { } //just dont add if its out of range
                }
            }

            //right verticals

            if (x + 1 < 8)
            {
                for (int i = -1; i < 2; ++i)
                {
                    try
                    {
                        if (b[x + 1, y + i] == null || b[x + 1, y + i].Black != b[x, y].Black)
                            moves.Add(new Point(x + 1, y + i));
                    }
                    catch (IndexOutOfRangeException) { } //just dont add if its out of range
                }
            }

            //top and bottom
            if (y + 1 < 8 && (b[x, y + 1] == null || b[x, y + 1].Black != b[x, y].Black))
                moves.Add(new Point(x, y + 1));

            if (y - 1 >= 0 && (b[x, y - 1] == null || b[x, y - 1].Black != b[x, y].Black))
                moves.Add(new Point(x, y - 1));

            return moves;
        }

        /// <summary>
        /// Eventhandler to clear the current peice selection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClearSelection_Button_Click(object sender, EventArgs e)
        {
            SelectedSquare = new Point(-1, -1);
            PossibleMoves.Clear();
            Canvas.Refresh();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Surrender_Button_Click(object sender, EventArgs e)
        {

            //if the game wasnt already over treat it like a checkmate
            if (!EndGame)
            {
                GameOverCheckmate();
                EndGame = true;
                return;
            }

            //reset all the global resources and the gamestate
            Time = 0;
            Turn = 0;
            if (!whiteTurn)
                ChangeTurn();
            CheckMate_Label.Text = "";
            Check_Label.Text = "";
            initBoard();
            Surrender_Button.BackColor = Color.White;
            SelectedSquare = new Point(-1, -1);
            PossibleMoves.Clear();
            secondTimer.Start();
            EndGame = false;


            Canvas.Refresh();

        }

        /// <summary>
        /// Represents a chess piece on the board
        /// </summary>
        public class Piece
        {
            public enum Type { Pawn, Rook, Knight, Bishop, Queen, King, Other }
            private bool _isBlack;
            private Type _type;

            private bool hasMoved; //used for pawns
            public bool HasMoved
            {
                set { hasMoved = value; }
                get { return hasMoved; }
            }

            public bool Black => _isBlack;
            public bool White => !_isBlack;
            public Type PieceType => _type;

            public Piece()
            {
                _isBlack = false;
                _type = Type.Other;
                hasMoved = false;
            }

            /// <summary>
            /// Constuctor for the peices class
            /// </summary>
            /// <param name="isBlack"></param>
            /// <param name="thisType"></param>
            public Piece(bool isBlack, Type thisType)
            {
                _isBlack = isBlack;
                _type = thisType;
                hasMoved = false;
            }

            /// <summary>
            /// Utility function used to draw the correct chess peice on the board
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            /// <param name="g"></param>
            public void DrawPeice(int x, int y, Graphics g)
            {
                Image peiceImg;

                switch (PieceType)
                {
                    case Type.King:
                        if (Black)
                            peiceImg = Image.FromFile("ChessPieces/kingb.png");
                        else
                            peiceImg = Image.FromFile("ChessPieces/kingw.png");
                        break;
                    case Type.Bishop:
                        if (Black)
                            peiceImg = Image.FromFile("ChessPieces/bishopb.png");
                        else
                            peiceImg = Image.FromFile("ChessPieces/bishopw.png");
                        break;
                    case Type.Queen:
                        if (Black)
                            peiceImg = Image.FromFile("ChessPieces/queenb.png");
                        else
                            peiceImg = Image.FromFile("ChessPieces/queenw.png");
                        break;
                    case Type.Pawn:
                        if (Black)
                            peiceImg = Image.FromFile("ChessPieces/pawnb.png");
                        else
                            peiceImg = Image.FromFile("ChessPieces/pawnw.png");
                        break;
                    case Type.Knight:
                        if (Black)
                            peiceImg = Image.FromFile("ChessPieces/knightb.png");
                        else
                            peiceImg = Image.FromFile("ChessPieces/knightw.png");
                        break;
                    case Type.Rook:
                        if (Black)
                            peiceImg = Image.FromFile("ChessPieces/rookb.png");
                        else
                            peiceImg = Image.FromFile("ChessPieces/rookw.png");
                        break;
                    default:
                        peiceImg = Image.FromFile("ChessPieces/pawnw.png");
                        throw new Exception("Invalid peice draw");
                        //break;
                }

                Rectangle drawRect = new Rectangle(x * X_UNIT, y * Y_UNIT, X_UNIT, Y_UNIT);
                g.DrawImage(peiceImg, drawRect);
            }

        }



    }
}
