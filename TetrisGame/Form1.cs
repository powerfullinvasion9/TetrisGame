using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TetrisGame
{
    public partial class Form1 : Form
    {
        Shape currentShape;
        Shape nextShape;
        Timer timer = new Timer();

        public Form1()
        {
            
            InitializeComponent();
            loadCanvas();

            currentShape = getRandomShapeWithCenterAigned();

            timer.Tick += Timer_Tick;
            timer.Interval = 500;
            timer.Start();

            this.KeyDown += Form1_KeyDown;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            var horizontalMove = 0;
            var verticalMove = 0;  

            switch (e.KeyCode)
            {
                case Keys.Left:
                    verticalMove--;
                    break;

                case Keys.Right:
                    verticalMove++;
                    break;

                case Keys.Down:
                    horizontalMove++;
                    break;

                case Keys.Up:
                    currentShape.turn();
                    break;

                default:
                    return;
            }

            var isMoveSuccess = moveShapeIfPossible(horizontalMove, verticalMove);
            if (!isMoveSuccess && e.KeyCode == Keys.Up)
                currentShape.rollBack();
        }

        Bitmap canvasBitmap;
        Graphics canvasGraphics;
        int canvasWidth = 15;
        int canvasHeight = 20;
        int[,] canvasDotArray;
        int dotSize = 20;

        private void loadCanvas()
        {
            pictureBox1.Width = canvasWidth * dotSize;
            pictureBox1.Height = canvasHeight * dotSize;

            canvasBitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            canvasGraphics = Graphics.FromImage(canvasBitmap);

            canvasGraphics.FillRectangle(Brushes.LightGray, 0, 0, canvasBitmap.Width, canvasBitmap.Height);

            pictureBox1.Image = canvasBitmap;
            canvasDotArray = new int[canvasWidth, canvasHeight];
        }

        int currentX;
        int currentY;

        private Shape getRandomShapeWithCenterAigned()
        {
            var shape = ShapeHandler.GetRandomShape();
            currentX = 7;
            currentY = -shape.Height;

            return shape;
        }

        private bool moveShapeIfPossible(int moveDown = 0, int moveSide = 0)
        {
            var newX = currentX + moveSide;
            var newY = currentY + moveDown;

            if (newX < 0 || newX + currentShape.Width > canvasWidth
                || newY + currentShape.Height > canvasHeight)
            {
                return false;
            }
                
            for(int i = 0; i < currentShape.Width; i++)
            {
                for(int j =0; j < currentShape.Height; j++)
                {
                    if (newY + j > 0 && canvasDotArray[newX + i, newY + j] == 1 && currentShape.Dots[j, i] == 1)
                    {
                        return false;
                    }
                }
            }

            currentX = newX;
            currentY = newY;

            drawShape();
            return true;
        }

        Bitmap workingBitmap;
        Graphics workingGraphics;

        private void drawShape()
        {
            workingBitmap = new Bitmap(canvasBitmap);
            workingGraphics = Graphics.FromImage(workingBitmap);

            for (int i = 0; i < currentShape.Width; i++)
            {
                for (int j = 0; j < currentShape.Height; j++)
                {
                    if (currentShape.Dots[j, i] == 1)
                        workingGraphics.FillRectangle(Brushes.Black, (currentX + i) * dotSize, (currentY + j) * dotSize, dotSize, dotSize);
                }
            }

            pictureBox1.Image = workingBitmap;
        }

        private void updateCanvasDotArrayWithCurrentShape()
        {
            for (int i = 0; i < currentShape.Width; i++)
            {
                for (int j = 0; j < currentShape.Height; j++)
                {
                    if (currentShape.Dots[j, i] == 1)
                    {
                        checkIfGameOver();

                        canvasDotArray[currentX + i, currentY + j] = 1;
                    }
                }
            }
        }

        private void checkIfGameOver()
        {
            if (currentY < 0)
            {
                timer.Stop();
                MessageBox.Show("Game Over");
                Application.Restart();
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            var isMoveSuccess = moveShapeIfPossible(moveDown: 1);

            if (!isMoveSuccess)
            {
                canvasBitmap = new Bitmap(workingBitmap);

                updateCanvasDotArrayWithCurrentShape();

                currentShape = getRandomShapeWithCenterAigned();

                clearFilledRowsAndUpdateScore();
            }
        }

        int score;

        public void clearFilledRowsAndUpdateScore()
        {
            // check through each rows
            for (int i = 0; i < canvasHeight; i++)
            {
                int j;
                for (j = canvasWidth - 1; j >= 0; j--)
                {
                    if (canvasDotArray[j, i] == 0)
                        break;
                }

                if (j == -1)
                { 
                    // update the dot array based on the check
                    for (j = 0; j < canvasWidth; j++)
                    {
                        for (int k = i; k > 0; k--)
                        {
                            canvasDotArray[j, k] = canvasDotArray[j, k - 1];
                        }

                        canvasDotArray[j, 0] = 0;
                    }
                }
            }

            for (int i = 0; i < canvasWidth; i++)
            {
                for (int j = 0; j < canvasHeight; j++)
                {
                    canvasGraphics = Graphics.FromImage(canvasBitmap);
                    canvasGraphics.FillRectangle(
                        canvasDotArray[i, j] == 1 ? Brushes.Black : Brushes.LightGray,
                        i * dotSize, j * dotSize, dotSize, dotSize
                        );
                }
            }

            pictureBox1.Image = canvasBitmap;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
