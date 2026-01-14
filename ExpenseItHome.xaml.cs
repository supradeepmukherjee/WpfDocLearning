using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;

namespace WpfDocLearning
{
    /// <summary>
    /// Interaction logic for ExpenseItHome.xaml
    /// </summary>
    public partial class ExpenseItHome : Page
    {
        public ExpenseItHome()
        {
            InitializeComponent();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            XmlElement selectedPerson =
                peopleListBox.SelectedItem as XmlElement;

            if (selectedPerson == null)
            {
                MessageBox.Show("Please select a person first.");
                return;
            }

            ExpenseReportPage expenseReportPage =
                new ExpenseReportPage(selectedPerson);
            NavigationService.Navigate(expenseReportPage);
        }
    }
}
