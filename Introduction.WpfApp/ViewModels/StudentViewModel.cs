using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Introduction.WpfApp.ViewModels
{
    public class StudentViewModel : BaseViewModel
    {
        public string MatriculationNumber { get; set; } = string.Empty;
        public string Firstname { get; set; } = string.Empty;
        public string Lastname { get; set; } = string.Empty;

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
        private void Add()
        {
            var model = new Logic.Models.Student()
            {
                MatriculationNumber = this.MatriculationNumber,
                Firstname = this.Firstname,
                Lastname = this.Lastname,
            };
            Task.Run(async () =>
            {
                try
                {
                    await Logic.Data.StudentRepository.InsertAsync(model);
                    await Logic.Data.StudentRepository.SaveChangesAsync();
                    MatriculationNumber = string.Empty;
                    Firstname = string.Empty;
                    Lastname = string.Empty;
                    OnPropertyChanged(nameof(MatriculationNumber));
                    OnPropertyChanged(nameof(Firstname));
                    OnPropertyChanged(nameof(Lastname));
                }
                catch (Exception ex)
                {
                    string caption = "Students";
                    string messageBoxText = ex.Message;

                    MessageBoxButton button = MessageBoxButton.OK;
                    MessageBoxImage icon = MessageBoxImage.Stop;

                    MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);
                }
            }).Wait();
        }
    }
}
