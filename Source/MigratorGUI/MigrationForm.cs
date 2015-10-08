using Migrator;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MigratorGUI
{
    public partial class MigrationForm : Form
    {

        #region Constructors
        public MigrationForm()
        {
            InitializeComponent();
        }
        #endregion

        #region Load
        private void MigrationForm_Load(object sender, EventArgs e)
        {
            ConnStringTextBox.Text = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
            MigrationFolderTextBox.Text = ConfigurationManager.AppSettings["MigrationFolder"];           
            SchemaFileTextBox.Text = ConfigurationManager.AppSettings["SchemaFile"];
            MigrationTableTextBox.Text = ConfigurationManager.AppSettings["VersionTable"];
            UseMigrationTableCheckBox.Checked = (MigrationTableTextBox.Text != "");
        }
        #endregion

        #region File Folder Buttons
        private void MigrationFolderButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.SelectedPath = MigrationFolderTextBox.Text;
            fbd.ShowDialog(this);

            if (fbd.SelectedPath != "")
            {
                MigrationFolderTextBox.Text = fbd.SelectedPath;
            }
        }

        private void SchemaFileButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.FileName = SchemaFileTextBox.Text;
            sfd.ShowDialog();

            if (sfd.FileName != "")
            {
                SchemaFileTextBox.Text = sfd.FileName;
            }
        }
        #endregion

        #region Button Events
        private void CreateNewMigrationButton_Click(object sender, EventArgs e)
        {
            InputBox.InputBoxResult result = InputBox.Show("Please enter a description for the Migration.", "Migration Description", "", null);
            if (result.OK)
            {
                string newFile = Migration.CreateMigrationFile(MigrationFolderTextBox.Text, MigrationTableTextBox.Text, result.Text);
                
                AddResult("Migration file created at '" + newFile + "'");
            }
        }

        private void MigrateButton_Click(object sender, EventArgs e)
        {
            try
            {
                Migration mig = new Migration(ConnStringTextBox.Text, MigrationTableTextBox.Text);
                mig.Migrate(MigrationFolderTextBox.Text, AddResult);
            }
            catch (Exception ex)
            {
                AddResult("Exception: " + ex.GetBaseException().Message);
            }
        }

        private void GenSchemaButton_Click(object sender, EventArgs e)
        {
            try
            {
                SchemaSql sch = new SchemaSql(ConnStringTextBox.Text);
                sch.WriteSchemaToFile(SchemaFileTextBox.Text, GetMigrationTable(), AddResult);

                AddResult("Schema successfully created at: " + SchemaFileTextBox.Text);
            }
            catch (Exception ex)
            {
                AddResult("Exception: " + ex.GetBaseException().Message);
            }
        }

        private void ClearResultsButton_Click(object sender, EventArgs e)
        {
            ResultsTextBox.Text = "";
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        #region Migration Table Events
        private void UseMigrationTableCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            FormStatusUpdate();
        }
        #endregion

        /// <summary>
        /// Return the migration table basedon the use migration checkbox status.
        /// </summary>
        /// <returns>Returns the text in the migration textbox unless the UseMigration checkbox
        /// is false then "" is returned.</returns>
        private string GetMigrationTable()
        {
            if (UseMigrationTableCheckBox.Checked)
            {
                return MigrationTableTextBox.Text;
            }
            else
            {
                return "";
            }
        }

        #region Status Update
        private void FormStatusUpdate()
        {
            MigrationTableTextBox.Enabled = UseMigrationTableCheckBox.Checked;
        }
        #endregion

        #region Results
        private void AddResult(string msg)
        {
            ResultsTextBox.Text += DateTime.Now.ToString() + " - " + msg + Environment.NewLine;
        }
        #endregion
        
    }
}
