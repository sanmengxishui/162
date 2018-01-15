using System;
using ViewROI;
using System.Collections;
using System.Collections.ObjectModel;
using HalconDotNet;
using JPTCG.Common;

namespace ViewROI
{
	public delegate void IconicDelegate(int val);
	public delegate void FuncDelegate();

	/// <summary>
	/// This class works as a wrapper class for the HALCON window
	/// HWindow. HWndCtrl is in charge of the visualization.
	/// You can move and zoom the visible image part by using GUI component 
	/// inputs or with the mouse. The class HWndCtrl uses a graphics stack 
	/// to manage the iconic objects for the display. Each object is linked 
	/// to a graphical context, which determines how the object is to be drawn.
	/// The context can be changed by calling changeGraphicSettings().
	/// The graphical "modes" are defined by the class GraphicsContext and 
	/// map most of the dev_set_* operators provided in HDevelop.
	/// </summary>
	public class HWndCtrl
	{
        public ObservableCollection<JPTCG.Vision.HalconInspection.RectData> myRect = new ObservableCollection<JPTCG.Vision.HalconInspection.RectData>();
        //public ObservableCollection<int> failIdx = new ObservableCollection<int>();
        public bool SamplingMode = false;

        public bool bShowCrossHair = true;
        public void ClearResult()
        {
            myRect.Clear();
            //repaint();
        }

        public  bool IsDrawCross1 = false;   //是否画十字
        public bool IsDrawCross2 = false;   //是否画十字
        public static bool IsOneshotGrab = false; //是否单次拍照变量
        private Object lockershow = new Object();
		/// <summary>No action is performed on mouse events</summary>
		public const int MODE_VIEW_NONE       = 10;

		/// <summary>Zoom is performed on mouse events</summary>
		public const int MODE_VIEW_ZOOM       = 11;

		/// <summary>Move is performed on mouse events</summary>
		public const int MODE_VIEW_MOVE       = 12;

		/// <summary>Magnification is performed on mouse events</summary>
		public const int MODE_VIEW_ZOOMWINDOW	= 13;


		public const int MODE_INCLUDE_ROI     = 1;

		public const int MODE_EXCLUDE_ROI     = 2;


		/// <summary>
		/// Constant describes delegate message to signal new image
		/// </summary>
		public const int EVENT_UPDATE_IMAGE   = 31;
		/// <summary>
		/// Constant describes delegate message to signal error
		/// when reading an image from file
		/// </summary>
		public const int ERR_READING_IMG      = 32;
		/// <summary> 
		/// Constant describes delegate message to signal error
		/// when defining a graphical context
		/// </summary>
		public const int ERR_DEFINING_GC      = 33;

		/// <summary> 
		/// Maximum number of HALCON objects that can be put on the graphics 
		/// stack without loss. For each additional object, the first entry 
		/// is removed from the stack again.
		/// </summary>
		private const int MAXNUMOBJLIST       = 5;


		private int    stateView;
		private bool   mousePressed = false;
		private double startX,startY;

		/// <summary>HALCON window</summary>
		private HWindowControl viewPort;

		/// <summary>
		/// Instance of ROIController, which manages ROI interaction
		/// </summary>
        private ROIController roiManager;

		/* dispROI is a flag to know when to add the ROI models to the 
		   paint routine and whether or not to respond to mouse events for 
		   ROI objects */
		private int           dispROI;


		/* Basic parameters, like dimension of window and displayed image part */
		private int   windowWidth;
		private int   windowHeight;
		private int   imageWidth;
		private int   imageHeight;

		private int[] CompRangeX;
		private int[] CompRangeY;


		private int    prevCompX, prevCompY;
		private double stepSizeX, stepSizeY;


		/* Image coordinates, which describe the image part that is displayed  
		   in the HALCON window */
		private double ImgRow1, ImgCol1, ImgRow2, ImgCol2;

		/// <summary>Error message when an exception is thrown</summary>
		public string  exceptionText = "";


		/* Delegates to send notification messages to other classes */
		/// <summary>
		/// Delegate to add information to the HALCON window after 
		/// the paint routine has finished
		/// </summary>
		public FuncDelegate   addInfoDelegate;

		/// <summary>
		/// Delegate to notify about failed tasks of the HWndCtrl instance
		/// </summary>
		public IconicDelegate NotifyIconObserver;


		private HWindow ZoomWindow;
		private double  zoomWndFactor;
		private double  zoomAddOn;
		private int     zoomWndSize;


		/// <summary> 
		/// List of HALCON objects to be drawn into the HALCON window. 
		/// The list shouldn't contain more than MAXNUMOBJLIST objects, 
		/// otherwise the first entry is removed from the list.
		/// </summary>
		private ArrayList HObjList;

		/// <summary>
		/// Instance that describes the graphical context for the
		/// HALCON window. According on the graphical settings
		/// attached to each HALCON object, this graphical context list 
		/// is updated constantly.
		/// </summary>
		private GraphicsContext	mGC;


