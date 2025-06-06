﻿namespace SincronizadorAplix_Nidux
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
            this.ServicioCemaco = new System.ServiceProcess.ServiceProcessInstaller();
            this.serviceInstaller1 = new System.ServiceProcess.ServiceInstaller();
            // 
            // ServicioCemaco
            // 
            this.ServicioCemaco.Account = System.ServiceProcess.ServiceAccount.LocalService;
            this.ServicioCemaco.Password = null;
            this.ServicioCemaco.Username = null;
            this.ServicioCemaco.AfterInstall += new System.Configuration.Install.InstallEventHandler(this.serviceProcessInstaller1_AfterInstall);
            // 
            // serviceInstaller1
            // 
            this.serviceInstaller1.Description = "Sincronizador CEMACO con Nidux por Grupo AplixV2.3";
            this.serviceInstaller1.ServiceName = "Aplix Sincronizador CemacoV2.3";
            this.serviceInstaller1.AfterInstall += new System.Configuration.Install.InstallEventHandler(this.serviceInstaller1_AfterInstall);
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.ServicioCemaco,
            this.serviceInstaller1});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller ServicioCemaco;
        private System.ServiceProcess.ServiceInstaller serviceInstaller1;
    }
}