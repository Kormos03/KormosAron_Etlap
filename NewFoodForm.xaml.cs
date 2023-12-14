using System.Diagnostics;
using System.Windows;

namespace KormosAron_Etlap
{
    /// <summary>
    /// Interaction logic for NewFoodForm.xaml
    /// </summary>
    public partial class NewFoodForm : Window
    {
        Etel etel;
        EtlapService service;
        public NewFoodForm()
        {
            InitializeComponent();
            service = new EtlapService();
            etel = new Etel();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (validate())
            {

                etel.Nev = NameTextBox.Text;
                etel.Leiras = DescriptionTextBox.Text;
                etel.Ar = int.Parse(PriceTextBox.Text);
                etel.Kategoria = CategoryComboBox.Text;
                Debug.WriteLine("{0},{1},{2},{3}", etel.Nev, etel.Leiras, etel.Ar, etel.Kategoria);
                if (service.Insert(etel))
                {
                    MessageBox.Show("Sikeres hozzáadás!");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Sikertelen Hozzáadás!");
                }

            }
        }

        public bool validate()
        {
            if (NameTextBox.Text.Trim() == null || NameTextBox.Text.Trim() == "")
            {
                MessageBox.Show("Név megadása kötelező!");
                return false;
            }
            else if (DescriptionTextBox.Text.Trim() == null || DescriptionTextBox.Text.Trim() == "")
            {
                MessageBox.Show("Leírás megadása kötelező!");
                return false;
            }
            else if (!int.TryParse(PriceTextBox.Text, out int price))
            {
                MessageBox.Show("Az árnak számnak kell lennie!");
                return false;
            }
            else if (CategoryComboBox.Text.Trim() == null || CategoryComboBox.Text.Trim() == "")
            {
                MessageBox.Show("Kategória megadása kötelező!");
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
