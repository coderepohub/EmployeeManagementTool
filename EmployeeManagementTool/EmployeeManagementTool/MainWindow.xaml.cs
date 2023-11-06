using EmployeeManagementTool.Contracts;
using EmployeeManagementTool.Models;
using EmployeeManagementTool.Models.Enums;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace EmployeeManagementTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Employee EmployeeModel { get; set; }
        private readonly IEmployeeAgent _employeeAgent;
        private readonly List<EmployeeDto> _editedEmployeeList;

        public MainWindow(IEmployeeAgent employeeAgent)
        {
            _employeeAgent = employeeAgent ?? throw new ArgumentNullException(nameof(employeeAgent));
            _editedEmployeeList = new List<EmployeeDto>();
            InitializeComponent();
            DataContext = this;

            Initialize();
        }


        #region Initialization
        private void Initialize()
        {
            EmployeeModel = new Employee();
            DataContext = EmployeeModel;

            //Default Gender value
            GenderComboBox.SelectedValue = Gender.Male;
            //Load Employees on form load
            LoadEmployees();
        }

        /// <summary>
        /// Return all Gender values from Gender Enum
        /// </summary>
        public IEnumerable<Gender> GenderValues
        {
            get { return Enum.GetValues(typeof(Gender)).Cast<Gender>(); }
        }

        /// <summary>
        /// Returns all applicable statuses from Status Enum
        /// </summary>
        public IEnumerable<Status> Statuses
        {
            get { return Enum.GetValues(typeof(Status)).Cast<Status>(); }
        }

        /// <summary>
        /// Fetch All Employees
        /// </summary>
        private async void LoadEmployees()
        {
            var employees = await _employeeAgent.GetAllEmployeesAsync();
            EmployeeDataGridXAML.ItemsSource = employees;
        }

        #endregion


        #region Event Handlers
        //Add Employee Handler
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
                LoadEmployees();
            }
            else
            {
                MessageBox.Show(response.ErrorMessage);
            }
        }

        //Edit/Update Employee Handler
        private async void btn_update_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            int employeeId = (int)button.Tag;


            var editedEmployeeData = _editedEmployeeList.FindAll(c => c.Id == employeeId).First();
            if (editedEmployeeData == null)
            {
                MessageBox.Show("There is nothing to update!!");
                return;
            }
            progressSearch.Visibility = Visibility.Visible;
            var result = await _employeeAgent.EditEmployeeAsync(editedEmployeeData);
            _editedEmployeeList.Clear();
            string updateMessage = result ? $"Employee {editedEmployeeData.Name} updated sucessfully!" : $"Updating employee {editedEmployeeData.Name} failed!";
            progressSearch.Visibility = Visibility.Hidden;
            MessageBox.Show(updateMessage);

        }

        //Delete Employee Handler
        private async void btn_delete_Click(object sender, RoutedEventArgs e)
        {
            progressSearch.Visibility = Visibility.Visible;
            Button button = (Button)sender;
            int employeeId = (int)button.Tag;
            var isDeleted = await _employeeAgent.DeleteEmployeeAsync(employeeId);
            string deleteMessage = isDeleted ? $"Employee data deleted sucessfully!" : $"Deleting Employee Data failed!";
            LoadEmployees();
            progressSearch.Visibility = Visibility.Hidden;
            MessageBox.Show(deleteMessage);
        }

        //Search Employee By name Handler
        private async void btn_search_by_name_Click(object sender, RoutedEventArgs e)
        {
            progressSearch.Visibility = Visibility.Visible;
            var result = await _employeeAgent.SearchEmployeeByNameAsync(searchNameTextBox.Text);
            progressSearch.Visibility = Visibility.Hidden;
            EmployeeDataGridXAML.ItemsSource = result;

        }

        //Grid Row Change Handler
        private void EmployeeDataGridXAML_CellEditEnding(object sender,
                                DataGridCellEditEndingEventArgs e)
        {
            var editedEmployeeData = (EmployeeDto)e.Row.Item;
            _editedEmployeeList.Add(editedEmployeeData);
        }

        //Export Data to CSV Handler
        private void btn_export_to_csv_Click(object sender, RoutedEventArgs e)
        {
            // Creating a SaveFileDialog to specify the CSV file location
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "CSV Files (*.csv)|*.csv";
            if (saveFileDialog.ShowDialog() == true)
            {
                string filePath = saveFileDialog.FileName;

                // Get the data from the DataGrid
                var data = EmployeeDataGridXAML.Items.OfType<EmployeeDto>().ToList();

                // Export data to CSV
                ExportToCsv(filePath, data);
            }
        }

        private void ExportToCsv(string filePath, List<EmployeeDto> data)
        {
            // Create a StringBuilder to build the CSV content
            StringBuilder csv = new StringBuilder();

            // Add headers
            csv.AppendLine("Name,Email,Gender,Status");

            // Add data rows
            foreach (var employee in data)
            {
                csv.AppendLine($"{employee.Name},{employee.Email},{employee.Gender}");
            }

            // Write the CSV content to the file
            File.WriteAllText(filePath, csv.ToString());

            // Open the saved CSV file in the default application
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = filePath,
                UseShellExecute = true
            });
        }


        #endregion

        //Clear out the add empoyee form
        private void ClearNewEmployeeForm()
        {
            NameTextBox.Text = string.Empty;
            EmailTextBox.Text = string.Empty;
            GenderComboBox.SelectedValue = Gender.Male;
        }

        //Set Visibility for add employee
        private void SetVisibility(bool showProgressBar, bool showAddEmployeeButton)
        {
            progressBar.Visibility = showProgressBar ? Visibility.Visible : Visibility.Hidden;
            btn_add_employee.Visibility = showAddEmployeeButton ? Visibility.Visible : Visibility.Hidden;
        }

    }
}
