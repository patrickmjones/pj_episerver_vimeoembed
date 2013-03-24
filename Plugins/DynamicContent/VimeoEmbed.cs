/*  
   Copyright 2012 Patrick Jones - EPiServer Vimeo Video DynamicContent Plugin

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.  
 */

using System.Collections.Specialized;
using System.Text;
using EPiServer;
using EPiServer.Core;
using EPiServer.DynamicContent;
using EPiServer.PlugIn;

namespace PJEPiServer.Plugins.DynamicContent
{
	[DynamicContentPlugIn(
			DisplayName = "Vimeo Video",
			Description = "Displays a vimeo video",
			Area = PlugInArea.DynamicContent)]
	public class VimeoEmbed : IDynamicContent
	{
		/// <summary>
		/// The video width
		/// </summary>
		public string keyWidth = "Video Width";

		/// <summary>
		/// The video height
		/// </summary>
		public string keyHeight = "Video Height";

		/// <summary>
		/// The Video ID
		/// </summary>
		public string keyID = "Video ID";

		/// <summary>
		/// Show the title on the video. Defaults to 1
		/// </summary>
		public string keyTitle = "Show Title";

		/// <summary>
		/// Show the user’s byline on the video. Defaults to 1.
		/// </summary>
		public string keyShowByLine = "Show ByLine";

		/// <summary>
		/// Show the user’s portrait on the video. Defaults to 1.
		/// </summary>
		public string keyShowPortrait = "Show Portrait";

		/// <summary>
		/// Specify the color of the video controls. Defaults to 00adef. Make sure that you don’t include the #.
		/// </summary>
		public string keyColor = "Color";

		/// <summary>
		/// Play the video automatically on load. Defaults to 0. Note that this won’t work on some devices.
		/// </summary>
		public string keyAutoplay = "Allow Autoplay";

		/// <summary>
		/// Play the video again when it reaches the end. Defaults to 0.
		/// </summary>
		public string keyLoop = "Loop Video";

		public VimeoEmbed()
		{
			Properties.Add(keyID, new PropertyString());
			Properties[keyID].IsRequired = true;

			// Options
			Properties.Add(keyTitle, new PropertyBoolean());
			Properties[keyTitle].Value = true;

			Properties.Add(keyShowByLine, new PropertyBoolean());
			Properties[keyShowByLine].Value = true;

			Properties.Add(keyShowPortrait, new PropertyBoolean());
			Properties[keyShowPortrait].Value = true;

			Properties.Add(keyColor, new PropertyString());

			Properties.Add(keyAutoplay, new PropertyBoolean());
			Properties[keyAutoplay].Value = false;

			Properties.Add(keyLoop, new PropertyBoolean());
			Properties[keyLoop].Value = false;

			// Dimensions
			Properties.Add(keyWidth, new PropertyNumber());
			Properties[keyWidth].Value = 480;

			Properties.Add(keyHeight, new PropertyNumber());
			Properties[keyHeight].Value = 360;
		}

		public System.Web.UI.Control GetControl(PageBase hostPage)
		{
			return null;
		}

		private PropertyDataCollection _properties;
		public EPiServer.Core.PropertyDataCollection Properties
		{
			get
			{
				if (_properties == null)
				{
					_properties = new PropertyDataCollection();
				}
				return _properties;
			}
			set { _properties = value; }
		}

		public string Render(PageBase hostPage)
		{
			NameValueCollection queryString = System.Web.HttpUtility.ParseQueryString(string.Empty);
			queryString["title"] = GetPropertyValue(keyTitle) ? "1" : "0";
			queryString["byline"] = GetPropertyValue(keyShowByLine) ? "1" : "0";
			queryString["portrait"] = GetPropertyValue(keyShowPortrait) ? "1" : "0";
			queryString["autoplay"] = GetPropertyValue(keyAutoplay) ? "1" : "0";
			queryString["loop"] = GetPropertyValue(keyLoop) ? "1" : "0";

			StringBuilder embed = new StringBuilder();
			embed.AppendFormat("<iframe class=\"vimeo-video\" width=\"{0}\" height\"{1}\" src=\"http://player.vimeo.com/video/{2}?{3}\" webkitAllowFullScreen mozallowfullscreen allowFullScreen></iframe>",
					Properties[keyWidth].Value, Properties[keyHeight].Value, Properties[keyID].Value, queryString.ToString());
			return embed.ToString();
		}

		public bool RendersWithControl
		{
			get { return false; }
		}

		public string State
		{
			get
			{
				return string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}",
									 Properties[keyID].Value,
									 Properties[keyTitle].Value,
									 Properties[keyShowByLine].Value,
									 Properties[keyShowPortrait].Value,
									 Properties[keyColor].Value,
									 Properties[keyAutoplay].Value,
									 Properties[keyLoop].Value,
									 Properties[keyWidth].Value,
									 Properties[keyHeight].Value
								);
			}
			set
			{
				var values = value.Split('|');

				if (values.Length == 9)
				{
					Properties[keyID].Value = values[0];
					Properties[keyTitle].Value = values[1];
					Properties[keyShowByLine].Value = values[2];
					Properties[keyShowPortrait].Value = values[3];
					Properties[keyColor].Value = values[4];
					Properties[keyAutoplay].Value = values[5];
					Properties[keyLoop].Value = values[6];
					Properties[keyWidth].Value = values[7];
					Properties[keyHeight].Value = values[8];
				}
			}
		}

		protected bool GetPropertyValue(string key)
		{
			if (Properties[key] == null || Properties[key].Value == null)
			{
				return false;
			}
			return (bool)Properties[key].Value;
		}
	}
}
