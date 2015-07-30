
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using WebApi.OutputCache.V2;

namespace ToDo.Api.Controllers
{
    /// <summary>
    /// Controller que fornece as operações das tarefas.
    /// </summary>
    [RoutePrefix("api/todos")]
    public class ToDoController : ApiController
    {
        private readonly Models.Entities.ToDo _todo = new Models.Entities.ToDo();

        /// <summary>
        /// Recurso que obtem uma coleção de tarefas.
        /// </summary>
        /// <returns>Coleção de tarefas</returns>
        [HttpGet]
        [Route("")]
        [CacheOutput(ClientTimeSpan = 60, ServerTimeSpan = 60)]
        [ResponseType(typeof(ICollection<Models.Entities.ToDo>))]
        public async Task<IHttpActionResult> Get()
        {
            try
            {
                var doings = await _todo.Get();
                return Ok(doings);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Recurso que retorna uma tarefa especifíca.
        /// </summary>
        /// <param name="todoId">Identicador da tarefa desejada</param>
        /// <param name="userId">Identificador do usuário que está relacionado com a tarefa desejada</param>
        /// <returns>Retorna uma tarefa especifíca</returns>
        [HttpGet]
        [Route("{todoId:guid}/{userId}")]
        [ActionName("GetByToDoId")]
        [CacheOutput(ClientTimeSpan = 60, ServerTimeSpan = 60)]
        [ResponseType(typeof(Models.Entities.ToDo))]
        public async Task<IHttpActionResult> Get(Guid todoId, Guid userId)
        {
            try
            {
                var todo = await _todo.Get(todoId, userId);
                return Ok(todo);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Recurso que retorna uma coleção de tarefas de um determinado usuário
        /// </summary>
        /// <param name="userId">Identificador do usuário desejado</param>
        /// <returns>Retorna uma coleção de tarefas</returns>
        [HttpGet]
        [Route("{userId:guid}")]
        [ActionName("GetByUserId")]
        [CacheOutput(ClientTimeSpan = 60, ServerTimeSpan = 60)]
        [ResponseType(typeof(ICollection<Models.Entities.ToDo>))]
        public async Task<IHttpActionResult> Get(Guid userId)
        {
            try
            {
                var doings = await _todo.Get(userId);
                return Ok(doings);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Recurso que cadastra uma nova tarefa.
        /// </summary>
        /// <param name="userId">Identificador do usuário proprietário da tarefa</param>
        /// <param name="todo">Informações da tarefa</param>
        /// <returns>Retorna a tarefa cadastrada</returns>
        [HttpPost]
        [Route("{userId:guid}")]
        [ResponseType(typeof(Models.Entities.ToDo))]
        public async Task<IHttpActionResult> Post(Guid userId, [FromBody]Models.Entities.ToDo todo)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    for (int i = 0; i < 150; i++)
                    {
                        todo.Description = "aaa " + i.ToString();
                        todo = await _todo.Post(userId, todo);
                    }
                    return Ok(todo);
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Recurso que atualiza uma tarefa especifíca.
        /// </summary>
        /// <param name="todoId">Identificador do usuário proprietário da tarefa</param>
        /// <param name="todo">Informações que deseja editar na tarefa</param>
        /// <returns>Retorna a tarefa editada</returns>
        [HttpPut]
        [Route("{todoId:guid}")]
        [ResponseType(typeof(Models.Entities.ToDo))]
        public async Task<IHttpActionResult> Put(Guid todoId, [FromBody]Models.Entities.ToDo todo)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    todo = await _todo.Put(todoId, todo);
                    return Ok(todo);
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Recurso que deleta logicamente uma determinada tarefa.
        /// </summary>
        /// <param name="todoId">Identificador da tarefa que deseja deletar logicamente</param>
        /// <returns>Retorna Status 200 quando houver sucesso e 400 quando houver um erro</returns>
        [HttpDelete]
        [Route("{todoId:guid}")]
        public async Task<IHttpActionResult> Delete(Guid todoId)
        {
            try
            {
                await _todo.Delete(todoId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
