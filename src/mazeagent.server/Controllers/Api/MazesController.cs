using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using mazeagent.core.Models;
using mazeagent.mazeplusxml.Components;
using mazeagent.server.Helpers;
using mazeagent.server.Models.Output;
using mazeagent.server.Storage;

namespace mazeagent.server.Controllers.Api
{
    [EnableCors("*","","GET")]
    [RoutePrefix("api/mazes")]
    public class MazesController : ApiController
    {
        public const string RoutePrefix = "api/mazes";

        private Uri RelativeUriFromString(params string[] parts)
        {
            return new Uri(string.Join("/", parts), UriKind.Relative);
        }

        /// <summary>
        /// Return the list of available mazes.
        /// </summary>
        /// <example>
        /// http://example.com/api/mazes
        /// </example>
        [HttpGet, Route("", Name = "maze-home")]
        public HttpResponseMessage GetMazes(HttpRequestMessage request)
        {
            var currentMaze = MazeRepository.Instance.CurrentMaze;
            var linkBuilder = new LinkBuilder(request);

            var mazeVm =
                new MazeVm(linkBuilder.ResolveApplicationUri(this.RelativeUriFromString(RoutePrefix, currentMaze.ID)))
                {
                    Start =
                        linkBuilder.ResolveApplicationUri(this.RelativeUriFromString(RoutePrefix, currentMaze.ID,
                            currentMaze.Start.ID)),
                    Length = currentMaze.Size.Height,
                    Width = currentMaze.Size.Width
                };

            var mazeCollection = new MazeCollectionVm(request.RequestUri);
            mazeCollection.Mazes.Add(mazeVm);

            return Request.CreateResponse(HttpStatusCode.OK, mazeCollection);
        }

        /// <summary>
        /// Returns information about a specific maze.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <example>
        /// http://example.com/api/mazes/abc
        /// </example>
        [HttpGet, Route("{mId}")]
        public HttpResponseMessage GetMaze(HttpRequestMessage request)
        {
            var routeData = request.GetRouteData();
            var mazeid = (string) routeData.Values["mId"];

            var currentMaze = MazeRepository.Instance.CurrentMaze;
            if (mazeid != currentMaze.ID)
            {
                var errorDoc = new MazeErrorVm(request.RequestUri, "The requested maze was not found. The maze may have been retired."); 
                return Request.CreateResponse(HttpStatusCode.NotFound, errorDoc);
            }

            var linkBuilder = new LinkBuilder(request);
            var mazeVm =
                new MazeVm(linkBuilder.ResolveApplicationUri(this.RelativeUriFromString(RoutePrefix, currentMaze.ID)))
                {
                    Start =
                        linkBuilder.ResolveApplicationUri(this.RelativeUriFromString(RoutePrefix, currentMaze.ID,
                            currentMaze.Start.ID)),
                    Length = currentMaze.Size.Height,
                    Width = currentMaze.Size.Width
                };

            return Request.CreateResponse(HttpStatusCode.OK, mazeVm);
        }

        /// <summary>
        /// Returns information for navigating a maze
        /// </summary>
        /// <param name="request">The request.</param>
        /// <example>
        /// http://example.com/mazes/abc/xyz
        /// </example>
        [HttpGet, Route("{mId}/{cId}")]
        public HttpResponseMessage NavigateToCell(HttpRequestMessage request)
        {
            var routeData = request.GetRouteData();
            var mazeId = (string)routeData.Values["mId"];
            var cellId = (string) routeData.Values["cId"];

            var currentMaze = MazeRepository.Instance.CurrentMaze;
            if (mazeId != currentMaze.ID)
            {
                var errorDoc = new MazeErrorVm(request.RequestUri, "The requested maze was not found. The maze may have been retired.");
                return Request.CreateResponse(HttpStatusCode.NotFound, errorDoc);
            }

            var linkBuilder = new LinkBuilder(request);
            var cell = new MazeCellVm(linkBuilder.ResolveApplicationUri(this.RelativeUriFromString(RoutePrefix, currentMaze.ID, cellId)));

            IEnumerable<Maze.Edge> neighbors;
            try
            {
                neighbors = currentMaze.AccessibleNeighborsOf(cellId);
            }
            catch (ArgumentOutOfRangeException)
            {
                var errorDoc = new MazeErrorVm(request.RequestUri, "The requested cell was not found or the maze may have been retired.");
                return Request.CreateResponse(HttpStatusCode.NotFound, errorDoc);
            }

            foreach (var neighbor in neighbors.Where(c => c.Direction != Directions.Exit))
            {
                var link = new LinkVm(
                    linkBuilder.ResolveApplicationUri(this.RelativeUriFromString(RoutePrefix, currentMaze.ID, neighbor.Cell.ID)),
                    neighbor.Direction.ToString());
                cell.Links.Add(link);
            }

            if (neighbors.Any(c => c.Direction == Directions.Exit))
            {
                // the exit link returns the url with the entrance to the maze    
                var link = new LinkVm(
                    linkBuilder.ResolveApplicationUri(this.RelativeUriFromString(RoutePrefix, currentMaze.ID)),
                    Directions.Exit.ToString());
                cell.Links.Add(link);
            }

            return Request.CreateResponse(HttpStatusCode.OK, cell);
        }

        /// <summary>
        /// A fallback action method for GET requests that will return a 404
        /// </summary>
        /// <exception cref="System.Web.Http.HttpResponseException"></exception>
        [HttpGet, Route("{*url}", Order = 3)]
        public HttpResponseMessage NoMatch(HttpRequestMessage request)
        {
            var response = new HttpResponseMessage(HttpStatusCode.NotFound)
            {
                Content = new StringContent("The requested resource is not found")
            };
            throw new HttpResponseException(response);
        }

    }

    public static class DirectionExtensions
    {
        public static LinkRelation AsLinkRelation(this Directions direction)
        {
            switch (direction)
            {
                case Directions.North:
                    return LinkRelation.North;

                case Directions.East:
                    return LinkRelation.East;

                case Directions.South:
                    return LinkRelation.South;

                case Directions.West:
                    return LinkRelation.West;

                case Directions.Exit:
                    return LinkRelation.Exit;

                default:
                    throw new ArgumentOutOfRangeException("direction");
            }
        }
    }
}
