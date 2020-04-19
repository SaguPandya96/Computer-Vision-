using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSImageViewer {
    public class CountObjects {
        private int externalCount = 0;  //<<< count of external corners
        private int internalCount = 0;  //<<< count of internal corners
        private int objectCount   = 0;  //<<< count of obects

        private bool isBinary     = false;
        private bool isBorderOK   = false;
        private bool is4Connected = false;
        private int min = 0;
        private int max = 0;
        private int badCount = 0;


        public void visit ( GrayImageData imageData ) {

            int rows = imageData.getH();
            int cols = imageData.getW();

            if (getIsBinary( imageData ) && getIsBorderOK( imageData ) && getIs4Connected( imageData )) {

                if (badCount == 0 && min != max) {

                    for (int R = 0; R < rows - 1; R++) {
                        for (int C = 0; C < cols - 1; C++) {
                            if (isExternalMatch( imageData, R, C )) {//external Match
                                externalCount = externalCount + 1;
                            }
                            if (isInternalMatch( imageData, R, C )) {//internal Match

                                internalCount = internalCount + 1;
                            }
                        }
                    }
                }

                objectCount = ((externalCount - internalCount) / 4);
            }
        }

        public int get ( ) { return objectCount; }
        public int getInternal ( ) { return internalCount; }
        public int getExternal ( ) { return externalCount; }

        public bool getIsBinary ( GrayImageData imageData ) {
            CheckBinaryVisitor cbv = new CheckBinaryVisitor();
            cbv.visit( imageData );
            min = cbv.getMin();
            max = cbv.getMax();
            if (cbv.get() == true)
                isBinary = true;
            else
                isBinary = false;
            return isBinary;
        }
        public bool getIsBorderOK ( GrayImageData imageData ) {
            CheckBorderEmptyVisitor cbev = new  CheckBorderEmptyVisitor();
            cbev.visit( imageData );

            if (cbev.get() == true)
                isBorderOK = true;
            else
                isBorderOK = false;

            return isBorderOK;
        }
        public bool getIs4Connected ( GrayImageData imageData ) {
            CheckBinaryVisitor  b1=new  CheckBinaryVisitor();
            Check4ConnectedVisitor  c1=new  Check4ConnectedVisitor();
            b1.visit( imageData );
            if (b1.get() == true) { //binary  image here
                c1.visit( imageData );
                if (c1.get() == true)
                    is4Connected = true;
                else
                    is4Connected = false;

                badCount = c1.getBadCount();
            } else
                is4Connected = false;
            return is4Connected;
        }

        public bool isExternalMatch ( GrayImageData imageData, int row, int col ) {
            bool isExternalMatch = false;
            int a,b,c,d =0;
            //current pixel value
            a = imageData.getGray( row, col );

            // neighbors pixel value
            b = imageData.getGray( row + 1, col );
            c = imageData.getGray( row, col + 1 );
            c = imageData.getGray( row + 1, col + 1 );

            //checking for pattern 1,0,0,0 or 0,1,0,0 or 0,0,1,0 or 0,0,0,1
            if (a == max && b == min && c == min && d == min
                || a == min && b == max && c == min && d == min
                || a == min && b == min && c == max && d == min
                || a == min && b == min && c == min && d == max) {
                isExternalMatch = true;
            }
            return isExternalMatch;
        }

        public bool isInternalMatch ( GrayImageData imageData, int row, int col ) {
            bool isInternalMatch = false;
            int a,b,c,d =0;
            //current pixel value
            a = imageData.getGray( row, col );

            // neighbors pixel value
            b = imageData.getGray( row + 1, col );
            c = imageData.getGray( row, col + 1 );
            c = imageData.getGray( row + 1, col + 1 );

            //checking for pattern 0,1,1,1 or 1,0,1,1 or 1,1,0,1 or 1,1,1,0
            if (a == min && b == max && c == max && d == max
                || a == max && b == min && c == max && d == max
                || a == max && b == max && c == min && d == max
                || a == max && b == max && c == max && d == min) {
                isInternalMatch = true;
            }
            return isInternalMatch;

        }
    }
}