		/// <summary> 
		/// Initializes the image dimension, mouse delegation, and the 
		/// graphical context setup of the instance.
		/// </summary>
		/// <param name="view"> HALCON window </param>
		public HWndCtrl(HWindowControl view)
		{
            //ImgRow1=0;
            //ImgCol1=0;
            //ImgRow2 = 0;
            //ImgCol2 = 0;
               viewPort = view;
			stateView = MODE_VIEW_NONE;
			windowWidth = viewPort.Size.Width;
			windowHeight = viewPort.Size.Height;

			zoomWndFactor = (double)imageWidth / viewPort.Width;
			zoomAddOn = Math.Pow(0.9, 5);
			zoomWndSize = 150;

			/*default*/
			CompRangeX = new int[] { 0, 100 };
			CompRangeY = new int[] { 0, 100 };

			prevCompX = prevCompY = 0;

			dispROI = MODE_INCLUDE_ROI;//1;

			viewPort.HMouseUp += new HalconDotNet.HMouseEventHandler(this.mouseUp);
			viewPort.HMouseDown += new HalconDotNet.HMouseEventHandler(this.mouseDown);
			viewPort.HMouseMove += new HalconDotNet.HMouseEventHandler(this.mouseMoved);

			addInfoDelegate = new FuncDelegate(dummyV);
			NotifyIconObserver = new IconicDelegate(dummy);

			// graphical stack 
			HObjList = new ArrayList(20);
			mGC = new GraphicsContext();
			mGC.gcNotification = new GCDelegate(exceptionGC);

		}


		/// <summary>
		/// Registers an instance of an ROIController with this window 
		/// controller (and vice versa).
		/// </summary>
		/// <param name="rC"> 
		/// Controller that manages interactive ROIs for the HALCON window 
		/// </param>
		public void useROIController(ROIController rC)
		{
			roiManager = rC;
			rC.setViewController(this);
		}


		/// <summary>
		/// Read dimensions of the image to adjust own window settings
		/// </summary>
		/// <param name="image">HALCON image</param>
		private void setImagePart(HImage image)
		{
			string s;
			int w,h;

			image.GetImagePointer1(out s, out w, out h);
			setImagePart(0, 0, h, w);
		}


		/// <summary>
		/// Adjust window settings by the values supplied for the left 
		/// upper corner and the right lower corner
		/// </summary>
		/// <param name="r1">y coordinate of left upper corner</param>
		/// <param name="c1">x coordinate of left upper corner</param>
		/// <param name="r2">y coordinate of right lower corner</param>
		/// <param name="c2">x coordinate of right lower corner</param>
		private void setImagePart(int r1, int c1, int r2, int c2)
		{
			ImgRow1 = r1;
			ImgCol1 = c1;
			ImgRow2 = imageHeight = r2;
			ImgCol2 = imageWidth = c2;

			System.Drawing.Rectangle rect = viewPort.ImagePart;
			rect.X = (int)ImgCol1;
			rect.Y = (int)ImgRow1;
			rect.Height = (int)imageHeight;
			rect.Width = (int)imageWidth;
			viewPort.ImagePart = rect;
		}


		/// <summary>
		/// Sets the view mode for mouse events in the HALCON window
		/// (zoom, move, magnify or none).
		/// </summary>
		/// <param name="mode">One of the MODE_VIEW_* constants</param>
		public void setViewState(int mode)
		{
			stateView = mode;

			if (roiManager != null)
				roiManager.resetROI();
		}

        public int GetViewState()
        {
            return stateView; 
        }
		/********************************************************************/
		private void dummy(int val)
		{
		}

		private void dummyV()
		{
		}

		/*******************************************************************/
		private void exceptionGC(string message)
		{
			exceptionText = message;
			NotifyIconObserver(ERR_DEFINING_GC);
		}

		/// <summary>
		/// Paint or don't paint the ROIs into the HALCON window by 
		/// defining the parameter to be equal to 1 or not equal to 1.
		/// </summary>
		public void setDispLevel(int mode)
		{
			dispROI = mode;
		}

		/****************************************************************************/
		/*                          graphical element                               */
		/****************************************************************************/
		private void zoomImage(double x, double y, double scale)
		{
			double lengthC, lengthR;
			double percentC, percentR;
			int    lenC, lenR;

			percentC = (x - ImgCol1) / (ImgCol2 - ImgCol1);
			percentR = (y - ImgRow1) / (ImgRow2 - ImgRow1);

			lengthC = (ImgCol2 - ImgCol1) * scale;
			lengthR = (ImgRow2 - ImgRow1) * scale;

			ImgCol1 = x - lengthC * percentC;
			ImgCol2 = x + lengthC * (1 - percentC);

			ImgRow1 = y - lengthR * percentR;
			ImgRow2 = y + lengthR * (1 - percentR);

			lenC = (int)Math.Round(lengthC);
			lenR = (int)Math.Round(lengthR);

			System.Drawing.Rectangle rect = viewPort.ImagePart;
           
			rect.X = (int)Math.Round(ImgCol1);
			rect.Y = (int)Math.Round(ImgRow1);
           
            rect.Width = (lenC > 0) ? lenC : viewPort.ImagePart.Size.Width;
            rect.Height = (lenR > 0) ? lenR : viewPort.ImagePart.Size.Height;
          
           
			viewPort.ImagePart = rect;

			zoomWndFactor *= scale;
            //lock (mmlocker)
            //{
                repaint();
            //}
			
		}

