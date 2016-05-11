namespace WIUTAttendance.ReportingService
{
    partial class ProjectInstaller
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
            this.ReportingServiceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.ReportingServiceInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // ReportingServiceProcessInstaller
            // 
            this.ReportingServiceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.ReportingServiceProcessInstaller.Password = null;
            this.ReportingServiceProcessInstaller.Username = null;
            // 
            // ReportingServiceInstaller
            // 
            this.ReportingServiceInstaller.Description = "ReportingServiceInstaller";
            this.ReportingServiceInstaller.DisplayName = "ReportingServiceInstaller";
            this.ReportingServiceInstaller.ServiceName = "Service1";
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.ReportingServiceProcessInstaller,
            this.ReportingServiceInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller ReportingServiceProcessInstaller;
        private System.ServiceProcess.ServiceInstaller ReportingServiceInstaller;
    }
}