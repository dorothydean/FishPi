using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FishPi.Models
{
	public class FishPiCalculation //: IValidatableObject
	{
		[Key]
		public int MemberID { get; set; }

		[Required]
		public string Researcher { get; set; }

		[Required (ErrorMessage ="Please enter your date as MM/DD/YYYY; Cannot be a future date")]
		[DataType(DataType.DateTime)]
		[RestrictedDate]
		public DateTime Date { get; set; }

		[Required]
		public string Location { get; set; }

		[DataType(DataType.MultilineText)]
		public string Notes { get; set; }

		[Display(Name ="Pi As Fraction")]
		public string PiAsFraction { get; set; }

		[Required]
		public string Title { get; set; }

		public string FilePath { get; set; }
		public object FishPiCalcs { get; internal set; }
	}
}

public class RestrictedDate : ValidationAttribute
{
	public override bool IsValid(object date)
	{
		DateTime date1 = (DateTime)date;
		return date1 < DateTime.Now;
	}
}