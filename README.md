# Task-Management-System
This project is a TSM(Task Management System) with 2 APIs, in the first API (TMS.Storage.API) you can create, read, update, delete tasks and in the second API(TMS.Report.API) you can generate a CSV file.
I am using Swagger in this project. To learn more about swagger: [swagger](https://swagger.io/)

## TMS.Storage.API configuration:
- You have to change the file "appsettings.json" in the part of "ConnectionStrings": server for your SQLServer name and Database for your Database name.
- You have to change the route on lines 7 and 9 for the route of your files of SQLServer before execute the script.sql
- 3 types of State: Planned, inProgress and Completed.

## TMS.Storage.API endpoints:
- GET **/Tasks**: You will obtain a list of all task in the database.

- GET **/Task/{id}**: You will obtain the details of a task. It is necessary to put the Id of that task.
    >Example of id: *90a6dadf-3714-4944-b94c-42f928fc37bb*
    
- GET **/Subtasks/{id}**: You will obtain the details of a subtask. It is necessary to put the Id of that subtask.
    >Example of id: *d5a4ecd4-cbc9-47f4-a757-3f1c6d331fe5*
    
- GET **/Task/{id}/Subtasks**: You will obtain a list of all subtaks for a certain task. It is necessary tu put the Id of the task.
    >Example of id: *90a6dadf-3714-4944-b94c-42f928fc37bb*
    
- POST **/Task/Create**: You will create a task in the database. It is necessary to send a Name, a Description and a State for the new task.
    Example of request: 
    ```
    {
      "name": "Task 6",
      "description": "Doing task 6",
      "state": "Planned"
    }
    ```
- POST **/Subtask/Create**: You will create a subtask in the database. It is necessary to send the Id of the task connected to this subtask, a Name, a Description and a State for the new subtask.
    Example of request: 
    ```
    {
      "name": "Subtask 6",
      "description": "Doing subtask 6",
      "state": "Planned",
      "idTask": "90a6dadf-3714-4944-b94c-42f928fc37bb"
    }
     ```
- PUT **/Tasks/Edit/{id}**: You will edit a task. It is necessary to put the Id of the Task and send a Name, a Description and a State.
    >Example of id: *90a6dadf-3714-4944-b94c-42f928fc37bb*
    Example of request: 
    ```
    {
      "name": "Task 6",
      "description": "Doing task 6",
      "state": "Planned"
    }
    ```
- PUT **/Subtasks/Edit/{id}**: You will edit a subtask. It is necessary to put the Id of the Subtask and send a Name, a Description and a State.
    >Example of id: *d5a4ecd4-cbc9-47f4-a757-3f1c6d331fe5*
    Example of request: 
    ```
    {
      "name": "Subtask 6",
      "description": "Doing subtask 6",
      "state": "Planned"
    }
    ```
- DELETE **/Tasks/Delete/{id}**: You will delete a task. It is necessary to put the Id of the Task.
    >Example of id: *90a6dadf-3714-4944-b94c-42f928fc37bb*
    
- DELETE **/Subtasks/Delete/{id}**: You will delete a task. It is necessary to put the Id of the Task.
    >Example of id: *d5a4ecd4-cbc9-47f4-a757-3f1c6d331fe5*

## TMS.Report.API endpoints:
- GET **/CSV**: You will download a CSV file with the task inProgress for a certain Date. It is necessary to put the DateTime.
    >Example of DateTime: *2012-03-19 07:22:12.000*
