using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace CSharpEx
{
    /// <summary>
    /// Summary description for Form1.
    /// </summary>
    public class frmMain : System.Windows.Forms.Form
    {
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnProps;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        public frmMain()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
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
            this.btnStart = new System.Windows.Forms.Button();
            this.btnProps = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(112, 8);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(112, 32);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "Старт";
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnProps
            // 
            this.btnProps.Location = new System.Drawing.Point(112, 56);
            this.btnProps.Name = "btnProps";
            this.btnProps.Size = new System.Drawing.Size(112, 32);
            this.btnProps.TabIndex = 1;
            this.btnProps.Text = "Настройка свойств";
            this.btnProps.Click += new System.EventHandler(this.btnProps_Click);
            // 
            // frmMain
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(328, 103);
            this.Controls.Add(this.btnProps);
            this.Controls.Add(this.btnStart);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "frmMain";
            this.Text = "frmMain";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.ResumeLayout(false);

        }
        #endregion

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.Run(new frmMain());
        }

        private FprnM1C.IFprnM45 ECR = null;
        private void frmMain_Load(object sender, System.EventArgs e)
        {
            try
            {
                ECR = new FprnM1C.FprnM45Class();
            }
            catch (Exception)
            {
                MessageBox.Show("Не удалось создать объект общего драйвера ККМ!", Application.ProductName,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                Close();
            }
        }

        private void Demo()
        {
            // занимаем порт
            ECR.DeviceEnabled = true;
            if (ECR.ResultCode != 0)
                return;

            // получаем состояние ККМ
            if (ECR.GetStatus() != 0)
                return;

            // проверяем на всякий случай ККМ на фискализированность
            if (ECR.Fiscal)
                if (MessageBox.Show("ККМ фискализирована! Вы действительно хотите продолжить?",
                        Application.ProductName,
                        System.Windows.Forms.MessageBoxButtons.YesNo,
                        System.Windows.Forms.MessageBoxIcon.Question) == DialogResult.No)
                    return;

            // если есть открытый чек, то отменяем его
            if (ECR.CheckState != 0)
                if (ECR.CancelCheck() != 0)
                    return;
/*
            // если смена открыта снимаем Z-отчет
            if (ECR.SessionOpened)
            {
                // устанавливаем пароль системного администратора ККМ
                ECR.Password = "30";
                // входим в режим отчетов с гашением
                ECR.Mode = 3;
                if (ECR.SetMode() != 0)
                    return;
                // снимаем отчет
                ECR.ReportType = 1;
                if (ECR.Report() != 0)
                    return;
            }
*/
            // входим в режим регистрации
            // устанавливаем пароль кассира
            ECR.Password = "1";
            // входим в режим регистрации
            ECR.Mode = 1;
            if (ECR.SetMode() != 0)
                return;






            // продажа без сдачи
            // регистрация продажи
            ECR.Name = "Медицинские услуги";
            ECR.Price = 1;
            ECR.Quantity = 1;
            ECR.Department = 2;
            if (ECR.Registration() != 0)
                return;
/*
            // скидка суммой на предыдущую позицию
            ECR.Percents = 10;
            ECR.Destination = 1;
            if (ECR.PercentsDiscount() != 0)
                return;
*/
            // регистрация продажи
            ECR.Name = "Медицинские услуги";
            ECR.Price = 2.00;
            ECR.Quantity = 1;
            ECR.Department = 1;
            if (ECR.Registration() != 0)
                return;
/*
            // скидка суммой на весь чек
            ECR.Summ = 0.50;
            ECR.Destination = 0;
            if (ECR.SummDiscount() != 0)
                return;
*/
            // закрытие чека наличными без ввода полученной от клиента суммы
            ECR.TypeClose = 0;
            if (ECR.CloseCheck() != 0)
                return;





            /*
                        // продажа со сдачей
                        // регистрация продажи
                        ECR.Name = "Медицинские услуги";
                        ECR.Price = 1;
                        ECR.Quantity = 1;
                        ECR.Department = 2;
                        if (ECR.Registration() != 0)
                            return;
                        // регистрация продажи
                        ECR.Name = "Медицинские услуги";
                        ECR.Price = 1;
                        ECR.Quantity = 1;
                        ECR.Department = 1;
                        if (ECR.Registration() != 0)
                            return;
                        // сторно предыдущей регистрации
                        if (ECR.Storno() != 0)
                            return;
                        // регистрация продажи
                        ECR.Name = "Медицинские услуги";
                        ECR.Price = 1;
                        ECR.Quantity = 1;
                        ECR.Department = 1;
                        if (ECR.Registration() != 0)
                            return;
                        // скидка суммой на весь чек
                        ECR.Summ = 1;
                        ECR.Destination = 0;
                        if (ECR.SummDiscount() != 0)
                            return;
                        // закрытие чека наличными с вводом полученной от клиента суммы
                        ECR.Summ = 1;
                        ECR.TypeClose = 0;
                        if (ECR.Delivery() != 0)
                            return;


            */




            /*
                        // аннулирование
                        // регистрация аннулирования
                        ECR.Name = "Медицинские услуги";
                        ECR.Price = 1;
                        ECR.Quantity = 1;
                        if (ECR.Annulate() != 0)
                            return;
                        // регистрация аннулирования
                        ECR.Name = "Медицинские услуги";
                        ECR.Price = 1;
                        ECR.Quantity = 1;
                        if (ECR.Annulate() != 0)
                            return;
                        // закрытие чека
                        ECR.TypeClose = 0;
                        if (ECR.CloseCheck() != 0)
                            return;

                        // возврат
                        // регистрация возврата
                        ECR.Name = "Медицинские услуги";
                        ECR.Price = 1;
                        ECR.Quantity = 1;
                        if (ECR.Return() != 0)
                            return;
                        // регистрация возврата
                        ECR.Name = "Медицинские услуги";
                        ECR.Price = 1;
                        ECR.Quantity = 1;
                        if (ECR.Return() != 0)
                            return;
                        // скидка суммой на весь чек
                        ECR.Summ = 0.5;
                        ECR.Destination = 0;
                        if (ECR.SummDiscount() != 0)
                            return;
                        // закрытие чека
                        ECR.TypeClose = 0;
                        if (ECR.CloseCheck() != 0)
                            return;

                        // внесение наличности
                        ECR.Summ = 2;
                        if (ECR.CashIncome() != 0)
                            return;

                        // выплата наличности
                        ECR.Summ = 1;
                        if (ECR.CashOutcome() != 0)
                            return;

                        // X - отчет
                        // устанавливаем пароль администратора ККМ
                        ECR.Password = "29";
                        // входим в режим отчетов без гашения
                        ECR.Mode = 2;
                        if (ECR.SetMode() != 0)
                            return;
                        // снимаем отчет
                        ECR.ReportType = 2;
                        if (ECR.Report() != 0)
                            return;

                        // Z - отчет
                        // устанавливаем пароль системного администратора ККМ
                        ECR.Password = "30";
                        // входим в режим отчетов с гашением
                        ECR.Mode = 3;
                        if (ECR.SetMode() != 0)
                            return;
                        // снимаем отчет
                        ECR.ReportType = 1;
                        if (ECR.Report() != 0)
                            return;
            */
            // выходим в режим выбора, чтобы кто-то под введенными паролями не сделал что нибуть нехорошее
            if (ECR.ResetMode() != 0)
                return;

            // освобождаем порт
            ECR.DeviceEnabled = false;
            if (ECR.ResultCode != 0)
                return;

            MessageBox.Show("Все операции успешно выполнены.",
                Application.ProductName,
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }



        /*
                private void Demo()
                {
        // занимаем порт
                    ECR.DeviceEnabled = true;
                    if (ECR.ResultCode != 0)
                        return;

        // получаем состояние ККМ
                    if (ECR.GetStatus() != 0)
                        return;

        // проверяем на всякий случай ККМ на фискализированность
                    if (ECR.Fiscal)
                        if (MessageBox.Show("ККМ фискализирована! Вы действительно хотите продолжить?", 
                                Application.ProductName, 
                                System.Windows.Forms.MessageBoxButtons.YesNo,
                                System.Windows.Forms.MessageBoxIcon.Question) == DialogResult.No)
                                    return;

        // если есть открытый чек, то отменяем его
                    if (ECR.CheckState != 0)
                        if (ECR.CancelCheck() != 0)
                            return;

        // если смена открыта снимаем Z-отчет
                    if (ECR.SessionOpened)
                    {
                        // устанавливаем пароль системного администратора ККМ
                        ECR.Password = "30";
                        // входим в режим отчетов с гашением
                        ECR.Mode = 3;
                        if (ECR.SetMode() != 0)
                            return;
                        // снимаем отчет
                        ECR.ReportType = 1;
                        if (ECR.Report() != 0)
                            return;
                    }

        // входим в режим регистрации
        // устанавливаем пароль кассира
                    ECR.Password = "1";
          // входим в режим регистрации
                    ECR.Mode = 1;
                    if (ECR.SetMode() != 0)
                        return;

        // продажа без сдачи
        // регистрация продажи
                    ECR.Name = "Молоко";
                    ECR.Price = 10.45;
                    ECR.Quantity = 1;
                    ECR.Department = 2;
                    if (ECR.Registration() != 0)
                        return;
        // скидка суммой на предыдущую позицию
                    ECR.Percents = 10;
                    ECR.Destination = 1;
                    if (ECR.PercentsDiscount() != 0)
                        return;
        // регистрация продажи
                    ECR.Name = "Фанта";
                    ECR.Price = 25;
                    ECR.Quantity = 5;
                    ECR.Department = 1;
                    if (ECR.Registration() != 0)
                        return;
        // скидка суммой на весь чек
                    ECR.Summ = 10.40;
                    ECR.Destination = 0;
                    if (ECR.SummDiscount() != 0)
                        return;
        // закрытие чека наличными без ввода полученной от клиента суммы
                    ECR.TypeClose = 0;
                    if (ECR.CloseCheck() != 0)
                        return;

        // продажа со сдачей
        // регистрация продажи
                    ECR.Name = "Молоко";
                    ECR.Price = 10.45;
                    ECR.Quantity = 1;
                    ECR.Department = 2;
                    if (ECR.Registration() != 0)
                        return;
        // регистрация продажи
                    ECR.Name = "Пепси-кола";
                    ECR.Price = 25.00;
                    ECR.Quantity = 5;
                    ECR.Department = 1;
                    if (ECR.Registration() != 0)
                        return;
        // сторно предыдущей регистрации
                    if (ECR.Storno() != 0)
                        return;
        // регистрация продажи
                    ECR.Name = "Фанта";
                    ECR.Price = 25;
                    ECR.Quantity = 5;
                    ECR.Department = 1;
                    if (ECR.Registration() != 0)
                        return;
        // скидка суммой на весь чек
                    ECR.Summ = 50;
                    ECR.Destination = 0;
                    if (ECR.SummDiscount() != 0)
                        return;
        // закрытие чека наличными с вводом полученной от клиента суммы
                    ECR.Summ = 100;
                    ECR.TypeClose = 0;
                    if (ECR.Delivery() != 0)
                        return;

        // аннулирование
        // регистрация аннулирования
                    ECR.Name = "Dirol";
                    ECR.Price = 7;
                    ECR.Quantity = 1;
                    if (ECR.Annulate() != 0)
                        return;
        // регистрация аннулирования
                    ECR.Name = "Orbit";
                    ECR.Price = 8;
                    ECR.Quantity = 2;
                    if (ECR.Annulate() != 0)
                        return;
        // закрытие чека
                    ECR.TypeClose = 0;
                    if (ECR.CloseCheck() != 0)
                        return;

        // возврат
        // регистрация возврата
                    ECR.Name = "Молоко";
                    ECR.Price = 10.45;
                    ECR.Quantity = 1;
                    if (ECR.Return() != 0)
                        return;
        // регистрация возврата
                    ECR.Name = "Колбаса";
                    ECR.Price = 99.99;
                    ECR.Quantity = 1.235;
                    if (ECR.Return() != 0)
                        return;
        // скидка суммой на весь чек
                    ECR.Summ = 50;
                    ECR.Destination = 0;
                    if (ECR.SummDiscount() != 0)
                        return;
        // закрытие чека
                    ECR.TypeClose = 0;
                    if (ECR.CloseCheck() != 0)
                        return;

        // внесение наличности
                    ECR.Summ = 400.50;
                    if (ECR.CashIncome() != 0)
                        return;

        // выплата наличности
                    ECR.Summ = 121.34;
                    if (ECR.CashOutcome() != 0)
                        return;

        // X - отчет
        // устанавливаем пароль администратора ККМ
                    ECR.Password = "29";
        // входим в режим отчетов без гашения
                    ECR.Mode = 2;
                    if (ECR.SetMode() != 0)
                        return;
        // снимаем отчет
                    ECR.ReportType = 2;
                    if (ECR.Report() != 0)
                        return;

        // Z - отчет
        // устанавливаем пароль системного администратора ККМ
                    ECR.Password = "30";
        // входим в режим отчетов с гашением
                    ECR.Mode = 3;
                    if (ECR.SetMode() != 0)
                        return;
        // снимаем отчет
                    ECR.ReportType = 1;
                    if (ECR.Report() != 0)
                        return;

        // выходим в режим выбора, чтобы кто-то под введенными паролями не сделал что нибуть нехорошее
                    if (ECR.ResetMode() != 0)
                        return;

        // освобождаем порт
                    ECR.DeviceEnabled = false;
                    if (ECR.ResultCode != 0)
                        return;

                    MessageBox.Show("Все операции успешно выполнены.",
                        Application.ProductName, 
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
        */
        private void btnStart_Click(object sender, System.EventArgs e)
        {
            btnStart.Enabled = false;
            btnProps.Enabled = false;
            Demo();
            if (ECR.ResultCode != 0)
                MessageBox.Show(
                    "Ошибка ККМ: " + ECR.ResultDescription + "!",
                    Application.ProductName,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            btnStart.Enabled = true;
            btnProps.Enabled = true;

        }

        private void btnProps_Click(object sender, System.EventArgs e)
        {
            ECR.ShowProperties();
        }
    }
}
