using AppGas.Models;
using AppGas.ViewModels;
using AppGas.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AppGas.ViewModels
{
    public class GasListViewModel : BaseViewModel
    {
        // Variables
        static GasListViewModel instance;

        // Comandos
        private Command _NewCommand;
        public Command NewCommand => _NewCommand ?? (_NewCommand = new Command(NewAction));

        // Propiedades
        private List<GasModel> _Tasks;
        public List<GasModel> Tasks
        {
            get => _Tasks;
            set => SetProperty(ref _Tasks, value);
        }

        private GasModel _TaskSelected;
        public GasModel TaskSelected
        {
            get => _TaskSelected;
            set
            {
                if (SetProperty(ref _TaskSelected, value))
                {
                    SelectTaskAcction();
                }
            }
        }

        // Constructores
        public GasListViewModel()
        {
            // Guardamos en memoria esta misma clase (this)
            instance = this;

            // Cargamos las tareas de inicio
            LoadTasks();
        }

        // Metodos
        public static GasListViewModel GetInstance()
        {
            // Regresamos la instancia de esta misma clase(this)
            return instance;
        }
        public async void LoadTasks()
        {
            // Bindeamos todas las tareas
            Tasks = await App.TaskDatabase.GetAllTasksAsync();
        }
        private void NewAction()
        {
            // Abrimos la pagina de detalle en modalidad de crear una tarea
            Application.Current.MainPage.Navigation.PushAsync(new GasDetailPage());
        }

        private void SelectTaskAcction()
        {
            // Abrimos la pagina de detalle en modalidad de abrir una tarea existente
            Application.Current.MainPage.Navigation.PushAsync(new GasDetailPage(TaskSelected));
        }
    }
}
