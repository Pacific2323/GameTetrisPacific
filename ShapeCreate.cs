using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameTetrisPacific
{
    class ShapeCreate   //Creates random shape from list with coordinates in main cup
    {
        public int x;   //Cooordinates of matrix in the cup

        public int y;

        public Random r = new Random();

        public int Mat_size { get; set; }    //Size of matrix  

        public int[,] Matrix;             //Creates a few shapes in matrix form

        private int[,] Mat1 = new int[3, 3] {
                {0, 1, 0},
                {0, 1, 1},
                {0, 0, 1} };
        private int[,] Mat2 = new int[3, 3] {
                {0, 1, 0},
                {1, 1, 1},
                {0, 0, 0} };
        private int[,] Mat3 = new int[3, 3] {
                {0, 0, 1},
                {0, 1, 1},
                {0, 1, 0} };
        private int[,] Mat4 = new int[3, 3] {
                {1, 0, 0},
                {1, 1, 1},
                {0, 0, 0} };
        private int[,] Mat5 = new int[3, 3] {
                {0, 0, 1},
                {1, 1, 1},
                {0, 0, 0} };
        private int[,] Mat6 = new int[4, 4] {
                {0,0,0,0},
                {0,1,1,0},
                {0,1,1,0},
                {0,0,0,0} };
        private int[,] Mat7 = new int[4, 4] {
                {0,1,0,0},
                {0,1,0,0},
                {0,1,0,0},
                {0,1,0,0} };
        public void Create(int a, int b)        //Random choose of shapes with reference point in a=x, b=y
        {
            switch (r.Next(1, 8))
            {
                case 1:

                    Matrix = Mat1;
                    Mat_size = 3;
                    break;
                case 2:

                    Matrix = Mat2;
                    Mat_size = 3;
                    break;
                case 3:

                    Matrix = Mat3;
                    Mat_size = 3;
                    break;
                case 4:

                    Matrix = Mat4;
                    Mat_size = 3;
                    break;
                case 5:

                    Matrix = Mat5;
                    Mat_size = 3;
                    break;
                case 6:

                    Matrix = Mat6;
                    Mat_size = 4;
                    break;
                case 7:

                    Matrix = Mat7;
                    Mat_size = 4;
                    break;
            }
            x = a;
            y = b;

        }



    }
}
