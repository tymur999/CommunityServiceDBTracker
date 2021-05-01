using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunityServiceDBTracker.CServiceTrackingDataSetTableAdapters
{
	//overides in added partial calss delcaration to expose Command to be able to assign SQL select  statement on the fly
	//read codeproject related artilce: https://www.codeproject.com/Articles/17324/Extending-TableAdapters-for-Dynamic-SQL
	public partial class ReportTableAdapter : global::System.ComponentModel.Component
	{
		public global::System.Data.SqlClient.SqlCommand SelectCommand
		{
			get
			{
				if (this.CommandCollection is null)
					this.InitCommandCollection();

				return this._commandCollection[0];
			}
			set
			{
				this._commandCollection[0] = value;
			}
		}
	}
}
