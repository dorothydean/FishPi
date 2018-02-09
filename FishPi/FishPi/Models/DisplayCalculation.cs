using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace FishPi.Models
{
	public class DisplayCalculation
	{
		static WebsiteDB db = new WebsiteDB();


		public static List<FishPiCalculation> GetAllCalculations()
		{
			List<FishPiCalculation> FishPiCalculations =
				(from p in db.FishPiCalcs
				 select p).ToList();

			return FishPiCalculations;
		}

		internal static void AddCalculation(FishPiCalculation p)
		{
			db.FishPiCalcs.Add(p);
			db.SaveChanges();
		}

		//internal static FishPiCalculation GetProductById(int id)
		//{

		//	FishPiCalculation p = (from prod in db.FishPiCalcs
		//						   where prod.FishPiCalculationID == id
		//						   select prod).SingleOrDefault();
		//	return p;
		//}

		public static void UpdateCalculation(FishPiCalculation p)
		{
			db.Entry(p).State =
				EntityState.Modified;
			db.SaveChanges();
		}

		public static void DeleteCalculation(int value)
		{
			FishPiCalculation p = db.FishPiCalcs.Find(value);
			db.FishPiCalcs.Remove(p);
			db.SaveChanges();
		}
	}
	
}