		/// <summary>
		/// Scales the image in the HALCON window according to the 
		/// value scaleFactor
		/// </summary>
		public void zoomImage(double scaleFactor)
		{
			double midPointX, midPointY;            

			if (((ImgRow2 - ImgRow1) == scaleFactor * imageHeight) &&
				((ImgCol2 - ImgCol1) == scaleFactor * imageWidth))
			{
				repaint();
				return;
			}

            double newImgWidth = (imageWidth - (ImgCol1 * 2)) ;
            double newImgHeight = (imageHeight - (ImgRow1*2));

            if (scaleFactor < 1)
            {
                if ((newImgWidth < 200) || (newImgHeight < 200))
                    return;
            }
            else
            {
                if ((newImgWidth > (imageWidth * 2)) || (newImgHeight > (imageHeight * 2)))
                    return;
            }

            ImgRow2 = ImgRow1 + newImgHeight;
            ImgCol2 = ImgCol1 + newImgWidth;

            midPointX = imageWidth / 2;
            midPointY = imageHeight / 2;

			//zoomWndFactor = (double)imageWidth / viewPort.Width;
            //lock (mmlocker)
            //{
                zoomImage(midPointX, midPointY, scaleFactor);
            //}
			
		}


		/// <summary>
		/// Scales the HALCON window according to the value scale
		/// </summary>
		public void scaleWindow(double scale)
		{
			ImgRow1 = 0;
			ImgCol1 = 0;

			ImgRow2 = imageHeight;
			ImgCol2 = imageWidth;

			viewPort.Width = (int)(ImgCol2 * scale);
			viewPort.Height = (int)(ImgRow2 * scale);

			zoomWndFactor = ((double)imageWidth / viewPort.Width);
		}

		/// <summary>
		/// Recalculates the image-window-factor, which needs to be added to 
		/// the scale factor for zooming an image. This way the zoom gets 
		/// adjusted to the window-image relation, expressed by the equation 
		/// imageWidth/viewPort.Width.
		/// </summary>
		public void setZoomWndFactor()
		{
			zoomWndFactor = ((double)imageWidth / viewPort.Width);
		}

		/// <summary>
		/// Sets the image-window-factor to the value zoomF
		/// </summary>
		public void setZoomWndFactor(double zoomF)
		{
			zoomWndFactor = zoomF;
            //repaint();
		}

        public double GetZoomWndFactor()
        {
            return zoomWndFactor;
        }
		/*******************************************************************/
		private void moveImage(double motionX, double motionY)
		{
			ImgRow1 += -motionY;
			ImgRow2 += -motionY;

			ImgCol1 += -motionX;
			ImgCol2 += -motionX;

			System.Drawing.Rectangle rect = viewPort.ImagePart;
			rect.X = (int)Math.Round(ImgCol1);
			rect.Y = (int)Math.Round(ImgRow1);
			viewPort.ImagePart = rect;
            //lock (mmlocker)
            //{
                repaint();
            //}
			
		}


		/// <summary>
		/// Resets all parameters that concern the HALCON window display 
		/// setup to their initial values and clears the ROI list.
		/// </summary>
		public void resetAll()
		{
			ImgRow1 = 0;
			ImgCol1 = 0;
			ImgRow2 = imageHeight;
			ImgCol2 = imageWidth;

			zoomWndFactor = (double)imageWidth / viewPort.Width;

			System.Drawing.Rectangle rect = viewPort.ImagePart;
			rect.X = (int)ImgCol1;
			rect.Y = (int)ImgRow1;
			rect.Width = (int)imageWidth;
			rect.Height = (int)imageHeight;
			viewPort.ImagePart = rect;


			if (roiManager != null)
				roiManager.reset();
		}

		public void resetWindow()
		{
			ImgRow1 = 0;
			ImgCol1 = 0;
			ImgRow2 = imageHeight;
			ImgCol2 = imageWidth;

			zoomWndFactor = (double)imageWidth / viewPort.Width;

			System.Drawing.Rectangle rect = viewPort.ImagePart;
			rect.X = (int)ImgCol1;
			rect.Y = (int)ImgRow1;
			rect.Width = (int)imageWidth;
			rect.Height = (int)imageHeight;
			viewPort.ImagePart = rect;
		}


		/*************************************************************************/
		/*      			 Event handling for mouse	   	                     */
		/*************************************************************************/
		private void mouseDown(object sender, HalconDotNet.HMouseEventArgs e)
		{
            //lock (mmlocker)
            //{
                mousePressed = true;
                int activeROIidx = -1;
                double scale;

                if (roiManager != null && (dispROI == MODE_INCLUDE_ROI))
                {
                    activeROIidx = roiManager.mouseDownAction(e.X, e.Y);
                }

                if (activeROIidx == -1)
                {
                    switch (stateView)
                    {
                        case MODE_VIEW_MOVE:
                            startX = e.X;
                            startY = e.Y;
                            break;
                        case MODE_VIEW_ZOOM:
                            if (e.Button == System.Windows.Forms.MouseButtons.Left)
                                scale = 0.9;
                            else
                                scale = 1 / 0.9;

                            zoomImage(e.X, e.Y, scale);


                            break;
                        case MODE_VIEW_NONE:
                            break;
                        case MODE_VIEW_ZOOMWINDOW:
                            activateZoomWindow((int)e.X, (int)e.Y);
                            break;
                        default:
                            break;
                    }
                }

                //if ((myCircle.Count > 0) && SamplingMode)
                //{
                //    int myIdx = GetCircleIdx((int)e.X, (int)e.Y);
                //    if (myIdx != -1)
                //    {
                //        myCircle[myIdx].Selected = !myCircle[myIdx].Selected;
                //        repaint();
                //    }

                //}
            //}
			//end of if
		}
        //private int GetCircleIdx(int X, int Y)
        //{            
        //    for (int i = 0; i < myCircle.Count; i++)
        //    {
        //        if ((X > (myCircle[i].centerX - myCircle[i].radius)) &&
        //            (X < (myCircle[i].centerX + myCircle[i].radius)) &&
        //            (Y > (myCircle[i].centerY - myCircle[i].radius)) &&
        //            (Y < (myCircle[i].centerY + myCircle[i].radius)))
        //        {
        //            return i;
        //        }
        //    }
        //    return -1;
        //}

