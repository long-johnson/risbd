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
    public partial class AddClient : Form
    {
        NpgsqlConnection conn_A;      // подключение к БД A
        NpgsqlConnection conn_B;      // подключение к БД B

        /// <summary>
        /// Конструктор формы
        /// </summary>
        /// <param name="_conn_A">Подключение A</param>
        /// <param name="_conn_B">Подключение B</param>
        public AddClient(NpgsqlConnection _conn_A, NpgsqlConnection _conn_B)
        {
            InitializeComponent();
            conn_A = _conn_A;
            conn_B = _conn_B;
        }

        /// <summary>
        /// Перехват закрытия формы для того, чтобы форма скрывалась, а не уничтожалась
        /// </summary>
        /// <param name="e">Аргументы закрытия формы</param>
        private void AddClient_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;    // отмена уничтожения
            this.Hide();        // скрытие формы
        }

        /// <summary>
        /// Получим список всех клиентов
        /// </summary>
        private void button_refresh_Click(object sender, EventArgs e)
        {
            // Создадим новый набор данных
            DataSet datasetmain = new DataSet();
            // Открываем подключение
            if (!MainWindow.open_connection(ref conn_A)) return;    // используются функции из родительской формы !
            // Очищаем набор данных
            datasetmain.Clear();
            // Sql-запрос
            NpgsqlCommand command = new NpgsqlCommand("select * from initial.select_all_customers_A()", conn_A);
            // Новый адаптер нужен для заполнения набора данных
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(command);
            // Заполняем набор данных данными, которые вернул запрос       
            if (!MainWindow.fill_dataSet(da, ref datasetmain, "table1", ref conn_A)) return;
            conn_A.Close();
            dataGrid_results.DataSource = datasetmain;    // Связываем элемент DataGridView1 с набором данных
            dataGrid_results.DataMember = "table1";       // укажем, какую таблицу отображать   
        }

        /// <summary>
        /// Вставим нового клиента
        /// </summary>
        private void button_insert_Click(object sender, EventArgs e)
        {
            // проверка на пустую строку
            if (string.IsNullOrWhiteSpace(textBox_surname.Text) || string.IsNullOrWhiteSpace(textBox_name.Text) || string.IsNullOrWhiteSpace(textBox_patronymic.Text)
                || string.IsNullOrWhiteSpace(textBox_phone.Text) || string.IsNullOrWhiteSpace(textBox_email.Text) || string.IsNullOrWhiteSpace(textBox_series.Text)
                || string.IsNullOrWhiteSpace(textBox_number.Text) || string.IsNullOrWhiteSpace(textBox_issuedBy.Text) || string.IsNullOrWhiteSpace(textBox_address.Text))
            {
                MessageBox.Show("Не все поля заполнены", "Ошибка добавления", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // Открываем подключение
            if (!MainWindow.open_connection(ref conn_B)) return;    // используются функции из родительской формы !
            // Sql-запрос c параметром
            NpgsqlCommand command =
                new NpgsqlCommand("select * from initial.insert_clients_B(:surname,:name,:patronymic,:birthday,:phone,:email,:address,:series,:number,:issueDate,:issuedBy)", conn_B);
            command.Parameters.AddWithValue("surname", textBox_surname.Text);
            command.Parameters.AddWithValue("name", textBox_name.Text);
            command.Parameters.AddWithValue("patronymic", textBox_patronymic.Text);
            command.Parameters.AddWithValue("birthday", dateTime_birthday.Value);
            command.Parameters.AddWithValue("phone", textBox_phone.Text);
            command.Parameters.AddWithValue("email", textBox_email.Text);
            command.Parameters.AddWithValue("address", textBox_address.Text);
            command.Parameters.AddWithValue("series", textBox_series.Text);
            command.Parameters.AddWithValue("number", textBox_number.Text);
            command.Parameters.AddWithValue("issueDate", dateTime_issueDate.Value);
            command.Parameters.AddWithValue("issuedBy", textBox_issuedBy.Text);
            if (!AddCategory.executeNonQuery(command, conn_B)) return;  // попробуем произвести вставку
            conn_B.Close();                                             // закроем соединение
            //button_refresh_Click(null, null);                         // обновим таблицу категорий
        }

        /// <summary>
        /// Изменим клиента по ID
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_update_Click(object sender, EventArgs e)
        {
            // проверка на пустую строку
            if (string.IsNullOrWhiteSpace(textBox_surname.Text) || string.IsNullOrWhiteSpace(textBox_name.Text) || string.IsNullOrWhiteSpace(textBox_patronymic.Text)
                || string.IsNullOrWhiteSpace(textBox_phone.Text) || string.IsNullOrWhiteSpace(textBox_email.Text) || string.IsNullOrWhiteSpace(textBox_series.Text)
                || string.IsNullOrWhiteSpace(textBox_number.Text) || string.IsNullOrWhiteSpace(textBox_issuedBy.Text) || string.IsNullOrWhiteSpace(textBox_address.Text))
            {
                MessageBox.Show("Не все поля заполнены", "Ошибка добавления", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // проверка, что категория была выбрана
            if (dataGrid_results.SelectedRows.Count < 1)
            {
                MessageBox.Show("Выберите категорию для изменения", "Ошибка изменения", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // получим id категории
            int id = Convert.ToInt32(dataGrid_results.SelectedRows[0].Cells[0].Value);
            // Открываем подключение
            if (!MainWindow.open_connection(ref conn_B)) return;    // используются функции из родительской формы !
            // Sql-запрос c параметром
            NpgsqlCommand command =
                new NpgsqlCommand("select * from initial.update_clients_B(:id, :surname,:name,:patronymic,:birthday,:phone,:email,:address,:series,:number,:issueDate,:issuedBy)", conn_B);
            command.Parameters.AddWithValue("id", id);
            command.Parameters.AddWithValue("surname", textBox_surname.Text);
            command.Parameters.AddWithValue("name", textBox_name.Text);
            command.Parameters.AddWithValue("patronymic", textBox_patronymic.Text);
            command.Parameters.AddWithValue("birthday", dateTime_birthday.Value);
            command.Parameters.AddWithValue("phone", textBox_phone.Text);
            command.Parameters.AddWithValue("email", textBox_email.Text);
            command.Parameters.AddWithValue("address", textBox_address.Text);
            command.Parameters.AddWithValue("series", textBox_series.Text);
            command.Parameters.AddWithValue("number", textBox_number.Text);
            command.Parameters.AddWithValue("issueDate", dateTime_issueDate.Value);
            command.Parameters.AddWithValue("issuedBy", textBox_issuedBy.Text);
            if (!AddCategory.executeNonQuery(command, conn_B)) return;  // попробуем произвести вставку
            conn_B.Close();                                             // закроем соединение
            //button_refresh_Click(null, null);                         // обновим таблицу категорий
        }

        private void button_delete_Click(object sender, EventArgs e)
        {
            // проверка, что категория была выбрана
            if (dataGrid_results.SelectedRows.Count < 1)
            {
                MessageBox.Show("Выберите клиента для изменения", "Ошибка удаления", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // получим id категории
            int id = Convert.ToInt32(dataGrid_results.SelectedRows[0].Cells[0].Value);
            // Открываем подключение
            if (!MainWindow.open_connection(ref conn_B)) return;    // используются функции из родительской формы !
            // Sql-запрос c параметром
            NpgsqlCommand command = new NpgsqlCommand("select * from initial.delete_clients_B(:id)", conn_B);
            command.Parameters.AddWithValue("id", id);
            if (!AddCategory.executeNonQuery(command, conn_B)) return;                 // попробуем произвести вставку
            conn_B.Close();                                             // закроем соединение
            //button_refresh_Click(null, null);                          // обновим таблицу категорий
        }
    }
}
