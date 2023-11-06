# EmployeeManagementTool

## Project Description
Employee Management Tool is a C# desktop application developed using WPF, allowing users to manage employee details through a RESTful service. The application provides features for adding, editing, removing, viewing, searching, and exporting employee details to CSV or tab-separated files.

## Functionality
The Employee Management Tool provides the following functionalities:
- **Add Employee:** Users can add new employees to the system by providing their details, such as name, email, gender, and more.
- **Edit Employee:** Existing employee information can be edited and updated.
- **Remove Employee:** Employees can be removed from the system, ensuring data accuracy and cleanliness.
- **View Employee:** Users can view detailed information about individual employees, including their name, email, gender, and other relevant data.
- **Search Employee** The tool supports searching for employees based on their name, allowing users to find specific employees quickly.
- **Export Employee Details:** Employee details can be exported to CSV or tab-separated files for reporting or other purposes.

It has two tabs `Add Employee` and `Manage Employee`. The first tab has the Add Employee form, while the other tab lists all Employee records and also provides multiple features as mentioned above.

## RESTful Service
The Employee Management Tool interacts with the RESTful service provided by https://gorest.co.in/public/v2/. The following CRUD operations are supported through the service:
- **List All Users:** `GET /public/v2/users` to retrieve a list of all users.
- **Search Users by Name:** `GET /public/v2/users?name=mark` to list users whose names contain "mark"
- **Create New User:** `POST /public/v2/users` to create a new user.
- **Update User:** `PATCH /public/v2/users/123` to update the details of the user with ID 123.
- **Delete User:** `DELETE /public/v2/users/123` to delete the user with ID 123.

## Windows Application
The Employee Management Tool is developed using Windows Presentation Foundation (WPF) for a modern and user-friendly interface.

## Language and Framework
The application is built using C# and preferably .NET Core for robustness and compatibility.

## Usage
To use the Employee Management Tool, follow these steps:
1. Clone the GitHub repository or obtain the source files.
2. Compile the solution using your preferred C# development environment, ensuring that any necessary dependencies are installed and configured as specified in the additional document.
3. Run the application, and you can start managing employee details through the provided functionalities.

## Additional Dependencies
Make sure to provide the API token in place of the **Token** property in file `appsettings.json`


This Employee Management Tool simplifies the process of managing employee details and ensures data accuracy and efficiency in your organization. Enjoy using it and feel free to contribute to its development on GitHub.
