
namespace WorkTime
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label resultLabel;
        private System.Windows.Forms.Label startDateLabel;
        private System.Windows.Forms.Label endDateLabel;
        private System.Windows.Forms.Label descriptionLabel;
        private System.Windows.Forms.DataGridView dataGridViewTasks;  // Görevler tablosu
        private System.Windows.Forms.Label labelWorkTime;
        private System.Windows.Forms.Label lblAverageWorkTime;
        private System.Windows.Forms.Button Ortalama;
        private System.Windows.Forms.DataGridView usersGridView;  // Kullanıcılar ve ortalama süre tablosu

        private System.Windows.Forms.DataGridViewButtonColumn finishTaskButtonColumn;  // Buton Kolonu
        private System.Windows.Forms.DataGridViewButtonColumn finishUserButtonColumn;  // Kullanıcı butonu
        private System.Windows.Forms.ListBox listBoxTasks;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            dataGridViewTasks = new DataGridView();
            dataGridViewTextBoxColumn7 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn10 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn11 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn14 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn19 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn8 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn9 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn4 = new DataGridViewTextBoxColumn();
            finishTaskButtonColumn = new DataGridViewButtonColumn();
            finishButtonColumn = new DataGridViewButtonColumn();
            dataGridViewTextBoxColumn5 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn6 = new DataGridViewTextBoxColumn();
            usersGridView = new DataGridView();
            dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn3 = new DataGridViewTextBoxColumn();
            finishUserButtonColumn = new DataGridViewButtonColumn();
            averageWorkTimeColumn = new DataGridViewTextBoxColumn();
            resultLabel = new Label();
            startDateLabel = new Label();
            endDateLabel = new Label();
            descriptionLabel = new Label();
            labelWorkTime = new Label();
            lblAverageWorkTime = new Label();
            Ortalama = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridViewTasks).BeginInit();
            ((System.ComponentModel.ISupportInitialize)usersGridView).BeginInit();
            SuspendLayout();
            // 
            // dataGridViewTasks
            // 
            dataGridViewTasks.AllowUserToAddRows = false;
            dataGridViewTasks.AllowUserToDeleteRows = false;
            dataGridViewTasks.AllowUserToOrderColumns = true;
            dataGridViewTasks.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewTasks.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewTasks.Columns.AddRange(new DataGridViewColumn[] { dataGridViewTextBoxColumn7, dataGridViewTextBoxColumn10, dataGridViewTextBoxColumn11, dataGridViewTextBoxColumn14, dataGridViewTextBoxColumn19, dataGridViewTextBoxColumn8, dataGridViewTextBoxColumn9, finishTaskButtonColumn });
            dataGridViewTasks.Location = new Point(23, 26);
            dataGridViewTasks.Name = "dataGridViewTasks";
            dataGridViewTasks.RowHeadersWidth = 62;
            dataGridViewTasks.Size = new Size(1734, 303);
            dataGridViewTasks.TabIndex = 1;
            dataGridViewTasks.CellClick += dataGridViewTasks_CellClick;
            // 
            // dataGridViewTextBoxColumn7
            // 
            dataGridViewTextBoxColumn7.HeaderText = "User Name";
            dataGridViewTextBoxColumn7.MinimumWidth = 8;
            dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            // 
            // dataGridViewTextBoxColumn10
            // 
            dataGridViewTextBoxColumn10.HeaderText = "Task ID";
            dataGridViewTextBoxColumn10.MinimumWidth = 8;
            dataGridViewTextBoxColumn10.Name = "TaskID";
            // 
            // dataGridViewTextBoxColumn11
            // 
            dataGridViewTextBoxColumn11.HeaderText = "Description";
            dataGridViewTextBoxColumn11.MinimumWidth = 8;
            dataGridViewTextBoxColumn11.Name = "TaskDescription";
            // 
            // dataGridViewTextBoxColumn14
            // 
            dataGridViewTextBoxColumn14.HeaderText = "Start Time";
            dataGridViewTextBoxColumn14.MinimumWidth = 8;
            dataGridViewTextBoxColumn14.Name = "StartDate";
            // 
            // dataGridViewTextBoxColumn19
            // 
            dataGridViewTextBoxColumn19.HeaderText = "Last Date";
            dataGridViewTextBoxColumn19.MinimumWidth = 8;
            dataGridViewTextBoxColumn19.Name = "EndTime";
            // 
            // dataGridViewTextBoxColumn8
            // 
            dataGridViewTextBoxColumn8.HeaderText = "End Date";
            dataGridViewTextBoxColumn8.MinimumWidth = 8;
            dataGridViewTextBoxColumn8.Name = "EndDate";
            // 
            // dataGridViewTextBoxColumn9
            // 
            dataGridViewTextBoxColumn9.HeaderText = "Total Date";
            dataGridViewTextBoxColumn9.MinimumWidth = 8;
            dataGridViewTextBoxColumn9.Name = "TotalDate";
            //// 
            //// dataGridViewTextBoxColumn4
            //// 
            //dataGridViewTextBoxColumn4.HeaderText = "User Name";
            //dataGridViewTextBoxColumn4.MinimumWidth = 8;
            //dataGridViewTextBoxColumn4.Name = "UserName";
            // 
            // finishTaskButtonColumn
            // 
            finishTaskButtonColumn.HeaderText = "Finish Task";
            finishTaskButtonColumn.MinimumWidth = 8;
            finishTaskButtonColumn.Name = "FinishTask";
            finishTaskButtonColumn.Text = "Finish Task";
            finishTaskButtonColumn.UseColumnTextForButtonValue = true;
            // 
            // finishButtonColumn
            // 
            finishButtonColumn.MinimumWidth = 8;
            finishButtonColumn.Name = "finishButtonColumn";
            finishButtonColumn.Width = 150;
            // 
            // dataGridViewTextBoxColumn5
            // 
            dataGridViewTextBoxColumn5.MinimumWidth = 8;
            dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            dataGridViewTextBoxColumn5.Width = 150;
            // 
            // dataGridViewTextBoxColumn6
            // 
            dataGridViewTextBoxColumn6.MinimumWidth = 8;
            dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            dataGridViewTextBoxColumn6.Width = 150;
            // 
            // usersGridView
            // 
            usersGridView.AllowUserToAddRows = false;
            usersGridView.AllowUserToDeleteRows = false;
            usersGridView.AllowUserToOrderColumns = true;
            usersGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            usersGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            usersGridView.Columns.AddRange(new DataGridViewColumn[] { dataGridViewTextBoxColumn1, dataGridViewTextBoxColumn2, dataGridViewTextBoxColumn3, averageWorkTimeColumn });
            usersGridView.Location = new Point(23, 400);
            usersGridView.Name = "usersGridView";
            usersGridView.ReadOnly = true;
            usersGridView.RowHeadersWidth = 62;
            usersGridView.Size = new Size(956, 250);
            usersGridView.TabIndex = 0;
            usersGridView.CellClick += usersGridView_CellClick;
            // 
            // dataGridViewTextBoxColumn1
            // 
            dataGridViewTextBoxColumn1.HeaderText = "User Name";
            dataGridViewTextBoxColumn1.MinimumWidth = 8;
            dataGridViewTextBoxColumn1.Name = "UserId";
            dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            dataGridViewTextBoxColumn2.HeaderText = "Average Work Time";
            dataGridViewTextBoxColumn2.MinimumWidth = 8;
            dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn3
            // 
            dataGridViewTextBoxColumn3.HeaderText = "Average Work Time";
            dataGridViewTextBoxColumn3.MinimumWidth = 8;
            dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            dataGridViewTextBoxColumn3.ReadOnly = true;
            // 
            // finishUserButtonColumn
            // 
            //finishUserButtonColumn.HeaderText = "Finish User Task";
            //finishUserButtonColumn.MinimumWidth = 8;
            //finishUserButtonColumn.Name = "finishUserButtonColumn";
            //finishUserButtonColumn.ReadOnly = true;
            //finishUserButtonColumn.Text = "Görevi Bitir";
            //finishUserButtonColumn.UseColumnTextForButtonValue = true;
            // 
            // averageWorkTimeColumn
            // 
            averageWorkTimeColumn.HeaderText = "User ID";
            averageWorkTimeColumn.MinimumWidth = 8;
            averageWorkTimeColumn.Name = "User ID";
            averageWorkTimeColumn.ReadOnly = true;
            // 
            // resultLabel
            // 
            resultLabel.Location = new Point(0, 0);
            resultLabel.Name = "resultLabel";
            resultLabel.Size = new Size(100, 23);
            resultLabel.TabIndex = 4;
            // 
            // startDateLabel
            // 
            startDateLabel.Location = new Point(0, 0);
            startDateLabel.Name = "startDateLabel";
            startDateLabel.Size = new Size(100, 23);
            startDateLabel.TabIndex = 0;
            // 
            // endDateLabel
            // 
            endDateLabel.Location = new Point(0, 0);
            endDateLabel.Name = "endDateLabel";
            endDateLabel.Size = new Size(100, 23);
            endDateLabel.TabIndex = 0;
            // 
            // descriptionLabel
            // 
            descriptionLabel.Location = new Point(0, 0);
            descriptionLabel.Name = "descriptionLabel";
            descriptionLabel.Size = new Size(100, 23);
            descriptionLabel.TabIndex = 0;
            // 
            // labelWorkTime
            // 
            labelWorkTime.Location = new Point(0, 0);
            labelWorkTime.Name = "labelWorkTime";
            labelWorkTime.Size = new Size(100, 23);
            labelWorkTime.TabIndex = 0;
            // 
            // lblAverageWorkTime
            // 
            lblAverageWorkTime.Location = new Point(0, 0);
            lblAverageWorkTime.Name = "lblAverageWorkTime";
            lblAverageWorkTime.Size = new Size(100, 23);
            lblAverageWorkTime.TabIndex = 0;
            // 
            // Ortalama
            // 
            Ortalama.Location = new Point(0, 0);
            Ortalama.Name = "Ortalama";
            Ortalama.Size = new Size(75, 23);
            Ortalama.TabIndex = 0;
            // 
            // Form1
            // 
            BackColor = Color.FromArgb(204, 230, 255);
            ClientSize = new Size(1920, 722);
            Controls.Add(usersGridView);
            Controls.Add(dataGridViewTasks);
            Controls.Add(resultLabel);
            Name = "Form1";
            Text = "Görev Süresi Hesapla";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridViewTasks).EndInit();
            ((System.ComponentModel.ISupportInitialize)usersGridView).EndInit();
            ResumeLayout(false);
        }

        private void usersGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
        private DataGridViewButtonColumn finishButtonColumn;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn10;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn11;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn14;

        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn19;
        private DataGridViewTextBoxColumn averageWorkTimeColumn;
    }
}