using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace GmailNotifierPlus.Utilities {
	public class CalendarHelper {

		public static string HttpPostRequest(string url, string post) {
			var encoding = new ASCIIEncoding();
			byte[] data = encoding.GetBytes(post);
			WebRequest request = WebRequest.Create(url);
			request.Method = "POST";
			request.ContentType = "application/x-www-form-urlencoded";
			request.ContentLength = data.Length;
			Stream stream = request.GetRequestStream();
			stream.Write(data, 0, data.Length);
			stream.Close();
			WebResponse response = request.GetResponse();
			String result;
			using (var sr = new StreamReader(response.GetResponseStream())) {
				result = sr.ReadToEnd();
				sr.Close();
			}
			return result;
		}


		public static string HttpGetRequest(string url, string[] headers) {
			String result;
			WebRequest request = WebRequest.Create(url);
			if (headers.Length > 0) {
				foreach (var header in headers) {
					request.Headers.Add(header);
				}
			}
			WebResponse response = request.GetResponse();
			using (var sr = new StreamReader(response.GetResponseStream())) {
				result = sr.ReadToEnd();
				sr.Close();
			}
			return result;
		}

		private const string AuthenticationUrl = "https://www.google.com/accounts/ClientLogin";
		private const string AuthenticationPost = "accountType=GOOGLE&Email=powella@gmail.com&Passwd=aap!31085&service=cl&source=Shellscape-GmailNotifierPlus-Testing";

		private static string Authentication() {
			string key = null;
			string result = HttpPostRequest(AuthenticationUrl, AuthenticationPost);
			var tokens = result.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
			foreach (var item in tokens) {
				if (item.StartsWith("Auth="))
					key = item;
			}
			return key;
		}

		public static void getXMLData(string account) { //, IEnumerable<Dimension> dimensions,
			//  IEnumerable<Metric> metrics, DateTime from, DateTime to, Metric sort, SortDirection direction, int maxrecords) {
			//XDocument doc = null;
			var key = Authentication();

			if (key.Length == 0) {
				return;
			}

			//var dimension = new StringBuilder();
			//for (var i = 0; i < dimensions.Count(); i++) {
			//  dimension.Append("ga:" + dimensions.ElementAt(i));
			//  if (i < dimensions.Count() - 1)
			//    dimension.Append(",");
			//}
			//var metric = new StringBuilder();
			//for (var i = 0; i < metrics.Count(); i++) {
			//  metric.Append("ga:" + metrics.ElementAt(i));
			//  if (i < metrics.Count() - 1)
			//    metric.Append(",");
			//}
			//var sorter = "ga:" + sort;
			//if (direction == SortDirection.Descending)
			//  sorter = "-" + sorter;
			//var fromDate = from.ToString("yyyy-MM-dd");
			//var toDate = to.ToString("yyyy-MM-dd");
			//var url = string.Format(PageViewReportUrl, "ga:" + account, dimension, metric, fromDate, toDate, sorter, maxrecords);
			String url = "https://www.google.com/calendar/feeds/powella@gmail.com/private/full";
			var header = new[] { "Authorization: GoogleLogin " + key.Replace("Auth=", "auth=") };
			String result = HttpGetRequest(url, header);
			String foo = result;
		}

	}
}
