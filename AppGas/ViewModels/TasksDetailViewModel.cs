using AppGas.Models;
using AppGas.Services;

using Plugin.Media;

using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AppGas.ViewModels
{
    public class TasksDetailViewModel : BaseViewModel
    {
        // Variables

        // Comandos
        private Command _CancelCommand;
        public Command CancelCommand => _CancelCommand ?? (_CancelCommand = new Command(CancelAction));

        private Command _SaveCommand;
        public Command SaveCommand => _SaveCommand ?? (_SaveCommand = new Command(SaveAction));

        private Command _DeleteCommand;
        public Command DeleteCommand => _DeleteCommand ?? (_DeleteCommand = new Command(DeleteAction));

        private Command _TakePictureCommand;
        public Command TakePictureCommand => _TakePictureCommand ?? (_TakePictureCommand = new Command(TakePictureAction));

        private Command _SelectPictureCommand;
        public Command SelectPictureCommand => _SelectPictureCommand ?? (_SelectPictureCommand = new Command(SelectPictureAction));

        // Propiedades
        private TaskModel _TaskSelected;
        public TaskModel TaskSelected
        {
            get => _TaskSelected;
            set => SetProperty(ref _TaskSelected, value);
        }

        private string _Picture;
        public string Picture
        {
            get => _Picture;
            set => SetProperty(ref _Picture, value);
        }

        // Constructores
        public TasksDetailViewModel()
        {
            // Es nueva tarea, instanciamos
            TaskSelected = new TaskModel();
        }

        public TasksDetailViewModel(TaskModel taskSelected)
        {
            // Es tarea seleccionada (existente), cargamos
            TaskSelected = taskSelected;
            Picture = taskSelected.ImageBase64;
        }

        // Métodos
        private async void SaveAction()
        {
            // Guardamos la tarea en SQLite
            await App.TaskDatabase.SaveTaskAsync(TaskSelected);

            // Refrescamos el listado
            TasksListViewModel.GetInstance().LoadTasks();

            // Cerramos la página
            CancelAction();
        }

        private async void DeleteAction()
        {
            // Eliminamos la tarea en SQLite
            await App.TaskDatabase.DeleteTaskAsync(TaskSelected);

            // Regrescamos el listado
            TasksListViewModel.GetInstance().LoadTasks();

            // Cerramos la página
            CancelAction();
        }

        private async void CancelAction()
        {
            // Regresamos al listado
            await Application.Current.MainPage.Navigation.PopAsync();
        }

        private async void TakePictureAction()
        {
            // Inicializa la cámara
            await CrossMedia.Current.Initialize();

            // Si la cámara no está disponible o no está soportada lanza un mensaje y termina este método
            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await Application.Current.MainPage.DisplayAlert("No Camera", ":( No camera available.", "OK");
                return;
            }

            // Toma la fotografía y la regresa en el objeto file
            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                Directory = "AppGas",
                Name = "cam_picture.jpg"
            });

            // Si el objeto file es null termina este método
            if (file == null) return;

            // Asignamos la ruta de la fotografía en la propiedad de la imagen
            Picture = TaskSelected.ImageBase64 = await new ImageService().ConvertImageFileToBase64(file.Path); //file.Path;

            /*image.Source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                return stream;
            });*/
        }

        private async void SelectPictureAction()
        {
            // Inicializa la cámara
            await CrossMedia.Current.Initialize();

            // Si el seleccionar fotografía no está disponible o no está soportada lanza un mensaje y termina este método
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await Application.Current.MainPage.DisplayAlert("No Pick Photo", ":( No pick photo available.", "OK");
                return;
            }

            // Selecciona la fotografía del carrete y la regresa en el objeto file
            var file = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
            {
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium
            });

            // Si el objeto file es null termina este método
            if (file == null) return;

            // Asignamos la ruta de la fotografía en la propiedad de la imagen
            Picture = TaskSelected.ImageBase64 = await new ImageService().ConvertImageFileToBase64(file.Path);  //file.Path;

            /*image.Source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                return stream;
            });*/
        }
    }
}