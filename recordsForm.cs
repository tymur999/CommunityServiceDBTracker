using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommunityServiceDBTracker;

namespace CommunityServiceDBTracker
{
    public partial class recordsForm : Form
    {
        public recordsForm()
        {
            InitializeComponent();
        }

      
        private void recordsForm_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'studentsServiceRecords.Students' table. You can move, or remove it, as needed.
            this.studentsTableAdapter.Fill(this.studentsServiceRecords.Students);
            // TODO: This line of code loads data into the 'studentsServiceRecords.ServiceRecords' table. You can move, or remove it, as needed.
            this.serviceRecordsTableAdapter.Fill(this.studentsServiceRecords.ServiceRecords);

            saveButton.Enabled = false;
            cancelButton.Enabled = false;

            //added icon from resources
            this.Icon = CommunityServiceDBTracker.Properties.Resources.CommunityServiceIcon;
            
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
                SaveChanges();
        }

        private void categoriesBindingSource_ListChanged(object sender, ListChangedEventArgs e)
        {
            saveButton.Enabled = true;
            cancelButton.Enabled = true;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            studentsServiceRecords.RejectChanges();
            studentsServiceRecordsBindingSource.ResetBindings(false);

            saveButton.Enabled = false;
            cancelButton.Enabled = false;

        }
        public void SaveChanges()
        {
            try
            {
                studentsServiceRecordsBindingSource.EndEdit();
                serviceRecordsTableAdapter.Update(studentsServiceRecords.ServiceRecords);
                saveButton.Enabled = false;
                cancelButton.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(String.Format("Exception while trying to save changes to the Categories table: {0}", ex.Message),"Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateId()
        {
            bool bStatus = true;
            int Id;

            if (!int.TryParse(idTextBox.Text, out Id))
            {
                errorProvider1.SetError(idTextBox, "Student ID should contain digits only");
                bStatus = false;
            }
            else if (!studentsServiceRecords.Students.Any(s => s.Id == Id))
            {
                errorProvider1.SetError(idTextBox, String.Format("Student with ID {0} doesnt exists!", Id));
                bStatus = false;
            }
            else
                errorProvider1.SetError(idTextBox, "");
            return bStatus;
        }

        private void recordsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (saveButton.Enabled)
            {
                if (MessageBox.Show("The changes made to the table was not saved to database, all changes will be lost! Do you want to continue and lose your changes?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                    e.Cancel = true;
            }
        }

        private void studentsServiceRecordsBindingSource_ListChanged(object sender, ListChangedEventArgs e)
        {
            saveButton.Enabled = true;
            cancelButton.Enabled = true;
        }

        private void recordsDataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            studentsServiceRecordsBindingSource.ResetBindings(false);
        }

        #region Vars for printing
        StringFormat strFormat; //Used to format the grid rows.
        ArrayList arrColumnLefts = new ArrayList();//Used to save left coordinates of columns
        ArrayList arrColumnWidths = new ArrayList();//Used to save column widths
        int iCellHeight = 0; //Used to get/set the datagridview cell height
        int iTotalWidth = 0; //
        int iRow = 0;//Used as counter
        bool bFirstPage = false; //Used to check whether we are printing first page
        bool bNewPage = false;// Used to check whether we are printing a new page
        int iHeaderHeight = 0; //Used for the header height
        #endregion

        #region Print Button Click Event
        /// <summary>
        /// Handles the print button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void printStripButton_Click(object sender, EventArgs e)
        {
            //Open the print dialog
            //Get the document
            if (DialogResult.OK == printPreviewDialog1.ShowDialog())
            {
                printDocument1.DocumentName = String.Format("{0} Print", this.Text);
                printDocument1.Print();
            }
        }
        #endregion

        #region Begin Print Event Handler
        /// <summary>
        /// Handles the begin print event of print document
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void printDocument1_BeginPrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            try
            {
                strFormat = new StringFormat();
                strFormat.Alignment = StringAlignment.Near;
                strFormat.LineAlignment = StringAlignment.Center;
                strFormat.Trimming = StringTrimming.EllipsisCharacter;

                arrColumnLefts.Clear();
                arrColumnWidths.Clear();
                iCellHeight = 0;
                iRow = 0;
                bFirstPage = true;
                bNewPage = true;

                // Calculating Total Widths
                iTotalWidth = 0;
                foreach (DataGridViewColumn dgvGridCol in recordsDataGridView.Columns)
                {
                    iTotalWidth += dgvGridCol.Width;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Print Page Event
        /// <summary>
        /// Handles the print page event of print document
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            try
            {
                //Set the left margin
                int iLeftMargin = e.MarginBounds.Left;
                //Set the top margin
                int iTopMargin = e.MarginBounds.Top;
                //Whether more pages have to print or not
                bool bMorePagesToPrint = false;
                int iTmpWidth = 0;

                //For the first page to print set the cell width and header height
                if (bFirstPage)
                {
                    foreach (DataGridViewColumn GridCol in recordsDataGridView.Columns)
                    {
                        iTmpWidth = (int)(Math.Floor((double)((double)GridCol.Width /
                                       (double)iTotalWidth * (double)iTotalWidth *
                                       ((double)e.MarginBounds.Width / (double)iTotalWidth))));

                        iHeaderHeight = (int)(e.Graphics.MeasureString(GridCol.HeaderText,
                                    GridCol.InheritedStyle.Font, iTmpWidth).Height) + 11;

                        // Save width and height of headres
                        arrColumnLefts.Add(iLeftMargin);
                        arrColumnWidths.Add(iTmpWidth);
                        iLeftMargin += iTmpWidth;
                    }
                }
                //Loop till all the grid rows not get printed
                while (iRow <= recordsDataGridView.Rows.Count - 1)
                {
                    DataGridViewRow GridRow = recordsDataGridView.Rows[iRow];
                    //Set the cell height
                    iCellHeight = GridRow.Height + 5;
                    int iCount = 0;
                    //Check whether the current page settings allo more rows to print
                    if (iTopMargin + iCellHeight >= e.MarginBounds.Height + e.MarginBounds.Top)
                    {
                        bNewPage = true;
                        bFirstPage = false;
                        bMorePagesToPrint = true;
                        break;
                    }
                    else
                    {
                        if (bNewPage)
                        {
                            //Draw Header
                            e.Graphics.DrawString(this.Text, new Font(recordsDataGridView.Font, FontStyle.Bold),
                                    Brushes.Black, e.MarginBounds.Left, e.MarginBounds.Top -
                                    e.Graphics.MeasureString(this.Text, new Font(recordsDataGridView.Font,
                                    FontStyle.Bold), e.MarginBounds.Width).Height - 13);

                            String strDate = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToShortTimeString();
                            //Draw Date
                            e.Graphics.DrawString(strDate, new Font(recordsDataGridView.Font, FontStyle.Bold),
                                    Brushes.Black, e.MarginBounds.Left + (e.MarginBounds.Width -
                                    e.Graphics.MeasureString(strDate, new Font(recordsDataGridView.Font,
                                    FontStyle.Bold), e.MarginBounds.Width).Width), e.MarginBounds.Top -
                                    e.Graphics.MeasureString(this.Text, new Font(new Font(recordsDataGridView.Font,
                                    FontStyle.Bold), FontStyle.Bold), e.MarginBounds.Width).Height - 13);

                            //Draw Columns                 
                            iTopMargin = e.MarginBounds.Top;
                            foreach (DataGridViewColumn GridCol in recordsDataGridView.Columns)
                            {
                                e.Graphics.FillRectangle(new SolidBrush(Color.LightGray),
                                    new Rectangle((int)arrColumnLefts[iCount], iTopMargin,
                                    (int)arrColumnWidths[iCount], iHeaderHeight));

                                e.Graphics.DrawRectangle(Pens.Black,
                                    new Rectangle((int)arrColumnLefts[iCount], iTopMargin,
                                    (int)arrColumnWidths[iCount], iHeaderHeight));

                                e.Graphics.DrawString(GridCol.HeaderText, GridCol.InheritedStyle.Font,
                                    new SolidBrush(GridCol.InheritedStyle.ForeColor),
                                    new RectangleF((int)arrColumnLefts[iCount], iTopMargin,
                                    (int)arrColumnWidths[iCount], iHeaderHeight), strFormat);
                                iCount++;
                            }
                            bNewPage = false;
                            iTopMargin += iHeaderHeight;
                        }
                        iCount = 0;
                        //Draw Columns Contents                
                        foreach (DataGridViewCell Cel in GridRow.Cells)
                        {
                            if (Cel.Value != null)
                            {
                                string value = Cel is DataGridViewComboBoxCell ? ((DataGridViewComboBoxCell)Cel).EditedFormattedValue.ToString() : Cel.Value.ToString();
                                e.Graphics.DrawString(value, Cel.InheritedStyle.Font,
                                            new SolidBrush(Cel.InheritedStyle.ForeColor),
                                            new RectangleF((int)arrColumnLefts[iCount], (float)iTopMargin,
                                            (int)arrColumnWidths[iCount], (float)iCellHeight), strFormat);
                            }
                            //Drawing Cells Borders 
                            e.Graphics.DrawRectangle(Pens.Black, new Rectangle((int)arrColumnLefts[iCount],
                                    iTopMargin, (int)arrColumnWidths[iCount], iCellHeight));

                            iCount++;
                        }
                    }
                    iRow++;
                    iTopMargin += iCellHeight;
                }

                //If more lines exist, print another page.
                if (bMorePagesToPrint)
                    e.HasMorePages = true;
                else
                    e.HasMorePages = false;
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        private void recordsDataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show("The data entered is not in correct format! Please cancel the changes and try again!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void studentNameComboBox_Validating(object sender, CancelEventArgs e)
        {
            ValidateName();
        }
        private bool ValidateName()
        {
            bool bStatus = true;
                errorProvider2.SetError(studentNameComboBox, "");
            return bStatus;
        }

        private void idTextBox_Validating(object sender, CancelEventArgs e)
        {
            ValidateId();
        }

        private bool ValidateDate()
        {
            bool bStatus = true;
            DateTime date;

            if (!DateTime.TryParse(recordDateTimePicker.Text, out date))
            {
                errorProvider3.SetError(recordDateTimePicker, "service date should lloks like a date");
                bStatus = false;
            }
            else
                errorProvider3.SetError(idTextBox, "");
            return bStatus;
        }

        private void recordDateTimePicker_Validating(object sender, CancelEventArgs e)
        {
            ValidateDate();
        }

        private void hoursTextBox_Validating(object sender, CancelEventArgs e)
        {
            ValidateHours();
        }

        private bool ValidateHours()
        {
            bool bStatus = true;
            int hours;

            if (!int.TryParse(hoursTextBox.Text, out hours))
            {
                errorProvider1.SetError(hoursTextBox, "Service hours should contain digits only");
                bStatus = false;
            }
            else if (hours < 1 || hours > 24)
            {
                errorProvider1.SetError(hoursTextBox, String.Format("Service hours amount could be between 1 and 24!"));
                bStatus = false;
            }
            else
                errorProvider1.SetError(hoursTextBox, "");
            return bStatus;
        }

        private void studentNameComboBox_SelectedValueChanged(object sender, EventArgs e)
        {

        }
    }
}
