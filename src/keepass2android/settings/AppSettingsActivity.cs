/*
This file is part of Keepass2Android, Copyright 2013 Philipp Crocoll. This file is based on Keepassdroid, Copyright Brian Pellin.

  Keepass2Android is free software: you can redistribute it and/or modify
  it under the terms of the GNU General Public License as published by
  the Free Software Foundation, either version 3 of the License, or
  (at your option) any later version.

  Keepass2Android is distributed in the hope that it will be useful,
  but WITHOUT ANY WARRANTY; without even the implied warranty of
  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
  GNU General Public License for more details.

  You should have received a copy of the GNU General Public License
  along with Keepass2Android.  If not, see <http://www.gnu.org/licenses/>.
  */

using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Preferences;
using Android.Widget;
using keepass2android.Io;

namespace keepass2android
{
	/// <summary>
	/// Activity to configure the application, without database settings. Does not require an unlocked database, or close when the database is locked
	/// </summary>
	[Activity (Label = "@string/app_name", Theme="@style/NoTitleBar")]			
	public class AppSettingsActivity : LockingPreferenceActivity
	{
		private ActivityDesign _design;
		private KeyboardSwitchPrefManager _switchPrefManager;

		public AppSettingsActivity()
		{
			_design = new ActivityDesign(this);
		}

		public static void Launch(Context ctx)
		{
			ctx.StartActivity(new Intent(ctx, typeof(AppSettingsActivity)));
		}

		protected override void OnCreate(Bundle savedInstanceState) 
		{
			_design.ApplyTheme();
			base.OnCreate(savedInstanceState);
			
			
			AddPreferencesFromResource(Resource.Xml.preferences);
			
			FindPreference(GetString(Resource.String.keyfile_key)).PreferenceChange += OnRememberKeyFileHistoryChanged;
			Preference designPref = FindPreference(GetString(Resource.String.design_key));
			if (!_design.HasThemes())
			{
				try
				{
					((PreferenceScreen)FindPreference(GetString(Resource.String.display_prefs_key))).RemovePreference(designPref);
				}
				catch (Exception ex)
				{
					Kp2aLog.Log(ex.ToString());
					throw;
				}
			}
			else
			{
				designPref.PreferenceChange += (sender, args) => Recreate();
			}

			Util.PrepareNoDonatePreference(this, FindPreference(GetString(Resource.String.NoDonateOption_key)));
			
			Preference cachingPreference = FindPreference(GetString(Resource.String.UseOfflineCache_key));
			cachingPreference.PreferenceChange += OnUseOfflineCacheChanged;

#if NoNet
			try
			{
				((PreferenceScreen) FindPreference(GetString(Resource.String.FileHandling_prefs_key))).RemovePreference(cachingPreference);
			}
			catch (Exception ex)
			{
				Kp2aLog.Log(ex.ToString());	
			}
#endif
			try
			{
				//depending on Android version, we offer to use a transparent icon for QuickUnlock or use the notification priority (since API level 16)
				Preference hideQuickUnlockTranspIconPref = FindPreference(GetString(Resource.String.QuickUnlockIconHidden_key));
				Preference hideQuickUnlockIconPref = FindPreference(GetString(Resource.String.QuickUnlockIconHidden16_key));
				var quickUnlockScreen = ((PreferenceScreen) FindPreference(GetString(Resource.String.QuickUnlock_prefs_key)));
				if ((int) Android.OS.Build.VERSION.SdkInt >= 16)
				{
					quickUnlockScreen.RemovePreference(hideQuickUnlockTranspIconPref);
					FindPreference(GetString(Resource.String.ShowUnlockedNotification_key)).PreferenceChange += (sender, args) => App.Kp2a.UpdateOngoingNotification();
					hideQuickUnlockIconPref.PreferenceChange += OnQuickUnlockHiddenChanged;
				}
				else
				{
					//old version: only show transparent quickUnlock and no option to hide unlocked icon:
					quickUnlockScreen.RemovePreference(hideQuickUnlockIconPref);
					FindPreference(GetString(Resource.String.QuickUnlockIconHidden_key)).PreferenceChange +=
						delegate { App.Kp2a.UpdateOngoingNotification(); };
					((PreferenceScreen) FindPreference(GetString(Resource.String.display_prefs_key))).RemovePreference(
						FindPreference(GetString(Resource.String.ShowUnlockedNotification_key)));
				}
			}
			catch (Exception ex)
			{
				Kp2aLog.Log(ex.ToString());
			}

			FindPreference(GetString(Resource.String.db_key)).Enabled = false;
			_switchPrefManager = new KeyboardSwitchPrefManager(this);

			PrepareSeparateNotificationsPreference(this);

		}

		public static void PrepareSeparateNotificationsPreference(PreferenceActivity preferenceActivity)
		{
			try
			{
				//depending on Android version, we offer to show a combined notification (with action buttons) (since API level 16)
				Preference separateNotificationsPref = preferenceActivity.FindPreference(preferenceActivity.GetString(Resource.String.ShowSeparateNotifications_key));
				var passwordAccessScreen = ((PreferenceScreen)preferenceActivity.FindPreference(preferenceActivity.GetString(Resource.String.password_access_prefs_key)));
				if ((int)Build.VERSION.SdkInt < 16)
				{
					passwordAccessScreen.RemovePreference(separateNotificationsPref);
				}
			}
			catch (Exception ex)
			{
				Kp2aLog.Log(ex.ToString());
			}
		}

