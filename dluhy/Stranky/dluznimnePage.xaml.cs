using dluhy.Barvy;
using System.Collections.ObjectModel;

namespace dluhy.Stranky;

public partial class dluznimnePage : ContentPage
{
    ObservableCollection<Dluznik> ListDluzniku = new();

    public dluznimnePage()
    {
        InitializeComponent();
        CollectionView.ItemsSource = ListDluzniku;
        Dluznik.ReadFile("dluznicifile.txt", ListDluzniku);
        if(Preferences.Default.Get("isDark", true))
        {
            vymenaBarva(new Dark(), true);


        }
        else
        {
            vymenaBarva(new Light(), false);
        }
    }

    protected override void OnDisappearing()
    {
        gridMenu.IsVisible = false;
        base.OnDisappearing();
    }
    private void vymenaBarva(ResourceDictionary theme, bool isdark)
    {
        ICollection<ResourceDictionary> mergedDictionaries = Application.Current.Resources.MergedDictionaries;
        if (mergedDictionaries != null)
        {
            mergedDictionaries.Clear();
            mergedDictionaries.Add(theme);
        }
        Preferences.Default.Set("theme", isdark);
    }


    private void Button_Clicked(object sender, EventArgs e)
    {
        gridMenu.IsVisible = true;
    }
    private void SaveButton_Clicked(object sender, EventArgs e)
    {
        if (!Dluznik.CreateItem(sender,ListDluzniku,"dluznicifile.txt", sumEntry, nameEntry,descEntry, gridMenu))
        {
            DisplayAlert("Chyba", "Špatná hodnota, zadej èíslo", "OK");
        }
    }

    private void CancelButton_Clicked(object sender, EventArgs e)
    {
        gridMenu.IsVisible = false;
    }

    private void Delete_Clicked(object sender, EventArgs e)
    {
        Dluznik.DeleteItem(sender, ListDluzniku, "dluznicifile.txt");
    }

    private async void Add_Clicked(object sender, EventArgs e)
    {
        var menuItem = (MenuItem)sender;
        var item = (Dluznik)menuItem.BindingContext; // Replace YourDataType with the actual type of your item
        int index = ListDluzniku.IndexOf(item);
        ListDluzniku[index] = item;
        string input = await DisplayPromptAsync("Pøidat", "Kolik pøidat (Kè)", "OK", "Zrušit", null, -1, Keyboard.Numeric);
        if (int.TryParse(input, out int result))
        {
            ListDluzniku[index].Sum += result;
            string popis = await DisplayPromptAsync("Pøidat", "Za co", "OK", "", null, -1, Keyboard.Numeric);
            ListDluzniku[index].Popis += "," + popis;
            Dluznik.SaveFile("dluznicifile.txt", ListDluzniku);
        }
        else
        {
            await DisplayAlert("Chyba", "Špatná hodnota, zadej èíslo", "OK");
        }
    }

    private async void Remove_Clicked(object sender, EventArgs e)
    {
        var menuItem = (MenuItem)sender;
        var item = (Dluznik)menuItem.BindingContext; // Replace YourDataType with the actual type of your item
        int index = ListDluzniku.IndexOf(item);
        ListDluzniku[index] = item;
        string input = await DisplayPromptAsync("Odebrat", "Kolik Odebratt Kè", "OK", "Zrušit", null, -1, Keyboard.Numeric);
        if (int.TryParse(input, out int result))
        {
            ListDluzniku[index].Sum -= result;
            string popis = await DisplayPromptAsync("Odebrat", "Odebrat popis", "OK", "", null, -1, Keyboard.Numeric);
            List<string> PopisList = ListDluzniku[index].Popis.Split(',').ToList();
            PopisList.RemoveAll(x => x.Equals(popis));
            ListDluzniku[index].Popis = string.Join(",", PopisList);
            Dluznik.SaveFile("dluznicifile.txt", ListDluzniku);
        }
        else
        {
            await DisplayAlert("Chyba", "Špatná hodnota, zadej èíslo", "OK");
        }
    }

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        gridMenu.IsVisible = false;

    }
}