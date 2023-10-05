using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace TORServices.FormsTor
{
 

    [DefaultEvent("DoWork")]
    public class WorkerProgressbar : System.Windows.Forms.ProgressBar
    {
        // Private statics
        private static readonly object doWorkKey = new object();
        private static readonly object runWorkerCompletedKey = new object();
        private static readonly object progressChangedKey = new object();

        // Private instance members
        private bool canCancelWorker = false;
        private bool completeWorker = false;
        private bool workerReportsProgress = true;
        private bool cancellationPending = false;
        private bool isRunning = false;
        private AsyncOperation asyncOperation = null;
        private readonly WorkerThreadStartDelegate threadStart;
        private readonly SendOrPostCallback operationCompleted;
        public TimeSpan time;
        public  DateTime dStart;
        public  DateTime dFinal;
        public WorkerProgressbar()
        {
            threadStart = new WorkerThreadStartDelegate(WorkerThreadStart);
            operationCompleted = new SendOrPostCallback(AsyncOperationCompleted);
            base.ForeColor = SystemColors.ControlText;

        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams myParams = base.CreateParams;

                // Make the control use double buffering
                myParams.ExStyle |= NativeMethods.WS_EX_COMPOSITED;

                return myParams;
            }
        }

        protected override void WndProc(ref Message m)
        {
            int message = m.Msg;

            if (message == NativeMethods.WM_PAINT)
            {
                // Create a wrapper for the Handle
                HandleRef myHandle = new HandleRef(this, Handle);

                // Prepare the window for painting and retrieve a device context
                PAINTSTRUCT pAINTSTRUCT = new  PAINTSTRUCT();
                IntPtr hDC = UnsafeNativeMethods.BeginPaint(myHandle, ref pAINTSTRUCT);

                try
                {
                    // Apply hDC to message
                    m.WParam = hDC;

                    // Let Windows paint
                    base.WndProc(ref m);

                    // Custom painting
                    PaintPrivate(hDC);
                }
                finally
                {
                    // Release the device context that BeginPaint retrieved
                    UnsafeNativeMethods.EndPaint(myHandle, ref pAINTSTRUCT);
                }
                return;
            }

            if (message == NativeMethods.WM_PRINTCLIENT)
            {
                IntPtr hDC = m.WParam;

                // Let Windows paint
                base.WndProc(ref m);

                // Custom painting
                PaintPrivate(hDC);
                return;
            }
           // if(message == NativeMethods.)
            base.WndProc(ref m);
        }

        private void PaintPrivate(IntPtr device)
        {
            // Create a Graphics object for the device context
            using (Graphics graphics = Graphics.FromHdc(device))
            {
                graphics.FillRectangle(new SolidBrush(Color.FromArgb(0, Color.Black)), ClientRectangle);
                string s = "";
                if (cancellationPending)
                {
                    s = "Cancel Work";
                }
                else if (completeWorker)
                {
                    s = "complete Worker time: " + time.ToString(@"hh\:mm\:ss");
                }
                if (ShowPercent && Value< Maximum)
                {
                    time = DateTime.Now - dStart;

                    TextRenderer.DrawText(graphics, (Value * 100 / Maximum).ToString("0.00") + " % Time: " + time.ToString(@"hh\:mm\:ss"), Font, ClientRectangle, ForeColor); 
                
                }
                else
                { TextRenderer.DrawText(graphics, s, Font, ClientRectangle, ForeColor); }
                
            }
        }
        private bool showPercent;
        [System.ComponentModel.Browsable(true)]
        [System.ComponentModel.DefaultValue(false)]
        [System.ComponentModel.Category("Appearance")]
        [System.ComponentModel.Description("Show text Percent")]
      //  public enum SelectedPathType { colorDialog, folderDialog, fontDialog, OpenFileDialog, SaveFileDialog }
        public bool ShowPercent
        {
            get { return showPercent; }
            set
            {

                showPercent = value;
            }
        }


        private void AsyncOperationCompleted(object arg)
        {
            isRunning = false;
            cancellationPending = false;
            OnRunWorkerCompleted((RunWorkerCompletedEventArgs)arg);
        }

        [Browsable(false)]
        public bool CancellationPending
        {
            get { return cancellationPending; }
        }

        public void CancelAsync()
        {
            if (!WorkerSupportsCancellation)
            {
                throw new InvalidOperationException("BackgroundWorker_WorkerDoesntSupportCancellation");
            }

            cancellationPending = true;
        }

        public event DoWorkEventHandler DoWork
        {
            add
            {
                this.Events.AddHandler(doWorkKey, value);
            }
            remove
            {
                this.Events.RemoveHandler(doWorkKey, value);
            }
        }


        [Browsable(false)]
        public bool IsBusy
        {
            get
            {
                return isRunning;
            }
        }

        protected virtual void OnDoWork(DoWorkEventArgs e)
        {


            DoWorkEventHandler handler = (DoWorkEventHandler)(Events[doWorkKey]);
            if (handler != null)
            {
                handler(this, e);
            }
        }


        protected virtual void OnRunWorkerCompleted(RunWorkerCompletedEventArgs e)
        {
            RunWorkerCompletedEventHandler handler = (RunWorkerCompletedEventHandler)(Events[runWorkerCompletedKey]);
            completeWorker = true;
            dFinal = DateTime.Now;
            time = dFinal - dStart;
           
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public void RunWorkerAsync()
        {
            RunWorkerAsync(null);
        }
       /* public void RunWorkerAsync(ThreadStart method)
        {
            new System.Threading.Thread(method).Start();
        }*/

        public void RunWorkerAsync(object argument)
        {
            if (isRunning)
            {
                throw new InvalidOperationException("BackgroundWorker_WorkerAlreadyRunning");
            }
            dStart = DateTime.Now;
            isRunning = true;
            cancellationPending = false;

            asyncOperation = AsyncOperationManager.CreateOperation(null);
            threadStart.BeginInvoke(argument, null,  null);
        }


        public event RunWorkerCompletedEventHandler RunWorkerCompleted
        {
            add
            {
                this.Events.AddHandler(runWorkerCompletedKey, value);
            }
            remove
            {
                this.Events.RemoveHandler(runWorkerCompletedKey, value);
            }
        }

        [DefaultValue(false)]
        public bool WorkerReportsProgress
        {
            get { return workerReportsProgress; }
            set { workerReportsProgress = value; }
        }

        [DefaultValue(false)]
        public bool WorkerSupportsCancellation
        {
            get { return canCancelWorker; }
            set { canCancelWorker = value; }
        }

        private delegate void WorkerThreadStartDelegate(object argument);

        private void WorkerThreadStart(object argument)
        {
            object workerResult = null;
            Exception error = null;
            bool cancelled = false;

            try
            {
                DoWorkEventArgs doWorkArgs = new DoWorkEventArgs(argument);
                OnDoWork(doWorkArgs);
                if (doWorkArgs.Cancel)
                {
                    cancelled = true;
                }
                else
                {
                    workerResult = doWorkArgs.Result;
                }
            }
            catch (Exception exception)
            {
                error = exception;
            }

            RunWorkerCompletedEventArgs e =
                new RunWorkerCompletedEventArgs(workerResult, error, cancelled);

            asyncOperation.PostOperationCompleted(operationCompleted, e);
        }

    }
}
