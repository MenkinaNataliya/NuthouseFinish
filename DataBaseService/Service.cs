using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseService
{
    public class Service
    {
        public static string AddDictionary(Dictionary dict)
        {
            if (dict.Denomination != null)
            {
                return CheckDenomination(dict.Denomination.Naming);
            }
            if (dict.Mark != null)
            {
                return CheckMark(dict.Mark.Naming);
            }
            if (dict.Employee != null)
            {
                return CheckEmploy(dict.Employee);
            }
            return "Введите данные";

        }

        public static string[] GetNaminStrings(BaseClass[] items)
        {
            return items.Select(item => item.Naming).ToArray();
        }


        public static string[] GetEmployeeString(Employee[] employees)
        {
            return
                employees.Select(employee => employee.LastName + " " + employee.FirstName + " " + employee.SecondName)
                    .ToArray();
        }


        public static string AddEquipment(Equipment equip)
        {
            using (var db = new DbModel())
            {
                if (db.Equipments.Find(equip.InventoryNumber) != null || db.Equipments.Where(x => x.OldInventoryNumber == equip.InventoryNumber).ToList().Count != 0)
                    return "Устройство с таким инвентарным номером уже существует";

                CheckDenomination(equip.Denomination.Naming);
                CheckMark(equip.Mark.Naming);
                CheckEmploy(equip.Responsible);
                CheckEmploy(equip.WhoUses);

                var res = new Equipment
                {
                    InventoryNumber = equip.InventoryNumber,
                    OldInventoryNumber = equip.OldInventoryNumber,
                    Model = equip.Model,
                    Comment = equip.Comment,
                    Modernization = equip.Modernization,
                    Denomination = db.Denominations.FirstOrDefault(x => x.Naming == equip.Denomination.Naming),
                    Mark = db.Marks.FirstOrDefault(x => x.Naming == equip.Mark.Naming),
                    Responsible = db.Employees.FirstOrDefault(x => x.FirstName == equip.Responsible.FirstName && x.SecondName == equip.Responsible.SecondName && x.LastName == equip.Responsible.LastName),
                    WhoUses = db.Employees.FirstOrDefault(x => x.FirstName == equip.WhoUses.FirstName && x.SecondName == equip.WhoUses.SecondName && x.LastName == equip.WhoUses.LastName),
                    City = db.city.FirstOrDefault(x => x.Naming == equip.City.Naming),
                    Floor = equip.Floor,
                    Housing = equip.Housing,
                    Cabinet = equip.Cabinet,
                    Status = db.status.FirstOrDefault(x => x.Naming == equip.Status.Naming)
                };
                db.Equipments.Add(res);
                db.SaveChanges();
                return "Данные добавлены успешно";
            }
        }

        public static string ChangeStatus(string number, string newStatus, string newComment)
        {
            using (DbModel db = new DbModel())
            {
                var eq = db.Equipments.Include("Status").FirstOrDefault(x => x.InventoryNumber == number);
                var oldStatus = eq.Status;
                eq.Status = db.status.FirstOrDefault(x => x.Naming == newStatus);
                eq.Comment = newComment;
                db.SaveChanges();

                var res = new ChangeHistory
                {
                    InventNumber = number,
                    OldStatus = oldStatus,
                    NewStatus = db.status.FirstOrDefault(x => x.Naming == newStatus)
                };

                db.History.Add(res);
                db.SaveChanges();
                return "Данные изменены успешно";
            }
        }



        public static void CleanHistry()
        {
            using (DbModel db = new DbModel())
            {
                db.History.RemoveRange(db.History);
                db.SaveChanges();

            }
        }

        private static string CheckDenomination(string denomination)
        {
            using (var db = new DbModel())
            {
                var tmp = db.Denominations.FirstOrDefault(x => x.Naming == denomination);
                if (tmp != null) return "Такое наименование уже существует";
                db.Denominations.Add(new Denomination
                {
                    Naming = denomination
                });
                db.SaveChanges();
                return "OK";
            }
        }
        private static string CheckMark(string mark)
        {
            using (var db = new DbModel())
            {
                if (db.Marks.FirstOrDefault(x => x.Naming == mark) != null) return "Такая марка уже существует";
                db.Marks.Add(new Mark
                {
                    Naming = mark
                });
                db.SaveChanges();
                return "OK";
            }
        }
        private static string CheckEmploy(Employee employ)
        {
            using (var db = new DbModel())
            {
                if (
                    db.Employees.FirstOrDefault(x => x.SecondName == employ.SecondName && x.LastName == employ.LastName && x.FirstName == employ.FirstName) !=
                    null) return "Такой сотрудник уже существует";
                db.Employees.Add(new Employee
                {
                    FirstName = employ.FirstName,
                    SecondName = employ.SecondName,
                    LastName = employ.LastName
                });
                db.SaveChanges();
                return "OK";
            }
        }
    }
}
