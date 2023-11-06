using EmployeeManagementTool.Contracts;
using EmployeeManagementTool.Models;
using EmployeeManagementTool.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
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
