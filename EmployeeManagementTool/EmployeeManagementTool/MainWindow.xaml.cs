using EmployeeManagementTool.Contracts;
using EmployeeManagementTool.Models;
using EmployeeManagementTool.Models.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        //    Employees = new ObservableCollection<EmployeeDto>
        //{
        //    new EmployeeDto { Id = 1, Name = "John Doe", Email = "john@example.com", Gender = "Male", Status = "Active" },
        //    new EmployeeDto { Id = 2, Name = "Jane Smith", Email = "jane@example.com", Gender = "Female", Status = "Inactive" },
        //    // Add more employees as needed
        //};
        //    EmployeeDataGridXAML.Items.Add(Employees);
            DataContext = this;

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

            //Load Employees on form load
            LoadEmployees();
        }

        private async void LoadEmployees()
        {
            var employees = await _employeeAgent.GetAllEmployeesAsync();
            EmployeeDataGridXAML.ItemsSource = employees;
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

        private async void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
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
