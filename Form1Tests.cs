using Microsoft.VisualStudio.TestTools.UnitTesting;
using test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace test.Tests
{
    [TestClass()]
    public class Form1Tests
    {

        [TestMethod()]
        public void IsPuzzleSolvedTest()
        {

            // Arrange
            Form1 form = new Form1();
            Button[,] boardButtons = form.GetBoardButtons();



            // Set up the board with a solved puzzle
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    Button button = new Button();
                    button.Text = "";
                    boardButtons[row, col] = button;

                    // Add the button to the form (not shown in the unit test)
                    form.Controls.Add(button);
                }
            }

            // Place queens on the board to represent a solved puzzle
            boardButtons[0, 0].Text = "♛";
            boardButtons[1, 2].Text = "♛";
            boardButtons[2, 4].Text = "♛";
            boardButtons[3, 6].Text = "♛";
            boardButtons[4, 1].Text = "♛";
            boardButtons[5, 3].Text = "♛";
            boardButtons[6, 5].Text = "♛";
            boardButtons[7, 7].Text = "♛";

            // Act
            bool isSolved = form.IsPuzzleSolved();

            // Assert
            Assert.IsTrue(isSolved);
        }

    }
}



