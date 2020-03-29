using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSImageViewer
{
    public class Check4ConnectedVisitor   //Public is define already 
    {
        private bool is4Connected = false;
        private int badCount = 0;

        //record the position of one bad one (if any).
        // it does not matter which one (if any).
        private int badRow = -1;
        private int badCol = -1;

        /** \brief  <b> This method checks if input image is 4connected
             *  
             *  \param  g        GrayImageData class object
             *
             *
             *  \returns   sets value of badRow,badCount,badCol,is4Connected       
         */


        public void visit(GrayImageData g)
        {
            int row = g.getH();
            int col = g.getW();
            int[,] L = new int[row, col];   //array for the label row and col here
            int num = 1;
            CheckBinaryVisitor cb = new CheckBinaryVisitor();
            CheckBorderEmptyVisitor cb1 = new CheckBorderEmptyVisitor();
            cb.visit(g);
            cb1.visit(g);
            int min = cb.getMin();
            if (cb.get() == true && cb1.get() == true)
            {       //if image is Binary and border empty then method will applied
                for (int r = 1; r < row; r++)
                {
                    for (int c = 1; c < col; c++)
                    {
                        if (g.getGray(r, c) == min)
                        {     //if value is min then label is 0 else assign new label here
                            L[r, c] = 0;
                            badRow = r;
                            badCol = c;
                            badCount = badCount + 1;
                        }
                        else
                        {
                            if (L[r - 1, c] == 0 && L[r, c - 1] == 0)
                            {   //Check the label for left and top column here
                                L[r, c] = num++;
                            }
                            else if (L[r - 1, c] != 0 && L[r, c - 1] == 0) //if top col has label  use same label
                            {
                                L[r, c] = L[r - 1, c];
                                is4Connected = true;
                            }
                            else if (L[r - 1, c] == 0 && L[r, c - 1] != 0)  //if left col has label  use same label
                            {
                                L[r, c] = L[r, c - 1];
                                is4Connected = true;
                            }
                            else if (L[r - 1, c] != 0 && L[r, c - 1] != 0) //if both have label use same label
                            {
                                L[r, c - 1] = L[r - 1, c];
                                L[r, c] = L[r - 1, c];
                                is4Connected = true;

                            }
                        }
                    }

                }

            }
            else if (cb.get() == true && cb1.get() != true)  //if border is not empty mark label for non empty border positions
            {
                int h = g.getH();
                int w = g.getW();
                int l = 1;
                for (int r = 0; r < g.getW(); r++)
                {    // labels to top row if pixel not empty here
                    if (g.getGray(0, r) != min)
                        L[0, r] = l++;
                    else
                    {
                        L[0, r] = 0;
                    }

                }

                for (int r = 1; r < g.getH(); r++)
                {      //labels to left column if pixel not empty here
                    if (g.getGray(r, 0) != min)
                        L[r, 0] = l++;
                    else
                    {
                        L[r, 0] = 0;
                    }

                }

                //code to check 4 connected in  border(top  row &  left col) here

                if (L[0, 0] != 0)
                {            //Check first col label here
                    if (L[1, 0] != 0)
                    {         //Check second row first col here 
                        L[1, 0] = L[0, 0];
                        is4Connected = true;
                    }
                    else if (L[0, 1] != 0)
                    {  //Check first row here and  second col here
                        L[0, 1] = L[0, 0];
                        is4Connected = true;
                    }
                    else if (L[0, 1] != 0 && L[1, 0] != 0)
                    {   //if  both col below and col to right are connected  here
                        L[1, 0] = L[0, 1] = L[0, 0];
                        is4Connected = true;
                    }
                }

                //All other columns here
                for (int r = 1; r < row; r++)
                {
                    for (int c = 1; c < col; c++)
                    {
                        if (g.getGray(r, c) == min)
                        {     //if value is min then label is 0
                            L[r, c] = 0;
                            badRow = r;
                            badCol = c;
                            badCount = badCount + 1;
                        }
                        else
                        {
                            if (L[r - 1, c] == 0 && L[r, c - 1] == 0)
                            {   //Check label for left and top column here
                                L[r, c] = l++;
                            }
                            else if (L[r - 1, c] != 0 && L[r, c - 1] == 0) //if top col has label  use same label here
                            {
                                L[r, c] = L[r - 1, c];
                                is4Connected = true;
                            }
                            else if (L[r - 1, c] == 0 && L[r, c - 1] != 0)  //if left col has label  use same label here 
                            {
                                L[r, c] = L[r, c - 1];
                                is4Connected = true;
                            }
                            else if (L[r - 1, c] != 0 && L[r, c - 1] != 0) //if both have label use min label here 
                            {
                                L[r, c - 1] = L[r - 1, c];
                                L[r, c] = L[r - 1, c];
                                is4Connected = true; // when the method true

                            }
                        }
                    }

                }

            }

        }
        public bool get() { return is4Connected; }
        public int getBadRow() { return badRow; }
        public int getBadCol() { return badCol; }
        public int getBadCount() { return badCount; }
    }
}