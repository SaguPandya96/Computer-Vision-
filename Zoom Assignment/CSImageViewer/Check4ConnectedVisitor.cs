using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSImageViewer {
    class Check4ConnectedVisitor {
        private bool is4Connected = false;
        private int  badCount     = 0;

        //record the position of one bad one (if any).
        // it does not matter which one (if any).
        private int  badRow       = -1;
        private int  badCol       = -1;

        public void visit ( GrayImageData g ) {

        }

        public bool get ( ) { return is4Connected; }
        public int getBadRow ( ) { return badRow; }
        public int getBadCol ( ) { return badCol; }
        public int getBadCount ( ) { return badCount; }
    }
}
