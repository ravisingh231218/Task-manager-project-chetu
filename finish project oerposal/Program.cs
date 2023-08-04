// See https://aka.ms/new-console-template for more information
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

[Serializable]
public class Task
{
    public string TaskName { get; set; }
    public string Description { get; set; }
    public DateTime DueDate { get; set; }
    public bool IsCompleted { get; set; }
}

class Program
{
    private const string TasksFileName = "tasks.json";
    private static List<Task> tasks = new List<Task>();

    static void Main()
    {
        LoadTasksFromFile();

        int choice;
        do

        {
            Console.WriteLine("<<<<<Task Manager>>>>>");
            Console.WriteLine("1. Add a Task");
            Console.WriteLine("2. View Tasks");
            Console.WriteLine("3. Mark Completed");
            Console.WriteLine("4. Delete Task");
            Console.WriteLine("5. Exit");
            Console.Write("Enter your choice: ");

            if (int.TryParse(Console.ReadLine(), out choice))
            {
                switch (choice)
                {
                    case 1:
                        AddTask();
                        break;
                    case 2:
                        ViewTasks();
                        break;
                    case 3:
                        MarkCompleted();
                        break;
                    case 4:
                        DeleteTask();
                        break;
                    case 5:
                        SaveTasksToFile();
                        Console.WriteLine("Exiting Task Manager...");
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Invalid choice. Please try again.");
            }

            Console.WriteLine();
        } while (true);
    }

    private static void AddTask()
    {
        Console.WriteLine("<<<<<<<Add a Task>>>>>>>");
        Console.Write("Enter Task Name: ");
        string taskName = Console.ReadLine();

        Console.Write("Enter Description: ");
        string description = Console.ReadLine();

        DateTime dueDate;
        while (true)
        {
            Console.Write("Enter Due Date (yyyy-mm-dd): ");
            if (DateTime.TryParse(Console.ReadLine(), out dueDate))
                break;
            else
                Console.WriteLine("Invalid date format. Please try again.");
        }

        tasks.Add(new Task { TaskName = taskName, Description = description, DueDate = dueDate, IsCompleted = false });
        Console.WriteLine("Task added successfully!");
    }

    private static void ViewTasks()
    {
        Console.WriteLine("<<<<<<<<View Tasks>>>>>>>>");
        int taskNumber = 1;
        foreach (var task in tasks)
        {
            string status = task.IsCompleted ? "[Completed]" : "[Incomplete]";
            Console.WriteLine($"{taskNumber}. {status} {task.TaskName} (Due: {task.DueDate:yyyy-MM-dd})");
            Console.WriteLine($"   Description: {task.Description}");
            taskNumber++;
        }
    }

    private static void MarkCompleted()
    {
        Console.WriteLine("<<<<<<<<Mark Completed>>>>>>>");
        Console.Write("Enter the task number to mark as completed: ");
        if (int.TryParse(Console.ReadLine(), out int taskNumber) && taskNumber > 0 && taskNumber <= tasks.Count)
        {
            tasks[taskNumber - 1].IsCompleted = true;
            Console.WriteLine("Task marked as completed!");
        }
        else
        {
            Console.WriteLine("Invalid task number. Please try again.");
        }
    }

    private static void DeleteTask()
    {
        Console.WriteLine("<<<<<<<<Delete Task>>>>>>");
        Console.Write("Enter the task number to delete: ");
        if (int.TryParse(Console.ReadLine(), out int taskNumber) && taskNumber > 0 && taskNumber <= tasks.Count)
        {
            tasks.RemoveAt(taskNumber - 1);
            Console.WriteLine("Task deleted successfully!");
        }
        else
        {
            Console.WriteLine("Invalid task number. Please try again.");
        }
    }

    private static void SaveTasksToFile()
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(tasks, options);
        File.WriteAllText(TasksFileName, json);
    }

    private static void LoadTasksFromFile()
    {
        if (File.Exists(TasksFileName))
        {
            string json = File.ReadAllText(TasksFileName);
            tasks = JsonSerializer.Deserialize<List<Task>>(json);
        }
    }
}
