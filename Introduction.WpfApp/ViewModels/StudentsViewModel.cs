using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Introduction.WpfApp.ViewModels
{
    public class StudentsViewModel : BaseViewModel
    {
        public ObservableCollection<Logic.Models.Student> Students
        {
            get
            {
                var models = Task.Run(async () => await Logic.Data.StudentRepository.GetAllAsync()).Result;

                models = models == null ? Array.Empty<Logic.Models.Student>() : models;

                return new ObservableCollection<Logic.Models.Student>(models);
            }
        }
        public int DataItemWidth { get; set; } = 200;

        private ICommand? commandAdd;
        public ICommand CommandAdd
        {
            get
            {
                return RelayCommand.CreateCommand(ref commandAdd, p =>
                {
                    Add();
                },
                p => true); // Bedingung fuer den Button aktiv/deaktiv
            }
        }

        private ICommand? commandDelete;
        public ICommand CommandDelete
        {
            get
            {
                return RelayCommand.CreateCommand(ref commandDelete, p =>
                {
                    Delete(Convert.ToInt32(p));   // Kommand fuer den Button
                },
                p => true); // Bedingung fuer den Button aktiv/deaktiv
            }
        }

        public void Add()
        {
            var addWindow = new AddWindow();

            addWindow.ShowDialog();

            OnPropertyChanged(nameof(Students));
        }
        public void Delete(int id)
        {
            string caption = "Students";
            string messageBoxText = "Are you sure you want to delete this?";

            MessageBoxButton button = MessageBoxButton.YesNo;
            MessageBoxImage icon = MessageBoxImage.Stop;

            var result = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);

            if (result == MessageBoxResult.Yes)
            {
                Task.Run(async () =>
                {
                    await Logic.Data.StudentRepository.DeleteAsync(id);
                    await Logic.Data.StudentRepository.SaveChangesAsync();
                }).Wait();
                OnPropertyChanged(nameof(Students));
            }
        }

        public Visibility DeleteVisible => commandDelete != null ? (commandDelete.CanExecute(null) ? Visibility.Visible : Visibility.Hidden) : Visibility.Hidden;
    }
}
