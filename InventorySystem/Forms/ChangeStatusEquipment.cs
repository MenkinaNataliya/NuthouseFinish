using System.Windows.Forms;
using DataBaseService;


namespace InventorySystem.Forms
{
    public partial class ChangeStatusEquipment : Form
    {
        public ChangeStatusEquipment(string inventNum)
        {
            InitializeComponent();
            InitializeForm(inventNum);
        }

        private void InitializeForm(string inventNum)
        {

            var equipments = Get.Equipments(inventNum);

            if (equipments.Count == 0 ) error.Text = @"Данные не найдены";

            else
            {
                table.Columns.Add("InventNum", "Инвентарный номер");
                table.Columns.Add("OldInventNum", "Старый инвентарный номер");
                table.Columns.Add("Denomination", "Наименование");
                table.Columns.Add("Mark", "Марка");
                table.Columns.Add("Site", "Расположение");
                table.Columns.Add("Respon", "Ответственное лицо");
                table.Columns.Add("CurStatus", "Текущий статус");
                DataGridViewComboBoxColumn combo = new DataGridViewComboBoxColumn
                {
                    HeaderText = @"Изменить на",
                    Name = "Status"
                };
                combo.Items.Add("В работе");
                combo.Items.Add("В ремонте");
                combo.Items.Add("Списано");


                table.Columns.Add(combo);
                table.Columns.Add("Comment", "Примечание");

                DataGridViewButtonColumn button = new DataGridViewButtonColumn
                {
                    HeaderText = @"Изменить",
                    Text = "Изменить"
                };

                table.Columns.Add(button);

                foreach (var equip in equipments)
                {
                    table.Rows.Add(equip.InventoryNumber, equip.OldInventoryNumber,
                        equip.Denomination.Naming, equip.Mark.Naming + " "+ equip.Model, 
                        equip.City.Naming + " " + " " + equip.Housing,
                        equip.Responsible.EmployeeString(), equip.Status.Naming, "", equip.Comment, "");
                }
            }
        }


        private void table_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 9)
            {

                var rows = table.Rows[e.RowIndex];
                if (rows.Cells[7].Value.ToString() == "") MessageBox.Show(@"Выберите новый статус");
                else
                {
                    Service.ChangeStatus(rows.Cells[0].Value.ToString(), rows.Cells[7].Value.ToString(), rows.Cells[8].Value.ToString());
                    if (rows.Cells[7].Value.ToString() == "Списано")
                        GenerateDefect(rows.Cells[0].Value.ToString(), rows.Cells[2].Value.ToString(), rows.Cells[3].Value.ToString(), 
                            rows.Cells[5].Value.ToString(), rows.Cells[4].Value.ToString());
                    MessageBox.Show(@"Данные успешно измененны");
                    Close();
                }
            }

        }


        private static void GenerateDefect(string inventnumber, string denomination,string mark, string responsible, string place)
        {
            var generateService = new GenerateDocumentsReport();
            generateService.GenerateDeffectWorld(inventnumber, denomination, mark,  responsible, place);
        }
    }
}
