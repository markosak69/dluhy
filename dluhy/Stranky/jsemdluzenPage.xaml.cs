using System.Collections.ObjectModel;

namespace dluhy.Stranky;

public partial class jsemdluzenPage : ContentPage
{   
    ObservableCollection<Dluznik> ListDluznikuJa = new();

    public jsemdluzenPage()
	{
		InitializeComponent();
        CollectionView.ItemsSource = ListDluznikuJa;
        Dluznik.ReadFile("jsemdluzenfile.txt", ListDluznikuJa);
	}

    protected override void OnDisappearing()
    {
        gridMenu.IsVisible = false;
        base.OnDisappearing();
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        gridMenu.IsVisible = true;
    }
    private void SaveButton_Clicked(object sender, EventArgs e)
    {
        if (!Dluznik.CreateItem(sender, ListDluznikuJa, "jsemdluzenfile.txt", sumEntry, nameEntry, descEntry, gridMenu))
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
        Dluznik.DeleteItem(sender, ListDluznikuJa, "jsemdluzenfile.txt");
    }

    private async void Add_Clicked(object sender, EventArgs e)
    {
        var menuItem = (MenuItem)sender;
        var item = (Dluznik )menuItem.BindingContext; // Replace YourDataType with the actual type of your item
        int index = ListDluznikuJa.IndexOf(item);
        ListDluznikuJa[index] = item;
        string input = await DisplayPromptAsync("Pøidat", "Kolik pøidat (Kè)", "OK" ,"Zrušit" ,null ,-1 ,Keyboard.Numeric);
        if (int.TryParse(input, out int result))
        {
            ListDluznikuJa[index].Sum += result;
            string popis = await DisplayPromptAsync("Pøidat", "Za co", "OK", "", null, -1, Keyboard.Numeric);
            ListDluznikuJa[index].Popis += "," + popis;
            Dluznik.SaveFile("jsemdluzenfile.txt", ListDluznikuJa);
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
        int index = ListDluznikuJa.IndexOf(item);
        ListDluznikuJa[index] = item;
        string input = await DisplayPromptAsync("Odebrat", "Kolik Odebrat Kè", "OK", "Zrušit", null, -1, Keyboard.Numeric);
        if (int.TryParse(input, out int result))
        {
            ListDluznikuJa[index].Sum -= result;
            string popis = await DisplayPromptAsync("Odebrat", "Odebrat popis", "OK", "", null, -1, Keyboard.Numeric);
            List<string> PopisList = ListDluznikuJa[index].Popis.Split(',').ToList();
            PopisList.RemoveAll(x => x.Equals(popis));
            ListDluznikuJa[index].Popis = string.Join(",", PopisList);
            Dluznik.SaveFile("jsemdluzenfile.txt", ListDluznikuJa);
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