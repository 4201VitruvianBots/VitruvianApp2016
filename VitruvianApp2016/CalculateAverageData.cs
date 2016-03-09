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
			Console.WriteLine ("Start cloud");
			int teamNo = teamNumber;
			IDictionary<string, object> parameters = new Dictionary<string, object>
			{
				{"teamNo", teamNo}
			};
			Console.WriteLine ("about to call cloud");		
			ParseCloud.CallFunctionAsync<int>("avgScore", parameters).ContinueWith(
				t => {
					Console.WriteLine ("GOT RESULTS ");
					Console.WriteLine("**Result: " + t.Result);
					//Console.WriteLine("****Error: " + t.Exception);
				}
			);
			ParseCloud.CallFunctionAsync<int>("lowest3Scores", parameters).ContinueWith(
				t => {
					Console.WriteLine ("GOT RESULTS ");
					Console.WriteLine("**Result: " + t.Result);
					//Console.WriteLine("****Error: " + t.Exception);
				}
			);
			ParseCloud.CallFunctionAsync<int>("highest3Scores", parameters).ContinueWith(
				t => {
					Console.WriteLine ("GOT RESULTS ");
					Console.WriteLine("**Result: " + t.Result);
					//Console.WriteLine("****Error: " + t.Exception);
				}
			);
			ParseCloud.CallFunctionAsync<double>("totalTeleOpShotHighAccuracy", parameters).ContinueWith(
				t => {
					Console.WriteLine ("GOT RESULTS ");
					Console.WriteLine("**Result: " + t.Result);
					//Console.WriteLine("****Error: " + t.Exception);
				}
			);
			ParseCloud.CallFunctionAsync<double>("greatestTeleOpShotHighAccuracy", parameters).ContinueWith(
				t => {
					Console.WriteLine ("GOT RESULTS ");
					Console.WriteLine("**Result: " + t.Result);
					//Console.WriteLine("****Error: " + t.Exception);
				}
			);
			ParseCloud.CallFunctionAsync<double>("worstTeleOpShotHighAccuracy", parameters).ContinueWith(
				t => {
					Console.WriteLine ("GOT RESULTS ");
					Console.WriteLine("**Result: " + t.Result);
					//Console.WriteLine("****Error: " + t.Exception);
				}
			);
			ParseCloud.CallFunctionAsync<double>("totalTeleOpShotLowAccuracy", parameters).ContinueWith(
				t => {
					Console.WriteLine ("GOT RESULTS ");
					Console.WriteLine("**Result: " + t.Result);
					//Console.WriteLine("****Error: " + t.Exception);
				}
			);
			ParseCloud.CallFunctionAsync<double>("greatestTeleOpShotLowAccuracy", parameters).ContinueWith(
				t => {
					Console.WriteLine ("GOT RESULTS ");
					Console.WriteLine("**Result: " + t.Result);
					//Console.WriteLine("****Error: " + t.Exception);
				}
			);
			ParseCloud.CallFunctionAsync<double>("worstTeleOpShotLowAccuracy", parameters).ContinueWith(
				t => {
					Console.WriteLine ("GOT RESULTS ");
					Console.WriteLine("**Result: " + t.Result);
					//Console.WriteLine("****Error: " + t.Exception);
				}
			);
			ParseCloud.CallFunctionAsync<double>("calculateDefenses", parameters).ContinueWith(
				t => {
					Console.WriteLine ("GOT RESULTS ");
					Console.WriteLine("**Result: " + t.Result);
					//Console.WriteLine("****Error: " + t.Exception);
				}
			);
			Console.WriteLine ("done calling cloud");
		}
	}
}