		public class KeyboardSwitchPrefManager
		{
			private readonly PreferenceActivity _act;
			private CheckBoxPreference _switchPref;
			private CheckBoxPreference _openKp2aAutoPref;
			private CheckBoxPreference _openOnlyOnSearchPref;
			private CheckBoxPreference _switchBackPref;
			private PreferenceScreen _screen;

			public KeyboardSwitchPrefManager(PreferenceActivity act)
			{
				this._act = act;

				_switchPref = (CheckBoxPreference)_act.FindPreference("kp2a_switch_rooted");
				_openKp2aAutoPref =
					(CheckBoxPreference)act.FindPreference(act.GetString(Resource.String.OpenKp2aKeyboardAutomatically_key));
				_openOnlyOnSearchPref =
					(CheckBoxPreference)
					act.FindPreference(act.GetString(Resource.String.OpenKp2aKeyboardAutomaticallyOnlyAfterSearch_key));
				_switchBackPref =
					(CheckBoxPreference)act.FindPreference(act.GetString(Resource.String.AutoSwitchBackKeyboard_key));
				_screen = (PreferenceScreen)act.FindPreference(act.GetString(Resource.String.keyboardswitch_prefs_key));
				EnableSwitchPreferences(_switchPref.Checked);
				
				_switchPref.PreferenceChange += (sender, args) =>
				{
					bool switchOnRooted = (bool)args.NewValue;
					EnableSwitchPreferences(switchOnRooted);
				};
			}


			private void EnableSwitchPreferences(bool switchOnRooted)
			{
				if (!switchOnRooted)
				{
					if (_act.FindPreference(_act.GetString(Resource.String.OpenKp2aKeyboardAutomatically_key)) == null)
					{
						_screen.AddPreference(_openKp2aAutoPref);
					}
					if (_act.FindPreference(_act.GetString(Resource.String.OpenKp2aKeyboardAutomaticallyOnlyAfterSearch_key)) != null)
					{
						_screen.RemovePreference(_openOnlyOnSearchPref);
					}
					if (_act.FindPreference(_act.GetString(Resource.String.AutoSwitchBackKeyboard_key)) != null)
					{
						_screen.RemovePreference(_switchBackPref);
					}
				}
				else
				{
					if (_act.FindPreference(_act.GetString(Resource.String.OpenKp2aKeyboardAutomatically_key)) != null)
					{
						_screen.RemovePreference(_openKp2aAutoPref);
					}
					if (_act.FindPreference(_act.GetString(Resource.String.OpenKp2aKeyboardAutomaticallyOnlyAfterSearch_key)) == null)
					{
						_screen.AddPreference(_openOnlyOnSearchPref);
					}
					if (_act.FindPreference(_act.GetString(Resource.String.AutoSwitchBackKeyboard_key)) == null)
					{
						_screen.AddPreference(_switchBackPref);
					}
				}
				/*_openKp2aAutoPref.Enabled = !switchOnRooted;

				_openOnlyOnSearchPref.Enabled = switchOnRooted;

				_switchBackPref.Enabled = switchOnRooted;*/
			}
		}

		private void OnDesignChange(object sender, Preference.PreferenceChangeEventArgs preferenceChangeEventArgs)
		{
			Recreate();
		}

		private void OnQuickUnlockHiddenChanged(object sender, Preference.PreferenceChangeEventArgs e)
		{
			App.Kp2a.UpdateOngoingNotification();
		}

		private void OnUseOfflineCacheChanged(object sender, Preference.PreferenceChangeEventArgs e)
		{
			//ensure the user gets a matching database
			if (App.Kp2a.GetDb().Loaded && !App.Kp2a.GetDb().Ioc.IsLocalFile())
				App.Kp2a.LockDatabase(false);

			if (!(bool)e.NewValue)
			{
				AlertDialog.Builder builder = new AlertDialog.Builder(this);
				builder.SetTitle(GetString(Resource.String.ClearOfflineCache_title));

				builder.SetMessage(GetString(Resource.String.ClearOfflineCache_question));

				builder.SetPositiveButton(App.Kp2a.GetResourceString(UiStringKey.yes), (o, args) =>
					 {
						 try
						 {
							 App.Kp2a.ClearOfflineCache();
						 }
						 catch (Exception ex)
						 {
							 Kp2aLog.Log(ex.ToString());
							 Toast.MakeText(Application.Context, ex.Message, ToastLength.Long).Show();
						 }
					 }
					);

				builder.SetNegativeButton(App.Kp2a.GetResourceString(UiStringKey.no), (o, args) =>
				{
				}
				);

				Dialog dialog = builder.Create();
				dialog.Show();

				
			}
		}

		internal static void OnRememberKeyFileHistoryChanged(object sender, Preference.PreferenceChangeEventArgs eventArgs)
		{
			if (!(bool)eventArgs.NewValue)
			{
				App.Kp2a.FileDbHelper.DeleteAllKeys();
			}
		}

		internal static void OnShowUnlockedNotificationChanged(object sender, Preference.PreferenceChangeEventArgs eventArgs)
		{
			App.Kp2a.UpdateOngoingNotification();
		}
	}

}

