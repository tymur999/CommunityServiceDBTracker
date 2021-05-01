using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;

namespace CommunityServiceDBTracker
{
    public partial class studentsForm : Form
    {
        //to use form as student selector run constructor with showSelector = "true"
        public studentsForm(bool showSelector = false)
        {
            InitializeComponent();
            selectorMode = showSelector;
        }

        //property will hold and return back selected student ID when use as selector
        public int SelectedId
        {
            get 
            {
                return selectedId;
            }
        }

        //to use form as students selector in other forms
        int selectedId = 0;
        bool selectorMode = false; 
        
        private void studentsForm_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'studentsDS.HoursAwardsPerStudentView' table. You can move, or remove it, as needed.
            this.hoursAwardsPerStudentViewTableAdapter.Fill(this.studentsDS.HoursAwardsPerStudentView);
            // TODO: This line of code loads data into the 'studentsDS.Students' table. You can move, or remove it, as needed.
            this.studentsTableAdapter.Fill(this.studentsDS.Students);

            saveButton.Enabled = false;
            cancelButton.Enabled = false;

            //added icon from resources
            this.Icon = CommunityServiceDBTracker.Properties.Resources.CommunityServiceIcon;
            
            if (selectorMode)
            {
                splitContainer1.Panel2Collapsed = true;
                splitContainer1.Panel2.Hide();
                Width = 400;
                studentsDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
                SaveChanges();
        }

        private void studentsBindingSource_ListChanged(object sender, ListChangedEventArgs e)
        {
            saveButton.Enabled = true;
            cancelButton.Enabled = true;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            studentsDS.RejectChanges();
            studentsBindingSource.ResetBindings(false);

            saveButton.Enabled = false;
            cancelButton.Enabled = false;

        }
        public void SaveChanges()
        {
            try
            {
                studentsBindingSource.EndEdit();
                studentsTableAdapter.Update(studentsDS.Students);
                saveButton.Enabled = false;
                cancelButton.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(String.Format("Exception while trying to save changes to the Students table: {0}", ex.Message),"Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void idtextBox_Validating(object sender, CancelEventArgs e)
        {
            ValidateId();
        }

        private bool ValidateId()
        {
            bool bStatus = true;
            if (idtextBox.Text.Trim() == "")
            {
                errorProvider1.SetError(idtextBox, "Please enter student ID");
                bStatus = false;
            }
            else if(!int.TryParse(idtextBox.Text, out int i))
            {
                errorProvider1.SetError(idtextBox, "Student ID should contain digits only");
                bStatus = false;
            }
            else
                errorProvider1.SetError(idtextBox, "");
            return bStatus;
        }

        private void nametextBox_Validating(object sender, CancelEventArgs e)
        {
            ValidateName();
        }

        private bool ValidateName()
        {
            bool bStatus = true;
            if (nametextBox.Text.Trim().Length > 32)
            {
                errorProvider2.SetError(nametextBox, "First and Last names length should be not greater than 32 symbols");
                bStatus = false;
            }
            else
                errorProvider2.SetError(nametextBox, "");
            return bStatus;
        }

        private void gradeTextBox_Validating(object sender, CancelEventArgs e)
        {
            ValidateGrade();
        }

        private bool ValidateGrade()
        {
            bool bStatus = true;
            int grade;

            if (!int.TryParse(gradeTextBox.Text, out grade))
            {
                errorProvider3.SetError(gradeTextBox, "Student grade should contain digits only");
                bStatus = false;
            }
            else if (grade < 9 || grade > 12)
            {
                errorProvider3.SetError(gradeTextBox, "Student grade should be from 9 to 12 only");
                bStatus = false;
            }
            else
                errorProvider3.SetError(gradeTextBox, "");
            return bStatus;
        }

        private void studentsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (saveButton.Enabled)
            {
                if (MessageBox.Show("The changes made to the table was not saved to database, all changes will be lost! Do you want to continue and lose your changes?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                    e.Cancel = true;
            }
        }
        //used when form in selector mode
        private void studentsDataGridView_DoubleClick(object sender, EventArgs e)
        {
            if (selectorMode && studentsBindingSource.Current != null)
            {
                selectedId = ((StudentsDS.StudentsRow)((DataRowView)studentsBindingSource.Current).Row).Id;
                Close();
            }
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
                foreach (DataGridViewColumn dgvGridCol in studentsDataGridView.Columns)
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
                    foreach (DataGridViewColumn GridCol in studentsDataGridView.Columns)
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
                while (iRow <= studentsDataGridView.Rows.Count - 1)
                {
                    DataGridViewRow GridRow = studentsDataGridView.Rows[iRow];
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
                            e.Graphics.DrawString(this.Text, new Font(studentsDataGridView.Font, FontStyle.Bold),
                                    Brushes.Black, e.MarginBounds.Left, e.MarginBounds.Top -
                                    e.Graphics.MeasureString(this.Text, new Font(studentsDataGridView.Font,
                                    FontStyle.Bold), e.MarginBounds.Width).Height - 13);

                            String strDate = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToShortTimeString();
                            //Draw Date
                            e.Graphics.DrawString(strDate, new Font(studentsDataGridView.Font, FontStyle.Bold),
                                    Brushes.Black, e.MarginBounds.Left + (e.MarginBounds.Width -
                                    e.Graphics.MeasureString(strDate, new Font(studentsDataGridView.Font,
                                    FontStyle.Bold), e.MarginBounds.Width).Width), e.MarginBounds.Top -
                                    e.Graphics.MeasureString(this.Text, new Font(new Font(studentsDataGridView.Font,
                                    FontStyle.Bold), FontStyle.Bold), e.MarginBounds.Width).Height - 13);

                            //Draw Columns                 
                            iTopMargin = e.MarginBounds.Top;
                            foreach (DataGridViewColumn GridCol in studentsDataGridView.Columns)
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
                                e.Graphics.DrawString(Cel.Value.ToString(), Cel.InheritedStyle.Font,
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

        private void studentsDataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show("The data entered is not in correct format! Please cancel the changes and try again!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

  
        private void studentsBindingSource_PositionChanged(object sender, EventArgs e)
        {
            hoursAwardsPerStudentViewBindingSource.ResetBindings(false);
        }

        private void awardNameComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            awardPictureBox.Visible = awardNameComboBox.Text.Length > 0;
        }
    }
}