		/*******************************************************************/
		private void activateZoomWindow(int X, int Y)
		{
			double posX, posY;
			int zoomZone;

			if (ZoomWindow != null)
				ZoomWindow.Dispose();

			HOperatorSet.SetSystem("border_width", 10);
			ZoomWindow = new HWindow();

			posX = ((X - ImgCol1) / (ImgCol2 - ImgCol1)) * viewPort.Width;
			posY = ((Y - ImgRow1) / (ImgRow2 - ImgRow1)) * viewPort.Height;

			zoomZone = (int)((zoomWndSize / 2) * zoomWndFactor * zoomAddOn);
			ZoomWindow.OpenWindow((int)posY - (zoomWndSize / 2), (int)posX - (zoomWndSize / 2),
								   zoomWndSize, zoomWndSize,
								   viewPort.HalconID, "visible", "");
			ZoomWindow.SetPart(Y - zoomZone, X - zoomZone, Y + zoomZone, X + zoomZone);
			repaint(ZoomWindow);
			ZoomWindow.SetColor("black");
		}

		/*******************************************************************/
		private void mouseUp(object sender, HalconDotNet.HMouseEventArgs e)
		{
			mousePressed = false;

			if (roiManager != null
				&& (roiManager.activeROIidx != -1)
				&& (dispROI == MODE_INCLUDE_ROI))
			{
				roiManager.NotifyRCObserver(ROIController.EVENT_UPDATE_ROI);
			}
			else if (stateView == MODE_VIEW_ZOOMWINDOW)
			{
				ZoomWindow.Dispose();
			}
		}

		/*******************************************************************/
		private void mouseMoved(object sender, HalconDotNet.HMouseEventArgs e)
		{
            //lock (mmlocker)
            //{
                double motionX, motionY;
                double posX, posY;
                double zoomZone;
           
                if (!mousePressed)
                    return;

                if (roiManager != null && (roiManager.activeROIidx != -1) && (dispROI == MODE_INCLUDE_ROI))
                {
                    roiManager.mouseMoveAction(e.X, e.Y);
                }
                else if (stateView == MODE_VIEW_MOVE)
                {
                    motionX = ((e.X - startX));
                    motionY = ((e.Y - startY));

                    if (((int)motionX != 0) || ((int)motionY != 0))
                    {
                        moveImage(motionX, motionY);
                        startX = e.X - motionX;
                        startY = e.Y - motionY;
                    }
                }
                else if (stateView == MODE_VIEW_ZOOMWINDOW)
                {
                    HSystem.SetSystem("flush_graphic", "false");
                    ZoomWindow.ClearWindow();


                    posX = ((e.X - ImgCol1) / (ImgCol2 - ImgCol1)) * viewPort.Width;
                    posY = ((e.Y - ImgRow1) / (ImgRow2 - ImgRow1)) * viewPort.Height;
                    zoomZone = (zoomWndSize / 2) * zoomWndFactor * zoomAddOn;

                    ZoomWindow.SetWindowExtents((int)posY - (zoomWndSize / 2),
                                                (int)posX - (zoomWndSize / 2),
                                                zoomWndSize, zoomWndSize);
                    ZoomWindow.SetPart((int)(e.Y - zoomZone), (int)(e.X - zoomZone),
                                       (int)(e.Y + zoomZone), (int)(e.X + zoomZone));
                    repaint(ZoomWindow);

                    HSystem.SetSystem("flush_graphic", "true");
                    ZoomWindow.DispLine(-100.0, -100.0, -100.0, -100.0);
            }
			
            //}
		}

		/// <summary>
		/// To initialize the move function using a GUI component, the HWndCtrl
		/// first needs to know the range supplied by the GUI component. 
		/// For the x direction it is specified by xRange, which is 
		/// calculated as follows: GuiComponentX.Max()-GuiComponentX.Min().
		/// The starting value of the GUI component has to be supplied 
		/// by the parameter Init
		/// </summary>
		public void setGUICompRangeX(int[] xRange, int Init)
		{
			int cRangeX;

			CompRangeX = xRange;
			cRangeX = xRange[1] - xRange[0];
			prevCompX = Init;
			stepSizeX = ((double)imageWidth / cRangeX) * (imageWidth / windowWidth);

		}

