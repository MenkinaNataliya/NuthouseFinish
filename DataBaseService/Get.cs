using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.IO;


namespace DataBaseService
{
    public class Get
    {
        public static string[] Denominations()
        {
            using (var db = new DbModel())
                return Service.GetNaminStrings(db.Denominations.ToArray());
        }
        public static string[] Marks()
        {
            using (var db = new DbModel())
                return Service.GetNaminStrings(db.Marks.ToArray());
        }

        public static string[] City()
        {
            using (var db = new DbModel())
                return Service.GetNaminStrings(db.city.ToArray());
        }

        public static string[] Employee()
        {
            using (var db = new DbModel())
                return Service.GetEmployeeString(db.Employees.ToArray());
        }

        public static List<Equipment> Equipments(ReportFilter report)
        {
            using (var db = new DbModel())
            {
                var bigResult = db.Equipments.Include(x => x.Status).Include(x => x.Mark)
                                   .Include(x => x.Responsible).Include(x => x.WhoUses)
                                   .Include(x => x.City).Include(x => x.Denomination);

                if (report.FilterModernisation != "whatever")
                    bigResult = report.FilterModernisation == "true" ? bigResult.Where(x => x.Modernization) : bigResult.Where(x => x.Modernization == false);
                if (report.FilterCities.Count != 0)
                    bigResult = bigResult.Where(x => report.FilterCities.Any(y => y == x.City.Naming));
                if (report.FilterDenominations.Count != 0)
                    bigResult = bigResult.Where(x => report.FilterDenominations.Any(y => y == x.Denomination.Naming)); ;
                if (report.FilterMarks.Count != 0)
                    bigResult = bigResult.Where(x => report.FilterMarks.Any(y => y == x.Mark.Naming));
                if (report.FilterStatus.Count != 0)
                    bigResult = bigResult.Where(x => report.FilterStatus.Any(y => y == x.Status.Naming));
                if (report.FilterResponsibles.Count != 0)
                    bigResult = bigResult.Where(x => report.FilterResponsibles.Any(y => y == (x.Responsible.LastName + " " + x.Responsible.FirstName + " " + x.Responsible.SecondName)));
                return bigResult.ToList();

            }

        }

        public static List<Equipment> Equipments(string num)
        {
            using (var db = new DbModel())
            {
                if (num == "-1")
                    return db.Equipments.Include(x => x.Status).Include(x => x.Mark)
                                   .Include(x => x.Responsible).Include(x => x.WhoUses)
                                   .Include(x => x.City).Include(x => x.Denomination).ToList();
                return db.Equipments.Include(x => x.Status).Include(x => x.Mark)
                                   .Include(x => x.Responsible).Include(x => x.WhoUses)
                                   .Include(x => x.City).Include(x => x.Denomination)
                                    .Where(x => x.InventoryNumber == num || x.OldInventoryNumber == num).ToList();
            }
        }

        public static Equipment Equipments(string id, string oldId)
        {
            using (var db = new DbModel())
            {
                return db.Equipments.Include(x => x.Status).Include(x => x.Mark)
                                   .Include(x => x.Responsible).Include(x => x.WhoUses)
                                   .Include(x => x.City).Include(x => x.Denomination).FirstOrDefault(x => x.InventoryNumber == id && x.OldInventoryNumber == oldId);
            }
        }

        public static List<ChangeHistory> History()
        {
            using (var db = new DbModel())
            {
                var list = db.History.Include(x => x.NewStatus).Include(x => x.OldStatus).ToList();
                db.History.RemoveRange(db.History);
                db.SaveChanges();
                return list;
            }
        }
    }
}
