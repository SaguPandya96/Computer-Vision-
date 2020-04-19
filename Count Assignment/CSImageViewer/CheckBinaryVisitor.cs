using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSImageViewer {
    public class CheckBinaryVisitor {

        private bool isBinary = false;
        private int  min      = 0;
        private int  max      = 0;

        public void visit ( GrayImageData g ) {
            int size = g.getW()*g.getH();  // size of array for width and height

            for (int i = 0; i < size; i++) {
                if (g.getData( i ) < min || min == 0) min = g.getData( i );// value less then min
                if (g.getData( i ) > max) max = g.getData( i ); // value is greater then max
            }

            for (int i = 0; i < size; i++) {

                if ((g.getData( i ) == min) || (g.getData( i ) == max)) { // value is greater then max and min here
                    isBinary = true;
                } else {
                    isBinary = false;
                    break;
                }

            }   

        }

        public bool get ( ) { return isBinary; }
        public int getMin ( ) { return min; }
        public int getMax ( ) { return max; }
    }
}
