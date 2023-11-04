using EmployeeManagementTool.Contracts;
using EmployeeManagementTool.Models;
using EmployeeManagementTool.Models.Enums;
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

namespace EmployeeManagementTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Employee EmployeeModel { get; set; }
        private readonly IEmployeeAgent _employeeAgent;

        public MainWindow(IEmployeeAgent employeeAgent)
        {
            _employeeAgent = employeeAgent ?? throw new ArgumentNullException(nameof(employeeAgent));
            InitializeComponent();

            Initialize();
        }

        private void Initialize()
        {
            EmployeeModel = new Employee();
            DataContext = EmployeeModel;

            // Set the ComboBox's ItemsSource to the values of the Gender enum
            GenderComboBox.ItemsSource = Enum.GetValues(typeof(Gender));

            // Optionally, you can set the initial selected value (e.g., Male) like this:
            GenderComboBox.SelectedValue = Gender.Male;
        }

        private async void btn_add_employee_Click(object sender, RoutedEventArgs e)
        {
            SetVisibility(showProgressBar: true, showAddEmployeeButton: false);

            var employee = new Employee
            {
                Name = NameTextBox.Text,
                Email = EmailTextBox.Text,
                Gender = (Gender)GenderComboBox.SelectedItem,
            };
            var response = await _employeeAgent.SaveEmployeeAsync(employee);

            SetVisibility(showProgressBar: false, showAddEmployeeButton: true);

            if (response != null && response.IsSuccess)
            {
                MessageBox.Show($"Employee {NameTextBox.Text} Added Sucessfully");
                ClearNewEmployeeForm();
            }
            else
            {
                MessageBox.Show(response.ErrorMessage);
            }
        }

        private void ClearNewEmployeeForm()
        {
            NameTextBox.Text = string.Empty;
            EmailTextBox.Text = string.Empty;
            GenderComboBox.SelectedValue = Gender.Male;
        }

        private void SetVisibility(bool showProgressBar, bool showAddEmployeeButton)
        {
            progressBar.Visibility = showProgressBar ? Visibility.Visible : Visibility.Hidden;
            btn_add_employee.Visibility = showAddEmployeeButton ? Visibility.Visible : Visibility.Hidden;
        }

    }
}
