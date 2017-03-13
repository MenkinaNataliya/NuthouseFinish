﻿using System;
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
    public partial class GenerateReport : Form
    {
        public GenerateReport()
        {
            InitializeComponent();
        }

        private void добавитьОборудованиеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void изменитьСтатусToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
            var change = new ChangeStatus();
            change.Activate();

            change.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();

            var filterReport = new ReportFilter();

            foreach (var city in Cities.CheckedItems)
                filterReport.FilterCities.Add(city.ToString());

            foreach (var denom in Denominations.CheckedItems)
                filterReport.FilterDenominations.Add(denom.ToString());

            foreach (var mark in Marks.CheckedItems)
                filterReport.FilterMarks.Add(mark.ToString());

            foreach (var status in Status.CheckedItems)
                filterReport.FilterStatus.Add(status.ToString());

            foreach (var resp in Responsible.CheckedItems)
                filterReport.FilterResponsibles.Add(resp.ToString());

            filterReport.FilterModernisation = Modernisation.Checked.ToString();

            var report = new Report(filterReport);
            report.Activate();

            report.ShowDialog();
        }

        private void GenerateReports_Load(object sender, EventArgs e)
        {
            Cities.Items.Add("Копейск");
            Cities.Items.Add("Челябинск");
            Denominations.Items.AddRange(Get.Denominations());
            Responsible.Items.AddRange(Get.Employee());
            Status.Items.Add("В работе");
            Status.Items.Add("В ремонте");
            Status.Items.Add("Списано");
            Marks.Items.AddRange(Get.Marks());

        }
    }
}
