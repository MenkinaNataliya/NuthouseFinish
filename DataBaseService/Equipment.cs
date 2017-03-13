using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataBaseService
{
    public class Dictionary
    {
        [Display(Name = "Наименование")]
        public Denomination Denomination { get; set; }
        [Display(Name = "Марка")]
        public Mark Mark { get; set; }
        [Display(Name = "Новый сотрудник")]
        public Employee Employee { get; set; }
    }


    public class Equipment
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string InventoryNumber { get; set; }
        public string OldInventoryNumber { get; set; }
        public Denomination Denomination { get; set; }
        public Mark Mark { get; set; }
        public string Model { get; set; }
        public string Comment { get; set; }
        public bool Modernization { get; set; }
        public Employee Responsible { get; set; }
        public Employee WhoUses { get; set; }
        public City City { get; set; }
        public int Floor { get; set; }
        public string Housing { get; set; }
        public string Cabinet { get; set; }
        public Status Status { get; set; }
    }

    public class Employee
    {
        public ICollection<Equipment> equipments { get; set; }
        public int Id { get; set; }

        [Required(ErrorMessage = "Поле должно быть установлено")]
        [Display(Name = "Имя")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Поле должно быть установлено")]
        [Display(Name = "Отчество")]
        public string SecondName { get; set; }

        [Required(ErrorMessage = "Поле должно быть установлено")]
        [Display(Name = "Фамилия")]
        public string LastName { get; set; }

       public string EmployeeString()
        {
            return LastName + " " + FirstName + " " + SecondName;
        }
    }


    public class City : BaseClass { }

    public class Status : BaseClass
    {
        public ICollection<Equipment> equipments { get; set; }
    }
    public class Denomination : BaseClass
    {
        public ICollection<Equipment> equipments { get; set; }
    }
    public class Mark : BaseClass
    {
        public ICollection<Equipment> equipments { get; set; }
    }


    public class BaseClass
    {

        public int Id { get; set; }
        [Display(Name = "Название")]
        public string Naming { get; set; }
    }

    public class ChangeHistory
    {
        public int Id { get; set; }
        public string InventNumber { get; set; }
        public Status OldStatus { get; set; }
        public Status NewStatus { get; set; }
    }
    public class ReportFilter
    {
        public List<string> FilterCities;
        public List<string> FilterResponsibles;
        public List<string> FilterStatus;
        public List<string> FilterDenominations;
        public string FilterModernisation;
        public List<string> FilterMarks;

        public ReportFilter()
        {
            FilterCities = new List<string>();
            FilterResponsibles = new List<string>();
            FilterStatus = new List<string>();
            FilterDenominations = new List<string>();
            FilterMarks = new List<string>();
            FilterModernisation = "";
        }

    }
}
