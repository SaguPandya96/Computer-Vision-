/**
    \file   CSImageViewer.cs
    \brief  Contains CSImageViewer class definition.
    \author George J. Grevera, Ph.D., ggrevera@sju.edu

    Copyright (C) 2010, George J. Grevera

    This program is free software; you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program; if not, write to the Free Software
    Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307
    USA or from http://www.gnu.org/licenses/gpl.txt.

    This General Public License does not permit incorporating this
    code into proprietary programs.  (So a hypothetical company such
    as GH (Generally Hectic) should NOT incorporate this code into
    their proprietary programs.)
 */
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using System.Windows.Forms;
//----------------------------------------------------------------------
#pragma warning disable IDE1006

/** \brief read, represent, process, save, and display image data. */
namespace CSImageViewer {

    /** \brief Instantiate this class to display an image.
     *  v4.1 adds support for drag-and-drop of input files.
     *  \version 4.2 fixes scrolling issues.
     */
    public class CSImageViewer : Form {

        /** \brief  input file name filter string for image files */
        private const String sFilter
            = "All image files (*.bmp;*.ico;*.gif;*.jpg;*.png;*.pnm;*.ppm;*.pgm;*.tif;*.tiff)|*.bmp;*.ico;*.gif;*.jpg;*.png;*.pnm;*.ppm;*.pgm;*.tif;*.tiff"
            + "|Bitmap files (*.bmp)|*.bmp"
            + "|Icon files   (*.ico)|*.ico"
            + "|GIF files    (*.gif)|*.gif"
            + "|JPEG files   (*.jpg)|*.jpg"
            + "|PNG files    (*.png)|*.png"
            + "|PNM files    (*.pnm;*.ppm;*.pgm)|*.pnm;*.ppm;*.pgm"
            + "|TIFF files   (*.tif;*.tiff)|*.tif;*.tiff"
            + "|audio files  (*.wav)|*.wav"
            + "|All files    (*.*)|*.*";
        /** \brief allowable input file name extensions (endings).  keep lowercase! */
        private readonly String[] mExtentions = { ".bmp", ".ico", ".gif", ".jpg", ".png", ".pnm", ".ppm", ".pgm", ".tif", ".tiff" };
        public ImageData mImage;          ///< actual image data
        public ColorImageData mCImage;    /// < color   image data
        public  GrayImageData mGImage;   /// < gray image data
        private MainMenu mainMenu1;       ///< menubar
        private MenuItem FileMenuItem;    ///< file menu item
        private MenuItem FileOpen;        ///< open menu item
        private MenuItem FileSave;        ///< save menu item
        private MenuItem FileSaveAs;      ///< save as menu item
        private MenuItem menuSeparator;   ///< separator
        private MenuItem FileClose;       ///< close menu item
        private MenuItem FileExit;        ///< exit menu item

        private double Zoom = 1.0;        ///< zoom/scale factor
        private IContainer components;    ///< do not use directly

        private bool mMouseMoveValid = false;  ///< is mouse (x,y) below valid?
        private int  mMouseX;                  ///< mouse movement x position
        private int  mMouseY;                  ///< mouse movement y position
        private int  mPixelGray;               ///< grey pixel value at mouse location
        private int  mPixelRed;                ///< red pixel value at mouse location
        private int  mPixelGreen;              ///< green pixel value at mouse location
        private int  mPixelBlue;               ///< blue pixel value at mouse location

        private MenuItem menuItemRecentFiles;  ///< menu of recent files
        private MenuItem menuItemRecent1;      ///< recent file 1
        private MenuItem menuItemRecent2;      ///< recent file 2
        private MenuItem menuItemRecent3;      ///< recent file 3
        private MenuItem menuItemRecent4;      ///< recent file 4
        private MenuItem menuItemRecent5;      ///< recent file 5

        private MenuItem menuItemHelp;         ///< help
        private MenuItem menuItemAbout;        ///< about

        private readonly Font mFont = new Font( "Times New Roman", 12 );  ///< font for drawing strings
        private LinearGradientBrush  mBrush1;
        private MenuItem menuItem1;
        private MenuItem menuItem2;
        private MenuItem menuItem3;
        private MenuItem menuItem4;

