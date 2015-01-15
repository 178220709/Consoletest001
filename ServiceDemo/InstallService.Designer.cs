namespace PriceIndex.BackService
{
    partial class InstallService
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
            this.pInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.sInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // pInstaller
            // 
            this.pInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.pInstaller.Password = null;
            this.pInstaller.Username = null;
            // 
            // sInstaller
            // 
            this.sInstaller.DisplayName = "PriceIndexEnd Create Today Base Data";
            this.sInstaller.ServiceName = "WSCreateTodayBaseData";
            this.sInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // InstallService
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.pInstaller,
            this.sInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller pInstaller;
        private System.ServiceProcess.ServiceInstaller sInstaller;
    }
}