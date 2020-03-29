using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSImageViewer {
    class CheckBorderEmptyVisitor {

        private bool isBorderEmpty = false;

        public void visit ( GrayImageData g ) {
        }

        public bool get ( ) { return isBorderEmpty; }
    }
}
