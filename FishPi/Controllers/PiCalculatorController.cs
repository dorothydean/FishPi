using FishPi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Data;

namespace FishPi.Controllers
{
	[Authorize]
	public class PiCalculatorController : Controller
	{
		static WebsiteDB db = new WebsiteDB();

		// GET: Calculator
		public ActionResult FishPiCalculation()
		{
			return View();
		}

		[HttpPost]
		public ActionResult FishPiCalculation(FishPiCalculation p, HttpPostedFileBase textFile)
		{

			//checks to make sure a file is being uploaded
			//validates file type
			if (textFile == null)
			{
				ModelState.AddModelError("MessageError", "Please upload a .txt file");
			}
			else
			{
				string e = Path.GetExtension(textFile.FileName);
				if (e != ".txt")
				{
					ModelState.AddModelError("MessageError", "Please upload a .txt file");
				}
			}

			//adds path of file to DB
			ModelState.Remove("Title");
			if (ModelState.IsValid)
			{
				if (textFile != null && textFile.ContentLength > 0)
				{
					p.Title = Path.GetFileName(textFile.FileName);
					string fileName = Guid.NewGuid().ToString() + Path.GetExtension(textFile.FileName);

					//generate a path with the file name appended
					string path = Path.Combine(Server.MapPath("~/Files"), fileName);

					textFile.SaveAs(path);
					p.FilePath = fileName;
					DisplayCalculation.AddCalculation(p);
					return RedirectToAction("FishPiCalculation", "PiCalculator");
				}
			}
			return View(p);
		}

		//move to profile page
		[HttpGet]
		public ActionResult CalcsList(int? id)
		{
			int currPage = (id.HasValue) ? id.Value : 1;
			const int CalcsPerPage = 10;

			List<FishPiCalculation> calcs =
				db.FishPiCalcs.OrderBy(p => p.Date)
				  .Skip((currPage - 1) * CalcsPerPage)
				  .Take(CalcsPerPage)
				  .ToList();


			ViewBag.MaxPage = Math.Ceiling(
				db.FishPiCalcs.Count() / (double)CalcsPerPage);
			ViewBag.CurrentPage = currPage;

			return View(calcs);
		}

		[HttpGet]
		public ActionResult Edit(int? id)
		{
			//redirect
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.NotFound);
			}

			FishPiCalculation p = DisplayCalculation.GetCalcById(id.Value);

			if (p == null)
				return HttpNotFound();

			return View(p);
		}


		[HttpPost]
		public ActionResult Edit(FishPiCalculation p)
		{
			//Not writing to DB- why??
			if (ModelState.IsValid)
			{
				DisplayCalculation.UpdateCalculation(p);
				return RedirectToAction("Index", "Manage");
			}
			else {
				ModelState.AddModelError("MessageError", "Your changes were not saved");
			}

			return View(p);
		}

		public ActionResult Delete(int? id)
		{
			DisplayCalculation.DeleteCalculation(id.Value);

			return RedirectToAction("Index","Manage");
		}

		public PartialViewResult GetProds(int id)
		{
			int currPage = id;
			const int ProdsPerPage = 2;

			List<FishPiCalculation> prods =
				db.FishPiCalcs
				.OrderBy(p => p.Date)
				.Skip((currPage - 1) * ProdsPerPage)
				.Take(ProdsPerPage)
				.ToList();

			return PartialView("_PartialProdList", prods);
		}


		//Nei's pi- sum of 
		public int DoTheSum()
		{
			//read file in
			int counter = 0;
			string line;

			//Read the file and display it line by line.  
			//TODO specify which file- newest?  
			System.IO.StreamReader file = new System.IO.StreamReader(@"c:\test.txt");

			//Initialize Dictionary
			Dictionary<string, int> dict = new Dictionary<string, int>();
			List<string> genome = new List<string>();

			while ((line = file.ReadLine()) != null)
			{
				if (line.StartsWith(">"))
				{
					continue;
				}
				else
				{
					genome.Add(line);

					//dict = numerator
					if (dict.ContainsKey(line))
					{
						dict.Add(line, dict[line]++);
					}
					else
					{
						dict.Add(line, 1);
					}
					counter++;
					return counter; //counts total samples- denominator- moved to else because it causes unreachable code if outside the else statement
				}
			}
			file.Close();
			

			//counter is never used, it looks like we're not actually calling the counter in any of the actual calculation code


			//TODO- break into two different methods?  

			List<int> diffSum = new List<int>();

			int count = genome.Count;
			int[][] diffs = new int[count][]; //creates jagged array

			for (int i = 0; i < count; i++)
			{
				diffs[i] = new int[i + 1];
				for (int j = 0; j < i; j++)
				{
					diffs[i][j] = GetHammingDistance(genome[i], genome[j]); //compares two genome strings
				}
			}

			double total = 0;
			for (int i = 0; i < count; i++)
			{
				for (int j = 0; j < i; j++)
				{
					//get value from Dictionary = dict.ElementAt(i).Value
					//(freq i) * (freq j) * Hamming
					total += 1.0 * dict.ElementAt(i).Value * dict.ElementAt(j).Value * diffs[i][j] / counter / counter;
					//1.0 converts everything to double without having to cast
				}
			}

			//placeholder for actual return statement- we want to return total from last for loop
			//wtf do I put??? 
			return 0;

			//display result on screen/store in DB

			//TODO recalculate pi as percent
		}

		public static int GetHammingDistance(string source, string target)
		{
			//number of characters in 1st genome string = target
			//count number of characters in first string
			if (source.Length != target.Length)
			{
				throw new Exception("Strings must be equal length");
			}

			//validate for non AGCT characters

			int distance =
				source.ToCharArray()
				.Zip(target.ToCharArray(), (c1, c2) => new { c1, c2 })
				.Count(m => m.c1 != m.c2);

			return distance;
		}
	}
}