using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace mazeagent.server.Controllers.Api
{
    [RoutePrefix("api/mazes")]
    public class MazesController : ApiController
    {
        /// <summary>
        /// Return the list of available mazes.
        /// </summary>
        /// <example>
        /// http://example.com/mazes
        /// </example>
        [HttpGet, Route("")]
        public async Task<IHttpActionResult> GetMazes()
        {
            return this.InternalServerError();
        }

        /// <summary>
        /// Returns information about a specific maze.
        /// </summary>
        /// <param name="mId">The m identifier.</param>
        /// <example>
        /// http://example.com/mazes/abc
        /// </example>
        [HttpGet, Route("{mId:string}")]
        public async Task<IHttpActionResult> GetMaze(string mId)
        {
            return this.InternalServerError();
        }

        /// <summary>
        /// Returns information about a specific maze.
        /// </summary>
        /// <param name="mId">The m identifier.</param>
        /// <example>
        /// http://example.com/mazes/abc/xyz
        /// </example>
        [HttpGet, Route("{mId:string}/{cId:string}")]
        public async Task<IHttpActionResult> NavigateToCell(string mId, string cId)
        {
            return this.InternalServerError();
        }


    }
}
