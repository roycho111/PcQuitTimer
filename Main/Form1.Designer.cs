namespace Main
{
    partial class Form1
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.confirmBtn = new System.Windows.Forms.Button();
            this.input_hour = new System.Windows.Forms.NumericUpDown();
            this.input_minute = new System.Windows.Forms.NumericUpDown();
            this.input_second = new System.Windows.Forms.NumericUpDown();
            this.labe_hour = new System.Windows.Forms.Label();
            this.label_minute = new System.Windows.Forms.Label();
            this.label_second = new System.Windows.Forms.Label();
            this.cancelBtn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.input_hour)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.input_minute)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.input_second)).BeginInit();
            this.SuspendLayout();
            // 
            // confirmBtn
            // 
            this.confirmBtn.Location = new System.Drawing.Point(432, 78);
            this.confirmBtn.Name = "confirmBtn";
            this.confirmBtn.Size = new System.Drawing.Size(75, 23);
            this.confirmBtn.TabIndex = 0;
            this.confirmBtn.Text = "확인";
            this.confirmBtn.UseVisualStyleBackColor = true;
            this.confirmBtn.Click += new System.EventHandler(this.ConfirmBtnClick);
            // 
            // input_hour
            // 
            this.input_hour.Location = new System.Drawing.Point(92, 78);
            this.input_hour.Name = "input_hour";
            this.input_hour.Size = new System.Drawing.Size(59, 21);
            this.input_hour.TabIndex = 5;
            // 
            // input_minute
            // 
            this.input_minute.Location = new System.Drawing.Point(186, 78);
            this.input_minute.Name = "input_minute";
            this.input_minute.Size = new System.Drawing.Size(56, 21);
            this.input_minute.TabIndex = 6;
            // 
            // input_second
            // 
            this.input_second.Location = new System.Drawing.Point(272, 78);
            this.input_second.Name = "input_second";
            this.input_second.Size = new System.Drawing.Size(50, 21);
            this.input_second.TabIndex = 7;
            // 
            // labe_hour
            // 
            this.labe_hour.AutoSize = true;
            this.labe_hour.Location = new System.Drawing.Point(154, 83);
            this.labe_hour.Name = "labe_hour";
            this.labe_hour.Size = new System.Drawing.Size(29, 12);
            this.labe_hour.TabIndex = 8;
            this.labe_hour.Text = "시간";
            // 
            // label_minute
            // 
            this.label_minute.AutoSize = true;
            this.label_minute.Location = new System.Drawing.Point(245, 83);
            this.label_minute.Name = "label_minute";
            this.label_minute.Size = new System.Drawing.Size(17, 12);
            this.label_minute.TabIndex = 8;
            this.label_minute.Text = "분";
            // 
            // label_second
            // 
            this.label_second.AutoSize = true;
            this.label_second.Location = new System.Drawing.Point(325, 82);
            this.label_second.Name = "label_second";
            this.label_second.Size = new System.Drawing.Size(101, 12);
            this.label_second.TabIndex = 8;
            this.label_second.Text = "초 후 시스템 종료";
            // 
            // cancelBtn
            // 
            this.cancelBtn.Location = new System.Drawing.Point(432, 125);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new System.Drawing.Size(75, 23);
            this.cancelBtn.TabIndex = 0;
            this.cancelBtn.Text = "취소";
            this.cancelBtn.UseVisualStyleBackColor = true;
            this.cancelBtn.Click += new System.EventHandler(this.CancelBtnClick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(576, 272);
            this.Controls.Add(this.label_second);
            this.Controls.Add(this.label_minute);
            this.Controls.Add(this.labe_hour);
            this.Controls.Add(this.input_second);
            this.Controls.Add(this.input_minute);
            this.Controls.Add(this.input_hour);
            this.Controls.Add(this.cancelBtn);
            this.Controls.Add(this.confirmBtn);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.input_hour)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.input_minute)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.input_second)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button confirmBtn;
        private System.Windows.Forms.NumericUpDown input_hour;
        private System.Windows.Forms.NumericUpDown input_minute;
        private System.Windows.Forms.NumericUpDown input_second;
        private System.Windows.Forms.Label labe_hour;
        private System.Windows.Forms.Label label_minute;
        private System.Windows.Forms.Label label_second;
        private System.Windows.Forms.Button cancelBtn;
    }
}

