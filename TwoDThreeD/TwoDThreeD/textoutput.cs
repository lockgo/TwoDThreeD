/*
 * Created by SharpDevelop.
 * User: Crogogo
 * Date: 7/17/2020
 * Time: 9:03 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace TwoDThreeD
{
	/// <summary>
	/// Description of textoutput.
	/// </summary>
	public partial class textoutput : Form
	{
		public string alltext = "";
		public textoutput()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		public textoutput(string texttoshow)
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			richTextBox1.Text = texttoshow;
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		public void loadText()
		{
			richTextBox1.Text = alltext;
		}
	}
}
