using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AppGas.Extensions
{
    public static class TaskExtensions
    {
        /// <param name="task"> es la tarea que se esta extendiendo </param>
        /// <param name="returnToCallingContext">booleano que se manda al metodo Configure await</param>
        /// <param name="onException">accion que se ejecuta cuando hay una excepcion</param>
        public static async void SafeFireAndForget(this Task task, bool returnToCallingContext, Action<Exception> onException = null)
        {
            try
            {
                await task.ConfigureAwait(returnToCallingContext);
            }
            catch (Exception ex) when (onException != null)
            {
                onException(ex);
            }
        }
    }
}
