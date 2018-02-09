using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FishPi.Models
{
	public class FishPiCalculation : IValidatableObject
	{
		[Key]
		public int MemberID { get; set; }

		[Required]
		public string Researcher { get; set; }

		[Required (ErrorMessage ="Please enter your date as MM/DD/YYYY")]
		[DataType(DataType.DateTime)]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")] 
		public DateTime Date { get; set; }

		[Required]
		public string Location { get; set; }

		[DataType(DataType.MultilineText)]
		public string Notes { get; set; }

		[Required]
		public string Title { get; set; }

		public string FilePath { get; set; }

		public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			var date = new[] { "Date" };
			if (Date > DateTime.Now)
			{
				yield return new ValidationResult("Sorry Future Date cannot be accepted.", date);
			}
		}//can add an aerror to ModelState
	}
}