		/// <summary>
		/// To initialize the move function using a GUI component, the HWndCtrl
		/// first needs to know the range supplied by the GUI component. 
		/// For the y direction it is specified by yRange, which is 
		/// calculated as follows: GuiComponentY.Max()-GuiComponentY.Min().
		/// The starting value of the GUI component has to be supplied 
		/// by the parameter Init
		/// </summary>
		public void setGUICompRangeY(int[] yRange, int Init)
		{
			int cRangeY;

			CompRangeY = yRange;
			cRangeY = yRange[1] - yRange[0];
			prevCompY = Init;
			stepSizeY = ((double)imageHeight / cRangeY) * (imageHeight / windowHeight);
		}


		/// <summary>
		/// Resets to the starting value of the GUI component.
		/// </summary>
		public void resetGUIInitValues(int xVal, int yVal)
		{
			prevCompX = xVal;
			prevCompY = yVal;
		}

		/// <summary>
		/// Moves the image by the value valX supplied by the GUI component
		/// </summary>
		public void moveXByGUIHandle(int valX)
		{
			double motionX;

			motionX = (valX - prevCompX) * stepSizeX;

			if (motionX == 0)
				return;

			moveImage(motionX, 0.0);
			prevCompX = valX;
		}


		/// <summary>
		/// Moves the image by the value valY supplied by the GUI component
		/// </summary>
		public void moveYByGUIHandle(int valY)
		{
			double motionY;

			motionY = (valY - prevCompY) * stepSizeY;

			if (motionY == 0)
				return;

			moveImage(0.0, motionY);
			prevCompY = valY;
		}

		/// <summary>
		/// Zooms the image by the value valF supplied by the GUI component
		/// </summary>
		public void zoomByGUIHandle(double valF)
		{
			double x, y, scale;
			double prevScaleC;



			x = (ImgCol1 + (ImgCol2 - ImgCol1) / 2);
			y = (ImgRow1 + (ImgRow2 - ImgRow1) / 2);

			prevScaleC = (double)((ImgCol2 - ImgCol1) / imageWidth);
			scale = ((double)1.0 / prevScaleC * (100.0 / valF));

			zoomImage(x, y, scale);
		}


