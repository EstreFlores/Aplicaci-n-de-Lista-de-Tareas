using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;

namespace ToDoListApp
{
    public partial class MainWindow : Window
    {
        private List<Tarea> tareas;

        public MainWindow()
        {
            InitializeComponent();
            tareas = new List<Tarea>();
            LoadTasks();
            UpdatePlaceholders();
        }

        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(titleTextBox.Text) && !string.IsNullOrEmpty(descriptionTextBox.Text))
            {
                var nuevaTarea = new Tarea
                {
                    Titulo = titleTextBox.Text,
                    Descripcion = descriptionTextBox.Text,
                    Completada = false
                };
                tareas.Add(nuevaTarea);
                RefreshTaskList();

                titleTextBox.Clear();
                descriptionTextBox.Clear();
                UpdatePlaceholders();
            }
            else
            {
                MessageBox.Show("Por favor, complete todos los campos.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void DeleteTask_Click(object sender, RoutedEventArgs e)
        {
            if (taskListBox.SelectedItem is Tarea tareaSeleccionada)
            {
                tareas.Remove(tareaSeleccionada);
                RefreshTaskList();

                titleTextBox.Clear();
                descriptionTextBox.Clear();
                UpdatePlaceholders();
            }
            else
            {
                MessageBox.Show("Seleccione una tarea para eliminar.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void SaveTasks_Click(object sender, RoutedEventArgs e)
        {
            SaveTasks();
        }

        private void LoadTasks()
        {
            if (File.Exists("tareas.json"))
            {
                string json = File.ReadAllText("tareas.json");
                tareas = JsonSerializer.Deserialize<List<Tarea>>(json) ?? new List<Tarea>();
                RefreshTaskList();
            }
        }

        private void SaveTasks()
        {
            string json = JsonSerializer.Serialize(tareas);
            File.WriteAllText("tareas.json", json);
        }

        private void RefreshTaskList()
        {
            taskListBox.ItemsSource = null;
            taskListBox.ItemsSource = tareas;
        }

        private void UpdatePlaceholders()
        {
            titlePlaceholder.Visibility = string.IsNullOrEmpty(titleTextBox.Text) ? Visibility.Visible : Visibility.Collapsed;
            descriptionPlaceholder.Visibility = string.IsNullOrEmpty(descriptionTextBox.Text) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void TitleTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            UpdatePlaceholders();
        }

        private void DescriptionTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            UpdatePlaceholders();
        }

        private void TitleTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            titlePlaceholder.Visibility = Visibility.Collapsed;
        }

        private void DescriptionTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            descriptionPlaceholder.Visibility = Visibility.Collapsed;
        }

        public class Tarea
        {
            public string Titulo { get; set; }
            public string Descripcion { get; set; }
            public bool Completada { get; set; }
        }
    }
}
