using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DataBaseService;

namespace InventorySystem.Forms
{
    public partial class Equipments : Form
    {
        public Equipments()
        {
            InitializeComponent();
        }
        private void Equipments_Load(object sender, EventArgs e)
        {

            var equipments = Get.Equipments("");
            DataTable t = new DataTable();
            t.Columns.Add("Инвентарный номер");
            t.Columns.Add("Старый инвентарный номер");
            t.Columns.Add("Наименование");
            t.Columns.Add("Марка");
            t.Columns.Add("Расположение");
            t.Columns.Add("Ответственное лицо");
            t.Columns.Add("Кто использует");
            t.Columns.Add("Статус");
            t.Columns.Add("Примечание");

            foreach (var equip in equipments)
            {
                var res = new Employee[2];
                res[0] = equip.Responsible;
                res[1] = equip.WhoUses;
                var result = Service.GetEmployeeString(res);
                t.Rows.Add(equip.InventoryNumber, equip.OldInventoryNumber, equip.Denomination.Naming,
                                equip.Mark.Naming, equip.City.Naming + ", корпус " + equip.Housing + ", кабинет " + equip.Cabinet, result[0], result[1],
                                equip.Status.Naming, equip.Comment);

            }
            table.DataSource = t;
        }
    }
}
