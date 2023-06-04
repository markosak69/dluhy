using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace dluhy
{
    class Dluznik : INotifyPropertyChanged
    {
        private string _name;
        private int _sum;
        private string _popis;

        public static async void ReadFile(string fileName, ObservableCollection<Dluznik> SentList)
        {
            string mainDir = FileSystem.Current.AppDataDirectory;
            string filePath = System.IO.Path.Combine(mainDir, fileName);

            try
            {
                using Stream fileStream = System.IO.File.OpenRead(filePath);
                using StreamReader reader = new StreamReader(fileStream);
                string fileContent = await reader.ReadToEndAsync();
                string[] arrayclass = fileContent.Split('.');
                arrayclass = arrayclass.SkipLast(1).ToArray();
                foreach (var item in arrayclass)
                {
                    string[] arrayitem = item.Split(';');
                    SentList.Add(new Dluznik
                    {
                        Name = arrayitem[0],
                        Sum = int.Parse(arrayitem[1]),
                        Popis = arrayitem[2],
                    });
                }
                fileStream.Dispose();
                reader.Dispose();

                
            }
            catch (Exception)
            {
            }
        }

        public static async void SaveFile(string fileName, ObservableCollection<Dluznik> ListDluzniku)
        {
            string mainDir = FileSystem.Current.AppDataDirectory;
            string filePath = System.IO.Path.Combine(mainDir, fileName);
            File.Delete(filePath);

            using FileStream outputStream = System.IO.File.OpenWrite(filePath);
            using StreamWriter streamWriter = new StreamWriter(outputStream);
            await streamWriter.WriteAsync(string.Empty);

            foreach (var item in ListDluzniku)
            {
                string itemString = (item.Name + ";" + item.Sum.ToString() + ";" + item.Popis + ".");
                try
                {
                    await streamWriter.WriteAsync(itemString);
                    // File saved successfully
                    
                }
                catch (Exception ex)
                {
                    // Handle the exception (e.g., show an error message)
                }
            }
        }
        public static bool CreateItem(object sender, ObservableCollection<Dluznik> ListDluzniku, string fileName, Entry sumEntry, Entry nameEntry, Entry descEntry, Grid gridMenu)
        {
            Dluznik newItem = new Dluznik();

            if (int.TryParse(sumEntry.Text, out int result))
            {
                newItem.Name = nameEntry.Text;
                newItem.Sum = result;
                newItem.Popis = descEntry.Text;

                ListDluzniku.Add(newItem);
                Dluznik.SaveFile(fileName, ListDluzniku);

                nameEntry.Text = null;
                sumEntry.Text = null;
                descEntry.Text = null;
            }
            else
            {
                return false; /*DisplayAlert("Chyba", "Špatná hodnota, zadej číslo", "OK");*/
            }
            gridMenu.IsVisible = false;
            return true;
        }

        public static void DeleteItem(object sender, ObservableCollection<Dluznik> ListDluzniku, string fileName)
        {
            var menuItem = (MenuItem)sender;
            var item = (Dluznik)menuItem.BindingContext; // Replace YourDataType with the actual type of your item
            ListDluzniku.Remove(item);
            Dluznik.SaveFile(fileName, ListDluzniku);
        }



        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged();
                }
            }
        }
        public int Sum
        {
            get { return _sum; }
            set
            {
                if (_sum != value)
                {
                    _sum = value;
                    OnPropertyChanged();
                }
            }
        }
        public string Popis
        {
            get { return _popis; }
            set
            {
                if (_popis != value)
                {
                    _popis = value;
                    OnPropertyChanged();
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}