        public void disp_message(HTuple hv_WindowHandle, HTuple hv_String, HTuple hv_CoordSystem,
    HTuple hv_Row, HTuple hv_Column, HTuple hv_Color, HTuple hv_Box)
        {


            // Local control variables 

            HTuple hv_Red, hv_Green, hv_Blue, hv_Row1Part;
            HTuple hv_Column1Part, hv_Row2Part, hv_Column2Part, hv_RowWin;
            HTuple hv_ColumnWin, hv_WidthWin, hv_HeightWin, hv_MaxAscent;
            HTuple hv_MaxDescent, hv_MaxWidth, hv_MaxHeight, hv_R1 = new HTuple();
            HTuple hv_C1 = new HTuple(), hv_FactorRow = new HTuple(), hv_FactorColumn = new HTuple();
            HTuple hv_Width = new HTuple(), hv_Index = new HTuple(), hv_Ascent = new HTuple();
            HTuple hv_Descent = new HTuple(), hv_W = new HTuple(), hv_H = new HTuple();
            HTuple hv_FrameHeight = new HTuple(), hv_FrameWidth = new HTuple();
            HTuple hv_R2 = new HTuple(), hv_C2 = new HTuple(), hv_DrawMode = new HTuple();
            HTuple hv_Exception = new HTuple(), hv_CurrentColor = new HTuple();

            HTuple hv_Color_COPY_INP_TMP = hv_Color.Clone();
            HTuple hv_Column_COPY_INP_TMP = hv_Column.Clone();
            HTuple hv_Row_COPY_INP_TMP = hv_Row.Clone();
            HTuple hv_String_COPY_INP_TMP = hv_String.Clone();

            // Initialize local and output iconic variables 

            //This procedure displays text in a graphics window.
            //
            //Input parameters:
            //WindowHandle: The WindowHandle of the graphics window, where
            //   the message should be displayed
            //String: A tuple of strings containing the text message to be displayed
            //CoordSystem: If set to 'window', the text position is given
            //   with respect to the window coordinate system.
            //   If set to 'image', image coordinates are used.
            //   (This may be useful in zoomed images.)
            //Row: The row coordinate of the desired text position
            //   If set to -1, a default value of 12 is used.
            //Column: The column coordinate of the desired text position
            //   If set to -1, a default value of 12 is used.
            //Color: defines the color of the text as string.
            //   If set to [], '' or 'auto' the currently set color is used.
            //   If a tuple of strings is passed, the colors are used cyclically
            //   for each new textline.
            //Box: If set to 'true', the text is written within a white box.
            //
            //prepare window
            HOperatorSet.GetRgb(hv_WindowHandle, out hv_Red, out hv_Green, out hv_Blue);
            HOperatorSet.GetPart(hv_WindowHandle, out hv_Row1Part, out hv_Column1Part, out hv_Row2Part,
                out hv_Column2Part);
            HOperatorSet.GetWindowExtents(hv_WindowHandle, out hv_RowWin, out hv_ColumnWin,
                out hv_WidthWin, out hv_HeightWin);
            HOperatorSet.SetPart(hv_WindowHandle, 0, 0, hv_HeightWin - 1, hv_WidthWin - 1);
            //
            //default settings
            if ((int)(new HTuple(hv_Row_COPY_INP_TMP.TupleEqual(-1))) != 0)
            {
                hv_Row_COPY_INP_TMP = 12;
            }
            if ((int)(new HTuple(hv_Column_COPY_INP_TMP.TupleEqual(-1))) != 0)
            {
                hv_Column_COPY_INP_TMP = 12;
            }
            if ((int)(new HTuple(hv_Color_COPY_INP_TMP.TupleEqual(new HTuple()))) != 0)
            {
                hv_Color_COPY_INP_TMP = "";
            }
            //
            hv_String_COPY_INP_TMP = ((("" + hv_String_COPY_INP_TMP) + "")).TupleSplit("\n");
            //
            //Estimate extentions of text depending on font size.
            HOperatorSet.GetFontExtents(hv_WindowHandle, out hv_MaxAscent, out hv_MaxDescent,
                out hv_MaxWidth, out hv_MaxHeight);
            if ((int)(new HTuple(hv_CoordSystem.TupleEqual("window"))) != 0)
            {
                hv_R1 = hv_Row_COPY_INP_TMP.Clone();
                hv_C1 = hv_Column_COPY_INP_TMP.Clone();
            }
            else
            {
                //transform image to window coordinates
                hv_FactorRow = (1.0 * hv_HeightWin) / ((hv_Row2Part - hv_Row1Part) + 1);
                hv_FactorColumn = (1.0 * hv_WidthWin) / ((hv_Column2Part - hv_Column1Part) + 1);
                hv_R1 = ((hv_Row_COPY_INP_TMP - hv_Row1Part) + 0.5) * hv_FactorRow;
                hv_C1 = ((hv_Column_COPY_INP_TMP - hv_Column1Part) + 0.5) * hv_FactorColumn;
            }
            //
            //display text box depending on text size
            if ((int)(new HTuple(hv_Box.TupleEqual("true"))) != 0)
            {
                //calculate box extents
                hv_String_COPY_INP_TMP = (" " + hv_String_COPY_INP_TMP) + " ";
                hv_Width = new HTuple();
                for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_String_COPY_INP_TMP.TupleLength()
                    )) - 1); hv_Index = (int)hv_Index + 1)
                {
                    HOperatorSet.GetStringExtents(hv_WindowHandle, hv_String_COPY_INP_TMP.TupleSelect(
                        hv_Index), out hv_Ascent, out hv_Descent, out hv_W, out hv_H);
                    hv_Width = hv_Width.TupleConcat(hv_W);
                }
                hv_FrameHeight = hv_MaxHeight * (new HTuple(hv_String_COPY_INP_TMP.TupleLength()
                    ));
                hv_FrameWidth = (((new HTuple(0)).TupleConcat(hv_Width))).TupleMax();
                hv_R2 = hv_R1 + hv_FrameHeight;
                hv_C2 = hv_C1 + hv_FrameWidth;
                //display rectangles
                HOperatorSet.GetDraw(hv_WindowHandle, out hv_DrawMode);
                HOperatorSet.SetDraw(hv_WindowHandle, "fill");
                HOperatorSet.SetColor(hv_WindowHandle, "light gray");
                HOperatorSet.DispRectangle1(hv_WindowHandle, hv_R1 + 3, hv_C1 + 3, hv_R2 + 3, hv_C2 + 3);
                HOperatorSet.SetColor(hv_WindowHandle, "white");
                HOperatorSet.DispRectangle1(hv_WindowHandle, hv_R1, hv_C1, hv_R2, hv_C2);
                HOperatorSet.SetDraw(hv_WindowHandle, hv_DrawMode);
            }
            else if ((int)(new HTuple(hv_Box.TupleNotEqual("false"))) != 0)
            {
                hv_Exception = "Wrong value of control parameter Box";
                throw new HalconException(hv_Exception);
            }
            //Write text.
            for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_String_COPY_INP_TMP.TupleLength()
                )) - 1); hv_Index = (int)hv_Index + 1)
            {
                hv_CurrentColor = hv_Color_COPY_INP_TMP.TupleSelect(hv_Index % (new HTuple(hv_Color_COPY_INP_TMP.TupleLength()
                    )));
                if ((int)((new HTuple(hv_CurrentColor.TupleNotEqual(""))).TupleAnd(new HTuple(hv_CurrentColor.TupleNotEqual(
                    "auto")))) != 0)
                {
                    HOperatorSet.SetColor(hv_WindowHandle, hv_CurrentColor);
                }
                else
                {
                    HOperatorSet.SetRgb(hv_WindowHandle, hv_Red, hv_Green, hv_Blue);
                }
                hv_Row_COPY_INP_TMP = hv_R1 + (hv_MaxHeight * hv_Index);
                HOperatorSet.SetTposition(hv_WindowHandle, hv_Row_COPY_INP_TMP, hv_C1);
                HOperatorSet.WriteString(hv_WindowHandle, hv_String_COPY_INP_TMP.TupleSelect(
                    hv_Index));
            }
            //reset changed window settings
            HOperatorSet.SetRgb(hv_WindowHandle, hv_Red, hv_Green, hv_Blue);
            HOperatorSet.SetPart(hv_WindowHandle, hv_Row1Part, hv_Column1Part, hv_Row2Part,
                hv_Column2Part);

            return;
        }
        /// <summary>
        /// Repaints the HALCON window 'window'
        /// </summary>
        ///

        public void SaveInspectedImage(string pathname)
        {
            //lock (lockershow)
            //{
                HObject ho_ImageOut = new HObject();
                HOperatorSet.DumpWindowImage(out ho_ImageOut, viewPort.HalconWindow);
                HOperatorSet.WriteImage(ho_ImageOut, "bmp", 0, pathname);
                ho_ImageOut.Dispose();
            //}
           
        }

		/// <summary>
		/// Triggers a repaint of the HALCON window
		/// </summary>
		public void repaint()
		{            
			repaint(viewPort.HalconWindow);
            viewPort.Refresh();
		}

		/// <summary>
		/// Repaints the HALCON window 'window'
		/// </summary>
		public void repaint(HalconDotNet.HWindow window)
		{
            lock (lockershow)
            {
                int count = HObjList.Count;
                //HObjectEntry entry;

                HSystem.SetSystem("flush_graphic", "false");
                window.ClearWindow();
                mGC.stateOfSettings.Clear();

                //for (int i = 0; i < count; i++)
                //{
                //    entry = ((HObjectEntry)HObjList[i]);
                //    mGC.applyContext(window, entry.gContext);
                //    window.DispObj(entry.HObj);
                //}
                if (entry != null)
                {
                    try
                    {
                        mGC.applyContext(window, entry.gContext);
                        window.DispObj(entry.HObj);                        
                    }
                    catch (Exception ex)
                    { }
                }
                addInfoDelegate();

                if (roiManager != null && (dispROI == MODE_INCLUDE_ROI))
                    roiManager.paintData(window);

                HSystem.SetSystem("flush_graphic", "true");

                if (myRect.Count != 0)
                {
                    window.SetColor("green");
                    window.SetLineWidth(2);
                    HObject ho_Rect = new HObject();
                                        
                    for (int i = 0; i < myRect.Count; i++)
                    {
                        if (myRect[i].isCircle)
                        {
                            window.SetColor("green");
                            window.SetLineWidth(2);
                            if (myRect[i].Found)
                            {
                                HOperatorSet.GenCircleContourXld(out ho_Rect, myRect[i].Y, myRect[i].X, myRect[i].Radius, 0, 6.28318, "positive", 1.0);
                                window.DispObj(ho_Rect);
                            }
                            //window.DispCircle(myRect[i].Y, myRect[i].X, myRect[i].Radius);
                            window.DispCross(myRect[i].Y, myRect[i].X, 50, 0);

                            disp_message(window, "X:" + myRect[i].X.ToString("F1") + ", Y:" + myRect[i].Y.ToString("F1")+", Means:" + myRect[i].Means.ToString("F1"), "image", 100, 50, "green", "false");
                            disp_message(window, "Radius:" + myRect[i].Radius.ToString("F1") +",Angle:"+Helper.GetDegreeFromRadian((float)myRect[i].Angle).ToString("F1"), "image", 300, 50, "green", "false");                            
                        }
                        else
                        {
                            
                            window.SetColor("green");
                            window.SetLineWidth(2);
                            if (myRect[i].Found)
                            {
                                HOperatorSet.GenRectangle2ContourXld(out ho_Rect, myRect[i].Y, myRect[i].X, myRect[i].Angle, myRect[i].Width / 2, myRect[i].Height / 2);
                                window.DispObj(ho_Rect);
                            }
                            //if (myRect[i].Found)
                            //{
                                //window.SetColor("cyan");
                                //window.DispObj(myRect[i].rect_border);
                            //}
                            window.DispCross(myRect[i].Y, myRect[i].X, 50, 0);

                            disp_message(window, "X:" + myRect[i].X.ToString("F1") + ", Y:" + myRect[i].Y.ToString("F1") + ", Means:" + myRect[i].Means.ToString("F1"), "image", 100, 50, "green", "false");
                            disp_message(window, "Width:" + myRect[i].Width.ToString("F1") + ", Height:" + myRect[i].Height.ToString("F1"), "image", 300, 50, "green", "false");
                            //disp_message(window, "Means:" + myRect[i].Means.ToString("F1"), "image", 500, 50, "green", "false");
                        }
                    }
                    
                    //for (int i = 0; i < myRect.Count; i++)
                    //    window.DispRectangle2(myRect[i].Y, myRect[i].X, myRect[i].Angle, myRect[i].Width / 2, myRect[i].Length / 2);
                }

                if (bShowCrossHair)
                {
                    window.SetColor("red");
                    window.SetLineWidth(2);
                    int winrow, winclom, winwidth, winHeight;
                    window.GetPart(out winrow, out winclom, out winwidth, out winHeight);
                    window.DispLine(winrow + (winwidth - winrow) / 2, (double)winclom, winrow + (winwidth - winrow) / 2, winHeight);
                    window.DispLine((double)winrow, winclom + (winHeight - winclom) / 2, winwidth, winclom + (winHeight - winclom) / 2);
                }
                                
                if (IsDrawCross1)
                {
                    window.DispCross(200.0, 200.0, 40, 0);
                    //window.WriteString("标定1");
                }
                if (IsDrawCross2)
                {
                    //window.DispCross(200.0, 200.0, 2, 0);
                    window.DispCross(600.0, 600.0, 40, 0);
                   // window.WriteString("标定2");
                }                  
            }

        }

        public void InspectedImage(ref HObject ho_ImageOut)
        {
            HOperatorSet.DumpWindowImage(out ho_ImageOut, viewPort.HalconWindow);
            //HOperatorSet.WriteImage(ho_ImageOut, "bmp", 0, pathname);
        }

        public void DrawRect(JPTCG.Vision.HalconInspection.RectData myRes, HObject obj)
         {
             myRect.Add(myRes);
             addIconicVar(obj);
             //repaint();
         }


		/********************************************************************/
		/*                      GRAPHICSSTACK                               */
		/********************************************************************/

		/// <summary>
		/// Adds an iconic object to the graphics stack similar to the way
		/// it is defined for the HDevelop graphics stack.
		/// </summary>
		/// <param name="obj">Iconic object</param>
         /// 
        
        HObjectEntry entry;
        HImage tempImage = new HImage();
		public void addIconicVar(HObject obj)
		{		
			if (obj == null)
				return;

            //if (entry != null)
            //viewPort.HalconWindow.DispObj(entry.HObj);

          //  HRegion ddd = new HRegion();
            tempImage.Dispose();
            tempImage = new HImage(obj);

            if (tempImage != null)
            //if (obj is HImage)
			{                

				double r, c;
				int h, w, area;
				string s;
                area = tempImage.GetDomain().AreaCenter(out r, out c);
                tempImage.GetImagePointer1(out s, out w, out h);

                //area = ((HImage)obj).GetDomain().AreaCenter(out r, out c);
                //((HImage)obj).GetImagePointer1(out s, out w, out h);

                if (area == (w * h))
                {
					clearList();

					if ((h != imageHeight) || (w != imageWidth))
					{
						imageHeight = h;
						imageWidth = w;
						zoomWndFactor = (double)imageWidth / viewPort.Width;
						setImagePart(0, 0, h, w);
					}
                }//if
			}//if

            //entry.
            entry = new HObjectEntry(tempImage, mGC.copyContextList());

			//HObjList.Add(entry);

            //if (HObjList.Count > MAXNUMOBJLIST)
            //    HObjList.RemoveAt(1);

            repaint();
		}


		/// <summary>
		/// Clears all entries from the graphics stack 
		/// </summary>
		public void clearList()
		{
			HObjList.Clear();
		}

		/// <summary>
		/// Returns the number of items on the graphics stack
		/// </summary>
		public int getListCount()
		{
			return HObjList.Count;
		}

		/// <summary>
		/// Changes the current graphical context by setting the specified mode
		/// (constant starting by GC_*) to the specified value.
		/// </summary>
		/// <param name="mode">
		/// Constant that is provided by the class GraphicsContext
		/// and describes the mode that has to be changed, 
		/// e.g., GraphicsContext.GC_COLOR
		/// </param>
		/// <param name="val">
		/// Value, provided as a string, 
		/// the mode is to be changed to, e.g., "blue" 
		/// </param>
		public void changeGraphicSettings(string mode, string val)
		{
			switch (mode)
			{
				case GraphicsContext.GC_COLOR:
					mGC.setColorAttribute(val);
					break;
				case GraphicsContext.GC_DRAWMODE:
					mGC.setDrawModeAttribute(val);
					break;
				case GraphicsContext.GC_LUT:
					mGC.setLutAttribute(val);
					break;
				case GraphicsContext.GC_PAINT:
					mGC.setPaintAttribute(val);
					break;
				case GraphicsContext.GC_SHAPE:
					mGC.setShapeAttribute(val);
					break;
				default:
					break;
			}
		}

		/// <summary>
		/// Changes the current graphical context by setting the specified mode
		/// (constant starting by GC_*) to the specified value.
		/// </summary>
		/// <param name="mode">
		/// Constant that is provided by the class GraphicsContext
		/// and describes the mode that has to be changed, 
		/// e.g., GraphicsContext.GC_LINEWIDTH
		/// </param>
		/// <param name="val">
		/// Value, provided as an integer, the mode is to be changed to, 
		/// e.g., 5 
		/// </param>
		public void changeGraphicSettings(string mode, int val)
		{
			switch (mode)
			{
				case GraphicsContext.GC_COLORED:
					mGC.setColoredAttribute(val);
					break;
				case GraphicsContext.GC_LINEWIDTH:
					mGC.setLineWidthAttribute(val);
					break;
				default:
					break;
			}
		}

		/// <summary>
		/// Changes the current graphical context by setting the specified mode
		/// (constant starting by GC_*) to the specified value.
		/// </summary>
		/// <param name="mode">
		/// Constant that is provided by the class GraphicsContext
		/// and describes the mode that has to be changed, 
		/// e.g.,  GraphicsContext.GC_LINESTYLE
		/// </param>
		/// <param name="val">
		/// Value, provided as an HTuple instance, the mode is 
		/// to be changed to, e.g., new HTuple(new int[]{2,2})
		/// </param>
		public void changeGraphicSettings(string mode, HTuple val)
		{
			switch (mode)
			{
				case GraphicsContext.GC_LINESTYLE:
					mGC.setLineStyleAttribute(val);
					break;
				default:
					break;
			}
		}

		/// <summary>
		/// Clears all entries from the graphical context list
		/// </summary>
		public void clearGraphicContext()
		{
			mGC.clear();
		}

		/// <summary>
		/// Returns a clone of the graphical context list (hashtable)
		/// </summary>
		public Hashtable getGraphicContext()
		{
			return mGC.copyContextList();
		}

	}//end of class
}//end of namespace
