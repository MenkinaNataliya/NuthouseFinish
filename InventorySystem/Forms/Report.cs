using System;
using System.Collections.Generic;
using DataBaseService;
using System.Windows.Forms;

namespace InventorySystem.Forms
{
    public partial class Report : Form
    {
        List<DataBaseService.Equipment> _equips;


        public Report(ReportFilter reportFilter)
        {
            InitializeComponent();
            InitializeForm(reportFilter);
        }
        public void InitializeForm(ReportFilter reportFilter)
        {

            _equips = Get.Equipments(reportFilter);


            System.Data.DataTable t = new System.Data.DataTable();
            t.Columns.Add("Инвентарный номер");
            t.Columns.Add("Старый инвентарный номер");
            t.Columns.Add("Наименование");
            t.Columns.Add("Марка");
            t.Columns.Add("Расположение");
            t.Columns.Add("Ответственное лицо");
            t.Columns.Add("Статус");
            t.Columns.Add("Примечание");

            foreach (var equip in _equips)
            {
                var res = new Employee[1];
                res[0] = equip.Responsible;
                t.Rows.Add(equip.InventoryNumber, equip.OldInventoryNumber, equip.Denomination.Naming,
                                equip.Mark.Naming, equip.City.Naming + ", корпус " + equip.Housing + ", кабинет " + equip.Cabinet, Service.GetEmployeeString(res)[0],
                                equip.Status.Naming, equip.Comment);

            }
            table.DataSource = t;

        }

        private void ok_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void print_Click(object sender, EventArgs e)
        {
            var generateService = new GenerateDocumentsReport();
            generateService.GenerateReportExcel(_equips);
        }
    }
}
