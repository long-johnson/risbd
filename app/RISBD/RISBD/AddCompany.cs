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
    public partial class AddCompany : Form
    {
        NpgsqlConnection conn_A;      // подключение к БД A
        NpgsqlConnection conn_B;      // подключение к БД B

        /// <summary>
        /// Конструктор формы
        /// </summary>
        /// <param name="_conn_A">Подключение A</param>
        /// <param name="_conn_B">Подключение B</param>
        public AddCompany(NpgsqlConnection _conn_A, NpgsqlConnection _conn_B)
        {
            InitializeComponent();
            conn_A = _conn_A;
            conn_B = _conn_B;
        }

        /// <summary>
        /// Заполним таблицу компаний, а также получим список стран в комбобокс
        /// </summary>
        private void button_refresh_Click(object sender, EventArgs e)
        {
            // Создадим новый набор данных
            DataSet datasetmain = new DataSet();
            NpgsqlCommand command;
            NpgsqlDataAdapter da;
            // СТРАНЫ (если кнопка нажимается физически, а не вызывается другой функцией)
            if (e != null) 
            { 
                // Открываем подключение
                if (!MainWindow.open_connection(ref conn_A)) return;
                // Очищаем набор данных
                datasetmain.Clear();
                // Sql-запрос для получения списка стран
                command = new NpgsqlCommand("select * from initial.select_all_countries_A()", conn_A);
                // Новый адаптер нужен для заполнения набора данных
                da = new NpgsqlDataAdapter(command);
                // Заполняем набор данных данными, которые вернул запрос       
                if (!MainWindow.fill_dataSet(da, ref datasetmain, "countries", ref conn_A)) return;
                comboBox_countries.DataSource = datasetmain.Tables["countries"];    // Связываем элемент DataGridView1 с набором данных
                comboBox_countries.DisplayMember = "name";           // элемент отображения для Комбо
                comboBox_countries.ValueMember = "id";               // возвращаемый Комбо элемент
            }

            // КОМПАНИИ
            // Sql-запрос
            command = new NpgsqlCommand("select * from initial.select_all_companies_A();", conn_A);
            // Новый адаптер нужен для заполнения набора данных
            da = new NpgsqlDataAdapter(command);
            // Заполняем набор данных данными, которые вернул запрос       
            if (!MainWindow.fill_dataSet(da, ref datasetmain, "companies", ref conn_A)) return;
            conn_A.Close();
            dataGrid_results.DataSource = datasetmain;    // Связываем элемент DataGridView1 с набором данных
            dataGrid_results.DataMember = "companies";    // укажем, какую таблицу отображать   
        }

        /// <summary>
        /// Перехват закрытия формы для того, чтобы форма скрывалась, а не уничтожалась
        /// </summary>
        /// <param name="e">Аргументы закрытия формы</param>
        private void AddCompany_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;    // отмена уничтожения
            this.Hide();        // скрытие формы
        }

        /// <summary>
        /// Добавление компании
        /// </summary>
        private void button_insert_Click(object sender, EventArgs e)
        {
            // проверка на пустую строку
            if (string.IsNullOrWhiteSpace(textBox_name.Text) || string.IsNullOrWhiteSpace(textBox_address.Text) || string.IsNullOrWhiteSpace(textBox_bankAccounts.Text)
                || string.IsNullOrWhiteSpace(textBox_phone.Text) || string.IsNullOrWhiteSpace(textBox_fullName.Text))
            {
                MessageBox.Show("Не все поля заполнены", "Ошибка добавления", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // проверка, выбран ли элемент в comboBox
            if (comboBox_countries.Items.Count == 0)
            {
                MessageBox.Show("Не выбрана страна", "Ошибка добавления", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            int country_id;
            try
            {
                country_id = Convert.ToInt32(comboBox_countries.SelectedValue);
            }
            catch (Exception)
            {
                MessageBox.Show("Не выбрана страна", "Ошибка добавления", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // Открываем подключение
            if (!MainWindow.open_connection(ref conn_B)) return;    // используются функции из родительской формы !
            // Sql-запрос c параметром
            NpgsqlCommand command =
                new NpgsqlCommand("select * from initial.insert_companies_B(:name, :country_id, :head_full_name, :phone, :address, :bank_details)", conn_B);
            command.Parameters.AddWithValue("name", textBox_name.Text);
            command.Parameters.AddWithValue("country_id", country_id);
            command.Parameters.AddWithValue("head_full_name", textBox_fullName.Text);
            command.Parameters.AddWithValue("phone", textBox_phone.Text);
            command.Parameters.AddWithValue("address", textBox_address.Text);
            command.Parameters.AddWithValue("bank_details", textBox_bankAccounts.Text);
            if (!AddCategory.executeNonQuery(command, conn_B)) return;  // попробуем произвести вставку
            conn_B.Close();                                             // закроем соединение
            button_refresh_Click(null, null);                         // обновим таблицу категорий
        }

        /// <summary>
        /// Изменение компании
        /// </summary>
        private void button_update_Click(object sender, EventArgs e)
        {
            // проверка на пустую строку
            if (string.IsNullOrWhiteSpace(textBox_name.Text) || string.IsNullOrWhiteSpace(textBox_address.Text) || string.IsNullOrWhiteSpace(textBox_bankAccounts.Text)
                || string.IsNullOrWhiteSpace(textBox_phone.Text) || string.IsNullOrWhiteSpace(textBox_fullName.Text))
            {
                MessageBox.Show("Не все поля заполнены", "Ошибка добавления", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // проверка, выбран ли элемент в comboBox
            if (comboBox_countries.Items.Count == 0)
            {
                MessageBox.Show("Не выбрана страна", "Ошибка добавления", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            int country_id;
            try
            {
                country_id = Convert.ToInt32(comboBox_countries.SelectedValue);
            }
            catch (Exception)
            {
                MessageBox.Show("Не выбрана страна", "Ошибка добавления", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // проверка, что компания была выбрана
            if (dataGrid_results.SelectedRows.Count < 1)
            {
                MessageBox.Show("Выберите категорию для изменения", "Ошибка изменения", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // получим id компании
            int id = Convert.ToInt32(dataGrid_results.SelectedRows[0].Cells[0].Value);
            // Открываем подключение
            if (!MainWindow.open_connection(ref conn_B)) return;    // используются функции из родительской формы !
            // Sql-запрос c параметром
            NpgsqlCommand command =
                new NpgsqlCommand("select * from initial.update_companies_B(:id, :name, :country_id, :head_full_name, :phone, :address, :bank_details)", conn_B);
            command.Parameters.AddWithValue("id", id);
            command.Parameters.AddWithValue("name", textBox_name.Text);
            command.Parameters.AddWithValue("country_id", country_id);
            command.Parameters.AddWithValue("head_full_name", textBox_fullName.Text);
            command.Parameters.AddWithValue("phone", textBox_phone.Text);
            command.Parameters.AddWithValue("address", textBox_address.Text);
            command.Parameters.AddWithValue("bank_details", textBox_bankAccounts.Text);
            if (!AddCategory.executeNonQuery(command, conn_B)) return;  // попробуем произвести вставку
            conn_B.Close();                                             // закроем соединение
            button_refresh_Click(null, null);                         // обновим таблицу категорий
        }

        private void button_delete_Click(object sender, EventArgs e)
        {
            // проверка, что категория была выбрана
            if (dataGrid_results.SelectedRows.Count < 1)
            {
                MessageBox.Show("Выберите компанию для изменения", "Ошибка удаления", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // получим id категории
            int id = Convert.ToInt32(dataGrid_results.SelectedRows[0].Cells[0].Value);
            // Открываем подключение
            if (!MainWindow.open_connection(ref conn_B)) return;    // используются функции из родительской формы !
            // Sql-запрос c параметром
            NpgsqlCommand command = new NpgsqlCommand("select * from initial.delete_companies_B(:id)", conn_B);
            command.Parameters.AddWithValue("id", id);
            if (!AddCategory.executeNonQuery(command, conn_B)) return;                 // попробуем произвести вставку
            conn_B.Close();                                             // закроем соединение
            button_refresh_Click(null, null);                          // обновим таблицу компаний
        }


    }
}
