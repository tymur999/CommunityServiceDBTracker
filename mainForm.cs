using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CommunityServiceDBTracker
{
    public partial class mainForm : Form
    {

        public mainForm()
        {
            InitializeComponent();
        }

        private void mainForm_Load(object sender, EventArgs e)
        {
            //added icon from resources
            this.Icon = CommunityServiceDBTracker.Properties.Resources.CommunityServiceIcon;
        }

        private void studentsButton_Click(object sender, EventArgs e)
        {
            var studentsForm = new studentsForm(false);
            studentsForm.MdiParent = this;
            studentsForm.Show();
        }

        private void awardsButton_Click(object sender, EventArgs e)
        {
            var awardsForm = new awardsForm();
            awardsForm.MdiParent = this;
            awardsForm.Show();
        }

        private void recordsButton_Click(object sender, EventArgs e)
        {
            var recordsForm = new recordsForm();
            recordsForm.MdiParent = this;
            recordsForm.Show();

        }

        private void reportsButton_Click(object sender, EventArgs e)
        {
            var reportsForm = new Reprots();
            reportsForm.MdiParent = this;
            reportsForm.Show();
        }

        private void aboutButton_Click(object sender, EventArgs e)
        {
            var aboutForm = new aboutForm();
            aboutForm.MdiParent = this;
            aboutForm.Show();
        }
    }
}
