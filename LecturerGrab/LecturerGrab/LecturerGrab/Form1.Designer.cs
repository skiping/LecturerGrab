namespace LecturerGrab
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btnStart = new System.Windows.Forms.Button();
            this.lecturerList = new System.Windows.Forms.ListView();
            this.btnExport = new System.Windows.Forms.Button();
            this.lblMsg = new System.Windows.Forms.Label();
            this.prevPage = new System.Windows.Forms.Button();
            this.nextPage = new System.Windows.Forms.Button();
            this.lblPage = new System.Windows.Forms.Label();
            this.lblPage1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(26, 12);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(117, 32);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "抓取";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // lecturerList
            // 
            this.lecturerList.Font = new System.Drawing.Font("宋体", 12F);
            this.lecturerList.ForeColor = System.Drawing.Color.Black;
            this.lecturerList.FullRowSelect = true;
            this.lecturerList.GridLines = true;
            this.lecturerList.HoverSelection = true;
            this.lecturerList.Location = new System.Drawing.Point(26, 81);
            this.lecturerList.Name = "lecturerList";
            this.lecturerList.Size = new System.Drawing.Size(922, 597);
            this.lecturerList.TabIndex = 1;
            this.lecturerList.UseCompatibleStateImageBehavior = false;
            this.lecturerList.View = System.Windows.Forms.View.Details;
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(175, 12);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(107, 32);
            this.btnExport.TabIndex = 2;
            this.btnExport.Text = "导出";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // lblMsg
            // 
            this.lblMsg.AutoSize = true;
            this.lblMsg.ForeColor = System.Drawing.Color.Red;
            this.lblMsg.Location = new System.Drawing.Point(479, 44);
            this.lblMsg.Name = "lblMsg";
            this.lblMsg.Size = new System.Drawing.Size(41, 12);
            this.lblMsg.TabIndex = 3;
            this.lblMsg.Text = "label1";
            // 
            // prevPage
            // 
            this.prevPage.Location = new System.Drawing.Point(706, 701);
            this.prevPage.Name = "prevPage";
            this.prevPage.Size = new System.Drawing.Size(75, 23);
            this.prevPage.TabIndex = 4;
            this.prevPage.Text = "上一页";
            this.prevPage.UseVisualStyleBackColor = true;
            this.prevPage.Click += new System.EventHandler(this.prevPage_Click);
            // 
            // nextPage
            // 
            this.nextPage.Location = new System.Drawing.Point(818, 701);
            this.nextPage.Name = "nextPage";
            this.nextPage.Size = new System.Drawing.Size(75, 23);
            this.nextPage.TabIndex = 5;
            this.nextPage.Text = "下一页";
            this.nextPage.UseVisualStyleBackColor = true;
            this.nextPage.Click += new System.EventHandler(this.nextPage_Click);
            // 
            // lblPage
            // 
            this.lblPage.AutoSize = true;
            this.lblPage.Location = new System.Drawing.Point(495, 707);
            this.lblPage.Name = "lblPage";
            this.lblPage.Size = new System.Drawing.Size(0, 12);
            this.lblPage.TabIndex = 6;
            // 
            // lblPage1
            // 
            this.lblPage1.AutoSize = true;
            this.lblPage1.Location = new System.Drawing.Point(644, 707);
            this.lblPage1.Name = "lblPage1";
            this.lblPage1.Size = new System.Drawing.Size(0, 12);
            this.lblPage1.TabIndex = 7;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(994, 787);
            this.Controls.Add(this.lblPage1);
            this.Controls.Add(this.lblPage);
            this.Controls.Add(this.nextPage);
            this.Controls.Add(this.prevPage);
            this.Controls.Add(this.lblMsg);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.lecturerList);
            this.Controls.Add(this.btnStart);
            this.Name = "Form1";
            this.Text = "讲师抓取";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.ListView lecturerList;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Label lblMsg;
        private System.Windows.Forms.Button prevPage;
        private System.Windows.Forms.Button nextPage;
        private System.Windows.Forms.Label lblPage;
        private System.Windows.Forms.Label lblPage1;
    }
}

