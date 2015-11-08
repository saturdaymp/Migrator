namespace MigratorGUI
{
    partial class MigrationForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MigrationForm));
            this.ConnStringLabel = new System.Windows.Forms.Label();
            this.MigrationFolderLabel = new System.Windows.Forms.Label();
            this.ConnStringTextBox = new System.Windows.Forms.TextBox();
            this.MigrationFolderTextBox = new System.Windows.Forms.TextBox();
            this.MigrationFolderButton = new System.Windows.Forms.Button();
            this.ResultsGroupBox = new System.Windows.Forms.GroupBox();
            this.ResultsTextBox = new System.Windows.Forms.TextBox();
            this.CloseButton = new System.Windows.Forms.Button();
            this.MigrateButton = new System.Windows.Forms.Button();
            this.CreateNewMigrationButton = new System.Windows.Forms.Button();
            this.MigrationTableTextBox = new System.Windows.Forms.TextBox();
            this.MigrationTableLabel = new System.Windows.Forms.Label();
            this.GenSchemaButton = new System.Windows.Forms.Button();
            this.SchemaFileTextBox = new System.Windows.Forms.TextBox();
            this.SchemaFileLabel = new System.Windows.Forms.Label();
            this.SchemaFileButton = new System.Windows.Forms.Button();
            this.ClearResultsButton = new System.Windows.Forms.Button();
            this.UseMigrationTableCheckBox = new System.Windows.Forms.CheckBox();
            this.ResultsGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // ConnStringLabel
            // 
            this.ConnStringLabel.AutoSize = true;
            this.ConnStringLabel.Location = new System.Drawing.Point(24, 27);
            this.ConnStringLabel.Name = "ConnStringLabel";
            this.ConnStringLabel.Size = new System.Drawing.Size(94, 13);
            this.ConnStringLabel.TabIndex = 0;
            this.ConnStringLabel.Text = "Connection String:";
            // 
            // MigrationFolderLabel
            // 
            this.MigrationFolderLabel.AutoSize = true;
            this.MigrationFolderLabel.Location = new System.Drawing.Point(24, 54);
            this.MigrationFolderLabel.Name = "MigrationFolderLabel";
            this.MigrationFolderLabel.Size = new System.Drawing.Size(85, 13);
            this.MigrationFolderLabel.TabIndex = 1;
            this.MigrationFolderLabel.Text = "Migration Folder:";
            // 
            // ConnStringTextBox
            // 
            this.ConnStringTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ConnStringTextBox.Location = new System.Drawing.Point(124, 24);
            this.ConnStringTextBox.Name = "ConnStringTextBox";
            this.ConnStringTextBox.Size = new System.Drawing.Size(644, 20);
            this.ConnStringTextBox.TabIndex = 2;
            // 
            // MigrationFolderTextBox
            // 
            this.MigrationFolderTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MigrationFolderTextBox.Location = new System.Drawing.Point(124, 50);
            this.MigrationFolderTextBox.Name = "MigrationFolderTextBox";
            this.MigrationFolderTextBox.Size = new System.Drawing.Size(644, 20);
            this.MigrationFolderTextBox.TabIndex = 3;
            // 
            // MigrationFolderButton
            // 
            this.MigrationFolderButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.MigrationFolderButton.Location = new System.Drawing.Point(764, 48);
            this.MigrationFolderButton.Name = "MigrationFolderButton";
            this.MigrationFolderButton.Size = new System.Drawing.Size(24, 23);
            this.MigrationFolderButton.TabIndex = 4;
            this.MigrationFolderButton.Text = "...";
            this.MigrationFolderButton.UseVisualStyleBackColor = true;
            this.MigrationFolderButton.Click += new System.EventHandler(this.MigrationFolderButton_Click);
            // 
            // ResultsGroupBox
            // 
            this.ResultsGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ResultsGroupBox.Controls.Add(this.ResultsTextBox);
            this.ResultsGroupBox.Location = new System.Drawing.Point(27, 151);
            this.ResultsGroupBox.Name = "ResultsGroupBox";
            this.ResultsGroupBox.Size = new System.Drawing.Size(746, 319);
            this.ResultsGroupBox.TabIndex = 5;
            this.ResultsGroupBox.TabStop = false;
            this.ResultsGroupBox.Text = "Results";
            // 
            // ResultsTextBox
            // 
            this.ResultsTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ResultsTextBox.Location = new System.Drawing.Point(6, 19);
            this.ResultsTextBox.Multiline = true;
            this.ResultsTextBox.Name = "ResultsTextBox";
            this.ResultsTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.ResultsTextBox.Size = new System.Drawing.Size(734, 294);
            this.ResultsTextBox.TabIndex = 0;
            this.ResultsTextBox.WordWrap = false;
            // 
            // CloseButton
            // 
            this.CloseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CloseButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CloseButton.Location = new System.Drawing.Point(692, 476);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(75, 23);
            this.CloseButton.TabIndex = 6;
            this.CloseButton.Text = "&Close";
            this.CloseButton.UseVisualStyleBackColor = true;
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // MigrateButton
            // 
            this.MigrateButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.MigrateButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.MigrateButton.Location = new System.Drawing.Point(191, 476);
            this.MigrateButton.Name = "MigrateButton";
            this.MigrateButton.Size = new System.Drawing.Size(75, 23);
            this.MigrateButton.TabIndex = 7;
            this.MigrateButton.Text = "&Migrate";
            this.MigrateButton.UseVisualStyleBackColor = true;
            this.MigrateButton.Click += new System.EventHandler(this.MigrateButton_Click);
            // 
            // CreateNewMigrationButton
            // 
            this.CreateNewMigrationButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CreateNewMigrationButton.Location = new System.Drawing.Point(38, 476);
            this.CreateNewMigrationButton.Name = "CreateNewMigrationButton";
            this.CreateNewMigrationButton.Size = new System.Drawing.Size(147, 23);
            this.CreateNewMigrationButton.TabIndex = 8;
            this.CreateNewMigrationButton.Text = "Create New Migration File";
            this.CreateNewMigrationButton.UseVisualStyleBackColor = true;
            this.CreateNewMigrationButton.Click += new System.EventHandler(this.CreateNewMigrationButton_Click);
            // 
            // MigrationTableTextBox
            // 
            this.MigrationTableTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MigrationTableTextBox.Location = new System.Drawing.Point(124, 76);
            this.MigrationTableTextBox.Name = "MigrationTableTextBox";
            this.MigrationTableTextBox.Size = new System.Drawing.Size(246, 20);
            this.MigrationTableTextBox.TabIndex = 10;
            // 
            // MigrationTableLabel
            // 
            this.MigrationTableLabel.AutoSize = true;
            this.MigrationTableLabel.Location = new System.Drawing.Point(24, 80);
            this.MigrationTableLabel.Name = "MigrationTableLabel";
            this.MigrationTableLabel.Size = new System.Drawing.Size(83, 13);
            this.MigrationTableLabel.TabIndex = 9;
            this.MigrationTableLabel.Text = "Migration Table:";
            // 
            // GenSchemaButton
            // 
            this.GenSchemaButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.GenSchemaButton.Location = new System.Drawing.Point(272, 476);
            this.GenSchemaButton.Name = "GenSchemaButton";
            this.GenSchemaButton.Size = new System.Drawing.Size(117, 23);
            this.GenSchemaButton.TabIndex = 11;
            this.GenSchemaButton.Text = "Generate Schema";
            this.GenSchemaButton.UseVisualStyleBackColor = true;
            this.GenSchemaButton.Click += new System.EventHandler(this.GenSchemaButton_Click);
            // 
            // SchemaFileTextBox
            // 
            this.SchemaFileTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SchemaFileTextBox.Location = new System.Drawing.Point(123, 102);
            this.SchemaFileTextBox.Name = "SchemaFileTextBox";
            this.SchemaFileTextBox.Size = new System.Drawing.Size(644, 20);
            this.SchemaFileTextBox.TabIndex = 13;
            // 
            // SchemaFileLabel
            // 
            this.SchemaFileLabel.AutoSize = true;
            this.SchemaFileLabel.Location = new System.Drawing.Point(23, 106);
            this.SchemaFileLabel.Name = "SchemaFileLabel";
            this.SchemaFileLabel.Size = new System.Drawing.Size(68, 13);
            this.SchemaFileLabel.TabIndex = 12;
            this.SchemaFileLabel.Text = "Schema File:";
            // 
            // SchemaFileButton
            // 
            this.SchemaFileButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SchemaFileButton.Location = new System.Drawing.Point(764, 101);
            this.SchemaFileButton.Name = "SchemaFileButton";
            this.SchemaFileButton.Size = new System.Drawing.Size(24, 23);
            this.SchemaFileButton.TabIndex = 14;
            this.SchemaFileButton.Text = "...";
            this.SchemaFileButton.UseVisualStyleBackColor = true;
            this.SchemaFileButton.Click += new System.EventHandler(this.SchemaFileButton_Click);
            // 
            // ClearResultsButton
            // 
            this.ClearResultsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ClearResultsButton.Location = new System.Drawing.Point(395, 476);
            this.ClearResultsButton.Name = "ClearResultsButton";
            this.ClearResultsButton.Size = new System.Drawing.Size(95, 23);
            this.ClearResultsButton.TabIndex = 15;
            this.ClearResultsButton.Text = "Clear Results";
            this.ClearResultsButton.UseVisualStyleBackColor = true;
            this.ClearResultsButton.Click += new System.EventHandler(this.ClearResultsButton_Click);
            // 
            // UseMigrationTableCheckBox
            // 
            this.UseMigrationTableCheckBox.AutoSize = true;
            this.UseMigrationTableCheckBox.Location = new System.Drawing.Point(376, 78);
            this.UseMigrationTableCheckBox.Name = "UseMigrationTableCheckBox";
            this.UseMigrationTableCheckBox.Size = new System.Drawing.Size(121, 17);
            this.UseMigrationTableCheckBox.TabIndex = 16;
            this.UseMigrationTableCheckBox.Text = "Use Migration Table";
            this.UseMigrationTableCheckBox.UseVisualStyleBackColor = true;
            // 
            // MigrationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CloseButton;
            this.ClientSize = new System.Drawing.Size(802, 511);
            this.Controls.Add(this.UseMigrationTableCheckBox);
            this.Controls.Add(this.ClearResultsButton);
            this.Controls.Add(this.SchemaFileButton);
            this.Controls.Add(this.SchemaFileTextBox);
            this.Controls.Add(this.SchemaFileLabel);
            this.Controls.Add(this.GenSchemaButton);
            this.Controls.Add(this.MigrationTableTextBox);
            this.Controls.Add(this.MigrationTableLabel);
            this.Controls.Add(this.CreateNewMigrationButton);
            this.Controls.Add(this.MigrateButton);
            this.Controls.Add(this.CloseButton);
            this.Controls.Add(this.ResultsGroupBox);
            this.Controls.Add(this.MigrationFolderButton);
            this.Controls.Add(this.MigrationFolderTextBox);
            this.Controls.Add(this.ConnStringTextBox);
            this.Controls.Add(this.MigrationFolderLabel);
            this.Controls.Add(this.ConnStringLabel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MigrationForm";
            this.Text = "Migrator";
            this.Load += new System.EventHandler(this.MigrationForm_Load);
            this.ResultsGroupBox.ResumeLayout(false);
            this.ResultsGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label ConnStringLabel;
        private System.Windows.Forms.Label MigrationFolderLabel;
        private System.Windows.Forms.TextBox ConnStringTextBox;
        private System.Windows.Forms.TextBox MigrationFolderTextBox;
        private System.Windows.Forms.Button MigrationFolderButton;
        private System.Windows.Forms.GroupBox ResultsGroupBox;
        private System.Windows.Forms.TextBox ResultsTextBox;
        private System.Windows.Forms.Button CloseButton;
        private System.Windows.Forms.Button MigrateButton;
        private System.Windows.Forms.Button CreateNewMigrationButton;
        private System.Windows.Forms.TextBox MigrationTableTextBox;
        private System.Windows.Forms.Label MigrationTableLabel;
        private System.Windows.Forms.Button GenSchemaButton;
        private System.Windows.Forms.TextBox SchemaFileTextBox;
        private System.Windows.Forms.Label SchemaFileLabel;
        private System.Windows.Forms.Button SchemaFileButton;
        private System.Windows.Forms.Button ClearResultsButton;
        private System.Windows.Forms.CheckBox UseMigrationTableCheckBox;
    }
}

