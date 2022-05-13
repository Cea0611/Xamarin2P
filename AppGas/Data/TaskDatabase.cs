using System;
using SQLite;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using AppGas.Extensions;
using AppGas.Models;

namespace AppGas.Data
{
    public class TaskDatabase
    {
        //Instanciamos y abrimos la conexion a SQLLite donde Lazy nos permite generar la condición sin que se bloquee nuestra app
        static readonly Lazy<SQLiteAsyncConnection> lazyInitializer = new Lazy<SQLiteAsyncConnection>(() =>
        {
            return new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        });

        // Variable estatica para regresar la conexión de SQLite
        static SQLiteAsyncConnection Database => lazyInitializer.Value;

        // Variable estática para saber si la base de datos de SQL está inicializada
        static bool isInitialized = false;

        // Constructor
        public TaskDatabase()
        {
            // Llamamos al metodo de inicializar con la extensi´on de llamado seguro
            InitializedAsync().SafeFireAndForget(false);
        }

        // Método para crear la tabla de tasks si no existiera
        async Task InitializedAsync()
        {
            if (!isInitialized)
            {
                if (!Database.TableMappings.Any(m => m.MappedType.Name == typeof(TaskModel).Name))
                {
                    await Database.CreateTablesAsync(CreateFlags.None, typeof(TaskModel)).ConfigureAwait(false);
                    isInitialized = true;
                }
            }
        }

        // Metodos CRUD para TaskModel
        public Task<List<TaskModel>> GetAllTasksAsync()
        {
            return Database.Table<TaskModel>().ToListAsync();
        }

        public Task<TaskModel> GetTaskAsync(int id)
        {
            return Database.Table<TaskModel>().Where(i => i.ID == id).FirstOrDefaultAsync();
        }

        public Task<List<TaskModel>> GetTasksNotDoneAsync()
        {
            // Recupera el nombre de la tarea que se realizo sin exito
            return Database.QueryAsync<TaskModel>($"SELECT * FROM [{typeof(TaskModel).Name}] WHERE [Done] = 0");
        }

        public Task<List<TaskModel>> GetTasksDoneAsync()
        {
            // Recupera el nombre de la tarea que se realizo con exito
            return Database.QueryAsync<TaskModel>($"SELECT * FROM [{typeof(TaskModel).Name}] WHERE [Done] = 1");
        }

        public Task<int> SaveTaskAsync(TaskModel model)
        {
            if (model.ID == 0)
            {
                // Crear
                return Database.InsertAsync(model);
            }
            else
            {
                // Actualizar
                return Database.UpdateAsync(model);
            }
        }

        public Task<int> DeleteTaskAsync(TaskModel model)
        {
            // Delete
            return Database.DeleteAsync(model);
        }
    }
}
