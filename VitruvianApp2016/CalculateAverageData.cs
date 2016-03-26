using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Parse;

namespace VitruvianApp2016
{
	public class CalculateAverageData
	{
		public CalculateAverageData (int teamNumber)
		{
			CalculateData (teamNumber);
		}

		async void CalculateData(int teamNumber){
			Console.WriteLine ("Start cloud");
			int teamNo = teamNumber;
			IDictionary<string, object> parameters = new Dictionary<string, object>
			{
				{"teamNo", teamNo}
			};
			Console.WriteLine ("about to call cloud");		
			ParseCloud.CallFunctionAsync<Task>("avgScore", parameters).ContinueWith(
				t => {
					//Console.WriteLine ("GOT RESULTS ");
					//Console.WriteLine("**Result: " + t.Result);
					//Console.WriteLine("****Error: " + t.Exception);
				}
			);
			ParseCloud.CallFunctionAsync<Task>("totalTeleOpShotHighAccuracy", parameters).ContinueWith(
				t => {
					//Console.WriteLine ("GOT RESULTS ");
					//Console.WriteLine("**Result: " + t.Result);
					//Console.WriteLine("****Error: " + t.Exception);
				}
			);
			ParseCloud.CallFunctionAsync<Task> ("totalTeleOpShotLowAccuracy", parameters).ContinueWith (
				t => {
					//Console.WriteLine ("GOT RESULTS ");
					//Console.WriteLine ("**Result: " + t.Result);
					//Console.WriteLine("****Error: " + t.Exception);
				}
			);
			ParseCloud.CallFunctionAsync<Task> ("calculateDefenses", parameters).ContinueWith (
				t => {
					//Console.WriteLine ("GOT RESULTS ");
					//Console.WriteLine ("**Result: " + t.Result);
					//Console.WriteLine("****Error: " + t.Exception);
				}
			);
			Console.WriteLine ("done calling cloud");

		}
	}
}

