using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;

namespace RISBD
{
    /// <summary>
    /// Класс главного окна
    /// </summary>
    public partial class MainWindow : Form
    {
        NpgsqlConnection conn_A;      // подключение к БД A
        NpgsqlConnection conn_B;      // подключение к БД B
        DataSet dataSetQuery8;        // таблицы из запроса 8 (клиент + его_покупки)
        DataSet dataSetQuery9;        // таблица проданных товаров из некоторой категории ()

        /// <summary>
        /// конструктор
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            // Создадим набор данных для 8 запроса
            dataSetQuery8 = new DataSet();
            dataSetQuery9 = new DataSet();
            // Очищаем набор данных
            dataSetQuery8.Clear();
            dataSetQuery9.Clear();
        }

        private void tableLayoutPanel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tableLayoutPanel14_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dataGridView5_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        /// <summary>
        /// Открыть подключение
        /// </summary>
        /// <param name="conn">Подключение</param>
        /// <returns>Удалось ли открыть</returns>
        private bool open_connection(ref NpgsqlConnection conn)
        {
            try
            {
                conn.Open();
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show("Ошибка:\n" + ex.ToString(), "Ошибка подключения", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Заполнить датасет данными
        /// </summary>
        /// <param name="da">Адаптер</param>
        /// <param name="datasetmain">Датасет</param>
        /// <returns>Удалось ли</returns>
        private bool fill_dataSet(NpgsqlDataAdapter da, ref DataSet datasetmain, string tableName)
        {
            try
            {
                da.Fill(datasetmain, tableName);
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show("Ошибка:\n" + ex.ToString(), "Ошибка подключения", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Зададим строки подключения при загрузке формы
        /// </summary>
        private void MainWindow_Load(object sender, EventArgs e)
        {
            conn_A = new NpgsqlConnection("server=students.ami.nstu.ru; database=risbd4; user Id=risbd4; password=ris14bd4");
            conn_B = new NpgsqlConnection("server=localhost; database=postgres; user Id=test; password=test");
            //button_refresh8_Click(null, null);
        }

        /// <summary>
        /// Поиск продаж на сервере A (запрос 2)
        /// </summary>
        private void button_search2_Click(object sender, EventArgs e)
        {
            // Создадим новый набор данных
            DataSet datasetmain = new DataSet();
            // Открываем подключение
            if (!open_connection(ref conn_A)) return;
            // Очищаем набор данных
            datasetmain.Clear();                                                                                                
            // Sql-запрос, параметризованный
            NpgsqlCommand command = new NpgsqlCommand("select * from initial.select_2(:month_num, :price_to_search)", conn_A);
            command.Parameters.Add(new NpgsqlParameter("month_num", this.numeric_month2.Value));                                
            command.Parameters.Add(new NpgsqlParameter("price_to_search", this.numeric_sum2.Value));
            // Новый адаптер нужен для заполнения набора данных
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(command);
            // Заполняем набор данных данными, которые вернул запрос       
            if (!fill_dataSet(da, ref datasetmain, "table1")) return;
            dataGrid_results2.DataSource = datasetmain;     // Связываем элемент DataGridView1 с набором данных
            dataGrid_results2.DataMember = "table1";
            conn_A.Close();                                 // Закрываем подключение
        }

        private void button_search3_Click(object sender, EventArgs e)
        {
            // Создадим новый набор данных
            DataSet datasetmain = new DataSet();
            // Открываем подключение
            if (!open_connection(ref conn_A)) return;
            // Очищаем набор данных
            datasetmain.Clear();
            // Sql-запрос
            NpgsqlCommand command = new NpgsqlCommand("select * from initial.select_3()", conn_A);
            // Новый адаптер нужен для заполнения набора данных
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(command);
            // Заполняем набор данных данными, которые вернул запрос       
            if (!fill_dataSet(da, ref datasetmain, "table1")) return;
            dataGrid_results3.DataSource = datasetmain;     // Связываем элемент DataGridView1 с набором данных
            dataGrid_results3.DataMember = "table1";
            conn_A.Close();                                 // Закрываем подключение
        }

        private void button_search4_Click(object sender, EventArgs e)
        {
            // Создадим новый набор данных
            DataSet datasetmain = new DataSet();
            // Открываем подключение
            if (!open_connection(ref conn_A)) return;
            // Очищаем набор данных
            datasetmain.Clear();
            // Sql-запрос
            NpgsqlCommand command = new NpgsqlCommand("select * from initial.select_4()", conn_A);
            // Новый адаптер нужен для заполнения набора данных
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(command);
            // Заполняем набор данных данными, которые вернул запрос       
            if (!fill_dataSet(da, ref datasetmain, "table1")) return;
            dataGrid_results4.DataSource = datasetmain;     // Связываем элемент DataGridView1 с набором данных
            dataGrid_results4.DataMember = "table1";
            conn_A.Close();                                 // Закрываем подключение
        }

        private void button_search5_Click(object sender, EventArgs e)
        {
            // Создадим новый набор данных
            DataSet datasetmain = new DataSet();
            // Открываем подключение
            if (!open_connection(ref conn_A)) return;
            // Очищаем набор данных
            datasetmain.Clear();
            // Sql-запрос, параметризованный
            NpgsqlCommand command = new NpgsqlCommand("select * from initial.select_5(:month_num)", conn_A);
            command.Parameters.Add(new NpgsqlParameter("month_num", this.numeric_month5.Value));  
            // Новый адаптер нужен для заполнения набора данных
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(command);
            // Заполняем набор данных данными, которые вернул запрос       
            if (!fill_dataSet(da, ref datasetmain, "table1")) return;
            dataGrid_results5.DataSource = datasetmain;     // Связываем элемент DataGridView1 с набором данных
            dataGrid_results5.DataMember = "table1";
            conn_A.Close();                                 // Закрываем подключение
        }

        private void button_search6_Click(object sender, EventArgs e)
        {
            // Создадим новый набор данных
            DataSet datasetmain = new DataSet();
            // Открываем подключение
            if (!open_connection(ref conn_B)) return;
            // Очищаем набор данных
            datasetmain.Clear();
            // Sql-запрос, параметризованный
            NpgsqlCommand command = new NpgsqlCommand("select * from initial.select_6(:month_num, :price_to_search)", conn_B);
            command.Parameters.Add(new NpgsqlParameter("month_num", this.numeric_month6.Value));
            command.Parameters.Add(new NpgsqlParameter("price_to_search", this.numeric_sum6.Value));
            // Новый адаптер нужен для заполнения набора данных
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(command);
            // Заполняем набор данных данными, которые вернул запрос       
            if (!fill_dataSet(da, ref datasetmain, "table1")) return;
            dataGrid_results6.DataSource = datasetmain;     // Связываем элемент DataGridView1 с набором данных
            dataGrid_results6.DataMember = "table1";
            conn_B.Close();                                 // Закрываем подключение
        }

        private void button_search7_Click(object sender, EventArgs e)
        {
            // Создадим новый набор данных
            DataSet datasetmain = new DataSet();
            // Открываем подключение
            if (!open_connection(ref conn_B)) return;
            // Очищаем набор данных
            datasetmain.Clear();
            // Sql-запрос
            NpgsqlCommand command = new NpgsqlCommand("select * from initial.select_7()", conn_B);
            // Новый адаптер нужен для заполнения набора данных
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(command);
            // Заполняем набор данных данными, которые вернул запрос       
            if (!fill_dataSet(da, ref datasetmain, "table1")) return;
            dataGrid_results7.DataSource = datasetmain;     // Связываем элемент DataGridView1 с набором данных
            dataGrid_results7.DataMember = "table1";
            conn_B.Close();                                 // Закрываем подключение
        }

        private void button_refresh8_Click(object sender, EventArgs e)
        {
            // Создадим новый набор данных
            DataSet datasetmain = new DataSet();
            // Открываем подключение
            if (!open_connection(ref conn_B)) return;
            // Очищаем набор данных
            datasetmain.Clear();
            // Sql-запрос
            NpgsqlCommand command = new NpgsqlCommand("select * from initial.select_8_1()", conn_B);
            // Новый адаптер нужен для заполнения набора данных
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(command);
            // Заполняем набор данных данными, которые вернул запрос       
            if (!fill_dataSet(da, ref datasetmain, "table1")) return;
            conn_B.Close();        
            comboBox_clients8.DataSource = datasetmain.Tables["table1"];    // Связываем элемент DataGridView1 с набором данных
            comboBox_clients8.DisplayMember = "info";       // элемент отображения для Комбо
            comboBox_clients8.ValueMember = "id";           // возвращаемый Комбо элемент
            //try { comboBox_clients8.SelectedIndex = 0; }    // выберем первый элемент (если список не пуст)
            //catch (Exception ex) { return; }
            comboBox_clients8_SelectedIndexChanged(null, null); // появились новые элементы => индекс изменился
        }

        private void button_search8_Click(object sender, EventArgs e)
        {
            // если клиенты не подгружены
            if (comboBox_clients8.SelectedValue == null)
                return;
            // Открываем подключение
            if (!open_connection(ref conn_B)) return;
            // Sql-запрос, параметризованный
            NpgsqlCommand command = new NpgsqlCommand("select * from initial.select_8_3(:client_id, :beg_date, :end_date)", conn_B);
            command.Parameters.Add(new NpgsqlParameter("client_id", this.comboBox_clients8.SelectedValue));
            command.Parameters.Add(new NpgsqlParameter("beg_date", this.dateTime_from8.Value));
            command.Parameters.Add(new NpgsqlParameter("end_date", this.dateTime_to8.Value));
            // Новый адаптер нужен для заполнения набора данных
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(command);
            // Заполняем набор данных данными, которые вернул запрос       
            if (!fill_dataSet(da, ref dataSetQuery8, "Orders")) return;
            
            dataGrid_results8.DataSource = dataSetQuery8;     // Связываем элемент DataGridView1 с набором данных
            dataGrid_results8.DataMember = "Orders";
            conn_B.Close();  
        }

        private void comboBox_clients8_SelectedIndexChanged(object sender, EventArgs e)
        {
            // если клиенты не подгружены
            if (comboBox_clients8.SelectedValue == null || comboBox_clients8.SelectedValue == DBNull.Value)
                return;
            // если клиенты не полгружены (т.е. индекс нельзя превратить в число)
            try{Convert.ToInt32(comboBox_clients8.SelectedValue);}
            catch(Exception err) {return;}
            //MessageBox.Show(comboBox_clients8.SelectedValue.ToString(), "comboBox_clients8.SelectedValue.ToString()", MessageBoxButtons.OK, MessageBoxIcon.Information);
            // очистим датасет
            dataSetQuery8.Clear();
            // Открываем подключение
            if (!open_connection(ref conn_B)) return;
            // Sql-запрос, параметризованный
            NpgsqlCommand command = new NpgsqlCommand("select * from initial.select_8_2(:client_id)", conn_B);
            command.Parameters.Add(new NpgsqlParameter("client_id", Convert.ToInt32(this.comboBox_clients8.SelectedValue.ToString())));
            // Новый адаптер нужен для заполнения набора данных
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(command);
            // Заполняем набор данных данными, которые вернул запрос       
            if (!fill_dataSet(da, ref dataSetQuery8, "Client")) return;
            dataGrid_client8.DataSource = dataSetQuery8;     // Связываем элемент DataGridView1 с набором данных
            dataGrid_client8.DataMember = "Client";
            conn_B.Close();  
        }

        private void button_search9_Click(object sender, EventArgs e)
        {
            // если категории не подгружены
            if (comboBox_category9.SelectedValue == null)
                return;
            // Открываем подключение
            if (!open_connection(ref conn_B)) return;
            dataSetQuery9.Clear();
            // Sql-запрос, параметризованный
            NpgsqlCommand command = new NpgsqlCommand("select * from initial.select_9_2(:category_id, :date)", conn_B);
            command.Parameters.Add(new NpgsqlParameter("category_id", this.comboBox_category9.SelectedValue));
            command.Parameters.Add(new NpgsqlParameter("date", this.dateTime_onSaleDate9.Value));
            // Новый адаптер нужен для заполнения набора данных
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(command);
            // Заполняем набор данных данными, которые вернул запрос       
            if (!fill_dataSet(da, ref dataSetQuery9, "Sales")) return;

            dataGrid_results9.DataSource = dataSetQuery9;     // Связываем элемент DataGridView1 с набором данных
            dataGrid_results9.DataMember = "Sales";
            conn_B.Close();  
        }

        private void button_refresh9_Click(object sender, EventArgs e)
        {
            // Создадим новый набор данных
            DataSet datasetmain = new DataSet();
            // Открываем подключение
            if (!open_connection(ref conn_B)) return;
            // Очищаем набор данных
            datasetmain.Clear();
            // Sql-запрос
            NpgsqlCommand command = new NpgsqlCommand("select * from initial.select_9_1()", conn_B);
            // Новый адаптер нужен для заполнения набора данных
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(command);
            // Заполняем набор данных данными, которые вернул запрос       
            if (!fill_dataSet(da, ref datasetmain, "table1")) return;
            conn_B.Close();
            comboBox_category9.DataSource = datasetmain.Tables["table1"];    // Связываем элемент DataGridView1 с набором данных
            comboBox_category9.DisplayMember = "category";       // элемент отображения для Комбо
            comboBox_category9.ValueMember = "id";               // возвращаемый Комбо элемент
            //comboBox_clients8_SelectedIndexChanged(null, null); // появились новые элементы => индекс изменился
        }


    }
}
