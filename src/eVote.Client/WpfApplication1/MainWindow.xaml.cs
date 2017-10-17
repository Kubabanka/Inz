using eVote.Database;
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

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var polls = new List<Poll>();
            polls.Add(new Poll("Glosowanie 1", DateTime.Now, DateTime.Now.AddMinutes(5)));
            polls.Add(new Poll("Glosowanie 2", DateTime.Now, DateTime.Now.AddMinutes(5)));
            polls.Add(new Poll("Glosowanie 3", DateTime.Now, DateTime.Now.AddMinutes(5)));
            polls.Add(new Poll("Glosowanie 4", DateTime.Now, DateTime.Now.AddMinutes(5)));
            polls.Add(new Poll("Glosowanie 5", DateTime.Now, DateTime.Now.AddMinutes(5)));
            polls.Add(new Poll("Glosowanie 6", DateTime.Now, DateTime.Now.AddMinutes(5)));
            PollsListBox.ItemsSource = polls;
        }

        private void pollsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
