using AppGas.Models;
using AppGas.ViewModels;
using Plugin.Media;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AppGas.ViewModels
{
    public class TasksDetailViewModel : BaseViewModel
    {
        // Variable

        //comandos
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
            TaskSelected = new TaskModel();
        }
        public TasksDetailViewModel(TaskModel taskSelected)
        {
            TaskSelected = taskSelected;
        }

        //metodos 
        private async void CancelAction()
        {
            //regresamos al listado
            await Application.Current.MainPage.Navigation.PopAsync();
        }

        private async void SaveAction()
        {
            //guardamos la tarea en SQLIte
            await App.TaskDatabase.SaveTaskAsync(TaskSelected);

            //refrescamos el listado 
            TasksListViewModel.GetInstance().LoadTasks();

            // Cerrar la pagina
            CancelAction();
        }

        private async void DeleteAction()
        {
            //eliminamos de sqlite
            await App.TaskDatabase.DeleteTaskAsync(TaskSelected);

            //refrescamos el listado 
            TasksListViewModel.GetInstance().LoadTasks();

            // Cerrar la pagina
            CancelAction();
        }

        private async void TakePictureAction()
        {
            //Inicializa la camara
            await CrossMedia.Current.Initialize();

            //Si la camara no esta disponible o no esta soportada lanza un mensaje y termina este método
            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await Application.Current.MainPage.DisplayAlert("No Camera", ":( No camera available.", "OK");
                return;
            }


            //Toma la fotografia y la regresa en el objeto file
            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                Directory = "AppGas",
                Name = "my_cam_picture.jpg"
            });

            //Si el objeto file es null termina este método
            if (file == null) return;

            //Asignamos la ruta de la fotografia en la propiedad de la imagen
            Picture = file.Path;
            //await DisplayAlert("File Location", file.Path, "OK");

            /*image.Source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                return stream;
            });*/
        }

        private async void SelectPictureAction()
        {
            //Inicializa la camara
            await CrossMedia.Current.Initialize();

            //Si la foto seleccionada no esta disponible o no esta soportada lanza un mensaje y termina este método
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await Application.Current.MainPage.DisplayAlert("No pic", ":( No pic available.", "OK");
                return;
            }


            //Selecciona la fotografia y la regresa en el objeto file
            var file = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
            {
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium
            });

            //Si el objeto file es null termina este método
            if (file == null) return;

            //Asignamos la ruta de la fotografia en la propiedad de la imagen
            Picture = file.Path;
        }
    }
}
