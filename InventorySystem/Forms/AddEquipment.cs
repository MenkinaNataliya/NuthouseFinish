using System;
using System.ComponentModel;
using System.Windows.Forms;
using DataBaseService;
using InventorySystem.Forms;

namespace InventorySystem
{
    public partial class AddEquipment : Form
    {
        public AddEquipment()
        {
            InitializeComponent();
            InitializeForm();
        }

        private void InitializeForm()
        {
            ErrModel.Text = ErrMark.Text = ErrDenomination.Text = OldInventNumber.Text =
                ErrOldInventNumber.Text = ErrInventNumber.Text = InventNumber.Text = Model.Text =
                    ErrCabinet.Text = ErrCity.Text = ErrComment.Text = ErrFloor.Text = ErrHousing.Text = Housing.Text =
                        ErrRespPerson.Text = ErrWhoUses.Text = ErrStatus.Text = Cabinet.Text = "";

            City.Text = "Выберите город";
            City.Items.Clear();
            City.Items.AddRange(DataBaseService.Get.City());

            Mark.Text = @"Выберите марку";
            Mark.Items.Clear();
            Mark.Items.AddRange(DataBaseService.Get.Marks());

            Denomination.Text = "Выберите наименование";
            Denomination.Items.Clear();
            Denomination.Items.AddRange(DataBaseService.Get.Denominations());

            Status.Text = "Выберите статус";
            Status.Items.Clear();
            Status.Items.Add("В работе");
            Status.Items.Add("В ремонте");
            Status.Items.Add("Списано");

            var Employee = DataBaseService.Get.Employee();
            WhoUses.Text = "Выберите кто пользуется оборудованием";
            WhoUses.Items.Clear();
            WhoUses.Items.AddRange(Employee);

            ResponsiblePerson.Text = "Выберите ответственное лицо";
            ResponsiblePerson.Items.Clear();
            ResponsiblePerson.Items.AddRange(Employee);

           

            Floor.Text = "Выберите этаж";
            Floor.Items.Clear();
            Floor.Items.AddRange(new object[] { "1", "2", "3", "4" });

        }

        private void изменитьСтатусToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var change = new ChangeStatus();
            change.Activate();

            change.ShowDialog();
        }

        private void сгенерироватьОтчетToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var generate = new GenerateReport();
            generate.Activate();

