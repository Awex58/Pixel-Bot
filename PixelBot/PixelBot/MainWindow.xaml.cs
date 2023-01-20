using System;
using System.Windows; // WPF
using System.Drawing; // Color, Bitmap, Graphics
using System.Runtime.InteropServices;
using System.Threading.Tasks; // User32.dll (and dll import)
using System.Windows.Forms;
using System.Windows.Input;
using Clipboard = System.Windows.Clipboard; // Screen height and width
using MessageBox = System.Windows.MessageBox; // Use message box of wpf (not forms)
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media;
using Color = System.Drawing.Color;
using Cursor = System.Windows.Forms.Cursor;
using Point = System.Windows.Point;
using System.Drawing.Imaging;
using System.Diagnostics;

namespace PixelBot
{
    // usecases: clicker bot for game - tell 
    public partial class MainWindow : Window
    {
        private const UInt32 MOUSEEVENTF_LEFTDOWN = 0x0002;
        private const UInt32 MOUSEEVENTF_LEFTUP = 0x0004;

        [DllImport("user32.dll")]
        private static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, uint dwExtraInf);

        [DllImport("user32.dll")]
        private static extern bool SetCursorPos(int x, int y);




        // Or use whatever point class you like for the implicit cast operator

        /// <summary>
        /// Struct representing a point.
        /// </summary>
        
         [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;

            public static implicit operator Point(POINT point)
            {
                return new Point(point.X, point.Y);
            }
        }
        /// <summary>
        /// Retrieves the cursor's position, in screen coordinates.
        /// </summary>
        /// <see>See MSDN documentation for further information.</see>
        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(out POINT lpPoint);

        public static Point GetCursorPosition()
        {
            POINT lpPoint;
            GetCursorPos(out lpPoint);
            // NOTE: If you need error handling
            // bool success = GetCursorPos(out lpPoint);
            // if (!success)

            return lpPoint;
        }


        


        public MainWindow()
        {
            InitializeComponent();
            
        }

        public bool runcontrol = false;
        private async void OnButtonSearchPixelClick(object sender, RoutedEventArgs e)
        {
            // 01. get hex value from input field
            string inputHexColorCode = TextBoxHexColor.Text;
            string inputhexcolorcode = TextBoxHexColor_Copy.Text;
            int delay = Convert.ToInt32(TextBoxDelay.Text);
            int firstxsearch = Convert.ToInt32(fxs.Text);
            int firstysearch = Convert.ToInt32(fys.Text);
            int Lasttxsearch = Convert.ToInt32(lxs.Text);
            int lastYsearch = Convert.ToInt32(lys.Text);

            int cursorclickx = Convert.ToInt32(cxl.Text);
            int cursorclicky = Convert.ToInt32(cyl.Text);


            ButtonSearchPixel.IsEnabled = false;
            ButtonSearchPixel_stop.IsEnabled = true;
            ButtonSearchPixel.Content = "Already Running!";
            runcontrol = true; 
            SearchPixel(inputHexColorCode,inputhexcolorcode,delay, firstxsearch, firstysearch, Lasttxsearch, lastYsearch, cursorclickx, cursorclicky);
            
        }

        public  void falsebtn()
        {
            ButtonSearchPixel.IsEnabled = true;
            ButtonSearchPixel_stop.IsEnabled = false;
            ButtonSearchPixel.Content = "Run!";
            runcontrol = false;
        }
        private async void ButtonSearchPixel_stop_Click(object sender, RoutedEventArgs e)
        {
           falsebtn();
        }


        public int ysize, xsize;
        private void deneme_Click(object sender, RoutedEventArgs e)
        {
            int firstxsearch = Convert.ToInt32(fxs.Text);
            int firstysearch = Convert.ToInt32(fys.Text);
            int Lasttxsearch = Convert.ToInt32(lxs.Text);
            int lastYsearch = Convert.ToInt32(lys.Text);

             xsize = Math.Abs(Lasttxsearch - firstxsearch);
             ysize = Math.Abs(lastYsearch - firstysearch);


            var codeBitmap = new Bitmap(xsize, ysize);
            Graphics graphics = Graphics.FromImage(codeBitmap as Image);

            graphics.CopyFromScreen(firstxsearch, firstysearch, 0, 0, codeBitmap.Size);

            codeBitmap.Save("myfile.png", ImageFormat.Jpeg);
            Process.Start(@"myfile.png");

        }






        
        public async Task SearchPixel(string hexcode, string hexcode2,int delay,int firstxsearch,int firstysearch,int Lasttxsearch,int lastYsearch,int cursorclickx,int cursorclicky)
        {
        
        try
        {
             xsize = Math.Abs(Lasttxsearch - firstxsearch);
             ysize = Math.Abs(lastYsearch - firstysearch);
             
            tekrar:

                if (runcontrol == true)
                {


                    Bitmap bitmap = new Bitmap(xsize,ysize );

                    Graphics graphics = Graphics.FromImage(bitmap as Image); 

                    graphics.CopyFromScreen(firstxsearch, firstysearch, 0, 0, bitmap.Size); // Screenshot moment → screen content to graphics object



                    Color desiredPixelColor = ColorTranslator.FromHtml(hexcode);
                    Color desiredPixelColor2 = ColorTranslator.FromHtml(hexcode2);

                    

                    for (int x = 0; x < xsize; x++)
                    {
                        for (int y = 0; y < ysize; y++)
                        {
                            // Get the current pixels color
                            Color currentPixelColor = bitmap.GetPixel(x, y);

                            // Finally compare the pixels hex color and the desired hex color (if they match we found a pixel)
                            if (desiredPixelColor == currentPixelColor || desiredPixelColor2 == currentPixelColor)
                            {
                                runcontrol = false; // MessageBox.Show("Found Pixel - Now set mouse cursor");
                                DoubleClickAtPosition(cursorclickx, cursorclicky);
                                await Task.CompletedTask;
                                goto iptal;
                                
                            }


                        }


                    }

                // MessageBox.Show("Did not find pixel");
                iptal:
                    await Task.Delay(delay);
                    goto tekrar;
                }
                else
                {
                    falsebtn();
                    await Task.CompletedTask;

                }

            }
            catch (Exception e)
            {
                MessageBox.Show("Error! The information entered is incomplete or incorrect.","Error!",MessageBoxButton.OK,MessageBoxImage.Error);
                
            }
       
        }

        private void DoubleClickAtPosition(int posX, int posY)
        {
            SetCursorPos(posX, posY);

            Click();
            System.Threading.Thread.Sleep(250);
            Click(); 
        }

        private void Click()
        {
            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
        }

        private void Buttonauto2_Click(object sender, RoutedEventArgs e)
        {
            
            TextBoxHexColor_Copy.Text = Clipboard.GetText();
            
        }
       

        private void Buttonauto_Click(object sender, RoutedEventArgs e)
        {
            TextBoxHexColor.Text = Clipboard.GetText();
        }
        
        

        private async void cursorcapturestop_Click(object sender, RoutedEventArgs e)
        {
            cursorcapturerun.IsEnabled = true;
            cursorcapturestop.IsEnabled = false;
            cursoropen = false;
        }
        public bool cursoropen= false;
        private async void cursorcapturerun_Click(object sender, RoutedEventArgs e)
        {
            cursorcapturestop.IsEnabled = true;
            cursorcapturerun.IsEnabled = false;
            cursoropen = true;

            cursorgett();


        }

        public async Task cursorgett()
        {
           ;
           while (cursoropen == true)
           {
               cursorloc.Content = GetCursorPosition();
               await Task.Delay(150);
           }

            await Task.CompletedTask;
        }

       
    }
}
