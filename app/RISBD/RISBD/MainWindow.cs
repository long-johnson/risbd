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
/// Ссылки на классы работы с Word и Excel документами
using Word = Microsoft.Office.Interop.Word;
using Excel = Microsoft.Office.Interop.Excel;

using System.Runtime.InteropServices;

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

        AddCategory formAddCategory;  // форма манипуляций с категориями
        AddClient formAddClient;      // форма манипуляций с клиентами
        AddCompany formAddCompany;    // форма манипуляций с компаниями

        // подключение библиотеки
        [DllImport("lib.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "check12")]
        // проверка, лежит ли число в диапазоне 1-12
        public static extern int check12(int t);

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
            // задаим строки подключения
            conn_A = new NpgsqlConnection("server=students.ami.nstu.ru; database=risbd4; user Id=risbd4; password=ris14bd4");
            conn_B = new NpgsqlConnection("server=localhost; database=postgres; user Id=test; password=test");
            formAddCategory = new AddCategory(conn_B);
            formAddClient = new AddClient(conn_A, conn_B);
            formAddCompany = new AddCompany(conn_A, conn_B);
           // formAddCategory.Show();
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
        static public bool open_connection(ref NpgsqlConnection conn)
        {
            try
            {
                conn.Open();
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show("Ошибка:\n" + ex.Message, "Ошибка подключения", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка:\n" + ex.Message, "Неизвестная ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
        static public bool fill_dataSet(NpgsqlDataAdapter da, ref DataSet datasetmain, string tableName, ref NpgsqlConnection conn)
        {
            try
            {
                da.Fill(datasetmain, tableName);
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show("Ошибка:\n" + ex.Message, "Ошибка подключения", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                conn.Close();
                return false;
            }
            return true;
        }

        /// <summary>
        /// Зададим строки подключения при загрузке формы
        /// </summary>
        private void MainWindow_Load(object sender, EventArgs e)
        {
            //button_refresh8_Click(null, null);
        }

        /// <summary>
        /// Поиск продаж на сервере A (запрос 2)
        /// </summary>
        private void button_search2_Click(object sender, EventArgs e)
        {
            // проверка на то, что месяц лежит в 1-12
            //if (check12(Convert.ToInt32(numeric_month2.Value)) == 0)
                //return;
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
            if (!fill_dataSet(da, ref datasetmain, "table1", ref conn_A)) return;
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
            if (!fill_dataSet(da, ref datasetmain, "table1", ref conn_A)) return;
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
            if (!fill_dataSet(da, ref datasetmain, "table1", ref conn_A)) return;
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
            if (!fill_dataSet(da, ref datasetmain, "table1", ref conn_A)) return;
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
            if (!fill_dataSet(da, ref datasetmain, "table1", ref conn_B)) return;
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
            if (!fill_dataSet(da, ref datasetmain, "table1", ref conn_B)) return;
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
            if (!fill_dataSet(da, ref datasetmain, "table1", ref conn_B)) return;
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
            if (!fill_dataSet(da, ref dataSetQuery8, "Orders", ref conn_B)) return;
            
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
            if (!fill_dataSet(da, ref dataSetQuery8, "Client", ref conn_B)) return;
            dataGrid_client8.DataSource = dataSetQuery8;     // Связываем элемент DataGridView1 с набором данных
            dataGrid_client8.DataMember = "Client";
            conn_B.Close();  
        }

        /// <summary>
        /// Поиск товаров, проданных из выбранной категории
        /// </summary>
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
            if (!fill_dataSet(da, ref dataSetQuery9, "Sales", ref conn_B)) return;

            dataGrid_results9.DataSource = dataSetQuery9;     // Связываем элемент DataGridView1 с набором данных
            dataGrid_results9.DataMember = "Sales";
            conn_B.Close();  
        }

        /// <summary>
        /// Загрузить данные в список категорий
        /// </summary>
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
            if (!fill_dataSet(da, ref datasetmain, "table1", ref conn_B)) return;
            conn_B.Close();
            comboBox_category9.DataSource = datasetmain.Tables["table1"];    // Связываем элемент DataGridView1 с набором данных
            comboBox_category9.DisplayMember = "category";       // элемент отображения для Комбо
            comboBox_category9.ValueMember = "id";               // возвращаемый Комбо элемент
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void категорииToolStripMenuItem_Click(object sender, EventArgs e)
        {
            formAddCategory.Show();
        }

        private void клиентыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            formAddClient.Show();
        }

        private void компанииToolStripMenuItem_Click(object sender, EventArgs e)
        {
            formAddCompany.Show();
        }

        /// <summary>
        /// Экспорт документа в Ворд документ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_export_word8_Click(object sender, EventArgs e)
        {
            ExportWord(ref dataSetQuery8, dateTime_from8.Value, dateTime_to8.Value);
        }

        static private void ExportWord(ref DataSet dataSetQuery, DateTime from, DateTime to)
        {
            //Если данных получено не было или если их недостаточно, выходим
            if (dataSetQuery.Tables.Count < 2 || dataSetQuery.Tables[0].Rows.Count < 1)
                return;
            //Создаём экземпляр приложения
            Word.Application wordapp = new Word.Application();
            //Делаем его видимым
            //Если не хотим отображать окно, можно закомментировать
            //Тогда появится только диалог сохранения
            wordapp.Visible = true;
            //Создаём видимый пустой документ
            Word.Document doc = wordapp.Documents.Add(DocumentType: Word.WdNewDocumentType.wdNewBlankDocument, Visible: true);
            //Активируем его редактирование
            doc.Activate();
            //Изменяем ширину левого и правого полей
            doc.Content.ParagraphFormat.LeftIndent = doc.Content.Application.CentimetersToPoints((float)-2);
            doc.Content.ParagraphFormat.RightIndent = doc.Content.Application.CentimetersToPoints((float)1);
            //переходим в конец данных
            wordapp.Selection.EndKey(Unit: Word.WdUnits.wdStory, Extend: Word.WdMovementType.wdMove);
            //Изменяем размер и жирность шрифта
            wordapp.Selection.Font.Size = 16;
            wordapp.Selection.Font.Bold = 1;
            //Выводим заголовок
            wordapp.Selection.TypeText("Список Совершенных сделок от " + from.ToShortDateString() + " до " + to.ToShortDateString());
            //Вставляем параграф
            wordapp.Selection.InsertParagraph();
            //Переходим в конец документа
            wordapp.Selection.EndKey(Unit: Word.WdUnits.wdStory, Extend: Word.WdMovementType.wdMove);
            //Уменьшаем шрифт и выключаем жирность
            wordapp.Selection.Font.Size = 12;
            wordapp.Selection.Font.Bold = 0;
            //Добавляем текст
            wordapp.Selection.TypeText("Уважаемый " + dataSetQuery.Tables[0].Rows[0][2].ToString() + " " + dataSetQuery.Tables[0].Rows[0][3].ToString() + "!\n\rС глубочайшим уважением сообщаем, что для клиента");
            //Параграф
            wordapp.Selection.InsertParagraph();
            //Создаём таблицу из 3 столбцов с одной строкой, с автоматической подгонкой ширины столбцов под размер данных
            Word.Table wordtable = doc.Tables.Add(Range: wordapp.Selection.Range, NumColumns: 3, NumRows: 1, DefaultTableBehavior: Word.WdDefaultTableBehavior.wdWord9TableBehavior, AutoFitBehavior: Word.WdAutoFitBehavior.wdAutoFitContent);
            //Вставляем данные
            //ID
            wordtable.Cell(1, 1).Range.Text = "ID " + dataSetQuery.Tables[0].Rows[0][0].ToString();
            //ФИО
            wordtable.Cell(1, 2).Range.Text = dataSetQuery.Tables[0].Rows[0][1].ToString() + " " + dataSetQuery.Tables[0].Rows[0][2].ToString() + " " + dataSetQuery.Tables[0].Rows[0][3].ToString();
            //Дата рождения
            wordtable.Cell(1, 3).Range.Text = Convert.ToDateTime(dataSetQuery.Tables[0].Rows[0][4]).ToShortDateString();
            //Перемещаем выделение в конец документа
            wordapp.Selection.EndKey(Unit: Word.WdUnits.wdStory, Extend: Word.WdMovementType.wdMove);
            //Снова параграф
            wordapp.Selection.InsertParagraphAfter();
            //Перемещаем выделение в конец документа
            wordapp.Selection.EndKey(Unit: Word.WdUnits.wdStory, Extend: Word.WdMovementType.wdMove);
            //Добавляем текст
            wordapp.Selection.TypeText(" были совершены следующие сделки в числе " + dataSetQuery.Tables[0].Rows.Count + " единиц\n");
            wordapp.Selection.InsertParagraph();
            wordapp.Selection.EndKey(Unit: Word.WdUnits.wdStory, Extend: Word.WdMovementType.wdMove);
            //Добавляем таблицу о шести столбцах
            wordtable = doc.Tables.Add(Range: wordapp.Selection.Range, NumColumns: 6, NumRows: dataSetQuery.Tables[1].Rows.Count + 1, DefaultTableBehavior: Word.WdDefaultTableBehavior.wdWord9TableBehavior, AutoFitBehavior: Word.WdAutoFitBehavior.wdAutoFitContent);
            //Вставляем заголовки
            wordtable.Cell(1, 1).Range.Text = "Дата сделки";
            wordtable.Cell(1, 2).Range.Text = "Категория";
            wordtable.Cell(1, 3).Range.Text = "Компания";
            wordtable.Cell(1, 4).Range.Text = "Модель";
            wordtable.Cell(1, 5).Range.Text = "Количество";
            wordtable.Cell(1, 6).Range.Text = "Метод оплаты";
            //Со второй строки
            int i = 2;
            //Вставляем данные
            foreach (DataRow row in dataSetQuery.Tables[1].Rows)
            {
                wordtable.Cell(i, 1).Range.Text = Convert.ToDateTime(row[0]).ToShortDateString();
                wordtable.Cell(i, 2).Range.Text = row[1].ToString();
                wordtable.Cell(i, 3).Range.Text = row[2].ToString();
                wordtable.Cell(i, 4).Range.Text = row[3].ToString();
                wordtable.Cell(i, 5).Range.Text = row[4].ToString();
                wordtable.Cell(i, 6).Range.Text = row[5].ToString();
                ++i;
            }
            //i = check12(i);
            //Пробуем сохранить на диске
            try
            {
                doc.Save();
            }
            catch (Exception) { };
        }
        //private void ExportExcel()

        private void button_export_excel9_Click(object sender, EventArgs e)
        {
            if (comboBox_category9.SelectedIndex == -1)
                return;
            ExportExcel(ref dataSetQuery9, comboBox_category9.Text, dateTime_onSaleDate9.Value);
        }

        private void ExportExcel(ref DataSet salesData, String category, DateTime date)
        {
            //Если не было получено данных, выходим
            if (salesData.Tables.Count < 1)
                return;
            //Создаём экземпляр приложения
            Excel.Application app = new Excel.Application();
            //Отображаем его окно
            app.Visible = true;
            //Устанавливаем число страниц в новых книгах
            app.SheetsInNewWorkbook = 1;
            //Создаём книгу
            Excel.Workbook book = app.Workbooks.Add();
            //Получаем первую страницу книги
            Excel.Worksheet sheet = book.Worksheets.get_Item(1);
            //Переименовываем страницу
            sheet.Name = "Результат";
            //Выбираем первые 9 столбцов первой строки
            Excel.Range cells = sheet.get_Range("A1", "I1");
            //Объединяем ячейки
            cells.Merge();
            //Задаем выравнивание по центру
            cells.HorizontalAlignment = Excel.Constants.xlCenter;
            //Выводим Заголовок
            cells.Value2 = "Таблица результатов для категории " + category + " за " + date.Date.ToShortDateString() + " с умопомрачительным оформлением";
            //Выбираем первые 9 столбцов второй строки, повторяем действия
            cells = sheet.get_Range("A2", "I2");
            cells.Merge();
            cells.HorizontalAlignment = Excel.Constants.xlCenter;
            //Устанавливаем цвет в 47 предустановленное значение
            cells.Font.ColorIndex = 47;
            //Выводим саркастический подзаголовок
            cells.Value2 = "(в моих мечтах)";
            //Выбираем первые 9 столбцов второй строки
            cells = sheet.get_Range("A3", "I3");
            //Устанавливаем цвет границ в чёрный цвет
            cells.Borders.ColorIndex = 1;
            //Тип - неприрывные
            cells.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
            //Толщина - широкие
            cells.Borders.Weight = Excel.XlBorderWeight.xlThick;
            //Вводим имена столбцов
            sheet.get_Range("A3").Value2 = "Компания";
            sheet.get_Range("B3").Value2 = "Модель";
            sheet.get_Range("C3").Value2 = "Число сделок";
            sheet.get_Range("D3").Value2 = "Метод оплаты";
            sheet.get_Range("E3").Value2 = "Тип сделки";
            sheet.get_Range("F3").Value2 = "Фамилия";
            sheet.get_Range("G3").Value2 = "Имя";
            sheet.get_Range("H3").Value2 = "Отчество";
            sheet.get_Range("I3").Value2 = "Дата рождения";

            //Устанавливаем выделение в первую строку данных
            cells = sheet.get_Range("A4");
            //Для каждой строки таблицы
            foreach (DataRow row in salesData.Tables[0].Rows)
            {
                //Выводим данные с учётом смещения
                cells.get_Offset(0, 0).Value2 = row[0].ToString();
                cells.get_Offset(0, 1).Value2 = row[1].ToString();
                cells.get_Offset(0, 2).Value2 = row[2].ToString();
                cells.get_Offset(0, 3).Value2 = row[3].ToString();
                cells.get_Offset(0, 4).Value2 = row[4].ToString();
                cells.get_Offset(0, 5).Value2 = row[5].ToString();
                cells.get_Offset(0, 6).Value2 = row[6].ToString();
                cells.get_Offset(0, 7).Value2 = row[7].ToString();
                cells.get_Offset(0, 8).Value2 = Convert.ToDateTime(row[8]).ToShortDateString();
                //Смещаем выделение на строку ниже
                cells = cells.get_Offset(1, 0);
            }
            //Выделяем заголовок
            cells = sheet.get_Range("A3", "I3");
            //Изменяем ширину выделенных столбцов
            cells.ColumnWidth = 16;
            //Выделяем все данные, обрамляем тонкой рамкой
            cells = sheet.get_Range(sheet.get_Range("A4", "I4"), cells.get_Offset(salesData.Tables[0].Rows.Count, 0));
            cells.Borders.ColorIndex = 1;
            cells.Borders.Weight = Excel.XlBorderWeight.xlThin;

            try
            {
                //Вызываем диалог сохранения
                book.SaveAs();
            }   //ОСТОРОЖНО!!!
            //Он любит бросать исключения
            catch (Exception) { };
        }

    }
}
