namespace PriceIndex.BackService
{
    partial class MainService
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.TimeForminute = new System.Timers.Timer();
            ((System.ComponentModel.ISupportInitialize)(this.TimeForminute)).BeginInit();
            // 
            // TimeForminute
            // 
            this.TimeForminute.Enabled = true;
            this.TimeForminute.Interval = 1000D;
            this.TimeForminute.Elapsed += new System.Timers.ElapsedEventHandler(this.TimeForminute_Elapsed);
            // 
            // MainService
            // 
            this.ServiceName = "WSCreateTodayBaseData";
            ((System.ComponentModel.ISupportInitialize)(this.TimeForminute)).EndInit();

        }

        #endregion

        private System.Timers.Timer TimeForminute;

    }
}
