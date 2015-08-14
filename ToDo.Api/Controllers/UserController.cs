
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using ToDo.Api.Filters;
using WebApi.OutputCache.V2;

namespace ToDo.Api.Controllers
{
    /// <summary>
    /// Controller que fornece as operações dos usuários
    /// </summary>
    [AutoInvalidateCacheOutput]
    [RoutePrefix("api/users")]
    public class UserController : ApiController
    {
        private readonly Models.Entities.User _user = new Models.Entities.User();

        /// <summary>
        /// Recurso que obtem uma coleção de usuários cadastrados.
        /// </summary>
        /// <returns>Retorna uma coleção de usuários</returns>
        [HttpGet]
        [Route("")]
        [CacheOutput(ClientTimeSpan = 60, ServerTimeSpan = 60)]
        [GzipCompression]
        [ResponseType(typeof(ICollection<Models.Entities.User>))]
        [Authorize]
        public async Task<IHttpActionResult> Get()
        {
            try
            {
                var users = await _user.Get();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Recurso que obtem um usuário especifíco.
        /// </summary>
        /// <param name="userId">Identificador do usuário que deseja obter</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{userId:guid}")]
        [ActionName("GetByUserId")]
        [CacheOutput(ClientTimeSpan = 60, ServerTimeSpan = 60)]
        [ResponseType(typeof(Models.Entities.User))]
        [Authorize]
        public async Task<IHttpActionResult> Get(Guid userId)
        {
            try
            {
                var user = await _user.Get(userId);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Recurso que cadastra um novo usuário.
        /// </summary>
        /// <param name="user">Informações do usuário</param>
        /// <returns>Retorna o usuário cadastrado</returns>
        [Authorize]
        [HttpPost]
        [Route("")]
        [ResponseType(typeof(Models.Entities.User))]
        [AllowAnonymous]
        public async Task<IHttpActionResult> Post([FromBody]Models.Entities.User user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    user = await _user.Post(user);
                    return Ok(user);
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Recurso que altera as informações de um determinado usuário
        /// </summary>
        /// <param name="userId">Identificador do usuário que deseja alterar</param>
        /// <param name="user">Informações que deseja alterar no usuário selecionado</param>
        /// <returns>Retorna as informações do usuário alterado</returns>
        [HttpPut]
        [Route("{userId:guid}")]
        [ResponseType(typeof(Models.Entities.User))]
        [Authorize]
        public async Task<IHttpActionResult> Put(Guid userId, [FromBody]Models.Entities.User user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    user = await _user.Put(userId, user);
                    return Ok(user);
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Recurso que deleta logicamente um usuário
        /// </summary>
        /// <param name="userId">Identificado do usuário que deseja deletar logicamente</param>
        /// <returns>Retorna Status 200 quando houver sucesso e 400 quando houver um erro</returns>
        [HttpDelete]
        [Route("{userId:guid}")]
        [Authorize]
        public async Task<IHttpActionResult> Delete(Guid userId)
        {
            try
            {
                await _user.Delete(userId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
