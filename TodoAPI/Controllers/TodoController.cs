using Dapper;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Http.Cors;
using TodoAPI.Models;

namespace TodoAPI.Controllers
{
    [EnableCors("*", "*", "GET,POST,PUT,DELETE,PATCH")]
    [RoutePrefix("todo")]
    public class TodoController : ApiController
    {
        [Route("")]
        public IEnumerable<TodoItem> Get()
        {
            var db = new SqlConnection(
                ConfigurationManager
                .ConnectionStrings["db"]
                .ConnectionString);

            var items = db.Query<TodoItem>
                ("SELECT * FROM TodoItems ORDER BY CreateDate DESC");

            return items;
        }

        [Route("{id}")]
        public IHttpActionResult Get(int id)
        {
            var db = new SqlConnection(
                ConfigurationManager
                .ConnectionStrings["db"]
                .ConnectionString);

            var items = db.Query<TodoItem>
                ("SELECT * FROM TodoItems WHERE Id = @id",
                new
                {
                    id
                });

            var item = items.FirstOrDefault();

            if (item == null)
                return NotFound();
            else
                return Content(HttpStatusCode.OK, item);
        }

        [Route("")]
        public void Post([FromBody] TodoItem item)
        {
            var db = new SqlConnection(
                ConfigurationManager
                .ConnectionStrings["db"]
                .ConnectionString);

            db.Execute(
                "INSERT INTO TodoItems " +
                "(Title, Description, CreateDate)" +
                " VALUES (@title, @desc, GETDATE())", new
                {
                    title = item.Title,
                    desc = item.Description
                });
        }

        [Route("{id}")]
        public void Put(int id, [FromBody] TodoItem item)
        {
            var db = new SqlConnection(
                ConfigurationManager
                .ConnectionStrings["db"]
                .ConnectionString);

            db.Execute(
                "UPDATE TodoItems " +
                "SET Title = @title, Description = @desc " +
                "WHERE Id = @id", new
                {
                    id = id,
                    title = item.Title,
                    desc = item.Description
                });
        }

        [Route("{id}")]
        public void Delete(int id)
        {
            var db = new SqlConnection(
                ConfigurationManager
                .ConnectionStrings["db"]
                .ConnectionString);

            db.Execute(
                "DELETE FROM TodoItems " +
                "WHERE Id = @id", new
                {
                    id = id
                });
        }

        [Route("{id}/title")]
        [HttpPatch]
        public void UpdateTodoTitle(int id, [FromBody] TodoItem item)
        {
            var db = new SqlConnection(
                ConfigurationManager
                .ConnectionStrings["db"]
                .ConnectionString);

            db.Execute(
                "UPDATE TodoItems SET Title = @title WHERE Id = @id", new
                {
                    id,
                    title = item.Title
                });
        }

        [Route("{id}/description")]
        [HttpPatch]
        public void UpdateTodoDescription(int id, [FromBody] TodoItem item)
        {
            var db = new SqlConnection(
                ConfigurationManager
                .ConnectionStrings["db"]
                .ConnectionString);

            db.Execute(
                "UPDATE TodoItems SET Description = @desc WHERE Id = @id", new
                {
                    id,
                    desc = item.Description
                });
        }

        [Route("upload")]
        [HttpPost]
        public async Task<HttpResponseMessage> UploadFile(HttpRequestMessage request)
        {
            var uploadFilePath = 
                HostingEnvironment.MapPath("/uploadfiles");
            var provider = new MultipartFormDataStreamProvider(
                uploadFilePath);

            provider = await request
                .Content.ReadAsMultipartAsync(provider);
            
            foreach (MultipartFileData file in provider.FileData)
            {
                var fileSrc = new FileInfo(
                    Path.Combine(uploadFilePath, 
                    file.LocalFileName));
                fileSrc.MoveTo(Path.Combine(
                    uploadFilePath,
                    file.Headers.ContentDisposition.FileName.Replace("\"", "")));
            }

            return request.CreateResponse(HttpStatusCode.Accepted);
        }
    }
}
