using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Content.Res;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Text;
using Android.Text.Method;
using Android.Text.Util;
using Android.Views;
using Android.Webkit;
using Android.Widget;

namespace keepass2android
{
	public static class ChangeLog
	{
		public static void ShowChangeLog(Context ctx, Action onDismiss)
		{
			AlertDialog.Builder builder = new AlertDialog.Builder(new ContextThemeWrapper(ctx, Android.Resource.Style.ThemeHoloLightDialog));
			builder.SetTitle(ctx.GetString(Resource.String.ChangeLog_title));
			List<string> changeLog = new List<string>{
					
					ctx.GetString(Resource.String.ChangeLog_0_9_6),
					ctx.GetString(Resource.String.ChangeLog_0_9_5),
					ctx.GetString(Resource.String.ChangeLog_0_9_4),
					ctx.GetString(Resource.String.ChangeLog_0_9_3_r5),
					ctx.GetString(Resource.String.ChangeLog_0_9_3),
					ctx.GetString(Resource.String.ChangeLog_0_9_2),
					ctx.GetString(Resource.String.ChangeLog_0_9_1),
					ctx.GetString(Resource.String.ChangeLog_0_9),
					ctx.GetString(Resource.String.ChangeLog_0_8_6),
					ctx.GetString(Resource.String.ChangeLog_0_8_5),
					ctx.GetString(Resource.String.ChangeLog_0_8_4),
					ctx.GetString(Resource.String.ChangeLog_0_8_3),
					ctx.GetString(Resource.String.ChangeLog_0_8_2),
					ctx.GetString(Resource.String.ChangeLog_0_8_1),
					ctx.GetString(Resource.String.ChangeLog_0_8),
					ctx.GetString(Resource.String.ChangeLog_0_7),
					ctx.GetString(Resource.String.ChangeLog)
					 };

			String version;
			try {
				PackageInfo packageInfo = ctx.PackageManager.GetPackageInfo(ctx.PackageName, 0);
				version = packageInfo.VersionName;
				
			} catch (PackageManager.NameNotFoundException) {
				version = "";
			}

			string warning = "";
			if (version.Contains("pre"))
			{
				warning = ctx.GetString(Resource.String.PreviewWarning);
			}

			builder.SetPositiveButton(Android.Resource.String.Ok, (dlgSender, dlgEvt) => {((AlertDialog)dlgSender).Dismiss(); });
			builder.SetCancelable(false);

			WebView wv = new WebView(ctx);

			wv.SetBackgroundColor(Color.White);
			wv.LoadDataWithBaseURL(null, GetLog(changeLog, warning), "text/html", "UTF-8", null);


			//builder.SetMessage("");
			builder.SetView(wv);
			Dialog dialog = builder.Create();
			dialog.DismissEvent += (sender, e) =>
			{
				onDismiss();
			};
			dialog.Show();
			/*TextView message = (TextView)dialog.FindViewById(Android.Resource.Id.Message);

			
			message.TextFormatted = Html.FromHtml(ConcatChangeLog(ctx, changeLog.ToArray()));
			message.AutoLinkMask=MatchOptions.WebUrls;*/

		}

		private const string HtmlStart = @"<html>
  <head>
    <style type='text/css'>
      a            { color:#000000 }
      div.title    { 
          color:287AA9; 
          font-size:1.2em; 
          font-weight:bold; 
          margin-top:1em; 
          margin-bottom:0.5em; 
          text-align:center }
      div.subtitle { 
          color:287AA9; 
          font-size:0.8em; 
          margin-bottom:1em; 
          text-align:center }
      div.freetext { color:#000000 }
      div.list     { color:#000000 }
    </style>
  </head>
  <body>";
		private const string HtmlEnd = @"</body>
</html>";
		private static string GetLog(List<string> changeLog, string warning)
		{
			StringBuilder sb = new StringBuilder(HtmlStart);
			if (!string.IsNullOrEmpty(warning))
			{
				sb.Append(warning);
			}
			bool inList = false;
			foreach (string versionLog in changeLog)
			{
				bool title = true;
				foreach (string line in versionLog.Split('\n'))
				{
					string w = line.Trim();
					if (title)
					{
						if (inList)
						{
							sb.Append("</ul></div>\n");
							inList = false;
						}
						w = w.Replace("<b>","");
						w = w.Replace("</b>", "");
						w = w.Replace("\\n", "");
						sb.Append("<div class='title'>"
								+ w.Trim() + "</div>\n");
						title = false;
					}
					else
					{
						w = w.Replace("\\n", "<br />");
						if (w.StartsWith("*"))
						{
							if (!inList)
							{
								sb.Append("<div class='list'><ul>\n");
								inList = true;
							}
							sb.Append("<li>");
							sb.Append(w.Substring(1).Trim());
							sb.Append("</li>\n");
						}
						else
						{
							if (inList)
							{
								sb.Append("</ul></div>\n");
								inList = false;
							}
							sb.Append(w);
						}
					}
				}
			}
			sb.Append(HtmlEnd);
			return sb.ToString();
		}


		static string ConcatChangeLog(Context ctx, string[] changeLog)
		{
			string res = "";
			bool isFirst = true;
			foreach (string c in changeLog)
			{
				res += c;
				if (isFirst)
				{
					if (res.EndsWith("\n") == false)
						res += "\n";
					string donateUrl = ctx.GetString(Resource.String.donate_url,
														 new Java.Lang.Object[]{ctx.Resources.Configuration.Locale.Language,
						ctx.PackageName
					});
					res += " * <a href=\"" + donateUrl
						+ "\">" +
						ctx.GetString(Resource.String.ChangeLog_keptDonate)
							+ "<a/>";
					isFirst = false;
				}

				while (res.EndsWith("\n\n") == false)
					res += "\n";
			}
			return res.Replace("\n", "<br>");

		}
	}
}