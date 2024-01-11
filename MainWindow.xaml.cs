using System.Collections.Generic;
using System.Windows;

namespace KormosAron_Etlap
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        NewFoodForm newFoodForm;
        EtlapService service;
        List<Etel> etelek;
        public MainWindow()
        {
            InitializeComponent();
            service = new EtlapService();
            foodList.ItemsSource = service.GetAll();
            etelek = service.GetAll();
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            newFoodForm = new NewFoodForm();
            newFoodForm.ShowDialog();
            foodList.ItemsSource = service.GetAll();
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {

            if (foodList.SelectedItem == null)
            {
                MessageBox.Show("Nincs kiválasztva étel!");
                return;
            }
            MessageBoxResult result = MessageBox.Show("Biztosan törölni akarod?", "Törlés", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.No)
            {
                return;
            }
            if (service.Delete((Etel)foodList.SelectedItem!))
            {
                MessageBox.Show("Sikeres törlés!");
            }
            else
            {
                MessageBox.Show("Sikertelen törlés!");
            }
            foodList.ItemsSource = service.GetAll();
        }

        private void aremelesButton_Click(object sender, RoutedEventArgs e)
        {
            service.GetAll();
            Etel etel = new Etel();
            if (szazalekosCheckBox.IsChecked.Value)
            {
                if (!int.TryParse(aremelesTextBox.Text, out int newar) || newar < 5 || newar > 50 || newar % 5 != 0)
                {
                    MessageBox.Show("Az áremelésnek számnak kell lennie és 5% és 50% közötti értéket vehet fel 5%-os lépésekkel!");
                    return;
                }
            }
            else
            {
                if (!int.TryParse(aremelesTextBox.Text, out int newar) || newar < 50 || newar > 3000 || newar % 50 != 0)
                {
                    MessageBox.Show("Az áremelésnek számnak kell lennie és 50Ft és 3000Ft közötti értéket vehet fel 50Ft-os lépésekkel!");
                    return;
                }
            }
            if (foodList.SelectedItem == null)
            {
                MessageBoxResult result = MessageBox.Show("Biztosan emelni akarod az összes ételnek az árát?", "Áremelés", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.No)
                {
                    return;
                }
                etelek.ForEach(etel =>
                {

                    service.Modify(etel, int.Parse(aremelesTextBox.Text), szazalekosCheckBox.IsChecked.Value);
                });
            }
            else
            {
                etel = (Etel)foodList.SelectedItem!;

                MessageBoxResult result = MessageBox.Show($"Biztosan emelni akarod az étel árát?: {etel.Nev}", "Áremelés", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.No)
                {
                    return;
                }
                service.Modify(etel, int.Parse(aremelesTextBox.Text), szazalekosCheckBox.IsChecked.Value);

            }
            foodList.ItemsSource = service.GetAll();
        }
    }
}
