﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using mazeagent.core.Models;
using mazeagent.mazeplusxml.Components;
using mazeagent.server.Helpers;
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
            var doc = new MazeDocument(request.RequestUri);
            var collection = new MazeCollection(linkBuilder.ResolveApplicationUri(new Uri(RoutePrefix, UriKind.Relative)));
            collection.AddLink(linkBuilder.ResolveApplicationUri(this.RelativeUriFromString(RoutePrefix, currentMaze.ID)));
            doc.AddElement(collection);

            return Request.CreateResponse(HttpStatusCode.OK, doc);
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
                var errorDoc = this.RenderErrorDocument("The requested maze was not found. The maze may have been retired.", null);
                errorDoc.Self = request.RequestUri;
                return Request.CreateResponse(HttpStatusCode.NotFound, errorDoc);
            }

            var linkBuilder = new LinkBuilder(request);
            var doc = new MazeDocument(request.RequestUri);
            var mazeItem =
                new MazeItem(
                    linkBuilder.ResolveApplicationUri(this.RelativeUriFromString(RoutePrefix, currentMaze.ID)),
                    linkBuilder.ResolveApplicationUri(this.RelativeUriFromString(RoutePrefix, currentMaze.ID, currentMaze.Start.ID)));

            doc.AddElement(mazeItem);

            return Request.CreateResponse(HttpStatusCode.OK, doc);
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
                var errorDoc = this.RenderErrorDocument("The requested maze was not found. The maze may have been retired.",null);
                errorDoc.Self = request.RequestUri;
                return Request.CreateResponse(HttpStatusCode.NotFound, errorDoc);
            }

            var linkBuilder = new LinkBuilder(request);
            var doc = new MazeDocument(request.RequestUri);
            var mazeCell = new MazeCell(linkBuilder.ResolveApplicationUri(this.RelativeUriFromString(RoutePrefix, currentMaze.ID, cellId)));

            IEnumerable<Maze.Edge> neighbors;
            try
            {
                neighbors = currentMaze.AccessibleNeighborsOf(cellId);
            }
            catch (ArgumentOutOfRangeException)
            {
                var errorDoc  = this.RenderErrorDocument("The requested cell was not found or the maze may have been retired.", null);
                errorDoc.Self = request.RequestUri;
                return Request.CreateResponse(HttpStatusCode.NotFound, errorDoc);
            }

            foreach (var neighbor in neighbors.Where(c => c.Direction != Directions.Exit))
            {
                mazeCell.AddLink(
                    linkBuilder.ResolveApplicationUri(this.RelativeUriFromString(RoutePrefix, currentMaze.ID, neighbor.Cell.ID)),
                    neighbor.Direction.AsLinkRelation());
            }

            if (neighbors.Any(c => c.Direction == Directions.Exit))
            {
                // the exit link returns the url with the entrance to the maze    
                mazeCell.AddLink(
                    linkBuilder.ResolveApplicationUri(this.RelativeUriFromString(RoutePrefix, currentMaze.ID)),
                    Directions.Exit.AsLinkRelation());
            }

            doc.AddElement(mazeCell);

            return Request.CreateResponse(HttpStatusCode.OK, doc);
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

        private MazeDocument RenderErrorDocument(string title, string message)
        {
            var doc = new MazeDocument();
            var errorElement = new MazeError(title, null);
            if (!string.IsNullOrWhiteSpace(message)) errorElement.AddMessage(message);

            doc.AddElement(errorElement);
            return doc;
        }

//        private HttpResponseMessage MazeToHttpResponseMessage(MazeDocument doc, HttpStatusCode statusCode = HttpStatusCode.OK)
//        {
//            var writer = new StringWriter();
//            var mazeWriter = new XmlMazeWriter(writer);
//            mazeWriter.Write(doc);
//            var response = new HttpResponseMessage(statusCode)
//            {
//                Content = new StringContent(writer.ToString())
//            };
//            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.amundsen.maze+xml");
//            return response;
//        }

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
