using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisGame
{
    class Shape
    {
        public int Width;
        public int Height;
        public int[,] Dots;

        private int[,] backUpDots;

        public void turn()
        {
            backUpDots = Dots;
            Dots = new int[Width, Height];

            for(int i = 0; i < Width; i++)
            {
                for(int j = 0; j < Height; j++)
                {
                    Dots[i, j] = backUpDots[Height - 1 - j, i];
                }
            }

            var temp = Width;
            Width = Height;
            Height = temp;
        }

        public void rollBack()
        {
            Dots = backUpDots;
            var temp = Width;
            Width = Height;
            Height = temp;
        }
    }
    internal static class ShapeHandler
    {
        private static Shape[] shapesArray;

        static ShapeHandler()
        {
            shapesArray = new Shape[]
            {
                new Shape
                {
                    Width = 2,
                    Height = 2,
                    Dots = new int[,]
                    {
                        {1,1},
                        {1,1}
                    }
                },
                new Shape {
                        Width = 1,
                        Height = 4,
                        Dots = new int[,]
                        {
                            { 1 },
                            { 1 },
                            { 1 },
                            { 1 }
                        }
                    },
                    new Shape {
                        Width = 3,
                        Height = 2,
                        Dots = new int[,]
                        {
                            { 0, 1, 0 },
                            { 1, 1, 1 }
                        }
                    },
                    new Shape {
                        Width = 3,
                        Height = 2,
                        Dots = new int[,]
                        {
                            { 0, 0, 1 },
                            { 1, 1, 1 }
                        }
                    },
                    new Shape {
                        Width = 3,
                        Height = 2,
                        Dots = new int[,]
                        {
                            { 1, 0, 0 },
                            { 1, 1, 1 }
                        }
                    },
                    new Shape {
                        Width = 3,
                        Height = 2,
                        Dots = new int[,]
                        {
                            { 1, 1, 0 },
                            { 0, 1, 1 }
                        }
                    },
                    new Shape {
                        Width = 3,
                        Height = 2,
                        Dots = new int[,]
                        {
                            { 0, 1, 1 },
                            { 1, 1, 0 }
                        }
                    }
            };
        }
        public static Shape GetRandomShape()
        {
            var shape = shapesArray[new Random().Next(shapesArray.Length)];
            return shape;
        }
    }
}
