using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameTetrisPacific
{
    public partial class Form1 : Form
    {
        ShapeCreate new_shape = new ShapeCreate();
        public int size_elem = 45;                   //Size of rectange element on cup
        public int[,] stakan = new int[8, 15];      //Array of main cup
        public bool m_right = true;                 //Variables for edge cases(right - left border, bottom of cup, collisions with other shapes)
        public bool m_left = true;
        public bool m_down = true;
        public int score = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Graphics g = CreateGraphics();
        }

        public void button1_Click(object sender, EventArgs e)   //Start button create shape from class, clear the cup, blocks button while the game continues, launches timer
        {
            new_shape.Create(2, 0);

            label2.Text = "";

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 15; j++)
                {
                    stakan[i, j] = 0;
                }
            }

            button1.Enabled = false;

            timer1.Enabled = true;

            timer1.Interval = 700;

            timer1.Start();

            Connect_map_stakan();

            DrawField();

            Clear_Field();
        }

        public void update_cycle(object sender, EventArgs e)  //It's works with timer interval for update coordinate y (moves shape down) and check border cases(right-left-bottom, collision with other shapes and delete filling lines in the cup)
        {
            chek_fill_line();

            DrawField();

            Clear_Field();

            m_down = true;

            Chek_Border_Event_Collision();

            if (m_down)
            {
                new_shape.y++;
            }

            Connect_map_stakan();

            DrawField();

        }

        private void chek_fill_line()       //Checks the horizontal lines in the cup and deletes, if it's filling. After moves down top part of the cup in empty lines.
        {
            for (int i = 0; i < 15; i++)
            {

                int count = 0;

                for (int j = 0; j < 8; j++)
                {
                    if (stakan[j, i] == 2)
                    {
                        count++;

                        if (count == 8)
                        {
                            for (int k = 0; k < 8; k++)
                            {
                                stakan[k, i] = 0;
                            }
                            for (int i1 = i; i1 > 0; i1--)
                            {
                                for (int j1 = 0; j1 < 8; j1++)
                                {
                                    if (i1 > 0)
                                    {
                                        stakan[j1, i1] = stakan[j1, i1 - 1];
                                    }

                                }
                            }
                            score += 100;                       // After deleting lines writes score on the form (100 points for 1 lines)
                            label1.Text = "Score = " + score;
                        }
                    }
                }
            }
        }

        private void Chek_Border_Event_Collision()          //Cheks if the shape collisions with bottom of the cup or other shapes. If its true, rewrites matrix in the cup with values = 2 (function fill_two())
        {
            for (int i = new_shape.x; i < new_shape.x + new_shape.Mat_size; i++)
            {
                for (int j = new_shape.y; j < new_shape.y + new_shape.Mat_size; j++)
                {
                    if (new_shape.Matrix[j - new_shape.y, i - new_shape.x] == 1)
                    {
                        if (j == 14 || stakan[i, j + 1] == 2)
                        {
                            fill_two();         //if was collisions with other shapes or borders of cup, stops moving and rewrites all units into twos in the cup
                            m_down = false;
                            break;
                        }
                    }
                }
                if (m_down == false)
                {
                    if (new_shape.y == 0)     //Cheks the filling of cup to the top. Then Game Over, StopTimer
                    {
                        timer1.Enabled = false;
                        label2.Text = "Game over!";
                        button1.Enabled = true;
                        break;

                    }
                    else
                    {
                        timer1.Interval = 700;          //or continue game with new shape from class
                        new_shape.Create(2, 0);
                        break;
                    }
                }
            }
        }

        private void Clear_Field()          // After each step of the timer deletes old position of matrix rewrites 1 into 0
        {
            for (int i = new_shape.x; i < new_shape.x + new_shape.Mat_size; i++)
            {
                for (int j = new_shape.y; j < new_shape.y + new_shape.Mat_size; j++)
                {

                    if (new_shape.Matrix[j - new_shape.y, i - new_shape.x] == 1)
                    {

                        stakan[i, j] = 0;

                    }
                }
            }
        }

        private void Connect_map_stakan()           //Replace elements of matrix on the cup if matrix's element is 1. Writes all 1 in the cup.
        {
            for (int i = new_shape.x; i < new_shape.x + new_shape.Mat_size; i++)
            {
                for (int j = new_shape.y; j < new_shape.y + new_shape.Mat_size; j++)
                {

                    if (new_shape.Matrix[j - new_shape.y, i - new_shape.x] == 1)
                    {

                        stakan[i, j] = new_shape.Matrix[j - new_shape.y, i - new_shape.x];

                    }
                }
            }
        }

        private void DrawField()            //Redraws the moving and fixed shapes in the cup (with values = 1 and 2), also draws the black grid and background
        {
            Graphics g = CreateGraphics();
            SolidBrush br = new SolidBrush(Color.AntiqueWhite);

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 15; j++)
                {
                    if (stakan[i, j] == 1 || stakan[i, j] == 2)
                    {
                        g.FillRectangle(br, i * size_elem, j * size_elem, size_elem, size_elem);
                    }
                    else
                    {
                        g.FillRectangle(Brushes.Gray, i * size_elem, j * size_elem, size_elem, size_elem);
                    }
                    g.DrawRectangle(Pens.Black, i * size_elem, j * size_elem, size_elem, size_elem);
                }
            }

        }

        private void Chek_Border_Event_Left_Right()         // Cheks if the shape does not go beyond the borders (left and right). If it's true, stop event move_left or move_right
        {
            for (int j = 0; j < 15; j++)
            {
                if (stakan[0, j] == 1)
                {
                    m_left = false;
                    break;
                }
                if (stakan[7, j] == 1)
                {
                    m_right = false;
                    break;
                }

                for (int k = new_shape.x; k < new_shape.x + new_shape.Mat_size; k++)        //This loop cheks the position of shape near borders(left and right) for the rotate. If it is so close, moves the shape out from border on the lenth of matrix(mat_size)
                {
                    if (new_shape.y > 15 - new_shape.Mat_size)
                    {
                        Clear_Field();
                        new_shape.Mat_size = 15 - new_shape.y;
                    }
                    for (int p = new_shape.y; p < new_shape.y + new_shape.Mat_size; p++)        //Cheks if is there other shape on the right or on the left. If it's true, blocks move_right or move_left
                    {
                        if (new_shape.Matrix[p - new_shape.y, k - new_shape.x] == 1)
                        {
                            if (k < 7 && stakan[k + 1, p] == 2)
                            {
                                m_right = false;
                            }
                            if (k > 0 && stakan[k - 1, p] == 2)
                            {
                                m_left = false;
                            }
                        }

                    }
                }
            }
        }

        public void fill_two()      //Fills the fixed shapes with value =2 in the cup
        {
            for (int i = new_shape.x; i < new_shape.x + new_shape.Mat_size; i++)
            {
                for (int j = new_shape.y; j < new_shape.y + new_shape.Mat_size; j++)
                {
                    if (new_shape.Matrix[j - new_shape.y, i - new_shape.x] == 1)
                    {
                        stakan[i, j] = 2;
                    }
                }
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)     //Handler of events for buttons controls of the game
        {
            if (e.KeyCode == Keys.Down)     //acceleration of movement shape down
            {
                timer1.Interval = 1;
            }
            if (e.KeyCode == Keys.Right)        //move right x++
            {
                m_right = true;
                Chek_Border_Event_Left_Right();     //Cheks border conditions

                Clear_Field();

                if (m_right)
                {
                    new_shape.x++;
                }
            }

            if (e.KeyCode == Keys.Left)     //Move left x--
            {
                m_left = true;

                Chek_Border_Event_Left_Right();

                Clear_Field();

                if (m_left)
                {
                    new_shape.x--;
                }
            }

            if (e.KeyCode == Keys.Up)       //Rotates the shape
            {
                if (new_shape.x < 0)        //Cheks border conditions for correct rotate
                {
                    Clear_Field();
                    new_shape.x = 0;
                }
                if (new_shape.x + new_shape.Mat_size > 8)
                {
                    Clear_Field();
                    new_shape.x = 8 - new_shape.Mat_size;
                }
                Clear_Field();

                int[,] n = new int[new_shape.Mat_size, new_shape.Mat_size];     //Additional array for rotate in two stages. 

                for (int i = 0; i < new_shape.Mat_size; i++)
                {
                    for (int j = 0; j < new_shape.Mat_size; j++)
                    {
                        if (new_shape.Matrix[i, j] == 1 || new_shape.Matrix[i, j] == 0)
                        {
                            n[i, j] = new_shape.Matrix[new_shape.Mat_size - i - 1, j];      //First stage - create the mirror array
                        }

                    }
                }

                for (int i = 0; i < new_shape.Mat_size; i++)
                {
                    for (int j = 0; j < new_shape.Mat_size; j++)
                    {
                        new_shape.Matrix[i, j] = n[j, i];           //Second stage - rotate mirror array
                    }
                }
            }

            Connect_map_stakan();       //Definites position new array in the cap
            DrawField();                    // Draws the new rotate shape
        }
    }
}
