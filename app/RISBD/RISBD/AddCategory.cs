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
    public partial class AddCategory : Form
    {
        NpgsqlConnection conn_B;      // подключение к БД B

        /// <summary>
        /// Конструктор формы
        /// </summary>
        /// <param name="_conn_B">Подключение B</param>
        public AddCategory(NpgsqlConnection _conn_B)
        {
            InitializeComponent();
            conn_B = _conn_B;
        }

        /// <summary>
        /// Обновим список категорий
        /// </summary>
        private void button_refresh_Click(object sender, EventArgs e)
        {
            // Создадим новый набор данных
            DataSet datasetmain = new DataSet();
            // Открываем подключение
            if (!MainWindow.open_connection(ref conn_B)) return;    // используются функции из родительской формы !
            // Очищаем набор данных
            datasetmain.Clear();
            // Sql-запрос
            NpgsqlCommand command = new NpgsqlCommand("select * from initial.select_9_1()", conn_B);
            // Новый адаптер нужен для заполнения набора данных
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(command);
            // Заполняем набор данных данными, которые вернул запрос       
            if (!MainWindow.fill_dataSet(da, ref datasetmain, "table1", ref conn_B)) return;
            conn_B.Close();
            dataGrid_results.DataSource = datasetmain;    // Связываем элемент DataGridView1 с набором данных
            dataGrid_results.DataMember = "table1";       // укажем, какую таблицу отображать    
        }

        /// <summary>
        /// Перехват закрытия формы для того, чтобы форма скрывалась, а не уничтожалась
        /// </summary>
        /// <param name="e">Аргументы закрытия формы</param>
        private void AddCategory_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;    // отмена уничтожения
            this.Hide();        // скрытие формы
        }

        static public bool executeNonQuery(NpgsqlCommand command, NpgsqlConnection conn)
        {
            try
            {
                int rows_affected = -command.ExecuteNonQuery();
                MessageBox.Show("Строк изменено: " + rows_affected.ToString(), "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (NpgsqlException e)
            {
                MessageBox.Show("Ошибка:\n" + e.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                conn.Close();
                return false;
            }

            return true;
        }

        /// <summary>
        /// Добавление категории
        /// </summary>
        private void button_insert_Click(object sender, EventArgs e)
        {
            // проверка на пустую строку
            if (string.IsNullOrWhiteSpace(textBox_title.Text))
            {
                MessageBox.Show("Введите название категории!", "Ошибка добавления", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // Открываем подключение
            if (!MainWindow.open_connection(ref conn_B)) return;    // используются функции из родительской формы !
            // Sql-запрос c параметром
            NpgsqlCommand command = new NpgsqlCommand("select * from initial.insert_categories_B(:title)", conn_B);
            command.Parameters.AddWithValue("title", textBox_title.Text);
            if (!executeNonQuery(command, conn_B))  return;                 // попробуем произвести вставку
            conn_B.Close();                                         // закроем соединение
            button_refresh_Click(null, null);                       // обновим таблицу категорий
            
        }

        /// <summary>
        /// Изменение названия категории
        /// </summary>
        private void button_update_Click(object sender, EventArgs e)
        {
            // проверка на пустую строку
            if (string.IsNullOrWhiteSpace(textBox_title.Text))
            {
                MessageBox.Show("Введите новое название категории!", "Ошибка изменения", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            NpgsqlCommand command = new NpgsqlCommand("select * from initial.update_categories_B(:id,:title)", conn_B);
            command.Parameters.AddWithValue("id", id);
            command.Parameters.AddWithValue("title", textBox_title.Text);
            if (!executeNonQuery(command, conn_B)) return;                 // попробуем произвести вставку
            conn_B.Close();                                         // закроем соединение
            button_refresh_Click(null, null);                       // обновим таблицу категорий
        }

        private void button_delete_Click(object sender, EventArgs e)
        {
            // проверка, что категория была выбрана
            if (dataGrid_results.SelectedRows.Count < 1)
            {
                MessageBox.Show("Выберите категорию для удаления", "Ошибка удаления", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // получим id категории
            int id = Convert.ToInt32(dataGrid_results.SelectedRows[0].Cells[0].Value);
            // Открываем подключение
            if (!MainWindow.open_connection(ref conn_B)) return;    // используются функции из родительской формы !
            // Sql-запрос c параметром
            NpgsqlCommand command = new NpgsqlCommand("select * from initial.delete_categories_B(:id)", conn_B);
            command.Parameters.AddWithValue("id", id);
            if (!executeNonQuery(command, conn_B)) return;                 // попробуем произвести вставку
            conn_B.Close();                                         // закроем соединение
            button_refresh_Click(null, null);                       // обновим таблицу категорий
        }
    }
}
