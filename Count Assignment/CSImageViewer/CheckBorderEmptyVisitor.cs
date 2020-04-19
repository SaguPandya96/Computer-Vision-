
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//----------------------------------------------------------------------
namespace CSImageViewer {
    public class CheckBorderEmptyVisitor {
        private bool isBorderEmpty = false;

        /** \brief  <b> This method checks if image border is empty  here</b>
             *  
             *  \param  g        object of GrayImageData here
             * 
             *
             *
             *  \returns  sets value of isBorderEmpty here
             */

        public void visit ( GrayImageData g ) {

            CheckBinaryVisitor  cbv=new CheckBinaryVisitor(); //  gets min value of image here
            cbv.visit( g );
            int min=cbv.getMin();
            int c1=0;
            int c2=0;
            int c3=0;
            int c4=0;
            int h=g.getH();
            int w = g.getW();

            for (int r = 0; r < g.getW(); r++) {    //Check top row here
                if (g.getGray( 0, r ) == min)
                    c1 = 1;
                else {
                    c1 = 0;
                    break;
                }
            }
            for (int r = 0; r < g.getW(); r++) {     //Check bottom row here
                if (g.getGray( h - 1, r ) == min)
                    c2 = 1;
                else {
                    c2 = 0;
                    break;
                }
            }
            for (int r = 0; r < g.getH(); r++) {      //Check left column here

                if (g.getGray( r, 0 ) == min)
                    c3 = 1;
                else {
                    c3 = 0;
                    break;
                }
            }
            for (int r = 0; r < g.getH(); r++) {    //Check right column here
                if (g.getGray( r, w - 1 ) == min)
                    c4 = 1;
                else {
                    c4 = 0;
                    break;
                }
            }
            if (c1 == 1 && c2 == 1 && c3 == 1 && c4 == 1)   // borders are empty  here 
                isBorderEmpty = true;
        }

        public bool get ( )
            { 
            return isBorderEmpty; 
        }
    }
}
