using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CommunityServiceDBTracker
{
    public partial class Reprots : Form
    {
        private List<Report> reports = new List<Report>();
        private Report currentReport = null;
        
        public Reprots()
        {
            InitializeComponent();

            LoadReports(String.Format(@"{0}\Reports.xml", Application.StartupPath));

            //saving report example

            //Report report = new Report();
            //report.ReportName = "TestStudents";
            //report.ReportTitle = "Test Students";
            //report.QueryText = "select * from students where StudentId = @studentId";
            //report.Parameters.Add(new QueryParameter { ParameterName = "@studentName", ParameterValue = "Test Name" });
            //report.Parameters.Add(new QueryParameter { ParameterName = "@studentId", ParameterValue = 1000011 });

            //reports.Add(report);

            //var xmlreport = Report.ObjectToXMLGeneric<List<Report>>(reports);

        }
        private void SQLcheckBox_CheckedChanged(object sender, EventArgs e)
        {
            queryTextBox.Visible = SQLcheckBox.Checked;
        }
        public void LoadReports(string xmlReportsFile)
        {
            try
            {
                reports = Report.XMLToObject<List<Report>>(File.ReadAllText(xmlReportsFile));
                reportNameComboBox.DataSource = reports;
            }
            catch(Exception ex) 
            {
                _ = MessageBox.Show(string.Format("Error while trying to load reports definitions from xml file \n{0}! Exception: {1}\n Please make sure file exists and is not corrupted.", xmlReportsFile, ex.Message), "Report Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void studentNameComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentReport = (Report)reportNameComboBox.SelectedItem;
            reportsGridView.Visible = false;
            paremetersGrid.SelectedObject = currentReport;
            queryTextBox.Text = currentReport.QueryText;
        }

        private void execButton_Click(object sender, EventArgs e)
        {
            ExecuteReport();
            reportsGridView.Visible = true;
            printButton.Enabled = true;
        }

        private void ExecuteReport()
        {
            string queryStr = queryTextBox.Text;
            foreach (QueryParameter parameter in currentReport.Parameters)
            {
                var @switch = new Dictionary<Type, Action>
                    {
                        { typeof(string), () => parameter.ParameterValue = !parameter.ParameterValue.Contains("'") ? "'" + parameter.ParameterValue + "'" : parameter.ParameterValue},
                        { typeof(DateTime), () => parameter.ParameterValue = !parameter.ParameterValue.Contains("'") ? "'" + parameter.ParameterValue + "'" : parameter.ParameterValue},
                        { typeof(Int32), () => parameter.ParameterValue = parameter.ParameterValue},
                        { typeof(double), () => parameter.ParameterValue = parameter.ParameterValue}
                    };
                Type parameterType = parameter.ParameterValue.GetType();
                @switch[parameterType]();
                queryStr = PopulateQueryString(queryStr, parameter.ParameterName, parameter.ParameterValue.ToString());
            }
            
            //see overides in added partial calsse for QueryTableAdapter.cs
            //and read codeproject articale related: https://www.codeproject.com/Articles/17324/Extending-TableAdapters-for-Dynamic-SQL
            // plus this usefull explanation: https://docs.microsoft.com/en-us/visualstudio/data-tools/fill-datasets-by-using-tableadapters?view=vs-2019

            this.reportTableAdapter = new CServiceTrackingDataSetTableAdapters.ReportTableAdapter();
            this.reportTableAdapter.SelectCommand.CommandText = queryStr;

            reportsGridView.DataSource = this.reportTableAdapter.GetData();

        }
        private string PopulateQueryString(string queryString, string parameterName, string parameterValue)
        {
            StringBuilder builder = new StringBuilder(queryString);
            return builder.Replace(parameterName, parameterValue).ToString();
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
        private void printButton_Click(object sender, EventArgs e)
        {
            //Open the print dialog
            //Get the document
            if (DialogResult.OK == printPreviewDialog1.ShowDialog())
            {
                printDocument1.DocumentName = String.Format("{0} Print", this.Text);
                printDocument1.DefaultPageSettings.Landscape = true;
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
                foreach (DataGridViewColumn dgvGridCol in reportsGridView.Columns)
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
                    foreach (DataGridViewColumn GridCol in reportsGridView.Columns)
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
                while (iRow <= reportsGridView.Rows.Count - 1)
                {
                    DataGridViewRow GridRow = reportsGridView.Rows[iRow];
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
                            e.Graphics.DrawString(this.reportNameComboBox.Text, new Font(reportsGridView.Font, FontStyle.Bold),
                                    Brushes.Black, e.MarginBounds.Left, e.MarginBounds.Top -
                                    e.Graphics.MeasureString(this.Text, new Font(reportsGridView.Font,
                                    FontStyle.Bold), e.MarginBounds.Width).Height - 13);

                            String strDate = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToShortTimeString();
                            //Draw Date
                            e.Graphics.DrawString(strDate, new Font(reportsGridView.Font, FontStyle.Bold),
                                    Brushes.Black, e.MarginBounds.Left + (e.MarginBounds.Width -
                                    e.Graphics.MeasureString(strDate, new Font(reportsGridView.Font,
                                    FontStyle.Bold), e.MarginBounds.Width).Width), e.MarginBounds.Top -
                                    e.Graphics.MeasureString(this.Text, new Font(new Font(reportsGridView.Font,
                                    FontStyle.Bold), FontStyle.Bold), e.MarginBounds.Width).Height - 13);

                            //Draw Columns                 
                            iTopMargin = e.MarginBounds.Top;
                            foreach (DataGridViewColumn GridCol in reportsGridView.Columns)
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

    }
}