            generate.ShowDialog();
        }

        private void посмотретьВсеОборудованиеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var showAll = new Equipments();
            showAll.Activate();
            showAll.ShowDialog();
        }

        private void Add_Click(object sender, EventArgs e)
        {
            if (Status.SelectedIndex != -1 && ErrModel.Text == "" && ErrMark.Text == "" && ErrDenomination.Text == "" &&
                 ErrOldInventNumber.Text == "" && ErrInventNumber.Text == "" &&
                 InventNumber.Text != "" && Housing.Text != "")
            {
                string oldNum;
                if (OldInventNumber.Text == "") oldNum = "";
                else oldNum = OldInventNumber.Text;


                var responsible = (ResponsiblePerson.SelectedIndex == -1
                    ? ResponsiblePerson.Text
                    : ResponsiblePerson.SelectedItem.ToString()).Split(' ');
                var who = (WhoUses.SelectedIndex == -1 ? WhoUses.Text : WhoUses.SelectedItem.ToString()).Split(' ');

                var equip = new Equipment
                {
                    InventoryNumber = InventNumber.Text,
                    OldInventoryNumber = oldNum.ToString(),
                    Model = Model.Text,
                    Comment = Comment.Text,
                    Modernization = Modernization.Checked,
                    Denomination = new Denomination { Naming = (Denomination.SelectedIndex == -1 ? Denomination.Text : Denomination.SelectedItem.ToString()) },
                    Mark = new Mark() { Naming = (Mark.SelectedIndex == -1 ? Mark.Text : Mark.SelectedItem.ToString()) },
                    Responsible =
                       new Employee { FirstName = responsible[1], SecondName = responsible[2], LastName = responsible[0] },
                    WhoUses = new Employee { FirstName = who[1], SecondName = who[2], LastName = who[0] },
                    City = new City { Naming = City.SelectedItem.ToString() },
                    Floor = Int32.Parse(Floor.SelectedItem.ToString()),
                    Housing = Housing.Text,
                    Cabinet = Cabinet.Text,
                    Status = new Status { Naming = Status.SelectedItem.ToString() }
                };


                var error = Service.AddEquipment(equip);
                if (error == "Данные добавлены успешно")
                {

                    MessageBox.Show("Устройство добавлено успешно");
                    InitializeForm();
                }
                else
                {
                    MessageBox.Show(error);
                }

            }
            else MessageBox.Show("Данные введены неверно");
        }

        private void OldInventNumber_Validating(object sender, CancelEventArgs e)
        {
            if (OldInventNumber.Text == string.Empty)
            {
                ErrOldInventNumber.Text = "";
                return;
            }

            ErrOldInventNumber.Text = "";
        }

        private void InventNumber_Validating(object sender, CancelEventArgs e)
        {
            if (InventNumber.Text == string.Empty)
            {
                ErrInventNumber.Text = "Поле должно быть заполнено";
                return;
            }

            ErrInventNumber.Text = "";
        }
        private void Denomination_Validating(object sender, CancelEventArgs e)
        {
            if (Denomination.SelectedIndex == -1 && Denomination.Text == "Выберите наименование")
            {
                ErrDenomination.Text = "Выберите наименование";
            }
            else ErrDenomination.Text = "";
        }

        private void Mark_Validating(object sender, CancelEventArgs e)
        {
            if (Mark.SelectedIndex == -1 && Mark.Text == "Выберите марку") ErrMark.Text = "Выберите марку";
            else ErrMark.Text = "";
        }

        private void Status_Validating(object sender, CancelEventArgs e)
        {
            if (Status.SelectedIndex == -1) ErrStatus.Text = "Выберите статус";
            else ErrStatus.Text = "";
        }

        private void City_Validating(object sender, CancelEventArgs e)
        {
            if (City.SelectedIndex == -1) ErrCity.Text = "Выберите город";
            else ErrCity.Text = "";
        }

        private void ResponsiblePerson_Validating(object sender, CancelEventArgs e)
        {
            if (ResponsiblePerson.SelectedIndex == -1 && ResponsiblePerson.Text == "Выберите ответственное лицо")
                ErrRespPerson.Text = "Выберите ответственное лицо";
            else ErrRespPerson.Text = "";
        }

        private void WhoUses_Validating(object sender, CancelEventArgs e)
        {
            if (WhoUses.SelectedIndex == -1 && WhoUses.Text == "Выберите сотрудника") ErrWhoUses.Text = "Выберите сотрудника";
            else ErrWhoUses.Text = "";
        }


       /* public DataBase.Equipment TranslateDbEquipment(Equipment equip)
        {
            var responsible = equip.Responsible.Split(' ');
            var who = equip.WhoUses.Split(' ');

            return new DataBase.Equipment
            {
                InventoryNumber = equip.InventoryNumber,
                OldInventoryNumber = equip.OldInventoryNumber,
                Model = equip.Model,
                Comment = equip.Comment,
                Modernization = equip.Modernization,
                Denomination = new Denomination { Naming = equip.Denomination },
                Mark = new Mark() { Naming = equip.Mark },
                Responsible =
                    new Employee { FirstName = responsible[1], SecondName = responsible[2], LastName = responsible[0] },
                WhoUses = new Employee { FirstName = who[1], SecondName = who[2], LastName = who[0] },
                City = new City { Naming = equip.City },
                Floor = equip.Floor,
                Housing = equip.Housing,
                Cabinet = equip.Cabinet,
                Status = new Status { Naming = equip.Status }
            };
        }*/

        private void Cabinet_Validating(object sender, CancelEventArgs e)
        {
            int cab;
            if (!Int32.TryParse(Cabinet.Text, out cab))
            {
                ErrCabinet.Text = "Поле может содержать только цифры";
                return;
            }
            else ErrCabinet.Text = "";
        }
    }
}
