/*
 * Created by SharpDevelop.
 * User: Crogogo
 * Date: 7/17/2020
 * Time: 7:04 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using ObjParser.Types;
using ObjParser;


namespace TwoDThreeD
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		private System.ComponentModel.BackgroundWorker backgroundWorker1;
		public static int picwidth = 0;
		public static int picheight = 0;
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			InitializeBackgroundWorker();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		// Set up the BackgroundWorker object by 
        // attaching event handlers. 
        private void InitializeBackgroundWorker()
        {
        	this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
        	this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);
            backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker1_RunWorkerCompleted);
            backgroundWorker1.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker1_ProgressChanged);
        }
        internal double GetAngleFromPoint(Point point, Point centerPoint)
	{
	    double dy = (point.Y - centerPoint.Y);
	    double dx = (point.X - centerPoint.X);
	
	    double theta = Math.Atan2(dy,dx);
	
	    double angle = (90 - ((theta * 180) / Math.PI)) % 360;
	
	    return angle;
	}
              

        // This event handler is where the actual,
        // potentially time-consuming work is done.
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {   
            
            BackgroundWorker worker = sender as BackgroundWorker;// Get the BackgroundWorker that raised this event.
            Object[] something = (Object[])e.Argument;
            Obj modeling = something[0] as Obj;
            string modelpath = (string)something[1];
            modeling.LoadObj(modelpath);

            //e.Result = ComputeFibonacci((Object[])e.Argument, worker, e);
            e.Result = Compute3dto2d(modeling, worker, e);
        }
        object Compute3dto2d(Obj n, System.ComponentModel.BackgroundWorker worker, DoWorkEventArgs e)
        {
            //if ((n < 0) || (n > 91))            {                throw new ArgumentException("value must be >= 0 and <= 91", "n");            }// The parameter n must be >= 0 and <= 91. Fib(n), with n > 91, overflows a long.
            Object results = new Object();
            double theta = 20.0;
            
            double hw = picwidth / 2;
            double hh = picheight / 2;
            double fl_top = hw / Math.Tan(theta/2);
            double fl_side = hh / Math.Tan(theta/2);
            double fl_average = (fl_top + fl_side) / 2;
            double fov = 1.0 / Math.Tan(90/2.0);
            
            // Abort the operation if the user has canceled. Note that a call to CancelAsync may have set CancellationPending to true just after the last invocation of this method exits, so this  code will not have the opportunity to set the  DoWorkEventArgs. Cancel flag to true. This means that RunWorkerCompletedEventArgs.Cancelled will not be set to true in your RunWorkerCompleted event handler. This is a race condition.
            if (worker.CancellationPending)            {                   e.Cancel = true;            }
            else
            {   
                //if (n < 2)                {                       result = 1;                }
                //else                {                       result = ComputeFibonacci(n - 1, worker, e) + ComputeFibonacci(n - 2, worker, e);                }
                //int percentComplete = (int)((float)n / (float)numberToCompute * 100);// Report progress as a percentage of the total task.
                //if (percentComplete > highestPercentageReached)                {                    highestPercentageReached = percentComplete;                    worker.ReportProgress(percentComplete);                }
                //Object[] something = (Object[])n;
                //	string modelpath = (string)something[1];
                //	Obj modelObject =(n[0] as Obj);
                //modelObject.LoadObj(modelpath);
                //percentComplete++;
                //worker.ReportProgress((int)modelObject.Size.XMax);
                //results = (Object)modelObject;
               
                //double PERSPECTIVE = picwidth * 0.8;
                double PERSPECTIVE = picwidth * 0.8;
            double PROJECTION_CENTER_X = picwidth / 2; // x center of the canvas
			double PROJECTION_CENTER_Y = picheight / 2; // y center of the canvas
			double max = n.VertexList.Count;
			
			double xProjected = 0; // x coordinate on the 2D world
    double yProjected = 0; // y coordinate on the 2D world
    
    double radius = 20;
			
                for(int i = 0; i < n.VertexList.Count; i++)
                {
                	double percentComplete  = ((i+1)/max)*100 ;
			
                	double scaleProjected = 0; // Scale of the element on the 2D world (further = smaller)
                	//Console.WriteLine( n.VertexList[i].X + " " + n.VertexList[i].Y + " " + n.VertexList[i].Z);
                	scaleProjected = (PERSPECTIVE / (PERSPECTIVE + n.VertexList[i].Z));
                	
                	// The xProjected is the x position on the 2D world
    xProjected = (n.VertexList[i].X * scaleProjected) + PROJECTION_CENTER_X;
    // The yProjected is the y position on the 2D world
    yProjected = (n.VertexList[i].Y * scaleProjected) + PROJECTION_CENTER_Y;
    double globalalpha = Math.Abs(1 - n.VertexList[i].Z / picwidth);
                	//ctx.fillRect(this.xProjected - this.radius, this.yProjected - this.radius, this.radius * 2 * this.scaleProjected, this.radius * 2 * this.scaleProjected);
                	object[] pixelz = { Convert.ToInt32(xProjected - radius), Convert.ToInt32(yProjected - radius), radius * 2 * scaleProjected, radius * 2 * scaleProjected, globalalpha };
                	worker.ReportProgress(Convert.ToInt32(percentComplete), (object)pixelz);
                	((Bitmap)pictureBox1.Image).SetPixel( Convert.ToInt32(xProjected - radius),  Convert.ToInt32(yProjected - radius), System.Drawing.Color.Black);
        	//pictureBox1.Refresh();
                	//worker.ReportProgress(i, (object)n.VertexList[i]);
                }
                Graphics g = Graphics.FromImage((Bitmap)pictureBox1.Image);
                //int index1 = 0;
                //int index2 = 0;
                //int index3 = 0;
//                for(int i = 0; i < n.FaceList.Count; i++)
//                {
//                	
//
//                	try {
//                		if(n.FaceList[i].VertexIndexList.Length >= 3 )
//                	{
//                		double scaleProjected = 0;
//                	index1 = n.FaceList[i].VertexIndexList[0];
//                	scaleProjected = (PERSPECTIVE / (PERSPECTIVE + n.VertexList[index1].Z));
//                	Point point1 = new Point(
//                		Convert.ToInt32((n.VertexList[index1].X * scaleProjected) + PROJECTION_CENTER_X),Convert.ToInt32((n.VertexList[index1].Y * scaleProjected) + PROJECTION_CENTER_Y)
//                	);
//                	index2 = n.FaceList[i].VertexIndexList[1];
//                	scaleProjected = (PERSPECTIVE / (PERSPECTIVE + n.VertexList[index2].Z));
//                	Point point2 = new Point(
//                		Convert.ToInt32((n.VertexList[index2].X * scaleProjected) + PROJECTION_CENTER_X),Convert.ToInt32((n.VertexList[index2].Y * scaleProjected) + PROJECTION_CENTER_Y)
//                	);
//                	index3 = n.FaceList[i].VertexIndexList[2];
//                	scaleProjected = (PERSPECTIVE / (PERSPECTIVE + n.VertexList[index3].Z));
//                	Point point3 = new Point(
//                		Convert.ToInt32((n.VertexList[index3].X * scaleProjected) + PROJECTION_CENTER_X),Convert.ToInt32((n.VertexList[index3].Y * scaleProjected) + PROJECTION_CENTER_Y)
//                	);
//                	}
//                		
//                	} catch (Exception eee) {
//                		
//                		MessageBox.Show(eee.Message);
//
//                	}
//     
//                	
//                	   
//                	//n.VertexList[]
//                	//n.VertexList[[n.FaceList[i].VertexIndexList[i]].
//                	/*
//                	GraphicsPath gPath = new GraphicsPath();
//					Rectangle rect = new Rectangle(0, 0, 500, 250);
//					gPath.AddRectangle(rect);
//					PathGradientBrush pathGradientBrush = new PathGradientBrush(gPath);
//					
////
////                	using (var p = new GraphicsPath())
////        			{
////            			p.AddPolygon(new Point[] {
////                	//new Point(this.Width / 2, 0), 
////                	///new Point(0, Height), 
////                	//new Point(Width, Height) 
////                	point1, point2, point3
////                	});
////                		this.Region = new Region(p);
////            //base.OnPaint(pe);
////        			}
//
//					pathGradientBrush.CenterColor = System.Drawing.Color.DeepPink;
//					
//					System.Drawing.Color[] colors = { System.Drawing.Color.FromArgb(196, 128, 128, 128) };
//					pathGradientBrush.SurroundColors = colors;
//					        		
//					g.FillPolygon(pathGradientBrush, new Point[] { point1, point2, point3 });
//					*/
//					          		
//                }
               
               
                results = (" Xmin= "+ n.Size.XMin.ToString() + " XMax= " + n.Size.XMax + " Ymin= " + n.Size.YMin + " Ymin= " + n.Size.YMax.ToString() + " Zmin= " + n.Size.ZMin + " ZMax= " + n.Size.ZMax + " ") as object;
                
                
            }
            return results;
        }

        // This event handler deals with the results of the
        // background operation.
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // First, handle the case where an exception was thrown.
            if (e.Error != null) 	{                MessageBox.Show(e.Error.Message);            }
            else if (e.Cancelled) 	{ resultLabel.Text = "Canceled"; } // Next, handle the case where the user canceled the operation. Note that due to a race condition in the DoWork event handler, the Cancelled flag may not have been set, even though CancelAsync was called.
            else { resultLabel.Text = e.Result as String; }// Finally, handle the case where the operation succeeded.
            button1.Enabled = true; // Enable the Start button.
            cancelAsyncButton.Enabled = false; // Disable the Cancel button.
            pictureBox1.Refresh();
        }

        // This event handler updates the progress bar.
        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)        
        {   
        	object[] results = (object[])e.UserState;
        	//double x = (double)results[0];
        	//double y = (double)results[1];
        	int x = (int)results[0];
        	int y = (int)results[1];
        	this.textBox1.Text = e.ProgressPercentage.ToString();
        	
        	this.progressBar1.Value = e.ProgressPercentage;        
        	//Vertex curVer = ((Vertex)(e.UserState));
        	//richTextBox1.Text = richTextBox1.Text + e.ProgressPercentage.ToString() + " " + curVer.X + " " + curVer.Y + " " + curVer.Z + " \n";
        	//richTextBox1.Text = richTextBox1.Text + e.ProgressPercentage.ToString() + " " + (double)results[2] + " " + (double)results[3] + " " + (double)results[4] + "\n";
        	//if(x < pictureBox1.Width && y < pictureBox1.Height && x >= 0 && y >= 0)        	{        		((Bitmap)pictureBox1.Image).SetPixel( Convert.ToInt32(x),  Convert.ToInt32(y), System.Drawing.Color.Black);        	}
        	//((Bitmap)pictureBox1.Image).SetPixel( Convert.ToInt32(x),  Convert.ToInt32(y), System.Drawing.Color.Black);
        	//richTextBox1.Text = richTextBox1.Text + "\n" + e.ProgressPercentage; 
        	//((Bitmap)pictureBox1.Image).SetPixel( x,  y, System.Drawing.Color.Black);
        	//pictureBox1.Refresh();
        	
        	
        }
		void Button1Click(object sender, EventArgs e)
		{
			picwidth = pictureBox1.Width;
			picheight = pictureBox1.Height;
			resultLabel.Text = String.Empty;
			this.button1.Enabled = false;
            this.cancelAsyncButton.Enabled = true;
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            //panel1.Pa
            

            
            var fileContent = string.Empty;
			var filePath = string.Empty;
			
			using (OpenFileDialog openFileDialog = new OpenFileDialog())
			{
			    openFileDialog.InitialDirectory = "C:\\";
			    openFileDialog.Filter = "obj files (*.obj)|*.obj|All files (*.*)|*.*";
			    openFileDialog.FilterIndex = 2;
			    openFileDialog.RestoreDirectory = true;
			
			    if (openFileDialog.ShowDialog() == DialogResult.OK)
			    {
			        //Get the path of specified file
			        filePath = openFileDialog.FileName;
			
			        //Read the contents of the file into a stream
			        var fileStream = openFileDialog.OpenFile();
			        Obj model = new Obj();
			        object[] casting = {model, filePath};
					//					backgroundWorker1.RunWorkerAsync(model);// Start the asynchronous operation.
			        backgroundWorker1.RunWorkerAsync((object)casting);// Start the asynchronous operation.
            		//using (StreamReader reader = new StreamReader(fileStream))			        {			            fileContent = reader.ReadToEnd();			        }
            		fileContent = Encoding.Default.GetString(File.ReadAllBytes(filePath));
            		textoutput showmodelstuff = new textoutput(fileContent);
            		//showmodelstuff.Location.X = 900;
            		//showmodelstuff.Location.Y = 900;
            		
            		showmodelstuff.Activate();
            		showmodelstuff.Show();
			        
			    }
			}
            
			
		}
		private void cancelAsyncButton_Click(System.Object sender, System.EventArgs e)
        {   
            this.backgroundWorker1.CancelAsync();// Cancel the asynchronous operation.
            cancelAsyncButton.Enabled = false;// Disable the Cancel button.
        }
	}
}
