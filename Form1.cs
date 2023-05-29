using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace test
{
    public partial class Form1 : Form
    {

        public static string user = "";


        private const int BoardSize = 8;
        private Button[,] boardButtons;
        private int solutionsFound = 0;
        private List<string> identifiedSolutions= new List<string>();
        private string connectionString = "Data Source=DESKTOP-BDQFKCF;Initial Catalog=EightQueen;Integrated Security=True";
       

        public object Interaction { get; private set; }

        public Form1()
        {
            InitializeComponent();
            CreateGameBoard();
            user = Login.SetValueForText;
        }

        private void CreateGameBoard()
        {
            boardButtons = new Button[BoardSize, BoardSize];
            



            for (int row = 0; row < BoardSize; row++)
            {
                for (int col = 0; col < BoardSize; col++)
                {
                    Button button = new Button
                    {
                        Width = 90,
                        Height = 90,
                        Top = row * 90,
                        Left = col * 90,
                        Tag = new Point(row, col),
                        Font = new Font("Arial", 35),
                        ForeColor = Color.White,
                        ImageAlign=ContentAlignment.MiddleCenter,
                        BackColor = Color.LightSeaGreen,
                        TextAlign = ContentAlignment.MiddleCenter

                    };
                   BackColor = Color.White;
                    button.Click += QueenButton_Click;
                    Controls.Add(button);
                    boardButtons[row, col] = button;
               
                }
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            label2.Text = Login.SetValueForText;

        }
        public Button[,] GetBoardButtons()
        {
            return boardButtons;
        }

        public void QueenButton_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;

            Point position = (Point)button.Tag;
            int row = position.X;
            int col = position.Y;

            // Toggle queen placement
            if (button.Text == "♛")
            {
                button.Text = "";
                button.BackColor = Color.LightGray;
            }
            else
            {
                button.Text = "♛";
                button.BackColor = Color.LightSlateGray;
                
            }

            // Check if the queen threatens other queens
            if (CheckQueensThreatened())
            {
                MessageBox.Show("Queens are threatening each other! Try again.", "Invalid Placement", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                button.Text = "";
                button.BackColor = Color.Gray;
            }
            else
            {
                if (IsPuzzleSolved())
                {
                    MessageBox.Show("Congratulations! You solved the puzzle.", "Eight Queens' Puzzle", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Check if the solution has already been identified
                    if (!identifiedSolutions.Contains(GetBoardState()))
                    {
                        identifiedSolutions.Add(GetBoardState());
                        solutionsFound++;

                        // Save the solution in the database
                        SaveSolution(GetBoardState());

                        if (solutionsFound == GetMaxSolutions())
                        {
                            MessageBox.Show("You found all the solutions!", "Eight Queens' Puzzle", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ClearIdentifiedSolutions();
                            Application.Exit();
                        }
                    }
                    else
                    {
                        MessageBox.Show("This solution has already been identified.", "Eight Queens' Puzzle", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }

        private bool CheckQueensThreatened()
        {
            for (int row = 0; row < BoardSize; row++)
            {
                for (int col = 0; col < BoardSize; col++)
                {
                    Button button = boardButtons[row, col];

                    if (button.Text == "♛" && IsQueenThreatened(row, col))
                        return true;
                }
            }

            return false;
        }

        private bool IsQueenThreatened(int row, int col)
        {
            // Check row and column
            for (int i = 0; i < BoardSize; i++)
            {
                if (i != col && boardButtons[row, i].Text == "♛")
                    return true;

                if (i != row && boardButtons[i, col].Text == "♛")
                    return true;
            }

            // Check diagonals
            int[] dx = { -1, -1, 1, 1 };
            int[] dy = { -1, 1, -1, 1 };

            for (int i = 0; i < 4; i++)
            {
                int x = row;
                int y = col;

                while (x >= 0 && x < BoardSize && y >= 0 && y < BoardSize)
                {
                    if (x != row && y != col && boardButtons[x, y].Text == "♛")
                        return true;

                    x += dx[i];
                    y += dy[i];
                }
            }

            return false;
        }
       public bool IsPuzzleSolved()
        {
            int[] rows = new int[BoardSize];
            int[] cols = new int[BoardSize];

            for (int row = 0; row < BoardSize; row++)
            {
                for (int col = 0; col < BoardSize; col++)
                {
                    if (boardButtons[row, col].Text == "♛")
                    {
                        rows[row]++;
                        cols[col]++;

                        if (rows[row] > 1 || cols[col] > 1)
                            return false;
                    }
                }
            }

            // Check the diagonals for multiple queens
            for (int row = 0; row < BoardSize; row++)
            {
                for (int col = 0; col < BoardSize; col++)
                {
                    if (boardButtons[row, col].Text == "♛")
                    {
                        // Check upper-left diagonal
                        for (int i = 1; row - i >= 0 && col - i >= 0; i++)
                        {
                            if (boardButtons[row - i, col - i].Text == "♛")
                                return false;
                        }

                        // Check upper-right diagonal
                        for (int i = 1; row - i >= 0 && col + i < BoardSize; i++)
                        {
                            if (boardButtons[row - i, col + i].Text == "♛")
                                return false;
                        }

                        // Check lower-left diagonal
                        for (int i = 1; row + i < BoardSize && col - i >= 0; i++)
                        {
                            if (boardButtons[row + i, col - i].Text == "♛")
                                return false;
                        }

                        // Check lower-right diagonal
                        for (int i = 1; row + i < BoardSize && col + i < BoardSize; i++)
                        {
                            if (boardButtons[row + i, col + i].Text == "♛")
                                return false;
                        }
                    }
                }
            }
            return true;
        }
        private string GetBoardState()
        {
            string state = "";

            for (int row = 0; row < BoardSize; row++)
            {
                for (int col = 0; col < BoardSize; col++)
                {
                    if (boardButtons[row, col].Text == "♛")
                        state += "Q";
                    else
                        state += "-";
                }
            }

            return state;
        }
        private int GetMaxSolutions()
        {
          
            return 8; 
        }

        private void SaveSolution(string solution)
        {
            string userName = txtName.Text.Trim();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand("INSERT INTO Solutions (Username, Solution) VALUES (@Username, @Solution)", connection);
                    command.Parameters.AddWithValue("@Username", userName);
                    command.Parameters.AddWithValue("@Solution", solution);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving solution: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearIdentifiedSolutions()
        {
            identifiedSolutions.Clear();
            solutionsFound = 0;
        }
        private void Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void label2_Click(object sender, EventArgs e)
        {
            
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }

}

    