        ///< brush for text overlay
        private LinearGradientBrush  mBrush2;  ///< another brush for text overlay
        //--------------------------------------------------------------
        /** \brief    Ctor that simply creates an empty window.
         *  \returns  nothing (ctor)
         */
        public CSImageViewer ( ) {
            init();
            this.Text = "CSImageViewer: <empty>";
            this.Cursor = Cursors.Cross;
            mImage = null;
        }
        //--------------------------------------------------------------
        /** \brief    Ctor that creates a window that displays the specified
         *  image file
         *  \param    fname  file name of image displayed in window
         *  \returns  nothing (ctor)
         */
        public CSImageViewer ( String fname ) {
            init();
            this.Cursor = Cursors.WaitCursor;
            this.mImage = ImageData.load( fname );
            this.Text = "CSImageViewer: " + fname;
            this.AutoScroll = true;
            if (this.AutoScroll)
                this.AutoScrollMinSize = new Size( (int)(mImage.mDisplayImage.Width * Zoom),
                                                   (int)(mImage.mDisplayImage.Height * Zoom) );
            this.Invalidate();
            this.Cursor = Cursors.Cross;
        }
        //--------------------------------------------------------------
        /**
         * \brief   common ctor initialization
         * \returns nothing (void)
         */
        private void init ( ) {
            InitializeComponent();
            mBrush1 = new LinearGradientBrush( this.ClientRectangle, Color.Black, Color.Yellow, LinearGradientMode.Horizontal );
            mBrush2 = new LinearGradientBrush( this.ClientRectangle, Color.White, Color.Yellow, LinearGradientMode.Horizontal );
            loadRecent();
        }
        //--------------------------------------------------------------
        /** \brief    Clean up any resources being used.
         *  \returns  nothing (void)
         */
        protected override void Dispose ( bool disposing ) {
            if (disposing) {
                if (components != null) {
                    components.Dispose();
                }
            }
            base.Dispose( disposing );
        }
        //--------------------------------------------------------------
        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent ( ) {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CSImageViewer));
            this.mainMenu1 = new System.Windows.Forms.MainMenu( this.components );
            this.FileMenuItem = new System.Windows.Forms.MenuItem();
            this.FileOpen = new System.Windows.Forms.MenuItem();
            this.FileSave = new System.Windows.Forms.MenuItem();
            this.FileSaveAs = new System.Windows.Forms.MenuItem();
            this.menuSeparator = new System.Windows.Forms.MenuItem();
            this.FileClose = new System.Windows.Forms.MenuItem();
            this.menuItemRecentFiles = new System.Windows.Forms.MenuItem();
            this.menuItemRecent1 = new System.Windows.Forms.MenuItem();
            this.menuItemRecent2 = new System.Windows.Forms.MenuItem();
            this.menuItemRecent3 = new System.Windows.Forms.MenuItem();
            this.menuItemRecent4 = new System.Windows.Forms.MenuItem();
            this.menuItemRecent5 = new System.Windows.Forms.MenuItem();
            this.FileExit = new System.Windows.Forms.MenuItem();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.menuItem4 = new System.Windows.Forms.MenuItem();
            this.menuItemHelp = new System.Windows.Forms.MenuItem();
            this.menuItemAbout = new System.Windows.Forms.MenuItem();
            this.SuspendLayout();
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.AddRange( new System.Windows.Forms.MenuItem[] {
            this.FileMenuItem,
            this.menuItem1,
            this.menuItemHelp} );
            // 
            // FileMenuItem
            // 
            this.FileMenuItem.Index = 0;
            this.FileMenuItem.MenuItems.AddRange( new System.Windows.Forms.MenuItem[] {
            this.FileOpen,
            this.FileSave,
            this.FileSaveAs,
            this.menuSeparator,
            this.FileClose,
            this.menuItemRecentFiles,
            this.FileExit} );
            this.FileMenuItem.Text = "File";
            // 
            // FileOpen
            // 
            this.FileOpen.Index = 0;
            this.FileOpen.Shortcut = System.Windows.Forms.Shortcut.CtrlO;
            this.FileOpen.Text = "Open";
            this.FileOpen.Click += new System.EventHandler( this.OnFileOpen );
            // 
            // FileSave
            // 
            this.FileSave.Index = 1;
            this.FileSave.Shortcut = System.Windows.Forms.Shortcut.CtrlS;
            this.FileSave.Text = "Save";
            this.FileSave.Click += new System.EventHandler( this.OnFileSave );
            // 
            // FileSaveAs
            // 
            this.FileSaveAs.Index = 2;
            this.FileSaveAs.Shortcut = System.Windows.Forms.Shortcut.CtrlA;
            this.FileSaveAs.Text = "Save As";
            this.FileSaveAs.Click += new System.EventHandler( this.OnFileSaveAs );
            // 
            // menuSeparator
            // 
            this.menuSeparator.Index = 3;
            this.menuSeparator.Text = "-";
            // 
            // FileClose
            // 
            this.FileClose.Index = 4;
            this.FileClose.Text = "Close";
            this.FileClose.Click += new System.EventHandler( this.OnFileClose );
            // 
            // menuItemRecentFiles
            // 
            this.menuItemRecentFiles.Index = 5;
            this.menuItemRecentFiles.MenuItems.AddRange( new System.Windows.Forms.MenuItem[] {
            this.menuItemRecent1,
            this.menuItemRecent2,
            this.menuItemRecent3,
            this.menuItemRecent4,
            this.menuItemRecent5} );
            this.menuItemRecentFiles.Text = "Recent Files";
            // 
            // menuItemRecent1
            // 
            this.menuItemRecent1.Index = 0;
            this.menuItemRecent1.Text = "1";
            this.menuItemRecent1.Click += new System.EventHandler( this.onRecentFile );
            // 
            // menuItemRecent2
            // 
            this.menuItemRecent2.Index = 1;
            this.menuItemRecent2.Text = "2";
            this.menuItemRecent2.Click += new System.EventHandler( this.onRecentFile );
            // 
            // menuItemRecent3
            // 
            this.menuItemRecent3.Index = 2;
            this.menuItemRecent3.Text = "3";
            this.menuItemRecent3.Click += new System.EventHandler( this.onRecentFile );
            // 
            // menuItemRecent4
            // 
            this.menuItemRecent4.Index = 3;
            this.menuItemRecent4.Text = "4";
            this.menuItemRecent4.Click += new System.EventHandler( this.onRecentFile );
            // 
            // menuItemRecent5
            // 
            this.menuItemRecent5.Index = 4;
            this.menuItemRecent5.Text = "5";
            this.menuItemRecent5.Click += new System.EventHandler( this.onRecentFile );
            // 
            // FileExit
            // 
            this.FileExit.Index = 6;
            this.FileExit.Text = "Exit";
            this.FileExit.Click += new System.EventHandler( this.OnFileExit );
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 1;
            this.menuItem1.MenuItems.AddRange( new System.Windows.Forms.MenuItem[] {
            this.menuItem2,
            this.menuItem3,
            this.menuItem4} );
            this.menuItem1.Text = "Binary";
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 0;
            this.menuItem2.Text = "Binary?";
            this.menuItem2.Click += new System.EventHandler( this.isBinaryCheck_Click );
            // 
            // menuItem3
            // 
            this.menuItem3.Index = 1;
            this.menuItem3.Text = "Border Empty?";
            this.menuItem3.Click += new System.EventHandler( this.isBorderEmpty_Click );
            // 
            // menuItem4
            // 
            this.menuItem4.Index = 2;
            this.menuItem4.Text = "4-connected?";
            this.menuItem4.Click += new System.EventHandler( this.is4Connected_Check );
            // 
            // menuItemHelp
            // 
            this.menuItemHelp.Index = 2;
            this.menuItemHelp.MenuItems.AddRange( new System.Windows.Forms.MenuItem[] {
            this.menuItemAbout} );
            this.menuItemHelp.Text = "Help";
            // 
            // menuItemAbout
            // 
            this.menuItemAbout.Index = 0;
            this.menuItemAbout.Text = "About CSImageViewer";
            this.menuItemAbout.Click += new System.EventHandler( this.OnHelpAbout );
            // 
            // CSImageViewer
            // 
            this.AllowDrop = true;
            this.AutoScaleBaseSize = new System.Drawing.Size( 5, 13 );
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size( 784, 540 );
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject( "$this.Icon" )));
            this.Menu = this.mainMenu1;
            this.Name = "CSImageViewer";
            this.Text = "CSImageViewer";
            this.Scroll += new System.Windows.Forms.ScrollEventHandler( this.OnScroll );
            this.DragDrop += new System.Windows.Forms.DragEventHandler( this.OnDragDrop );
            this.DragEnter += new System.Windows.Forms.DragEventHandler( this.OnDragEnter );
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler( this.OnKeyPress );
            this.MouseMove += new System.Windows.Forms.MouseEventHandler( this.OnMouseMoved );
            this.ResumeLayout( false );

        }
        #endregion
        //--------------------------------------------------------------
        /** \brief    Track mouse movement.
         *  \param    sender  sender
         *  \param    e  mouse event
         *  \returns  nothing (void)
         */
        private void OnMouseMoved ( object sender, MouseEventArgs e ) {
            //throw new Exception( "The method or operation is not implemented." );
            mMouseMoveValid = true;
            mMouseX = e.X;
            mMouseY = e.Y;
            Invalidate();
        }
        //--------------------------------------------------------------
        /** \brief    redraw the panel contents. This method simply draws/displays the ImageData's display image.
         *  \param    e  paint event args
         *  \returns  nothing (void)
         */
        protected override void OnPaint ( PaintEventArgs e ) {
            Graphics g = e.Graphics;
            g.InterpolationMode = InterpolationMode.NearestNeighbor;

            if (mImage != null && mImage.mDisplayImage != null) {
                if (this.AutoScroll)
                    g.DrawImage( mImage.mDisplayImage,
                                 new Rectangle( AutoScrollPosition.X, AutoScrollPosition.Y, (int)(mImage.mDisplayImage.Width * Zoom + 0.5), (int)(mImage.mDisplayImage.Height * Zoom + 0.5) ) );
                else
                    g.DrawImage( mImage.mDisplayImage,
                                 new Rectangle( 0, 0, (int)(mImage.mDisplayImage.Width * Zoom + 0.5), (int)(mImage.mDisplayImage.Height * Zoom + 0.5) ) );
            } else
                g.Clear( Color.DarkGray );

            if (mMouseMoveValid) {
                //we must add offset when scrolled.
                mMouseX += HorizontalScroll.Value;
                mMouseY += VerticalScroll.Value;

                //report position
                //calculate pixel
                if (mImage == null) {    //when no image is opened (initial  open)

                    g.DrawString( "(" + mMouseX + "," + mMouseY + ")", mFont, mBrush1, 20, 40 );
                    g.DrawString( "(" + mMouseX + "," + mMouseY + ")", mFont, mBrush2, 21, 41 );
                } else if (mImage.getIsColor() == false) { //when grey image is opened

                    mGImage = (GrayImageData)mImage;   //Initialize gray image class object
                    int size=   mImage.getW()  *  mImage.getH()  ;
                    int offset= ( mMouseY  * mImage.getW())+ mMouseX;
                    if (offset < size) //to handle Array out of bound for pointer over image boundaries 
                        {
                        mPixelGray = mGImage.getGray( mMouseY, mMouseX ); //Pixel value at mouse location
                        g.DrawString( "(" + mMouseX + "," + mMouseY + "," + "Pixel value:  " + mPixelGray + ")", mFont, mBrush1, 20, 40 );
                        g.DrawString( "(" + mMouseX + "," + mMouseY + "," + "Pixel value:  " + mPixelGray + ")", mFont, mBrush2, 21, 41 );
                    } else {  //when  mouse is outside image boundary
                        g.DrawString( "(" + mMouseX + "," + mMouseY + ")", mFont, mBrush1, 20, 40 );
                        g.DrawString( "(" + mMouseX + "," + mMouseY + ")", mFont, mBrush2, 21, 41 );

                    }
                } else if (mImage.getIsColor() == true) {

                    mCImage = (ColorImageData)mImage; //Initialize color image class object
                    int size=   mImage.getW()   *  mImage.getH()  *   3;
                    int offset= 3*((mMouseY * mImage.getW())+  mMouseX);
                    if (offset < size) { //to handle Array out of bound for pointer over image boundaries  
                        mPixelRed = mCImage.getRed( mMouseY, mMouseX );
                        mPixelGreen = mCImage.getGreen( mMouseY, mMouseX );
                        mPixelBlue = mCImage.getBlue( mMouseY, mMouseX );
                        g.DrawString( "(" + mMouseX + "," + mMouseY + "," + "Pixel value(R,G,B):  " + mPixelRed + "," + mPixelGreen + "," + mPixelBlue + ")", mFont, mBrush1, 20, 40 );
                        g.DrawString( "(" + mMouseX + "," + mMouseY + "," + "Pixel value(R,G,B):  " + mPixelRed + ", " + mPixelGreen + "," + mPixelBlue + ")", mFont, mBrush2, 21, 41 );
                    } else {        //when  mouse is outside image boundary
                        g.DrawString( "(" + mMouseX + "," + mMouseY + ")", mFont, mBrush1, 20, 40 );
                        g.DrawString( "(" + mMouseX + "," + mMouseY + ")", mFont, mBrush2, 21, 41 );

                    }
                }
            }
        }
        //--------------------------------------------------------------
        /** \brief    Handle file-open.
         *  \param    sender  sender
         *  \param    e  event args
         *  \returns  nothing (void)
         */
        private void OnFileOpen ( object sender, EventArgs e ) {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            //openFileDialog.InitialDirectory = "c:\\";
            openFileDialog.Filter = sFilter;
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;
            openFileDialog.Multiselect = true;

            if (DialogResult.OK == openFileDialog.ShowDialog()) {
                if (mImage == null) {  //any image currently?
                    this.mImage = ImageData.load( openFileDialog.FileName );
                    this.Text = "CSImageViewer: " + openFileDialog.FileName;
                    this.AutoScroll = true;
                    if (this.AutoScroll)
                        this.AutoScrollMinSize = new Size( (int)(mImage.mDisplayImage.Width * Zoom), (int)(mImage.mDisplayImage.Height * Zoom) );
                    Invalidate();
                } else {
                    for (int i = 0; i < openFileDialog.FileNames.Length; i++) {
                        CSImageViewer tmp = new CSImageViewer( openFileDialog.FileNames[i] );
                        tmp.Show();
                    }
                }

                saveRecent( openFileDialog.FileName );
                loadRecent();
            }

        }
        //--------------------------------------------------------------
        /** \brief    Handle file-save.
         *  \param    sender  sender
         *  \param    e  event args
         *  \returns  nothing (void)
         */
        private void OnFileSave ( object sender, EventArgs e ) {
            if (mImage == null) return;
            if (mImage.getFname() == null) return;
            DialogResult  dr = MessageBox.Show( "Replace file?", "Are you sure?", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning );
            if (dr != DialogResult.OK) return;

            mImage.saveDisplayImage( mImage.getFname() );
        }
        //--------------------------------------------------------------
        /** \brief    Handle file-saveas.
         *  \param    sender  sender
         *  \param    e  event args
         *  \returns  nothing (void)
         */
        private void OnFileSaveAs ( object sender, EventArgs e ) {
            if (mImage == null) return;
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            //saveFileDialog.InitialDirectory = "c:\\";
            saveFileDialog.Filter = sFilter;
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.RestoreDirectory = true;

            if (DialogResult.OK == saveFileDialog.ShowDialog()) {
                mImage.saveDisplayImage( saveFileDialog.FileName );
            }
        }
        //--------------------------------------------------------------
        /** \brief    Handle file-exit by exitting the application.
         *  \param    sender  sender
         *  \param    e  event args
         *  \returns  nothing (void)
         */
        private void OnFileExit ( object sender, System.EventArgs e ) {
            Environment.Exit( 0 );
        }
        //--------------------------------------------------------------
        /** \brief    Handle file-close by closing this window.
         *  \param    sender  sender
         *  \param    e  event args
         *  \returns  nothing (void)
         */
        private void OnFileClose ( object sender, EventArgs e ) {
            Close();
        }
        //--------------------------------------------------------------
        /** \brief    Handle help-about.
         *  \param    sender  sender
         *  \param    e  event args
         *  \returns  nothing (void)
         */
        private void OnHelpAbout ( object sender, EventArgs e ) {
            String msg = "This program brought to you by ... \n\n"
                       + "    George J. Grevera, Ph.D. \n\n";
            if (this.mImage != null) {
                msg += mImage;
            }
            MessageBox.Show( msg );
        }
        //--------------------------------------------------------------
        /** \brief    Handle drag-and-drop of files by creating a new
         *            CSImageViewer window for each.
         *  \param    sender  sender
         *  \param    e  event args
         *  \returns  nothing (void)
         */
        private void OnDragDrop ( object sender, DragEventArgs e ) {
            string[] files = (string[])e.Data.GetData( DataFormats.FileDrop );
            foreach (string f in files) {
                if (mImage == null) {
                    this.mImage = ImageData.load( f );
                    this.Text = "CSImageViewer: " + f;
                    this.AutoScroll = true;
                    if (this.AutoScroll)
                        this.AutoScrollMinSize = new Size( (int)(mImage.mDisplayImage.Width * Zoom), (int)(mImage.mDisplayImage.Height * Zoom) );
                    this.Invalidate();
                } else {
                    CSImageViewer tmp = new CSImageViewer(f);
                    tmp.Show();
                }
            }
        }
        //--------------------------------------------------------------
        /** \brief    Handle drag-enter (for drag-and-drop of files) by 
         *            check each file name extension, and rejecting bad
         *            ones.
         *  \param    sender  sender
         *  \param    e  event args
         *  \returns  nothing (void)
         */
        private void OnDragEnter ( object sender, DragEventArgs e ) {
            if (e.Data.GetDataPresent( DataFormats.FileDrop, false )) {
                //get the names of the files being dropped
                string[] files = (string[])e.Data.GetData( DataFormats.FileDrop );
                //check the extension of each file name
                foreach (string f in files) {
                    string lcf = f.ToLower();
                    bool foundIt = false;
                    foreach (string ext in mExtentions) {
                        if (lcf.EndsWith( ext )) {
                            foundIt = true;
                            break;
                        }
                    }
                    if (!foundIt) {
                        return;  //found a bad one!
                    }
                }
                e.Effect = DragDropEffects.All;  //all ok.
            }
        }
        //--------------------------------------------------------------
        /**
         * \brief add the file to the list of most recent files.
         * \param s is the file name to be added to the list.
         */
        private void saveRecent ( String s ) {
            //check for duplicates
            if (Properties.Settings.Default.File1.Equals( s )) return;
            if (Properties.Settings.Default.File2.Equals( s )) return;
            if (Properties.Settings.Default.File3.Equals( s )) return;
            if (Properties.Settings.Default.File4.Equals( s )) return;
            if (Properties.Settings.Default.File5.Equals( s )) return;
            //save most recent 5 files
            Properties.Settings.Default.File5 = Properties.Settings.Default.File4;
            Properties.Settings.Default.File4 = Properties.Settings.Default.File3;
            Properties.Settings.Default.File3 = Properties.Settings.Default.File2;
            Properties.Settings.Default.File2 = Properties.Settings.Default.File1;
            Properties.Settings.Default.File1 = s;

            Properties.Settings.Default.Save();
        }
        //--------------------------------------------------------------
        /**
         * \brief set up the sub menu of most recent files.
         */
        private void loadRecent ( ) {
            menuItemRecent1.Text = Properties.Settings.Default.File1;
            menuItemRecent2.Text = Properties.Settings.Default.File2;
            menuItemRecent3.Text = Properties.Settings.Default.File3;
            menuItemRecent4.Text = Properties.Settings.Default.File4;
            menuItemRecent5.Text = Properties.Settings.Default.File5;
        }
        //--------------------------------------------------------------
        /** \brief    one of the most recent files was selected.  create a viewer for it.
         *  \param    sender  sender
         *  \param    e  event args
         */
        private void onRecentFile ( object sender, EventArgs e ) {
            MenuItem mi = (MenuItem)sender;
            if (!System.IO.File.Exists( mi.Text )) return;
            if (mImage == null) {  //any image currently?
                this.mImage = ImageData.load( mi.Text );
                this.Text = "CSImageViewer: " + mi.Text;
                this.AutoScroll = true;
                if (this.AutoScroll)
                    this.AutoScrollMinSize = new Size( (int)(mImage.mDisplayImage.Width * Zoom), (int)(mImage.mDisplayImage.Height * Zoom) );
                Invalidate();
            } else {
                CSImageViewer tmp = new CSImageViewer( mi.Text );
                tmp.Show();
            }
        }
        //--------------------------------------------------------------
        /** \brief    scrolling. if we don't do this, a strange artifact
         *            will appear when scrolling large images.
         *  \param    sender  sender
         *  \param    e  event args
         */
        private void OnScroll ( object sender, ScrollEventArgs e ) {
            //Console.WriteLine( "OnScroll" );
            mMouseMoveValid = false;
            this.Invalidate();  //cause repaint
        }
        //--------------------------------------------------------------
        private void OnKeyPress ( object sender, KeyPressEventArgs e ) {
            Console.WriteLine( "OnKeyPress" );
        }

        private void isBinaryCheck_Click ( object sender, EventArgs e ) {
            CheckBinaryVisitor cbv = new CheckBinaryVisitor();
            if (mImage != null && mImage.getIsColor() != true) {
                mGImage = (GrayImageData)mImage;
                cbv.visit( mGImage );
                if (cbv.get() == true)
                    MessageBox.Show( "Image is a Binary Image" + "\n max value: " + cbv.getMax() + "\n min value: " + cbv.getMin() );
                else
                    MessageBox.Show( "Image is not a Binary Image" + "\n max value: " + cbv.getMax() + "\n min value: " + cbv.getMin() );
            } else
                MessageBox.Show( "Invalid Image" );
        }

        private void isBorderEmpty_Click ( object sender, EventArgs e ) {
            CheckBorderEmptyVisitor cbev = new  CheckBorderEmptyVisitor();
            if (mImage != null && mImage.getIsColor() != true) {
                mGImage = (GrayImageData)mImage;
                cbev.visit( mGImage );
                if (cbev.get() == true)
                    MessageBox.Show( "Image has empty borders" );
                else
                    MessageBox.Show( "Image border is not empty" );
            } else
                MessageBox.Show( "Invalid Image" );
        }

        private void is4Connected_Check ( object sender, EventArgs e ) {
            CheckBinaryVisitor  b1=new  CheckBinaryVisitor();
            Check4ConnectedVisitor  c1=new  Check4ConnectedVisitor();
            if (mImage != null && mImage.getIsColor() != true) {
                mGImage = (GrayImageData)mImage;
                b1.visit( mGImage );
                if (b1.get() == true) { //binary  image here
                    c1.visit( mGImage );
                    if (c1.get() == true)
                        MessageBox.Show( "Image is 4 Connected" + "\n bad count: " + c1.getBadCount() );
                    else
                        MessageBox.Show( "Image is not 4 connected" + "\n bad count: " + c1.getBadCount() + "\n bad pixel: (" + c1.getBadRow() + "," + c1.getBadCol() + ")" );
                } else
                    MessageBox.Show( "Image is not binary image" );
            } else
                MessageBox.Show( "Invalid Image" );
        }
    }
}
//----------------------------------------------------------------------

