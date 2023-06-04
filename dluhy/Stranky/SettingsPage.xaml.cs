using dluhy.Barvy;

namespace dluhy.Stranky;

public partial class SettingsPage : ContentPage
{
	public SettingsPage()
	{
		InitializeComponent();
	}

    private void Button_Clicked(object sender, EventArgs e)
    {
        if (!Preferences.Default.Get("isDark", true))
        {
            btn.Text = "Dark";
            vymenaBarva(new Dark(), true);
        }
        else
        {
            btn.Text = "Light";
            vymenaBarva(new Light(), false);
        }
    }

    private void vymenaBarva(ResourceDictionary theme, bool isdark)
    {
        ICollection<ResourceDictionary> mergedDictionaries = Application.Current.Resources.MergedDictionaries;
        if (mergedDictionaries != null)
        {
            mergedDictionaries.Clear();
            mergedDictionaries.Add(theme);
        }
        Preferences.Default.Set("isDark", isdark);
    }
}